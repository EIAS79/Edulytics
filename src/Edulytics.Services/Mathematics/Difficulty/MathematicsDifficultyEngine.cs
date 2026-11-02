using Edulytics.Core.Mathematics.Difficulty;
using Edulytics.Core.Mathematics.Planning;

namespace Edulytics.Services.Mathematics.Difficulty;

/// <summary>
/// Projects the internal multidimensional complexity vector to the existing
/// Easy/Medium/Challenging UI bands, while retaining the full vector for adaptive
/// selection. Scoring is deterministic and capped to prevent one pathological
/// dimension from dominating the result.
/// </summary>
public sealed class MathematicsDifficultyEngine
{
    public MathematicsDifficultyAssessment Assess(MathematicsStrategyPlan plan)
    {
        ArgumentNullException.ThrowIfNull(plan);
        var c = plan.Complexity;
        var score =
            Cap(c.ExpressionNodeCount / 4, 12) +
            Cap(c.ExpressionDepth, 10) +
            Cap(c.RequiredTransformationCount * 3, 15) +
            Cap(c.RequiredStrategyCount * 2, 8) +
            Cap(c.BranchCount * 2, 10) +
            Cap(c.UnknownCount * 4, 12) +
            Cap(c.DomainConstraintCount * 2, 8) +
            Cap(c.RepresentationSwitchCount * 4, 12) +
            Cap(c.AlgebraicManipulationDepth * 2, 14) +
            Cap(c.ProofBurden * 5, 15) +
            Cap(c.ModellingBurden * 5, 15) +
            Cap(c.InterpretationBurden * 3, 12) +
            Cap(c.UnfamiliarityLevel * 4, 12) +
            Cap(c.MultiPartDependencyDepth * 4, 16);

        var band = score switch
        {
            <= 24 => MathematicsDifficultyBand.Easy,
            <= 55 => MathematicsDifficultyBand.Medium,
            _ => MathematicsDifficultyBand.Challenging
        };

        var reasons = new List<string>();
        if (c.RequiredTransformationCount >= 4) reasons.Add("multi-step transformation burden");
        if (c.UnknownCount >= 2) reasons.Add("multiple unknowns");
        if (c.BranchCount >= 2) reasons.Add("branching reasoning");
        if (c.RepresentationSwitchCount > 0) reasons.Add("representation switching");
        if (c.ProofBurden > 0) reasons.Add("verification/reasoning burden");
        if (c.ModellingBurden > 0) reasons.Add("mathematical modelling burden");
        if (c.DomainConstraintCount > 0) reasons.Add("domain/assumption constraints");
        if (reasons.Count == 0) reasons.Add("routine exact procedure");

        return new MathematicsDifficultyAssessment(band, score, c, reasons);
    }

    public MathematicsAdaptiveRecommendation Recommend(MathematicsAdaptiveLearnerState learner)
    {
        ArgumentNullException.ThrowIfNull(learner);
        ValidateRatio(learner.SkillMastery, nameof(learner.SkillMastery));
        ValidateRatio(learner.PrerequisiteMastery, nameof(learner.PrerequisiteMastery));
        ValidateRatio(learner.RepresentationFluency, nameof(learner.RepresentationFluency));
        if (learner.RecentMisconceptionCount < 0 || learner.RecentSuccessfulItems < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(learner));
        }

        var reasons = new List<string>();
        var readiness = (learner.SkillMastery * 5m + learner.PrerequisiteMastery * 3m + learner.RepresentationFluency * 2m) / 10m;
        var misconceptionPenalty = Math.Min(0.30m, learner.RecentMisconceptionCount * 0.05m);
        readiness = Math.Max(0m, readiness - misconceptionPenalty);

        MathematicsDifficultyBand band;
        int target;
        if (readiness < 0.45m)
        {
            band = MathematicsDifficultyBand.Easy;
            target = 20;
            reasons.Add("build prerequisite and skill stability before increasing complexity");
        }
        else if (readiness < 0.78m)
        {
            band = MathematicsDifficultyBand.Medium;
            target = 42;
            reasons.Add("consolidate the target skill with moderate transformation and representation load");
        }
        else
        {
            band = MathematicsDifficultyBand.Challenging;
            target = 68;
            reasons.Add("mastery profile supports higher reasoning and transfer demand");
        }

        if (learner.RecentMisconceptionCount > 0)
        {
            reasons.Add("recent misconception history lowers the target complexity until the error pattern clears");
        }
        if (learner.RepresentationFluency < 0.5m)
        {
            reasons.Add("representation fluency should be strengthened explicitly");
        }

        return new MathematicsAdaptiveRecommendation(band, target, reasons);
    }

    private static int Cap(int value, int max) => Math.Min(Math.Max(value, 0), max);

    private static void ValidateRatio(decimal value, string name)
    {
        if (value is < 0m or > 1m)
        {
            throw new ArgumentOutOfRangeException(name, "Adaptive mastery values must be in [0,1].");
        }
    }
}
