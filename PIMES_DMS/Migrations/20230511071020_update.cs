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
            migrationBuilder.CreateTable(
                name: "SRDb",
                columns: table => new
                {
                    SumRepID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DefCat = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateFound = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IssueNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Product = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AffectedQty = table.Column<int>(type: "int", nullable: false),
                    ProblemDesc = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SRDb", x => x.SumRepID);
                });

            migrationBuilder.CreateTable(
                name: "ActionModel",
                columns: table => new
                {
                    ActionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Action = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PIC = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: true),
                    Files = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SumRepModelSumRepID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActionModel", x => x.ActionID);
                    table.ForeignKey(
                        name: "FK_ActionModel_SRDb_SumRepModelSumRepID",
                        column: x => x.SumRepModelSumRepID,
                        principalTable: "SRDb",
                        principalColumn: "SumRepID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActionModel_SumRepModelSumRepID",
                table: "ActionModel",
                column: "SumRepModelSumRepID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActionModel");

            migrationBuilder.DropTable(
                name: "SRDb");
        }
    }
}
