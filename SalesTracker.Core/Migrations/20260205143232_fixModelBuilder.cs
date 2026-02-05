using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SalesTracker.Core.Migrations
{
    /// <inheritdoc />
    public partial class fixModelBuilder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoryTargets_Categories_CategoryId1",
                table: "CategoryTargets");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Categories_CategoryId1",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_LeadChannels_LeadChannelId1",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_CategoryId1",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_LeadChannelId1",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_CategoryTargets_CategoryId1",
                table: "CategoryTargets");

            migrationBuilder.DropColumn(
                name: "CategoryId1",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "LeadChannelId1",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "CategoryId1",
                table: "CategoryTargets");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.AddColumn<int>(
                name: "CategoryId1",
                table: "CategoryTargets",
                type: "integer",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "4aaf6859-03df-4c50-b2c4-32f623abd1fd");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2",
                column: "ConcurrencyStamp",
                value: "b3db9336-bd24-4926-ae61-d97156164f48");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3",
                column: "ConcurrencyStamp",
                value: "9bd130ea-d227-41ba-9b29-29977dc33dbe");

            migrationBuilder.UpdateData(
                table: "LeadChannels",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 5, 14, 29, 28, 449, DateTimeKind.Utc).AddTicks(460));

            migrationBuilder.UpdateData(
                table: "LeadChannels",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 5, 14, 29, 28, 449, DateTimeKind.Utc).AddTicks(463));

            migrationBuilder.UpdateData(
                table: "LeadChannels",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 5, 14, 29, 28, 449, DateTimeKind.Utc).AddTicks(464));

            migrationBuilder.UpdateData(
                table: "LeadChannels",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 5, 14, 29, 28, 449, DateTimeKind.Utc).AddTicks(465));

            migrationBuilder.UpdateData(
                table: "LeadChannels",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 5, 14, 29, 28, 449, DateTimeKind.Utc).AddTicks(466));

            migrationBuilder.UpdateData(
                table: "SystemSettings",
                keyColumn: "Id",
                keyValue: 1,
                column: "UpdatedAt",
                value: new DateTime(2026, 2, 5, 14, 29, 28, 449, DateTimeKind.Utc).AddTicks(489));

            migrationBuilder.CreateIndex(
                name: "IX_Projects_CategoryId1",
                table: "Projects",
                column: "CategoryId1");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_LeadChannelId1",
                table: "Projects",
                column: "LeadChannelId1");

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
                name: "FK_Projects_Categories_CategoryId1",
                table: "Projects",
                column: "CategoryId1",
                principalTable: "Categories",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_LeadChannels_LeadChannelId1",
                table: "Projects",
                column: "LeadChannelId1",
                principalTable: "LeadChannels",
                principalColumn: "Id");
        }
    }
}
