using System.Reflection;
using Edulytics.Core.Curriculum;
using Edulytics.Core.Lessons;
using Edulytics.Services.LessonContent;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Prevent the UAE framework's global English academic-language default from
/// silently overriding Grade 1 Common's independently authored Arabic lessons.
/// Runtime HTTPS/DB E2E remains the authoritative rendered-UI check.
/// </summary>
public sealed class UaeGrade1ArabicStudentLocaleTests
{
    private static readonly MethodInfo Selector =
        typeof(LessonContentService).GetMethod(
            "SelectAcademicContent", BindingFlags.NonPublic | BindingFlags.Static)
        ?? throw new InvalidOperationException("Canonical academic language selector missing");

    private static CanonicalLessonTranslationRecord Content(
        string culture, string title) =>
        new(culture, title, "Explanation", "Concepts", "Examples",
            "Steps", "Mistakes", "Summary");

    private static CanonicalLessonTranslationRecord? Choose(
        string packCode, string lessonCode,
        IReadOnlyList<CanonicalLessonTranslationRecord> translations) =>
        (CanonicalLessonTranslationRecord?)Selector.Invoke(
            null, [translations, packCode, lessonCode]);

    [Theory]
    [InlineData("PED:UAE-MOE-MATH:L1:COMMON:01:01")]
    [InlineData("PED:UAE-MOE-MATH:L1:COMMON:05:01")]
    [InlineData("PED:UAE-MOE-MATH:L1:COMMON:08:01")]
    [InlineData("PED:UAE-MOE-MATH:L1:COMMON:11:04")]
    public void Grade1Common_RequiresArabicRatherThanUaeFrameworkEnglish(
        string lessonCode)
    {
        var translations = new[]
        {
            Content("en", "English placeholder"),
            Content("ar", "تعليم الرياضيات للصف الأول")
        };
        var chosen = Choose(MathematicsCurriculumPackRegistry.UaeCode,
            lessonCode, translations);

        Assert.NotNull(chosen);
        Assert.Equal("ar", chosen.CultureCode);
        Assert.Equal("تعليم الرياضيات للصف الأول", chosen.Title);
    }

    [Fact]
    public void MissingArabicGrade1Body_DoesNotFallbackToEnglish()
    {
        const string code = "PED:UAE-MOE-MATH:L1:COMMON:01:01";
        Assert.Null(Choose(MathematicsCurriculumPackRegistry.UaeCode,
            code, [Content("en", "English only")]));
    }

    [Theory]
    [InlineData("PED:UAE-MOE-MATH:L2:COMMON:01:01")]
    [InlineData("PED:UAE-MOE-MATH:L2:COMMON:05:01")]
    [InlineData("PED:UAE-MOE-MATH:L2:COMMON:12:05")]
    public void Grade2Common_UsesReviewedArabicAcademicContent(string code)
    {
        var chosen = Choose(MathematicsCurriculumPackRegistry.UaeCode,
            code, [Content("en", "English placeholder"), Content("ar", "رياضيات الصف الثاني")]);
        Assert.NotNull(chosen);
        Assert.Equal("ar", chosen.CultureCode);
        Assert.Equal("رياضيات الصف الثاني", chosen.Title);
    }

    [Fact]
    public void MissingArabicGrade2Body_DoesNotLeakEnglishFallback()
    {
        const string code = "PED:UAE-MOE-MATH:L2:COMMON:01:01";
        Assert.Null(Choose(MathematicsCurriculumPackRegistry.UaeCode,
            code, [Content("en", "Unapproved English fallback")]));
    }

    [Theory]
    [InlineData("PED:UAE-MOE-MATH:L3:COMMON:01:01")]
    [InlineData("PED:UAE-MOE-MATH:L3:COMMON:14:10")]
    public void Grade3Common_UsesArabicAndCannotLeakEnglish(string code)
    {
        var selected = Choose(MathematicsCurriculumPackRegistry.UaeCode,
            code, [Content("en", "Unapproved English fallback"), Content("ar", "??? ???? ????")]);
        Assert.NotNull(selected);
        Assert.Equal("ar", selected.CultureCode);
        Assert.Null(Choose(MathematicsCurriculumPackRegistry.UaeCode,
            code, [Content("en", "Unapproved English fallback")]));
    }

    [Theory]
    [InlineData("PED:UAE-MOE-MATH:L11:ADVANCED:05:01:MULTIVARIABLE-LINEAR-SYSTEMS-AND-ELEMENTARY-ROW-OPERATIONS")]
    public void OtherUaeGrades_KeepExistingEnglishDefault(string code)
    {
        var chosen = Choose(MathematicsCurriculumPackRegistry.UaeCode,
            code, [Content("en", "Unchanged"), Content("ar", "العربية")]);

        Assert.Equal("en", chosen?.CultureCode);
    }

    [Fact]
    public void AmericanSchoolGrade1_IsUnchanged()
    {
        var chosen = Choose(MathematicsCurriculumPackRegistry.CommonCoreCode,
            "PED:US-CCSS-MATH:G1:U01:L01",
            [Content("en", "Unchanged"), Content("ar", "العربية")]);

        Assert.Equal("en", chosen?.CultureCode);
    }
}
