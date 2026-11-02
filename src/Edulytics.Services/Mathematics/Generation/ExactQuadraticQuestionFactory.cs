using System.Globalization;
using System.Numerics;
using Edulytics.Core.Mathematics.Ast;
using Edulytics.Core.Mathematics.Identifiers;
using Edulytics.Core.Mathematics.Solving;
using Edulytics.Services.Mathematics.Contracts;

namespace Edulytics.Services.Mathematics.Generation;

/// <summary>
/// Solver-grounded deterministic generator for real quadratic equations. Easy and
/// medium variants are constructed from integer roots; challenging variants use
/// positive non-square discriminants so the exact answer exercises quadratic-surd
/// handling. Every problem is published only after solve + independent verify.
/// </summary>
public sealed class ExactQuadraticQuestionFactory
{
    public const string FamilyId = "algebra.quadratic.solve.integer_coefficients.real_roots";
    public static readonly SkillId Skill = new("algebra.quadratic.solve.real_roots");

    private static readonly (int A, int B, int C)[] IrrationalCatalogue =
    [
        (1, 1, -1),   // D=5
        (1, 2, -1),   // D=8
        (2, 3, -1),   // D=17
        (2, 1, -2),   // D=17
        (3, 1, -1),   // D=13
        (1, 3, -1)    // D=13
    ];

    private readonly IMathematicsSolver solver;
    private readonly IMathematicsVerifier verifier;

    public ExactQuadraticQuestionFactory(
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

        var state = unchecked((uint)variantKey * 2654435761u + 2246822519u);
        int Next(int min, int maxInclusive)
        {
            state = unchecked(state * 1664525u + 1013904223u);
            return min + (int)(state % (uint)(maxInclusive - min + 1));
        }

        int a;
        int b;
        int c;
        if (difficultyBand < 3)
        {
            var limit = difficultyBand == 1 ? 6 : 12;
            var r1 = Next(-limit, limit);
            var r2 = Next(-limit, limit);
            if (r1 == r2)
            {
                r2 = r2 == limit ? r2 - 1 : r2 + 1;
            }

            a = difficultyBand == 1 ? 1 : Next(2, 4);
            if (Next(0, 1) == 1)
            {
                a = -a;
            }
            b = checked(-a * (r1 + r2));
            c = checked(a * r1 * r2);
        }
        else
        {
            var item = IrrationalCatalogue[(int)(state % (uint)IrrationalCatalogue.Length)];
            a = item.A;
            b = item.B;
            c = item.C;
            if (Next(0, 1) == 1)
            {
                a = -a;
                b = -b;
                c = -c;
            }
        }

        var equation = new EquationNode(
            new AddNode([
                new MultiplyNode([
                    new IntegerNode(new BigInteger(a)),
                    new PowerNode(new SymbolNode("x"), new IntegerNode(new BigInteger(2)))
                ]),
                new MultiplyNode([
                    new IntegerNode(new BigInteger(b)),
                    new SymbolNode("x")
                ]),
                new IntegerNode(new BigInteger(c))
            ]),
            new IntegerNode(BigInteger.Zero));

        var request = new MathematicsSolveRequest(
            equation,
            [
                new CapabilityId("rational.exact_arithmetic"),
                new CapabilityId("algebra.polynomial.normalize.exact_rational"),
                new CapabilityId("algebra.quadratic.solve.real.exact"),
                new CapabilityId("algebra.quadratic.verify.exact_substitution")
            ],
            []);
        var solveResult = solver.Solve(request);
        if (solveResult.Status != MathematicsSolveStatus.Solved || solveResult.ExactResult is null)
        {
            throw new InvalidOperationException(
                $"Generated quadratic problem was not solved successfully: {string.Join("; ", solveResult.Diagnostics)}");
        }

        var verification = verifier.Verify(request, solveResult);
        if (!verification.IsVerified)
        {
            throw new InvalidOperationException(
                $"Generated quadratic problem failed independent verification: {string.Join("; ", verification.Diagnostics)}");
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
                ["variantKey"] = variantKey.ToString(CultureInfo.InvariantCulture),
                ["difficultyBand"] = difficultyBand.ToString(CultureInfo.InvariantCulture),
                ["a"] = a.ToString(CultureInfo.InvariantCulture),
                ["b"] = b.ToString(CultureInfo.InvariantCulture),
                ["c"] = c.ToString(CultureInfo.InvariantCulture)
            });
    }
}
