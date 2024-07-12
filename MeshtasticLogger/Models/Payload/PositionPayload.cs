namespace MeshtasticLogger.Models.Payload;

public class PositionPayload
{
    public long altitude { get; set; }
    public long latitude_i { get; set; }
    public long longitude_i { get; set; }
    public long sats_in_view { get; set; }
}