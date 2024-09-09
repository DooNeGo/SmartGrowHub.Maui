namespace SmartGrowHub.Maui.Services;

public interface ITokenProvider
{
    TryOptionAsync<string> GetAsync(CancellationToken cancellationToken);
    TryAsync<Unit> SetAsync(string token, CancellationToken cancellationToken);
    Try<bool> Remove();
}

public sealed class TokenProvider(ISecureStorageService secureStorage) : ITokenProvider
{
    private const string AuthTokenKey = "AuthToken";

    public TryOptionAsync<string> GetAsync(CancellationToken cancellationToken) =>
        secureStorage.GetAsync(AuthTokenKey, cancellationToken);

    public Try<bool> Remove() => secureStorage.Remove(AuthTokenKey);

    public TryAsync<Unit> SetAsync(string token, CancellationToken cancellationToken) =>
        secureStorage.SetAsync(AuthTokenKey, token, cancellationToken);
}