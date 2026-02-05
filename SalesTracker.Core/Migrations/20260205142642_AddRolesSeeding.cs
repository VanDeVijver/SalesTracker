using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SalesTracker.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddRolesSeeding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Categories_CategoryId",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_LeadChannels_LeadChannelId",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_Date_Status",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_CategoryTargets_CategoryId",
                table: "CategoryTargets");

            migrationBuilder.DropIndex(
                name: "IX_CategoryTargets_Year_CategoryId",
                table: "CategoryTargets");

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

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "SystemSettings",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Projects",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDate",
                table: "Projects",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "Projects",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Projects",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AddColumn<int>(
                name: "CategoryId1",
                table: "Projects",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LeadChannelId1",
                table: "Projects",
                type: "integer",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "LeadChannels",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "CategoryTargets",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "CategoryTargets",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AddColumn<int>(
                name: "CategoryId1",
                table: "CategoryTargets",
                type: "integer",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "AspNetUsers",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1", "bd3e8890-0eda-4f5a-ae7a-090f48ae89a2", "Admin", "ADMIN" },
                    { "2", "b18fb81f-c318-402a-8ffd-c99741da77b8", "Manager", "MANAGER" },
                    { "3", "8ed8d122-aef8-4d23-b774-018d43c31945", "User", "USER" }
                });

            migrationBuilder.UpdateData(
                table: "LeadChannels",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 5, 14, 26, 41, 575, DateTimeKind.Utc).AddTicks(9718));

            migrationBuilder.UpdateData(
                table: "LeadChannels",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 5, 14, 26, 41, 575, DateTimeKind.Utc).AddTicks(9721));

            migrationBuilder.UpdateData(
                table: "LeadChannels",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 5, 14, 26, 41, 575, DateTimeKind.Utc).AddTicks(9723));

            migrationBuilder.UpdateData(
                table: "LeadChannels",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 5, 14, 26, 41, 575, DateTimeKind.Utc).AddTicks(9724));

            migrationBuilder.UpdateData(
                table: "LeadChannels",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 5, 14, 26, 41, 575, DateTimeKind.Utc).AddTicks(9725));

            migrationBuilder.UpdateData(
                table: "SystemSettings",
                keyColumn: "Id",
                keyValue: 1,
                column: "UpdatedAt",
                value: new DateTime(2026, 2, 5, 14, 26, 41, 575, DateTimeKind.Utc).AddTicks(9754));

            migrationBuilder.CreateIndex(
                name: "IX_Projects_CategoryId1",
                table: "Projects",
                column: "CategoryId1");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_Date",
                table: "Projects",
                column: "Date");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_LeadChannelId1",
                table: "Projects",
                column: "LeadChannelId1");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_Status",
                table: "Projects",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryTargets_CategoryId_Year",
                table: "CategoryTargets",
                columns: new[] { "CategoryId", "Year" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CategoryTargets_CategoryId1",
                table: "CategoryTargets",
                column: "CategoryId1");

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryTargets_Categories_CategoryId1",
                table: "CategoryTargets",
                column: "CategoryId1",
                principalTable: "Categories",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Categories_CategoryId",
                table: "Projects",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Categories_CategoryId1",
                table: "Projects",
                column: "CategoryId1",
                principalTable: "Categories",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_LeadChannels_LeadChannelId",
                table: "Projects",
                column: "LeadChannelId",
                principalTable: "LeadChannels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_LeadChannels_LeadChannelId1",
                table: "Projects",
                column: "LeadChannelId1",
                principalTable: "LeadChannels",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoryTargets_Categories_CategoryId1",
                table: "CategoryTargets");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Categories_CategoryId",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Categories_CategoryId1",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_LeadChannels_LeadChannelId",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_LeadChannels_LeadChannelId1",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_CategoryId1",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_Date",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_LeadChannelId1",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_Status",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_CategoryTargets_CategoryId_Year",
                table: "CategoryTargets");

            migrationBuilder.DropIndex(
                name: "IX_CategoryTargets_CategoryId1",
                table: "CategoryTargets");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3");

            migrationBuilder.DropColumn(
                name: "CategoryId1",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "LeadChannelId1",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "CategoryId1",
                table: "CategoryTargets");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "SystemSettings",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Projects",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDate",
                table: "Projects",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "Projects",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Projects",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "LeadChannels",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "CategoryTargets",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "CategoryTargets",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "AspNetUsers",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

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

            migrationBuilder.CreateIndex(
                name: "IX_Projects_Date_Status",
                table: "Projects",
                columns: new[] { "Date", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_CategoryTargets_CategoryId",
                table: "CategoryTargets",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryTargets_Year_CategoryId",
                table: "CategoryTargets",
                columns: new[] { "Year", "CategoryId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Categories_CategoryId",
                table: "Projects",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_LeadChannels_LeadChannelId",
                table: "Projects",
                column: "LeadChannelId",
                principalTable: "LeadChannels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
