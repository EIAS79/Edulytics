using Edulytics.Core.Mathematics.Practice;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class PracticeAssessmentComposerTests
{
    [Fact]
    public void ShapeStandardBlueprintUsesOnlyStandardCapableForms()
    {
        var blueprint = new PracticeAssessmentComposer().Compose(
            ["supporting.geometry.shape_dimension"],
            10,
            PracticeCognitiveDifficulty.Standard);

        Assert.Equal(10, blueprint.RequestedQuestionCount);
        Assert.Equal(8, blueprint.Candidates.Count);
        Assert.Equal(
            new[]
            {
                PracticeQuestionForm.Identify,
                PracticeQuestionForm.Classify
            },
            blueprint.Forms.OrderBy(x => x).ToArray());

        Assert.DoesNotContain(
            blueprint.Candidates,
            item => item.Form is
                PracticeQuestionForm.ErrorAnalysis or
                PracticeQuestionForm.Transfer);
        Assert.All(
            blueprint.Candidates,
            item => Assert.Equal(
                PracticeCognitiveDifficulty.Standard,
                item.PlannedDifficulty));
    }

    [Fact]
    public void ShapeChallengeBlueprintUsesOnlyChallengeCapableForms()
    {
        var blueprint = new PracticeAssessmentComposer().Compose(
            ["supporting.geometry.shape_dimension"],
            10,
            PracticeCognitiveDifficulty.Challenge);

        Assert.Equal(8, blueprint.Candidates.Count);
        Assert.Equal(
            new[]
            {
                PracticeQuestionForm.ErrorAnalysis,
                PracticeQuestionForm.Transfer
            },
            blueprint.Forms.OrderBy(x => x).ToArray());

        Assert.All(
            blueprint.Candidates,
            item => Assert.Equal(
                PracticeCognitiveDifficulty.Challenge,
                item.PlannedDifficulty));
    }

    [Fact]
    public void MultipleFamiliesAreInterleavedBeforeAnyFamilyRepeats()
    {
        var families = new[]
        {
            "supporting.powers10.evaluate",
            "supporting.powers10.multiply",
            "supporting.powers10.divide",
            "supporting.powers10.missing_exponent"
        };

        var blueprint = new PracticeAssessmentComposer().Compose(
            families,
            10,
            PracticeCognitiveDifficulty.Standard);

        Assert.True(blueprint.Candidates.Count >= 10);
        Assert.Equal(
            families,
            blueprint.Candidates
                .Take(families.Length)
                .Select(item => item.Family)
                .ToArray());

        Assert.Equal(
            families.Length,
            blueprint.Candidates
                .Take(families.Length)
                .Select(item => item.Family)
                .Distinct(StringComparer.Ordinal)
                .Count());
    }
}
