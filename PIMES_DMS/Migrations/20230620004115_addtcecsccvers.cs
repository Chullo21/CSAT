using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PIMES_DMS.Migrations
{
    /// <inheritdoc />
    public partial class addtcecsccvers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ECVerDb",
                columns: table => new
                {
                    VerID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Verificator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ActionID = table.Column<int>(type: "int", nullable: false),
                    ControlNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Files = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    DateVer = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateClosed = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Result = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ECVerDb", x => x.VerID);
                });

            migrationBuilder.CreateTable(
                name: "SCVerDb",
                columns: table => new
                {
                    VerID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Verificator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ActionID = table.Column<int>(type: "int", nullable: false),
                    ControlNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Files = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    DateVer = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateClosed = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Result = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SCVerDb", x => x.VerID);
                });

            migrationBuilder.CreateTable(
                name: "TCVerDb",
                columns: table => new
                {
                    VerID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Verificator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ActionID = table.Column<int>(type: "int", nullable: false),
                    ControlNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Files = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    DateVer = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateClosed = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Result = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TCVerDb", x => x.VerID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ECVerDb");

            migrationBuilder.DropTable(
                name: "SCVerDb");

            migrationBuilder.DropTable(
                name: "TCVerDb");
        }
    }
}
