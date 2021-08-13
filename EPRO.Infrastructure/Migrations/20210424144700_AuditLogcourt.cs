using Microsoft.EntityFrameworkCore.Migrations;

namespace EPRO.Infrastructure.Migrations
{
    public partial class AuditLogcourt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_audit_log_nom_court_court_id",
                table: "audit_log");

            migrationBuilder.AlterColumn<int>(
                name: "court_id",
                table: "audit_log",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "fk_audit_log_nom_court_court_id",
                table: "audit_log",
                column: "court_id",
                principalTable: "nom_court",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_audit_log_nom_court_court_id",
                table: "audit_log");

            migrationBuilder.AlterColumn<int>(
                name: "court_id",
                table: "audit_log",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "fk_audit_log_nom_court_court_id",
                table: "audit_log",
                column: "court_id",
                principalTable: "nom_court",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
