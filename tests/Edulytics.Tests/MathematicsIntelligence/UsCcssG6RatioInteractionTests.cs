using Edulytics.Core.Mathematics.Practice;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class UsCcssG6RatioInteractionTests
{
    [Theory]
    [InlineData("usccss.g6.u02.l01.ordered_collection_ratios", "exact_ordered_ratio")]
    [InlineData("usccss.g6.u02.l02.labeled_two_row_ratios", "exact_ordered_ratio")]
    [InlineData("usccss.g6.u02.l02.three_ingredient_ratio_cards", "exact_ordered_ratio_three_term")]
    public void OriginalOrderedRatioFamiliesResolveToStructuredEntry(
        string family, string expectedAnswerType)
    {
        Assert.True(LessonPracticeInteractionRegistry.TryResolve(family, out var interaction));
        Assert.NotNull(interaction);
        Assert.Equal(expectedAnswerType, interaction.AnswerType);
        Assert.Equal(LessonPracticeInteractionKind.StructuredEntry, interaction.InteractionKind);
    }

    [Fact]
    public void Grade1InteractionsRemainResolvableAfterGrade6RegistryIntegration()
    {
        Assert.True(LessonPracticeInteractionRegistry.All.Count > 1000);
        Assert.True(LessonPracticeInteractionRegistry.TryResolve(
            "usccss.g1.u02.story.compare_unknowns_equations", out var interaction));
        Assert.NotNull(interaction);
        Assert.Equal(LessonPracticeInteractionKind.ContractDriven, interaction.InteractionKind);
    }
}
