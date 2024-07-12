using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MeshtasticLogger.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DeviceMetrics",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AirUtilTx = table.Column<double>(type: "double precision", nullable: false),
                    BatteryLevel = table.Column<long>(type: "bigint", nullable: false),
                    ChannelUtilization = table.Column<double>(type: "double precision", nullable: false),
                    Voltage = table.Column<double>(type: "double precision", nullable: false),
                    From = table.Column<long>(type: "bigint", nullable: false),
                    Timestamp = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceMetrics", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NodeInfos",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Hardware = table.Column<long>(type: "bigint", nullable: false),
                    NodeId = table.Column<string>(type: "text", nullable: false),
                    LongName = table.Column<string>(type: "text", nullable: false),
                    ShortName = table.Column<string>(type: "text", nullable: false),
                    From = table.Column<long>(type: "bigint", nullable: false),
                    Timestamp = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NodeInfos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Positions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Alt = table.Column<long>(type: "bigint", nullable: false),
                    Lat = table.Column<long>(type: "bigint", nullable: false),
                    Long = table.Column<long>(type: "bigint", nullable: false),
                    Sats = table.Column<long>(type: "bigint", nullable: false),
                    From = table.Column<long>(type: "bigint", nullable: false),
                    Timestamp = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Positions", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DeviceMetrics_From_Timestamp",
                table: "DeviceMetrics",
                columns: new[] { "From", "Timestamp" });

            migrationBuilder.CreateIndex(
                name: "IX_NodeInfos_From_Timestamp",
                table: "NodeInfos",
                columns: new[] { "From", "Timestamp" });

            migrationBuilder.CreateIndex(
                name: "IX_Positions_From_Timestamp",
                table: "Positions",
                columns: new[] { "From", "Timestamp" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeviceMetrics");

            migrationBuilder.DropTable(
                name: "NodeInfos");

            migrationBuilder.DropTable(
                name: "Positions");
        }
    }
}
