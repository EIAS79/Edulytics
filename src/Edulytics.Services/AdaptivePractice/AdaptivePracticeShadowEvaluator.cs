using Edulytics.Core.AdaptivePractice;

namespace Edulytics.Services.AdaptivePractice;

/// <summary>
/// Read-only shadow evaluator. It computes the V2 proposal but owns no
/// persistence and cannot alter Practice V1.
/// </summary>
public sealed class AdaptivePracticeShadowEvaluator(
    AdaptiveNextItemDecisionEngine decisionEngine)
{
    public AdaptiveNextItemDecision Evaluate(
        AdaptivePracticeLearningState state) =>
        decisionEngine.Decide(state);
}
