using System.Data;
using System.Data.Common;
using System.Security.Claims;
using System.Text.Json;
using Edulytics.Core.Constants;
using Edulytics.Data.Contexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Edulytics.Web.Controllers;

// Course-owned G11 Algebra 2 teacher-scored rubrics: deliberately separate from
// machine-graded Practice and private mastery/analytics.
public sealed record G11Alg2RubricRow(
    Guid Id, Guid SchoolId, Guid ClassGroupId, Guid StudentProfileId,
    Guid LessonId, string ResponseText, DateTime SubmittedAtUtc,
    Guid? TeacherUserId, int? TeacherScore, string? TeacherFeedback,
    DateTime? ReviewedAtUtc);
public sealed record G11Alg2RubricStudentPage(
    Guid LessonId, Guid CurriculumAdoptionId, G11Alg2RubricRow? Existing);
public sealed record G11Alg2RubricTeacherPage(
    G11Alg2RubricRow Submission, string[] Criteria);

internal static class G11Alg2RubricStorage
{
    public const string Prefix = "PED:US-CCSS-MATH:HS:TRAD:ALG2:IM-1E:";
    public const string AdoptionLevelKey = "US-CCSS-MATH:L11:TRADITIONAL:COURSE-ALG2";
    public static readonly string[] Criteria =
    [
        "Mathematical functions, variables, equations, data or graphical models are represented accurately",
        "Calculations, symbolic transformations and stated constraints are mathematically correct",
        "A complete explanation justifies the reasoning, domain assumptions, and conclusions",
        "Written labels, units, interpretation and any teacher-inspected evidence meet the lesson goals"
    ];

    private static DbParameter P(DbCommand command, string name, object? value)
    {
        var parameter = command.CreateParameter();
        parameter.ParameterName = name;
        parameter.Value = value ?? DBNull.Value;
        command.Parameters.Add(parameter);
        return parameter;
    }

    private static async Task<DbCommand> NewCommand(
        EdulyticsDbContext db, string sql, CancellationToken ct, params (string, object?)[] args)
    {
        await db.Database.OpenConnectionAsync(ct);
        var command = db.Database.GetDbConnection().CreateCommand();
        command.CommandText = sql;
        foreach (var (name, value) in args)
            P(command, name, value);
        return command;
    }

    private static G11Alg2RubricRow Read(DbDataReader r) => new(
        r.GetGuid(0), r.GetGuid(1), r.GetGuid(2), r.GetGuid(3), r.GetGuid(4),
        r.GetString(5), r.GetDateTime(6),
        r.IsDBNull(7) ? null : r.GetGuid(7),
        r.IsDBNull(8) ? null : r.GetInt32(8),
        r.IsDBNull(9) ? null : r.GetString(9),
        r.IsDBNull(10) ? null : r.GetDateTime(10));

    private const string Fields = """
        SELECT "Id","SchoolId","ClassGroupId","StudentProfileId",
               "CurriculumPedagogicalLessonId","ResponseText","SubmittedAtUtc",
               "TeacherUserId","TeacherScore","TeacherFeedback","ReviewedAtUtc"
          FROM "Grade11Algebra2TeacherRubrics"
        """;

    public static async Task<G11Alg2RubricRow?> FindAsync(
        EdulyticsDbContext db, Guid id, CancellationToken ct)
    {
        await using var cmd = await NewCommand(db, Fields + " WHERE \"Id\"=@id",
            ct, ("id", id));
        await using var reader = await cmd.ExecuteReaderAsync(ct);
        return await reader.ReadAsync(ct) ? Read(reader) : null;
    }

    public static async Task<G11Alg2RubricRow?> FindStudentAsync(
        EdulyticsDbContext db, Guid school, Guid group, Guid student,
        Guid lesson, CancellationToken ct)
    {
        await using var cmd = await NewCommand(db,
            Fields + """
                 WHERE "SchoolId"=@school AND "ClassGroupId"=@group
                   AND "StudentProfileId"=@student
                   AND "CurriculumPedagogicalLessonId"=@lesson
                """, ct, ("school", school), ("group", group),
            ("student", student), ("lesson", lesson));
        await using var reader = await cmd.ExecuteReaderAsync(ct);
        return await reader.ReadAsync(ct) ? Read(reader) : null;
    }

    public static async Task<bool> InsertAsync(
        EdulyticsDbContext db, Guid school, Guid group, Guid student,
        Guid lesson, string response, CancellationToken ct)
    {
        await using var cmd = await NewCommand(db, """
            INSERT INTO "Grade11Algebra2TeacherRubrics"
            ("Id","SchoolId","ClassGroupId","StudentProfileId",
             "CurriculumPedagogicalLessonId","ResponseText","SubmittedAtUtc")
            VALUES (@id,@school,@group,@student,@lesson,@response,@submitted)
            ON CONFLICT ("SchoolId","ClassGroupId","StudentProfileId",
                         "CurriculumPedagogicalLessonId") DO NOTHING
            """, ct, ("id", Guid.NewGuid()), ("school", school), ("group", group),
            ("student", student), ("lesson", lesson),
            ("response", response), ("submitted", DateTime.UtcNow));
        return await cmd.ExecuteNonQueryAsync(ct) == 1;
    }

    public static async Task<bool> GradeAsync(
        EdulyticsDbContext db, G11Alg2RubricRow submission,
        Guid teacher, int[] marks, string feedback, CancellationToken ct)
    {
        // Four teacher-awarded binary points, never an automatic numeric scorer.
        if (marks.Length != 4 || marks.Any(x => x is not 0 and not 1))
            return false;
        await using var cmd = await NewCommand(db, """
            UPDATE "Grade11Algebra2TeacherRubrics"
               SET "TeacherUserId"=@teacher,
                   "CriteriaJson"=@criteria,
                   "TeacherScore"=@score,
                   "TeacherFeedback"=@feedback,
                   "ReviewedAtUtc"=@at
             WHERE "Id"=@id AND "SchoolId"=@school
               AND "ClassGroupId"=@group AND "TeacherUserId" IS NULL
            """, ct, ("teacher", teacher),
            ("criteria", JsonSerializer.Serialize(marks)),
            ("score", marks.Sum()), ("feedback", feedback),
            ("at", DateTime.UtcNow), ("id", submission.Id),
            ("school", submission.SchoolId), ("group", submission.ClassGroupId));
        return await cmd.ExecuteNonQueryAsync(ct) == 1;
    }

    public static async Task<IReadOnlyList<G11Alg2RubricRow>> QueueAsync(
        EdulyticsDbContext db, Guid school, Guid group, CancellationToken ct)
    {
        await using var cmd = await NewCommand(db,
            Fields + """
                 WHERE "SchoolId"=@school AND "ClassGroupId"=@group
                 ORDER BY "SubmittedAtUtc" DESC LIMIT 500
                """, ct, ("school", school), ("group", group));
        await using var reader = await cmd.ExecuteReaderAsync(ct);
        var records = new List<G11Alg2RubricRow>();
        while (await reader.ReadAsync(ct))
            records.Add(Read(reader));
        return records;
    }
}

public sealed class Grade11Algebra2RubricService(EdulyticsDbContext db)
{
    public Task<G11Alg2RubricRow?> FindAsync(Guid id, CancellationToken ct) =>
        G11Alg2RubricStorage.FindAsync(db, id, ct);

    public Task<G11Alg2RubricRow?> FindStudentAsync(
        Guid school, Guid group, Guid student, Guid lesson, CancellationToken ct) =>
        G11Alg2RubricStorage.FindStudentAsync(db, school, group, student, lesson, ct);

    public Task<bool> InsertAsync(
        Guid school, Guid group, Guid student, Guid lesson,
        string response, CancellationToken ct) =>
        G11Alg2RubricStorage.InsertAsync(db, school, group, student, lesson, response, ct);

    public Task<bool> GradeAsync(
        G11Alg2RubricRow submission, Guid teacher, int[] marks,
        string feedback, CancellationToken ct) =>
        G11Alg2RubricStorage.GradeAsync(db, submission, teacher, marks, feedback, ct);

    public Task<IReadOnlyList<G11Alg2RubricRow>> QueueAsync(
        Guid school, Guid group, CancellationToken ct) =>
        G11Alg2RubricStorage.QueueAsync(db, school, group, ct);

    public async Task<(Guid School, Guid Class, Guid Student)?> StudentScopeAsync(
        Guid actor, Guid lessonId, Guid curriculumAdoptionId, CancellationToken ct)
    {
        var row = await (
            from p in db.StudentProfiles.AsNoTracking()
            join e in db.StudentEnrollments.AsNoTracking()
                on new { p.SchoolId, Student = p.Id }
                equals new { e.SchoolId, Student = e.StudentProfileId }
            join cls in db.ClassGroups.AsNoTracking()
                on new { e.SchoolId, e.ClassGroupId, e.AcademicYearId }
                equals new { cls.SchoolId, ClassGroupId = cls.Id, cls.AcademicYearId }
            join adoption in db.SchoolCurriculumAdoptions.AsNoTracking()
                on new { cls.SchoolId, Adopt = cls.CurriculumAdoptionId }
                equals new { adoption.SchoolId, Adopt = (Guid?)adoption.Id }
            join lesson in db.CurriculumPedagogicalLessons.AsNoTracking()
                on adoption.FrameworkVersionId equals lesson.FrameworkVersionId
            where p.UserId == actor && !p.IsArchived && adoption.IsActive &&
                  (curriculumAdoptionId == Guid.Empty || adoption.Id == curriculumAdoptionId) &&
                  lesson.Id == lessonId && lesson.Code.StartsWith(G11Alg2RubricStorage.Prefix) &&
                  adoption.CurriculumLevelKey == G11Alg2RubricStorage.AdoptionLevelKey
            orderby e.EnrolledAtUtc descending
            select new { p.SchoolId, ClassGroupId = cls.Id, StudentId = p.Id }
        ).FirstOrDefaultAsync(ct);
        return row is null ? null : (row.SchoolId, row.ClassGroupId, row.StudentId);
    }

    public async Task<bool> MayReviewAsync(
        Guid actor, G11Alg2RubricRow row, CancellationToken ct)
    {
        return await (
            from t in db.TeacherAssignments.AsNoTracking()
            join e in db.StudentEnrollments.AsNoTracking()
                on new { t.SchoolId, t.ClassGroupId, t.AcademicYearId }
                equals new { e.SchoolId, e.ClassGroupId, e.AcademicYearId }
            join cls in db.ClassGroups.AsNoTracking()
                on new { t.SchoolId, t.ClassGroupId }
                equals new { cls.SchoolId, ClassGroupId = cls.Id }
            join adoption in db.SchoolCurriculumAdoptions.AsNoTracking()
                on new { t.SchoolId, Adopt = cls.CurriculumAdoptionId }
                equals new { adoption.SchoolId, Adopt = (Guid?)adoption.Id }
            join lesson in db.CurriculumPedagogicalLessons.AsNoTracking()
                on adoption.FrameworkVersionId equals lesson.FrameworkVersionId
            where t.TeacherUserId == actor && t.SchoolId == row.SchoolId &&
                  t.ClassGroupId == row.ClassGroupId &&
                  t.SubjectId == adoption.SubjectId &&
                  e.StudentProfileId == row.StudentProfileId &&
                  lesson.Id == row.LessonId && lesson.Code.StartsWith(G11Alg2RubricStorage.Prefix) &&
                  adoption.CurriculumLevelKey == G11Alg2RubricStorage.AdoptionLevelKey
            select t.Id
        ).AnyAsync(ct);
    }

    public async Task<(Guid SchoolId, Guid ClassGroupId)[]> TeacherGroupsAsync(
        Guid actor, CancellationToken ct)
    {
        var groups = await db.TeacherAssignments.AsNoTracking()
            .Where(x => x.TeacherUserId == actor)
            .Select(x => new { x.SchoolId, x.ClassGroupId })
            .Distinct().ToArrayAsync(ct);
        return groups.Select(x => (x.SchoolId, x.ClassGroupId)).ToArray();
    }
}
