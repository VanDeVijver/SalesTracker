using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SalesTracker.Core.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LeadChannels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeadChannels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SystemSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Value = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CategoryTargets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Year = table.Column<int>(type: "integer", nullable: false),
                    CategoryId = table.Column<int>(type: "integer", nullable: false),
                    TargetAmount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryTargets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CategoryTargets_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Customer = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    ClientType = table.Column<int>(type: "integer", nullable: false),
                    CategoryId = table.Column<int>(type: "integer", nullable: false),
                    LeadChannelId = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Purchase = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    ManualMargin = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Hours = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    CafcaMargin = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    CafcaHours = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    FinalInvoiceAmount = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LostReason = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Notes = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Projects_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Projects_LeadChannels_LeadChannelId",
                        column: x => x.LeadChannelId,
                        principalTable: "LeadChannels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "IsActive", "Name" },
                values: new object[,]
                {
                    { 1, true, "PV-BAT B2C" },
                    { 2, true, "PV-BAT B2B" },
                    { 3, true, "WeVolt" },
                    { 4, true, "Lightweight PV" },
                    { 5, true, "BESS" },
                    { 6, true, "Charge" },
                    { 7, true, "ALSB" }
                });

            migrationBuilder.InsertData(
                table: "LeadChannels",
                columns: new[] { "Id", "CreatedAt", "IsActive", "Name" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 25, 11, 39, 29, 843, DateTimeKind.Utc).AddTicks(9147), true, "Eigen leads" },
                    { 2, new DateTime(2026, 1, 25, 11, 39, 29, 843, DateTimeKind.Utc).AddTicks(9151), true, "Davy" },
                    { 3, new DateTime(2026, 1, 25, 11, 39, 29, 843, DateTimeKind.Utc).AddTicks(9153), true, "SMA" },
                    { 4, new DateTime(2026, 1, 25, 11, 39, 29, 843, DateTimeKind.Utc).AddTicks(9154), true, "SolarWatt" },
                    { 5, new DateTime(2026, 1, 25, 11, 39, 29, 843, DateTimeKind.Utc).AddTicks(9155), true, "Wienerberger" }
                });

            migrationBuilder.InsertData(
                table: "SystemSettings",
                columns: new[] { "Id", "Key", "UpdatedAt", "Value" },
                values: new object[] { 1, "HourlyRate", new DateTime(2026, 1, 25, 11, 39, 29, 843, DateTimeKind.Utc).AddTicks(9175), "50" });

            migrationBuilder.CreateIndex(
                name: "IX_CategoryTargets_CategoryId",
                table: "CategoryTargets",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryTargets_Year_CategoryId",
                table: "CategoryTargets",
                columns: new[] { "Year", "CategoryId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Projects_CategoryId",
                table: "Projects",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_Date_Status",
                table: "Projects",
                columns: new[] { "Date", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_Projects_LeadChannelId",
                table: "Projects",
                column: "LeadChannelId");

            migrationBuilder.CreateIndex(
                name: "IX_SystemSettings_Key",
                table: "SystemSettings",
                column: "Key",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CategoryTargets");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "SystemSettings");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "LeadChannels");
        }
    }
}
