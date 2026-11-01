using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// G5 U03:L03 AUTHORING ONLY, TESTS AUTHORED_NOT_RUN.
/// The course-local candidate is NOT wired into the shared Practice engine.
/// </summary>
public sealed class UsCcssG5U03L03UnitFractionCandidateTests
{
    private static Dictionary<string, int> P(int scenario, int slot = 0, int reversed = 0) =>
        new(StringComparer.Ordinal)
        {
            ["case"] = scenario, ["slot"] = slot, ["reversed"] = reversed
        };

    [Theory]
    [InlineData(0, 0, "1")] // 4 columns × 2 rows = 1/8
    [InlineData(1, 1, "2")] // 4 × 3 = 1/12
    [InlineData(2, 2, "3")] // 4 × 4 = 1/16
    [InlineData(3, 0, "1")] // 4 × 5 = 1/20
    [InlineData(4, 1, "2")] // predicted next 4 × 6 = 1/24
    public void OriginalFourColumnPattern_UsesTheOriginalWhole(
        int sourceCase, int slot, string expected)
    {
        var p = P(sourceCase, slot);
        Assert.Equal(expected,
            UsCcssG5U03L03UnitFractionQuestions.Solve(
                UsCcssG5U03L03UnitFractionQuestions.PatternFamily, p));
        Assert.True(UsCcssG5U03L03UnitFractionQuestions.Verify(
            UsCcssG5U03L03UnitFractionQuestions.PatternFamily, p, expected));
        Assert.False(UsCcssG5U03L03UnitFractionQuestions.Verify(
            UsCcssG5U03L03UnitFractionQuestions.PatternFamily, p, "0"));
    }

    [Theory]
    [InlineData(0, 0, "1")] // source 1/5 × 1/3 = 1/15
    [InlineData(1, 1, "2")] // source 1/2 × 1/6 = 1/12
    [InlineData(2, 2, "3")] // source 1/4 × 1/6 = 1/24
    public void SourceActivityTwo_ExactProductsHaveBoundedChoices(
        int sourceCase, int slot, string expected)
    {
        var family = UsCcssG5U03L03UnitFractionQuestions.EquationFamily;
        var p = P(sourceCase, slot);
        Assert.Equal(expected, UsCcssG5U03L03UnitFractionQuestions.Solve(family, p));
        Assert.True(UsCcssG5U03L03UnitFractionQuestions.Verify(family, p, expected));
        Assert.False(UsCcssG5U03L03UnitFractionQuestions.Verify(family, p,
            expected == "1" ? "2" : "1"));
        var reversed = P(sourceCase, slot, 1);
        Assert.Equal(expected, UsCcssG5U03L03UnitFractionQuestions.Solve(family, reversed));
    }

    [Fact]
    public void MalformedForeignAndOutOfDomainParametersFailClosed()
    {
        foreach (var family in new[]
        {
            UsCcssG5U03L03UnitFractionQuestions.PatternFamily,
            UsCcssG5U03L03UnitFractionQuestions.EquationFamily
        })
        {
            foreach (var p in new[]
            {
                P(-1), P(5), P(0, 3), P(0, -1), P(0, 0, 2), P(0, 0, -1)
            })
                Assert.False(UsCcssG5U03L03UnitFractionQuestions.Verify(family, p, "1"));
            var extra = P(0);
            extra["otherLesson"] = 1;
            Assert.False(UsCcssG5U03L03UnitFractionQuestions.Verify(family, extra, "1"));
            Assert.False(UsCcssG5U03L03UnitFractionQuestions.Verify(family, P(0), "1/8"));
            Assert.False(UsCcssG5U03L03UnitFractionQuestions.Verify(family, P(0), " 1"));
            Assert.False(UsCcssG5U03L03UnitFractionQuestions.Verify(family, null, "1"));
        }
        Assert.False(UsCcssG5U03L03UnitFractionQuestions.Supports(
            UsCcssG5U03L03UnitFractionQuestions.PatternFamily + ".foreign"));
    }

    [Fact]
    public void WrittenSourceReasoningRemainsTeacherReviewed()
    {
        foreach (var family in new[]
        {
            UsCcssG5U03L03UnitFractionQuestions.PatternFamily,
            UsCcssG5U03L03UnitFractionQuestions.EquationFamily
        })
        {
            var prompt = UsCcssG5U03L03UnitFractionQuestions.Build(family,new Random(7));
            Assert.Contains("ORIGINAL", prompt.Prompt);
            Assert.Contains("TEACHER_REVIEW_REQUIRED", prompt.Prompt);
            Assert.Contains("not an assessment of the open source drawing", prompt.Solution);
        }
    }
}
