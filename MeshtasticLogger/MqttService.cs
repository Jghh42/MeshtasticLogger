using System.Text;
using System.Text.Json;
using MeshtasticLogger.Models;
using MeshtasticLogger.Models.DB;
using MeshtasticLogger.Models.Payload;
using MQTTnet;
using MQTTnet.Client;

namespace MeshtasticLogger;

public class MqttService : IHostedService
{
    private readonly ILogger<MqttService> _logger;
    private readonly IConfiguration _configuration;
    private readonly IMqttClient _mqttClient;

    public MqttService(ILogger<MqttService> logger, IConfiguration configuration, IServiceProvider services)
    {
        _logger = logger;
        _configuration = configuration;
        _mqttClient = new MqttFactory().CreateMqttClient();
        _mqttClient.ApplicationMessageReceivedAsync += e =>
        {
            try
            {
                using var scope = services.CreateScope();
                var dbContext = scope.ServiceProvider.GetService<MeshtasticDbContext>();
                if (!e.ApplicationMessage.Topic.Contains("json")) return Task.CompletedTask;
                
                var jsonPayload = Encoding.UTF8.GetString(e.ApplicationMessage.PayloadSegment);
                _logger.LogDebug(e.ApplicationMessage.Topic+":"+jsonPayload);
                
                var message = JsonSerializer.Deserialize<Message>(jsonPayload);
                switch (message?.type)
                {
                    case "nodeinfo":
                    {
                        HandleNodeInfo(dbContext, jsonPayload);
                        break;
                    }
                    case "telemetry":
                    {
                        HandleTelemetry(dbContext, jsonPayload);
                        break;
                    }
                    case "position":
                    {
                        HandlePosition(dbContext, jsonPayload);
                        break;
                    }
                    case "neighborinfo":
                    {
                        HandleNeighborInfo(dbContext, jsonPayload);
                        break;
                    }
                    default:
                    {
                        Console.WriteLine("Unhandled type: " + message?.type);
                        break;
                    }
                }
            }
            catch (Exception exception)
            {
                return Task.FromException(exception);
            }

            return Task.CompletedTask;
        };
    }

    private void HandlePosition(MeshtasticDbContext? dbContext, string jsonPayload)
    {
        var position = JsonSerializer.Deserialize<MessageWithPaylod<PositionPayload>>(jsonPayload);

        if (position == null)
        {
            _logger.LogError("Position error, json: {}",jsonPayload);
            return;
        }
        
        dbContext.Add(new Position
            {
                Alt = position.payload.altitude,
                Lat = position.payload.latitude_i,
                Long = position.payload.longitude_i,
                Sats = position.payload.sats_in_view,
                From = position.from,
                Timestamp = position.timestamp
            }
        );
        
        dbContext.SaveChanges();
    }    
    private void HandleNeighborInfo(MeshtasticDbContext? dbContext, string jsonPayload)
    {
        var position = JsonSerializer.Deserialize<MessageWithPaylod<NeighborInfo>>(jsonPayload);

        if (position == null)
        {
            _logger.LogError("NeighborInfo error, json: {}",jsonPayload);
            return;
        }
        
        //TODO
    }

    private void HandleTelemetry(MeshtasticDbContext? dbContext, string jsonPayload)
    {
        var deviceMetrics = JsonSerializer.Deserialize<MessageWithPaylod<DeviceMetricsPayload>>(jsonPayload);

        if (deviceMetrics == null)
        {
            _logger.LogError("DeviceMetrics error, json: {}",jsonPayload);
            return;
        }
        
        dbContext.Add(new DeviceMetrics
        {
            From = deviceMetrics.from,
            Timestamp = deviceMetrics.timestamp,
            Voltage = deviceMetrics.payload.voltage,
            BatteryLevel = deviceMetrics.payload.battery_level,
            ChannelUtilization = deviceMetrics.payload.channel_utilization,
            AirUtilTx = deviceMetrics.payload.air_util_tx
        });
        dbContext.SaveChanges();
    }

    private void HandleNodeInfo(MeshtasticDbContext? dbContext, string jsonPayload)
    {
        var nodeInfo = JsonSerializer.Deserialize<MessageWithPaylod<NodeInfoPayload>>(jsonPayload);
        
        if (nodeInfo == null)
        {
            _logger.LogError("NodeInfo error, json: {}",jsonPayload);
            return;
        }

        dbContext.Add(new NodeInfo
        {
            ShortName = nodeInfo.payload.shortname,
            LongName = nodeInfo.payload.longname,
            Hardware = nodeInfo.payload.hardware,
            NodeId = nodeInfo.payload.id,
            From = nodeInfo.from,
            Timestamp = nodeInfo.timestamp
        });
        dbContext.SaveChanges();
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var mqttClientOptions = new MqttClientOptionsBuilder().WithTcpServer(_configuration.GetValue<string>("MqttHost")).Build();
        var response = await _mqttClient.ConnectAsync(mqttClientOptions, cancellationToken);

        if (response.ResultCode == MqttClientConnectResultCode.Success)
        {
            var mqttClientSubscribeOptions = new MqttClientSubscribeOptionsBuilder().WithTopicFilter(f => f.WithTopic("#")).Build();
            var mqttClientSubscribeResult = await _mqttClient.SubscribeAsync(mqttClientSubscribeOptions, cancellationToken);

            if (mqttClientSubscribeResult.Items.Any(r => (int)r.ResultCode > 10))
            {
                _logger.LogCritical("Couldn't subscribe to topic, errors: {}",mqttClientSubscribeResult.ReasonString);
            }
        }
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await _mqttClient.DisconnectAsync(new MqttClientDisconnectOptions(),cancellationToken);
    }
}