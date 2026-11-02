using Edulytics.Web.Presentation;

namespace Edulytics.Tests.Acceptance;

public sealed class StudentLessonSlugTests
{
    [Fact]
    public void CambridgeLesson_UsesStableCurriculumCodeAndReadableTitle()
    {
        var slug = StudentLessonSlug.Create(
            "PED:CAMBRIDGE-INTL-MATH:S4:4F-2:APPLY",
            "Equivalent proper fractions: Reason and Apply");

        Assert.Equal(
            "cambridge-intl-math-s4-4f-2-apply--equivalent-proper-fractions-reason-and-apply",
            slug);
        Assert.Equal("cambridge-intl-math-s4-4f-2-apply",
            StudentLessonSlug.GetCodeToken(slug));
    }

    [Fact]
    public void SameTitle_DifferentCurricula_ProduceDifferentSlugs()
    {
        const string title = "Equivalent fractions";
        var cambridge = StudentLessonSlug.Create(
            "PED:CAMBRIDGE-INTL-MATH:S4:4F-2:APPLY", title);
        var commonCore = StudentLessonSlug.Create(
            "PED:US-CCSS-MATH:G4:U01:L01", title);

        Assert.NotEqual(cambridge, commonCore);
    }

    [Fact]
    public void CodeToken_RemainsStableWhenLessonTitleChanges()
    {
        var first = StudentLessonSlug.Create(
            "PED:PL-NATIONAL-MATH:L4:01", "Ułamki równoważne");
        var second = StudentLessonSlug.Create(
            "PED:PL-NATIONAL-MATH:L4:01", "Equivalent fractions");

        Assert.Equal(
            StudentLessonSlug.GetCodeToken(first),
            StudentLessonSlug.GetCodeToken(second));
        Assert.Contains("ulamki-rownowazne", first, StringComparison.Ordinal);
    }

    [Theory]
    [InlineData("")]
    [InlineData("some-title")]
    [InlineData("--missing-code")]
    [InlineData("code--")]
    [InlineData("code---title")]
    [InlineData("Code--TITLE")]
    public void MalformedSlugs_AreNotRoutable(string slug) =>
        Assert.Null(StudentLessonSlug.GetCodeToken(slug));

    [Fact]
    public void ArabicTitle_PreservesReadableCharacters()
    {
        var slug = StudentLessonSlug.Create(
            "PED:UAE-MOE-MATH:L4:COMMON:14:09", "فهم التناظر");
        Assert.Contains("فهم-التناظر", slug, StringComparison.Ordinal);
        Assert.Equal("uae-moe-math-l4-common-14-09",
            StudentLessonSlug.GetCodeToken(slug));
    }
}
