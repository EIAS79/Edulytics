using System.Globalization;
using System.Numerics;
using Edulytics.Core.Mathematics.Ast;
using Edulytics.Core.Mathematics.Domains;
using Edulytics.Core.Mathematics.Identifiers;
using Edulytics.Services.Mathematics.Contracts;

namespace Edulytics.Services.Mathematics.Generation;

public sealed class ExactRectangleAreaQuestionFactory
{
    public const string FamilyId = "geometry.rectangle.area.exact";
    public static readonly SkillId Skill = new("geometry.rectangle.area");
    private readonly IMathematicsSolver solver;
    private readonly IMathematicsVerifier verifier;

    public ExactRectangleAreaQuestionFactory(IMathematicsSolver solver, IMathematicsVerifier verifier)
    {
        this.solver = solver ?? throw new ArgumentNullException(nameof(solver));
        this.verifier = verifier ?? throw new ArgumentNullException(nameof(verifier));
    }

    public VerifiedGeneratedMathematicsProblem Generate(int variantKey, int difficultyBand)
    {
        ExactFunctionsGraphsSequencesGeneration.ValidateDifficulty(difficultyBand);
        var state = ExactFunctionsGraphsSequencesGeneration.Seed(variantKey, 0x243F6A88u);
        var width = ExactGeometryGeneration.Dimension(ref state, difficultyBand, 11);
        var height = ExactGeometryGeneration.Dimension(ref state, difficultyBand, 17);
        var problem = new FunctionCallNode("rectangle_area_exact", [width, height]);
        return ExactGeometryGeneration.Build(FamilyId, Skill, problem, solver, verifier,
            ["rational.exact_arithmetic", "geometry.rectangle.area.exact", "geometry.verify.rectangle.area"],
            variantKey, difficultyBand, "rectangle-area");
    }
}

public sealed class ExactRectanglePerimeterQuestionFactory
{
    public const string FamilyId = "geometry.rectangle.perimeter.exact";
    public static readonly SkillId Skill = new("geometry.rectangle.perimeter");
    private readonly IMathematicsSolver solver;
    private readonly IMathematicsVerifier verifier;

    public ExactRectanglePerimeterQuestionFactory(IMathematicsSolver solver, IMathematicsVerifier verifier)
    {
        this.solver = solver ?? throw new ArgumentNullException(nameof(solver));
        this.verifier = verifier ?? throw new ArgumentNullException(nameof(verifier));
    }

    public VerifiedGeneratedMathematicsProblem Generate(int variantKey, int difficultyBand)
    {
        ExactFunctionsGraphsSequencesGeneration.ValidateDifficulty(difficultyBand);
        var state = ExactFunctionsGraphsSequencesGeneration.Seed(variantKey, 0x85A308D3u);
        var width = ExactGeometryGeneration.Dimension(ref state, difficultyBand, 13);
        var height = ExactGeometryGeneration.Dimension(ref state, difficultyBand, 19);
        var problem = new FunctionCallNode("rectangle_perimeter_exact", [width, height]);
        return ExactGeometryGeneration.Build(FamilyId, Skill, problem, solver, verifier,
            ["rational.exact_arithmetic", "geometry.rectangle.perimeter.exact", "geometry.verify.rectangle.perimeter"],
            variantKey, difficultyBand, "rectangle-perimeter");
    }
}

public sealed class ExactTriangleBaseHeightAreaQuestionFactory
{
    public const string FamilyId = "geometry.triangle.area.base_height.exact";
    public static readonly SkillId Skill = new("geometry.triangle.area.base_height");
    private readonly IMathematicsSolver solver;
    private readonly IMathematicsVerifier verifier;

    public ExactTriangleBaseHeightAreaQuestionFactory(IMathematicsSolver solver, IMathematicsVerifier verifier)
    {
        this.solver = solver ?? throw new ArgumentNullException(nameof(solver));
        this.verifier = verifier ?? throw new ArgumentNullException(nameof(verifier));
    }

    public VerifiedGeneratedMathematicsProblem Generate(int variantKey, int difficultyBand)
    {
        ExactFunctionsGraphsSequencesGeneration.ValidateDifficulty(difficultyBand);
        var state = ExactFunctionsGraphsSequencesGeneration.Seed(variantKey, 0x13198A2Eu);
        var @base = ExactGeometryGeneration.Dimension(ref state, difficultyBand, 17);
        var height = ExactGeometryGeneration.Dimension(ref state, difficultyBand, 23);
        var problem = new FunctionCallNode("triangle_area_base_height_exact", [@base, height]);
        return ExactGeometryGeneration.Build(FamilyId, Skill, problem, solver, verifier,
            ["rational.exact_arithmetic", "geometry.triangle.area.base_height.exact", "geometry.verify.triangle.area.base_height"],
            variantKey, difficultyBand, "triangle-area-base-height");
    }
}

public sealed class ExactPythagoreanQuestionFactory
{
    public const string FamilyId = "geometry.right_triangle.pythagorean.exact";
    public static readonly SkillId Skill = new("geometry.right_triangle.pythagorean");
    private static readonly (int A, int B, int C)[] Triples = [(3,4,5), (5,12,13), (8,15,17), (7,24,25)];
    private readonly IMathematicsSolver solver;
    private readonly IMathematicsVerifier verifier;

    public ExactPythagoreanQuestionFactory(IMathematicsSolver solver, IMathematicsVerifier verifier)
    {
        this.solver = solver ?? throw new ArgumentNullException(nameof(solver));
        this.verifier = verifier ?? throw new ArgumentNullException(nameof(verifier));
    }

    public VerifiedGeneratedMathematicsProblem Generate(int variantKey, int difficultyBand)
    {
        ExactFunctionsGraphsSequencesGeneration.ValidateDifficulty(difficultyBand);
        var state = ExactFunctionsGraphsSequencesGeneration.Seed(variantKey, 0x03707344u);
        var triple = Triples[ExactFunctionsGraphsSequencesGeneration.Next(ref state, 0, Triples.Length - 1)];
        var scaleNumerator = difficultyBand == 1 ? 1 : ExactFunctionsGraphsSequencesGeneration.Next(ref state, 1, 5);
        var scaleDenominator = difficultyBand < 3 ? 1 : ExactFunctionsGraphsSequencesGeneration.Next(ref state, 2, 5);
        var a = ExactGeometryGeneration.Scale(triple.A, scaleNumerator, scaleDenominator);
        var b = ExactGeometryGeneration.Scale(triple.B, scaleNumerator, scaleDenominator);
        var c = ExactGeometryGeneration.Scale(triple.C, scaleNumerator, scaleDenominator);

        var solveLeg = difficultyBand >= 2 && ExactFunctionsGraphsSequencesGeneration.Next(ref state, 0, 1) == 1;
        var problem = solveLeg
            ? new FunctionCallNode("pythagorean_leg_exact", [c, a])
            : new FunctionCallNode("pythagorean_hypotenuse_exact", [a, b]);

        return ExactGeometryGeneration.Build(FamilyId, Skill, problem, solver, verifier,
            ["rational.exact_arithmetic", "geometry.right_triangle.pythagorean.exact", "geometry.verify.right_triangle.pythagorean"],
            variantKey, difficultyBand, solveLeg ? "pythagorean-leg" : "pythagorean-hypotenuse");
    }
}

internal static class ExactGeometryGeneration
{
    public static MathNode Dimension(ref uint state, int difficultyBand, int salt)
    {
        var numerator = ExactFunctionsGraphsSequencesGeneration.Next(ref state, 2, 8 + difficultyBand * salt / 3);
        if (difficultyBand < 3)
        {
            return ExactFunctionsGraphsSequencesGeneration.I(numerator);
        }
        var denominator = ExactFunctionsGraphsSequencesGeneration.Next(ref state, 2, 5);
        return new RationalNode(new ExactRational(new BigInteger(numerator), new BigInteger(denominator)));
    }

    public static MathNode Scale(int value, int numerator, int denominator)
    {
        var rational = new ExactRational(new BigInteger(value * numerator), new BigInteger(denominator));
        return rational.Denominator == BigInteger.One ? new IntegerNode(rational.Numerator) : new RationalNode(rational);
    }

    public static VerifiedGeneratedMathematicsProblem Build(
        string familyId,
        SkillId skill,
        MathNode problem,
        IMathematicsSolver solver,
        IMathematicsVerifier verifier,
        IReadOnlyList<string> capabilityIds,
        int variantKey,
        int difficultyBand,
        string geometryKind)
    {
        return ExactFunctionsGraphsSequencesGeneration.SolveVerifyAndBuild(
            familyId,
            skill,
            problem,
            solver,
            verifier,
            capabilityIds.Select(id => new CapabilityId(id)).ToArray(),
            variantKey,
            difficultyBand,
            new Dictionary<string, string>(StringComparer.Ordinal)
            {
                ["geometryKind"] = geometryKind,
                ["difficultyBand"] = difficultyBand.ToString(CultureInfo.InvariantCulture)
            });
    }
}
