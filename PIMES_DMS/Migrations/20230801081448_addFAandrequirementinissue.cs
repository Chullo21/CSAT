using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PIMES_DMS.Migrations
{
    /// <inheritdoc />
    public partial class addFAandrequirementinissue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateReceived",
                table: "RMADb",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "FA",
                table: "RMADb",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<string>(
                name: "Requirement",
                table: "IssueDb",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateReceived",
                table: "RMADb");

            migrationBuilder.DropColumn(
                name: "FA",
                table: "RMADb");

            migrationBuilder.DropColumn(
                name: "Requirement",
                table: "IssueDb");
        }
    }
}
