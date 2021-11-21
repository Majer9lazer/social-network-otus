using Microsoft.EntityFrameworkCore.Migrations;

namespace social_network_otus.Data.Migrations
{
    public partial class Added_FirebaseMessageToken_To_ApplicationUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FirebaseMessageToken",
                table: "AspNetUsers",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirebaseMessageToken",
                table: "AspNetUsers");
        }
    }
}
