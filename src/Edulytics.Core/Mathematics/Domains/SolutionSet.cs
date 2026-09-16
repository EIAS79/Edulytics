using Edulytics.Core.Mathematics.Ast;

namespace Edulytics.Core.Mathematics.Domains;

public abstract record SolutionSet;

public sealed record EmptySolutionSet : SolutionSet;

public sealed record AllRealNumbersSolutionSet : SolutionSet;

public sealed record FiniteSolutionSet(IReadOnlyList<MathNode> Values) : SolutionSet;

public enum IntervalBoundary
{
    Open = 1,
    Closed = 2
}

public sealed record IntervalSolutionSet(
    MathNode? Lower,
    MathNode? Upper,
    IntervalBoundary LowerBoundary,
    IntervalBoundary UpperBoundary) : SolutionSet;

public sealed record UnionSolutionSet(IReadOnlyList<SolutionSet> Sets) : SolutionSet;
