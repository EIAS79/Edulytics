using System.Globalization;
using System.Numerics;
using Edulytics.Core.Mathematics.Ast;
using Edulytics.Core.Mathematics.Domains;
using Edulytics.Core.Mathematics.Identifiers;
using Edulytics.Core.Mathematics.Solving;
using Edulytics.Services.Mathematics.Contracts;

namespace Edulytics.Services.Mathematics.Generation;

/// <summary>
/// Solver-grounded deterministic generators for the Functions / Graphs / Sequences
/// Mathematics V2 shadow slice. Every generated problem is solved and independently
/// verified before it can leave the factory.
/// </summary>
public sealed class ExactFunctionEvaluationQuestionFactory
{
    public const string FamilyId = "functions.evaluate.exact_rational";
    public static readonly SkillId Skill = new("functions.evaluate.exact");

    private readonly IMathematicsSolver solver;
    private readonly IMathematicsVerifier verifier;

    public ExactFunctionEvaluationQuestionFactory(IMathematicsSolver solver, IMathematicsVerifier verifier)
    {
        this.solver = solver ?? throw new ArgumentNullException(nameof(solver));
        this.verifier = verifier ?? throw new ArgumentNullException(nameof(verifier));
    }

    public VerifiedGeneratedMathematicsProblem Generate(int variantKey, int difficultyBand)
    {
        ExactFunctionsGraphsSequencesGeneration.ValidateDifficulty(difficultyBand);
        var state = ExactFunctionsGraphsSequencesGeneration.Seed(variantKey, 0x51F15EEDu);
        var a = ExactFunctionsGraphsSequencesGeneration.NonZeroSigned(ref state, difficultyBand == 1 ? 5 : 8);
        var b = ExactFunctionsGraphsSequencesGeneration.Next(ref state, -12, 12);
        var c = ExactFunctionsGraphsSequencesGeneration.Next(ref state, -10, 10);
        var input = difficultyBand < 3
            ? ExactFunctionsGraphsSequencesGeneration.IntegerValue(
                ExactFunctionsGraphsSequencesGeneration.Next(ref state, -6, 6))
            : ExactFunctionsGraphsSequencesGeneration.RationalValue(
                ExactFunctionsGraphsSequencesGeneration.NonZeroSigned(ref state, 7),
                ExactFunctionsGraphsSequencesGeneration.Next(ref state, 2, 5));

        MathNode expression = difficultyBand switch
        {
            1 => new AddNode([
                new MultiplyNode([ExactFunctionsGraphsSequencesGeneration.I(a), new SymbolNode("x")]),
                ExactFunctionsGraphsSequencesGeneration.I(b)
            ]),
            2 => new AddNode([
                new MultiplyNode([
                    ExactFunctionsGraphsSequencesGeneration.I(a),
                    new PowerNode(new SymbolNode("x"), ExactFunctionsGraphsSequencesGeneration.I(2))
                ]),
                new MultiplyNode([ExactFunctionsGraphsSequencesGeneration.I(b), new SymbolNode("x")]),
                ExactFunctionsGraphsSequencesGeneration.I(c)
            ]),
            _ => new DivideNode(
                new AddNode([
                    new MultiplyNode([
                        ExactFunctionsGraphsSequencesGeneration.I(a),
                        new PowerNode(new SymbolNode("x"), ExactFunctionsGraphsSequencesGeneration.I(2))
                    ]),
                    new MultiplyNode([ExactFunctionsGraphsSequencesGeneration.I(b), new SymbolNode("x")]),
                    ExactFunctionsGraphsSequencesGeneration.I(c)
                ]),
                ExactFunctionsGraphsSequencesGeneration.I(
                    ExactFunctionsGraphsSequencesGeneration.Next(ref state, 2, 7)))
        };

        var problem = new FunctionCallNode("evaluate_exact", [expression, new SymbolNode("x"), input]);
        return ExactFunctionsGraphsSequencesGeneration.SolveVerifyAndBuild(
            FamilyId,
            Skill,
            problem,
            solver,
            verifier,
            [
                new CapabilityId("rational.exact_arithmetic"),
                new CapabilityId("functions.evaluate.exact_rational"),
                new CapabilityId("functions.verify.exact_evaluation")
            ],
            variantKey,
            difficultyBand);
    }
}

public sealed class ExactGraphSamplingQuestionFactory
{
    public const string FamilyId = "functions.graph.sample.exact_coordinates";
    public static readonly SkillId Skill = new("functions.graph.sample.coordinates");

    private readonly IMathematicsSolver solver;
    private readonly IMathematicsVerifier verifier;

    public ExactGraphSamplingQuestionFactory(IMathematicsSolver solver, IMathematicsVerifier verifier)
    {
        this.solver = solver ?? throw new ArgumentNullException(nameof(solver));
        this.verifier = verifier ?? throw new ArgumentNullException(nameof(verifier));
    }

    public VerifiedGeneratedMathematicsProblem Generate(int variantKey, int difficultyBand)
    {
        ExactFunctionsGraphsSequencesGeneration.ValidateDifficulty(difficultyBand);
        var state = ExactFunctionsGraphsSequencesGeneration.Seed(variantKey, 0x6A09E667u);
        var a = ExactFunctionsGraphsSequencesGeneration.NonZeroSigned(ref state, difficultyBand == 1 ? 4 : 6);
        var b = ExactFunctionsGraphsSequencesGeneration.Next(ref state, -8, 8);
        var c = ExactFunctionsGraphsSequencesGeneration.Next(ref state, -8, 8);

        MathNode expression = difficultyBand switch
        {
            1 => new AddNode([
                new MultiplyNode([ExactFunctionsGraphsSequencesGeneration.I(a), new SymbolNode("x")]),
                ExactFunctionsGraphsSequencesGeneration.I(b)
            ]),
            2 => new AddNode([
                new MultiplyNode([
                    ExactFunctionsGraphsSequencesGeneration.I(a),
                    new PowerNode(new SymbolNode("x"), ExactFunctionsGraphsSequencesGeneration.I(2))
                ]),
                new MultiplyNode([ExactFunctionsGraphsSequencesGeneration.I(b), new SymbolNode("x")]),
                ExactFunctionsGraphsSequencesGeneration.I(c)
            ]),
            _ => new DivideNode(
                new AddNode([
                    new MultiplyNode([
                        ExactFunctionsGraphsSequencesGeneration.I(a),
                        new PowerNode(new SymbolNode("x"), ExactFunctionsGraphsSequencesGeneration.I(2))
                    ]),
                    new MultiplyNode([ExactFunctionsGraphsSequencesGeneration.I(b), new SymbolNode("x")]),
                    ExactFunctionsGraphsSequencesGeneration.I(c)
                ]),
                ExactFunctionsGraphsSequencesGeneration.I(
                    ExactFunctionsGraphsSequencesGeneration.Next(ref state, 2, 5)))
        };

        var pointCount = difficultyBand == 1 ? 3 : difficultyBand == 2 ? 5 : 7;
        var start = ExactFunctionsGraphsSequencesGeneration.Next(ref state, -6, 0);
        var xValues = Enumerable.Range(0, pointCount)
            .Select(offset => (MathNode)ExactFunctionsGraphsSequencesGeneration.I(start + offset))
            .ToArray();
        var problem = new FunctionCallNode(
            "sample_graph_exact",
            [expression, new SymbolNode("x"), new VectorNode(xValues)]);

        return ExactFunctionsGraphsSequencesGeneration.SolveVerifyAndBuild(
            FamilyId,
            Skill,
            problem,
            solver,
            verifier,
            [
                new CapabilityId("rational.exact_arithmetic"),
                new CapabilityId("functions.evaluate.exact_rational"),
                new CapabilityId("functions.graph.sample.exact_rational"),
                new CapabilityId("functions.graph.verify.exact_points")
            ],
            variantKey,
            difficultyBand,
            new Dictionary<string, string>(StringComparer.Ordinal)
            {
                ["pointCount"] = pointCount.ToString(CultureInfo.InvariantCulture)
            });
    }
}

public sealed class ExactArithmeticSequenceQuestionFactory
{
    public const string FamilyId = "sequences.arithmetic.nth_term.exact";
    public static readonly SkillId Skill = new("sequences.arithmetic.nth_term");

    private readonly IMathematicsSolver solver;
    private readonly IMathematicsVerifier verifier;

    public ExactArithmeticSequenceQuestionFactory(IMathematicsSolver solver, IMathematicsVerifier verifier)
    {
        this.solver = solver ?? throw new ArgumentNullException(nameof(solver));
        this.verifier = verifier ?? throw new ArgumentNullException(nameof(verifier));
    }

    public VerifiedGeneratedMathematicsProblem Generate(int variantKey, int difficultyBand)
    {
        ExactFunctionsGraphsSequencesGeneration.ValidateDifficulty(difficultyBand);
        var state = ExactFunctionsGraphsSequencesGeneration.Seed(variantKey, 0xBB67AE85u);
        MathNode first;
        MathNode difference;
        if (difficultyBand < 3)
        {
            first = ExactFunctionsGraphsSequencesGeneration.I(
                ExactFunctionsGraphsSequencesGeneration.Next(ref state, -20, 20));
            difference = ExactFunctionsGraphsSequencesGeneration.I(
                ExactFunctionsGraphsSequencesGeneration.NonZeroSigned(ref state, difficultyBand == 1 ? 6 : 12));
        }
        else
        {
            first = ExactFunctionsGraphsSequencesGeneration.RationalValue(
                ExactFunctionsGraphsSequencesGeneration.NonZeroSigned(ref state, 15),
                ExactFunctionsGraphsSequencesGeneration.Next(ref state, 2, 5));
            difference = ExactFunctionsGraphsSequencesGeneration.RationalValue(
                ExactFunctionsGraphsSequencesGeneration.NonZeroSigned(ref state, 12),
                ExactFunctionsGraphsSequencesGeneration.Next(ref state, 2, 5));
        }

        var n = ExactFunctionsGraphsSequencesGeneration.Next(
            ref state,
            difficultyBand == 1 ? 4 : 8,
            difficultyBand == 1 ? 12 : difficultyBand == 2 ? 30 : 60);
        var problem = new FunctionCallNode(
            "arithmetic_nth_term",
            [first, difference, ExactFunctionsGraphsSequencesGeneration.I(n)]);

        return ExactFunctionsGraphsSequencesGeneration.SolveVerifyAndBuild(
            FamilyId,
            Skill,
            problem,
            solver,
            verifier,
            [
                new CapabilityId("rational.exact_arithmetic"),
                new CapabilityId("sequences.arithmetic.nth_term.exact"),
                new CapabilityId("sequences.verify.exact_nth_term")
            ],
            variantKey,
            difficultyBand,
            new Dictionary<string, string>(StringComparer.Ordinal)
            {
                ["n"] = n.ToString(CultureInfo.InvariantCulture)
            });
    }
}

public sealed class ExactGeometricSequenceQuestionFactory
{
    public const string FamilyId = "sequences.geometric.nth_term.exact";
    public static readonly SkillId Skill = new("sequences.geometric.nth_term");

    private readonly IMathematicsSolver solver;
    private readonly IMathematicsVerifier verifier;

    public ExactGeometricSequenceQuestionFactory(IMathematicsSolver solver, IMathematicsVerifier verifier)
    {
        this.solver = solver ?? throw new ArgumentNullException(nameof(solver));
        this.verifier = verifier ?? throw new ArgumentNullException(nameof(verifier));
    }

    public VerifiedGeneratedMathematicsProblem Generate(int variantKey, int difficultyBand)
    {
        ExactFunctionsGraphsSequencesGeneration.ValidateDifficulty(difficultyBand);
        var state = ExactFunctionsGraphsSequencesGeneration.Seed(variantKey, 0x3C6EF372u);
        MathNode first;
        MathNode ratio;
        if (difficultyBand < 3)
        {
            first = ExactFunctionsGraphsSequencesGeneration.I(
                ExactFunctionsGraphsSequencesGeneration.NonZeroSigned(ref state, 12));
            ratio = ExactFunctionsGraphsSequencesGeneration.I(
                ExactFunctionsGraphsSequencesGeneration.NonZeroSigned(ref state, difficultyBand == 1 ? 3 : 5));
        }
        else
        {
            first = ExactFunctionsGraphsSequencesGeneration.RationalValue(
                ExactFunctionsGraphsSequencesGeneration.NonZeroSigned(ref state, 12),
                ExactFunctionsGraphsSequencesGeneration.Next(ref state, 2, 4));
            ratio = ExactFunctionsGraphsSequencesGeneration.RationalValue(
                ExactFunctionsGraphsSequencesGeneration.NonZeroSigned(ref state, 7),
                ExactFunctionsGraphsSequencesGeneration.Next(ref state, 2, 4));
        }

        var n = ExactFunctionsGraphsSequencesGeneration.Next(ref state, 3, difficultyBand == 1 ? 6 : 8);
        var problem = new FunctionCallNode(
            "geometric_nth_term",
            [first, ratio, ExactFunctionsGraphsSequencesGeneration.I(n)]);

        return ExactFunctionsGraphsSequencesGeneration.SolveVerifyAndBuild(
            FamilyId,
            Skill,
            problem,
            solver,
            verifier,
            [
                new CapabilityId("rational.exact_arithmetic"),
                new CapabilityId("sequences.geometric.nth_term.exact"),
                new CapabilityId("sequences.verify.exact_nth_term")
            ],
            variantKey,
            difficultyBand,
            new Dictionary<string, string>(StringComparer.Ordinal)
            {
                ["n"] = n.ToString(CultureInfo.InvariantCulture)
            });
    }
}

internal static class ExactFunctionsGraphsSequencesGeneration
{
    public static void ValidateDifficulty(int difficultyBand)
    {
        if (difficultyBand is < 1 or > 3)
        {
            throw new ArgumentOutOfRangeException(nameof(difficultyBand));
        }
    }

    public static uint Seed(int variantKey, uint salt) =>
        unchecked((uint)variantKey * 2654435761u + salt);

    public static int Next(ref uint state, int min, int maxInclusive)
    {
        state = unchecked(state * 1664525u + 1013904223u);
        return min + (int)(state % (uint)(maxInclusive - min + 1));
    }

    public static int NonZeroSigned(ref uint state, int limit)
    {
        var magnitude = Next(ref state, 1, limit);
        return Next(ref state, 0, 1) == 0 ? magnitude : -magnitude;
    }

    public static IntegerNode I(int value) => new(new BigInteger(value));

    public static MathNode IntegerValue(int value) => I(value);

    public static MathNode RationalValue(int numerator, int denominator) =>
        new RationalNode(new ExactRational(new BigInteger(numerator), new BigInteger(denominator)));

    public static VerifiedGeneratedMathematicsProblem SolveVerifyAndBuild(
        string familyId,
        SkillId skill,
        MathNode problem,
        IMathematicsSolver solver,
        IMathematicsVerifier verifier,
        IReadOnlyList<CapabilityId> capabilities,
        int variantKey,
        int difficultyBand,
        IReadOnlyDictionary<string, string>? extraParameters = null)
    {
        var request = new MathematicsSolveRequest(problem, capabilities, []);
        var solveResult = solver.Solve(request);
        if (solveResult.Status != MathematicsSolveStatus.Solved || solveResult.ExactResult is null)
        {
            throw new InvalidOperationException(
                $"Generated Mathematics V2 problem was not solved successfully: {string.Join("; ", solveResult.Diagnostics)}");
        }

        var verification = verifier.Verify(request, solveResult);
        if (!verification.IsVerified)
        {
            throw new InvalidOperationException(
                $"Generated Mathematics V2 problem failed independent verification: {string.Join("; ", verification.Diagnostics)}");
        }

        var parameters = new Dictionary<string, string>(StringComparer.Ordinal)
        {
            ["variantKey"] = variantKey.ToString(CultureInfo.InvariantCulture),
            ["difficultyBand"] = difficultyBand.ToString(CultureInfo.InvariantCulture)
        };
        if (extraParameters is not null)
        {
            foreach (var pair in extraParameters)
            {
                parameters[pair.Key] = pair.Value;
            }
        }

        return new VerifiedGeneratedMathematicsProblem(
            familyId,
            skill,
            problem,
            solveResult.ExactResult,
            solveResult,
            verification,
            parameters);
    }
}
