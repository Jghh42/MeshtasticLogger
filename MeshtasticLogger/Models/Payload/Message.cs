namespace MeshtasticLogger.Models.Payload;

public class MessageWithPaylod<T> : Message
{
    public T payload { get; set; }
}
public class Message
{
    public long channel { get; set; }
    public long from { get; set; }
    public long id { get; set; }
    public long rssi { get; set; }
    public string sender { get; set; }
    public double snr { get; set; }
    public long timestamp { get; set; }
    public long to { get; set; }
    public string type { get; set; }
}



