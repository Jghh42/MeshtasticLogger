using MeshtasticLogger.Models;
using MeshtasticLogger.Models.DB;
using Microsoft.EntityFrameworkCore;

namespace MeshtasticLogger;

public sealed class MeshtasticDbContext(NpsqlConfig config) : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(config.ConnectionString)
            .LogTo(s => System.Diagnostics.Debug.WriteLine(s))
            .EnableDetailedErrors()
            .EnableSensitiveDataLogging();
    }
    
    public DbSet<NodeInfo> NodeInfos { get; set; }
    public DbSet<Position> Positions { get; set; }
    public DbSet<DeviceMetrics> DeviceMetrics { get; set; }
}