using System.Text.Json;

namespace SmartGrowHub.Maui.Services.Infrastructure;

public interface IJsonSerializer
{
    Option<T> Deserialize<T>(string json);
    Option<T> Deserialize<T>(Stream stream);
    OptionT<IO, T> DeserializeAsync<T>(Stream stream);
    ValueTask<T?> DeserializeAsync<T>(Stream stream, CancellationToken cancellationToken);
    Fin<string> Serialize<T>(T value);
    IO<Unit> SerializeAsync<T>(Stream stream, T? value);
}

public sealed class SystemJsonSerializer(JsonSerializerOptions options) : IJsonSerializer
{
    public Option<T> Deserialize<T>(string json) => JsonSerializer.Deserialize<T>(json, options);

    public Option<T> Deserialize<T>(Stream stream) => JsonSerializer.Deserialize<T>(stream, options);

    public OptionT<IO, T> DeserializeAsync<T>(Stream stream) =>
        IO.liftVAsync(env => JsonSerializer
            .DeserializeAsync<T>(stream, options, env.Token)
            .Map(Prelude.Optional));

    public ValueTask<T?> DeserializeAsync<T>(Stream stream, CancellationToken cancellationToken) =>
        JsonSerializer.DeserializeAsync<T>(stream, options, cancellationToken);

    public Fin<string> Serialize<T>(T value)
    {
        try
        {
            string result = JsonSerializer.Serialize(value, options);
            return Fin.Succ(result);
        }
        catch (Exception e)
        {
            return Fin.Fail<string>(e);
        }
    }

    public IO<Unit> SerializeAsync<T>(Stream stream, T? value) =>
        IO.liftAsync(env => JsonSerializer.SerializeAsync(stream, value, options, env.Token).ToUnit());
}