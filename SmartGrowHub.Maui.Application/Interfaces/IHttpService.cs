namespace SmartGrowHub.Maui.Application.Interfaces;

public interface IHttpService
{
    Eff<Either<TError, TResponse>> GetAsync<TResponse, TError>(string urn, CancellationToken cancellationToken);
    Eff<Either<TError, TResponse>> PostAsync<TRequest, TResponse, TError>(string urn, TRequest request, CancellationToken cancellationToken);
}
