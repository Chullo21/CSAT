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
                name: "ERDb",
                columns: table => new
                {
                    ERID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ControlNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WHSESOH = table.Column<int>(type: "int", nullable: false),
                    WHSEGOOD = table.Column<int>(type: "int", nullable: false),
                    WHSENOGOOD = table.Column<int>(type: "int", nullable: false),
                    WHSEDis = table.Column<int>(type: "int", nullable: false),
                    IQASOH = table.Column<int>(type: "int", nullable: false),
                    IQAGOOD = table.Column<int>(type: "int", nullable: false),
                    IQANOGOOD = table.Column<int>(type: "int", nullable: false),
                    IQADis = table.Column<int>(type: "int", nullable: false),
                    WIPSOH = table.Column<int>(type: "int", nullable: false),
                    WIPGOOD = table.Column<int>(type: "int", nullable: false),
                    WIPNOGOOD = table.Column<int>(type: "int", nullable: false),
                    WIPDis = table.Column<int>(type: "int", nullable: false),
                    FGSOH = table.Column<int>(type: "int", nullable: false),
                    FGGOOD = table.Column<int>(type: "int", nullable: false),
                    FGNOGOOD = table.Column<int>(type: "int", nullable: false),
                    FGDis = table.Column<int>(type: "int", nullable: false),
                    Rep = table.Column<bool>(type: "bit", nullable: false),
                    RMAno = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ERDb", x => x.ERID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ERDb");
        }
    }
}
