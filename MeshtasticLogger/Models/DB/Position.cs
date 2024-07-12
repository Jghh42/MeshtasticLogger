namespace MeshtasticLogger.Models.DB;

public class Position
{
    public long Id { get; set; }
    public long From { get; set; }
    public long Timestamp { get; set; }
    public long Alt { get; set; }
    public long Lat { get; set; }
    public long Long { get; set; }
    public long Sats { get; set; }
}