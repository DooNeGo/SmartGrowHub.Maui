namespace SmartGrowHub.Maui.Services.Extensions;

public static class SecureStorageExtensions
{
    public static Eff<string> GetEff(this ISecureStorage secureStorage, string key,
        CancellationToken cancellationToken) =>
        liftEff(() => secureStorage.GetAsync(key).WaitAsync(cancellationToken))
            .Bind(value => Optional(value)
                .ToEff(Error.New($"No value found in Secure storage for the key: {key}")));

    public static Eff<bool> RemoveEff(this ISecureStorage secureStorage, string key) =>
        liftEff(() => secureStorage.Remove(key));

    public static Eff<Unit> SetEff(this ISecureStorage secureStorage, string key, string value,
        CancellationToken cancellationToken) =>
        liftEff(() => secureStorage
            .SetAsync(key, value)
            .WaitAsync(cancellationToken)
            .ToUnit());
}