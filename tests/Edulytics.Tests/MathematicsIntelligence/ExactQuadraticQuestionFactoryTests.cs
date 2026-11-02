using System.Text.Json;
using Edulytics.Core.Mathematics.Ast;
using Edulytics.Core.Mathematics.Solving;
using Edulytics.Services.Mathematics.Generation;
using Edulytics.Services.Mathematics.Solving;
using Edulytics.Services.Mathematics.Verification;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class ExactQuadraticQuestionFactoryTests
{
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public void Generate_ProducesSolverAndVerifierApprovedProblem(int difficultyBand)
    {
        var factory = CreateFactory();

        var generated = factory.Generate(20260916, difficultyBand);

        Assert.Equal(ExactQuadraticQuestionFactory.FamilyId, generated.QuestionFamilyId);
        Assert.Equal("algebra.quadratic.solve.real_roots", generated.Skill.Value);
        Assert.Equal(MathematicsSolveStatus.Solved, generated.SolveResult.Status);
        Assert.True(generated.Verification.IsVerified);
        Assert.False(string.IsNullOrWhiteSpace(JsonSerializer.Serialize(generated.ExpectedAnswer)));
    }

    [Fact]
    public void Generate_IsDeterministicForSameVariantAndDifficulty()
    {
        var factory = CreateFactory();

        var first = factory.Generate(415, 3);
        var second = factory.Generate(415, 3);

        Assert.Equal(
            JsonSerializer.Serialize(first.Problem),
            JsonSerializer.Serialize(second.Problem));
        Assert.Equal(
            JsonSerializer.Serialize(first.ExpectedAnswer),
            JsonSerializer.Serialize(second.ExpectedAnswer));
        Assert.Equal(first.Parameters, second.Parameters);
    }

    [Fact]
    public void ChallengingGeneration_ExercisesExactSurdAnswer()
    {
        var factory = CreateFactory();

        var generated = factory.Generate(99, 3);

        var vector = Assert.IsType<VectorNode>(generated.ExpectedAnswer);
        Assert.Equal(2, vector.Components.Count);
        Assert.All(vector.Components, component => Assert.IsType<DivideNode>(component));
        Assert.True(generated.Verification.IsVerified);
    }

    private static ExactQuadraticQuestionFactory CreateFactory() =>
        new(new ExactQuadraticEquationSolver(), new ExactQuadraticEquationVerifier());
}
