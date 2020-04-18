using Microsoft.EntityFrameworkCore.Migrations;

namespace NMS.Saga.Sample.Core.Migrations
{
    public partial class DisableAutoGenId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Coding",
                columns: table => new
                {
                    IdCoding = table.Column<int>(nullable: false),
                    Title = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coding", x => x.IdCoding);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Coding");
        }
    }
}
