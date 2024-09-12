using Microsoft.IdentityModel.JsonWebTokens;

namespace SmartGrowHub.Maui.Services;

public interface ITokenStorage
{
    TryOptionAsync<string> GetAsync(CancellationToken cancellationToken);
    TryAsync<Unit> SetAsync(string token, CancellationToken cancellationToken);
    Try<bool> Remove();
    TryOptionAsync<bool> IsTokenExpiredAsync(CancellationToken cancellationToken);
}

public sealed class TokenStorage(ISecureStorageService secureStorage) : ITokenStorage
{
    private const string AuthTokenKey = "AuthToken";

    private readonly JsonWebTokenHandler _tokenHandler = new();

    public TryOptionAsync<string> GetAsync(CancellationToken cancellationToken) =>
        secureStorage.GetAsync(AuthTokenKey, cancellationToken);

    public Try<bool> Remove() => secureStorage.Remove(AuthTokenKey);

    public TryAsync<Unit> SetAsync(string token, CancellationToken cancellationToken) =>
        secureStorage.SetAsync(AuthTokenKey, token, cancellationToken);

    public TryOptionAsync<bool> IsTokenExpiredAsync(CancellationToken cancellationToken) =>
        GetAsync(cancellationToken)
            .Map(token => _tokenHandler
                .ReadJsonWebToken(token)
                .GetPayloadValue<int>("exp"))
            .Map(expiration =>
                DateTime.UtcNow >= DateTime.UnixEpoch.AddSeconds(expiration));
}