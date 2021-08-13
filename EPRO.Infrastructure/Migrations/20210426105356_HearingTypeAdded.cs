using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace EPRO.Infrastructure.Migrations
{
    public partial class HearingTypeAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "judge_family_name",
                table: "dismissal");

            migrationBuilder.DropColumn(
                name: "judge_given_name",
                table: "dismissal");

            migrationBuilder.DropColumn(
                name: "judge_middle_name",
                table: "dismissal");

            migrationBuilder.DropColumn(
                name: "replacejudge_family_name",
                table: "dismissal");

            migrationBuilder.DropColumn(
                name: "replacejudge_given_name",
                table: "dismissal");

            migrationBuilder.DropColumn(
                name: "replacejudge_middle_name",
                table: "dismissal");

            migrationBuilder.DropColumn(
                name: "side_family_name",
                table: "dismissal");

            migrationBuilder.DropColumn(
                name: "side_given_name",
                table: "dismissal");

            migrationBuilder.DropColumn(
                name: "side_middle_name",
                table: "dismissal");

            migrationBuilder.DropColumn(
               name: "hearing_type",
               table: "dismissal");

            migrationBuilder.AddColumn<int>(
               name: "hearing_type",
               table: "dismissal",
               nullable: false);

            migrationBuilder.CreateTable(
                name: "nom_hearing_type",
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
                    table.PrimaryKey("pk_nom_hearing_type", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_dismissal_hearing_type",
                table: "dismissal",
                column: "hearing_type");

            migrationBuilder.AddForeignKey(
                name: "fk_dismissal_nom_hearing_type_hearing_type",
                table: "dismissal",
                column: "hearing_type",
                principalTable: "nom_hearing_type",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_dismissal_nom_hearing_type_hearing_type",
                table: "dismissal");

            migrationBuilder.DropTable(
                name: "nom_hearing_type");

            migrationBuilder.DropIndex(
                name: "ix_dismissal_hearing_type",
                table: "dismissal");

            migrationBuilder.DropColumn(
                name: "hearing_type",
                table: "dismissal");

            migrationBuilder.AddColumn<string>(
               name: "hearing_type",
               table: "dismissal",
               nullable: true);            

            migrationBuilder.AddColumn<string>(
                name: "judge_family_name",
                table: "dismissal",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "judge_given_name",
                table: "dismissal",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "judge_middle_name",
                table: "dismissal",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "replacejudge_family_name",
                table: "dismissal",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "replacejudge_given_name",
                table: "dismissal",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "replacejudge_middle_name",
                table: "dismissal",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "side_family_name",
                table: "dismissal",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "side_given_name",
                table: "dismissal",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "side_middle_name",
                table: "dismissal",
                type: "text",
                nullable: true);
        }
    }
}
