using Microsoft.EntityFrameworkCore;

namespace MeshtasticLogger.Models.DB;

[Index(nameof(From), nameof(Timestamp))]
public class BaseEntity
{
    public long Id { get; set; }
    public long From { get; set; }
    public long Timestamp { get; set; }
}