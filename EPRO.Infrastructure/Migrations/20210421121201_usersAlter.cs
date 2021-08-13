using Microsoft.EntityFrameworkCore.Migrations;

namespace EPRO.Infrastructure.Migrations
{
    public partial class usersAlter : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_application_user_nom_court_court_id",
                table: "identity_users");

            migrationBuilder.AlterColumn<int>(
                name: "court_id",
                table: "identity_users",
                nullable: true,
                defaultValue: 1,
                oldClrType: typeof(int),
                oldType: "integer",
                oldDefaultValue: 1);

            migrationBuilder.AddForeignKey(
                name: "fk_application_user_nom_court_court_id",
                table: "identity_users",
                column: "court_id",
                principalTable: "nom_court",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_application_user_nom_court_court_id",
                table: "identity_users");

            migrationBuilder.AlterColumn<int>(
                name: "court_id",
                table: "identity_users",
                type: "integer",
                nullable: false,
                defaultValue: 1,
                oldClrType: typeof(int),
                oldNullable: true,
                oldDefaultValue: 1);

            migrationBuilder.AddForeignKey(
                name: "fk_application_user_nom_court_court_id",
                table: "identity_users",
                column: "court_id",
                principalTable: "nom_court",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
