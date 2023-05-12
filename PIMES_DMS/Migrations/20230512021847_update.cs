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
                name: "AccountsDb",
                columns: table => new
                {
                    AccID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccUCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AccName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    isDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountsDb", x => x.AccID);
                });

            migrationBuilder.CreateTable(
                name: "ActionDb",
                columns: table => new
                {
                    ActionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Action = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PIC = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: true),
                    Files = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ControlNo = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActionDb", x => x.ActionID);
                });

            migrationBuilder.CreateTable(
                name: "ERDb",
                columns: table => new
                {
                    ERID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ControlNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WHSESOH = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WHSEGOOD = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WHSENOGOOD = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WHSEDis = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IQASOH = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IQAGOOD = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IQANOGOOD = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IQADis = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WIPSOH = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WIPGOOD = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WIPNOGOOD = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WIPDis = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FGSOH = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FGGOOD = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FGNOGOOD = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FGDis = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Rep = table.Column<bool>(type: "bit", nullable: false),
                    RMAno = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ERDb", x => x.ERID);
                });

            migrationBuilder.CreateTable(
                name: "IssueDb",
                columns: table => new
                {
                    IssueID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IssueCreator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IssueNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateFound = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Product = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SerialNo = table.Column<int>(type: "int", nullable: false),
                    AffectedPN = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Desc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProbDesc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClientRep = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    Acknowledged = table.Column<bool>(type: "bit", nullable: false),
                    ValidatedStatus = table.Column<bool>(type: "bit", nullable: false),
                    ValidationRepSum = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CoD = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Report = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    ControlNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateAck = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateVdal = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HasCR = table.Column<bool>(type: "bit", nullable: false),
                    isDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Qty = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IssueDb", x => x.IssueID);
                });

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountsDb");

            migrationBuilder.DropTable(
                name: "ActionDb");

            migrationBuilder.DropTable(
                name: "ERDb");

            migrationBuilder.DropTable(
                name: "IssueDb");

            migrationBuilder.DropTable(
                name: "SRDb");
        }
    }
}
