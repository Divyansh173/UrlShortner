using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace URLShortner.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UrlShorteners",
                columns: table => new
                {
                    ShortUrl = table.Column<string>(type: "nvarchar(7)", maxLength: 7, nullable: false),
                    FullUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UrlShorteners", x => x.ShortUrl);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UrlShorteners");
        }
    }
}
