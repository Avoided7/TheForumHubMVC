using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheForumHubMVC.Migrations
{
    public partial class AddedRatingSystem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rating",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "Rating",
                table: "Answers");

            migrationBuilder.CreateTable(
                name: "AnswerRating",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AnswerId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnswerRating", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnswerRating_Answers_AnswerId",
                        column: x => x.AnswerId,
                        principalTable: "Answers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AnswerRating_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuestionRating",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    QuestionId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionRating", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestionRating_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuestionRating_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserRating",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ToUser = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRating", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRating_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnswerRating_AnswerId",
                table: "AnswerRating",
                column: "AnswerId");

            migrationBuilder.CreateIndex(
                name: "IX_AnswerRating_UserId",
                table: "AnswerRating",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionRating_QuestionId",
                table: "QuestionRating",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionRating_UserId",
                table: "QuestionRating",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRating_UserId",
                table: "UserRating",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnswerRating");

            migrationBuilder.DropTable(
                name: "QuestionRating");

            migrationBuilder.DropTable(
                name: "UserRating");

            migrationBuilder.AddColumn<int>(
                name: "Rating",
                table: "Questions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Rating",
                table: "Answers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
