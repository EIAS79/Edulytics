using Edulytics.Core.Curriculum;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Authored Stage A–C evidence only. Tests are deliberately NOT executed;
/// source image/PDF, deployed runtime and academic acceptance remain separate gates.
/// </summary>
public sealed class UsCcssG3Unit1Lesson1RichV2SourceCoverageTests
{
    private const string Code = "PED:US-CCSS-MATH:G3:U01:L01";

    [Fact]
    public void RichV2RegistryContainsSpecificFirstEditionTeachingBody()
    {
        var rich = RichLessonContentV2Registry.Find(Code, "en");
        Assert.NotNull(rich);
        Assert.Equal("Make Sense of Data", rich.Title);
        Assert.True(rich.ExplanationParagraphs.Count >= 5);
        var body = string.Join(" ", rich.ExplanationParagraphs);
        Assert.Contains("2021 first-edition", body);
        Assert.Contains("4, 3, 1, 2, 7 and 6", body);
        Assert.Contains("Walk 4, Bike 3, Skateboard 1, Car 2, Bus 7, Train 6", body);
        Assert.Contains("0 through 8 in increments of 1", body);
        Assert.Contains("title 'How We Get Home' but NOT the category labels", body);
        Assert.Contains("five-minute", string.Join(" ", rich.SummaryPoints));
    }

    [Fact]
    public void SourceOpenResponsesAreNotRepresentedAsNumericPasses()
    {
        var rich = RichLessonContentV2Registry.Find(Code, "en");
        Assert.NotNull(rich);
        Assert.Contains(rich.KeyConcepts, item =>
            item.Title == "Answerable data questions");
        Assert.Contains(rich.CommonMistakes, item =>
            item.Mistake.Contains("Substituting a correct numeric response",
                StringComparison.Ordinal));
        Assert.Contains(rich.SummaryPoints, point =>
            point.Contains("Five distinct source Student Facing subparts",
                StringComparison.Ordinal));
        Assert.Contains(rich.ExplanationParagraphs, paragraph =>
            paragraph.Contains("teacher evaluation, not a numeric pass",
                StringComparison.Ordinal));
        Assert.DoesNotContain(rich.Visuals, visual =>
            visual.Title.Contains("original publisher", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void WorkedExamplesPreserveRevealOrderAndSelfConsistentMath()
    {
        var rich = RichLessonContentV2Registry.Find(Code, "en");
        Assert.NotNull(rich);
        Assert.Equal(3, rich.WorkedExamples.Count);
        Assert.StartsWith("Warm-up", rich.WorkedExamples[0].Title);
        Assert.StartsWith("Activity 1", rich.WorkedExamples[1].Title);
        Assert.StartsWith("Activity 2", rich.WorkedExamples[2].Title);
        Assert.Contains("7 − 4 = 3", rich.WorkedExamples[0].Check);
        Assert.Contains("7 − 1 = 6", rich.WorkedExamples[2].Check);
        Assert.Contains("7 + 6 = 13", rich.WorkedExamples[2].Check);
        Assert.All(rich.WorkedExamples, example =>
        {
            Assert.True(example.Steps.Count >= 3);
            Assert.False(string.IsNullOrWhiteSpace(example.Answer));
            Assert.False(string.IsNullOrWhiteSpace(example.Check));
        });
    }

    [Fact]
    public void OtherGradesDoNotInheritTheGrade3RichBody()
    {
        var candidate = RichLessonContentV2Registry.Find(
            "PED:US-CCSS-MATH:G3:U01:L02", "en");
        Assert.True(candidate is null ||
            !candidate.ExplanationParagraphs.Any(x =>
                x.Contains("The numbers of symbols", StringComparison.Ordinal)));
    }
}
