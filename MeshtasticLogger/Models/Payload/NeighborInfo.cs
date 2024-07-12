namespace MeshtasticLogger.Models.Payload;

public class NeighborInfo
{
    public long last_sent_by_id { get; set; }
    public List<Neighbor> neighbors { get; set; }
    public int neighbors_count { get; set; }
    public int node_broadcast_interval_secs { get; set; }
    public int node_id { get; set; }
}

public class Neighbor
{
    public object node_id { get; set; }
    public int snr { get; set; }
}