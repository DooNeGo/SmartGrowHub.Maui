using MQTTnet;
using Serilog;
using SmartGrowHub.Shared.GrowHubs.Model;

namespace SmartGrowHub.Maui.Services.Infrastructure;

public interface IMessagesService
{
    event Action<SensorMeasurementDto>? MeasurementReceived;
    
    IO<Unit> Start();
    IO<Unit> Stop();
}

public sealed class MessagesService : IMessagesService
{
    private readonly IMqttClient _mqttClient;
    private readonly IJsonSerializer _jsonSerializer;
    private readonly ILogger _logger;

    public MessagesService(
        IMqttClient mqttClient,
        IJsonSerializer jsonSerializer,
        ILogger logger)
    {
        _mqttClient = mqttClient;
        _jsonSerializer = jsonSerializer;
        _logger = logger;
    }
    
    public event Action<SensorMeasurementDto>? MeasurementReceived;

    public IO<Unit> Start() =>
        IO.liftAsync(async env =>
        {
            MqttClientOptions options = new MqttClientOptionsBuilder()
                .WithTcpServer("10.42.0.1", 1883)
                .Build();

            await _mqttClient.ConnectAsync(options, env.Token).ConfigureAwait(false);

            MqttClientSubscribeOptions subscribeOptions = new MqttClientSubscribeOptionsBuilder()
                .WithTopicFilter("clients/growHubs/+/sensors/+")
                .Build();

            await _mqttClient.SubscribeAsync(subscribeOptions, env.Token).ConfigureAwait(false);

            _mqttClient.ApplicationMessageReceivedAsync += OnMessageRecieved;

            return Unit.Default;
        }).Catch(error => IO.lift(() => _logger.Error(error.ToErrorException(), "Failed to start mqtt"))).As();

    private Task OnMessageRecieved(MqttApplicationMessageReceivedEventArgs args)
    {
        string payload = args.ApplicationMessage.ConvertPayloadToString();
        Option<SensorMeasurementDto> measurement = _jsonSerializer.Deserialize<SensorMeasurementDto>(payload);
        measurement.IfSome(m => MeasurementReceived?.Invoke(m));
        return Task.CompletedTask;
    }

    public IO<Unit> Stop() =>
        IO.liftAsync(async () =>
        {
            MqttClientDisconnectOptions options = new MqttClientDisconnectOptionsBuilder()
                .WithReason(MqttClientDisconnectOptionsReason.NormalDisconnection)
                .Build();
            
            await _mqttClient.DisconnectAsync(options).ConfigureAwait(false);
            
            _mqttClient.ApplicationMessageReceivedAsync -= OnMessageRecieved;

            return Unit.Default;
        });
}