using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace VideoGameStore.Data.Migrations
{
    public partial class AddingPrimaryKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Game",
                columns: table => new
                {
                    gameGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    gameTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    gameDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    gameReleaseYear = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Game", x => x.gameGuid);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Game");
        }
    }
}
