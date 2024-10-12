using Microsoft.IdentityModel.JsonWebTokens;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Features.RefreshTokens;
using SmartGrowHub.Domain.Model;

namespace SmartGrowHub.Maui.Services;

public interface IUserSessionProvider
{
    event Func<Unit>? SessionSet;
    event Func<Unit>? SessionRemoved;

    Eff<Option<UserSession>> GetUserSessionAsync(CancellationToken cancellationToken);
    Eff<Option<Id<UserSession>>> GetUserSessionIdAsync(CancellationToken cancellationToken);
    Eff<Option<AccessToken>> GetAccessTokenIfNotExpiredAsync(CancellationToken cancellationToken);
    Eff<Option<RefreshToken>> GetRefreshTokenAsync(CancellationToken cancellationToken);
    Eff<Unit> SetSession(UserSession session);
    Eff<Unit> SaveAndSetSessionAsync(UserSession session, CancellationToken cancellationToken);
    Eff<Unit> Remove();
    Eff<Unit> UpdateTokensAsync(AuthTokens authTokens, CancellationToken cancellationToken);
}

public sealed class UserSessionProvider(ISecureStorageService secureStorage) : IUserSessionProvider
{
    private const string RefreshTokenKey = "refresh_token";
    private const string AccessTokenKey = "access_token";
    private const string SessionIdKey = "session_id";
    private const string UserIdKey = "user_id";

    private static readonly JsonWebTokenHandler TokenHandler = new();

    private Option<UserSession> _currentUserSession = None;

    public event Func<Unit>? SessionSet;
    public event Func<Unit>? SessionRemoved;

    public Eff<Option<UserSession>> GetUserSessionAsync(CancellationToken cancellationToken) =>
        _currentUserSession.Match(
            Some: session => Pure(Some(session)),
            None: FetchAndSetUserSessionAsync(cancellationToken));

    public Eff<Option<Id<UserSession>>> GetUserSessionIdAsync(CancellationToken cancellationToken) =>
        _currentUserSession.Match(
            Some: session => Pure(Some(session.Id)),
            None: FetchAndSetUserSessionAsync(cancellationToken)
                .Map(option => option.Map(session => session.Id)));

    public Eff<Option<AccessToken>> GetAccessTokenIfNotExpiredAsync(CancellationToken cancellationToken) =>
        GetAccessTokenAsync(cancellationToken)
            .Map(option => option
                .Bind(token => IsTokenExpired(token)
                    ? None : Some(token)));

    public Eff<Option<RefreshToken>> GetRefreshTokenAsync(CancellationToken cancellationToken) =>
        _currentUserSession.Match(
            Some: session => Pure(Some(session.AuthTokens.RefreshToken)),
            None: FetchAndSetUserSessionAsync(cancellationToken)
                .Map(option => option.Map(session => session.AuthTokens.RefreshToken)));

    public Eff<Unit> SetSession(UserSession session) =>
        liftEff(() =>
        {
            _currentUserSession = session;
            SessionSet?.Invoke();
        });

    public Eff<Unit> SaveAndSetSessionAsync(UserSession session, CancellationToken cancellationToken) =>
        SaveSessionAsync(session, cancellationToken) >>
        SetSession(session);

    public Eff<Unit> Remove() =>
        RemoveSavedValue(SessionIdKey) >>
        RemoveSavedValue(UserIdKey) >>
        RemoveSavedValue(AccessTokenKey) >>
        RemoveSavedValue(RefreshTokenKey) >>
        RemoveCurrentSession();

    public Eff<Unit> UpdateTokensAsync(AuthTokens authTokens, CancellationToken cancellationToken) =>
        UpdateCurrentSessionTokens(authTokens) >>
        UpdateSavedTokensAsync(authTokens, cancellationToken);

    private Eff<Unit> UpdateCurrentSessionTokens(AuthTokens authTokens) =>
        liftEff(() => _currentUserSession = _currentUserSession
                .Map(session => session with { AuthTokens = authTokens }))
            .Map(_ => unit);

    private Eff<Unit> UpdateSavedTokensAsync(AuthTokens authTokens, CancellationToken cancellationToken) =>
        FetchAuthTokensAsync(cancellationToken)
            .Bind(option => option.Match(
                Some: _ => SaveAuthTokensAsync(authTokens, cancellationToken),
                None: unitEff));
    
    private Eff<Option<AccessToken>> GetAccessTokenAsync(CancellationToken cancellationToken) =>
        _currentUserSession.Match(
            Some: session => Pure(Some(session.AuthTokens.AccessToken)),
            None: FetchAndSetUserSessionAsync(cancellationToken)
                .Map(option => option.Map(session => session.AuthTokens.AccessToken)));

    private Eff<Unit> RemoveCurrentSession() =>
        liftEff(() =>
        {
            _currentUserSession = None;
            SessionRemoved?.Invoke();
        });

    private Eff<Option<UserSession>> FetchAndSetUserSessionAsync(CancellationToken cancellationToken) =>
        FetchUserSessionAsync(cancellationToken)
            .Bind(option => option.Match(
                Some: session => SetSession(session).Map(_ => Some(session)),
                None: Pure(Option<UserSession>.None)));

    private Eff<Unit> SaveSessionAsync(UserSession session, CancellationToken cancellationToken) =>
        SaveSessionIdAsync(session.Id, cancellationToken) >>
        SaveUserIdAsync(session.UserId, cancellationToken) >>
        SaveAuthTokensAsync(session.AuthTokens, cancellationToken);

    private Eff<Unit> SaveAuthTokensAsync(AuthTokens authTokens, CancellationToken cancellationToken) =>
        SaveAccessTokenAsync(authTokens.AccessToken, cancellationToken) >>
        SaveRefreshTokenAsync(authTokens.RefreshToken, cancellationToken);
    
    private Eff<Unit> SaveSessionIdAsync(Id<UserSession> id, CancellationToken cancellationToken) =>
        secureStorage.SaveDomainTypeAsync(SessionIdKey, id, cancellationToken);

    private Eff<Unit> SaveUserIdAsync(Id<User> id, CancellationToken cancellationToken) =>
        secureStorage.SaveDomainTypeAsync(UserIdKey, id, cancellationToken);

    private Eff<Unit> SaveAccessTokenAsync(AccessToken token, CancellationToken cancellationToken) =>
        secureStorage.SaveDomainTypeAsync(AccessTokenKey, token, cancellationToken);

    private Eff<Unit> SaveRefreshTokenAsync(RefreshToken token, CancellationToken cancellationToken) =>
        secureStorage.SaveDomainTypeAsync(RefreshTokenKey, token, cancellationToken);
    
    private Eff<Option<UserSession>> FetchUserSessionAsync(CancellationToken cancellationToken) =>
        from optionSessionId in FetchSessionIdAsync(cancellationToken)
        from optionUserId in FetchUserIdAsync(cancellationToken)
        from optionAuthTokens in FetchAuthTokensAsync(cancellationToken)
        select from sessionId in optionSessionId
               from userId in optionUserId
               from authTokens in optionAuthTokens
               select new UserSession(sessionId, userId, authTokens);
    
    private Eff<Option<AuthTokens>> FetchAuthTokensAsync(CancellationToken cancellationToken) =>
        from optionAccessToken in FetchAccessTokenAsync(cancellationToken)
        from optionRefreshToken in FetchRefreshTokenAsync(cancellationToken)
        select from accessToken in optionAccessToken
               from refreshToken in optionRefreshToken
               select new AuthTokens(accessToken, refreshToken);

    private Eff<Option<Id<UserSession>>> FetchSessionIdAsync(CancellationToken cancellationToken) =>
        secureStorage.GetDomainTypeAsync<Id<UserSession>>(SessionIdKey, cancellationToken);

    private Eff<Option<Id<User>>> FetchUserIdAsync(CancellationToken cancellationToken) =>
        secureStorage.GetDomainTypeAsync<Id<User>>(UserIdKey, cancellationToken);

    private Eff<Option<AccessToken>> FetchAccessTokenAsync(CancellationToken cancellationToken) =>
        secureStorage.GetDomainTypeAsync<AccessToken>(AccessTokenKey, cancellationToken);

    private Eff<Option<RefreshToken>> FetchRefreshTokenAsync(CancellationToken cancellationToken) =>
        secureStorage.GetDomainTypeAsync<RefreshToken>(RefreshTokenKey, cancellationToken);

    private Eff<Unit> RemoveSavedValue(string key) =>
        secureStorage.Remove(key).Map(_ => unit);

    private static bool IsTokenExpired(AccessToken token)
    {
        var expiration = TokenHandler
            .ReadJsonWebToken(token.To())
            .GetPayloadValue<int>("exp");

        return DateTime.UtcNow.AddSeconds(3) >= DateTime.UnixEpoch.AddSeconds(expiration);
    }
}