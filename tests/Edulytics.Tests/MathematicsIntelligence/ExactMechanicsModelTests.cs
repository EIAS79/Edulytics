using System.Numerics;
using System.Text.Json;
using Edulytics.Core.Mathematics.Ast;
using Edulytics.Core.Mathematics.Domains;
using Edulytics.Core.Mathematics.Solving;
using Edulytics.Services.Mathematics.Generation;
using Edulytics.Services.Mathematics.Solving;
using Edulytics.Services.Mathematics.Verification;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class ExactMechanicsModelSolverTests
{
    private readonly ExactMechanicsModelSolver solver = new();
    private readonly ExactMechanicsModelVerifier verifier = new();

    [Theory]
    [InlineData("mechanics_constant_acceleration_velocity_exact", 7, "m_per_s")]
    [InlineData("mechanics_constant_acceleration_displacement_exact", 16, "m")]
    [InlineData("mechanics_newton_second_law_force_exact", 12, "N")]
    [InlineData("mechanics_newton_third_law_reaction_exact", -6, "N")]
    [InlineData("mechanics_impulse_momentum_exact", 9, "kg_m_per_s")]
    [InlineData("mechanics_work_energy_exact", 14, "J")]
    [InlineData("mechanics_power_exact", 5, "W")]
    public void ScalarMechanicsModels_SolveExactly_AndVerifyIndependently(string model, int expected, string expectedUnit)
    {
        var request = model switch
        {
            "mechanics_constant_acceleration_velocity_exact" => Request(Call(model, Q(1, "m_per_s"), Q(2, "m_per_s2"), Q(3, "s"))),
            "mechanics_constant_acceleration_displacement_exact" => Request(Call(model, Q(2, "m_per_s"), Q(2, "m_per_s2"), Q(2, "s"))),
            "mechanics_newton_second_law_force_exact" => Request(Call(model, Q(3, "kg"), Q(4, "m_per_s2"))),
            "mechanics_newton_third_law_reaction_exact" => Request(Call(model, Q(6, "N"))),
            "mechanics_impulse_momentum_exact" => Request(Call(model, Q(4, "kg_m_per_s"), Q(5, "N_s"))),
            "mechanics_work_energy_exact" => Request(Call(model, Q(10, "J"), Q(4, "J"))),
            "mechanics_power_exact" => Request(Call(model, Q(20, "J"), Q(4, "s"))),
            _ => throw new ArgumentOutOfRangeException(nameof(model))
        };

        var result = solver.Solve(request);

        Assert.Equal(MathematicsSolveStatus.Solved, result.Status);
        AssertQuantity(result.ExactResult, expected, expectedUnit);
        Assert.True(verifier.Verify(request, result).IsVerified);

        var forgedQuantity = Q(expected + 1, expectedUnit);
        var forged = result with
        {
            ExactResult = forgedQuantity,
            SolutionSet = new FiniteSolutionSet([forgedQuantity])
        };
        Assert.False(verifier.Verify(request, forged).IsVerified);
    }

    [Fact]
    public void Equilibrium_ReturnsExactBalancingForce_AndRejectsMutation()
    {
        var request = Request(Call(
            "mechanics_equilibrium_balancing_force_1d_exact",
            new VectorNode([Q(10, "N"), Q(-4, "N"), Q(1, "N")])));

        var result = solver.Solve(request);
        Assert.Equal(MathematicsSolveStatus.Solved, result.Status);
        AssertQuantity(result.ExactResult, -7, "N");
        Assert.True(verifier.Verify(request, result).IsVerified);

        var forgedQuantity = Q(-6, "N");
        var forged = result with { ExactResult = forgedQuantity, SolutionSet = new FiniteSolutionSet([forgedQuantity]) };
        Assert.False(verifier.Verify(request, forged).IsVerified);
    }

    [Fact]
    public void Mechanics_FailsClosedOnDimensionMismatch_AndVerifierCannotBeForged()
    {
        var request = Request(Call(
            "mechanics_newton_second_law_force_exact",
            Q(3, "m"),
            Q(4, "m_per_s2")));

        var result = solver.Solve(request);
        Assert.Equal(MathematicsSolveStatus.Unsupported, result.Status);

        var forgedQuantity = Q(12, "N");
        var forged = new MathematicsSolveResult(
            MathematicsSolveStatus.Solved,
            forgedQuantity,
            new FiniteSolutionSet([forgedQuantity]),
            request.Assumptions,
            "forged",
            new MathematicsSolutionTrace([]),
            "test",
            "test-v1",
            []);
        Assert.False(verifier.Verify(request, forged).IsVerified);
    }

    [Theory]
    [InlineData("mechanics_constant_acceleration_velocity_exact")]
    [InlineData("mechanics_constant_acceleration_displacement_exact")]
    public void Kinematics_RejectsNegativeElapsedTime(string model)
    {
        var request = Request(Call(model, Q(1, "m_per_s"), Q(2, "m_per_s2"), Q(-1, "s")));
        var result = solver.Solve(request);
        Assert.Equal(MathematicsSolveStatus.Unsupported, result.Status);

        var forgedQuantity = Q(0, model.EndsWith("velocity_exact", StringComparison.Ordinal) ? "m_per_s" : "m");
        var forged = new MathematicsSolveResult(
            MathematicsSolveStatus.Solved,
            forgedQuantity,
            new FiniteSolutionSet([forgedQuantity]),
            request.Assumptions,
            "forged",
            new MathematicsSolutionTrace([]),
            "test",
            "test-v1",
            []);
        Assert.False(verifier.Verify(request, forged).IsVerified);
    }

    [Fact]
    public void PhysicalBounds_RejectNonPositiveMass_NegativeKineticEnergy_AndNonPositivePowerTime()
    {
        Assert.Equal(
            MathematicsSolveStatus.Unsupported,
            solver.Solve(Request(Call("mechanics_newton_second_law_force_exact", Q(0, "kg"), Q(3, "m_per_s2")))).Status);

        Assert.Equal(
            MathematicsSolveStatus.Unsupported,
            solver.Solve(Request(Call("mechanics_work_energy_exact", Q(2, "J"), Q(-3, "J")))).Status);

        Assert.Equal(
            MathematicsSolveStatus.Unsupported,
            solver.Solve(Request(Call("mechanics_power_exact", Q(10, "J"), Q(0, "s")))).Status);
    }

    [Fact]
    public void Mechanics_RejectsWrongArity_AndOversizedExactScalars()
    {
        var wrongArity = solver.Solve(Request(Call("mechanics_power_exact", Q(10, "J"))));
        Assert.Equal(MathematicsSolveStatus.Unsupported, wrongArity.Status);

        var huge = new IntegerNode(BigInteger.One << 5000);
        var hugeMass = new FunctionCallNode("quantity_si", [huge, new SymbolNode("kg")]);
        var oversized = solver.Solve(Request(Call("mechanics_newton_second_law_force_exact", hugeMass, Q(1, "m_per_s2"))));
        Assert.Equal(MathematicsSolveStatus.ResourceLimit, oversized.Status);
    }

    private static MathematicsSolveRequest Request(MathNode problem) => new(problem, [], []);

    private static FunctionCallNode Call(string name, params MathNode[] arguments) => new(name, arguments);

    private static FunctionCallNode Q(int value, string unit) =>
        new("quantity_si", [new IntegerNode(new BigInteger(value)), new SymbolNode(unit)]);

    private static void AssertQuantity(MathNode? node, int expected, string unit)
    {
        var quantity = Assert.IsType<FunctionCallNode>(node);
        Assert.Equal("quantity_si", quantity.FunctionName);
        Assert.Equal(2, quantity.Arguments.Count);
        Assert.Equal(new IntegerNode(new BigInteger(expected)), quantity.Arguments[0]);
        Assert.Equal(new SymbolNode(unit), quantity.Arguments[1]);
    }
}

public sealed class ExactMechanicsQuestionFactoryTests
{
    public static TheoryData<string> Families => new()
    {
        ExactMechanicsQuestionFactory.VelocityFamily,
        ExactMechanicsQuestionFactory.DisplacementFamily,
        ExactMechanicsQuestionFactory.NewtonSecondFamily,
        ExactMechanicsQuestionFactory.NewtonThirdFamily,
        ExactMechanicsQuestionFactory.EquilibriumFamily,
        ExactMechanicsQuestionFactory.MomentumFamily,
        ExactMechanicsQuestionFactory.WorkEnergyFamily,
        ExactMechanicsQuestionFactory.PowerFamily
    };

    [Theory]
    [MemberData(nameof(Families))]
    public void EveryMechanicsFamily_IsVerifiedDeterministicAndUnitTagged(string family)
    {
        var factory = new ExactMechanicsQuestionFactory(new ExactMechanicsModelSolver(), new ExactMechanicsModelVerifier());

        for (var band = 1; band <= 3; band++)
        {
            var first = factory.Generate(family, 20260917, band);
            var second = factory.Generate(family, 20260917, band);

            Assert.Equal(family, first.QuestionFamilyId);
            Assert.True(first.Verification.IsVerified);
            Assert.Equal(JsonSerializer.Serialize(first.Problem), JsonSerializer.Serialize(second.Problem));
            Assert.Equal(JsonSerializer.Serialize(first.ExpectedAnswer), JsonSerializer.Serialize(second.ExpectedAnswer));
            Assert.Contains(first.RequiredCapabilities, capability => capability.Value == "mechanics.units.si_dimensional_analysis");
            Assert.Contains(first.RequiredCapabilities, capability => capability.Value == "mechanics.verify.models_and_units");
            Assert.IsType<FunctionCallNode>(first.ExpectedAnswer);
        }
    }
}
