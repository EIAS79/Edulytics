using System.Numerics;
using Edulytics.Core.Mathematics.Ast;
using Edulytics.Core.Mathematics.Identifiers;
using Edulytics.Core.Mathematics.Solving;
using Edulytics.Core.Mathematics.Verification;
using Edulytics.Services.Mathematics.Contracts;

namespace Edulytics.Services.Mathematics.Generation;

public sealed record VerifiedGeneratedMathematicsProblem(
    string QuestionFamilyId,
    SkillId Skill,
    MathNode Problem,
    MathNode ExpectedAnswer,
    MathematicsSolveResult SolveResult,
    MathematicsVerificationResult Verification,
    IReadOnlyDictionary<string, string> Parameters);

/// <summary>
/// Solver-grounded shadow generator for the first Mathematics V2 vertical slice.
/// It constructs a deterministic a*x+b=c instance, then accepts the instance only
/// after the configured solver and independent verifier agree on the exact answer.
/// </summary>
public sealed class ExactLinearEquationQuestionFactory
{
    public const string FamilyId = "algebra.linear.ax_plus_b_equals_c";
    public static readonly SkillId Skill = new("algebra.linear.solve");

    private readonly IMathematicsSolver solver;
    private readonly IMathematicsVerifier verifier;

    public ExactLinearEquationQuestionFactory(
        IMathematicsSolver solver,
        IMathematicsVerifier verifier)
    {
        this.solver = solver ?? throw new ArgumentNullException(nameof(solver));
        this.verifier = verifier ?? throw new ArgumentNullException(nameof(verifier));
    }

    public VerifiedGeneratedMathematicsProblem Generate(int variantKey, int difficultyBand)
    {
        if (difficultyBand is < 1 or > 3)
        {
            throw new ArgumentOutOfRangeException(nameof(difficultyBand));
        }

        var state = unchecked((uint)variantKey * 747796405u + 2891336453u);
        int Next(int min, int maxInclusive)
        {
            state = unchecked(state * 1664525u + 1013904223u);
            var width = (uint)(maxInclusive - min + 1);
            return min + (int)(state % width);
        }

        var solutionLimit = difficultyBand switch
        {
            1 => 8,
            2 => 15,
            _ => 30
        };
        var coefficientLimit = difficultyBand switch
        {
            1 => 5,
            2 => 9,
            _ => 12
        };
        var offsetLimit = difficultyBand switch
        {
            1 => 10,
            2 => 25,
            _ => 50
        };

        var solution = NonZeroSigned(Next, solutionLimit);
        var coefficient = NonZeroSigned(Next, coefficientLimit);
        var offset = Next(-offsetLimit, offsetLimit);
        var right = checked(coefficient * solution + offset);

        MathNode left = new AddNode([
            new MultiplyNode([
                new IntegerNode(new BigInteger(coefficient)),
                new SymbolNode("x")
            ]),
            new IntegerNode(new BigInteger(offset))
        ]);
        var equation = new EquationNode(
            left,
            new IntegerNode(new BigInteger(right)));

        var request = new MathematicsSolveRequest(
            equation,
            [
                new CapabilityId("rational.exact_arithmetic"),
                new CapabilityId("algebra.linear.solve.exact_rational")
            ],
            []);
        var solveResult = solver.Solve(request);
        if (solveResult.Status != MathematicsSolveStatus.Solved || solveResult.ExactResult is null)
        {
            throw new InvalidOperationException(
                $"Generated linear problem was not solved successfully: {string.Join("; ", solveResult.Diagnostics)}");
        }

        var verification = verifier.Verify(request, solveResult);
        if (!verification.IsVerified)
        {
            throw new InvalidOperationException(
                $"Generated linear problem failed independent verification: {string.Join("; ", verification.Diagnostics)}");
        }

        return new VerifiedGeneratedMathematicsProblem(
            FamilyId,
            Skill,
            equation,
            solveResult.ExactResult,
            solveResult,
            verification,
            new Dictionary<string, string>(StringComparer.Ordinal)
            {
                ["variantKey"] = variantKey.ToString(System.Globalization.CultureInfo.InvariantCulture),
                ["difficultyBand"] = difficultyBand.ToString(System.Globalization.CultureInfo.InvariantCulture),
                ["coefficient"] = coefficient.ToString(System.Globalization.CultureInfo.InvariantCulture),
                ["offset"] = offset.ToString(System.Globalization.CultureInfo.InvariantCulture),
                ["right"] = right.ToString(System.Globalization.CultureInfo.InvariantCulture)
            });
    }

    private static int NonZeroSigned(Func<int, int, int> next, int limit)
    {
        var magnitude = next(1, limit);
        return next(0, 1) == 0 ? magnitude : -magnitude;
    }
}
