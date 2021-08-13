using Microsoft.EntityFrameworkCore.Migrations;

namespace EPRO.Infrastructure.Migrations
{
    public partial class FullNamesAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "judge_full_name",
                table: "dismissal",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "replacejudge_full_name",
                table: "dismissal",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "side_full_name",
                table: "dismissal",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "judge_full_name",
                table: "dismissal");

            migrationBuilder.DropColumn(
                name: "replacejudge_full_name",
                table: "dismissal");

            migrationBuilder.DropColumn(
                name: "side_full_name",
                table: "dismissal");
        }
    }
}
