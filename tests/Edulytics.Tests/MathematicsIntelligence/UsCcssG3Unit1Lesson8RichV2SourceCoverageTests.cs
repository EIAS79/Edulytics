using Edulytics.Core.Curriculum;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>Authored Stage A-C assertions; not executed until Stage D.</summary>
public sealed class UsCcssG3Unit1Lesson8RichV2SourceCoverageTests
{
    private const string Code="PED:US-CCSS-MATH:G3:U01:L08";
    [Fact]
    public void RichV2ContainsFirstEditionTwoStepAndThreeReads()
    {
        var r=RichLessonContentV2Registry.Find(Code,"en");
        Assert.NotNull(r);
        var p=string.Join(" ",r.ExplanationParagraphs);
        Assert.Contains("Excited 55, Nervous 23 and Curious 25",p);
        Assert.Contains("Oak 29, Pine 82, Maple 15, Fir 58",p);
        Assert.Contains("55−23=32",p);
        Assert.Contains("82−(29+15)=38",p);
        Assert.Contains("three reasoning stages",p);
    }
    [Fact]
    public void OriginalApproximateCooldownNeverPromotedToExactGrading()
    {
        var r=RichLessonContentV2Registry.Find(Code,"en");
        Assert.NotNull(r);
        var p=r.ExplanationParagraphs.Last();
        Assert.Contains("Morning ABOUT 11–12",p);
        Assert.Contains("NEVER fixed-passed",p);
        Assert.Contains("BEFORE Cool-down",p);
        Assert.Equal(4,r.WorkedExamples.Count);
        Assert.Contains(r.WorkedExamples,x=>x.Check.Contains("23−11=12",StringComparison.Ordinal));
        Assert.Empty(r.Visuals);
        Assert.Empty(r.Videos);
    }
}
