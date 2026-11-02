using Edulytics.Core.Curriculum;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>Stage A/B/C checks AUTHORED ONLY; do not execute yet.</summary>
public sealed class UsCcssG3Unit1Lesson6RichV2SourceCoverageTests
{
    private const string Code="PED:US-CCSS-MATH:G3:U01:L06";
    [Fact]
    public void SourceSpecificRichBodyPreserves2021NumericalTruth()
    {
        var rich=RichLessonContentV2Registry.Find(Code,"en");
        Assert.NotNull(rich);
        Assert.Equal("Choose a Scale",rich.Title);
        var body=string.Join(" ",rich.ExplanationParagraphs);
        Assert.Contains("Cat 16, Dog 13, Guinea Pig 6, Goldfish 10, Python 30",body);
        Assert.Contains("26 DISTINCT visible green triangles, 5 orange squares, 15 red trapezoids and 10 yellow hexagons",body);
        Assert.Contains("Mai's scale 2, Noah's scale 5 or Priya's scale 10",body);
        Assert.Contains("Winter 24, Spring 13, Summer 40, Fall 22",body);
    }
    [Fact]
    public void CooldownIsOriginalOpenWritingAndCommunityComesAfter()
    {
        var rich=RichLessonContentV2Registry.Find(Code,"en");
        Assert.NotNull(rich);
        var timeline=rich.ExplanationParagraphs.Last();
        Assert.Contains("BEFORE Cool-down",timeline);
        Assert.Contains("TWO SEPARATE OPEN writing questions",timeline);
        Assert.Contains("AFTER Cool-down",timeline);
        Assert.Contains("NOT be replaced by a graded fictional-club number drill",timeline);
    }
    [Fact]
    public void ExamplesCoverOriginalTaskFamiliesAndNoArtworkIsMislabelled()
    {
        var rich=RichLessonContentV2Registry.Find(Code,"en");
        Assert.NotNull(rich);
        Assert.Equal(4,rich.WorkedExamples.Count);
        Assert.Contains(rich.WorkedExamples,x=>x.Check.Contains("26+5+15+10=56",StringComparison.Ordinal));
        Assert.Contains(rich.WorkedExamples,x=>x.Check.Contains("24+13+40+22=99",StringComparison.Ordinal));
        Assert.All(rich.WorkedExamples,x=>Assert.True(x.Steps.Count>=4));
        Assert.Empty(rich.Visuals);
        Assert.Empty(rich.Videos);
    }
}
