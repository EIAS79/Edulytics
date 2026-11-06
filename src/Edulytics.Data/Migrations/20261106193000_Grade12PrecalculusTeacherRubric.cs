using Edulytics.Data.Contexts;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Edulytics.Data.Migrations;

// G12 Precalculus Section 1.1 exercise-scoped teacher-graded work, isolated by school, class and learner. No changes to the
// shared PracticeService, automatic scoring pipeline, or mastery evidence.
[DbContext(typeof(EdulyticsDbContext))]
[Migration("20261106193000_Grade12PrecalculusTeacherRubric")]
public sealed class Grade12PrecalculusTeacherRubric : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Grade12PrecalculusTeacherRubrics",
            columns: t => new
            {
                Id = t.Column<Guid>(type: "uuid", nullable: false),
                SchoolId = t.Column<Guid>(type: "uuid", nullable: false),
                ClassGroupId = t.Column<Guid>(type: "uuid", nullable: false),
                StudentProfileId = t.Column<Guid>(type: "uuid", nullable: false),
                CurriculumPedagogicalLessonId = t.Column<Guid>(type: "uuid", nullable: false),
                ExerciseNumber = t.Column<int>(type: "integer", nullable: false),
                ResponseText = t.Column<string>(type: "character varying(8000)", maxLength: 8000, nullable: false),
                SubmittedAtUtc = t.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                TeacherUserId = t.Column<Guid>(type: "uuid", nullable: true),
                CriteriaJson = t.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                TeacherScore = t.Column<int>(type: "integer", nullable: true),
                TeacherFeedback = t.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                ReviewedAtUtc = t.Column<DateTime>(type: "timestamp with time zone", nullable: true)
            },
            constraints: t =>
            {
                t.PrimaryKey("PK_Grade12PrecalculusTeacherRubrics", x => x.Id);
                t.ForeignKey("FK_G12PrecalcRubric_School", x => x.SchoolId,
                    "Schools", "Id", onDelete: ReferentialAction.Restrict);
                t.ForeignKey("FK_G12PrecalcRubric_Lesson", x => x.CurriculumPedagogicalLessonId,
                    "CurriculumPedagogicalLessons", "Id", onDelete: ReferentialAction.Restrict);
                t.ForeignKey("FK_G12PrecalcRubric_Student",
                    x => new { x.SchoolId, x.StudentProfileId },
                    "StudentProfiles", new[] { "SchoolId", "Id" },
                    onDelete: ReferentialAction.Restrict);
            });
        migrationBuilder.CreateIndex(
            name: "UX_G12PrecalcRubric_SchoolClassStudentLesson",
            table: "Grade12PrecalculusTeacherRubrics",
            columns: new[] { "SchoolId", "ClassGroupId", "StudentProfileId", "CurriculumPedagogicalLessonId", "ExerciseNumber" },
            unique: true);
        migrationBuilder.CreateIndex(
            name: "IX_G12PrecalcRubric_TeacherQueue",
            table: "Grade12PrecalculusTeacherRubrics",
            columns: new[] { "SchoolId", "ClassGroupId", "ReviewedAtUtc" });
        migrationBuilder.Sql("""
            ALTER TABLE "Grade12PrecalculusTeacherRubrics"
            ADD CONSTRAINT "CK_G12PrecalcRubric_ReviewConsistent"
            CHECK (
                ("TeacherUserId" IS NULL AND "CriteriaJson" IS NULL
                 AND "TeacherScore" IS NULL AND "TeacherFeedback" IS NULL
                 AND "ReviewedAtUtc" IS NULL)
                OR
                ("TeacherUserId" IS NOT NULL AND "CriteriaJson" IS NOT NULL
                 AND "TeacherScore" BETWEEN 0 AND 4
                 AND "TeacherFeedback" IS NOT NULL
                 AND "ReviewedAtUtc" IS NOT NULL)
            );
            """);
        migrationBuilder.Sql("""
            ALTER TABLE "Grade12PrecalculusTeacherRubrics"
            ADD CONSTRAINT "CK_G12PrecalcRubric_ExerciseNumberRange"
            CHECK ("ExerciseNumber" BETWEEN 1 AND 92);
            """);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
        => migrationBuilder.DropTable(name: "Grade12PrecalculusTeacherRubrics");
}
