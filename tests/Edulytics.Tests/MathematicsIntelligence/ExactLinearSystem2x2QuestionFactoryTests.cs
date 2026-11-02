using System.Text.Json;
using Edulytics.Core.Mathematics.Ast;
using Edulytics.Core.Mathematics.Solving;
using Edulytics.Core.Mathematics.Verification;
using Edulytics.Services.Mathematics.Generation;
using Edulytics.Services.Mathematics.Solving;
using Edulytics.Services.Mathematics.Verification;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class ExactLinearSystem2x2QuestionFactoryTests
{
    [Theory]
    [InlineData(1, 1)]
    [InlineData(11, 1)]
    [InlineData(42, 2)]
    [InlineData(99, 3)]
    [InlineData(-17, 3)]
    public void Generate_ProducesUniqueSolverVerifiedSystem(int variantKey, int difficultyBand)
    {
        var factory = new ExactLinearSystem2x2QuestionFactory(
            new ExactLinearSystem2x2Solver(),
            new ExactLinearSystem2x2Verifier());

        var generated = factory.Generate(variantKey, difficultyBand);

        Assert.Equal(ExactLinearSystem2x2QuestionFactory.FamilyId, generated.QuestionFamilyId);
        Assert.Equal("algebra.linear.systems.two_by_two.solve", generated.Skill.Value);
        Assert.IsType<EquationSystemNode>(generated.Problem);
        Assert.IsType<VectorNode>(generated.ExpectedAnswer);
        Assert.Equal(MathematicsSolveStatus.Solved, generated.SolveResult.Status);
        Assert.Equal(MathematicsVerificationStatus.Verified, generated.Verification.Status);
    }

    [Fact]
    public void Generate_SameVariantAndDifficulty_IsStable()
    {
        var factory = new ExactLinearSystem2x2QuestionFactory(
            new ExactLinearSystem2x2Solver(),
            new ExactLinearSystem2x2Verifier());

        var first = factory.Generate(12345, 2);
        var second = factory.Generate(12345, 2);

        Assert.Equal(JsonSerializer.Serialize(first.Problem), JsonSerializer.Serialize(second.Problem));
        Assert.Equal(JsonSerializer.Serialize(first.ExpectedAnswer), JsonSerializer.Serialize(second.ExpectedAnswer));
        Assert.Equal(
            first.Parameters.OrderBy(x => x.Key).ToArray(),
            second.Parameters.OrderBy(x => x.Key).ToArray());
    }

    [Theory]
    [InlineData(0)]
    [InlineData(4)]
    public void Generate_InvalidDifficulty_FailsClosed(int difficultyBand)
    {
        var factory = new ExactLinearSystem2x2QuestionFactory(
            new ExactLinearSystem2x2Solver(),
            new ExactLinearSystem2x2Verifier());

        Assert.Throws<ArgumentOutOfRangeException>(() => factory.Generate(1, difficultyBand));
    }
}
