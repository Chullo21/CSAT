using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PIMES_DMS.Migrations
{
    /// <inheritdoc />
    public partial class addneedRMA : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ForceClose",
                table: "IssueDb");

            migrationBuilder.AddColumn<string>(
                name: "NeedRMA",
                table: "IssueDb",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NeedRMA",
                table: "IssueDb");

            migrationBuilder.AddColumn<bool>(
                name: "ForceClose",
                table: "IssueDb",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
