using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PIMES_DMS.Migrations
{
    /// <inheritdoc />
    public partial class reverttonotcecsc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ECDb");

            migrationBuilder.DropTable(
                name: "ECVerDb");

            migrationBuilder.DropTable(
                name: "SCDb");

            migrationBuilder.DropTable(
                name: "SCVerDb");

            migrationBuilder.DropTable(
                name: "TCDb");

            migrationBuilder.DropTable(
                name: "TCVerDb");

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "ActionDb",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "ActionDb");

            migrationBuilder.CreateTable(
                name: "ECDb",
                columns: table => new
                {
                    ECAID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Action = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ActionStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ControlNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HasVer = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    PIC = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TESID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TargetDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ECDb", x => x.ECAID);
                });

            migrationBuilder.CreateTable(
                name: "ECVerDb",
                columns: table => new
                {
                    VerID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActionID = table.Column<int>(type: "int", nullable: false),
                    ControlNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateClosed = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateVer = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Files = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Result = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Verificator = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ECVerDb", x => x.VerID);
                });

            migrationBuilder.CreateTable(
                name: "SCDb",
                columns: table => new
                {
                    SCAID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Action = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ActionStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ControlNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HasVer = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    PIC = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TESID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TargetDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SCDb", x => x.SCAID);
                });

            migrationBuilder.CreateTable(
                name: "SCVerDb",
                columns: table => new
                {
                    VerID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActionID = table.Column<int>(type: "int", nullable: false),
                    ControlNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateClosed = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateVer = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Files = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Result = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Verificator = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SCVerDb", x => x.VerID);
                });

            migrationBuilder.CreateTable(
                name: "TCDb",
                columns: table => new
                {
                    TCAID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Action = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ActionStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ControlNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HasVer = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    PIC = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TESID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TargetDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TCDb", x => x.TCAID);
                });

            migrationBuilder.CreateTable(
                name: "TCVerDb",
                columns: table => new
                {
                    VerID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActionID = table.Column<int>(type: "int", nullable: false),
                    ControlNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateClosed = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateVer = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Files = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Result = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Verificator = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TCVerDb", x => x.VerID);
                });
        }
    }
}
