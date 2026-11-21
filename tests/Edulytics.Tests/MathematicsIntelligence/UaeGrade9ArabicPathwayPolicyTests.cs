using System.Reflection;
using System.Text.Json;
using Edulytics.Core.Curriculum;
using Edulytics.Core.Lessons;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.LessonContent;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Tests Grade 9 General Arabic student/staff selection policy, not authenticated
/// HTTPS, DOM screenshots, school isolation or Stage D source certification.
/// </summary>
public sealed class UaeGrade9ArabicPathwayPolicyTests
{
    private const string Framework = MathematicsCurriculumPackRegistry.UaeCode;
    private static readonly MethodInfo Selector =
        typeof(LessonContentService).GetMethod(
            "SelectAcademicContent", BindingFlags.NonPublic | BindingFlags.Static)
        ?? throw new InvalidOperationException("Language selector missing");

    private static CanonicalLessonTranslationRecord Content(string culture, string title) =>
        new(culture, title, "Explanation", "Concepts", "Examples", "Steps", "Mistakes", "Summary");

    private static CanonicalLessonTranslationRecord? Choose(
        string framework, string code,
        params CanonicalLessonTranslationRecord[] available) =>
        (CanonicalLessonTranslationRecord?)Selector.Invoke(null, [available, framework, code]);

    [Theory]
    [InlineData("PED:UAE-MOE-MATH:L9:GENERAL:01:01:VARIABLES-AND-EXPRESSIONS")]
    [InlineData("PED:UAE-MOE-MATH:L9:GENERAL:01:02:ORDER-OF-OPERATIONS")]
    public void DraftG9GeneralIsRejectedEvenIfEnglishOrArabicContentExists(string code)
    {
        Assert.True(LessonContentPolicy.IsUaeGrade9GeneralScope(Framework, code));
        Assert.False(LessonContentPolicy.IsUaeG9ArabicLesson(Framework, code));
        Assert.False(LessonContentPolicy.CanExposeStudentLesson(Framework, false, code));
        Assert.False(LessonContentPolicy.CanExposeStudentLesson(Framework, true, code));
        Assert.Null(Choose(Framework, code, Content("en", "Old English"), Content("ar", "Unapproved draft")));
    }

    [Theory]
    [InlineData("PED:UAE-MOE-MATH:L9:GENERAL:01:01:UNREVIEWED")]
    [InlineData("PED:UAE-MOE-MATH:L9:GENERAL:07:01:UNREVIEWED")]
    public void LegacyUnknownGrade9EnglishRecordFailsClosed(string code)
    {
        Assert.True(LessonContentPolicy.IsUaeGrade9GeneralScope(Framework, code));
        Assert.False(LessonContentPolicy.CanExposeStudentLesson(Framework, false, code));
        Assert.Null(Choose(Framework, code, Content("en", "Legacy English")));
    }

    [Fact]
    public void G7ArabicAndUsEnglishRemainSelectedIndependently()
    {
        Assert.False(LessonContentPolicy.IsUaeGrade9GeneralScope(
            Framework, "PED:UAE-MOE-MATH:L9:APPLIED:01:01:EXAMPLE"));
        Assert.False(LessonContentPolicy.IsUaeGrade9GeneralScope(
            Framework, "PED:UAE-MOE-MATH:L9:ADVANCED:01:01:EXAMPLE"));
        Assert.False(LessonContentPolicy.IsUaeGrade9GeneralScope(
            MathematicsCurriculumPackRegistry.CommonCoreCode,
            "PED:UAE-MOE-MATH:L9:GENERAL:01:01:VARIABLES-AND-EXPRESSIONS"));
        const string g7 = "PED:UAE-MOE-MATH:L7:GENERAL:01:01:RATES";
        Assert.True(LessonContentPolicy.IsUaeG7ArabicLesson(Framework, g7));
        Assert.Equal("ar", Choose(Framework, g7,
            Content("en", "Old English"), Content("ar", "Arabic Grade7"))?.CultureCode);
        Assert.Equal("en", Choose(MathematicsCurriculumPackRegistry.CommonCoreCode,
            "PED:US-CCSS-MATH:G1:U01:L01",
            Content("en", "US English"), Content("ar", "Wrong Arabic"))?.CultureCode);
    }

    public static IEnumerable<object[]> All82AuthoringGrade9Codes()
    {
        // The repository's authored scope is 82, NOT the 86 raw book entries.
        // Locate the checked-in draft pack on both the local developer machine
        // and an isolated CI agent without referencing production databases.
        const string packPath = "src/Edulytics.Core/Curriculum/LessonContent/Packs/" +
                                "uae-g9-general-ar-2025-student-u01l01.lesson-content-pack.json";
        var startDirectories = new[] { Directory.GetCurrentDirectory(), AppContext.BaseDirectory };
        string? source = null;
        foreach (var start in startDirectories)
        {
            for (var dir = new DirectoryInfo(start); dir is not null; dir = dir.Parent)
            {
                var path = Path.Combine(dir.FullName, packPath);
                if (File.Exists(path))
                {
                    source = path;
                    break;
                }
            }
            if (source is not null)
                break;
        }

        if (source is null)
            throw new InvalidOperationException("Required G9 82-lesson authoring pack not located");

        using var json = JsonDocument.Parse(File.ReadAllText(source));
        var lessons = json.RootElement.GetProperty("Lessons")
            .EnumerateArray()
            .Select(x => x.GetProperty("LessonCode").GetString()
                ?? throw new InvalidOperationException("G9 LessonCode cannot be null"))
            .ToArray();
        if (lessons.Length != 82 || lessons.Distinct(StringComparer.Ordinal).Count() != 82)
            throw new InvalidOperationException("G9 authored 82-lesson identity baseline changed unexpectedly");
        foreach (var lesson in lessons)
            yield return new object[] { lesson };
    }

    private const string G9DigitalReleaseToken =
        "CONFIRM_RELEASE_UAE_GRADE9_GENERAL_80_DIGITAL_PRACTICE";

    [Theory]
    [InlineData("PED:UAE-MOE-MATH:L9:GENERAL:13:07:CONGRUENCE-TRANSFORMATIONS")]
    [InlineData("PED:UAE-MOE-MATH:L9:GENERAL:13:08:TRIANGLES-AND-COORDINATE-PROOF")]
    public void ExactTeacherOnlyG9ArabicPagesCanEnterReleaseWithoutAutograding(
        string lessonCode)
    {
        Assert.True(LessonContentPolicy.IsUaeGrade9GeneralScope(Framework, lessonCode));
        Assert.True(LessonContentPolicy.IsUaeG9TeacherOnlyArabicLesson(
            Framework, lessonCode, G9DigitalReleaseToken));
        Assert.False(LessonContentPolicy.IsUaeG9TeacherOnlyArabicLesson(
            Framework, lessonCode, null));
        Assert.False(LessonContentPolicy.IsUaeG9TeacherOnlyArabicLesson(
            Framework, lessonCode, "WRONG_RELEASE_TOKEN"));
        Assert.False(LessonContentPolicy.IsUaeG9TeacherOnlyArabicLesson(
            MathematicsCurriculumPackRegistry.CommonCoreCode, lessonCode,
            G9DigitalReleaseToken));
        Assert.False(LessonPracticeContractRegistry.TryResolve(lessonCode, out _));
    }

    [Theory]
    [InlineData("PED:UAE-MOE-MATH:L9:GENERAL:13:06:SIMILARITY")]
    [InlineData("PED:UAE-MOE-MATH:L9:GENERAL:13:09:PARALLELOGRAM-TRIANGLE-AREA")]
    [InlineData("PED:UAE-MOE-MATH:L9:GENERAL:13:07:UNREVIEWED")]
    [InlineData("PED:UAE-MOE-MATH:L9:ADVANCED:13:07:CONGRUENCE-TRANSFORMATIONS")]
    public void TeacherOnlyG9ContentReleaseDoesNotWhitelistAnyOtherLesson(
        string lessonCode)
    {
        Assert.False(LessonContentPolicy.IsUaeG9TeacherOnlyArabicLesson(
            Framework, lessonCode, G9DigitalReleaseToken));
    }

    [Theory]
    [MemberData(nameof(All82AuthoringGrade9Codes))]
    public void EveryUnapprovedG9AuthoredIdentityRejectsStudentFallback(
        string lessonCode)
    {
        Assert.True(LessonContentPolicy.IsUaeGrade9GeneralScope(Framework, lessonCode));
        Assert.False(LessonContentPolicy.IsUaeG9ArabicLesson(Framework, lessonCode));
        Assert.False(LessonContentPolicy.CanExposeStudentLesson(Framework, false, lessonCode));
        Assert.False(LessonContentPolicy.CanExposeStudentLesson(Framework, true, lessonCode));
        Assert.Null(Choose(Framework, lessonCode,
            Content("en", "Legacy English content"),
            Content("ar", "Unverified Arabic draft")));
    }

}
