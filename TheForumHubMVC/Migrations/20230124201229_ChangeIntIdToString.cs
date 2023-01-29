using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheForumHubMVC.Migrations
{
    public partial class ChangeIntIdToString : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Answers_AspNetUsers_UserId1",
                table: "Answers");

            migrationBuilder.DropForeignKey(
                name: "FK_Questions_AspNetUsers_UserId1",
                table: "Questions");

            migrationBuilder.DropIndex(
                name: "IX_Questions_UserId1",
                table: "Questions");

            migrationBuilder.DropIndex(
                name: "IX_Answers_UserId1",
                table: "Answers");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "Answers");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Questions",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Answers",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Questions_UserId",
                table: "Questions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Answers_UserId",
                table: "Answers",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Answers_AspNetUsers_UserId",
                table: "Answers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_AspNetUsers_UserId",
                table: "Questions",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Answers_AspNetUsers_UserId",
                table: "Answers");

            migrationBuilder.DropForeignKey(
                name: "FK_Questions_AspNetUsers_UserId",
                table: "Questions");

            migrationBuilder.DropIndex(
                name: "IX_Questions_UserId",
                table: "Questions");

            migrationBuilder.DropIndex(
                name: "IX_Answers_UserId",
                table: "Answers");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Questions",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "Questions",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Answers",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "Answers",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_UserId1",
                table: "Questions",
                column: "UserId1");

            migrationBuilder.CreateIndex(
                name: "IX_Answers_UserId1",
                table: "Answers",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Answers_AspNetUsers_UserId1",
                table: "Answers",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_AspNetUsers_UserId1",
                table: "Questions",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
