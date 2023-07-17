using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PIMES_DMS.Migrations
{
    /// <inheritdoc />
    public partial class customizevermodel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsVer",
                table: "VerDb");

            migrationBuilder.AddColumn<string>(
                name: "RCType",
                table: "VerDb",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RCType",
                table: "VerDb");

            migrationBuilder.AddColumn<bool>(
                name: "IsVer",
                table: "VerDb",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
