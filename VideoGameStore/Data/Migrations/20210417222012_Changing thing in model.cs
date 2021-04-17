using Microsoft.EntityFrameworkCore.Migrations;

namespace VideoGameStore.Data.Migrations
{
    public partial class Changingthinginmodel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "gameReleaseYear",
                table: "Game",
                newName: "gameReleaseDate");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "gameReleaseDate",
                table: "Game",
                newName: "gameReleaseYear");
        }
    }
}
