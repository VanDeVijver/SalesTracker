using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SalesTracker.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddProjectEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5d595a14-4d90-4651-8770-39c99a578787");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "67c3f919-deeb-4582-acde-ba7e3088ebba");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "63209b0f-a301-4fc2-9d15-5d8b1be774f1", "869c3459-9177-47b0-b703-e9c3824232e1" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "63209b0f-a301-4fc2-9d15-5d8b1be774f1");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "869c3459-9177-47b0-b703-e9c3824232e1");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "5d595a14-4d90-4651-8770-39c99a578787", null, "User", "USER" },
                    { "63209b0f-a301-4fc2-9d15-5d8b1be774f1", null, "Admin", "ADMIN" },
                    { "67c3f919-deeb-4582-acde-ba7e3088ebba", null, "Manager", "MANAGER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "CreatedAt", "Email", "EmailConfirmed", "FirstName", "IsActive", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "869c3459-9177-47b0-b703-e9c3824232e1", 0, "de422752-2a45-4def-b125-84a057578616", new DateTime(2026, 1, 29, 16, 53, 41, 861, DateTimeKind.Utc).AddTicks(4822), "admin@salestracker.com", true, "System", true, "Administrator", false, null, "ADMIN@SALESTRACKER.COM", "ADMIN@SALESTRACKER.COM", "AQAAAAIAAYagAAAAELRWSGqiFOafhDwaeeXnCvEiWrbs79U62l34b1NYLTYtn1YWqfcoakXSXPYfnmPGow==", null, false, "68f9159c-9f9a-40ba-b276-66b9b23b6db4", false, "admin@salestracker.com" });

            migrationBuilder.UpdateData(
                table: "LeadChannels",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 29, 16, 53, 41, 939, DateTimeKind.Utc).AddTicks(6146));

            migrationBuilder.UpdateData(
                table: "LeadChannels",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 29, 16, 53, 41, 939, DateTimeKind.Utc).AddTicks(6149));

            migrationBuilder.UpdateData(
                table: "LeadChannels",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 29, 16, 53, 41, 939, DateTimeKind.Utc).AddTicks(6151));

            migrationBuilder.UpdateData(
                table: "LeadChannels",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 29, 16, 53, 41, 939, DateTimeKind.Utc).AddTicks(6152));

            migrationBuilder.UpdateData(
                table: "LeadChannels",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 29, 16, 53, 41, 939, DateTimeKind.Utc).AddTicks(6153));

            migrationBuilder.UpdateData(
                table: "SystemSettings",
                keyColumn: "Id",
                keyValue: 1,
                column: "UpdatedAt",
                value: new DateTime(2026, 1, 29, 16, 53, 41, 939, DateTimeKind.Utc).AddTicks(6179));

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "63209b0f-a301-4fc2-9d15-5d8b1be774f1", "869c3459-9177-47b0-b703-e9c3824232e1" });
        }
    }
}
