using System.Text.Json;

namespace SmartGrowHub.Maui.Services.Infrastructure;

public interface IJsonSerializer
{
    T? Deserialize<T>(string json);
    T? Deserialize<T>(Stream stream);
    ValueTask<T?> DeserializeAsync<T>(Stream stream, CancellationToken cancellationToken);
    string Serialize<T>(T value);
    Task SerializeAsync<T>(Stream stream, T value, CancellationToken cancellationToken);
}

public sealed class SystemJsonSerializer : IJsonSerializer
{
    private readonly JsonSerializerOptions _options;

    public SystemJsonSerializer(JsonSerializerOptions options) => _options = options;

    public T? Deserialize<T>(string json) => JsonSerializer.Deserialize<T>(json, _options);

    public T? Deserialize<T>(Stream stream) => JsonSerializer.Deserialize<T>(stream, _options);

    public ValueTask<T?> DeserializeAsync<T>(Stream stream, CancellationToken cancellationToken) =>
        JsonSerializer.DeserializeAsync<T>(stream, _options, cancellationToken);

    public string Serialize<T>(T value) => JsonSerializer.Serialize(value, _options);

    public Task SerializeAsync<T>(Stream stream, T value, CancellationToken cancellationToken) =>
        JsonSerializer.SerializeAsync(stream, value, _options, cancellationToken);
}