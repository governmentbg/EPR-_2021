using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace EPRO.Infrastructure.Migrations
{
    public partial class ActTypeAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "act_type",
                table: "dismissal");

            migrationBuilder.AddColumn<int>(
                name: "act_type_id",
                table: "dismissal",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "entry_type",
                table: "dismissal",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "nom_act_type",
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
                    table.PrimaryKey("pk_nom_act_type", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_dismissal_act_type_id",
                table: "dismissal",
                column: "act_type_id");

            migrationBuilder.AddForeignKey(
                name: "fk_dismissal_nom_act_type_act_type_id",
                table: "dismissal",
                column: "act_type_id",
                principalTable: "nom_act_type",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_dismissal_nom_act_type_act_type_id",
                table: "dismissal");

            migrationBuilder.DropTable(
                name: "nom_act_type");

            migrationBuilder.DropIndex(
                name: "ix_dismissal_act_type_id",
                table: "dismissal");

            migrationBuilder.DropColumn(
                name: "act_type_id",
                table: "dismissal");

            migrationBuilder.DropColumn(
                name: "entry_type",
                table: "dismissal");

            migrationBuilder.AddColumn<string>(
                name: "act_type",
                table: "dismissal",
                type: "text",
                nullable: true);
        }
    }
}
