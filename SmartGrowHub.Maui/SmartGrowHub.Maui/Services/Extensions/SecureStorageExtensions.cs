using SmartGrowHub.Domain.Extensions;

namespace SmartGrowHub.Maui.Services.Extensions;

public static class SecureStorageExtensions
{
    public static Eff<string> GetEff(this ISecureStorage secureStorage, string key,
        CancellationToken cancellationToken) =>
        secureStorage.GetAsync(key)
            .WaitAsync(cancellationToken).ToEff()
            .Bind(value => Optional(value)
                .ToEff(Error.New($"No value found in Secure storage for the key: {key}")));

    public static Eff<bool> RemoveEff(this ISecureStorage secureStorage, string key) =>
        liftEff(() => secureStorage.Remove(key));

    public static Eff<Unit> SetEff(this ISecureStorage secureStorage, string key, string value,
        CancellationToken cancellationToken) =>
        secureStorage
            .SetAsync(key, value)
            .WaitAsync(cancellationToken)
            .ToEff();

    public static Eff<Unit> SaveDomainType<T>(this ISecureStorage secureStorage, string key, T domainType,
        CancellationToken cancellationToken) where T : DomainType<T, string> =>
        secureStorage.SetEff(key, domainType.To(), cancellationToken);

    public static Eff<T> GetDomainType<T>(this ISecureStorage secureStorage, string key,
        CancellationToken cancellationToken) where T : DomainType<T, string> =>
        secureStorage.GetEff(key, cancellationToken).Bind(value => T.From(value).ToEff());
}