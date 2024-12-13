using Microsoft.IdentityModel.JsonWebTokens;
using SmartGrowHub.Maui.Services.Extensions;
using SmartGrowHub.Shared.Tokens;
using System.Text.Json;

namespace SmartGrowHub.Maui.Services.Infrastructure;

public interface ITokensStorage
{
    Eff<string> GetRefreshToken(CancellationToken cancellationToken);
    Eff<Unit> SetRefreshToken(string value, CancellationToken cancellationToken);
    Eff<string> GetAccessToken(CancellationToken cancellationToken);
    Eff<Unit> SetAccessToken(string value, CancellationToken cancellationToken);
    Eff<string> GetAccessTokenIfNotExpired(CancellationToken cancellationToken);
    Eff<Unit> SetAuthTokens(AuthTokensDto tokens, CancellationToken cancellationToken);
    Eff<Unit> RemoveTokens();
}

public sealed class TokensStorage(ISecureStorage secureStorage) : ITokensStorage
{
    private const string RefreshTokenKey = "refresh_token";
    private const string AccessTokenKey = "access_token";
    
    private const int TokenExpirationBufferSeconds = 3;

    private static readonly Error AccessTokenExpiredError =
        Error.New("The access token have already expired");

    private static readonly JsonWebTokenHandler TokenHandler = new();
    
    public Eff<string> GetRefreshToken(CancellationToken cancellationToken) =>
        secureStorage.GetEff(RefreshTokenKey, cancellationToken);
    
    public Eff<Unit> SetRefreshToken(string value, CancellationToken cancellationToken) =>
        secureStorage.SetEff(RefreshTokenKey, value, cancellationToken);
    
    public Eff<string> GetAccessToken(CancellationToken cancellationToken) =>
        secureStorage.GetEff(AccessTokenKey, cancellationToken);
    
    public Eff<Unit> SetAccessToken(string value, CancellationToken cancellationToken) =>
        secureStorage.SetEff(AccessTokenKey, value, cancellationToken);
    
    public Eff<string> GetAccessTokenIfNotExpired(CancellationToken cancellationToken) =>
        GetAccessToken(cancellationToken)
            .Bind(token => IsTokenExpired(token)
                ? AccessTokenExpiredError
                : SuccessEff(token));

    public Eff<Unit> SetAuthTokens(AuthTokensDto tokens, CancellationToken cancellationToken) =>
        from _ in SetAccessToken(tokens.AccessToken, cancellationToken)
        from __ in SetRefreshToken(JsonSerializer.Serialize(tokens.RefreshToken), cancellationToken)
        select __;
    
    public Eff<Unit> RemoveTokens() =>
        from _ in RemoveAccessToken()
        from __ in RemoveRefreshToken()
        select unit;
    
    private Eff<bool> RemoveAccessToken() => secureStorage.RemoveEff(AccessTokenKey);
    
    private Eff<bool> RemoveRefreshToken() => secureStorage.RemoveEff(RefreshTokenKey);
    
    private static bool IsTokenExpired(string token)
    {
        var expiration = TokenHandler
            .ReadJsonWebToken(token)
            .GetPayloadValue<int>("exp");

        return DateTime.UtcNow.AddSeconds(TokenExpirationBufferSeconds) >= DateTime.UnixEpoch.AddSeconds(expiration);
    }
}