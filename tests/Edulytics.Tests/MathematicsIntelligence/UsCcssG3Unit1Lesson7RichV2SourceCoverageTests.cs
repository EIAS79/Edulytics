using Edulytics.Core.Curriculum;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>Authored Stage A-C contract, intentionally not run.</summary>
public sealed class UsCcssG3Unit1Lesson7RichV2SourceCoverageTests
{
    private const string Code="PED:US-CCSS-MATH:G3:U01:L07";
    [Fact]
    public void RichV2PreservesOriginalDotSportAndBugGraphMathematics()
    {
        var r=RichLessonContentV2Registry.Find(Code,"en");
        Assert.NotNull(r);
        var s=string.Join(" ",r.ExplanationParagraphs);
        Assert.Contains("seven and twenty-two dots",s);
        Assert.Contains("Winter 24, Spring 13, Summer 40, Fall 22",s);
        Assert.Contains("FOUR questions",s);
        Assert.Contains("FIVE distinct parts",s);
        Assert.Contains("Football 28, Soccer 61, Basketball 35, Hockey 22",s);
        Assert.Contains("61−28=33",s);
        Assert.Contains("35−22=13",s);
    }
    [Fact]
    public void EstimateRationaleAndSourceCooldownHaveDifferentGrading()
    {
        var r=RichLessonContentV2Registry.Find(Code,"en");
        Assert.NotNull(r);
        var a=r.ExplanationParagraphs.Single(x=>x.StartsWith("Activity 2 — Questions About Bugs in the Garden",StringComparison.Ordinal));
        Assert.Contains("NEVER the pass authority",a);
        Assert.Contains("Teacher",r.WorkedExamples[2].Check,StringComparison.OrdinalIgnoreCase);
        Assert.Contains("BEFORE the source Cool-down",r.ExplanationParagraphs.Last());
        Assert.Equal(4,r.WorkedExamples.Count);
        Assert.Empty(r.Visuals);
        Assert.Empty(r.Videos);
    }
}
