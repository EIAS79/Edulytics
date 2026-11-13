using System.Globalization;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

// Source: Grade 5 Arabic Unit 10 lesson 7, printed pp. 771-776.
// Exact generator tests authored; Stage D / CI explicitly not run.
public sealed class UaeGrade5Unit10Lesson07LogicalCluesTests
{
    [Theory]
    [InlineData("uae.g5.u10.l07.logical_remaining")]
    [InlineData("uae.g5.u10.l07.logical_original_total")]
    public void LogicClueFamiliesUseOrderingAndIndependentExpectedValue(string family)
    {
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
        var engine = new ExactSkillContractQuestionEngine();
        var cases = engine.Generate(
            "PED:UAE-MOE-MATH:L5:GENERAL:10:07",
            family, [family],
            ExactSkillQuestionDifficulty.Standard, 10, 20261113, []);
        foreach (var item in cases)
        {
            var p = item.Parameters;
            var a = p["a"];
            var b = p["b"];
            var c = p["c"];
            var expected = family.EndsWith("logical_remaining", StringComparison.Ordinal)
                ? a * 100 + b + c
                : a + b + c;
            Assert.Equal(expected.ToString(CultureInfo.InvariantCulture), item.CorrectAnswer);
            Assert.True(ExactSkillContractQuestionEngine.Verify(family, p, item.CorrectAnswer));
            Assert.False(ExactSkillContractQuestionEngine.Verify(family, p, "-999"));
        }
    }
}
