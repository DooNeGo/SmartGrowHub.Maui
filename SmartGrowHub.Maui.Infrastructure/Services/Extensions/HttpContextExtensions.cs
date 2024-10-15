using System.Net.Http.Json;

namespace SmartGrowHub.Maui.Infrastructure.Services.Extensions;

public static class HttpContextExtensions
{
    public static Eff<Either<TError, TResponse>> ReadJsonOrErrorAsync<TResponse, TError>(
        this HttpContent content, CancellationToken cancellationToken) =>
        content.ReadJsonEffAsync<TResponse>(cancellationToken)
            .Map(response => (Either<TError, TResponse>)response) |
        content.ReadJsonEffAsync<TError>(cancellationToken)
            .Map(error => (Either<TError, TResponse>)error);

    private static Eff<T> ReadJsonEffAsync<T>(
        this HttpContent content, CancellationToken cancellationToken) =>
        liftEff(() => content
            .ReadFromJsonAsync<T>(cancellationToken)
            .Map(Optional))
        .Bind(option => option.ToEff());
}
