namespace SmartGrowHub.Maui.Services;

public interface ISecureStorageService
{
    Eff<Unit> SetAsync(string key, string value, CancellationToken cancellationToken);
    Eff<Option<string>> GetAsync(string key, CancellationToken cancellationToken);
    Eff<bool> Remove(string key);
    Eff<Unit> SaveDomainTypeAsync<T>(string key, T domainType, CancellationToken cancellationToken) where T : DomainType<T, string>;
    Eff<Option<T>> GetDomainTypeAsync<T>(string key, CancellationToken cancellationToken) where T : DomainType<T, string>;
}

public sealed class SecureStorageService(ISecureStorage secureStorage) : ISecureStorageService
{
    public Eff<Option<string>> GetAsync(string key, CancellationToken cancellationToken) =>
        liftEff(() => secureStorage
            .GetAsync(key)
            .WaitAsync(cancellationToken)
            .Map(Optional));

    public Eff<bool> Remove(string key) =>
        liftEff(() => secureStorage.Remove(key));

    public Eff<Unit> SetAsync(string key, string value, CancellationToken cancellationToken) =>
        liftEff(() => secureStorage
            .SetAsync(key, value)
            .WaitAsync(cancellationToken)
            .ToUnit());

    public Eff<Unit> SaveDomainTypeAsync<T>(string key, T domainType, CancellationToken cancellationToken)
        where T : DomainType<T, string> =>
        SetAsync(key, domainType.To(), cancellationToken);

    public Eff<Option<T>> GetDomainTypeAsync<T>(string key, CancellationToken cancellationToken)
        where T : DomainType<T, string> =>
        GetAsync(key, cancellationToken)
            .Map(option => option
                .Map(token => T.FromUnsafe(token)));
}
