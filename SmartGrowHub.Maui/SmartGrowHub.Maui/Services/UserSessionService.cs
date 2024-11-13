using Microsoft.IdentityModel.JsonWebTokens;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Model;
using SmartGrowHub.Maui.Application.Interfaces;
using SmartGrowHub.Maui.Services.Extensions;

namespace SmartGrowHub.Maui.Services;

internal sealed class UserSessionService(ISecureStorage secureStorage) : IUserSessionService
{
    private const int TokenExpirationBufferSeconds = 3;

    private static readonly Error AccessTokenExpiredError =
        Error.New("The access token have already expired");

    private static readonly JsonWebTokenHandler TokenHandler = new();

    private Option<UserSession> _currentUserSession = None;

    public Eff<UserSession> GetUserSession(CancellationToken cancellationToken) =>
        _currentUserSession.Match(
            Some: session => Pure(session),
            None: ReadAndSetUserSession(cancellationToken));

    public Eff<Id<UserSession>> GetUserSessionId(CancellationToken cancellationToken) =>
        _currentUserSession.Match(
            Some: session => Pure(session.Id),
            None: ReadAndSetUserSession(cancellationToken)
                .Map(session => session.Id));

    public Eff<Id<User>> GetUserId(CancellationToken cancellationToken) =>
        _currentUserSession.Match(
            Some: session => Pure(session.UserId),
            None: ReadAndSetUserSession(cancellationToken)
                .Map(session => session.UserId));

    public Eff<AccessToken> GetAccessTokenIfNotExpired(CancellationToken cancellationToken) =>
        GetAccessToken(cancellationToken)
            .Bind(token => IsTokenExpired(token)
                ? AccessTokenExpiredError
                : SuccessEff(token));

    public Eff<RefreshToken> GetRefreshToken(CancellationToken cancellationToken) =>
        _currentUserSession.Match(
            Some: session => Pure(session.AuthTokens.RefreshToken),
            None: ReadAndSetUserSession(cancellationToken)
                .Map(session => session.AuthTokens.RefreshToken));

    public IO<Unit> SetSession(UserSession session) =>
        lift(() =>
        {
            _currentUserSession = session;
        });

    public Eff<Unit> SaveAndSetSession(UserSession session, CancellationToken cancellationToken) =>
        secureStorage.SaveSession(session, cancellationToken) >> SetSession(session);

    public Eff<Unit> RemoveSession() =>
        from _1 in secureStorage.RemoveSession()
        from _2 in RemoveCurrentSession()
        select unit;

    public Eff<Unit> UpdateTokens(AuthTokens authTokens, CancellationToken cancellationToken) =>
        from _1 in UpdateCurrentSessionTokens(authTokens)
        from _2 in UpdateSavedTokens(authTokens, cancellationToken).IfFailEff(_ => unitEff)
        select unit;

    private IO<Unit> UpdateCurrentSessionTokens(AuthTokens authTokens) =>
        lift(() =>
        {
            _currentUserSession = _currentUserSession
                .Map(session => session.UpdateTokens(authTokens));
        });

    private Eff<Unit> UpdateSavedTokens(AuthTokens authTokens, CancellationToken cancellationToken) =>
        secureStorage.ReadAuthTokens(cancellationToken).Map(_ => unit) >>
        secureStorage.SaveAuthTokens(authTokens, cancellationToken);

    private Eff<AccessToken> GetAccessToken(CancellationToken cancellationToken) =>
        _currentUserSession.Match(
            Some: session => Pure(session.AuthTokens.AccessToken),
            None: ReadAndSetUserSession(cancellationToken)
                .Map(session => session.AuthTokens.AccessToken));

    private IO<Unit> RemoveCurrentSession() =>
        lift(() =>
        {
            _currentUserSession = None;
        });

    private Eff<UserSession> ReadAndSetUserSession(CancellationToken cancellationToken) =>
        from session in secureStorage.ReadUserSession(cancellationToken)
        from _ in SetSession(session)
        select session;

    private static bool IsTokenExpired(AccessToken token)
    {
        int expiration = TokenHandler
            .ReadJsonWebToken(token.To())
            .GetPayloadValue<int>("exp");

        return DateTime.UtcNow.AddSeconds(TokenExpirationBufferSeconds) >= DateTime.UnixEpoch.AddSeconds(expiration);
    }
}
