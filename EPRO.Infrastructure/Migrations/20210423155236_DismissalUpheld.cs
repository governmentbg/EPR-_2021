using Microsoft.EntityFrameworkCore.Migrations;

namespace EPRO.Infrastructure.Migrations
{
    public partial class DismissalUpheld : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "replacejudge_is_chairman",
                table: "dismissal",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true);

            migrationBuilder.DropColumn(
               name: "objection_upheld",
               table: "dismissal");

            migrationBuilder.AddColumn<bool>(
               name: "objection_upheld",
               table: "dismissal",
               nullable: true);
           
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "replacejudge_is_chairman",
                table: "dismissal",
                type: "boolean",
                nullable: true,
                oldClrType: typeof(bool));

            migrationBuilder.DropColumn(
               name: "objection_upheld",
               table: "dismissal");

            migrationBuilder.AddColumn<string>(
               name: "objection_upheld",
               table: "dismissal",
               nullable: true);
        }
    }
}
