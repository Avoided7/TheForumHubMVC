using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheForumHubMVC.Migrations
{
    public partial class removeQuestionCountInTag : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QuestionsCount",
                table: "Tags");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "QuestionsCount",
                table: "Tags",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
