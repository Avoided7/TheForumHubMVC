using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheForumHubMVC.Migrations
{
    public partial class AddedViews : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsersRating_AspNetUsers_UserId",
                table: "UsersRating");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UsersRating",
                table: "UsersRating");

            migrationBuilder.RenameTable(
                name: "UsersRating",
                newName: "UserRating");

            migrationBuilder.RenameIndex(
                name: "IX_UsersRating_UserId",
                table: "UserRating",
                newName: "IX_UserRating_UserId");

            migrationBuilder.AddColumn<int>(
                name: "Views",
                table: "Questions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserRating",
                table: "UserRating",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserRating_AspNetUsers_UserId",
                table: "UserRating",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserRating_AspNetUsers_UserId",
                table: "UserRating");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserRating",
                table: "UserRating");

            migrationBuilder.DropColumn(
                name: "Views",
                table: "Questions");

            migrationBuilder.RenameTable(
                name: "UserRating",
                newName: "UsersRating");

            migrationBuilder.RenameIndex(
                name: "IX_UserRating_UserId",
                table: "UsersRating",
                newName: "IX_UsersRating_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UsersRating",
                table: "UsersRating",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UsersRating_AspNetUsers_UserId",
                table: "UsersRating",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
