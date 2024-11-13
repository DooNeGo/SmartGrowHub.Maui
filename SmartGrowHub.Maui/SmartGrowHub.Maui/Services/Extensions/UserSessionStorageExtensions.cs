using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Model;

namespace SmartGrowHub.Maui.Services.Extensions;

public static class UserSessionStorageExtensions
{
    private const string RefreshTokenKey = "refresh_token";
    private const string AccessTokenKey = "access_token";
    private const string SessionIdKey = "session_id";
    private const string UserIdKey = "user_id";

    public static Eff<Unit> SaveSession(this ISecureStorage secureStorage, UserSession session,
        CancellationToken cancellationToken) =>
        secureStorage.SaveSessionId(session.Id, cancellationToken) >>
        secureStorage.SaveUserId(session.UserId, cancellationToken) >>
        secureStorage.SaveAuthTokens(session.AuthTokens, cancellationToken);

    public static Eff<UserSession> ReadUserSession(this ISecureStorage secureStorage, CancellationToken cancellationToken) =>
        from sessionId in secureStorage.ReadSessionId(cancellationToken)
        from userId in secureStorage.ReadUserId(cancellationToken)
        from authTokens in secureStorage.ReadAuthTokens(cancellationToken)
        select new UserSession(sessionId, userId, authTokens);

    public static Eff<Unit> SaveUserId(this ISecureStorage secureStorage, Id<User> userId, CancellationToken cancellationToken) =>
        secureStorage.SaveDomainType(UserIdKey, userId, cancellationToken);

    public static Eff<Id<User>> ReadUserId(this ISecureStorage secureStorage, CancellationToken cancellationToken) =>
        secureStorage.GetDomainType<Id<User>>(UserIdKey, cancellationToken);

    public static Eff<Unit> SaveAccessToken(this ISecureStorage secureStorage, AccessToken token, CancellationToken cancellationToken) =>
        secureStorage.SaveDomainType(AccessTokenKey, token, cancellationToken);

    public static Eff<AccessToken> ReadAccessToken(this ISecureStorage secureStorage, CancellationToken cancellationToken) =>
        secureStorage.GetDomainType<AccessToken>(AccessTokenKey, cancellationToken);

    public static Eff<Unit> SaveRefreshToken(this ISecureStorage secureStorage, RefreshToken token, CancellationToken cancellationToken) =>
        secureStorage.SaveDomainType(RefreshTokenKey, token, cancellationToken);

    public static Eff<RefreshToken> ReadRefreshToken(this ISecureStorage secureStorage, CancellationToken cancellationToken) =>
        secureStorage.GetDomainType<RefreshToken>(RefreshTokenKey, cancellationToken);

    public static Eff<Unit> SaveSessionId(this ISecureStorage secureStorage, Id<UserSession> sessionId, CancellationToken cancellationToken) =>
        secureStorage.SaveDomainType(SessionIdKey, sessionId, cancellationToken);

    public static Eff<Id<UserSession>> ReadSessionId(this ISecureStorage secureStorage, CancellationToken cancellationToken) =>
        secureStorage.GetDomainType<Id<UserSession>>(SessionIdKey, cancellationToken);

    public static Eff<Unit> SaveAuthTokens(this ISecureStorage secureStorage, AuthTokens authTokens,
        CancellationToken cancellationToken) =>
        secureStorage.SaveAccessToken(authTokens.AccessToken, cancellationToken) >>
        secureStorage.SaveRefreshToken(authTokens.RefreshToken, cancellationToken);

    public static Eff<AuthTokens> ReadAuthTokens(this ISecureStorage secureStorage, CancellationToken cancellationToken) =>
        from accessToken in secureStorage.ReadAccessToken(cancellationToken)
        from refreshToken in secureStorage.ReadRefreshToken(cancellationToken)
        select new AuthTokens(accessToken, refreshToken);

    public static Eff<bool> RemoveSession(this ISecureStorage secureStorage) =>
        secureStorage.RemoveEff(SessionIdKey) >>
        secureStorage.RemoveEff(UserIdKey) >>
        secureStorage.RemoveEff(AccessTokenKey) >>
        secureStorage.RemoveEff(RefreshTokenKey);
}