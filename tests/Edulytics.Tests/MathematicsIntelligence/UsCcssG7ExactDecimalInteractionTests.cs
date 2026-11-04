using Edulytics.Core.Mathematics.Practice;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class UsCcssG7ExactDecimalInteractionTests
{
    [Theory]
    [InlineData("usccss.g7.u01.l07.drawing_actual_distance_units")]
    [InlineData("usccss.g7.u01.l08.map_route_rate_time")]
    public void DecimalScaleAndMapFamiliesRenderNumericEntry(string familyId)
    {
        Assert.True(LessonPracticeInteractionRegistry.TryResolve(familyId, out var interaction));
        Assert.NotNull(interaction);
        Assert.Equal("exact_decimal", interaction.AnswerType);
        Assert.Equal(LessonPracticeInteractionKind.NumericEntry, interaction.InteractionKind);
    }
}