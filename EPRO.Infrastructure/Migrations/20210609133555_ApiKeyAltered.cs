using Microsoft.EntityFrameworkCore.Migrations;

namespace EPRO.Infrastructure.Migrations
{
    public partial class ApiKeyAltered : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "app_secrtet",
                table: "api_keys");

            migrationBuilder.DropColumn(
                name: "court_code",
                table: "api_keys");

            migrationBuilder.DropColumn(
                name: "court_name",
                table: "api_keys");

            migrationBuilder.AddColumn<string>(
                name: "app_secret",
                table: "api_keys",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "court_id",
                table: "api_keys",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "ix_api_keys_court_id",
                table: "api_keys",
                column: "court_id");

            migrationBuilder.AddForeignKey(
                name: "fk_api_keys_nom_court_court_id",
                table: "api_keys",
                column: "court_id",
                principalTable: "nom_court",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_api_keys_nom_court_court_id",
                table: "api_keys");

            migrationBuilder.DropIndex(
                name: "ix_api_keys_court_id",
                table: "api_keys");

            migrationBuilder.DropColumn(
                name: "app_secret",
                table: "api_keys");

            migrationBuilder.DropColumn(
                name: "court_id",
                table: "api_keys");

            migrationBuilder.AddColumn<string>(
                name: "app_secrtet",
                table: "api_keys",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "court_code",
                table: "api_keys",
                type: "character varying(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "court_name",
                table: "api_keys",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");
        }
    }
}
