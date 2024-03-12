using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace musicProject.Data.Migrations
{
    /// <inheritdoc />
    public partial class deleteAlbumReviewIssue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Albums",
                keyColumn: "Id",
                keyValue: 1,
                column: "Released",
                value: new DateTime(2024, 3, 11, 20, 7, 46, 665, DateTimeKind.Local).AddTicks(3665));

            migrationBuilder.UpdateData(
                table: "Tracks",
                keyColumn: "Id",
                keyValue: 1,
                column: "Released",
                value: new DateTime(2024, 3, 11, 20, 7, 46, 665, DateTimeKind.Local).AddTicks(3719));

            migrationBuilder.UpdateData(
                table: "Tracks",
                keyColumn: "Id",
                keyValue: 2,
                column: "Released",
                value: new DateTime(2024, 3, 11, 20, 7, 46, 665, DateTimeKind.Local).AddTicks(3722));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Albums",
                keyColumn: "Id",
                keyValue: 1,
                column: "Released",
                value: new DateTime(2024, 3, 7, 20, 19, 57, 166, DateTimeKind.Local).AddTicks(3315));

            migrationBuilder.UpdateData(
                table: "Tracks",
                keyColumn: "Id",
                keyValue: 1,
                column: "Released",
                value: new DateTime(2024, 3, 7, 20, 19, 57, 166, DateTimeKind.Local).AddTicks(3374));

            migrationBuilder.UpdateData(
                table: "Tracks",
                keyColumn: "Id",
                keyValue: 2,
                column: "Released",
                value: new DateTime(2024, 3, 7, 20, 19, 57, 166, DateTimeKind.Local).AddTicks(3378));
        }
    }
}
