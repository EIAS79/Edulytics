using Edulytics.Web.Presentation;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// UAE Grade 1's canonical Arabic texts already contain authored teaching
/// models. Generic English fallback diagrams must not replace or supplement
/// those mathematically specific models on learner pages.
/// </summary>
public sealed class UaeGrade1ArabicReaderPresentationTests
{
    [Theory]
    [InlineData("أعد ثلاث نقاط وأكتب العدد ثلاثة.", "explanation")]
    [InlineData("مثال: عندي خمس نقاط. أعدها واحدة واحدة.", "examples")]
    public void ArabicCanonicalSource_DoesNotInjectGenericEnglishDiagrams(
        string arabicSource, string sectionKind)
    {
        var parsed = LessonPresentationParser.Parse(
            arabicSource,
            sectionKind: sectionKind,
            includeFallbackVisual: false);

        Assert.NotEmpty(parsed);
        Assert.All(parsed, item => Assert.False(item.IsVisual));
        Assert.Contains(parsed, item => item.Text?.Contains("نقاط") is true);
    }

    [Theory]
    [InlineData("A number is a quantity represented with numerals.", "explanation")]
    [InlineData("Example: count the shapes.", "examples")]
    public void ExistingOtherCurricula_KeepTheirDefaultVisualFallback(
        string source, string sectionKind)
    {
        var parsed = LessonPresentationParser.Parse(
            source,
            sectionKind: sectionKind);

        Assert.Contains(parsed, item => item.IsVisual);
    }
}
