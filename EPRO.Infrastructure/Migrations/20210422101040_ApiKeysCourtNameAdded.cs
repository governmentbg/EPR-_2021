using Microsoft.EntityFrameworkCore.Migrations;

namespace EPRO.Infrastructure.Migrations
{
    public partial class ApiKeysCourtNameAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "court_name",
                table: "api_keys",
                maxLength: 200,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "court_name",
                table: "api_keys");
        }
    }
}
