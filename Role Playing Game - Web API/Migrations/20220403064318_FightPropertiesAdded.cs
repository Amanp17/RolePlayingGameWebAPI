using Microsoft.EntityFrameworkCore.Migrations;

namespace RolePlayingGameWebAPI.Migrations
{
    public partial class FightPropertiesAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Defeat",
                table: "Characters",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Fight",
                table: "Characters",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Victories",
                table: "Characters",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Defeat",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Fight",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Victories",
                table: "Characters");
        }
    }
}
