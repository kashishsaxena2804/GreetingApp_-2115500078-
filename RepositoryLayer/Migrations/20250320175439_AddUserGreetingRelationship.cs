using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RepositoryLayer.Migrations
{
    public partial class AddUserGreetingRelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Greetings");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "Greetings");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Greetings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Greetings_UserId",
                table: "Greetings",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Greetings_Users_UserId",
                table: "Greetings",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Greetings_Users_UserId",
                table: "Greetings");

            migrationBuilder.DropIndex(
                name: "IX_Greetings_UserId",
                table: "Greetings");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Greetings");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Greetings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Greetings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
