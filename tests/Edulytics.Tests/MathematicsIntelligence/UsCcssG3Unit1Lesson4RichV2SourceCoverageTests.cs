using Edulytics.Core.Curriculum;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Stage A-B-C source-specific assertions, authored only; NEVER run here.</summary>
public sealed class UsCcssG3Unit1Lesson4RichV2SourceCoverageTests
{
    private const string Code = "PED:US-CCSS-MATH:G3:U01:L04";

    [Fact]
    public void SourceSixCategoriesAndTeacherPrivateSurveyArePreserved()
    {
        var rich=RichLessonContentV2Registry.Find(Code,"en");
        Assert.NotNull(rich);
        var text=string.Join(" ",rich.ExplanationParagraphs);
        Assert.Contains("car (C), train (T), boat (B), balloon (Bal), plane (P) and helicopter (H)",text);
        Assert.Contains("teacher-controlled paper table",text);
        Assert.Contains("NOT store classmates' names",text);
        Assert.Contains("two-to-one",text);
    }

    [Fact]
    public void OriginalSynthesisGraphChangesAndCorrectChronologyAreExplicit()
    {
        var rich=RichLessonContentV2Registry.Find(Code,"en");
        Assert.NotNull(rich);
        var text=rich.ExplanationParagraphs.Last();
        Assert.Contains("BEFORE Cool-down",text);
        Assert.Contains("TWO more chose Balloon",text);
        Assert.Contains("ONE more chose Car",text);
        Assert.Contains(rich.KeyConcepts,x=>x.Title=="Synthesis — two more for Balloon");
        Assert.Contains(rich.KeyConcepts,x=>x.Title=="Synthesis — one more for Car");
        Assert.Contains(rich.WorkedExamples,x=>x.Check.Contains("3 × 2 + 1 = 7",StringComparison.Ordinal));
    }

    [Fact]
    public void SourceHumanWorkIsNotClaimedVerifiedByNumbersOrSourceArtwork()
    {
        var rich=RichLessonContentV2Registry.Find(Code,"en");
        Assert.NotNull(rich);
        Assert.Contains(rich.SummaryPoints,x=>x.Contains("without uploading names",StringComparison.Ordinal));
        Assert.Contains(rich.SummaryPoints,x=>x.Contains("Student Task Statements PDF remain",StringComparison.Ordinal));
        Assert.Empty(rich.Visuals);
        Assert.Empty(rich.Videos);
    }
}
