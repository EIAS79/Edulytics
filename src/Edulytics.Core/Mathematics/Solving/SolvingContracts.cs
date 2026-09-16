using Edulytics.Core.Mathematics.Ast;
using Edulytics.Core.Mathematics.Domains;
using Edulytics.Core.Mathematics.Identifiers;

namespace Edulytics.Core.Mathematics.Solving;

public enum MathematicsSolveStatus
{
    Solved = 1,
    Unsupported = 2,
    InvalidInput = 3,
    NoSolution = 4,
    Indeterminate = 5,
    ResourceLimit = 6,
    FailedVerification = 7
}

public sealed record MathematicsAssumption(
    string Subject,
    string Predicate);

public sealed record MathematicsSolveRequest(
    MathNode Problem,
    IReadOnlyList<CapabilityId> RequiredCapabilities,
    IReadOnlyList<MathematicsAssumption> Assumptions);

public sealed record MathematicsSolutionStep(
    string StepId,
    MathNode Before,
    string RuleId,
    string Operation,
    MathNode After,
    string Justification,
    IReadOnlyList<MathematicsAssumption> AssumptionsUsed,
    bool IsReversible);

public sealed record MathematicsSolutionTrace(
    IReadOnlyList<MathematicsSolutionStep> Steps);

public sealed record MathematicsSolveResult(
    MathematicsSolveStatus Status,
    MathNode? ExactResult,
    SolutionSet? SolutionSet,
    IReadOnlyList<MathematicsAssumption> Assumptions,
    string? SelectedStrategyId,
    MathematicsSolutionTrace? Trace,
    string SolverProvider,
    string SolverVersion,
    IReadOnlyList<string> Diagnostics);
