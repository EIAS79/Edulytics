using System.Globalization;
using System.Numerics;
using Edulytics.Core.Mathematics.Ast;
using Edulytics.Core.Mathematics.Domains;
using Edulytics.Core.Mathematics.Identifiers;
using Edulytics.Services.Mathematics.Contracts;

namespace Edulytics.Services.Mathematics.Generation;

public sealed class ExactIndexPowerQuestionFactory
{
    public const string FamilyId = "indices.power.integer_exponent.exact";
    public static readonly SkillId Skill = new("indices.integer_power.evaluate");

    private readonly IMathematicsSolver solver;
    private readonly IMathematicsVerifier verifier;

    public ExactIndexPowerQuestionFactory(IMathematicsSolver solver, IMathematicsVerifier verifier)
    {
        this.solver = solver ?? throw new ArgumentNullException(nameof(solver));
        this.verifier = verifier ?? throw new ArgumentNullException(nameof(verifier));
    }

    public VerifiedGeneratedMathematicsProblem Generate(int variantKey, int difficultyBand)
    {
        ExactFunctionsGraphsSequencesGeneration.ValidateDifficulty(difficultyBand);
        var state = ExactFunctionsGraphsSequencesGeneration.Seed(variantKey, 0xA54FF53Au);

        MathNode @base;
        int exponent;
        if (difficultyBand == 1)
        {
            @base = ExactFunctionsGraphsSequencesGeneration.I(ExactFunctionsGraphsSequencesGeneration.Next(ref state, 2, 9));
            exponent = ExactFunctionsGraphsSequencesGeneration.Next(ref state, 2, 5);
        }
        else if (difficultyBand == 2)
        {
            @base = ExactFunctionsGraphsSequencesGeneration.I(ExactFunctionsGraphsSequencesGeneration.NonZeroSigned(ref state, 8));
            exponent = ExactFunctionsGraphsSequencesGeneration.Next(ref state, 2, 6);
        }
        else
        {
            @base = new RationalNode(new ExactRational(
                new BigInteger(ExactFunctionsGraphsSequencesGeneration.NonZeroSigned(ref state, 7)),
                new BigInteger(ExactFunctionsGraphsSequencesGeneration.Next(ref state, 2, 5))));
            exponent = -ExactFunctionsGraphsSequencesGeneration.Next(ref state, 2, 4);
        }

        var problem = new FunctionCallNode(
            "index_power_exact",
            [@base, ExactFunctionsGraphsSequencesGeneration.I(exponent)]);

        return ExactFunctionsGraphsSequencesGeneration.SolveVerifyAndBuild(
            FamilyId,
            Skill,
            problem,
            solver,
            verifier,
            [
                new CapabilityId("rational.exact_arithmetic"),
                new CapabilityId("indices.power.integer_exponent.exact"),
                new CapabilityId("indices.verify.integer_power")
            ],
            variantKey,
            difficultyBand,
            new Dictionary<string, string>(StringComparer.Ordinal)
            {
                ["exponent"] = exponent.ToString(CultureInfo.InvariantCulture)
            });
    }
}

public sealed class ExactExponentialSameBaseQuestionFactory
{
    public const string FamilyId = "exponentials.solve.same_base.integer_exponent";
    public static readonly SkillId Skill = new("exponentials.same_base.solve");

    private readonly IMathematicsSolver solver;
    private readonly IMathematicsVerifier verifier;

    public ExactExponentialSameBaseQuestionFactory(IMathematicsSolver solver, IMathematicsVerifier verifier)
    {
        this.solver = solver ?? throw new ArgumentNullException(nameof(solver));
        this.verifier = verifier ?? throw new ArgumentNullException(nameof(verifier));
    }

    public VerifiedGeneratedMathematicsProblem Generate(int variantKey, int difficultyBand)
    {
        ExactFunctionsGraphsSequencesGeneration.ValidateDifficulty(difficultyBand);
        var state = ExactFunctionsGraphsSequencesGeneration.Seed(variantKey, 0x510E527Fu);
        var @base = ExactFunctionsGraphsSequencesGeneration.Next(ref state, 2, difficultyBand == 1 ? 5 : 9);
        var exponent = difficultyBand switch
        {
            1 => ExactFunctionsGraphsSequencesGeneration.Next(ref state, 2, 5),
            2 => NonZeroExponent(ref state, -4, 6),
            _ => NonZeroExponent(ref state, -6, 8)
        };
        var target = BuildIntegerBasePower(@base, exponent);
        var problem = new FunctionCallNode(
            "solve_exponential_same_base",
            [ExactFunctionsGraphsSequencesGeneration.I(@base), target]);

        return ExactFunctionsGraphsSequencesGeneration.SolveVerifyAndBuild(
            FamilyId,
            Skill,
            problem,
            solver,
            verifier,
            [
                new CapabilityId("rational.exact_arithmetic"),
                new CapabilityId("exponentials.solve.same_base.integer_exponent"),
                new CapabilityId("exponentials.verify.same_base.substitution")
            ],
            variantKey,
            difficultyBand,
            new Dictionary<string, string>(StringComparer.Ordinal)
            {
                ["base"] = @base.ToString(CultureInfo.InvariantCulture)
            });
    }

    private static int NonZeroExponent(ref uint state, int min, int max)
    {
        int value;
        do
        {
            value = ExactFunctionsGraphsSequencesGeneration.Next(ref state, min, max);
        }
        while (value == 0);
        return value;
    }

    internal static MathNode BuildIntegerBasePower(int @base, int exponent)
    {
        var magnitude = BigInteger.Pow(new BigInteger(@base), Math.Abs(exponent));
        return exponent >= 0
            ? new IntegerNode(magnitude)
            : new RationalNode(new ExactRational(BigInteger.One, magnitude));
    }
}

public sealed class ExactLogarithmQuestionFactory
{
    public const string FamilyId = "logarithms.evaluate.exact_integer_power";
    public static readonly SkillId Skill = new("logarithms.evaluate.exact");

    private readonly IMathematicsSolver solver;
    private readonly IMathematicsVerifier verifier;

    public ExactLogarithmQuestionFactory(IMathematicsSolver solver, IMathematicsVerifier verifier)
    {
        this.solver = solver ?? throw new ArgumentNullException(nameof(solver));
        this.verifier = verifier ?? throw new ArgumentNullException(nameof(verifier));
    }

    public VerifiedGeneratedMathematicsProblem Generate(int variantKey, int difficultyBand)
    {
        ExactFunctionsGraphsSequencesGeneration.ValidateDifficulty(difficultyBand);
        var state = ExactFunctionsGraphsSequencesGeneration.Seed(variantKey, 0x9B05688Cu);
        var @base = ExactFunctionsGraphsSequencesGeneration.Next(ref state, 2, difficultyBand == 1 ? 5 : 10);
        var exponent = difficultyBand switch
        {
            1 => ExactFunctionsGraphsSequencesGeneration.Next(ref state, 1, 4),
            2 => ExactFunctionsGraphsSequencesGeneration.Next(ref state, 2, 7),
            _ => -ExactFunctionsGraphsSequencesGeneration.Next(ref state, 1, 5)
        };
        var argument = ExactExponentialSameBaseQuestionFactory.BuildIntegerBasePower(@base, exponent);
        var problem = new FunctionCallNode(
            "log_exact",
            [ExactFunctionsGraphsSequencesGeneration.I(@base), argument]);

        return ExactFunctionsGraphsSequencesGeneration.SolveVerifyAndBuild(
            FamilyId,
            Skill,
            problem,
            solver,
            verifier,
            [
                new CapabilityId("rational.exact_arithmetic"),
                new CapabilityId("logarithms.evaluate.exact_integer_power"),
                new CapabilityId("logarithms.verify.inverse_exponentiation")
            ],
            variantKey,
            difficultyBand,
            new Dictionary<string, string>(StringComparer.Ordinal)
            {
                ["base"] = @base.ToString(CultureInfo.InvariantCulture)
            });
    }
}

public sealed class ExactSpecialAngleTrigonometryQuestionFactory
{
    public const string FamilyId = "trigonometry.special_angle.exact_degrees";
    public static readonly SkillId Skill = new("trigonometry.special_angles.evaluate");

    private static readonly (string Function, int Angle)[] EasyCases =
    [
        ("sin_degrees_exact", 30),
        ("cos_degrees_exact", 60),
        ("tan_degrees_exact", 45),
        ("sin_degrees_exact", 90),
        ("cos_degrees_exact", 0)
    ];

    private static readonly (string Function, int Angle)[] MediumCases =
    [
        ("sin_degrees_exact", 45),
        ("cos_degrees_exact", 30),
        ("tan_degrees_exact", 30),
        ("tan_degrees_exact", 60),
        ("sin_degrees_exact", 120),
        ("cos_degrees_exact", 135)
    ];

    private static readonly (string Function, int Angle)[] ChallengingCases =
    [
        ("sin_degrees_exact", -45),
        ("cos_degrees_exact", 150),
        ("tan_degrees_exact", 120),
        ("sin_degrees_exact", 390),
        ("cos_degrees_exact", -150),
        ("tan_degrees_exact", 330)
    ];

    private readonly IMathematicsSolver solver;
    private readonly IMathematicsVerifier verifier;

    public ExactSpecialAngleTrigonometryQuestionFactory(IMathematicsSolver solver, IMathematicsVerifier verifier)
    {
        this.solver = solver ?? throw new ArgumentNullException(nameof(solver));
        this.verifier = verifier ?? throw new ArgumentNullException(nameof(verifier));
    }

    public VerifiedGeneratedMathematicsProblem Generate(int variantKey, int difficultyBand)
    {
        ExactFunctionsGraphsSequencesGeneration.ValidateDifficulty(difficultyBand);
        var state = ExactFunctionsGraphsSequencesGeneration.Seed(variantKey, 0x1F83D9ABu);
        var cases = difficultyBand == 1 ? EasyCases : difficultyBand == 2 ? MediumCases : ChallengingCases;
        var selected = cases[ExactFunctionsGraphsSequencesGeneration.Next(ref state, 0, cases.Length - 1)];
        var problem = new FunctionCallNode(selected.Function, [ExactFunctionsGraphsSequencesGeneration.I(selected.Angle)]);

        return ExactFunctionsGraphsSequencesGeneration.SolveVerifyAndBuild(
            FamilyId,
            Skill,
            problem,
            solver,
            verifier,
            [
                new CapabilityId("trigonometry.special_angle.exact"),
                new CapabilityId("trigonometry.verify.special_angle.exact")
            ],
            variantKey,
            difficultyBand,
            new Dictionary<string, string>(StringComparer.Ordinal)
            {
                ["function"] = selected.Function,
                ["angleDegrees"] = selected.Angle.ToString(CultureInfo.InvariantCulture)
            });
    }
}
