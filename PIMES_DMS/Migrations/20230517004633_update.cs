using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PIMES_DMS.Migrations
{
    /// <inheritdoc />
    public partial class update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateVer",
                table: "ActionDb");

            migrationBuilder.DropColumn(
                name: "Files",
                table: "ActionDb");

            migrationBuilder.DropColumn(
                name: "IsVer",
                table: "ActionDb");

            migrationBuilder.DropColumn(
                name: "Result",
                table: "ActionDb");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "ActionDb");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateVer",
                table: "ActionDb",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<byte[]>(
                name: "Files",
                table: "ActionDb",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsVer",
                table: "ActionDb",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Result",
                table: "ActionDb",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "ActionDb",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
