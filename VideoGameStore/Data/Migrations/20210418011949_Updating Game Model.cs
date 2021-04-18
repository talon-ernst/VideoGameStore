using Microsoft.EntityFrameworkCore.Migrations;

namespace VideoGameStore.Data.Migrations
{
    public partial class UpdatingGameModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "gameCategory",
                table: "Game",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "gameDeveloper",
                table: "Game",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "gamePublisher",
                table: "Game",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "gameCategory",
                table: "Game");

            migrationBuilder.DropColumn(
                name: "gameDeveloper",
                table: "Game");

            migrationBuilder.DropColumn(
                name: "gamePublisher",
                table: "Game");
        }
    }
}
