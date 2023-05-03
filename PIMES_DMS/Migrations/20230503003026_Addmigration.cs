using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PIMES_DMS.Migrations
{
    /// <inheritdoc />
    public partial class Addmigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "ClientRep",
                table: "IssueDb",
                type: "varbinary(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClientRep",
                table: "IssueDb");
        }
    }
}
