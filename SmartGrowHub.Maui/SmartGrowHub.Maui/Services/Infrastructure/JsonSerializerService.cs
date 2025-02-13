using System.Text.Json;

namespace SmartGrowHub.Maui.Services.Infrastructure;

public interface IJsonSerializerService
{
    Option<TResult> Deserialize<TResult>(string json);
    Option<TResult> Deserialize<TResult>(Stream stream);
    OptionT<Eff, TResult> DeserializeAsync<TResult>(Stream stream, CancellationToken cancellationToken);
    string Serialize<TResult>(TResult value);
    IO<Unit> SerializeAsync<TValue>(Stream stream, TValue? value, CancellationToken cancellationToken);
}

public sealed class JsonSerializerService(JsonSerializerOptions options) : IJsonSerializerService
{
    public Option<TResult> Deserialize<TResult>(string json) =>
        Optional(JsonSerializer.Deserialize<TResult>(json, options));

    public Option<TResult> Deserialize<TResult>(Stream stream) =>
        Optional(JsonSerializer.Deserialize<TResult>(stream, options));

    public OptionT<Eff, TResult> DeserializeAsync<TResult>(Stream stream, CancellationToken cancellationToken) =>
        IO.liftAsync(() => JsonSerializer
            .DeserializeAsync<TResult>(stream, options, cancellationToken).AsTask()
            .Map(Optional));
    
    public string Serialize<TValue>(TValue value) => JsonSerializer.Serialize(value, options);
    
    public IO<Unit> SerializeAsync<TValue>(Stream stream, TValue? value, CancellationToken cancellationToken) =>
        IO.liftAsync(() => JsonSerializer.SerializeAsync(stream, value, options, cancellationToken).ToUnit());
}