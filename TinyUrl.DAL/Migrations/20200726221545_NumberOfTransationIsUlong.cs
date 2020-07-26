using Microsoft.EntityFrameworkCore.Migrations;

namespace TinyUrl.DAL.Migrations
{
    public partial class NumberOfTransationIsUlong : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "NumberOfTransitions",
                table: "Urls",
                type: "decimal(20,0)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "NumberOfTransitions",
                table: "Urls",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(20,0)");
        }
    }
}
