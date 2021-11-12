using Microsoft.EntityFrameworkCore.Migrations;

namespace social_network_otus.Data.Migrations
{
    public partial class Add_UserName_UserLastName_Index_To_ApplicationUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_UserName_UserLastName",
                table: "AspNetUsers",
                columns: new[] { "UserName", "UserLastName" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_UserName_UserLastName",
                table: "AspNetUsers");
        }
    }
}
