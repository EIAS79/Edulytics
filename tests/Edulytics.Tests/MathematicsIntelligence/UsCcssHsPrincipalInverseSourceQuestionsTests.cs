using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class UsCcssHsPrincipalInverseSourceQuestionsTests
{
    private static Dictionary<string, int> P(int mode) => new() { ["mode"] = mode };

    [Theory]
    [InlineData(0, "pi/6", "5pi/6")]
    [InlineData(1, "-pi/3", "4pi/3")]
    [InlineData(2, "2pi/3", "-2pi/3")]
    [InlineData(3, "pi/4", "5pi/4")]
    public void Coder9L02PrincipalInverseAcceptsSourceBranchAndRejectsWrongBranch(
        int mode, string correct, string incorrect)
    {
        var parameters = P(mode);
        Assert.Equal(correct, UsCcssHsPrincipalInverseSourceQuestions.Solve(
            UsCcssHsPrincipalInverseSourceQuestions.Family, parameters));
        Assert.True(ExactSkillContractQuestionEngine.Verify(
            UsCcssHsPrincipalInverseSourceQuestions.Family, parameters, correct));
        Assert.False(ExactSkillContractQuestionEngine.Verify(
            UsCcssHsPrincipalInverseSourceQuestions.Family, parameters, incorrect));
    }

    [Fact]
    public void PrincipalArcsinCompositionAcceptsEquivalentRadianDecimalButNotDegreeStrings()
    {
        var parameters = P(0);
        Assert.True(UsCcssHsPrincipalInverseSourceQuestions.Verify(
            UsCcssHsPrincipalInverseSourceQuestions.Family, parameters,
            "0.5235987755982988 rad"));
        Assert.False(UsCcssHsPrincipalInverseSourceQuestions.Verify(
            UsCcssHsPrincipalInverseSourceQuestions.Family, parameters, "30 degrees"));
    }

    [Theory]
    [InlineData(100, "OUT_OF_DOMAIN")]
    [InlineData(101, "OUT_OF_DOMAIN")]
    [InlineData(102, "WRONG_BRANCH")]
    public void InvalidRealDomainAndArctanBoundaryNeverScoreAsNumericallyCorrect(
        int mode, string status)
    {
        var parameters = P(mode);
        Assert.Equal(status, UsCcssHsPrincipalInverseSourceQuestions.Evaluate(parameters).Status);
        Assert.False(ExactSkillContractQuestionEngine.Verify(
            UsCcssHsPrincipalInverseSourceQuestions.Family, parameters, "pi/2"));
        Assert.Throws<ArgumentException>(() => UsCcssHsPrincipalInverseSourceQuestions.Solve(
            UsCcssHsPrincipalInverseSourceQuestions.Family, parameters));
    }

    [Fact]
    public void AllNineteenSourcePrincipalBranchExamplesAreValidAndVaried()
    {
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(
            UsCcssHsPrincipalInverseSourceQuestions.Family));
        var seen = new HashSet<string>(StringComparer.Ordinal);
        for (var mode = 0; mode < 19; mode++)
        {
            var parameters = P(mode);
            var answer = UsCcssHsPrincipalInverseSourceQuestions.Solve(
                UsCcssHsPrincipalInverseSourceQuestions.Family, parameters);
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                UsCcssHsPrincipalInverseSourceQuestions.Family, parameters, answer));
            seen.Add(answer);
        }
        Assert.True(seen.Count >= 8);
        for (var seed = 0; seed < 20; seed++)
        {
            var item = UsCcssHsPrincipalInverseSourceQuestions.Build(
                UsCcssHsPrincipalInverseSourceQuestions.Family, new Random(seed));
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                item.Family, item.Parameters,
                UsCcssHsPrincipalInverseSourceQuestions.Solve(item.Family, item.Parameters)));
        }
    }
}
