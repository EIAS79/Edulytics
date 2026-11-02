using Edulytics.Core.Mathematics.Rollout;

namespace Edulytics.Services.Mathematics.Rollout;

public sealed record AdvancedMathematicsRouteDecision(
    bool Allowed,
    string ReasonCode);

public static class AdvancedMathematicsAssessmentEligibility
{
    public static AdvancedMathematicsRouteDecision Evaluate(
        string familyId,
        AdvancedMathematicsV3Policy policy,
        string curriculumLevelKey,
        Guid schoolId)
    {
        var allowed =
            AdvancedMathematicsCertificationRegistry.CanRoute(
                familyId,
                AdvancedMathematicsSurface.Assessment,
                policy,
                curriculumLevelKey,
                schoolId);

        return new AdvancedMathematicsRouteDecision(
            allowed,
            allowed
                ? "ADVANCED_ASSESSMENT_READY"
                : "ADVANCED_ASSESSMENT_BLOCKED");
    }
}

public sealed record AdvancedMathematicsMarkSchemeContract(
    string FamilyId,
    string ExpectedAnswerType,
    int MethodMarks,
    int AccuracyMarks,
    bool RequiresVisualEvidence);

public static class AdvancedMathematicsExamEligibility
{
    public static AdvancedMathematicsRouteDecision Evaluate(
        AdvancedMathematicsMarkSchemeContract markScheme,
        AdvancedMathematicsV3Policy policy,
        string curriculumLevelKey,
        Guid schoolId)
    {
        ArgumentNullException.ThrowIfNull(markScheme);

        if (string.IsNullOrWhiteSpace(markScheme.FamilyId) ||
            string.IsNullOrWhiteSpace(markScheme.ExpectedAnswerType) ||
            markScheme.MethodMarks < 0 ||
            markScheme.AccuracyMarks < 1 ||
            markScheme.MethodMarks + markScheme.AccuracyMarks > 30)
        {
            return new AdvancedMathematicsRouteDecision(
                false,
                "INVALID_MARK_SCHEME");
        }

        var allowed =
            AdvancedMathematicsCertificationRegistry.CanRoute(
                markScheme.FamilyId,
                AdvancedMathematicsSurface.Exam,
                policy,
                curriculumLevelKey,
                schoolId);

        return new AdvancedMathematicsRouteDecision(
            allowed,
            allowed
                ? "ADVANCED_EXAM_READY"
                : "ADVANCED_EXAM_BLOCKED_ACADEMIC_OR_CERTIFICATION");
    }
}

public static class AdvancedMathematicsAdaptiveCompatibility
{
    public static AdvancedMathematicsRouteDecision Evaluate(
        string familyId,
        AdvancedMathematicsV3Policy policy,
        string curriculumLevelKey,
        Guid schoolId,
        int numericLearnerLevel)
    {
        if (numericLearnerLevel <= 6)
        {
            return new AdvancedMathematicsRouteDecision(
                false,
                "PRIMARY_BOUNDARY_HARD_BLOCK");
        }

        var allowed =
            AdvancedMathematicsCertificationRegistry.CanRoute(
                familyId,
                AdvancedMathematicsSurface.Adaptive,
                policy,
                curriculumLevelKey,
                schoolId,
                numericLearnerLevel);

        return new AdvancedMathematicsRouteDecision(
            allowed,
            allowed
                ? "ADVANCED_ADAPTIVE_READY"
                : "ADVANCED_ADAPTIVE_NOT_APPROVED");
    }
}
