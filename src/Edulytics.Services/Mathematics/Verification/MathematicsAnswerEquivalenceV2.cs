using System.Globalization;
using System.Numerics;
using Edulytics.Core.Mathematics.Ast;
using Edulytics.Core.Mathematics.Domains;
using Edulytics.Services.Mathematics.Contracts;

namespace Edulytics.Services.Mathematics.Verification;

/// <summary>
/// Shared exact answer-equivalence evaluator for Mathematics V2. It deliberately
/// fails closed outside the reviewed normalization subset and never uses floating
/// point as an exactness oracle.
/// </summary>
public sealed class MathematicsAnswerEquivalenceV2 : IMathematicsAnswerEvaluator
{
    private const int MaxPower = 64;
    private const int MaxBits = 4096;

    public MathematicsAnswerEvaluation Evaluate(MathNode submitted, MathNode expected, MathematicsAnswerEvaluationPolicy policy)
    {
        ArgumentNullException.ThrowIfNull(submitted);
        ArgumentNullException.ThrowIfNull(expected);
        ArgumentNullException.ThrowIfNull(policy);

        if (TryEvaluateRational(submitted, out var submittedValue)
            && TryEvaluateRational(expected, out var expectedValue))
        {
            if (submittedValue == expectedValue)
            {
                return Equivalent("exact-rational-normalization");
            }

            if (!policy.RequireExact && policy.AbsoluteTolerance is { } tolerance
                && TryToDecimal(submittedValue, out var submittedDecimal)
                && TryToDecimal(expectedValue, out var expectedDecimal)
                && decimal.Abs(submittedDecimal - expectedDecimal) <= tolerance)
            {
                return Equivalent("declared-decimal-absolute-tolerance");
            }

            return Different("Exact rational values differ.");
        }

        var submittedKey = CanonicalKey(submitted);
        var expectedKey = CanonicalKey(expected);
        if (submittedKey is not null && expectedKey is not null && submittedKey == expectedKey)
        {
            return Equivalent("canonical-symbolic-structure");
        }

        return Different("Answers are not equivalent in the reviewed exact V2 normalization subset.");
    }

    public bool AreEquivalentSolutionSets(SolutionSet submitted, SolutionSet expected)
    {
        ArgumentNullException.ThrowIfNull(submitted);
        ArgumentNullException.ThrowIfNull(expected);
        return SolutionSetKey(submitted) is { } left && SolutionSetKey(expected) is { } right && left == right;
    }

    private static MathematicsAnswerEvaluation Equivalent(string method) => new(true, method, []);
    private static MathematicsAnswerEvaluation Different(string diagnostic) => new(false, "not-equivalent", [diagnostic]);

    private static string? SolutionSetKey(SolutionSet set) => set switch
    {
        EmptySolutionSet => "empty",
        AllRealNumbersSolutionSet => "all-real",
        FiniteSolutionSet finite => FiniteSetKey(finite),
        IntervalSolutionSet interval => IntervalKey(interval),
        UnionSolutionSet union => UnionKey(union),
        _ => null
    };

    private static string? FiniteSetKey(FiniteSolutionSet set)
    {
        var values = set.Values.Select(CanonicalKey).ToArray();
        if (values.Any(value => value is null))
        {
            return null;
        }
        return "finite{" + string.Join(",", values.Select(value => value!).Distinct(StringComparer.Ordinal).OrderBy(value => value, StringComparer.Ordinal)) + "}";
    }

    private static string? IntervalKey(IntervalSolutionSet interval)
    {
        var lower = interval.Lower is null ? "-inf" : CanonicalKey(interval.Lower);
        var upper = interval.Upper is null ? "+inf" : CanonicalKey(interval.Upper);
        if (lower is null || upper is null)
        {
            return null;
        }
        return $"interval:{interval.LowerBoundary}:{lower}:{upper}:{interval.UpperBoundary}";
    }

    private static string? UnionKey(UnionSolutionSet union)
    {
        var sets = union.Sets.Select(SolutionSetKey).ToArray();
        if (sets.Any(set => set is null))
        {
            return null;
        }
        return "union{" + string.Join(",", sets.Select(set => set!).OrderBy(set => set, StringComparer.Ordinal)) + "}";
    }

    private static string? CanonicalKey(MathNode node)
    {
        if (TryEvaluateRational(node, out var numeric))
        {
            return $"q:{numeric.Numerator.ToString(CultureInfo.InvariantCulture)}/{numeric.Denominator.ToString(CultureInfo.InvariantCulture)}";
        }

        switch (node)
        {
            case SymbolNode symbol:
                return "s:" + symbol.Name;
            case NegateNode negate:
                return Wrap("neg", CanonicalKey(negate.Operand));
            case AddNode add:
                return Commutative("add", add.Terms);
            case MultiplyNode multiply:
                return Commutative("mul", multiply.Factors);
            case DivideNode divide:
                return Pair("div", CanonicalKey(divide.Numerator), CanonicalKey(divide.Denominator));
            case PowerNode power:
                return Pair("pow", CanonicalKey(power.Base), CanonicalKey(power.Exponent));
            case RootNode root:
                return Pair("root:" + root.Degree.ToString(CultureInfo.InvariantCulture), CanonicalKey(root.Radicand), "");
            case EquationNode equation:
            {
                var left = CanonicalKey(equation.Left);
                var right = CanonicalKey(equation.Right);
                if (left is null || right is null)
                {
                    return null;
                }
                return string.CompareOrdinal(left, right) <= 0 ? $"eq({left},{right})" : $"eq({right},{left})";
            }
            case EquationSystemNode system:
            {
                var equations = system.Equations.Select(CanonicalKey).ToArray();
                return equations.Any(x => x is null) ? null : "system{" + string.Join(",", equations.Select(x => x!).OrderBy(x => x, StringComparer.Ordinal)) + "}";
            }
            case InequalityNode inequality:
                return Triple("ineq:" + inequality.Relation, CanonicalKey(inequality.Left), CanonicalKey(inequality.Right), "");
            case FunctionCallNode call:
            {
                var args = call.Arguments.Select(CanonicalKey).ToArray();
                return args.Any(x => x is null) ? null : $"fn:{call.FunctionName}({string.Join(",", args.Select(x => x!))})";
            }
            case VectorNode vector:
            {
                var items = vector.Components.Select(CanonicalKey).ToArray();
                return items.Any(x => x is null) ? null : "vec[" + string.Join(",", items.Select(x => x!)) + "]";
            }
            case MatrixNode matrix:
            {
                var rows = new List<string>();
                foreach (var row in matrix.Rows)
                {
                    var cells = row.Select(CanonicalKey).ToArray();
                    if (cells.Any(x => x is null))
                    {
                        return null;
                    }
                    rows.Add("[" + string.Join(",", cells.Select(x => x!)) + "]");
                }
                return "mat[" + string.Join(";", rows) + "]";
            }
            case DerivativeNode derivative:
                return Triple("derivative:" + derivative.Order.ToString(CultureInfo.InvariantCulture), CanonicalKey(derivative.Expression), CanonicalKey(derivative.Variable), "");
            case IntegralNode integral:
            {
                var integrand = CanonicalKey(integral.Integrand);
                var variable = CanonicalKey(integral.Variable);
                var lower = integral.LowerBound is null ? "none" : CanonicalKey(integral.LowerBound);
                var upper = integral.UpperBound is null ? "none" : CanonicalKey(integral.UpperBound);
                return integrand is null || variable is null || lower is null || upper is null
                    ? null
                    : $"integral({integrand},{variable},{lower},{upper})";
            }
            default:
                return null;
        }
    }

    private static string? Commutative(string name, IReadOnlyList<MathNode> nodes)
    {
        var keys = nodes.Select(CanonicalKey).ToArray();
        return keys.Any(key => key is null)
            ? null
            : name + "{" + string.Join(",", keys.Select(key => key!).OrderBy(key => key, StringComparer.Ordinal)) + "}";
    }

    private static string? Wrap(string name, string? value) => value is null ? null : $"{name}({value})";
    private static string? Pair(string name, string? first, string? second) => first is null || second is null ? null : $"{name}({first},{second})";
    private static string? Triple(string name, string? first, string? second, string? third) => first is null || second is null || third is null ? null : $"{name}({first},{second},{third})";

    internal static bool TryEvaluateRational(MathNode node, out ExactRational value)
    {
        value = default;
        try
        {
            switch (node)
            {
                case IntegerNode integer:
                    value = ExactRational.FromInteger(integer.Value);
                    return IsBounded(value);
                case RationalNode rational:
                    value = rational.Value;
                    return IsBounded(value);
                case NegateNode negate when TryEvaluateRational(negate.Operand, out var operand):
                    value = new ExactRational(BigInteger.Negate(operand.Numerator), operand.Denominator);
                    return IsBounded(value);
                case AddNode add:
                {
                    var sum = ExactRational.FromInteger(BigInteger.Zero);
                    foreach (var term in add.Terms)
                    {
                        if (!TryEvaluateRational(term, out var termValue))
                        {
                            return false;
                        }
                        sum += termValue;
                        if (!IsBounded(sum))
                        {
                            return false;
                        }
                    }
                    value = sum;
                    return true;
                }
                case MultiplyNode multiply:
                {
                    var product = ExactRational.FromInteger(BigInteger.One);
                    foreach (var factor in multiply.Factors)
                    {
                        if (!TryEvaluateRational(factor, out var factorValue))
                        {
                            return false;
                        }
                        product *= factorValue;
                        if (!IsBounded(product))
                        {
                            return false;
                        }
                    }
                    value = product;
                    return true;
                }
                case DivideNode divide when TryEvaluateRational(divide.Numerator, out var numerator)
                                                && TryEvaluateRational(divide.Denominator, out var denominator)
                                                && !denominator.Numerator.IsZero:
                    value = numerator / denominator;
                    return IsBounded(value);
                case PowerNode power when TryEvaluateRational(power.Base, out var @base)
                                              && power.Exponent is IntegerNode exponent
                                              && exponent.Value >= -MaxPower
                                              && exponent.Value <= MaxPower:
                    return TryPower(@base, (int)exponent.Value, out value);
                case RootNode root when root.Degree is >= 2 and <= 8
                                        && TryEvaluateRational(root.Radicand, out var radicand):
                    return TryExactRoot(radicand, root.Degree, out value);
                default:
                    return false;
            }
        }
        catch (ArithmeticException)
        {
            value = default;
            return false;
        }
    }

    private static bool TryPower(ExactRational value, int exponent, out ExactRational result)
    {
        if (exponent == 0)
        {
            result = ExactRational.FromInteger(BigInteger.One);
            return true;
        }
        if (exponent < 0 && value.Numerator.IsZero)
        {
            result = default;
            return false;
        }
        var magnitude = Math.Abs(exponent);
        var numerator = BigInteger.Pow(value.Numerator, magnitude);
        var denominator = BigInteger.Pow(value.Denominator, magnitude);
        result = exponent > 0 ? new ExactRational(numerator, denominator) : new ExactRational(denominator, numerator);
        return IsBounded(result);
    }

    private static bool TryExactRoot(ExactRational value, int degree, out ExactRational root)
    {
        root = default;
        if (value.Numerator.Sign < 0 && degree % 2 == 0)
        {
            return false;
        }
        var numeratorSign = value.Numerator.Sign < 0 ? -1 : 1;
        var numeratorAbs = BigInteger.Abs(value.Numerator);
        var numeratorRoot = IntegerNthRoot(numeratorAbs, degree);
        var denominatorRoot = IntegerNthRoot(value.Denominator, degree);
        if (BigInteger.Pow(numeratorRoot, degree) != numeratorAbs || BigInteger.Pow(denominatorRoot, degree) != value.Denominator)
        {
            return false;
        }
        if (numeratorSign < 0)
        {
            numeratorRoot = BigInteger.Negate(numeratorRoot);
        }
        root = new ExactRational(numeratorRoot, denominatorRoot);
        return IsBounded(root);
    }

    private static BigInteger IntegerNthRoot(BigInteger value, int degree)
    {
        if (value <= BigInteger.One)
        {
            return value;
        }
        BigInteger low = BigInteger.Zero;
        BigInteger high = BigInteger.One;
        while (BigInteger.Pow(high, degree) < value)
        {
            high <<= 1;
        }
        while (high - low > BigInteger.One)
        {
            var middle = (low + high) >> 1;
            if (BigInteger.Pow(middle, degree) <= value)
            {
                low = middle;
            }
            else
            {
                high = middle;
            }
        }
        return BigInteger.Pow(high, degree) == value ? high : low;
    }

    private static bool IsBounded(ExactRational value) =>
        Bits(value.Numerator) <= MaxBits && Bits(value.Denominator) <= MaxBits;

    private static long Bits(BigInteger value) => value.IsZero ? 0 : BigInteger.Abs(value).GetBitLength();

    private static bool TryToDecimal(ExactRational value, out decimal result)
    {
        try
        {
            result = (decimal)value.Numerator / (decimal)value.Denominator;
            return true;
        }
        catch (OverflowException)
        {
            result = default;
            return false;
        }
    }
}
