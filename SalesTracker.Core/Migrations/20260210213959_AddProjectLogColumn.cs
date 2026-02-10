using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SalesTracker.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddProjectLogColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LogEntries",
                table: "Projects");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "f5b9dcde-8ad3-4048-aedc-2bbe4cbe6283");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2",
                column: "ConcurrencyStamp",
                value: "5e8e9c7a-39a6-4087-af63-68a9ca821400");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3",
                column: "ConcurrencyStamp",
                value: "0ae542d0-adfe-4fdf-833a-a6373f4aff67");

            migrationBuilder.UpdateData(
                table: "LeadChannels",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 10, 21, 39, 58, 673, DateTimeKind.Utc).AddTicks(9703));

            migrationBuilder.UpdateData(
                table: "LeadChannels",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 10, 21, 39, 58, 673, DateTimeKind.Utc).AddTicks(9707));

            migrationBuilder.UpdateData(
                table: "LeadChannels",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 10, 21, 39, 58, 673, DateTimeKind.Utc).AddTicks(9708));

            migrationBuilder.UpdateData(
                table: "LeadChannels",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 10, 21, 39, 58, 673, DateTimeKind.Utc).AddTicks(9709));

            migrationBuilder.UpdateData(
                table: "LeadChannels",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 10, 21, 39, 58, 673, DateTimeKind.Utc).AddTicks(9710));

            migrationBuilder.UpdateData(
                table: "SystemSettings",
                keyColumn: "Id",
                keyValue: 1,
                column: "UpdatedAt",
                value: new DateTime(2026, 2, 10, 21, 39, 58, 673, DateTimeKind.Utc).AddTicks(9734));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
    }
}
