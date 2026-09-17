using Edulytics.Core.Mathematics.Ast;

namespace Edulytics.Core.Mathematics.Domains;

public abstract record SolutionSet;

public sealed record EmptySolutionSet : SolutionSet;

public sealed record AllRealNumbersSolutionSet : SolutionSet;

public sealed record FiniteSolutionSet : SolutionSet
{
    public FiniteSolutionSet(IReadOnlyList<MathNode> values)
    {
        ArgumentNullException.ThrowIfNull(values);
        if (values.Any(value => value is null))
        {
            throw new ArgumentException("Finite solution values cannot contain null nodes.", nameof(values));
        }

        Values = values.ToArray();
    }

    public IReadOnlyList<MathNode> Values { get; }
}

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

public sealed record UnionSolutionSet : SolutionSet
{
    public UnionSolutionSet(IReadOnlyList<SolutionSet> sets)
    {
        ArgumentNullException.ThrowIfNull(sets);
        if (sets.Any(set => set is null))
        {
            throw new ArgumentException("Union solution sets cannot contain null sets.", nameof(sets));
        }

        Sets = sets.ToArray();
    }

    public IReadOnlyList<SolutionSet> Sets { get; }
}
