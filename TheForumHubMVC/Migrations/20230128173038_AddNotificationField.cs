using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheForumHubMVC.Migrations
{
    public partial class AddNotificationField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "OnNotification",
                table: "AspNetUsers",
                type: "bit",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OnNotification",
                table: "AspNetUsers");
        }
    }
}
