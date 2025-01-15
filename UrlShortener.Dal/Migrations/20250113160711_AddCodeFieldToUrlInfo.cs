using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UrlShortener.Dal.Migrations
{
    /// <inheritdoc />
    public partial class AddCodeFieldToUrlInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "UrlInfos");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "UrlInfos",
                type: "nvarchar(7)",
                maxLength: 7,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_UrlInfos_Code",
                table: "UrlInfos",
                column: "Code",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UrlInfos_Code",
                table: "UrlInfos");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "UrlInfos");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "UrlInfos",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
