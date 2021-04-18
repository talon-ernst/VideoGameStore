using Microsoft.EntityFrameworkCore.Migrations;

namespace VideoGameStore.Data.Migrations
{
    public partial class AddingGamePrice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "gamePrice",
                table: "Game",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "gamePrice",
                table: "Game");
        }
    }
}
