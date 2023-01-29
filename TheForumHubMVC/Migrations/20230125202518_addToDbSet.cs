using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheForumHubMVC.Migrations
{
    public partial class addToDbSet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnswerRating_Answers_AnswerId",
                table: "AnswerRating");

            migrationBuilder.DropForeignKey(
                name: "FK_AnswerRating_AspNetUsers_UserId",
                table: "AnswerRating");

            migrationBuilder.DropForeignKey(
                name: "FK_QuestionRating_AspNetUsers_UserId",
                table: "QuestionRating");

            migrationBuilder.DropForeignKey(
                name: "FK_QuestionRating_Questions_QuestionId",
                table: "QuestionRating");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRating_AspNetUsers_UserId",
                table: "UserRating");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserRating",
                table: "UserRating");

            migrationBuilder.DropPrimaryKey(
                name: "PK_QuestionRating",
                table: "QuestionRating");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AnswerRating",
                table: "AnswerRating");

            migrationBuilder.RenameTable(
                name: "UserRating",
                newName: "UsersRating");

            migrationBuilder.RenameTable(
                name: "QuestionRating",
                newName: "QuestionsRating");

            migrationBuilder.RenameTable(
                name: "AnswerRating",
                newName: "AnswersRating");

            migrationBuilder.RenameIndex(
                name: "IX_UserRating_UserId",
                table: "UsersRating",
                newName: "IX_UsersRating_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_QuestionRating_UserId",
                table: "QuestionsRating",
                newName: "IX_QuestionsRating_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_QuestionRating_QuestionId",
                table: "QuestionsRating",
                newName: "IX_QuestionsRating_QuestionId");

            migrationBuilder.RenameIndex(
                name: "IX_AnswerRating_UserId",
                table: "AnswersRating",
                newName: "IX_AnswersRating_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_AnswerRating_AnswerId",
                table: "AnswersRating",
                newName: "IX_AnswersRating_AnswerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UsersRating",
                table: "UsersRating",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_QuestionsRating",
                table: "QuestionsRating",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AnswersRating",
                table: "AnswersRating",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AnswersRating_Answers_AnswerId",
                table: "AnswersRating",
                column: "AnswerId",
                principalTable: "Answers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AnswersRating_AspNetUsers_UserId",
                table: "AnswersRating",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionsRating_AspNetUsers_UserId",
                table: "QuestionsRating",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionsRating_Questions_QuestionId",
                table: "QuestionsRating",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UsersRating_AspNetUsers_UserId",
                table: "UsersRating",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnswersRating_Answers_AnswerId",
                table: "AnswersRating");

            migrationBuilder.DropForeignKey(
                name: "FK_AnswersRating_AspNetUsers_UserId",
                table: "AnswersRating");

            migrationBuilder.DropForeignKey(
                name: "FK_QuestionsRating_AspNetUsers_UserId",
                table: "QuestionsRating");

            migrationBuilder.DropForeignKey(
                name: "FK_QuestionsRating_Questions_QuestionId",
                table: "QuestionsRating");

            migrationBuilder.DropForeignKey(
                name: "FK_UsersRating_AspNetUsers_UserId",
                table: "UsersRating");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UsersRating",
                table: "UsersRating");

            migrationBuilder.DropPrimaryKey(
                name: "PK_QuestionsRating",
                table: "QuestionsRating");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AnswersRating",
                table: "AnswersRating");

            migrationBuilder.RenameTable(
                name: "UsersRating",
                newName: "UserRating");

            migrationBuilder.RenameTable(
                name: "QuestionsRating",
                newName: "QuestionRating");

            migrationBuilder.RenameTable(
                name: "AnswersRating",
                newName: "AnswerRating");

            migrationBuilder.RenameIndex(
                name: "IX_UsersRating_UserId",
                table: "UserRating",
                newName: "IX_UserRating_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_QuestionsRating_UserId",
                table: "QuestionRating",
                newName: "IX_QuestionRating_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_QuestionsRating_QuestionId",
                table: "QuestionRating",
                newName: "IX_QuestionRating_QuestionId");

            migrationBuilder.RenameIndex(
                name: "IX_AnswersRating_UserId",
                table: "AnswerRating",
                newName: "IX_AnswerRating_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_AnswersRating_AnswerId",
                table: "AnswerRating",
                newName: "IX_AnswerRating_AnswerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserRating",
                table: "UserRating",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_QuestionRating",
                table: "QuestionRating",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AnswerRating",
                table: "AnswerRating",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AnswerRating_Answers_AnswerId",
                table: "AnswerRating",
                column: "AnswerId",
                principalTable: "Answers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AnswerRating_AspNetUsers_UserId",
                table: "AnswerRating",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionRating_AspNetUsers_UserId",
                table: "QuestionRating",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionRating_Questions_QuestionId",
                table: "QuestionRating",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserRating_AspNetUsers_UserId",
                table: "UserRating",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
