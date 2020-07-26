using Microsoft.EntityFrameworkCore.Migrations;

namespace TinyUrl.DAL.Migrations
{
    public partial class MakeTinyPathIsUnique : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Urls_TinyPath",
                table: "Urls",
                column: "TinyPath",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Urls_TinyPath",
                table: "Urls");
        }
    }
}
