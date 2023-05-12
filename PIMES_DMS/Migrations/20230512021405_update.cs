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
            migrationBuilder.DropForeignKey(
                name: "FK_ActionModel_SRDb_SumRepModelSumRepID",
                table: "ActionModel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ActionModel",
                table: "ActionModel");

            migrationBuilder.DropIndex(
                name: "IX_ActionModel_SumRepModelSumRepID",
                table: "ActionModel");

            migrationBuilder.DropColumn(
                name: "SumRepModelSumRepID",
                table: "ActionModel");

            migrationBuilder.RenameTable(
                name: "ActionModel",
                newName: "ActionDb");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ActionDb",
                table: "ActionDb",
                column: "ActionID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ActionDb",
                table: "ActionDb");

            migrationBuilder.RenameTable(
                name: "ActionDb",
                newName: "ActionModel");

            migrationBuilder.AddColumn<int>(
                name: "SumRepModelSumRepID",
                table: "ActionModel",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ActionModel",
                table: "ActionModel",
                column: "ActionID");

            migrationBuilder.CreateIndex(
                name: "IX_ActionModel_SumRepModelSumRepID",
                table: "ActionModel",
                column: "SumRepModelSumRepID");

            migrationBuilder.AddForeignKey(
                name: "FK_ActionModel_SRDb_SumRepModelSumRepID",
                table: "ActionModel",
                column: "SumRepModelSumRepID",
                principalTable: "SRDb",
                principalColumn: "SumRepID");
        }
    }
}
