using System.Reflection;
using Edulytics.Core.Curriculum;
using Edulytics.Core.Lessons;
using Edulytics.Services.LessonContent;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Verifies the Grade 7 General Arabic route itself, not only JSON metadata.
/// DB migrations, real role authentication, HTTPS and RTL remain separate Stage D gates.
/// </summary>
public sealed class UaeGrade7ArabicPathwayPolicyTests
{
    private static readonly MethodInfo Selector =
        typeof(LessonContentService).GetMethod(
            "SelectAcademicContent", BindingFlags.NonPublic | BindingFlags.Static)
        ?? throw new InvalidOperationException("Language selector missing");

    private static CanonicalLessonTranslationRecord Content(
        string language, string title) =>
        new(language, title, "Explanation", "Concepts",
            "Examples", "Steps", "Mistakes", "Summary");

    private static CanonicalLessonTranslationRecord? Choose(
        string framework, string code,
        params CanonicalLessonTranslationRecord[] available) =>
        (CanonicalLessonTranslationRecord?)Selector.Invoke(
            null, [available, framework, code]);

    [Theory]
    [InlineData("PED:UAE-MOE-MATH:L7:GENERAL:01:01:RATES")]
    [InlineData("PED:UAE-MOE-MATH:L7:GENERAL:08:04:PRISM-VOLUME-BASE-AREA-TIMES-HEIGHT")]
    [InlineData("PED:UAE-MOE-MATH:L7:GENERAL:11:05:SELECT-APPROPRIATE-DATA-DISPLAY")]
    public void ApprovedGrade7ArabicLesson_IsSelectableButNeverEnglish(
        string code)
    {
        const string framework = MathematicsCurriculumPackRegistry.UaeCode;
        Assert.True(LessonContentPolicy.IsUaeG7ArabicLesson(framework, code));
        Assert.True(LessonContentPolicy.CanExposeStudentLesson(
            framework, true, code));
        Assert.Equal("ar", Choose(framework, code,
            Content("en", "Old English fallback"),
            Content("ar", "الشرح العربي المعتمد"))?.CultureCode);
        Assert.Null(Choose(framework, code,
            Content("en", "Old English fallback")));
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void OldEnglishGrade7General_IsRejectedEvenWhenPublished(
        bool isSupporting)
    {
        const string framework = MathematicsCurriculumPackRegistry.UaeCode;
        const string legacy =
            "PED:UAE-MOE-MATH:L7:GENERAL:01:01:UNIT-RATES-INVOLVING-RATIOS-OF-FRACTIONS";
        Assert.True(LessonContentPolicy.IsUaeGrade7GeneralScope(framework, legacy));
        Assert.False(LessonContentPolicy.IsUaeG7ArabicLesson(framework, legacy));
        Assert.False(LessonContentPolicy.CanExposeStudentLesson(
            framework, isSupporting, legacy));
        Assert.Null(Choose(framework, legacy,
            Content("en", "Legacy English content"),
            Content("ar", "Unreviewed Arabic")));
    }

    [Fact]
    public void AmericanAndOtherUaePathwaysKeepTheirExistingPolicy()
    {
        const string american = "PED:US-CCSS-MATH:G1:U01:L01";
        Assert.Equal("en", Choose(MathematicsCurriculumPackRegistry.CommonCoreCode,
            american, Content("en", "English"), Content("ar", "العربية"))?.CultureCode);
        const string advanced = "PED:UAE-MOE-MATH:L7:ADVANCED:01:01:EXAMPLE";
        Assert.False(LessonContentPolicy.IsUaeGrade7GeneralScope(
            MathematicsCurriculumPackRegistry.UaeCode, advanced));
    }
}
