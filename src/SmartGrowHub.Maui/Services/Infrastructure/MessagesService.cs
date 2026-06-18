using MQTTnet;
using Serilog;
using SmartGrowHub.Shared.GrowHubs.Model;

namespace SmartGrowHub.Maui.Services.Infrastructure;

public interface IMessagesService
{
    event Action<SensorMeasurementDto>? MeasurementReceived;
    
    Task Start();
    Task Stop();
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

    public async Task Start()
    {
        try
        {
            var options = new MqttClientOptionsBuilder()
                .WithTcpServer("10.42.0.1", 1883)
                .Build();

            await _mqttClient.ConnectAsync(options, CancellationToken.None).ConfigureAwait(false);

            var subscribeOptions = new MqttClientSubscribeOptionsBuilder()
                .WithTopicFilter("clients/growHubs/+/sensors/+")
                .Build();

            await _mqttClient.SubscribeAsync(subscribeOptions, CancellationToken.None).ConfigureAwait(false);

            _mqttClient.ApplicationMessageReceivedAsync += OnMessageRecieved;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Failed to start mqtt");
        }
    }

    private Task OnMessageRecieved(MqttApplicationMessageReceivedEventArgs args)
    {
        string payload = args.ApplicationMessage.ConvertPayloadToString();
        var measurement = _jsonSerializer.Deserialize<SensorMeasurementDto>(payload);
        if (measurement is not null)
        {
            MeasurementReceived?.Invoke(measurement);
        }
        return Task.CompletedTask;
    }

    public async Task Stop()
    {
        var options = new MqttClientDisconnectOptionsBuilder()
            .WithReason(MqttClientDisconnectOptionsReason.NormalDisconnection)
            .Build();
        
        await _mqttClient.DisconnectAsync(options).ConfigureAwait(false);
        
        _mqttClient.ApplicationMessageReceivedAsync -= OnMessageRecieved;
    }
}