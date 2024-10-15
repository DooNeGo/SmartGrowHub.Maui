using Microsoft.IdentityModel.JsonWebTokens;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Model;
using SmartGrowHub.Maui.Application.Interfaces;

namespace SmartGrowHub.Maui.Infrastructure.Services;

internal sealed class UserSessionProvider(ISecureStorageService secureStorage) : IUserSessionProvider
{
    private static readonly Error AccessTokenExpiredError =
        Error.New("The access token have already expired");

    private const string RefreshTokenKey = "refresh_token";
    private const string AccessTokenKey = "access_token";
    private const string SessionIdKey = "session_id";
    private const string UserIdKey = "user_id";

    private static readonly JsonWebTokenHandler TokenHandler = new();

    private Option<UserSession> _currentUserSession = None;

    public event Func<Unit>? SessionSet;
    public event Func<Unit>? SessionRemoved;

    public Eff<UserSession> GetUserSession(CancellationToken cancellationToken) =>
        _currentUserSession.Match(
            Some: session => Pure(session),
            None: FetchAndSetUserSessionAsync(cancellationToken));

    public Eff<Id<UserSession>> GetUserSessionId(CancellationToken cancellationToken) =>
        _currentUserSession.Match(
            Some: session => Pure(session.Id),
            None: FetchAndSetUserSessionAsync(cancellationToken)
                .Map(session => session.Id));

    public Eff<AccessToken> GetAccessTokenIfNotExpired(CancellationToken cancellationToken) =>
        GetAccessTokenAsync(cancellationToken)
            .Bind(token => IsTokenExpired(token)
                ? AccessTokenExpiredError
                : SuccessEff(token));

    public Eff<RefreshToken> GetRefreshToken(CancellationToken cancellationToken) =>
        _currentUserSession.Match(
            Some: session => Pure(session.AuthTokens.RefreshToken),
            None: FetchAndSetUserSessionAsync(cancellationToken)
                .Map(session => session.AuthTokens.RefreshToken));

    public IO<Unit> SetSession(UserSession session) =>
        lift(() =>
        {
            _currentUserSession = session;
            SessionSet?.Invoke();
        });

    public Eff<Unit> SaveAndSetSession(UserSession session, CancellationToken cancellationToken) =>
        SaveSessionAsync(session, cancellationToken) >>
        SetSession(session);

    public Eff<Unit> RemoveSession() =>
        RemoveSavedValue(SessionIdKey) >>
        RemoveSavedValue(UserIdKey) >>
        RemoveSavedValue(AccessTokenKey) >>
        RemoveSavedValue(RefreshTokenKey) >>
        RemoveCurrentSession();

    public Eff<Unit> UpdateTokens(AuthTokens authTokens, CancellationToken cancellationToken) =>
        UpdateSavedTokensAsync(authTokens, cancellationToken) >>
        UpdateCurrentSessionTokens(authTokens);

    private IO<Unit> UpdateCurrentSessionTokens(AuthTokens authTokens) =>
        lift(() => _currentUserSession = _currentUserSession
            .Map(session => session.UpdateTokens(authTokens)))
        .Map(_ => unit);

    private Eff<Unit> UpdateSavedTokensAsync(AuthTokens authTokens, CancellationToken cancellationToken) =>
        FetchAuthTokensAsync(cancellationToken).Map(_ => unit) >>
        SaveAuthTokensAsync(authTokens, cancellationToken);

    private Eff<AccessToken> GetAccessTokenAsync(CancellationToken cancellationToken) =>
        _currentUserSession.Match(
            Some: session => Pure(session.AuthTokens.AccessToken),
            None: FetchAndSetUserSessionAsync(cancellationToken)
                .Map(session => session.AuthTokens.AccessToken));

    private Eff<Unit> RemoveCurrentSession() =>
        liftEff(() =>
        {
            _currentUserSession = None;
            SessionRemoved?.Invoke();
        });

    private Eff<UserSession> FetchAndSetUserSessionAsync(CancellationToken cancellationToken) =>
        from session in FetchUserSessionAsync(cancellationToken)
        from _ in SetSession(session)
        select session;

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

    private Eff<UserSession> FetchUserSessionAsync(CancellationToken cancellationToken) =>
        from sessionId in FetchSessionIdAsync(cancellationToken)
        from userId in FetchUserIdAsync(cancellationToken)
        from authTokens in FetchAuthTokensAsync(cancellationToken)
        select new UserSession(sessionId, userId, authTokens);

    private Eff<AuthTokens> FetchAuthTokensAsync(CancellationToken cancellationToken) =>
        from accessToken in FetchAccessTokenAsync(cancellationToken)
        from refreshToken in FetchRefreshTokenAsync(cancellationToken)
        select new AuthTokens(accessToken, refreshToken);

    private Eff<Id<UserSession>> FetchSessionIdAsync(CancellationToken cancellationToken) =>
        secureStorage.GetDomainTypeAsync<Id<UserSession>>(SessionIdKey, cancellationToken)
            .Bind(option => option.ToEff());

    private Eff<Id<User>> FetchUserIdAsync(CancellationToken cancellationToken) =>
        secureStorage.GetDomainTypeAsync<Id<User>>(UserIdKey, cancellationToken)
            .Bind(option => option.ToEff());

    private Eff<AccessToken> FetchAccessTokenAsync(CancellationToken cancellationToken) =>
        secureStorage.GetDomainTypeAsync<AccessToken>(AccessTokenKey, cancellationToken)
            .Bind(option => option.ToEff());

    private Eff<RefreshToken> FetchRefreshTokenAsync(CancellationToken cancellationToken) =>
        secureStorage.GetDomainTypeAsync<RefreshToken>(RefreshTokenKey, cancellationToken)
            .Bind(option => option.ToEff());

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