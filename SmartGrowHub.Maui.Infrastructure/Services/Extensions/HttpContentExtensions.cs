using SmartGrowHub.Domain.Extensions;
using SmartGrowHub.Shared.SerializerContext;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SmartGrowHub.Maui.Infrastructure.Services.Extensions;

public static class HttpContentExtensions
{
    private static readonly JsonSerializerOptions Options = CreateOptions();

    public static Eff<Either<TError, TResponse>> ReadResponseOrErrorAsync<TResponse, TError>(
        this HttpContent content, CancellationToken cancellationToken) =>
        content.ReadAsStringAsync(cancellationToken).ToEff()
            .Bind(json =>
                json.ReadValue<TResponse>().ToEff()
                    .Map(response => (Either<TError, TResponse>)response) |
                json.ReadValue<TError>().ToEff()
                    .Map(error => (Either<TError, TResponse>)error));

    private static Fin<T> ReadValue<T>(this string json) =>
        Optional(JsonSerializer.Deserialize<T>(json, Options)).ToFin();

    private static JsonSerializerOptions CreateOptions()
    {
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        };

        options.TypeInfoResolverChain.Add(SmartGrowHubSerializerContext.Default);

        return options;
    }
}
