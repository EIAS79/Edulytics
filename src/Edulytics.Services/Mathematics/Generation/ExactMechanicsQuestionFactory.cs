using System.Globalization;
using System.Numerics;
using Edulytics.Core.Mathematics.Ast;
using Edulytics.Core.Mathematics.Identifiers;
using Edulytics.Services.Mathematics.Contracts;

namespace Edulytics.Services.Mathematics.Generation;

public sealed class ExactMechanicsQuestionFactory
{
    public const string VelocityFamily = "mechanics.kinematics.constant_acceleration.velocity.exact";
    public const string DisplacementFamily = "mechanics.kinematics.constant_acceleration.displacement.exact";
    public const string NewtonSecondFamily = "mechanics.newton.second_law.force.exact";
    public const string NewtonThirdFamily = "mechanics.newton.third_law.reaction.exact";
    public const string EquilibriumFamily = "mechanics.equilibrium.balancing_force_1d.exact";
    public const string MomentumFamily = "mechanics.momentum.impulse.exact";
    public const string WorkEnergyFamily = "mechanics.energy.work_energy.exact";
    public const string PowerFamily = "mechanics.power.work_over_time.exact";

    private readonly IMathematicsSolver solver;
    private readonly IMathematicsVerifier verifier;

    public ExactMechanicsQuestionFactory(IMathematicsSolver solver, IMathematicsVerifier verifier)
    {
        this.solver = solver ?? throw new ArgumentNullException(nameof(solver));
        this.verifier = verifier ?? throw new ArgumentNullException(nameof(verifier));
    }

    public VerifiedGeneratedMathematicsProblem Generate(string familyId, int variantKey, int difficultyBand)
    {
        ExactFunctionsGraphsSequencesGeneration.ValidateDifficulty(difficultyBand);
        var problem = BuildProblem(familyId, variantKey, difficultyBand);
        var skill = new SkillId(SkillFor(familyId));
        var capability = CapabilityFor(familyId);

        return ExactFunctionsGraphsSequencesGeneration.SolveVerifyAndBuild(
            familyId,
            skill,
            problem,
            solver,
            verifier,
            [
                new CapabilityId("rational.exact_arithmetic"),
                new CapabilityId("mechanics.units.si_dimensional_analysis"),
                new CapabilityId(capability),
                new CapabilityId("mechanics.verify.models_and_units")
            ],
            variantKey,
            difficultyBand,
            new Dictionary<string, string>(StringComparer.Ordinal)
            {
                ["operation"] = familyId,
                ["difficultyBand"] = difficultyBand.ToString(CultureInfo.InvariantCulture),
                ["unitSystem"] = "SI-canonical",
                ["modelSemantics"] = "bounded exact-rational mechanics model with mandatory dimensional validation"
            });
    }

    private static MathNode BuildProblem(string familyId, int variantKey, int band)
    {
        var k = Pick(variantKey, 1, 5);
        return familyId switch
        {
            VelocityFamily => new FunctionCallNode(
                "mechanics_constant_acceleration_velocity_exact",
                [Q(k + 1, "m_per_s"), Q(band + 1, "m_per_s2"), Q(band + 1, "s")]),

            DisplacementFamily => new FunctionCallNode(
                "mechanics_constant_acceleration_displacement_exact",
                [Q(k, "m_per_s"), Q(band, "m_per_s2"), Q(band + 1, "s")]),

            NewtonSecondFamily => new FunctionCallNode(
                "mechanics_newton_second_law_force_exact",
                [Q(k + band + 1, "kg"), Q(band + 1, "m_per_s2")]),

            NewtonThirdFamily => new FunctionCallNode(
                "mechanics_newton_third_law_reaction_exact",
                [Q((variantKey & 1) == 0 ? k + band : -(k + band), "N")]),

            EquilibriumFamily => new FunctionCallNode(
                "mechanics_equilibrium_balancing_force_1d_exact",
                [new VectorNode([Q(k + 2, "N"), Q(-(band + 1), "N"), Q(band, "N")])]),

            MomentumFamily => new FunctionCallNode(
                "mechanics_impulse_momentum_exact",
                [Q(k + band, "kg_m_per_s"), Q(band + 2, "N_s")]),

            WorkEnergyFamily => new FunctionCallNode(
                "mechanics_work_energy_exact",
                [Q((k + 1) * 5, "J"), Q((band + 1) * 4, "J")]),

            PowerFamily => new FunctionCallNode(
                "mechanics_power_exact",
                [Q((k + band + 2) * (band + 1), "J"), Q(band + 1, "s")]),

            _ => throw new ArgumentOutOfRangeException(nameof(familyId), familyId, "Unknown Mechanics V2 family.")
        };
    }

    private static string SkillFor(string familyId) => familyId switch
    {
        VelocityFamily => "mechanics.kinematics.constant_acceleration.velocity",
        DisplacementFamily => "mechanics.kinematics.constant_acceleration.displacement",
        NewtonSecondFamily => "mechanics.newton.second_law.force",
        NewtonThirdFamily => "mechanics.newton.third_law.reaction",
        EquilibriumFamily => "mechanics.equilibrium.balancing_force_1d",
        MomentumFamily => "mechanics.momentum.impulse",
        WorkEnergyFamily => "mechanics.energy.work_energy",
        PowerFamily => "mechanics.power.work_over_time",
        _ => throw new ArgumentOutOfRangeException(nameof(familyId), familyId, "Unknown Mechanics V2 family.")
    };

    private static string CapabilityFor(string familyId) => familyId switch
    {
        VelocityFamily => "mechanics.kinematics.constant_acceleration.velocity.exact",
        DisplacementFamily => "mechanics.kinematics.constant_acceleration.displacement.exact",
        NewtonSecondFamily => "mechanics.newton.second_law.force.exact",
        NewtonThirdFamily => "mechanics.newton.third_law.reaction.exact",
        EquilibriumFamily => "mechanics.equilibrium.balancing_force_1d.exact",
        MomentumFamily => "mechanics.momentum.impulse.exact",
        WorkEnergyFamily => "mechanics.energy.work_energy.exact",
        PowerFamily => "mechanics.power.work_over_time.exact",
        _ => throw new ArgumentOutOfRangeException(nameof(familyId), familyId, "Unknown Mechanics V2 family.")
    };

    private static int Pick(int variantKey, int minimum, int span) =>
        minimum + (int)(Math.Abs((long)variantKey) % span);

    private static FunctionCallNode Q(int value, string unit) =>
        new("quantity_si", [new IntegerNode(new BigInteger(value)), new SymbolNode(unit)]);
}
