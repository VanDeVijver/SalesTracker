using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SalesTracker.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddProjectChecklistAndLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "CheckCafca",
                table: "Projects",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CheckFolder",
                table: "Projects",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CheckMaterial",
                table: "Projects",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CheckPlanning",
                table: "Projects",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ProjectLog",
                table: "Projects",
                type: "text",
                nullable: true);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CheckCafca",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "CheckFolder",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "CheckMaterial",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "CheckPlanning",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "ProjectLog",
                table: "Projects");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "733ee75f-c07a-4ccf-b97b-a0c4735ae164");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2",
                column: "ConcurrencyStamp",
                value: "fe8f08d2-0d41-4f60-a24b-4d9124d6c254");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3",
                column: "ConcurrencyStamp",
                value: "b700c13f-8306-469e-bf08-4388774e99bb");

            migrationBuilder.UpdateData(
                table: "LeadChannels",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 5, 14, 32, 32, 300, DateTimeKind.Utc).AddTicks(4202));

            migrationBuilder.UpdateData(
                table: "LeadChannels",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 5, 14, 32, 32, 300, DateTimeKind.Utc).AddTicks(4206));

            migrationBuilder.UpdateData(
                table: "LeadChannels",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 5, 14, 32, 32, 300, DateTimeKind.Utc).AddTicks(4207));

            migrationBuilder.UpdateData(
                table: "LeadChannels",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 5, 14, 32, 32, 300, DateTimeKind.Utc).AddTicks(4209));

            migrationBuilder.UpdateData(
                table: "LeadChannels",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 5, 14, 32, 32, 300, DateTimeKind.Utc).AddTicks(4210));

            migrationBuilder.UpdateData(
                table: "SystemSettings",
                keyColumn: "Id",
                keyValue: 1,
                column: "UpdatedAt",
                value: new DateTime(2026, 2, 5, 14, 32, 32, 300, DateTimeKind.Utc).AddTicks(4308));
        }
    }
}
