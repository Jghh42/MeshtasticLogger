namespace MeshtasticLogger.Models;

public class OverviewModel
{
    public long? NIFrom { get; set; }
    public long? DMFrom { get; set; }
    public long? PFrom { get; set; }

    public long From => (long)(NIFrom ?? DMFrom ?? PFrom)!;
    public long? NITimestamp { get; set; }
    public long? DMTimestamp { get; set; }
    public long? PTimestamp { get; set; }

    private DateTimeOffset? _timestamp;

    public DateTimeOffset Timestamp
    {
        get
        {
            var max = (long)new[] { NITimestamp, DMTimestamp, PTimestamp }.Max()!;
            _timestamp ??= DateTimeOffset.FromUnixTimeSeconds(max);
            return _timestamp.Value;
        }
    }

    public string NodeId { get; set; }
    public string ShortName { get; set; }
    public string LongName { get; set; }
    public string AirUtilTx { get; set; }
    public string ChannelUtilization { get; set; }
    public string BatteryLevel { get; set; }
    public string Voltage { get; set; }
    public string Alt { get; set; }
    public string Lat { get; set; }
    public string Long { get; set; }
    public string Sats { get; set; }
}