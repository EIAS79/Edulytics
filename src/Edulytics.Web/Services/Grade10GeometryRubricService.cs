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

// Course-owned G10 Geometry teacher-scored rubrics: deliberately separate from
// machine-graded Practice and private mastery/analytics.
public sealed record G10GeoRubricRow(
    Guid Id, Guid SchoolId, Guid ClassGroupId, Guid StudentProfileId,
    Guid LessonId, string ResponseText, DateTime SubmittedAtUtc,
    Guid? TeacherUserId, int? TeacherScore, string? TeacherFeedback,
    DateTime? ReviewedAtUtc);
public sealed record G10GeoRubricStudentPage(
    Guid LessonId, Guid CurriculumAdoptionId, G10GeoRubricRow? Existing);
public sealed record G10GeoRubricTeacherPage(
    G10GeoRubricRow Submission, string[] Criteria);

internal static class G10GeoRubricStorage
{
    public const string Prefix = "PED:US-CCSS-MATH:HS:TRAD:GEO:IM-1E:";
    public const string AdoptionLevelKey = "US-CCSS-MATH:L10:TRADITIONAL:COURSE-GEO";
    public static readonly string[] Criteria =
    [
        "The lesson-specific mathematical model, construction or event space is represented accurately",
        "Calculation, construction steps or mathematical notation are correct",
        "The learner justifies the result, proof or probability assumptions",
        "Labels, explanation and conclusions meet the lesson's communication goals"
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

    private static G10GeoRubricRow Read(DbDataReader r) => new(
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
          FROM "Grade10GeometryTeacherRubrics"
        """;

    public static async Task<G10GeoRubricRow?> FindAsync(
        EdulyticsDbContext db, Guid id, CancellationToken ct)
    {
        await using var cmd = await NewCommand(db, Fields + " WHERE \"Id\"=@id",
            ct, ("id", id));
        await using var reader = await cmd.ExecuteReaderAsync(ct);
        return await reader.ReadAsync(ct) ? Read(reader) : null;
    }

    public static async Task<G10GeoRubricRow?> FindStudentAsync(
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
            INSERT INTO "Grade10GeometryTeacherRubrics"
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
        EdulyticsDbContext db, G10GeoRubricRow submission,
        Guid teacher, int[] marks, string feedback, CancellationToken ct)
    {
        // Six teacher-awarded binary points, never an automatic numeric scorer.
        if (marks.Length != 4 || marks.Any(x => x is not 0 and not 1))
            return false;
        await using var cmd = await NewCommand(db, """
            UPDATE "Grade10GeometryTeacherRubrics"
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

    public static async Task<IReadOnlyList<G10GeoRubricRow>> QueueAsync(
        EdulyticsDbContext db, Guid school, Guid group, CancellationToken ct)
    {
        await using var cmd = await NewCommand(db,
            Fields + """
                 WHERE "SchoolId"=@school AND "ClassGroupId"=@group
                 ORDER BY "SubmittedAtUtc" DESC LIMIT 500
                """, ct, ("school", school), ("group", group));
        await using var reader = await cmd.ExecuteReaderAsync(ct);
        var records = new List<G10GeoRubricRow>();
        while (await reader.ReadAsync(ct))
            records.Add(Read(reader));
        return records;
    }
}

public sealed class Grade10GeometryRubricService(EdulyticsDbContext db)
{
    public Task<G10GeoRubricRow?> FindAsync(Guid id, CancellationToken ct) =>
        G10GeoRubricStorage.FindAsync(db, id, ct);

    public Task<G10GeoRubricRow?> FindStudentAsync(
        Guid school, Guid group, Guid student, Guid lesson, CancellationToken ct) =>
        G10GeoRubricStorage.FindStudentAsync(db, school, group, student, lesson, ct);

    public Task<bool> InsertAsync(
        Guid school, Guid group, Guid student, Guid lesson,
        string response, CancellationToken ct) =>
        G10GeoRubricStorage.InsertAsync(db, school, group, student, lesson, response, ct);

    public Task<bool> GradeAsync(
        G10GeoRubricRow submission, Guid teacher, int[] marks,
        string feedback, CancellationToken ct) =>
        G10GeoRubricStorage.GradeAsync(db, submission, teacher, marks, feedback, ct);

    public Task<IReadOnlyList<G10GeoRubricRow>> QueueAsync(
        Guid school, Guid group, CancellationToken ct) =>
        G10GeoRubricStorage.QueueAsync(db, school, group, ct);

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
                  lesson.Id == lessonId && lesson.Code.StartsWith(G10GeoRubricStorage.Prefix) &&
                  adoption.CurriculumLevelKey == G10GeoRubricStorage.AdoptionLevelKey
            orderby e.EnrolledAtUtc descending
            select new { p.SchoolId, ClassGroupId = cls.Id, StudentId = p.Id }
        ).FirstOrDefaultAsync(ct);
        return row is null ? null : (row.SchoolId, row.ClassGroupId, row.StudentId);
    }

    public async Task<bool> MayReviewAsync(
        Guid actor, G10GeoRubricRow row, CancellationToken ct)
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
                  lesson.Id == row.LessonId && lesson.Code.StartsWith(G10GeoRubricStorage.Prefix) &&
                  adoption.CurriculumLevelKey == G10GeoRubricStorage.AdoptionLevelKey
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
