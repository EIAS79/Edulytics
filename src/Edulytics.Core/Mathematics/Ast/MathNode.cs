using System.Numerics;
using System.Text.Json.Serialization;
using Edulytics.Core.Mathematics.Domains;

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
[JsonDerivedType(typeof(InequalityNode), "inequality")]
[JsonDerivedType(typeof(FunctionCallNode), "function")]
[JsonDerivedType(typeof(VectorNode), "vector")]
[JsonDerivedType(typeof(MatrixNode), "matrix")]
[JsonDerivedType(typeof(DerivativeNode), "derivative")]
[JsonDerivedType(typeof(IntegralNode), "integral")]
public abstract record MathNode;

public sealed record IntegerNode(BigInteger Value) : MathNode;

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

public sealed record NegateNode(MathNode Operand) : MathNode;

public sealed record AddNode(IReadOnlyList<MathNode> Terms) : MathNode;

public sealed record MultiplyNode(IReadOnlyList<MathNode> Factors) : MathNode;

public sealed record DivideNode(MathNode Numerator, MathNode Denominator) : MathNode;

public sealed record PowerNode(MathNode Base, MathNode Exponent) : MathNode;

public sealed record RootNode(MathNode Radicand, int Degree = 2) : MathNode;

public sealed record EquationNode(MathNode Left, MathNode Right) : MathNode;

public enum InequalityRelation
{
    LessThan = 1,
    LessThanOrEqual = 2,
    GreaterThan = 3,
    GreaterThanOrEqual = 4,
    NotEqual = 5
}

public sealed record InequalityNode(
    MathNode Left,
    InequalityRelation Relation,
    MathNode Right) : MathNode;

public sealed record FunctionCallNode : MathNode
{
    public FunctionCallNode(string functionName, IReadOnlyList<MathNode> arguments)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(functionName);
        ArgumentNullException.ThrowIfNull(arguments);
        FunctionName = functionName.Trim().ToLowerInvariant();
        Arguments = arguments.ToArray();
    }

    public string FunctionName { get; }
    public IReadOnlyList<MathNode> Arguments { get; }
}

public sealed record VectorNode(IReadOnlyList<MathNode> Components) : MathNode;

public sealed record MatrixNode(IReadOnlyList<IReadOnlyList<MathNode>> Rows) : MathNode;

public sealed record DerivativeNode(
    MathNode Expression,
    SymbolNode Variable,
    int Order = 1) : MathNode;

public sealed record IntegralNode(
    MathNode Integrand,
    SymbolNode Variable,
    MathNode? LowerBound = null,
    MathNode? UpperBound = null) : MathNode;
