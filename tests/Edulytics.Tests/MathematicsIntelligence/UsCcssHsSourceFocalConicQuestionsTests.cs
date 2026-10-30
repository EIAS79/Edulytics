using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Mathematical oracles for all seven Coder 9 L05 source fixtures. No
/// teacher-reviewed drawing, authenticated grade persistence or approval.
/// </summary>
public sealed class UsCcssHsSourceFocalConicQuestionsTests
{
    private const string Family = UsCcssHsSourceFocalConicQuestions.Family;
    private static Dictionary<string, int> P(int hyperbola, int vertical, int cx,
        int cy, int a, int c) => new()
    {
        ["hyperbola"] = hyperbola, ["vertical"] = vertical,
        ["centerX"] = cx, ["centerY"] = cy,
        ["a"] = a, ["c"] = c
    };

    [Theory]
    [InlineData(0, 0, 0, 0, 5, 3, "x^2/25+y^2/16=1", 5, 0)]
    [InlineData(0, 1, 0, 0, 5, 3, "y^2/25+x^2/16=1", 0, 5)]
    [InlineData(0, 0, 2, -1, 5, 3, "(x-2)^2/25+(y+1)^2/16=1", 7, -1)]
    [InlineData(1, 0, 0, 0, 3, 5, "x^2/9-y^2/16=1", 3, 0)]
    [InlineData(1, 1, 0, 0, 3, 5, "y^2/9-x^2/16=1", 0, -3)]
    public void OriginalConicCasesRequireCanonicalEquationAndCorrectFocalDistance(
        int h, int v, int cx, int cy, int a, int c, string equation, double px, double py)
    {
        var p = P(h, v, cx, cy, a, c);
        Assert.Equal(16, UsCcssHsSourceFocalConicQuestions.Evaluate(p).BSquared);
        Assert.Equal(equation, UsCcssHsSourceFocalConicQuestions.Solve(Family, p));
        Assert.True(ExactSkillContractQuestionEngine.Verify(Family, p, equation));
        Assert.True(UsCcssHsSourceFocalConicQuestions.IsPointOnConic(p, px, py));
        Assert.False(UsCcssHsSourceFocalConicQuestions.IsPointOnConic(p, px + 100, py + 100));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family, p, "x^2/9+y^2/16=1"));
    }

    [Fact]
    public void EquivalentCommutativeEllipseTermsAcceptedButNotAHyperbolaSignChange()
    {
        var ellipse = P(0, 0, 0, 0, 5, 3);
        Assert.True(ExactSkillContractQuestionEngine.Verify(
            Family, ellipse, "y^2/16 + x^2/25 = 1"));
        var hyperbola = P(1, 0, 0, 0, 3, 5);
        Assert.False(ExactSkillContractQuestionEngine.Verify(
            Family, hyperbola, "y^2/16+x^2/9=1"));
        Assert.True(ExactSkillContractQuestionEngine.Verify(
            Family, hyperbola, "x^2/9 - y^2/16 = 1"));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    public void DegenerateConicsAreOutOfDomainAndNeverAutomaticallyGraded(int kind)
    {
        var p = P(kind, 0, 0, 0, 3, 3);
        var evaluation = UsCcssHsSourceFocalConicQuestions.Evaluate(p);
        Assert.Equal("OUT_OF_DOMAIN", evaluation.Status);
        Assert.Null(evaluation.Equation);
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family, p, "0"));
        Assert.Throws<ArgumentException>(() =>
            UsCcssHsSourceFocalConicQuestions.Solve(Family, p));
        Assert.False(UsCcssHsSourceFocalConicQuestions.IsPointOnConic(p, 3, 0));
    }

    [Fact]
    public void GeneratedConicQuestionsAreDiverseAndMathematicallyVerified()
    {
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(Family));
        var prompts = new HashSet<string>(StringComparer.Ordinal);
        for (var seed = 0; seed < 24; seed++)
        {
            var q = UsCcssHsSourceFocalConicQuestions.Build(Family, new Random(seed));
            var expected = UsCcssHsSourceFocalConicQuestions.Solve(Family, q.Parameters);
            Assert.True(ExactSkillContractQuestionEngine.Verify(Family, q.Parameters, expected));
            Assert.False(ExactSkillContractQuestionEngine.Verify(Family, q.Parameters, "not-a-conic"));
            prompts.Add(q.Prompt);
        }
        Assert.True(prompts.Count >= 10);
    }
}
