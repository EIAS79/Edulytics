using Edulytics.Data.Contexts;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Edulytics.Data.Migrations;

// Lesson-owned, independently persisted educator scoring. No changes to the
// shared PracticeService, automatic scoring pipeline, or mastery evidence.
[DbContext(typeof(EdulyticsDbContext))]
[Migration("20261102023000_Grade5U03L02TeacherRubric")]
public sealed class Grade5U03L02TeacherRubric : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Grade5U03L02TeacherRubrics",
            columns: t => new
            {
                Id = t.Column<Guid>(type: "uuid", nullable: false),
                SchoolId = t.Column<Guid>(type: "uuid", nullable: false),
                ClassGroupId = t.Column<Guid>(type: "uuid", nullable: false),
                StudentProfileId = t.Column<Guid>(type: "uuid", nullable: false),
                CurriculumPedagogicalLessonId = t.Column<Guid>(type: "uuid", nullable: false),
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
                t.PrimaryKey("PK_Grade5U03L02TeacherRubrics", x => x.Id);
                t.ForeignKey("FK_G5U03L02Rubric_School", x => x.SchoolId,
                    "Schools", "Id", onDelete: ReferentialAction.Restrict);
                t.ForeignKey("FK_G5U03L02Rubric_Lesson", x => x.CurriculumPedagogicalLessonId,
                    "CurriculumPedagogicalLessons", "Id", onDelete: ReferentialAction.Restrict);
                t.ForeignKey("FK_G5U03L02Rubric_Student",
                    x => new { x.SchoolId, x.StudentProfileId },
                    "StudentProfiles", new[] { "SchoolId", "Id" },
                    onDelete: ReferentialAction.Restrict);
            });
        migrationBuilder.CreateIndex(
            name: "UX_G5U03L02Rubric_SchoolClassStudentLesson",
            table: "Grade5U03L02TeacherRubrics",
            columns: new[] { "SchoolId", "ClassGroupId", "StudentProfileId", "CurriculumPedagogicalLessonId" },
            unique: true);
        migrationBuilder.CreateIndex(
            name: "IX_G5U03L02Rubric_TeacherQueue",
            table: "Grade5U03L02TeacherRubrics",
            columns: new[] { "SchoolId", "ClassGroupId", "ReviewedAtUtc" });
        migrationBuilder.Sql("""
            ALTER TABLE "Grade5U03L02TeacherRubrics"
            ADD CONSTRAINT "CK_G5U03L02Rubric_ReviewConsistent"
            CHECK (
                ("TeacherUserId" IS NULL AND "CriteriaJson" IS NULL
                 AND "TeacherScore" IS NULL AND "TeacherFeedback" IS NULL
                 AND "ReviewedAtUtc" IS NULL)
                OR
                ("TeacherUserId" IS NOT NULL AND "CriteriaJson" IS NOT NULL
                 AND "TeacherScore" BETWEEN 0 AND 6
                 AND "TeacherFeedback" IS NOT NULL
                 AND "ReviewedAtUtc" IS NOT NULL)
            );
            """);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
        => migrationBuilder.DropTable(name: "Grade5U03L02TeacherRubrics");
}
