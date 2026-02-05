using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SalesTracker.Core.Migrations
{
    /// <inheritdoc />
    public partial class ChangeMarginToPercentage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "438d97b2-7540-4cc5-8252-5cf77784ae86");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a0202451-5e26-49b0-aa5a-c5bc2a71c384");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "a8414a98-c271-4288-aaf2-58a3ec3c236e", "5abd5139-23b0-4d5a-98bf-121a59bf2289" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a8414a98-c271-4288-aaf2-58a3ec3c236e");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "5abd5139-23b0-4d5a-98bf-121a59bf2289");

            migrationBuilder.DropColumn(
                name: "CafcaMargin",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "ManualMargin",
                table: "Projects");

            migrationBuilder.AddColumn<decimal>(
                name: "CafcaMarginPercentage",
                table: "Projects",
                type: "numeric(5,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ManualMarginPercentage",
                table: "Projects",
                type: "numeric(5,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "487e8222-d068-4ba3-9733-60f1dfd717e6", null, "User", "USER" },
                    { "5f5edbcd-49f0-47a4-8f96-d84cdf5ef89f", null, "Manager", "MANAGER" },
                    { "70be922a-d6d2-49d5-ad67-749aaca4f82c", null, "Admin", "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "CreatedAt", "Email", "EmailConfirmed", "FirstName", "IsActive", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "048d93b7-a0a3-4827-9044-3596b74548f6", 0, "61b16bc4-b806-49f1-a706-3735dc51593d", new DateTime(2026, 2, 5, 12, 56, 4, 125, DateTimeKind.Utc).AddTicks(3990), "admin@salestracker.com", true, "System", true, "Administrator", false, null, "ADMIN@SALESTRACKER.COM", "ADMIN@SALESTRACKER.COM", "AQAAAAIAAYagAAAAEMdHp/4kFJvyNieqSYZ7AesVlbwPO8YI3K0UQ5EHeKxNaiPtcMiUUSDMfL6omj50aQ==", null, false, "5dc40079-6188-4571-a46d-247acd88bdae", false, "admin@salestracker.com" });

            migrationBuilder.UpdateData(
                table: "LeadChannels",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 5, 12, 56, 4, 252, DateTimeKind.Utc).AddTicks(5610));

            migrationBuilder.UpdateData(
                table: "LeadChannels",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 5, 12, 56, 4, 252, DateTimeKind.Utc).AddTicks(5619));

            migrationBuilder.UpdateData(
                table: "LeadChannels",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 5, 12, 56, 4, 252, DateTimeKind.Utc).AddTicks(5621));

            migrationBuilder.UpdateData(
                table: "LeadChannels",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 5, 12, 56, 4, 252, DateTimeKind.Utc).AddTicks(5622));

            migrationBuilder.UpdateData(
                table: "LeadChannels",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 5, 12, 56, 4, 252, DateTimeKind.Utc).AddTicks(5624));

            migrationBuilder.UpdateData(
                table: "SystemSettings",
                keyColumn: "Id",
                keyValue: 1,
                column: "UpdatedAt",
                value: new DateTime(2026, 2, 5, 12, 56, 4, 252, DateTimeKind.Utc).AddTicks(5729));

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "70be922a-d6d2-49d5-ad67-749aaca4f82c", "048d93b7-a0a3-4827-9044-3596b74548f6" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "487e8222-d068-4ba3-9733-60f1dfd717e6");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5f5edbcd-49f0-47a4-8f96-d84cdf5ef89f");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "70be922a-d6d2-49d5-ad67-749aaca4f82c", "048d93b7-a0a3-4827-9044-3596b74548f6" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "70be922a-d6d2-49d5-ad67-749aaca4f82c");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "048d93b7-a0a3-4827-9044-3596b74548f6");

            migrationBuilder.DropColumn(
                name: "CafcaMarginPercentage",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "ManualMarginPercentage",
                table: "Projects");

            migrationBuilder.AddColumn<decimal>(
                name: "CafcaMargin",
                table: "Projects",
                type: "numeric(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ManualMargin",
                table: "Projects",
                type: "numeric(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "438d97b2-7540-4cc5-8252-5cf77784ae86", null, "Manager", "MANAGER" },
                    { "a0202451-5e26-49b0-aa5a-c5bc2a71c384", null, "User", "USER" },
                    { "a8414a98-c271-4288-aaf2-58a3ec3c236e", null, "Admin", "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "CreatedAt", "Email", "EmailConfirmed", "FirstName", "IsActive", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "5abd5139-23b0-4d5a-98bf-121a59bf2289", 0, "16b65908-03ee-4eca-a81f-ed7821f33d6a", new DateTime(2026, 1, 29, 18, 8, 58, 964, DateTimeKind.Utc).AddTicks(3802), "admin@salestracker.com", true, "System", true, "Administrator", false, null, "ADMIN@SALESTRACKER.COM", "ADMIN@SALESTRACKER.COM", "AQAAAAIAAYagAAAAEDA6n3Fk32gPg5efHYQTsI5IvUxnCbuzh0iQnKzosV0t/ULRQ4iSPRbfCh0os0pI7g==", null, false, "a483ee7f-905d-4d21-bfe8-eb2d4a378de2", false, "admin@salestracker.com" });

            migrationBuilder.UpdateData(
                table: "LeadChannels",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 29, 18, 8, 59, 31, DateTimeKind.Utc).AddTicks(5727));

            migrationBuilder.UpdateData(
                table: "LeadChannels",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 29, 18, 8, 59, 31, DateTimeKind.Utc).AddTicks(5730));

            migrationBuilder.UpdateData(
                table: "LeadChannels",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 29, 18, 8, 59, 31, DateTimeKind.Utc).AddTicks(5732));

            migrationBuilder.UpdateData(
                table: "LeadChannels",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 29, 18, 8, 59, 31, DateTimeKind.Utc).AddTicks(5734));

            migrationBuilder.UpdateData(
                table: "LeadChannels",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 29, 18, 8, 59, 31, DateTimeKind.Utc).AddTicks(5735));

            migrationBuilder.UpdateData(
                table: "SystemSettings",
                keyColumn: "Id",
                keyValue: 1,
                column: "UpdatedAt",
                value: new DateTime(2026, 1, 29, 18, 8, 59, 31, DateTimeKind.Utc).AddTicks(5773));

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "a8414a98-c271-4288-aaf2-58a3ec3c236e", "5abd5139-23b0-4d5a-98bf-121a59bf2289" });
        }
    }
}
