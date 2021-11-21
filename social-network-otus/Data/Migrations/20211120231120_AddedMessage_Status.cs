using Microsoft.EntityFrameworkCore.Migrations;

namespace social_network_otus.Data.Migrations
{
    public partial class AddedMessage_Status : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "ChatMessage",
                type: "varchar(9)",
                maxLength: 9,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "ChatMessage");
        }
    }
}
