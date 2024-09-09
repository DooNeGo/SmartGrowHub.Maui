namespace SmartGrowHub.Maui.Services;

public interface ISecureStorageService
{
    TryAsync<Unit> SetAsync(string key, string value, CancellationToken cancellationToken);
    TryOptionAsync<string> GetAsync(string key, CancellationToken cancellationToken);
    Try<bool> Remove(string key);
}

public sealed class SecureStorageService(ISecureStorage secureStorage) : ISecureStorageService
{
    public TryOptionAsync<string> GetAsync(string key, CancellationToken cancellationToken) =>
        TryOptionAsync(() => secureStorage
            .GetAsync(key)
            .WaitAsync(cancellationToken)
            .Map(Optional));

    public Try<bool> Remove(string key) =>
        () => secureStorage.Remove(key);

    public TryAsync<Unit> SetAsync(string key, string value, CancellationToken cancellationToken) =>
        TryAsync(() => secureStorage
            .SetAsync(key, value)
            .WaitAsync(cancellationToken)
            .ToUnit());
}
