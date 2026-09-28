using Edulytics.Data.Contexts;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Edulytics.Data.Migrations;

[DbContext(typeof(EdulyticsDbContext))]
[Migration("20260928013000_AdaptivePracticeV2ShadowObservations")]
public sealed class AdaptivePracticeV2ShadowObservations : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "AdaptivePracticeShadowObservations",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                SchoolId = table.Column<Guid>(type: "uuid", nullable: false),
                StudentProfileId = table.Column<Guid>(type: "uuid", nullable: false),
                CurriculumAdoptionId = table.Column<Guid>(type: "uuid", nullable: false),
                CurriculumPedagogicalLessonId = table.Column<Guid>(type: "uuid", nullable: false),
                V1AttemptId = table.Column<Guid>(type: "uuid", nullable: false),
                V1AttemptItemId = table.Column<Guid>(type: "uuid", nullable: false),
                V1Sequence = table.Column<int>(type: "integer", nullable: false),
                V1WasCorrect = table.Column<bool>(type: "boolean", nullable: false),
                V1QuestionFamily = table.Column<string>(
                    type: "character varying(240)",
                    maxLength: 240,
                    nullable: true),
                V1Difficulty = table.Column<int>(type: "integer", nullable: false),
                ProposedSkillId = table.Column<string>(
                    type: "character varying(200)",
                    maxLength: 200,
                    nullable: false),
                ProposedComplexity = table.Column<int>(type: "integer", nullable: false),
                ProposedFamily = table.Column<string>(
                    type: "character varying(240)",
                    maxLength: 240,
                    nullable: false),
                ProposedRepresentation = table.Column<string>(
                    type: "character varying(120)",
                    maxLength: 120,
                    nullable: true),
                ProposedMisconceptionFocusId = table.Column<string>(
                    type: "character varying(200)",
                    maxLength: 200,
                    nullable: true),
                ObservedMisconceptionId = table.Column<string>(
                    type: "character varying(200)",
                    maxLength: 200,
                    nullable: true),
                DecisionReasonCode = table.Column<string>(
                    type: "character varying(100)",
                    maxLength: 100,
                    nullable: false),
                GenerationFeasible = table.Column<bool>(
                    type: "boolean",
                    nullable: false),
                EngineVersion = table.Column<string>(
                    type: "character varying(100)",
                    maxLength: 100,
                    nullable: false),
                PolicyVersion = table.Column<string>(
                    type: "character varying(100)",
                    maxLength: 100,
                    nullable: false),
                CreatedAtUtc = table.Column<DateTime>(
                    type: "timestamp with time zone",
                    nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey(
                    "PK_AdaptivePracticeShadowObservations",
                    x => x.Id);
                table.UniqueConstraint(
                    "AK_AdaptivePracticeShadowObservations_SchoolId_Id",
                    x => new { x.SchoolId, x.Id });
                table.ForeignKey(
                    name: "FK_AdaptivePracticeShadowObservations_CurriculumPedagogicalL~",
                    column: x => x.CurriculumPedagogicalLessonId,
                    principalTable: "CurriculumPedagogicalLessons",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_AdaptivePracticeShadowObservations_SchoolCurriculumAdopti~",
                    columns: x => new { x.SchoolId, x.CurriculumAdoptionId },
                    principalTable: "SchoolCurriculumAdoptions",
                    principalColumns: new[] { "SchoolId", "Id" },
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_AdaptivePracticeShadowObservations_StudentProfiles_School~",
                    columns: x => new { x.SchoolId, x.StudentProfileId },
                    principalTable: "StudentProfiles",
                    principalColumns: new[] { "SchoolId", "Id" },
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateIndex(
            name: "IX_AdaptivePracticeShadowObservations_CurriculumPedagogicalLessonId",
            table: "AdaptivePracticeShadowObservations",
            column: "CurriculumPedagogicalLessonId");

        migrationBuilder.CreateIndex(
            name: "IX_AdaptivePracticeShadowObservations_SchoolId_CurriculumAdoptionId",
            table: "AdaptivePracticeShadowObservations",
            columns: new[] { "SchoolId", "CurriculumAdoptionId" });

        migrationBuilder.CreateIndex(
            name: "IX_AdaptivePracticeShadowObservations_SchoolId_StudentProfileId_Cre~",
            table: "AdaptivePracticeShadowObservations",
            columns: new[] { "SchoolId", "StudentProfileId", "CreatedAtUtc" });

        migrationBuilder.CreateIndex(
            name: "IX_AdaptivePracticeShadowObservations_SchoolId_V1AttemptId_V1Attemp~",
            table: "AdaptivePracticeShadowObservations",
            columns: new[] { "SchoolId", "V1AttemptId", "V1AttemptItemId" },
            unique: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "AdaptivePracticeShadowObservations");
    }
}
