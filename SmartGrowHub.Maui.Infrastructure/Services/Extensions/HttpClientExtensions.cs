using SmartGrowHub.Domain.Extensions;
using System.Net.Http.Json;

namespace SmartGrowHub.Maui.Infrastructure.Services.Extensions;

internal static class HttpClientExtensions
{
    public static Eff<Either<TError, TResponse>> GetAsync<TResponse, TError>(
        this HttpClient httpClient, string urn, CancellationToken cancellationToken) =>
        httpClient.GetAsync(urn, cancellationToken).ToEff()
            .Bind(response => response.Content
                .ReadResponseOrErrorAsync<TResponse, TError>(cancellationToken));

    public static Eff<Either<TError, TResponse>> PostAsync<TRequest, TResponse, TError>(
        this HttpClient httpClient, string urn, TRequest request, CancellationToken cancellationToken) =>
        httpClient.PostAsJsonAsync(urn, request, cancellationToken).ToEff()
            .Bind(response => response.Content
                .ReadResponseOrErrorAsync<TResponse, TError>(cancellationToken));
}
