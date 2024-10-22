using SmartGrowHub.Domain.Extensions;
using SmartGrowHub.Maui.Application.Interfaces;

namespace SmartGrowHub.Maui.Services;

public sealed class SecureStorageService(ISecureStorage secureStorage) : ISecureStorageService
{
    public Eff<string> GetAsync(string key, CancellationToken cancellationToken) =>
        secureStorage
            .GetAsync(key)
            .WaitAsync(cancellationToken)
            .Map(Optional)
            .ToEff()
            .Bind(option => option.ToEff());

    public Eff<bool> Remove(string key) =>
        liftEff(() => secureStorage.Remove(key));

    public Eff<Unit> SetAsync(string key, string value, CancellationToken cancellationToken) =>
        secureStorage
            .SetAsync(key, value)
            .WaitAsync(cancellationToken)
            .ToEff();

    public Eff<Unit> SaveDomainTypeAsync<T>(string key, T domainType, CancellationToken cancellationToken)
        where T : DomainType<T, string> =>
        SetAsync(key, domainType.To(), cancellationToken);

    public Eff<T> GetDomainTypeAsync<T>(string key, CancellationToken cancellationToken)
        where T : DomainType<T, string> =>
        GetAsync(key, cancellationToken).Bind(value =>
        {
            var s = T.From(value);
            return s.ToEff();
        });
}
