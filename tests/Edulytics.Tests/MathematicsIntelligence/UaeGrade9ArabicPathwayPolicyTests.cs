using System.Reflection;
using Edulytics.Core.Curriculum;
using Edulytics.Core.Lessons;
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
}
