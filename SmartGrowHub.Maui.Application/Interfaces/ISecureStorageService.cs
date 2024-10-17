namespace SmartGrowHub.Maui.Application.Interfaces;

public interface ISecureStorageService
{
    Eff<Unit> SetAsync(string key, string value, CancellationToken cancellationToken);
    Eff<string> GetAsync(string key, CancellationToken cancellationToken);
    Eff<bool> Remove(string key);
    Eff<Unit> SaveDomainTypeAsync<T>(string key, T domainType, CancellationToken cancellationToken) where T : DomainType<T, string>;
    Eff<T> GetDomainTypeAsync<T>(string key, CancellationToken cancellationToken) where T : DomainType<T, string>;
}
