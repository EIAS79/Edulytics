using Edulytics.Core.Curriculum;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>Stage A–C source-contract tests authored only; never run here.</summary>
public sealed class UsCcssG3Unit1Lesson9RichV2SourceCoverageTests
{
    private const string Code="PED:US-CCSS-MATH:G3:U01:L09";
    [Fact]
    public void FirstEditionAvoidsPrematureSymbolicMultiplication()
    {
        var r=RichLessonContentV2Registry.Find(Code,"en");
        Assert.NotNull(r);
        var body=string.Join(" ",r.ExplanationParagraphs);
        Assert.Contains("NOT introduced in this lesson",body);
        Assert.Contains("Jada has 3 bags",body);
        Assert.Contains("FOUR people each wearing TWO shoes",body);
        Assert.Contains("TWO boxes with TEN markers",body);
        Assert.Contains("THREE basketball teams with FIVE",body);
    }
    [Fact]
    public void SignGraphAndOriginalBraceletCooldownHaveRepresentations()
    {
        var r=RichLessonContentV2Registry.Find(Code,"en");
        Assert.NotNull(r);
        Assert.Contains(r.WorkedExamples,x=>x.Check.Contains("3 groups each of 2 are 6",StringComparison.Ordinal));
        Assert.Contains(r.WorkedExamples,x=>x.Check.Contains("typed '15' alone",StringComparison.Ordinal));
        Assert.Contains(r.SummaryPoints,x=>x.Contains("source Cool-down Jada",StringComparison.Ordinal));
        Assert.Equal(4,r.WorkedExamples.Count);
        Assert.Empty(r.Visuals);
        Assert.Empty(r.Videos);
    }
}
