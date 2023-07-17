using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PIMES_DMS.Migrations
{
    /// <inheritdoc />
    public partial class reset : Migration
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
                    BU = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ControlNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TESID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TargetDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    HasVer = table.Column<bool>(type: "bit", nullable: false),
                    ActionStatus = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActionDb", x => x.ActionID);
                });

            migrationBuilder.CreateTable(
                name: "ART_8D",
                columns: table => new
                {
                    ARTID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ControlNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateValidated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateClosed = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ART_8D", x => x.ARTID);
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
                    DateFound = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Product = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SerialNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AffectedPN = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Qty = table.Column<int>(type: "int", nullable: false),
                    Desc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProbDesc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClientRep = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    ValNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateAck = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ValidatedStatus = table.Column<bool>(type: "bit", nullable: false),
                    ValidationRepSum = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CoD = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ValRes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Report = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    ControlNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateVdal = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ForceClose = table.Column<bool>(type: "bit", nullable: false),
                    Acknowledged = table.Column<bool>(type: "bit", nullable: false),
                    HasTES = table.Column<bool>(type: "bit", nullable: false),
                    HasCR = table.Column<bool>(type: "bit", nullable: false),
                    isDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IssueDb", x => x.IssueID);
                });

            migrationBuilder.CreateTable(
                name: "SEDb",
                columns: table => new
                {
                    SMTPID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Port = table.Column<int>(type: "int", nullable: false),
                    SmtpServer = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SEDb", x => x.SMTPID);
                });

            migrationBuilder.CreateTable(
                name: "TCDb",
                columns: table => new
                {
                    TCAID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Action = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PIC = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ControlNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TESID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TargetDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    HasVer = table.Column<bool>(type: "bit", nullable: false),
                    ActionStatus = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TCDb", x => x.TCAID);
                });

            migrationBuilder.CreateTable(
                name: "TESDb",
                columns: table => new
                {
                    TESID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ControlNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TCWhy1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TCWhy2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TCWhy3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TCWhy4 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TRC = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ECWhy1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ECWhy2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ECWhy3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ECWhy4 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ERC = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SCWhy1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SCWhy2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SCWhy3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SCWhy4 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SRC = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TESDb", x => x.TESID);
                });

            migrationBuilder.CreateTable(
                name: "VerDb",
                columns: table => new
                {
                    VerID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActionID = table.Column<int>(type: "int", nullable: false),
                    ControlNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Files = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    DateVer = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateClosed = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Result = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsVer = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VerDb", x => x.VerID);
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
                name: "ART_8D");

            migrationBuilder.DropTable(
                name: "ERDb");

            migrationBuilder.DropTable(
                name: "IssueDb");

            migrationBuilder.DropTable(
                name: "SEDb");

            migrationBuilder.DropTable(
                name: "TCDb");

            migrationBuilder.DropTable(
                name: "TESDb");

            migrationBuilder.DropTable(
                name: "VerDb");
        }
    }
}
