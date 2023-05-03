using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PIMES_DMS.Migrations
{
    /// <inheritdoc />
    public partial class AddAccs : Migration
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
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountsDb", x => x.AccID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountsDb");
        }
    }
}
