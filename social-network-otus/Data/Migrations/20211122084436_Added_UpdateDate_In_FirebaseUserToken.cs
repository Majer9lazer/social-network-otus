using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace social_network_otus.Data.Migrations
{
    public partial class Added_UpdateDate_In_FirebaseUserToken : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Token",
                table: "FirebaseUserTokens",
                type: "varchar(767)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateDate",
                table: "FirebaseUserTokens",
                type: "datetime",
                nullable: true,
                defaultValueSql: "CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP");

            migrationBuilder.CreateIndex(
                name: "IX_FirebaseUserTokens_Token",
                table: "FirebaseUserTokens",
                column: "Token",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FirebaseUserTokens_Token",
                table: "FirebaseUserTokens");

            migrationBuilder.DropColumn(
                name: "UpdateDate",
                table: "FirebaseUserTokens");

            migrationBuilder.AlterColumn<string>(
                name: "Token",
                table: "FirebaseUserTokens",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(767)",
                oldNullable: true);
        }
    }
}
