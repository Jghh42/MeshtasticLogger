using System.ComponentModel.DataAnnotations.Schema;
using Dapper;
using MeshtasticLogger;
using MeshtasticLogger.Models;
using Microsoft.EntityFrameworkCore;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<NpsqlConfig>(_ => builder.Configuration.GetSection("Npsql").Get<NpsqlConfig>()!);
builder.Services.AddDbContext<MeshtasticDbContext>();
builder.Services.AddHostedService<MqttService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapGet("/overview", async (NpsqlConfig config, MeshtasticDbContext context) =>
    {
        await using var connection = new NpgsqlConnection(config.ConnectionString);

        var result = await connection.QueryAsync<OverviewModel>("""
                              WITH ranked_nodeinfo AS (
                                  SELECT *, ROW_NUMBER() over (PARTITION BY "From" ORDER BY "Id" DESC ) as rn
                                  FROM "NodeInfos"
                              ), ranked_devicemetrics AS (
                                  SELECT *, ROW_NUMBER() over (PARTITION BY "From" ORDER BY "Id" DESC ) as rn
                                  FROM "DeviceMetrics"
                              ), ranked_positions AS (
                                  SELECT *, ROW_NUMBER() over (PARTITION BY "From" ORDER BY "Id" DESC ) as rn
                                  FROM "Positions"
                              )
                              SELECT  NI."From" as NIFrom, NI."NodeId", NI."ShortName", NI."LongName", NI."Timestamp" as NITimestamp, DM."From" as DMFrom, DM."AirUtilTx",DM."ChannelUtilization", DM."BatteryLevel", DM."Voltage",DM."Timestamp" as DMTimestamp,P."From" as PFrom,P."Alt",P."Lat",P."Long",P."Sats",P."Timestamp" as PTimestamp
                              FROM ranked_nodeinfo NI
                                       FULL JOIN ranked_devicemetrics DM on DM."From" = NI."From"
                                       FULL JOIN ranked_positions P on P."From" = NI."From"
                              WHERE (NI.rn=1 or NI.rn is null)  and (DM.rn=1 or DM.rn is null) and (P.rn = 1 or P.rn is null);
                              """);
        
        return result;
    })
    .WithName("overview")
    .WithOpenApi();


app.MapGet("/test", async (MeshtasticDbContext dbContext) =>
    {
        var positionIds = dbContext.Positions.Select(p =>  p.Id).ToList();
        var nodeIds = dbContext.NodeInfos.Select(p => p.Id).ToList();
        var deviceIds = dbContext.DeviceMetrics.Select(p => p.Id).ToList();



        return new
        {
            positionCount=positionIds.Count,nodeCount=nodeIds.Count,deviceCount=deviceIds.Count,
            positionCountDistinct=positionIds.Distinct().Count(),nodeCountDistinct=nodeIds.Distinct().Count(),deviceCountDistinct=deviceIds.Distinct().Count()
        };
    })
    .WithName("test")
    .WithOpenApi();


app.Run();