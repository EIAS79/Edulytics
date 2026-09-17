using System.Numerics;
using System.Text.Json.Serialization;
using Edulytics.Core.Mathematics.Domains;
using Edulytics.Core.Mathematics.Serialization;

namespace Edulytics.Core.Mathematics.Ast;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "kind")]
[JsonDerivedType(typeof(IntegerNode), "integer")]
[JsonDerivedType(typeof(RationalNode), "rational")]
[JsonDerivedType(typeof(SymbolNode), "symbol")]
[JsonDerivedType(typeof(NegateNode), "negate")]
[JsonDerivedType(typeof(AddNode), "add")]
[JsonDerivedType(typeof(MultiplyNode), "multiply")]
[JsonDerivedType(typeof(DivideNode), "divide")]
[JsonDerivedType(typeof(PowerNode), "power")]
[JsonDerivedType(typeof(RootNode), "root")]
[JsonDerivedType(typeof(EquationNode), "equation")]
[JsonDerivedType(typeof(EquationSystemNode), "equationSystem")]
[JsonDerivedType(typeof(InequalityNode), "inequality")]
[JsonDerivedType(typeof(FunctionCallNode), "function")]
[JsonDerivedType(typeof(VectorNode), "vector")]
[JsonDerivedType(typeof(MatrixNode), "matrix")]
[JsonDerivedType(typeof(DerivativeNode), "derivative")]
[JsonDerivedType(typeof(IntegralNode), "integral")]
public abstract record MathNode;

public sealed record IntegerNode : MathNode
{
    public IntegerNode(BigInteger value)
    {
        Value = value;
    }

    [JsonConverter(typeof(BigIntegerJsonConverter))]
    public BigInteger Value { get; }
}

public sealed record RationalNode(ExactRational Value) : MathNode;

public sealed record SymbolNode : MathNode
{
    public SymbolNode(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        Name = name.Trim();
    }

    public string Name { get; }
}

public sealed record NegateNode : MathNode
{
    public NegateNode(MathNode operand)
    {
        ArgumentNullException.ThrowIfNull(operand);
        Operand = operand;
    }

    public MathNode Operand { get; }
}

public sealed record AddNode : MathNode
{
    public AddNode(IReadOnlyList<MathNode> terms)
    {
        ArgumentNullException.ThrowIfNull(terms);
        if (terms.Any(term => term is null))
        {
            throw new ArgumentException("Add terms cannot contain null nodes.", nameof(terms));
        }

        Terms = terms.ToArray();
    }

    public IReadOnlyList<MathNode> Terms { get; }
}

public sealed record MultiplyNode : MathNode
{
    public MultiplyNode(IReadOnlyList<MathNode> factors)
    {
        ArgumentNullException.ThrowIfNull(factors);
        if (factors.Any(factor => factor is null))
        {
            throw new ArgumentException("Multiply factors cannot contain null nodes.", nameof(factors));
        }

        Factors = factors.ToArray();
    }

    public IReadOnlyList<MathNode> Factors { get; }
}

public sealed record DivideNode : MathNode
{
    public DivideNode(MathNode numerator, MathNode denominator)
    {
        ArgumentNullException.ThrowIfNull(numerator);
        ArgumentNullException.ThrowIfNull(denominator);
        Numerator = numerator;
        Denominator = denominator;
    }

    public MathNode Numerator { get; }
    public MathNode Denominator { get; }
}

public sealed record PowerNode : MathNode
{
    public PowerNode(MathNode @base, MathNode exponent)
    {
        ArgumentNullException.ThrowIfNull(@base);
        ArgumentNullException.ThrowIfNull(exponent);
        Base = @base;
        Exponent = exponent;
    }

    public MathNode Base { get; }
    public MathNode Exponent { get; }
}

public sealed record RootNode : MathNode
{
    public RootNode(MathNode radicand, int degree = 2)
    {
        ArgumentNullException.ThrowIfNull(radicand);
        Radicand = radicand;
        Degree = degree;
    }

    public MathNode Radicand { get; }
    public int Degree { get; }
}

public sealed record EquationNode : MathNode
{
    public EquationNode(MathNode left, MathNode right)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);
        Left = left;
        Right = right;
    }

    public MathNode Left { get; }
    public MathNode Right { get; }
}

/// <summary>
/// A typed conjunction of equations that must be satisfied by the same variable
/// assignment. The current V2 systems slice supports exactly two affine equations
/// in exactly two distinct variables, while the AST itself can carry larger systems
/// for future solver providers.
/// </summary>
public sealed record EquationSystemNode : MathNode
{
    public EquationSystemNode(IReadOnlyList<EquationNode> equations)
    {
        ArgumentNullException.ThrowIfNull(equations);
        if (equations.Count == 0)
        {
            throw new ArgumentException("An equation system must contain at least one equation.", nameof(equations));
        }
        if (equations.Any(equation => equation is null))
        {
            throw new ArgumentException("An equation system cannot contain null equations.", nameof(equations));
        }

        Equations = equations.ToArray();
    }

    public IReadOnlyList<EquationNode> Equations { get; }
}

public enum InequalityRelation
{
    LessThan = 1,
    LessThanOrEqual = 2,
    GreaterThan = 3,
    GreaterThanOrEqual = 4,
    NotEqual = 5
}

public sealed record InequalityNode : MathNode
{
    public InequalityNode(MathNode left, InequalityRelation relation, MathNode right)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);
        Left = left;
        Relation = relation;
        Right = right;
    }

    public MathNode Left { get; }
    public InequalityRelation Relation { get; }
    public MathNode Right { get; }
}

public sealed record FunctionCallNode : MathNode
{
    public FunctionCallNode(string functionName, IReadOnlyList<MathNode> arguments)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(functionName);
        ArgumentNullException.ThrowIfNull(arguments);
        if (arguments.Any(argument => argument is null))
        {
            throw new ArgumentException("Function arguments cannot contain null nodes.", nameof(arguments));
        }

        FunctionName = functionName.Trim().ToLowerInvariant();
        Arguments = arguments.ToArray();
    }

    public string FunctionName { get; }
    public IReadOnlyList<MathNode> Arguments { get; }
}

public sealed record VectorNode : MathNode
{
    public VectorNode(IReadOnlyList<MathNode> components)
    {
        ArgumentNullException.ThrowIfNull(components);
        if (components.Any(component => component is null))
        {
            throw new ArgumentException("Vector components cannot contain null nodes.", nameof(components));
        }

        Components = components.ToArray();
    }

    public IReadOnlyList<MathNode> Components { get; }
}

public sealed record MatrixNode : MathNode
{
    public MatrixNode(IReadOnlyList<IReadOnlyList<MathNode>> rows)
    {
        ArgumentNullException.ThrowIfNull(rows);
        if (rows.Any(row => row is null))
        {
            throw new ArgumentException("Matrix rows cannot be null.", nameof(rows));
        }
        if (rows.Any(row => row.Any(cell => cell is null)))
        {
            throw new ArgumentException("Matrix rows cannot contain null nodes.", nameof(rows));
        }

        Rows = rows.Select(row => (IReadOnlyList<MathNode>)row.ToArray()).ToArray();
    }

    public IReadOnlyList<IReadOnlyList<MathNode>> Rows { get; }
}

public sealed record DerivativeNode(
    MathNode Expression,
    SymbolNode Variable,
    int Order = 1) : MathNode;

public sealed record IntegralNode(
    MathNode Integrand,
    SymbolNode Variable,
    MathNode? LowerBound = null,
    MathNode? UpperBound = null) : MathNode;
