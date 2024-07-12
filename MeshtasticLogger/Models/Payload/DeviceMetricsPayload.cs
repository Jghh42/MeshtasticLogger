namespace MeshtasticLogger.Models.Payload;

public class DeviceMetricsPayload
{
    public double air_util_tx { get; set; }
    public long battery_level { get; set; }
    public double channel_utilization { get; set; }
    public double voltage { get; set; }
}