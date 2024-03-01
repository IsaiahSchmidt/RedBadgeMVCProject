using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace musicProject.Data.Migrations
{
    /// <inheritdoc />
    public partial class seedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Artists",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "TEST" });

            migrationBuilder.InsertData(
                table: "Albums",
                columns: new[] { "Id", "ArtistId", "Genre", "Released", "Title" },
                values: new object[] { 1, 1, "rock", new DateTime(2024, 2, 29, 21, 2, 27, 936, DateTimeKind.Local).AddTicks(8552), "TESTAlbum" });

            migrationBuilder.InsertData(
                table: "BaseReviews",
                columns: new[] { "Id", "AlbumId", "Content", "Discriminator", "Rating", "UserId" },
                values: new object[] { 1, 1, "bjkvlyg", "AlbumReview", 5.0, null });

            migrationBuilder.InsertData(
                table: "Tracks",
                columns: new[] { "Id", "AlbumId", "ArtistId", "Released", "Title", "TrackNumber" },
                values: new object[,]
                {
                    { 1, 1, 1, new DateTime(2024, 2, 29, 21, 2, 27, 936, DateTimeKind.Local).AddTicks(8614), "testTitle", 1 },
                    { 2, 1, 1, new DateTime(2024, 2, 29, 21, 2, 27, 936, DateTimeKind.Local).AddTicks(8618), "testTitle2", 2 }
                });

            migrationBuilder.InsertData(
                table: "BaseReviews",
                columns: new[] { "Id", "Content", "Discriminator", "Rating", "TrackId", "UserId" },
                values: new object[] { 2, "kjbguyvl", "TrackReview", 4.0, 1, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "BaseReviews",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "BaseReviews",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Tracks",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Tracks",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Albums",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Artists",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
