using Microsoft.EntityFrameworkCore.Migrations;

namespace EPRO.Infrastructure.Migrations
{
    public partial class ApiKeysUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "key",
                table: "api_keys");

            migrationBuilder.AddColumn<string>(
                name: "app_key",
                table: "api_keys",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "app_secrtet",
                table: "api_keys",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "app_key",
                table: "api_keys");

            migrationBuilder.DropColumn(
                name: "app_secrtet",
                table: "api_keys");

            migrationBuilder.AddColumn<string>(
                name: "key",
                table: "api_keys",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }
    }
}
