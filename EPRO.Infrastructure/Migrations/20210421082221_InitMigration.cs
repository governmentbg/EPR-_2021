using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace EPRO.Infrastructure.Migrations
{
    public partial class InitMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "identity_roles",
                columns: table => new
                {
                    id = table.Column<string>(nullable: false),
                    name = table.Column<string>(maxLength: 256, nullable: true),
                    normalized_name = table.Column<string>(maxLength: 256, nullable: true),
                    concurrency_stamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_application_role", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "nom_case_role",
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
                    table.PrimaryKey("pk_nom_case_role", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "nom_case_type",
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
                    table.PrimaryKey("pk_nom_case_type", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "nom_court",
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
                    table.PrimaryKey("pk_nom_court", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "nom_dismissal_type",
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
                    table.PrimaryKey("pk_nom_dismissal_type", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "identity_role_claims",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    role_id = table.Column<string>(nullable: false),
                    claim_type = table.Column<string>(nullable: true),
                    claim_value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_application_role_claim", x => x.id);
                    table.ForeignKey(
                        name: "fk_application_role_claim_application_role_role_id",
                        column: x => x.role_id,
                        principalTable: "identity_roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "identity_users",
                columns: table => new
                {
                    id = table.Column<string>(nullable: false),
                    user_name = table.Column<string>(maxLength: 256, nullable: true),
                    normalized_user_name = table.Column<string>(maxLength: 256, nullable: true),
                    email = table.Column<string>(maxLength: 256, nullable: true),
                    normalized_email = table.Column<string>(maxLength: 256, nullable: true),
                    email_confirmed = table.Column<bool>(nullable: false),
                    password_hash = table.Column<string>(nullable: true),
                    security_stamp = table.Column<string>(nullable: true),
                    concurrency_stamp = table.Column<string>(nullable: true),
                    phone_number = table.Column<string>(nullable: true),
                    phone_number_confirmed = table.Column<bool>(nullable: false),
                    two_factor_enabled = table.Column<bool>(nullable: false),
                    lockout_end = table.Column<DateTimeOffset>(nullable: true),
                    lockout_enabled = table.Column<bool>(nullable: false),
                    access_failed_count = table.Column<int>(nullable: false),
                    uic = table.Column<string>(maxLength: 20, nullable: true),
                    full_name = table.Column<string>(maxLength: 500, nullable: true),
                    is_active = table.Column<bool>(nullable: false, defaultValue: false),
                    must_change_password = table.Column<bool>(nullable: false, defaultValue: false),
                    court_id = table.Column<int>(nullable: false, defaultValue: 1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_application_user", x => x.id);
                    table.ForeignKey(
                        name: "fk_application_user_nom_court_court_id",
                        column: x => x.court_id,
                        principalTable: "nom_court",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "audit_log",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<string>(nullable: true),
                    date_wrt = table.Column<DateTime>(nullable: false),
                    court_id = table.Column<int>(nullable: false),
                    operation = table.Column<string>(nullable: true),
                    object_info = table.Column<string>(nullable: true),
                    action_info = table.Column<string>(nullable: true),
                    request_url = table.Column<string>(nullable: true),
                    client_ip = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_audit_log", x => x.id);
                    table.ForeignKey(
                        name: "fk_audit_log_nom_court_court_id",
                        column: x => x.court_id,
                        principalTable: "nom_court",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_audit_log_application_user_user_id",
                        column: x => x.user_id,
                        principalTable: "identity_users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "dismissal",
                columns: table => new
                {
                    id = table.Column<string>(nullable: false),
                    user_id = table.Column<string>(nullable: true),
                    date_wrt = table.Column<DateTime>(nullable: false),
                    user_expired_id = table.Column<string>(nullable: true),
                    date_expired = table.Column<DateTime>(nullable: true),
                    description_expired = table.Column<string>(nullable: true),
                    court_id = table.Column<int>(nullable: false),
                    case_type_id = table.Column<int>(nullable: false),
                    case_number = table.Column<string>(nullable: true),
                    case_year = table.Column<int>(nullable: false),
                    judge_given_name = table.Column<string>(nullable: true),
                    judge_middle_name = table.Column<string>(nullable: true),
                    judge_family_name = table.Column<string>(nullable: true),
                    judge_is_chairman = table.Column<bool>(nullable: false),
                    case_role_id = table.Column<int>(nullable: false),
                    dismissal_type_id = table.Column<int>(nullable: false),
                    objection_upheld = table.Column<string>(nullable: true),
                    dismissalReason = table.Column<string>(nullable: true),
                    document_type = table.Column<string>(nullable: true),
                    document_document = table.Column<int>(nullable: false),
                    side_given_name = table.Column<string>(nullable: true),
                    side_middle_name = table.Column<string>(nullable: true),
                    side_family_name = table.Column<string>(nullable: true),
                    side_involvment_kind = table.Column<string>(nullable: true),
                    hearing_type = table.Column<string>(nullable: true),
                    hearing_date = table.Column<DateTime>(nullable: true),
                    act_type = table.Column<string>(nullable: true),
                    act_document = table.Column<int>(nullable: false),
                    act_declared_date = table.Column<DateTime>(nullable: true),
                    replacejudge_given_name = table.Column<string>(nullable: true),
                    replacejudge_middle_name = table.Column<string>(nullable: true),
                    replacejudge_family_name = table.Column<string>(nullable: true),
                    replacejudge_is_chairman = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_dismissal", x => x.id);
                    table.ForeignKey(
                        name: "fk_dismissal_nom_case_role_case_role_id",
                        column: x => x.case_role_id,
                        principalTable: "nom_case_role",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_dismissal_nom_case_type_case_type_id",
                        column: x => x.case_type_id,
                        principalTable: "nom_case_type",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_dismissal_nom_court_court_id",
                        column: x => x.court_id,
                        principalTable: "nom_court",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_dismissal_nom_dismissal_type_dismissal_type_id",
                        column: x => x.dismissal_type_id,
                        principalTable: "nom_dismissal_type",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_dismissal_application_user_user_expired_id",
                        column: x => x.user_expired_id,
                        principalTable: "identity_users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_dismissal_application_user_user_id",
                        column: x => x.user_id,
                        principalTable: "identity_users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "identity_user_claims",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<string>(nullable: false),
                    claim_type = table.Column<string>(nullable: true),
                    claim_value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_application_user_claim", x => x.id);
                    table.ForeignKey(
                        name: "fk_application_user_claim_application_user_user_id",
                        column: x => x.user_id,
                        principalTable: "identity_users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "identity_user_logins",
                columns: table => new
                {
                    login_provider = table.Column<string>(maxLength: 128, nullable: false),
                    provider_key = table.Column<string>(maxLength: 128, nullable: false),
                    provider_display_name = table.Column<string>(nullable: true),
                    user_id = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_identity_user_logins", x => new { x.provider_key, x.login_provider });
                    table.ForeignKey(
                        name: "fk_application_user_login_application_user_user_id",
                        column: x => x.user_id,
                        principalTable: "identity_users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "identity_user_roles",
                columns: table => new
                {
                    user_id = table.Column<string>(nullable: false),
                    role_id = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_identity_user_roles", x => new { x.user_id, x.role_id });
                    table.ForeignKey(
                        name: "fk_application_user_role_application_role_role_id",
                        column: x => x.role_id,
                        principalTable: "identity_roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_application_user_role_application_user_user_id",
                        column: x => x.user_id,
                        principalTable: "identity_users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "identity_user_tokens",
                columns: table => new
                {
                    user_id = table.Column<string>(nullable: false),
                    login_provider = table.Column<string>(maxLength: 128, nullable: false),
                    name = table.Column<string>(maxLength: 128, nullable: false),
                    value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_identity_user_tokens", x => new { x.user_id, x.login_provider, x.name });
                    table.ForeignKey(
                        name: "fk_application_user_token_application_user_user_id",
                        column: x => x.user_id,
                        principalTable: "identity_users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "mongo_file",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<string>(nullable: true),
                    date_wrt = table.Column<DateTime>(nullable: false),
                    user_expired_id = table.Column<string>(nullable: true),
                    date_expired = table.Column<DateTime>(nullable: true),
                    description_expired = table.Column<string>(nullable: true),
                    file_id = table.Column<string>(nullable: true),
                    source_type = table.Column<int>(nullable: false),
                    source_id = table.Column<string>(nullable: true),
                    file_name = table.Column<string>(nullable: true),
                    title = table.Column<string>(nullable: true),
                    file_size = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_mongo_file", x => x.id);
                    table.ForeignKey(
                        name: "fk_mongo_file_application_user_user_expired_id",
                        column: x => x.user_expired_id,
                        principalTable: "identity_users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_mongo_file_application_user_user_id",
                        column: x => x.user_id,
                        principalTable: "identity_users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "news",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<string>(nullable: true),
                    date_wrt = table.Column<DateTime>(nullable: false),
                    user_expired_id = table.Column<string>(nullable: true),
                    date_expired = table.Column<DateTime>(nullable: true),
                    description_expired = table.Column<string>(nullable: true),
                    label = table.Column<string>(nullable: false),
                    description = table.Column<string>(nullable: false),
                    date_start = table.Column<DateTime>(nullable: false),
                    date_end = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_news", x => x.id);
                    table.ForeignKey(
                        name: "fk_news_application_user_user_expired_id",
                        column: x => x.user_expired_id,
                        principalTable: "identity_users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_news_application_user_user_id",
                        column: x => x.user_id,
                        principalTable: "identity_users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_audit_log_court_id",
                table: "audit_log",
                column: "court_id");

            migrationBuilder.CreateIndex(
                name: "ix_audit_log_user_id",
                table: "audit_log",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_dismissal_case_role_id",
                table: "dismissal",
                column: "case_role_id");

            migrationBuilder.CreateIndex(
                name: "ix_dismissal_case_type_id",
                table: "dismissal",
                column: "case_type_id");

            migrationBuilder.CreateIndex(
                name: "ix_dismissal_court_id",
                table: "dismissal",
                column: "court_id");

            migrationBuilder.CreateIndex(
                name: "ix_dismissal_dismissal_type_id",
                table: "dismissal",
                column: "dismissal_type_id");

            migrationBuilder.CreateIndex(
                name: "ix_dismissal_user_expired_id",
                table: "dismissal",
                column: "user_expired_id");

            migrationBuilder.CreateIndex(
                name: "ix_dismissal_user_id",
                table: "dismissal",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_application_role_claim_role_id",
                table: "identity_role_claims",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "role_name_index",
                table: "identity_roles",
                column: "normalized_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_application_user_claim_user_id",
                table: "identity_user_claims",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_application_user_login_user_id",
                table: "identity_user_logins",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_application_user_role_role_id",
                table: "identity_user_roles",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "ix_application_user_court_id",
                table: "identity_users",
                column: "court_id");

            migrationBuilder.CreateIndex(
                name: "email_index",
                table: "identity_users",
                column: "normalized_email");

            migrationBuilder.CreateIndex(
                name: "user_name_index",
                table: "identity_users",
                column: "normalized_user_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_mongo_file_user_expired_id",
                table: "mongo_file",
                column: "user_expired_id");

            migrationBuilder.CreateIndex(
                name: "ix_mongo_file_user_id",
                table: "mongo_file",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_news_user_expired_id",
                table: "news",
                column: "user_expired_id");

            migrationBuilder.CreateIndex(
                name: "ix_news_user_id",
                table: "news",
                column: "user_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "audit_log");

            migrationBuilder.DropTable(
                name: "dismissal");

            migrationBuilder.DropTable(
                name: "identity_role_claims");

            migrationBuilder.DropTable(
                name: "identity_user_claims");

            migrationBuilder.DropTable(
                name: "identity_user_logins");

            migrationBuilder.DropTable(
                name: "identity_user_roles");

            migrationBuilder.DropTable(
                name: "identity_user_tokens");

            migrationBuilder.DropTable(
                name: "mongo_file");

            migrationBuilder.DropTable(
                name: "news");

            migrationBuilder.DropTable(
                name: "nom_case_role");

            migrationBuilder.DropTable(
                name: "nom_case_type");

            migrationBuilder.DropTable(
                name: "nom_dismissal_type");

            migrationBuilder.DropTable(
                name: "identity_roles");

            migrationBuilder.DropTable(
                name: "identity_users");

            migrationBuilder.DropTable(
                name: "nom_court");
        }
    }
}
