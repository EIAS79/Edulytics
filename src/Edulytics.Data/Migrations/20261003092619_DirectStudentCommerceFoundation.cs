using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Edulytics.Data.Migrations
{
    /// <inheritdoc />
    public partial class DirectStudentCommerceFoundation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DirectStudentProfiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    FirstName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    DisplayName = table.Column<string>(type: "character varying(205)", maxLength: 205, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "bytea", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DirectStudentProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DirectStudentProfiles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PersonalSubscriptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    StudentUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    PlanType = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    FrameworkVersionId = table.Column<Guid>(type: "uuid", nullable: false),
                    FrameworkCode = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    FrameworkName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    FrameworkVersionName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    CurriculumLevelKey = table.Column<string>(type: "character varying(160)", maxLength: 160, nullable: false),
                    CurriculumLogicalLevel = table.Column<int>(type: "integer", nullable: false),
                    CurriculumLevelLabel = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    CurriculumPathway = table.Column<string>(type: "character varying(160)", maxLength: 160, nullable: true),
                    SubjectCode = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    SubjectName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    BaseAmount = table.Column<decimal>(type: "numeric(14,2)", precision: 14, scale: 2, nullable: false),
                    DiscountPercent = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    PaidAmount = table.Column<decimal>(type: "numeric(14,2)", precision: 14, scale: 2, nullable: false),
                    CurrencyCode = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    PaymentProvider = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    ExternalCustomerReference = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ExternalPaymentReference = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ActivatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    StartsAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    EndsAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CancelledAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "bytea", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonalSubscriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PersonalSubscriptions_AspNetUsers_StudentUserId",
                        column: x => x.StudentUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PersonalSubscriptions_CurriculumFrameworkVersions_Framework~",
                        column: x => x.FrameworkVersionId,
                        principalTable: "CurriculumFrameworkVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PersonalEntitlements",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SubscriptionId = table.Column<Guid>(type: "uuid", nullable: false),
                    StudentUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    FrameworkVersionId = table.Column<Guid>(type: "uuid", nullable: false),
                    FrameworkCode = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    FrameworkName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    FrameworkVersionName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    CurriculumLevelKey = table.Column<string>(type: "character varying(160)", maxLength: 160, nullable: false),
                    CurriculumLogicalLevel = table.Column<int>(type: "integer", nullable: false),
                    CurriculumLevelLabel = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    CurriculumPathway = table.Column<string>(type: "character varying(160)", maxLength: 160, nullable: true),
                    SubjectCode = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    SubjectName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    StartsAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndsAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "bytea", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonalEntitlements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PersonalEntitlements_AspNetUsers_StudentUserId",
                        column: x => x.StudentUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PersonalEntitlements_CurriculumFrameworkVersions_FrameworkV~",
                        column: x => x.FrameworkVersionId,
                        principalTable: "CurriculumFrameworkVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PersonalEntitlements_PersonalSubscriptions_SubscriptionId",
                        column: x => x.SubscriptionId,
                        principalTable: "PersonalSubscriptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PersonalPaymentTransactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SubscriptionId = table.Column<Guid>(type: "uuid", nullable: false),
                    StudentUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Provider = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    ProviderCheckoutSessionId = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    ProviderPaymentId = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ProviderEventId = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Amount = table.Column<decimal>(type: "numeric(14,2)", precision: 14, scale: 2, nullable: false),
                    CurrencyCode = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "bytea", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonalPaymentTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PersonalPaymentTransactions_AspNetUsers_StudentUserId",
                        column: x => x.StudentUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PersonalPaymentTransactions_PersonalSubscriptions_Subscript~",
                        column: x => x.SubscriptionId,
                        principalTable: "PersonalSubscriptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DirectStudentProfiles_UserId",
                table: "DirectStudentProfiles",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PersonalEntitlements_FrameworkVersionId",
                table: "PersonalEntitlements",
                column: "FrameworkVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonalEntitlements_StudentUserId_IsActive_StartsAtUtc_End~",
                table: "PersonalEntitlements",
                columns: new[] { "StudentUserId", "IsActive", "StartsAtUtc", "EndsAtUtc" });

            migrationBuilder.CreateIndex(
                name: "IX_PersonalEntitlements_SubscriptionId",
                table: "PersonalEntitlements",
                column: "SubscriptionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PersonalPaymentTransactions_Provider_ProviderCheckoutSessio~",
                table: "PersonalPaymentTransactions",
                columns: new[] { "Provider", "ProviderCheckoutSessionId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PersonalPaymentTransactions_Provider_ProviderEventId",
                table: "PersonalPaymentTransactions",
                columns: new[] { "Provider", "ProviderEventId" },
                unique: true,
                filter: "\"ProviderEventId\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_PersonalPaymentTransactions_StudentUserId_CreatedAtUtc",
                table: "PersonalPaymentTransactions",
                columns: new[] { "StudentUserId", "CreatedAtUtc" });

            migrationBuilder.CreateIndex(
                name: "IX_PersonalPaymentTransactions_SubscriptionId",
                table: "PersonalPaymentTransactions",
                column: "SubscriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonalSubscriptions_ExternalPaymentReference",
                table: "PersonalSubscriptions",
                column: "ExternalPaymentReference");

            migrationBuilder.CreateIndex(
                name: "IX_PersonalSubscriptions_FrameworkVersionId",
                table: "PersonalSubscriptions",
                column: "FrameworkVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonalSubscriptions_StudentUserId_Status_EndsAtUtc",
                table: "PersonalSubscriptions",
                columns: new[] { "StudentUserId", "Status", "EndsAtUtc" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DirectStudentProfiles");

            migrationBuilder.DropTable(
                name: "PersonalEntitlements");

            migrationBuilder.DropTable(
                name: "PersonalPaymentTransactions");

            migrationBuilder.DropTable(
                name: "PersonalSubscriptions");
        }
    }
}