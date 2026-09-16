using Edulytics.Core.Mathematics.Solving;
using Edulytics.Services.Mathematics.Generation;
using Edulytics.Services.Mathematics.Solving;
using Edulytics.Services.Mathematics.Verification;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class ExactLinearEquationQuestionFactoryTests
{
    [Theory]
    [InlineData(1, 1)]
    [InlineData(42, 2)]
    [InlineData(-17, 3)]
    [InlineData(int.MaxValue, 3)]
    public void Generate_ProducesDeterministicSolverVerifiedProblem(int variantKey, int difficultyBand)
    {
        var factory = new ExactLinearEquationQuestionFactory(
            new ExactLinearEquationSolver(),
            new ExactLinearEquationVerifier());

        var first = factory.Generate(variantKey, difficultyBand);
        var second = factory.Generate(variantKey, difficultyBand);

        Assert.Equal(ExactLinearEquationQuestionFactory.FamilyId, first.QuestionFamilyId);
        Assert.Equal("algebra.linear.solve", first.Skill.Value);
        Assert.Equal(MathematicsSolveStatus.Solved, first.SolveResult.Status);
        Assert.True(first.Verification.IsVerified);
        Assert.Equal(first.Problem, second.Problem);
        Assert.Equal(first.ExpectedAnswer, second.ExpectedAnswer);
        Assert.Equal(
            first.Parameters.OrderBy(x => x.Key).ToArray(),
            second.Parameters.OrderBy(x => x.Key).ToArray());
    }

    [Fact]
    public void Generate_RejectsUnknownDifficultyBand()
    {
        var factory = new ExactLinearEquationQuestionFactory(
            new ExactLinearEquationSolver(),
            new ExactLinearEquationVerifier());

        Assert.Throws<ArgumentOutOfRangeException>(() => factory.Generate(1, 0));
        Assert.Throws<ArgumentOutOfRangeException>(() => factory.Generate(1, 4));
    }
}
