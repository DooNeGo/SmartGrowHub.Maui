using System.Text.Json;

namespace SmartGrowHub.Maui.Services.Infrastructure;

public interface IJsonSerializer
{
    Option<T> Deserialize<T>(string json);
    Option<T> Deserialize<T>(Stream stream);
    OptionT<IO, T> DeserializeAsync<T>(Stream stream, CancellationToken cancellationToken);
    string Serialize<T>(T value);
    IO<Unit> SerializeAsync<T>(Stream stream, T? value, CancellationToken cancellationToken);
}

public sealed class SystemJsonSerializer(JsonSerializerOptions options) : IJsonSerializer
{
    public Option<T> Deserialize<T>(string json) =>
        Optional(JsonSerializer.Deserialize<T>(json, options));

    public Option<T> Deserialize<T>(Stream stream) =>
        Optional(JsonSerializer.Deserialize<T>(stream, options));

    public OptionT<IO, T> DeserializeAsync<T>(Stream stream, CancellationToken cancellationToken) =>
        IO.liftVAsync(() => JsonSerializer
            .DeserializeAsync<T>(stream, options, cancellationToken)
            .Map(Optional));
    
    public string Serialize<T>(T value) => JsonSerializer.Serialize(value, options);
    
    public IO<Unit> SerializeAsync<T>(Stream stream, T? value, CancellationToken cancellationToken) =>
        IO.liftAsync(() => JsonSerializer.SerializeAsync(stream, value, options, cancellationToken).ToUnit());
}