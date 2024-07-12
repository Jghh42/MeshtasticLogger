namespace MeshtasticLogger.Models.DB;

public class NodeInfo : BaseEntity
{
    public long Hardware { get; set; }
    public string NodeId { get; set; }
    public string LongName { get; set; }
    public string ShortName { get; set; }
    
    public IEnumerable<Position> Positions { get; set; }
    public IEnumerable<DeviceMetrics> DeviceMetrics { get; set; }
}