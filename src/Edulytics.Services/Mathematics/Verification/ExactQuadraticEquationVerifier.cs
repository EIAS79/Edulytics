using System.Numerics;
using Edulytics.Core.Mathematics.Ast;
using Edulytics.Core.Mathematics.Domains;
using Edulytics.Core.Mathematics.Solving;
using Edulytics.Core.Mathematics.Verification;
using Edulytics.Services.Mathematics.Contracts;
using Edulytics.Services.Mathematics.Solving;

namespace Edulytics.Services.Mathematics.Verification;

/// <summary>
/// Independent verifier for the exact real-quadratic slice. It re-derives the
/// degree/discriminant classification, then substitutes every reported rational or
/// quadratic-surd root into both sides of the original equation using exact
/// arithmetic. It does not trust the solver trace or a decimal approximation.
/// </summary>
public sealed class ExactQuadraticEquationVerifier : IMathematicsVerifier
{
    private static readonly ExactRational Zero = new(0, 1);
    private static readonly ExactRational Four = new(4, 1);

    public MathematicsVerificationResult Verify(
        MathematicsSolveRequest request,
        MathematicsSolveResult result)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(result);

        if (request.Problem is not EquationNode equation)
        {
            return Unsupported("Quadratic verifier requires one equation.");
        }

        var symbols = ExactPolynomial.CollectSymbols(equation.Left)
            .Concat(ExactPolynomial.CollectSymbols(equation.Right))
            .Distinct(StringComparer.Ordinal)
            .OrderBy(x => x, StringComparer.Ordinal)
            .ToArray();
        if (symbols.Length != 1)
        {
            return Unsupported("Quadratic verifier requires exactly one variable.");
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
            return Unsupported("Normalized equation is not quadratic.");
        }

        var discriminant = b * b - Four * a * c;
        var discriminantSign = discriminant.CompareTo(Zero);
        if (discriminantSign < 0)
        {
            return result.Status == MathematicsSolveStatus.NoSolution
                   && result.ExactResult is null
                   && result.SolutionSet is EmptySolutionSet
                ? Verified(
                    "exact-negative-discriminant",
                    "The independently computed discriminant is negative, so there are no real roots.")
                : Rejected("Negative-discriminant quadratic must be reported as no real solution.");
        }

        if (result.Status != MathematicsSolveStatus.Solved
            || result.SolutionSet is not FiniteSolutionSet finite)
        {
            return Rejected("Non-negative-discriminant quadratic must return an exact finite real solution set.");
        }

        var expectedCount = discriminantSign == 0 ? 1 : 2;
        if (finite.Values.Count != expectedCount)
        {
            return Rejected($"Quadratic solution set must contain exactly {expectedCount} distinct real root(s).");
        }

        var parsedRoots = new List<QuadraticSurdValue>();
        foreach (var rootNode in finite.Values)
        {
            if (!QuadraticSurdValue.TryParse(rootNode, out var root, out var parseError))
            {
                return Rejected(parseError ?? "A reported root is not an exact rational/quadratic-surd value.");
            }

            if (!ExactQuadraticSurdEvaluator.TryEvaluate(
                    equation.Left,
                    variable,
                    root,
                    out var leftValue,
                    out var leftError))
            {
                return Unsupported(leftError ?? "Could not evaluate the original left side exactly.");
            }
            if (!ExactQuadraticSurdEvaluator.TryEvaluate(
                    equation.Right,
                    variable,
                    root,
                    out var rightValue,
                    out var rightError))
            {
                return Unsupported(rightError ?? "Could not evaluate the original right side exactly.");
            }
            if (!leftValue.EqualsExact(rightValue))
            {
                return Rejected("A reported root fails exact substitution into the original equation.");
            }

            parsedRoots.Add(root);
        }

        if (expectedCount == 2 && parsedRoots[0].EqualsExact(parsedRoots[1]))
        {
            return Rejected("Positive-discriminant quadratic must expose two distinct roots.");
        }

        if (!ResultPayloadMatches(result.ExactResult, finite.Values))
        {
            return Rejected("ExactResult payload does not match the finite quadratic solution set.");
        }

        return Verified(
            "exact-original-equation-substitution",
            $"All {expectedCount} reported real root(s) satisfy both sides of the original equation exactly; discriminant classification confirms completeness.");
    }

    private static bool ResultPayloadMatches(MathNode? exactResult, IReadOnlyList<MathNode> roots)
    {
        if (roots.Count == 1)
        {
            if (exactResult is null
                || !QuadraticSurdValue.TryParse(exactResult, out var reported, out _)
                || !QuadraticSurdValue.TryParse(roots[0], out var expected, out _))
            {
                return false;
            }
            return reported.EqualsExact(expected);
        }

        if (exactResult is not VectorNode vector || vector.Components.Count != roots.Count)
        {
            return false;
        }

        for (var i = 0; i < roots.Count; i++)
        {
            if (!QuadraticSurdValue.TryParse(vector.Components[i], out var reported, out _)
                || !QuadraticSurdValue.TryParse(roots[i], out var expected, out _)
                || !reported.EqualsExact(expected))
            {
                return false;
            }
        }
        return true;
    }

    private static MathematicsVerificationResult Verified(string method, string description) =>
        new(
            MathematicsVerificationStatus.Verified,
            [new MathematicsVerificationEvidence(method, description)],
            []);

    private static MathematicsVerificationResult Rejected(string diagnostic) =>
        new(MathematicsVerificationStatus.Rejected, [], [diagnostic]);

    private static MathematicsVerificationResult Unsupported(string diagnostic) =>
        new(MathematicsVerificationStatus.Unsupported, [], [diagnostic]);
}

internal readonly record struct QuadraticSurdValue(
    ExactRational RationalPart,
    ExactRational RadicalCoefficient,
    ExactRational Radicand)
{
    private static readonly ExactRational Zero = new(0, 1);
    private static readonly ExactRational One = new(1, 1);

    public bool EqualsExact(QuadraticSurdValue other)
    {
        if (RadicalCoefficient == Zero && other.RadicalCoefficient == Zero)
        {
            return RationalPart == other.RationalPart;
        }
        return RationalPart == other.RationalPart
            && RadicalCoefficient == other.RadicalCoefficient
            && Radicand == other.Radicand;
    }

    public static bool TryParse(MathNode node, out QuadraticSurdValue value, out string? error) =>
        TryEvaluateNode(node, out value, out error);

    public static bool TryEvaluateNode(MathNode node, out QuadraticSurdValue value, out string? error)
    {
        switch (node)
        {
            case IntegerNode integer:
                value = Rational(new ExactRational(integer.Value, BigInteger.One));
                error = null;
                return true;
            case RationalNode rational:
                value = Rational(rational.Value);
                error = null;
                return true;
            case NegateNode negate:
                if (!TryEvaluateNode(negate.Operand, out var operand, out error))
                {
                    value = default;
                    return false;
                }
                value = operand.Negate();
                error = null;
                return true;
            case AddNode add:
                var sum = Rational(Zero);
                foreach (var term in add.Terms)
                {
                    if (!TryEvaluateNode(term, out var next, out error)
                        || !TryAdd(sum, next, out sum, out error))
                    {
                        value = default;
                        return false;
                    }
                }
                value = sum;
                error = null;
                return true;
            case MultiplyNode multiply:
                var product = Rational(One);
                foreach (var factor in multiply.Factors)
                {
                    if (!TryEvaluateNode(factor, out var next, out error)
                        || !TryMultiply(product, next, out product, out error))
                    {
                        value = default;
                        return false;
                    }
                }
                value = product;
                error = null;
                return true;
            case DivideNode divide:
                if (!TryEvaluateNode(divide.Numerator, out var numerator, out error)
                    || !TryEvaluateNode(divide.Denominator, out var denominator, out error))
                {
                    value = default;
                    return false;
                }
                if (denominator.RadicalCoefficient != Zero || denominator.RationalPart == Zero)
                {
                    value = default;
                    error = "Exact quadratic-surd division currently requires a non-zero rational denominator.";
                    return false;
                }
                value = new QuadraticSurdValue(
                    numerator.RationalPart / denominator.RationalPart,
                    numerator.RadicalCoefficient / denominator.RationalPart,
                    numerator.Radicand);
                error = null;
                return true;
            case RootNode root when root.Degree == 2:
                if (!TryReadRational(root.Radicand, out var radicand) || radicand.CompareTo(Zero) < 0)
                {
                    value = default;
                    error = "Quadratic surd radicand must be a non-negative exact rational.";
                    return false;
                }
                value = new QuadraticSurdValue(Zero, One, radicand);
                error = null;
                return true;
            default:
                value = default;
                error = $"Node type {node.GetType().Name} is unsupported by the exact quadratic-surd evaluator.";
                return false;
        }
    }

    public QuadraticSurdValue Negate() =>
        new(
            new ExactRational(-RationalPart.Numerator, RationalPart.Denominator),
            new ExactRational(-RadicalCoefficient.Numerator, RadicalCoefficient.Denominator),
            Radicand);

    public static QuadraticSurdValue Rational(ExactRational value) =>
        new(value, Zero, Zero);

    public static bool TryAdd(
        QuadraticSurdValue left,
        QuadraticSurdValue right,
        out QuadraticSurdValue value,
        out string? error)
    {
        if (!TryResolveRadicand(left, right, out var radicand, out error))
        {
            value = default;
            return false;
        }
        value = new QuadraticSurdValue(
            left.RationalPart + right.RationalPart,
            left.RadicalCoefficient + right.RadicalCoefficient,
            radicand);
        error = null;
        return true;
    }

    public static bool TryMultiply(
        QuadraticSurdValue left,
        QuadraticSurdValue right,
        out QuadraticSurdValue value,
        out string? error)
    {
        if (!TryResolveRadicand(left, right, out var radicand, out error))
        {
            value = default;
            return false;
        }

        var rational = left.RationalPart * right.RationalPart
            + left.RadicalCoefficient * right.RadicalCoefficient * radicand;
        var radical = left.RationalPart * right.RadicalCoefficient
            + left.RadicalCoefficient * right.RationalPart;
        value = new QuadraticSurdValue(rational, radical, radicand);
        error = null;
        return true;
    }

    private static bool TryResolveRadicand(
        QuadraticSurdValue left,
        QuadraticSurdValue right,
        out ExactRational radicand,
        out string? error)
    {
        var leftHasRadical = left.RadicalCoefficient != Zero;
        var rightHasRadical = right.RadicalCoefficient != Zero;
        if (leftHasRadical && rightHasRadical && left.Radicand != right.Radicand)
        {
            radicand = default;
            error = "Operation contains incompatible quadratic radicals.";
            return false;
        }

        radicand = leftHasRadical ? left.Radicand : rightHasRadical ? right.Radicand : Zero;
        error = null;
        return true;
    }

    private static bool TryReadRational(MathNode node, out ExactRational value)
    {
        switch (node)
        {
            case IntegerNode integer:
                value = new ExactRational(integer.Value, BigInteger.One);
                return true;
            case RationalNode rational:
                value = rational.Value;
                return true;
            default:
                value = default;
                return false;
        }
    }
}

internal static class ExactQuadraticSurdEvaluator
{
    private static readonly ExactRational Zero = new(0, 1);
    private static readonly ExactRational One = new(1, 1);

    public static bool TryEvaluate(
        MathNode node,
        string variable,
        QuadraticSurdValue variableValue,
        out QuadraticSurdValue value,
        out string? error)
    {
        switch (node)
        {
            case IntegerNode integer:
                value = QuadraticSurdValue.Rational(new ExactRational(integer.Value, BigInteger.One));
                error = null;
                return true;
            case RationalNode rational:
                value = QuadraticSurdValue.Rational(rational.Value);
                error = null;
                return true;
            case SymbolNode symbol when string.Equals(symbol.Name, variable, StringComparison.Ordinal):
                value = variableValue;
                error = null;
                return true;
            case SymbolNode symbol:
                value = default;
                error = $"Unexpected symbol {symbol.Name}.";
                return false;
            case NegateNode negate:
                if (!TryEvaluate(negate.Operand, variable, variableValue, out var operand, out error))
                {
                    value = default;
                    return false;
                }
                value = operand.Negate();
                error = null;
                return true;
            case AddNode add:
                var sum = QuadraticSurdValue.Rational(Zero);
                foreach (var term in add.Terms)
                {
                    if (!TryEvaluate(term, variable, variableValue, out var next, out error)
                        || !QuadraticSurdValue.TryAdd(sum, next, out sum, out error))
                    {
                        value = default;
                        return false;
                    }
                }
                value = sum;
                error = null;
                return true;
            case MultiplyNode multiply:
                var product = QuadraticSurdValue.Rational(One);
                foreach (var factor in multiply.Factors)
                {
                    if (!TryEvaluate(factor, variable, variableValue, out var next, out error)
                        || !QuadraticSurdValue.TryMultiply(product, next, out product, out error))
                    {
                        value = default;
                        return false;
                    }
                }
                value = product;
                error = null;
                return true;
            case DivideNode divide:
                if (!TryEvaluate(divide.Numerator, variable, variableValue, out var numerator, out error)
                    || !TryEvaluate(divide.Denominator, variable, variableValue, out var denominator, out error))
                {
                    value = default;
                    return false;
                }
                if (denominator.RadicalCoefficient != Zero || denominator.RationalPart == Zero)
                {
                    value = default;
                    error = "Original polynomial expression has a non-rational or zero denominator after substitution.";
                    return false;
                }
                value = new QuadraticSurdValue(
                    numerator.RationalPart / denominator.RationalPart,
                    numerator.RadicalCoefficient / denominator.RationalPart,
                    numerator.Radicand);
                error = null;
                return true;
            case PowerNode power:
                if (power.Exponent is not IntegerNode exponentNode
                    || exponentNode.Value < BigInteger.Zero
                    || exponentNode.Value > new BigInteger(8))
                {
                    value = default;
                    error = "Exact verifier supports bounded non-negative integer powers only.";
                    return false;
                }
                if (!TryEvaluate(power.Base, variable, variableValue, out var baseValue, out error))
                {
                    value = default;
                    return false;
                }
                var result = QuadraticSurdValue.Rational(One);
                for (var i = 0; i < (int)exponentNode.Value; i++)
                {
                    if (!QuadraticSurdValue.TryMultiply(result, baseValue, out result, out error))
                    {
                        value = default;
                        return false;
                    }
                }
                value = result;
                error = null;
                return true;
            default:
                value = default;
                error = $"Node type {node.GetType().Name} is unsupported by exact original-equation substitution.";
                return false;
        }
    }
}
