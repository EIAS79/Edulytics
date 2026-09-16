using System.Text.Json;
using Edulytics.Core.Mathematics.Ast;
using Edulytics.Core.Mathematics.Solving;
using Edulytics.Core.Mathematics.Verification;
using Edulytics.Services.Mathematics.Generation;
using Edulytics.Services.Mathematics.Solving;
using Edulytics.Services.Mathematics.Verification;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class ExactLinearInequalityQuestionFactoryTests
{
    [Theory]
    [InlineData(1, 1)]
    [InlineData(7, 1)]
    [InlineData(19, 2)]
    [InlineData(77, 3)]
    [InlineData(211, 3)]
    public void Generate_ProducesSolverVerifiedDeterministicProblem(int variantKey, int difficultyBand)
    {
        var factory = new ExactLinearInequalityQuestionFactory(
            new ExactLinearInequalitySolver(),
            new ExactLinearInequalityVerifier());

        var generated = factory.Generate(variantKey, difficultyBand);

        Assert.Equal(ExactLinearInequalityQuestionFactory.FamilyId, generated.QuestionFamilyId);
        Assert.Equal("algebra.linear.inequality.solve", generated.Skill.Value);
        Assert.IsType<InequalityNode>(generated.Problem);
        Assert.IsType<InequalityNode>(generated.ExpectedAnswer);
        Assert.Equal(MathematicsSolveStatus.Solved, generated.SolveResult.Status);
        Assert.Equal(MathematicsVerificationStatus.Verified, generated.Verification.Status);
        Assert.NotNull(generated.SolveResult.SolutionSet);
    }

    [Fact]
    public void Generate_SameVariantAndDifficulty_IsStable()
    {
        var factory = new ExactLinearInequalityQuestionFactory(
            new ExactLinearInequalitySolver(),
            new ExactLinearInequalityVerifier());

        var first = factory.Generate(12345, 2);
        var second = factory.Generate(12345, 2);

        Assert.Equal(
            JsonSerializer.Serialize(first.Problem),
            JsonSerializer.Serialize(second.Problem));
        Assert.Equal(
            JsonSerializer.Serialize(first.ExpectedAnswer),
            JsonSerializer.Serialize(second.ExpectedAnswer));
        Assert.Equal(
            first.Parameters.OrderBy(x => x.Key).ToArray(),
            second.Parameters.OrderBy(x => x.Key).ToArray());
    }

    [Theory]
    [InlineData(0)]
    [InlineData(4)]
    public void Generate_InvalidDifficulty_FailsClosed(int difficultyBand)
    {
        var factory = new ExactLinearInequalityQuestionFactory(
            new ExactLinearInequalitySolver(),
            new ExactLinearInequalityVerifier());

        Assert.Throws<ArgumentOutOfRangeException>(() => factory.Generate(1, difficultyBand));
    }
}
