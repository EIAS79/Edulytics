using System.Numerics;
using Edulytics.Core.Mathematics.Ast;
using Edulytics.Core.Mathematics.Identifiers;
using Edulytics.Core.Mathematics.Solving;
using Edulytics.Services.Mathematics.Contracts;

namespace Edulytics.Services.Mathematics.Generation;

/// <summary>
/// Solver-grounded shadow generator for exact single-variable affine inequalities.
/// Every generated problem is accepted only after the configured exact solver and
/// independent verifier agree on the normalized inequality and typed solution set.
/// </summary>
public sealed class ExactLinearInequalityQuestionFactory
{
    public const string FamilyId = "algebra.linear.inequality.ax_plus_b_relation_c";
    public static readonly SkillId Skill = new("algebra.linear.inequality.solve");

    private readonly IMathematicsSolver solver;
    private readonly IMathematicsVerifier verifier;

    public ExactLinearInequalityQuestionFactory(
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

        var state = unchecked((uint)variantKey * 2246822519u + 3266489917u);
        int Next(int min, int maxInclusive)
        {
            state = unchecked(state * 1664525u + 1013904223u);
            var width = (uint)(maxInclusive - min + 1);
            return min + (int)(state % width);
        }

        var boundaryLimit = difficultyBand switch
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

        var boundary = Next(-boundaryLimit, boundaryLimit);
        var coefficient = NonZeroSigned(Next, coefficientLimit);
        var offset = Next(-offsetLimit, offsetLimit);
        var right = checked(coefficient * boundary + offset);
        var relation = (InequalityRelation)Next(
            (int)InequalityRelation.LessThan,
            (int)InequalityRelation.GreaterThanOrEqual);

        MathNode left = new AddNode([
            new MultiplyNode([
                new IntegerNode(new BigInteger(coefficient)),
                new SymbolNode("x")
            ]),
            new IntegerNode(new BigInteger(offset))
        ]);
        var inequality = new InequalityNode(
            left,
            relation,
            new IntegerNode(new BigInteger(right)));

        var request = new MathematicsSolveRequest(
            inequality,
            [
                new CapabilityId("rational.exact_arithmetic"),
                new CapabilityId("algebra.linear.affine_extract"),
                new CapabilityId("algebra.linear.inequality.solve.exact_rational")
            ],
            []);
        var solveResult = solver.Solve(request);
        if (solveResult.Status != MathematicsSolveStatus.Solved || solveResult.ExactResult is null)
        {
            throw new InvalidOperationException(
                $"Generated linear inequality was not solved successfully: {string.Join("; ", solveResult.Diagnostics)}");
        }

        var verification = verifier.Verify(request, solveResult);
        if (!verification.IsVerified)
        {
            throw new InvalidOperationException(
                $"Generated linear inequality failed independent verification: {string.Join("; ", verification.Diagnostics)}");
        }

        return new VerifiedGeneratedMathematicsProblem(
            FamilyId,
            Skill,
            inequality,
            solveResult.ExactResult,
            solveResult,
            verification,
            new Dictionary<string, string>(StringComparer.Ordinal)
            {
                ["variantKey"] = variantKey.ToString(System.Globalization.CultureInfo.InvariantCulture),
                ["difficultyBand"] = difficultyBand.ToString(System.Globalization.CultureInfo.InvariantCulture),
                ["coefficient"] = coefficient.ToString(System.Globalization.CultureInfo.InvariantCulture),
                ["offset"] = offset.ToString(System.Globalization.CultureInfo.InvariantCulture),
                ["right"] = right.ToString(System.Globalization.CultureInfo.InvariantCulture),
                ["relation"] = relation.ToString()
            });
    }

    private static int NonZeroSigned(Func<int, int, int> next, int limit)
    {
        var magnitude = next(1, limit);
        return next(0, 1) == 0 ? magnitude : -magnitude;
    }
}
