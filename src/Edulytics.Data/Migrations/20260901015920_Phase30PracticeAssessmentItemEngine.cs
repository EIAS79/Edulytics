using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Edulytics.Data.Migrations
{
    /// <inheritdoc />
    public partial class Phase30PracticeAssessmentItemEngine : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AssessmentItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SchoolId = table.Column<Guid>(type: "uuid", nullable: false),
                    CurriculumAdoptionId = table.Column<Guid>(type: "uuid", nullable: false),
                    CurriculumPedagogicalLessonId = table.Column<Guid>(type: "uuid", nullable: true),
                    CurriculumTopicId = table.Column<Guid>(type: "uuid", nullable: true),
                    Source = table.Column<int>(type: "integer", nullable: false),
                    ItemType = table.Column<int>(type: "integer", nullable: false),
                    Difficulty = table.Column<int>(type: "integer", nullable: false),
                    Prompt = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: false),
                    CorrectAnswer = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    Solution = table.Column<string>(type: "character varying(8000)", maxLength: 8000, nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    GenerationMethod = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    GenerationFamily = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    GenerationParametersJson = table.Column<string>(type: "jsonb", nullable: true),
                    ExposureFingerprint = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    ValidationMetadataJson = table.Column<string>(type: "jsonb", nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "bytea", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssessmentItems", x => x.Id);
                    table.UniqueConstraint("AK_AssessmentItems_SchoolId_Id", x => new { x.SchoolId, x.Id });
                    table.ForeignKey(
                        name: "FK_AssessmentItems_CurriculumPedagogicalLessons_CurriculumPeda~",
                        column: x => x.CurriculumPedagogicalLessonId,
                        principalTable: "CurriculumPedagogicalLessons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AssessmentItems_CurriculumTopics_SchoolId_CurriculumTopicId",
                        columns: x => new { x.SchoolId, x.CurriculumTopicId },
                        principalTable: "CurriculumTopics",
                        principalColumns: new[] { "SchoolId", "Id" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AssessmentItems_SchoolCurriculumAdoptions_SchoolId_Curricul~",
                        columns: x => new { x.SchoolId, x.CurriculumAdoptionId },
                        principalTable: "SchoolCurriculumAdoptions",
                        principalColumns: new[] { "SchoolId", "Id" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AssessmentItems_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PracticeAttempts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SchoolId = table.Column<Guid>(type: "uuid", nullable: false),
                    StudentProfileId = table.Column<Guid>(type: "uuid", nullable: false),
                    CurriculumAdoptionId = table.Column<Guid>(type: "uuid", nullable: false),
                    CurriculumPedagogicalLessonId = table.Column<Guid>(type: "uuid", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    StartedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    SubmittedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Score = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    MaxScore = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    Percentage = table.Column<decimal>(type: "numeric(7,2)", precision: 7, scale: 2, nullable: false),
                    RowVersion = table.Column<byte[]>(type: "bytea", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PracticeAttempts", x => x.Id);
                    table.UniqueConstraint("AK_PracticeAttempts_SchoolId_Id", x => new { x.SchoolId, x.Id });
                    table.ForeignKey(
                        name: "FK_PracticeAttempts_CurriculumPedagogicalLessons_CurriculumPed~",
                        column: x => x.CurriculumPedagogicalLessonId,
                        principalTable: "CurriculumPedagogicalLessons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PracticeAttempts_SchoolCurriculumAdoptions_SchoolId_Curricu~",
                        columns: x => new { x.SchoolId, x.CurriculumAdoptionId },
                        principalTable: "SchoolCurriculumAdoptions",
                        principalColumns: new[] { "SchoolId", "Id" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PracticeAttempts_StudentProfiles_SchoolId_StudentProfileId",
                        columns: x => new { x.SchoolId, x.StudentProfileId },
                        principalTable: "StudentProfiles",
                        principalColumns: new[] { "SchoolId", "Id" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AssessmentItemOutcomes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SchoolId = table.Column<Guid>(type: "uuid", nullable: false),
                    AssessmentItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    LearningOutcomeId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssessmentItemOutcomes", x => x.Id);
                    table.UniqueConstraint("AK_AssessmentItemOutcomes_SchoolId_Id", x => new { x.SchoolId, x.Id });
                    table.ForeignKey(
                        name: "FK_AssessmentItemOutcomes_AssessmentItems_SchoolId_AssessmentI~",
                        columns: x => new { x.SchoolId, x.AssessmentItemId },
                        principalTable: "AssessmentItems",
                        principalColumns: new[] { "SchoolId", "Id" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AssessmentItemOutcomes_LearningOutcomes_SchoolId_LearningOu~",
                        columns: x => new { x.SchoolId, x.LearningOutcomeId },
                        principalTable: "LearningOutcomes",
                        principalColumns: new[] { "SchoolId", "Id" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StudentItemExposures",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SchoolId = table.Column<Guid>(type: "uuid", nullable: false),
                    StudentProfileId = table.Column<Guid>(type: "uuid", nullable: false),
                    AssessmentItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    ExposureFingerprint = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    ExposedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentItemExposures", x => x.Id);
                    table.UniqueConstraint("AK_StudentItemExposures_SchoolId_Id", x => new { x.SchoolId, x.Id });
                    table.ForeignKey(
                        name: "FK_StudentItemExposures_AssessmentItems_SchoolId_AssessmentIte~",
                        columns: x => new { x.SchoolId, x.AssessmentItemId },
                        principalTable: "AssessmentItems",
                        principalColumns: new[] { "SchoolId", "Id" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StudentItemExposures_StudentProfiles_SchoolId_StudentProfil~",
                        columns: x => new { x.SchoolId, x.StudentProfileId },
                        principalTable: "StudentProfiles",
                        principalColumns: new[] { "SchoolId", "Id" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LearningEvidence",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SchoolId = table.Column<Guid>(type: "uuid", nullable: false),
                    StudentProfileId = table.Column<Guid>(type: "uuid", nullable: false),
                    LearningOutcomeId = table.Column<Guid>(type: "uuid", nullable: false),
                    PracticeAttemptId = table.Column<Guid>(type: "uuid", nullable: false),
                    AssessmentItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    EvidenceType = table.Column<int>(type: "integer", nullable: false),
                    Difficulty = table.Column<int>(type: "integer", nullable: false),
                    IsCorrect = table.Column<bool>(type: "boolean", nullable: false),
                    Score = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    MaxScore = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    OccurredAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LearningEvidence", x => x.Id);
                    table.UniqueConstraint("AK_LearningEvidence_SchoolId_Id", x => new { x.SchoolId, x.Id });
                    table.ForeignKey(
                        name: "FK_LearningEvidence_AssessmentItems_SchoolId_AssessmentItemId",
                        columns: x => new { x.SchoolId, x.AssessmentItemId },
                        principalTable: "AssessmentItems",
                        principalColumns: new[] { "SchoolId", "Id" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LearningEvidence_LearningOutcomes_SchoolId_LearningOutcomeId",
                        columns: x => new { x.SchoolId, x.LearningOutcomeId },
                        principalTable: "LearningOutcomes",
                        principalColumns: new[] { "SchoolId", "Id" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LearningEvidence_PracticeAttempts_SchoolId_PracticeAttemptId",
                        columns: x => new { x.SchoolId, x.PracticeAttemptId },
                        principalTable: "PracticeAttempts",
                        principalColumns: new[] { "SchoolId", "Id" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LearningEvidence_StudentProfiles_SchoolId_StudentProfileId",
                        columns: x => new { x.SchoolId, x.StudentProfileId },
                        principalTable: "StudentProfiles",
                        principalColumns: new[] { "SchoolId", "Id" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PracticeAttemptItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SchoolId = table.Column<Guid>(type: "uuid", nullable: false),
                    PracticeAttemptId = table.Column<Guid>(type: "uuid", nullable: false),
                    AssessmentItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    Order = table.Column<int>(type: "integer", nullable: false),
                    MaxScore = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PracticeAttemptItems", x => x.Id);
                    table.UniqueConstraint("AK_PracticeAttemptItems_SchoolId_Id", x => new { x.SchoolId, x.Id });
                    table.ForeignKey(
                        name: "FK_PracticeAttemptItems_AssessmentItems_SchoolId_AssessmentIte~",
                        columns: x => new { x.SchoolId, x.AssessmentItemId },
                        principalTable: "AssessmentItems",
                        principalColumns: new[] { "SchoolId", "Id" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PracticeAttemptItems_PracticeAttempts_SchoolId_PracticeAtte~",
                        columns: x => new { x.SchoolId, x.PracticeAttemptId },
                        principalTable: "PracticeAttempts",
                        principalColumns: new[] { "SchoolId", "Id" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PracticeResponses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SchoolId = table.Column<Guid>(type: "uuid", nullable: false),
                    PracticeAttemptItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    Answer = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    IsCorrect = table.Column<bool>(type: "boolean", nullable: false),
                    Score = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    Feedback = table.Column<string>(type: "character varying(8000)", maxLength: 8000, nullable: false),
                    AnsweredAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PracticeResponses", x => x.Id);
                    table.UniqueConstraint("AK_PracticeResponses_SchoolId_Id", x => new { x.SchoolId, x.Id });
                    table.ForeignKey(
                        name: "FK_PracticeResponses_PracticeAttemptItems_SchoolId_PracticeAtt~",
                        columns: x => new { x.SchoolId, x.PracticeAttemptItemId },
                        principalTable: "PracticeAttemptItems",
                        principalColumns: new[] { "SchoolId", "Id" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AssessmentItemOutcomes_SchoolId_AssessmentItemId_LearningOu~",
                table: "AssessmentItemOutcomes",
                columns: new[] { "SchoolId", "AssessmentItemId", "LearningOutcomeId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AssessmentItemOutcomes_SchoolId_LearningOutcomeId",
                table: "AssessmentItemOutcomes",
                columns: new[] { "SchoolId", "LearningOutcomeId" });

            migrationBuilder.CreateIndex(
                name: "IX_AssessmentItems_CurriculumPedagogicalLessonId",
                table: "AssessmentItems",
                column: "CurriculumPedagogicalLessonId");

            migrationBuilder.CreateIndex(
                name: "IX_AssessmentItems_SchoolId_CurriculumAdoptionId_CurriculumPed~",
                table: "AssessmentItems",
                columns: new[] { "SchoolId", "CurriculumAdoptionId", "CurriculumPedagogicalLessonId" });

            migrationBuilder.CreateIndex(
                name: "IX_AssessmentItems_SchoolId_CurriculumTopicId",
                table: "AssessmentItems",
                columns: new[] { "SchoolId", "CurriculumTopicId" });

            migrationBuilder.CreateIndex(
                name: "IX_AssessmentItems_SchoolId_ExposureFingerprint",
                table: "AssessmentItems",
                columns: new[] { "SchoolId", "ExposureFingerprint" });

            migrationBuilder.CreateIndex(
                name: "IX_LearningEvidence_SchoolId_AssessmentItemId",
                table: "LearningEvidence",
                columns: new[] { "SchoolId", "AssessmentItemId" });

            migrationBuilder.CreateIndex(
                name: "IX_LearningEvidence_SchoolId_LearningOutcomeId",
                table: "LearningEvidence",
                columns: new[] { "SchoolId", "LearningOutcomeId" });

            migrationBuilder.CreateIndex(
                name: "IX_LearningEvidence_SchoolId_PracticeAttemptId_AssessmentItemI~",
                table: "LearningEvidence",
                columns: new[] { "SchoolId", "PracticeAttemptId", "AssessmentItemId", "LearningOutcomeId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LearningEvidence_SchoolId_StudentProfileId_LearningOutcomeI~",
                table: "LearningEvidence",
                columns: new[] { "SchoolId", "StudentProfileId", "LearningOutcomeId", "OccurredAtUtc" });

            migrationBuilder.CreateIndex(
                name: "IX_PracticeAttemptItems_SchoolId_AssessmentItemId",
                table: "PracticeAttemptItems",
                columns: new[] { "SchoolId", "AssessmentItemId" });

            migrationBuilder.CreateIndex(
                name: "IX_PracticeAttemptItems_SchoolId_PracticeAttemptId_AssessmentI~",
                table: "PracticeAttemptItems",
                columns: new[] { "SchoolId", "PracticeAttemptId", "AssessmentItemId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PracticeAttemptItems_SchoolId_PracticeAttemptId_Order",
                table: "PracticeAttemptItems",
                columns: new[] { "SchoolId", "PracticeAttemptId", "Order" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PracticeAttempts_CurriculumPedagogicalLessonId",
                table: "PracticeAttempts",
                column: "CurriculumPedagogicalLessonId");

            migrationBuilder.CreateIndex(
                name: "IX_PracticeAttempts_SchoolId_CurriculumAdoptionId",
                table: "PracticeAttempts",
                columns: new[] { "SchoolId", "CurriculumAdoptionId" });

            migrationBuilder.CreateIndex(
                name: "IX_PracticeAttempts_SchoolId_StudentProfileId_StartedAtUtc",
                table: "PracticeAttempts",
                columns: new[] { "SchoolId", "StudentProfileId", "StartedAtUtc" });

            migrationBuilder.CreateIndex(
                name: "IX_PracticeResponses_SchoolId_PracticeAttemptItemId",
                table: "PracticeResponses",
                columns: new[] { "SchoolId", "PracticeAttemptItemId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StudentItemExposures_SchoolId_AssessmentItemId",
                table: "StudentItemExposures",
                columns: new[] { "SchoolId", "AssessmentItemId" });

            migrationBuilder.CreateIndex(
                name: "IX_StudentItemExposures_SchoolId_StudentProfileId_ExposureFing~",
                table: "StudentItemExposures",
                columns: new[] { "SchoolId", "StudentProfileId", "ExposureFingerprint", "ExposedAtUtc" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssessmentItemOutcomes");

            migrationBuilder.DropTable(
                name: "LearningEvidence");

            migrationBuilder.DropTable(
                name: "PracticeResponses");

            migrationBuilder.DropTable(
                name: "StudentItemExposures");

            migrationBuilder.DropTable(
                name: "PracticeAttemptItems");

            migrationBuilder.DropTable(
                name: "AssessmentItems");

            migrationBuilder.DropTable(
                name: "PracticeAttempts");
        }
    }
}
