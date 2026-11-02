using Edulytics.Core.Mathematics.Ast;
using Edulytics.Core.Mathematics.Difficulty;
using Edulytics.Core.Mathematics.Solving;

namespace Edulytics.Core.Mathematics.Planning;

public enum MathematicsPlanningStatus
{
    Planned = 1,
    Unsupported = 2,
    ResourceLimit = 3
}

public sealed record MathematicsStrategyCandidate(
    string StrategyId,
    string Rationale,
    int EstimatedStepCount,
    int Priority,
    IReadOnlyList<string> Preconditions);

public sealed record MathematicsPlanningRequest(
    MathNode Problem,
    IReadOnlyList<MathematicsAssumption> Assumptions,
    int MaxCandidateStrategies = 4,
    int MaxAstNodes = 512,
    int MaxAstDepth = 48);

public sealed record MathematicsStrategyPlan(
    MathematicsPlanningStatus Status,
    string? PrimaryStrategyId,
    IReadOnlyList<MathematicsStrategyCandidate> Candidates,
    MathematicsComplexityVector Complexity,
    IReadOnlyList<string> Diagnostics);

public interface IMathematicsStrategyPlanner
{
    MathematicsStrategyPlan Plan(MathematicsPlanningRequest request);
}
