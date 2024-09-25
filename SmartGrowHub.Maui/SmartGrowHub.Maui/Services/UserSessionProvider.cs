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
            None: GetSavedUserSessionAsync(cancellationToken));

    public Eff<Option<AccessToken>> GetAccessTokenIfNotExpiredAsync(CancellationToken cancellationToken) =>
        GetAccessTokenAsync(cancellationToken)
            .Map(option => option
                .Bind(token => IsTokenExpired(token)
                    ? None : Some(token)));

    public Eff<Option<RefreshToken>> GetRefreshTokenAsync(CancellationToken cancellationToken) =>
        _currentUserSession.Match(
            Some: session => Pure(Some(session.AuthTokens.RefreshToken)),
            None: GetSavedRefreshTokenAsync(cancellationToken));

    public Eff<Unit> SetSession(UserSession session) =>
        liftEff(() =>
        {
            _currentUserSession = session;
            SessionSet?.Invoke();
        });

    public Eff<Unit> SaveAndSetSessionAsync(UserSession session, CancellationToken cancellationToken) =>
        from _1 in SaveSessionAsync(session, cancellationToken)
        from _2 in SetSession(session)
        select unit;

    public Eff<Unit> Remove() =>
        from _1 in secureStorage.Remove(SessionIdKey)
        from _2 in secureStorage.Remove(UserIdKey)
        from _3 in secureStorage.Remove(AccessTokenKey)
        from _4 in secureStorage.Remove(RefreshTokenKey)
        from _5 in RemoveCurrentSession()
        select unit;

    public Eff<Unit> UpdateTokensAsync(AuthTokens authTokens, CancellationToken cancellationToken) =>
        from _1 in UpdateTokens(authTokens)
        from _2 in UpdateSavedTokensAsync(authTokens, cancellationToken)
        select unit;

    private Eff<Unit> UpdateTokens(AuthTokens authTokens) =>
        Pure(_currentUserSession = _currentUserSession
            .Map(session => session with { AuthTokens = authTokens }))
            .Map(_ => unit);

    private Eff<Unit> UpdateSavedTokensAsync(AuthTokens authTokens, CancellationToken cancellationToken) =>
        GetSavedAuthTokensAsync(cancellationToken)
            .Bind(option => option.Match(
                Some: _ => SaveAuthTokensAsync(authTokens, cancellationToken),
                None: Pure(unit)));

    private Eff<Unit> RemoveCurrentSession() =>
        liftEff(() =>
        {
            _currentUserSession = None;
            SessionRemoved?.Invoke();
        });

    private Eff<Option<AccessToken>> GetAccessTokenAsync(CancellationToken cancellationToken) =>
        _currentUserSession.Match(
            Some: session => Pure(Some(session.AuthTokens.AccessToken)),
            None: GetSavedAccessTokenAsync(cancellationToken));

    private Eff<Option<UserSession>> GetSavedUserSessionAsync(CancellationToken cancellationToken) =>
        from optionSessionId in GetSavedSessionIdAsync(cancellationToken)
        from optionUserId in GetSavedUserIdAsync(cancellationToken)
        from optionAccessToken in GetSavedAccessTokenAsync(cancellationToken)
        from optionRefreshToken in GetSavedRefreshTokenAsync(cancellationToken)
        select from sessionId in optionSessionId
               from userId in optionUserId
               from accessToken in optionAccessToken
               from refreshToken in optionRefreshToken
               select new UserSession(sessionId, userId, new AuthTokens(accessToken, refreshToken));

    private Eff<Unit> SaveSessionAsync(UserSession session, CancellationToken cancellationToken) =>
        from _1 in SaveSessionIdAsync(session.Id, cancellationToken)
        from _2 in SaveUserIdAsync(session.UserId, cancellationToken)
        from _3 in SaveAuthTokensAsync(session.AuthTokens, cancellationToken)
        select unit;

    private Eff<Unit> SaveSessionIdAsync(Id<UserSession> id, CancellationToken cancellationToken) =>
        secureStorage.SaveDomainTypeAsync(SessionIdKey, id, cancellationToken);

    private Eff<Unit> SaveUserIdAsync(Id<User> id, CancellationToken cancellationToken) =>
        secureStorage.SaveDomainTypeAsync(UserIdKey, id, cancellationToken);

    private Eff<Unit> SaveAccessTokenAsync(AccessToken token, CancellationToken cancellationToken) =>
        secureStorage.SaveDomainTypeAsync(AccessTokenKey, token, cancellationToken);

    private Eff<Unit> SaveRefreshTokenAsync(RefreshToken token, CancellationToken cancellationToken) =>
        secureStorage.SaveDomainTypeAsync(RefreshTokenKey, token, cancellationToken);

    private Eff<Unit> SaveAuthTokensAsync(AuthTokens authTokens, CancellationToken cancellationToken) =>
        from _1 in SaveAccessTokenAsync(authTokens.AccessToken, cancellationToken)
        from _2 in SaveRefreshTokenAsync(authTokens.RefreshToken, cancellationToken)
        select unit;

    private Eff<Option<Id<UserSession>>> GetSavedSessionIdAsync(CancellationToken cancellationToken) =>
        secureStorage.GetDomainTypeAsync<Id<UserSession>>(SessionIdKey, cancellationToken);

    private Eff<Option<Id<User>>> GetSavedUserIdAsync(CancellationToken cancellationToken) =>
        secureStorage.GetDomainTypeAsync<Id<User>>(UserIdKey, cancellationToken);

    private Eff<Option<AccessToken>> GetSavedAccessTokenAsync(CancellationToken cancellationToken) =>
        secureStorage.GetDomainTypeAsync<AccessToken>(AccessTokenKey, cancellationToken);

    private Eff<Option<RefreshToken>> GetSavedRefreshTokenAsync(CancellationToken cancellationToken) =>
        secureStorage.GetDomainTypeAsync<RefreshToken>(RefreshTokenKey, cancellationToken);

    private Eff<Option<AuthTokens>> GetSavedAuthTokensAsync(CancellationToken cancellationToken) =>
        from optionAccessToken in GetSavedAccessTokenAsync(cancellationToken)
        from optionRefreshToken in GetSavedRefreshTokenAsync(cancellationToken)
        select from accessToken in optionAccessToken
               from refreshToken in optionRefreshToken
               select new AuthTokens(accessToken, refreshToken);

    private static bool IsTokenExpired<T>(T token) where T : DomainType<T, string>
    {
        int expiration = TokenHandler
            .ReadJsonWebToken(token.To())
            .GetPayloadValue<int>("exp");

        return DateTime.UtcNow.AddSeconds(3) >= DateTime.UnixEpoch.AddSeconds(expiration);
    }
}