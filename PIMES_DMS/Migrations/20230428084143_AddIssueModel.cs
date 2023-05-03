using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PIMES_DMS.Migrations
{
    /// <inheritdoc />
    public partial class AddIssueModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IssueDb",
                columns: table => new
                {
                    IssueID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IssueNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateFound = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Product = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SerialNo = table.Column<int>(type: "int", nullable: false),
                    AffectedPN = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Desc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProbDesc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Acknowledged = table.Column<bool>(type: "bit", nullable: false),
                    ValidatedStatus = table.Column<bool>(type: "bit", nullable: false),
                    ValidationRepSum = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CoD = table.Column<bool>(type: "bit", nullable: false),
                    Report = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    ControlNumber = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IssueDb", x => x.IssueID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IssueDb");
        }
    }
}
