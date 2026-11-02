using System.Globalization;
using System.Numerics;
using Edulytics.Core.Mathematics.Ast;
using Edulytics.Core.Mathematics.Identifiers;
using Edulytics.Core.Mathematics.Solving;
using Edulytics.Services.Mathematics.Contracts;

namespace Edulytics.Services.Mathematics.Generation;

/// <summary>
/// Solver-grounded deterministic generator for exact 2x2 linear systems. It first
/// chooses an exact integer ordered pair, constructs two independent equations
/// through that pair, and publishes the instance only after solver and independent
/// verifier agree on the unique ordered-pair solution.
/// </summary>
public sealed class ExactLinearSystem2x2QuestionFactory
{
    public const string FamilyId = "algebra.linear.systems.two_by_two.integer_coefficients";
    public static readonly SkillId Skill = new("algebra.linear.systems.two_by_two.solve");

    private readonly IMathematicsSolver solver;
    private readonly IMathematicsVerifier verifier;

    public ExactLinearSystem2x2QuestionFactory(
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
            return min + (int)(state % (uint)(maxInclusive - min + 1));
        }

        var solutionLimit = difficultyBand switch
        {
            1 => 6,
            2 => 12,
            _ => 20
        };
        var coefficientLimit = difficultyBand switch
        {
            1 => 5,
            2 => 8,
            _ => 12
        };

        var x = Next(-solutionLimit, solutionLimit);
        var y = Next(-solutionLimit, solutionLimit);

        int a1;
        int b1;
        int a2;
        int b2;
        var attempts = 0;
        do
        {
            a1 = NonZero(Next, coefficientLimit);
            b1 = NonZero(Next, coefficientLimit);
            a2 = NonZero(Next, coefficientLimit);
            b2 = NonZero(Next, coefficientLimit);
            attempts++;
            if (attempts > 50)
            {
                throw new InvalidOperationException("Could not construct a non-singular deterministic 2x2 coefficient matrix.");
            }
        }
        while (a1 * b2 - a2 * b1 == 0);

        var c1 = checked(a1 * x + b1 * y);
        var c2 = checked(a2 * x + b2 * y);
        var system = new EquationSystemNode([
            Equation(a1, b1, c1),
            Equation(a2, b2, c2)
        ]);

        var request = new MathematicsSolveRequest(
            system,
            [
                new CapabilityId("rational.exact_arithmetic"),
                new CapabilityId("algebra.linear.systems.two_by_two.solve.exact_rational"),
                new CapabilityId("algebra.linear.systems.two_by_two.verify.substitution")
            ],
            []);
        var solveResult = solver.Solve(request);
        if (solveResult.Status != MathematicsSolveStatus.Solved || solveResult.ExactResult is not VectorNode)
        {
            throw new InvalidOperationException(
                $"Generated 2x2 system was not solved successfully: {string.Join("; ", solveResult.Diagnostics)}");
        }

        var verification = verifier.Verify(request, solveResult);
        if (!verification.IsVerified)
        {
            throw new InvalidOperationException(
                $"Generated 2x2 system failed independent verification: {string.Join("; ", verification.Diagnostics)}");
        }

        return new VerifiedGeneratedMathematicsProblem(
            FamilyId,
            Skill,
            system,
            solveResult.ExactResult,
            solveResult,
            verification,
            new Dictionary<string, string>(StringComparer.Ordinal)
            {
                ["variantKey"] = variantKey.ToString(CultureInfo.InvariantCulture),
                ["difficultyBand"] = difficultyBand.ToString(CultureInfo.InvariantCulture),
                ["a1"] = a1.ToString(CultureInfo.InvariantCulture),
                ["b1"] = b1.ToString(CultureInfo.InvariantCulture),
                ["c1"] = c1.ToString(CultureInfo.InvariantCulture),
                ["a2"] = a2.ToString(CultureInfo.InvariantCulture),
                ["b2"] = b2.ToString(CultureInfo.InvariantCulture),
                ["c2"] = c2.ToString(CultureInfo.InvariantCulture)
            });
    }

    private static EquationNode Equation(int a, int b, int c) =>
        new(
            new AddNode([
                new MultiplyNode([new IntegerNode(new BigInteger(a)), new SymbolNode("x")]),
                new MultiplyNode([new IntegerNode(new BigInteger(b)), new SymbolNode("y")])
            ]),
            new IntegerNode(new BigInteger(c)));

    private static int NonZero(Func<int, int, int> next, int limit)
    {
        var value = next(-limit, limit);
        return value == 0 ? 1 : value;
    }
}
