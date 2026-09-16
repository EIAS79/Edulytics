using System.Numerics;
using Edulytics.Core.Mathematics.Ast;
using Edulytics.Core.Mathematics.Domains;
using Edulytics.Core.Mathematics.Solving;
using Edulytics.Services.Mathematics.Contracts;

namespace Edulytics.Services.Mathematics.Solving;

/// <summary>
/// Exact Mathematics V2 solver for one-variable quadratic equations over the real
/// numbers. Both sides are normalized to a*x^2+b*x+c=0 using exact rational
/// arithmetic. Rational roots are simplified exactly; irrational real roots remain
/// exact RootNode expressions. Negative discriminants are classified as no real
/// solution and are intentionally left to the later complex-number capability.
/// </summary>
public sealed class ExactQuadraticEquationSolver : IMathematicsSolver
{
    public const string ProviderName = "edulytics-native-quadratic";
    public const string ProviderVersion = "quadratic-real-exact-v1";

    private static readonly ExactRational Zero = new(0, 1);
    private static readonly ExactRational Two = new(2, 1);
    private static readonly ExactRational Four = new(4, 1);

    public MathematicsSolveResult Solve(MathematicsSolveRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (request.Problem is not EquationNode equation)
        {
            return Unsupported("ExactQuadraticEquationSolver requires one equation.");
        }

        var symbols = ExactPolynomial.CollectSymbols(equation.Left)
            .Concat(ExactPolynomial.CollectSymbols(equation.Right))
            .Distinct(StringComparer.Ordinal)
            .OrderBy(x => x, StringComparer.Ordinal)
            .ToArray();
        if (symbols.Length != 1)
        {
            return Unsupported($"Quadratic solving requires exactly one variable; found {symbols.Length}.");
        }

        var variable = symbols[0];
        if (!ExactPolynomial.TryCreate(equation.Left, variable, 2, out var left, out var error)
            || !ExactPolynomial.TryCreate(equation.Right, variable, 2, out var right, out error))
        {
            return Unsupported(error ?? "Equation is outside the exact quadratic subset.");
        }

        var polynomial = left - right;
        var a = polynomial[2];
        var b = polynomial[1];
        var c = polynomial[0];
        if (a == Zero)
        {
            return Unsupported("Normalized equation is not quadratic because the x^2 coefficient is zero.");
        }

        var discriminant = b * b - Four * a * c;
        var normalizedEquation = new EquationNode(
            polynomial.ToCanonicalNode(variable),
            new IntegerNode(BigInteger.Zero));

        if (discriminant.CompareTo(Zero) < 0)
        {
            return new MathematicsSolveResult(
                MathematicsSolveStatus.NoSolution,
                null,
                new EmptySolutionSet(),
                request.Assumptions.ToArray(),
                "quadratic.discriminant-real-classification",
                new MathematicsSolutionTrace([
                    NormalizeStep(equation, normalizedEquation, request.Assumptions),
                    DiscriminantStep(normalizedEquation, discriminant, request.Assumptions)
                ]),
                ProviderName,
                ProviderVersion,
                ["Discriminant is negative; the quadratic has no real roots. Complex roots require the later complex-number capability."]);
        }

        var denominator = Two * a;
        var negativeB = new ExactRational(-b.Numerator, b.Denominator);
        if (discriminant == Zero)
        {
            var root = negativeB / denominator;
            var rootNode = ExactPolynomial.ToNode(root);
            return new MathematicsSolveResult(
                MathematicsSolveStatus.Solved,
                rootNode,
                new FiniteSolutionSet([rootNode]),
                request.Assumptions.ToArray(),
                "quadratic.formula.exact",
                new MathematicsSolutionTrace([
                    NormalizeStep(equation, normalizedEquation, request.Assumptions),
                    DiscriminantStep(normalizedEquation, discriminant, request.Assumptions),
                    FormulaStep(normalizedEquation, rootNode, request.Assumptions)
                ]),
                ProviderName,
                ProviderVersion,
                []);
        }

        MathNode first;
        MathNode second;
        if (TryPerfectSquare(discriminant, out var sqrtDiscriminant))
        {
            var root1 = (negativeB - sqrtDiscriminant) / denominator;
            var root2 = (negativeB + sqrtDiscriminant) / denominator;
            first = ExactPolynomial.ToNode(root1);
            second = ExactPolynomial.ToNode(root2);
        }
        else
        {
            var radical = new RootNode(ExactPolynomial.ToNode(discriminant), 2);
            var denominatorNode = ExactPolynomial.ToNode(denominator);
            first = new DivideNode(
                new AddNode([ExactPolynomial.ToNode(negativeB), new NegateNode(radical)]),
                denominatorNode);
            second = new DivideNode(
                new AddNode([ExactPolynomial.ToNode(negativeB), radical]),
                denominatorNode);
        }

        var roots = new VectorNode([first, second]);
        return new MathematicsSolveResult(
            MathematicsSolveStatus.Solved,
            roots,
            new FiniteSolutionSet([first, second]),
            request.Assumptions.ToArray(),
            "quadratic.formula.exact",
            new MathematicsSolutionTrace([
                NormalizeStep(equation, normalizedEquation, request.Assumptions),
                DiscriminantStep(normalizedEquation, discriminant, request.Assumptions),
                FormulaStep(normalizedEquation, roots, request.Assumptions)
            ]),
            ProviderName,
            ProviderVersion,
            []);
    }

    internal static bool TryPerfectSquare(ExactRational value, out ExactRational root)
    {
        if (value.CompareTo(Zero) < 0)
        {
            root = default;
            return false;
        }

        var numeratorRoot = IntegerSquareRoot(value.Numerator);
        var denominatorRoot = IntegerSquareRoot(value.Denominator);
        if (numeratorRoot * numeratorRoot != value.Numerator
            || denominatorRoot * denominatorRoot != value.Denominator)
        {
            root = default;
            return false;
        }

        root = new ExactRational(numeratorRoot, denominatorRoot);
        return true;
    }

    private static BigInteger IntegerSquareRoot(BigInteger value)
    {
        if (value < BigInteger.Zero)
        {
            throw new ArgumentOutOfRangeException(nameof(value));
        }
        if (value < 2)
        {
            return value;
        }

        var x0 = BigInteger.One << ((GetBitLength(value) + 1) / 2);
        while (true)
        {
            var x1 = (x0 + value / x0) >> 1;
            if (x1 >= x0)
            {
                return x0;
            }
            x0 = x1;
        }
    }

    private static int GetBitLength(BigInteger value)
    {
        var bytes = value.ToByteArray(isUnsigned: true, isBigEndian: true);
        if (bytes.Length == 0)
        {
            return 0;
        }

        var leading = bytes[0];
        var bits = (bytes.Length - 1) * 8;
        while (leading != 0)
        {
            bits++;
            leading >>= 1;
        }
        return bits;
    }

    private static MathematicsSolutionStep NormalizeStep(
        EquationNode original,
        EquationNode normalized,
        IReadOnlyList<MathematicsAssumption> assumptions) =>
        new(
            "normalize-quadratic",
            original,
            "polynomial.normalize.exact",
            "Expand and collect the equation into exact polynomial form a*x^2+b*x+c=0.",
            normalized,
            "Polynomial operations preserve equality and use exact rational coefficients.",
            assumptions.ToArray(),
            true);

    private static MathematicsSolutionStep DiscriminantStep(
        EquationNode normalized,
        ExactRational discriminant,
        IReadOnlyList<MathematicsAssumption> assumptions) =>
        new(
            "compute-discriminant",
            normalized,
            "quadratic.discriminant",
            "Compute D=b^2-4ac exactly.",
            ExactPolynomial.ToNode(discriminant),
            "The sign of the exact discriminant determines the number of real roots.",
            assumptions.ToArray(),
            true);

    private static MathematicsSolutionStep FormulaStep(
        EquationNode normalized,
        MathNode result,
        IReadOnlyList<MathematicsAssumption> assumptions) =>
        new(
            "apply-quadratic-formula",
            normalized,
            "quadratic.formula.exact",
            "Apply x=(-b±sqrt(D))/(2a) without decimal approximation.",
            result,
            "The quadratic formula gives every real root for a non-zero quadratic coefficient.",
            assumptions.ToArray(),
            true);

    private static MathematicsSolveResult Unsupported(string diagnostic) =>
        new(
            MathematicsSolveStatus.Unsupported,
            null,
            null,
            [],
            null,
            null,
            ProviderName,
            ProviderVersion,
            [diagnostic]);
}
