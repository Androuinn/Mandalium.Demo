using Microsoft.EntityFrameworkCore.Migrations;

namespace Mandalium.Core.Migrations
{
    public partial class CommentsAdded2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PublishStatus",
                table: "Comments",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PublishStatus",
                table: "Comments");
        }
    }
}
