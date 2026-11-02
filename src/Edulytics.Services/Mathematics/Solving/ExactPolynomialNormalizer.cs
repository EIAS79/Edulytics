using System.Numerics;
using Edulytics.Core.Mathematics.Ast;
using Edulytics.Core.Mathematics.Domains;

namespace Edulytics.Services.Mathematics.Solving;

/// <summary>
/// Deterministic exact-rational polynomial normalizer used by the Mathematics V2
/// algebra slices. It accepts one symbolic variable, exact rational constants,
/// +, -, multiplication, division by non-zero scalar constants, and non-negative
/// integer powers up to a bounded degree. Unsupported shapes fail closed.
/// </summary>
public sealed class ExactPolynomialNormalizer
{
    public ExactPolynomialNormalizationResult Normalize(
        MathNode expression,
        string variable,
        int maxDegree = 8)
    {
        ArgumentNullException.ThrowIfNull(expression);
        ArgumentException.ThrowIfNullOrWhiteSpace(variable);
        if (maxDegree is < 0 or > 32)
        {
            throw new ArgumentOutOfRangeException(nameof(maxDegree));
        }

        if (!ExactPolynomial.TryCreate(expression, variable.Trim(), maxDegree, out var polynomial, out var error))
        {
            return new ExactPolynomialNormalizationResult(
                false,
                variable.Trim(),
                -1,
                new Dictionary<int, ExactRational>(),
                null,
                [error ?? "Expression is outside the exact polynomial subset."]);
        }

        return new ExactPolynomialNormalizationResult(
            true,
            variable.Trim(),
            polynomial.Degree,
            polynomial.Coefficients,
            polynomial.ToCanonicalNode(variable.Trim()),
            []);
    }
}

public sealed record ExactPolynomialNormalizationResult(
    bool IsSupported,
    string Variable,
    int Degree,
    IReadOnlyDictionary<int, ExactRational> Coefficients,
    MathNode? CanonicalExpression,
    IReadOnlyList<string> Diagnostics);

internal sealed class ExactPolynomial
{
    private static readonly ExactRational Zero = new(0, 1);
    private static readonly ExactRational One = new(1, 1);

    private ExactPolynomial(IReadOnlyDictionary<int, ExactRational> coefficients)
    {
        Coefficients = coefficients
            .Where(pair => pair.Value != Zero)
            .OrderBy(pair => pair.Key)
            .ToDictionary(pair => pair.Key, pair => pair.Value);
    }

    public IReadOnlyDictionary<int, ExactRational> Coefficients { get; }
    public int Degree => Coefficients.Count == 0 ? 0 : Coefficients.Keys.Max();
    public ExactRational this[int degree] => Coefficients.TryGetValue(degree, out var value) ? value : Zero;

    public static ExactPolynomial Constant(ExactRational value) =>
        value == Zero
            ? new ExactPolynomial(new Dictionary<int, ExactRational>())
            : new ExactPolynomial(new Dictionary<int, ExactRational> { [0] = value });

    public static ExactPolynomial Variable() =>
        new(new Dictionary<int, ExactRational> { [1] = One });

    public static ExactPolynomial operator +(ExactPolynomial left, ExactPolynomial right)
    {
        var result = new Dictionary<int, ExactRational>();
        foreach (var degree in left.Coefficients.Keys.Concat(right.Coefficients.Keys).Distinct())
        {
            var value = left[degree] + right[degree];
            if (value != Zero)
            {
                result[degree] = value;
            }
        }
        return new ExactPolynomial(result);
    }

    public static ExactPolynomial operator -(ExactPolynomial left, ExactPolynomial right) =>
        left + right.Scale(new ExactRational(-1, 1));

    public ExactPolynomial Scale(ExactRational factor)
    {
        if (factor == Zero)
        {
            return Constant(Zero);
        }

        return new ExactPolynomial(Coefficients.ToDictionary(
            pair => pair.Key,
            pair => pair.Value * factor));
    }

    public ExactPolynomial Multiply(ExactPolynomial other, int maxDegree)
    {
        var result = new Dictionary<int, ExactRational>();
        foreach (var left in Coefficients)
        {
            foreach (var right in other.Coefficients)
            {
                var degree = left.Key + right.Key;
                if (degree > maxDegree)
                {
                    throw new PolynomialDegreeLimitException(maxDegree);
                }

                var existing = result.TryGetValue(degree, out var current) ? current : Zero;
                result[degree] = existing + left.Value * right.Value;
            }
        }
        return new ExactPolynomial(result);
    }

    public ExactPolynomial Pow(int exponent, int maxDegree)
    {
        if (exponent < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(exponent));
        }

        var result = Constant(One);
        var factor = this;
        var remaining = exponent;
        while (remaining > 0)
        {
            if ((remaining & 1) == 1)
            {
                result = result.Multiply(factor, maxDegree);
            }
            remaining >>= 1;
            if (remaining > 0)
            {
                factor = factor.Multiply(factor, maxDegree);
            }
        }
        return result;
    }

    public MathNode ToCanonicalNode(string variable)
    {
        if (Coefficients.Count == 0)
        {
            return new IntegerNode(BigInteger.Zero);
        }

        var terms = new List<MathNode>();
        foreach (var pair in Coefficients.OrderByDescending(pair => pair.Key))
        {
            var coefficient = pair.Value;
            if (pair.Key == 0)
            {
                terms.Add(ToNode(coefficient));
                continue;
            }

            MathNode power = pair.Key == 1
                ? new SymbolNode(variable)
                : new PowerNode(new SymbolNode(variable), new IntegerNode(new BigInteger(pair.Key)));

            if (coefficient == One)
            {
                terms.Add(power);
            }
            else if (coefficient == new ExactRational(-1, 1))
            {
                terms.Add(new NegateNode(power));
            }
            else
            {
                terms.Add(new MultiplyNode([ToNode(coefficient), power]));
            }
        }

        return terms.Count == 1 ? terms[0] : new AddNode(terms);
    }

    public static bool TryCreate(
        MathNode node,
        string variable,
        int maxDegree,
        out ExactPolynomial polynomial,
        out string? error)
    {
        try
        {
            return TryCreateCore(node, variable, maxDegree, out polynomial, out error);
        }
        catch (PolynomialDegreeLimitException)
        {
            polynomial = null!;
            error = $"Polynomial degree exceeds the configured limit of {maxDegree}.";
            return false;
        }
    }

    private static bool TryCreateCore(
        MathNode node,
        string variable,
        int maxDegree,
        out ExactPolynomial polynomial,
        out string? error)
    {
        switch (node)
        {
            case IntegerNode integer:
                polynomial = Constant(new ExactRational(integer.Value, BigInteger.One));
                error = null;
                return true;
            case RationalNode rational:
                polynomial = Constant(rational.Value);
                error = null;
                return true;
            case SymbolNode symbol when string.Equals(symbol.Name, variable, StringComparison.Ordinal):
                polynomial = Variable();
                error = null;
                return true;
            case SymbolNode symbol:
                polynomial = null!;
                error = $"Unexpected symbol {symbol.Name}; polynomial normalizer is bound to {variable}.";
                return false;
            case NegateNode negate:
                if (!TryCreateCore(negate.Operand, variable, maxDegree, out var operand, out error))
                {
                    polynomial = null!;
                    return false;
                }
                polynomial = operand.Scale(new ExactRational(-1, 1));
                error = null;
                return true;
            case AddNode add:
                var sum = Constant(Zero);
                foreach (var term in add.Terms)
                {
                    if (!TryCreateCore(term, variable, maxDegree, out var next, out error))
                    {
                        polynomial = null!;
                        return false;
                    }
                    sum += next;
                }
                polynomial = sum;
                error = null;
                return true;
            case MultiplyNode multiply:
                var product = Constant(One);
                foreach (var factor in multiply.Factors)
                {
                    if (!TryCreateCore(factor, variable, maxDegree, out var next, out error))
                    {
                        polynomial = null!;
                        return false;
                    }
                    product = product.Multiply(next, maxDegree);
                }
                polynomial = product;
                error = null;
                return true;
            case DivideNode divide:
                if (!TryCreateCore(divide.Numerator, variable, maxDegree, out var numerator, out error)
                    || !TryCreateCore(divide.Denominator, variable, maxDegree, out var denominator, out error))
                {
                    polynomial = null!;
                    return false;
                }
                if (denominator.Degree != 0 || denominator[0] == Zero)
                {
                    polynomial = null!;
                    error = "Polynomial division is supported only by a non-zero exact scalar denominator.";
                    return false;
                }
                polynomial = numerator.Scale(One / denominator[0]);
                error = null;
                return true;
            case PowerNode power:
                if (!TryReadNonNegativeInteger(power.Exponent, out var exponent))
                {
                    polynomial = null!;
                    error = "Polynomial powers must be non-negative integer constants.";
                    return false;
                }
                if (!TryCreateCore(power.Base, variable, maxDegree, out var basePolynomial, out error))
                {
                    polynomial = null!;
                    return false;
                }
                polynomial = basePolynomial.Pow(exponent, maxDegree);
                error = null;
                return true;
            default:
                polynomial = null!;
                error = $"Node type {node.GetType().Name} is unsupported by exact polynomial normalization.";
                return false;
        }
    }

    public static IReadOnlyList<string> CollectSymbols(MathNode node)
    {
        var symbols = new HashSet<string>(StringComparer.Ordinal);
        Visit(node, symbols);
        return symbols.OrderBy(x => x, StringComparer.Ordinal).ToArray();
    }

    private static void Visit(MathNode node, ISet<string> symbols)
    {
        switch (node)
        {
            case SymbolNode symbol:
                symbols.Add(symbol.Name);
                break;
            case NegateNode negate:
                Visit(negate.Operand, symbols);
                break;
            case AddNode add:
                foreach (var term in add.Terms) Visit(term, symbols);
                break;
            case MultiplyNode multiply:
                foreach (var factor in multiply.Factors) Visit(factor, symbols);
                break;
            case DivideNode divide:
                Visit(divide.Numerator, symbols);
                Visit(divide.Denominator, symbols);
                break;
            case PowerNode power:
                Visit(power.Base, symbols);
                Visit(power.Exponent, symbols);
                break;
            case RootNode root:
                Visit(root.Radicand, symbols);
                break;
        }
    }

    private static bool TryReadNonNegativeInteger(MathNode node, out int value)
    {
        if (node is IntegerNode integer
            && integer.Value >= BigInteger.Zero
            && integer.Value <= new BigInteger(32))
        {
            value = (int)integer.Value;
            return true;
        }
        value = 0;
        return false;
    }

    internal static MathNode ToNode(ExactRational value) =>
        value.Denominator == BigInteger.One
            ? new IntegerNode(value.Numerator)
            : new RationalNode(value);

    private sealed class PolynomialDegreeLimitException(int maxDegree) : Exception(
        $"Polynomial degree exceeds {maxDegree}.");
}
