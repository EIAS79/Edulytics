using Edulytics.Data.Contexts;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Edulytics.Data.Migrations;

[DbContext(typeof(EdulyticsDbContext))]
[Migration("20260928010000_AdaptivePracticeV2Foundation")]
public sealed class AdaptivePracticeV2Foundation : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "AdaptivePracticeSessions",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                SchoolId = table.Column<Guid>(type: "uuid", nullable: false),
                StudentProfileId = table.Column<Guid>(type: "uuid", nullable: false),
                CurriculumAdoptionId = table.Column<Guid>(type: "uuid", nullable: false),
                CurriculumLevelKey = table.Column<string>(
                    type: "character varying(160)",
                    maxLength: 160,
                    nullable: false),
                CurriculumPedagogicalLessonId = table.Column<Guid>(
                    type: "uuid",
                    nullable: false),
                LessonCode = table.Column<string>(
                    type: "character varying(240)",
                    maxLength: 240,
                    nullable: false),
                PrimarySkillId = table.Column<string>(
                    type: "character varying(200)",
                    maxLength: 200,
                    nullable: false),
                Purpose = table.Column<int>(type: "integer", nullable: false),
                Status = table.Column<int>(type: "integer", nullable: false),
                EngineVersion = table.Column<string>(
                    type: "character varying(100)",
                    maxLength: 100,
                    nullable: false),
                PolicyVersion = table.Column<string>(
                    type: "character varying(100)",
                    maxLength: 100,
                    nullable: false),
                CapabilityVersion = table.Column<string>(
                    type: "character varying(100)",
                    maxLength: 100,
                    nullable: false),
                FeatureFlagSnapshotJson = table.Column<string>(
                    type: "jsonb",
                    nullable: false),
                TargetQuestionCount = table.Column<int>(
                    type: "integer",
                    nullable: false),
                CurrentSequence = table.Column<int>(
                    type: "integer",
                    nullable: false),
                StartedAtUtc = table.Column<DateTime>(
                    type: "timestamp with time zone",
                    nullable: false),
                CompletedAtUtc = table.Column<DateTime>(
                    type: "timestamp with time zone",
                    nullable: true),
                StopReason = table.Column<string>(
                    type: "character varying(100)",
                    maxLength: 100,
                    nullable: true),
                RowVersion = table.Column<byte[]>(
                    type: "bytea",
                    nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey(
                    "PK_AdaptivePracticeSessions",
                    x => x.Id);
                table.UniqueConstraint(
                    "AK_AdaptivePracticeSessions_SchoolId_Id",
                    x => new { x.SchoolId, x.Id });
                table.ForeignKey(
                    name: "FK_AdaptivePracticeSessions_CurriculumPedagogicalLessons_Cur~",
                    column: x => x.CurriculumPedagogicalLessonId,
                    principalTable: "CurriculumPedagogicalLessons",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_AdaptivePracticeSessions_SchoolCurriculumAdoptions_School~",
                    columns: x => new
                    {
                        x.SchoolId,
                        x.CurriculumAdoptionId
                    },
                    principalTable: "SchoolCurriculumAdoptions",
                    principalColumns: new[] { "SchoolId", "Id" },
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_AdaptivePracticeSessions_Schools_SchoolId",
                    column: x => x.SchoolId,
                    principalTable: "Schools",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_AdaptivePracticeSessions_StudentProfiles_SchoolId_StudentP~",
                    columns: x => new
                    {
                        x.SchoolId,
                        x.StudentProfileId
                    },
                    principalTable: "StudentProfiles",
                    principalColumns: new[] { "SchoolId", "Id" },
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateTable(
            name: "StudentMisconceptionStates",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                SchoolId = table.Column<Guid>(type: "uuid", nullable: false),
                StudentProfileId = table.Column<Guid>(type: "uuid", nullable: false),
                CurriculumAdoptionId = table.Column<Guid>(type: "uuid", nullable: false),
                SkillId = table.Column<string>(
                    type: "character varying(200)",
                    maxLength: 200,
                    nullable: false),
                MisconceptionId = table.Column<string>(
                    type: "character varying(200)",
                    maxLength: 200,
                    nullable: false),
                QuestionFamily = table.Column<string>(
                    type: "character varying(240)",
                    maxLength: 240,
                    nullable: true),
                Status = table.Column<int>(type: "integer", nullable: false),
                Confidence = table.Column<decimal>(
                    type: "numeric(8,6)",
                    precision: 8,
                    scale: 6,
                    nullable: false),
                ObservationCount = table.Column<int>(
                    type: "integer",
                    nullable: false),
                FirstObservedAtUtc = table.Column<DateTime>(
                    type: "timestamp with time zone",
                    nullable: false),
                LastObservedAtUtc = table.Column<DateTime>(
                    type: "timestamp with time zone",
                    nullable: false),
                LastRemediationAtUtc = table.Column<DateTime>(
                    type: "timestamp with time zone",
                    nullable: true),
                ResolvedAtUtc = table.Column<DateTime>(
                    type: "timestamp with time zone",
                    nullable: true),
                EngineVersion = table.Column<string>(
                    type: "character varying(100)",
                    maxLength: 100,
                    nullable: false),
                RowVersion = table.Column<byte[]>(
                    type: "bytea",
                    nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey(
                    "PK_StudentMisconceptionStates",
                    x => x.Id);
                table.UniqueConstraint(
                    "AK_StudentMisconceptionStates_SchoolId_Id",
                    x => new { x.SchoolId, x.Id });
                table.ForeignKey(
                    name: "FK_StudentMisconceptionStates_SchoolCurriculumAdoptions_Scho~",
                    columns: x => new
                    {
                        x.SchoolId,
                        x.CurriculumAdoptionId
                    },
                    principalTable: "SchoolCurriculumAdoptions",
                    principalColumns: new[] { "SchoolId", "Id" },
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_StudentMisconceptionStates_StudentProfiles_SchoolId_Stude~",
                    columns: x => new
                    {
                        x.SchoolId,
                        x.StudentProfileId
                    },
                    principalTable: "StudentProfiles",
                    principalColumns: new[] { "SchoolId", "Id" },
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateTable(
            name: "StudentRepresentationFluencyStates",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                SchoolId = table.Column<Guid>(type: "uuid", nullable: false),
                StudentProfileId = table.Column<Guid>(type: "uuid", nullable: false),
                SkillId = table.Column<string>(
                    type: "character varying(200)",
                    maxLength: 200,
                    nullable: false),
                Representation = table.Column<string>(
                    type: "character varying(120)",
                    maxLength: 120,
                    nullable: false),
                EvidenceCount = table.Column<int>(
                    type: "integer",
                    nullable: false),
                SuccessCount = table.Column<int>(
                    type: "integer",
                    nullable: false),
                WeightedFluency = table.Column<decimal>(
                    type: "numeric(8,6)",
                    precision: 8,
                    scale: 6,
                    nullable: false),
                LatestEvidenceAtUtc = table.Column<DateTime>(
                    type: "timestamp with time zone",
                    nullable: false),
                EngineVersion = table.Column<string>(
                    type: "character varying(100)",
                    maxLength: 100,
                    nullable: false),
                RowVersion = table.Column<byte[]>(
                    type: "bytea",
                    nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey(
                    "PK_StudentRepresentationFluencyStates",
                    x => x.Id);
                table.UniqueConstraint(
                    "AK_StudentRepresentationFluencyStates_SchoolId_Id",
                    x => new { x.SchoolId, x.Id });
                table.ForeignKey(
                    name: "FK_StudentRepresentationFluencyStates_StudentProfiles_School~",
                    columns: x => new
                    {
                        x.SchoolId,
                        x.StudentProfileId
                    },
                    principalTable: "StudentProfiles",
                    principalColumns: new[] { "SchoolId", "Id" },
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateTable(
            name: "AdaptiveDecisionSnapshots",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                SchoolId = table.Column<Guid>(type: "uuid", nullable: false),
                SessionId = table.Column<Guid>(type: "uuid", nullable: false),
                Sequence = table.Column<int>(type: "integer", nullable: false),
                EngineVersion = table.Column<string>(
                    type: "character varying(100)",
                    maxLength: 100,
                    nullable: false),
                PolicyVersion = table.Column<string>(
                    type: "character varying(100)",
                    maxLength: 100,
                    nullable: false),
                SkillMasteryBefore = table.Column<decimal>(
                    type: "numeric(8,6)",
                    precision: 8,
                    scale: 6,
                    nullable: false),
                PrerequisiteMasteryBefore = table.Column<decimal>(
                    type: "numeric(8,6)",
                    precision: 8,
                    scale: 6,
                    nullable: false),
                RepresentationFluencyJson = table.Column<string>(
                    type: "jsonb",
                    nullable: false),
                ActiveMisconceptionsJson = table.Column<string>(
                    type: "jsonb",
                    nullable: false),
                CurrentComplexity = table.Column<int>(
                    type: "integer",
                    nullable: false),
                TargetComplexity = table.Column<int>(
                    type: "integer",
                    nullable: false),
                SelectedFamily = table.Column<string>(
                    type: "character varying(240)",
                    maxLength: 240,
                    nullable: false),
                SelectedRepresentation = table.Column<string>(
                    type: "character varying(120)",
                    maxLength: 120,
                    nullable: true),
                MisconceptionFocusId = table.Column<string>(
                    type: "character varying(200)",
                    maxLength: 200,
                    nullable: true),
                FreshnessConstraintsJson = table.Column<string>(
                    type: "jsonb",
                    nullable: false),
                DecisionReasonCode = table.Column<string>(
                    type: "character varying(100)",
                    maxLength: 100,
                    nullable: false),
                DecisionTraceJson = table.Column<string>(
                    type: "jsonb",
                    nullable: false),
                CreatedAtUtc = table.Column<DateTime>(
                    type: "timestamp with time zone",
                    nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey(
                    "PK_AdaptiveDecisionSnapshots",
                    x => x.Id);
                table.UniqueConstraint(
                    "AK_AdaptiveDecisionSnapshots_SchoolId_Id",
                    x => new { x.SchoolId, x.Id });
                table.ForeignKey(
                    name: "FK_AdaptiveDecisionSnapshots_AdaptivePracticeSessions_School~",
                    columns: x => new { x.SchoolId, x.SessionId },
                    principalTable: "AdaptivePracticeSessions",
                    principalColumns: new[] { "SchoolId", "Id" },
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateTable(
            name: "AdaptivePracticeTurns",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                SchoolId = table.Column<Guid>(type: "uuid", nullable: false),
                SessionId = table.Column<Guid>(type: "uuid", nullable: false),
                Sequence = table.Column<int>(type: "integer", nullable: false),
                AssessmentItemId = table.Column<Guid>(
                    type: "uuid",
                    nullable: false),
                DecisionSnapshotId = table.Column<Guid>(
                    type: "uuid",
                    nullable: false),
                SkillId = table.Column<string>(
                    type: "character varying(200)",
                    maxLength: 200,
                    nullable: false),
                QuestionFamily = table.Column<string>(
                    type: "character varying(240)",
                    maxLength: 240,
                    nullable: false),
                Representation = table.Column<string>(
                    type: "character varying(120)",
                    maxLength: 120,
                    nullable: true),
                MathematicalComplexityScore = table.Column<int>(
                    type: "integer",
                    nullable: false),
                UiDifficultyBand = table.Column<int>(
                    type: "integer",
                    nullable: false),
                MisconceptionFocusId = table.Column<string>(
                    type: "character varying(200)",
                    maxLength: 200,
                    nullable: true),
                IsIndependentConfirmation = table.Column<bool>(
                    type: "boolean",
                    nullable: false,
                    defaultValue: false),
                PresentedAtUtc = table.Column<DateTime>(
                    type: "timestamp with time zone",
                    nullable: false),
                AnsweredAtUtc = table.Column<DateTime>(
                    type: "timestamp with time zone",
                    nullable: true),
                SubmittedAnswer = table.Column<string>(
                    type: "character varying(2000)",
                    maxLength: 2000,
                    nullable: true),
                IsCorrect = table.Column<bool>(
                    type: "boolean",
                    nullable: true),
                Score = table.Column<decimal>(
                    type: "numeric(10,2)",
                    precision: 10,
                    scale: 2,
                    nullable: true),
                Feedback = table.Column<string>(
                    type: "character varying(8000)",
                    maxLength: 8000,
                    nullable: true),
                ResponseDurationMs = table.Column<long>(
                    type: "bigint",
                    nullable: true),
                ExposureFingerprint = table.Column<string>(
                    type: "character varying(128)",
                    maxLength: 128,
                    nullable: false),
                SemanticIdentityKey = table.Column<string>(
                    type: "character varying(256)",
                    maxLength: 256,
                    nullable: false),
                RowVersion = table.Column<byte[]>(
                    type: "bytea",
                    nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey(
                    "PK_AdaptivePracticeTurns",
                    x => x.Id);
                table.UniqueConstraint(
                    "AK_AdaptivePracticeTurns_SchoolId_Id",
                    x => new { x.SchoolId, x.Id });
                table.ForeignKey(
                    name: "FK_AdaptivePracticeTurns_AdaptiveDecisionSnapshots_SchoolId_~",
                    columns: x => new
                    {
                        x.SchoolId,
                        x.DecisionSnapshotId
                    },
                    principalTable: "AdaptiveDecisionSnapshots",
                    principalColumns: new[] { "SchoolId", "Id" },
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_AdaptivePracticeTurns_AdaptivePracticeSessions_SchoolId_S~",
                    columns: x => new
                    {
                        x.SchoolId,
                        x.SessionId
                    },
                    principalTable: "AdaptivePracticeSessions",
                    principalColumns: new[] { "SchoolId", "Id" },
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_AdaptivePracticeTurns_AssessmentItems_SchoolId_Assessment~",
                    columns: x => new
                    {
                        x.SchoolId,
                        x.AssessmentItemId
                    },
                    principalTable: "AssessmentItems",
                    principalColumns: new[] { "SchoolId", "Id" },
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateIndex(
            name: "IX_AdaptiveDecisionSnapshots_SchoolId_SessionId_Sequence",
            table: "AdaptiveDecisionSnapshots",
            columns: new[] { "SchoolId", "SessionId", "Sequence" },
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_AdaptivePracticeSessions_CurriculumPedagogicalLessonId",
            table: "AdaptivePracticeSessions",
            column: "CurriculumPedagogicalLessonId");

        migrationBuilder.CreateIndex(
            name: "IX_AdaptivePracticeSessions_SchoolId_CurriculumAdoptionId",
            table: "AdaptivePracticeSessions",
            columns: new[] { "SchoolId", "CurriculumAdoptionId" });

        migrationBuilder.CreateIndex(
            name: "IX_AdaptivePracticeSessions_SchoolId_Status_StartedAtUtc",
            table: "AdaptivePracticeSessions",
            columns: new[] { "SchoolId", "Status", "StartedAtUtc" });

        migrationBuilder.CreateIndex(
            name: "IX_AdaptivePracticeSessions_SchoolId_StudentProfileId_StartedAtUtc",
            table: "AdaptivePracticeSessions",
            columns: new[] { "SchoolId", "StudentProfileId", "StartedAtUtc" });

        migrationBuilder.CreateIndex(
            name: "IX_AdaptivePracticeTurns_SchoolId_AssessmentItemId",
            table: "AdaptivePracticeTurns",
            columns: new[] { "SchoolId", "AssessmentItemId" });

        migrationBuilder.CreateIndex(
            name: "IX_AdaptivePracticeTurns_SchoolId_DecisionSnapshotId",
            table: "AdaptivePracticeTurns",
            columns: new[] { "SchoolId", "DecisionSnapshotId" });

        migrationBuilder.CreateIndex(
            name: "IX_AdaptivePracticeTurns_SchoolId_SessionId_ExposureFingerprint",
            table: "AdaptivePracticeTurns",
            columns: new[] { "SchoolId", "SessionId", "ExposureFingerprint" },
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_AdaptivePracticeTurns_SchoolId_SessionId_Sequence",
            table: "AdaptivePracticeTurns",
            columns: new[] { "SchoolId", "SessionId", "Sequence" },
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_StudentMisconceptionStates_SchoolId_CurriculumAdoptionId",
            table: "StudentMisconceptionStates",
            columns: new[] { "SchoolId", "CurriculumAdoptionId" });

        migrationBuilder.CreateIndex(
            name: "IX_StudentMisconceptionStates_SchoolId_StudentProfileId_Curriculum~",
            table: "StudentMisconceptionStates",
            columns: new[]
            {
                "SchoolId",
                "StudentProfileId",
                "CurriculumAdoptionId",
                "SkillId",
                "MisconceptionId"
            },
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_StudentRepresentationFluencyStates_SchoolId_StudentProfileId_Ski~",
            table: "StudentRepresentationFluencyStates",
            columns: new[]
            {
                "SchoolId",
                "StudentProfileId",
                "SkillId",
                "Representation"
            },
            unique: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "AdaptivePracticeTurns");

        migrationBuilder.DropTable(
            name: "StudentMisconceptionStates");

        migrationBuilder.DropTable(
            name: "StudentRepresentationFluencyStates");

        migrationBuilder.DropTable(
            name: "AdaptiveDecisionSnapshots");

        migrationBuilder.DropTable(
            name: "AdaptivePracticeSessions");
    }
}
