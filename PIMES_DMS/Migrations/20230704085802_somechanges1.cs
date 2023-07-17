using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PIMES_DMS.Migrations
{
    /// <inheritdoc />
    public partial class somechanges1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DateClosed",
                table: "VerDb",
                newName: "StatusDate");

            migrationBuilder.AddColumn<string>(
                name: "Dependency",
                table: "ActionDb",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Dependency",
                table: "ActionDb");

            migrationBuilder.RenameColumn(
                name: "StatusDate",
                table: "VerDb",
                newName: "DateClosed");
        }
    }
}
