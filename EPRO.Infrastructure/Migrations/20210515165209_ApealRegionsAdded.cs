using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace EPRO.Infrastructure.Migrations
{
    public partial class ApealRegionsAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "dismissalReason",
                table: "dismissal",
                newName: "dismissal_reason");

            migrationBuilder.AddColumn<int>(
                name: "apeal_region_id",
                table: "nom_court",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "judge_full_name",
                table: "dismissal",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "hearing_date",
                table: "dismissal",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "case_number",
                table: "dismissal",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "act_declared_date",
                table: "dismissal",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "nom_apeal_region",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    order_number = table.Column<int>(nullable: false),
                    code = table.Column<string>(nullable: true),
                    label = table.Column<string>(nullable: false),
                    description = table.Column<string>(nullable: true),
                    date_start = table.Column<DateTime>(nullable: false),
                    date_end = table.Column<DateTime>(nullable: true),
                    is_active = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_nom_apeal_region", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_nom_court_apeal_region_id",
                table: "nom_court",
                column: "apeal_region_id");

            migrationBuilder.AddForeignKey(
                name: "fk_nom_court_nom_apeal_region_apeal_region_id",
                table: "nom_court",
                column: "apeal_region_id",
                principalTable: "nom_apeal_region",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_nom_court_nom_apeal_region_apeal_region_id",
                table: "nom_court");

            migrationBuilder.DropTable(
                name: "nom_apeal_region");

            migrationBuilder.DropIndex(
                name: "ix_nom_court_apeal_region_id",
                table: "nom_court");

            migrationBuilder.DropColumn(
                name: "apeal_region_id",
                table: "nom_court");

            migrationBuilder.RenameColumn(
                name: "dismissal_reason",
                table: "dismissal",
                newName: "dismissalReason");

            migrationBuilder.AlterColumn<string>(
                name: "judge_full_name",
                table: "dismissal",
                type: "text",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<DateTime>(
                name: "hearing_date",
                table: "dismissal",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<string>(
                name: "case_number",
                table: "dismissal",
                type: "text",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<DateTime>(
                name: "act_declared_date",
                table: "dismissal",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime));
        }
    }
}
