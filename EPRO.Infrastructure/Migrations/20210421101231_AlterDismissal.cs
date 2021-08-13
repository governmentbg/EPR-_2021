using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EPRO.Infrastructure.Migrations
{
    public partial class AlterDismissal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "document_document",
                table: "dismissal",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<DateTime>(
                name: "document_date",
                table: "dismissal",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "document_date",
                table: "dismissal");

            migrationBuilder.AlterColumn<int>(
                name: "document_document",
                table: "dismissal",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}
