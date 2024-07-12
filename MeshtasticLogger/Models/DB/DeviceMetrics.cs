namespace MeshtasticLogger.Models.DB;

public class DeviceMetrics : BaseEntity
{
    public double AirUtilTx { get; set; }
    public long BatteryLevel { get; set; }
    public double ChannelUtilization { get; set; }
    public double Voltage { get; set; }
}