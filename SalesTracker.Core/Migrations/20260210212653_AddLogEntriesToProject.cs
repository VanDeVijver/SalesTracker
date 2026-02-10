using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SalesTracker.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddLogEntriesToProject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LogEntries",
                table: "Projects",
                type: "text",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "93dded7a-6601-4ea2-b220-e730f17ffa9b");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2",
                column: "ConcurrencyStamp",
                value: "5f49c089-5580-4222-ac57-d144ea6d794f");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3",
                column: "ConcurrencyStamp",
                value: "f8b79dff-0843-4f07-8fba-df452669ad85");

            migrationBuilder.UpdateData(
                table: "LeadChannels",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 10, 21, 26, 52, 673, DateTimeKind.Utc).AddTicks(9567));

            migrationBuilder.UpdateData(
                table: "LeadChannels",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 10, 21, 26, 52, 673, DateTimeKind.Utc).AddTicks(9574));

            migrationBuilder.UpdateData(
                table: "LeadChannels",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 10, 21, 26, 52, 673, DateTimeKind.Utc).AddTicks(9576));

            migrationBuilder.UpdateData(
                table: "LeadChannels",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 10, 21, 26, 52, 673, DateTimeKind.Utc).AddTicks(9577));

            migrationBuilder.UpdateData(
                table: "LeadChannels",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 10, 21, 26, 52, 673, DateTimeKind.Utc).AddTicks(9578));

            migrationBuilder.UpdateData(
                table: "SystemSettings",
                keyColumn: "Id",
                keyValue: 1,
                column: "UpdatedAt",
                value: new DateTime(2026, 2, 10, 21, 26, 52, 673, DateTimeKind.Utc).AddTicks(9602));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LogEntries",
                table: "Projects");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "cb02c9b4-3ba5-4ee8-a230-3c43a78f4c23");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2",
                column: "ConcurrencyStamp",
                value: "c9eccfe7-16c9-4269-80fc-7c3f3e220d70");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3",
                column: "ConcurrencyStamp",
                value: "ef315471-00db-4ff1-b090-e0f8df1db44a");

            migrationBuilder.UpdateData(
                table: "LeadChannels",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 8, 16, 47, 16, 641, DateTimeKind.Utc).AddTicks(1210));

            migrationBuilder.UpdateData(
                table: "LeadChannels",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 8, 16, 47, 16, 641, DateTimeKind.Utc).AddTicks(1214));

            migrationBuilder.UpdateData(
                table: "LeadChannels",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 8, 16, 47, 16, 641, DateTimeKind.Utc).AddTicks(1215));

            migrationBuilder.UpdateData(
                table: "LeadChannels",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 8, 16, 47, 16, 641, DateTimeKind.Utc).AddTicks(1216));

            migrationBuilder.UpdateData(
                table: "LeadChannels",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 8, 16, 47, 16, 641, DateTimeKind.Utc).AddTicks(1217));

            migrationBuilder.UpdateData(
                table: "SystemSettings",
                keyColumn: "Id",
                keyValue: 1,
                column: "UpdatedAt",
                value: new DateTime(2026, 2, 8, 16, 47, 16, 641, DateTimeKind.Utc).AddTicks(1243));
        }
    }
}
