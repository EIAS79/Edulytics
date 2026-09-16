namespace Edulytics.Core.Mathematics.Curriculum;

public sealed record LessonGenerationReadinessInput(
    LessonSkillResolutionStatus SkillResolutionStatus,
    LessonContentSemanticStatus SemanticContentStatus,
    bool HasApprovedLessonSkillProfile,
    bool HasQuestionFamily,
    bool HasVerifiedSolverCapability,
    bool HasContextualGenerationCapability);

public sealed record LessonGenerationReadinessDecision(
    MathematicsGenerationReadiness Readiness,
    IReadOnlyList<string> Reasons)
{
    public bool IsGenerationReady =>
        Readiness is MathematicsGenerationReadiness.ReadyVerified
            or MathematicsGenerationReadiness.ReadyContextual;
}

/// <summary>
/// Fail-closed policy that combines curriculum mapping, semantic content quality,
/// question-family availability and mathematical capability into one generation
/// readiness decision. Candidate mappings are never promoted by this policy.
/// </summary>
public static class LessonGenerationReadinessPolicy
{
    public static LessonGenerationReadinessDecision Evaluate(LessonGenerationReadinessInput input)
    {
        ArgumentNullException.ThrowIfNull(input);

        if (input.SemanticContentStatus == LessonContentSemanticStatus.Blocked)
        {
            return Decision(
                MathematicsGenerationReadiness.Blocked,
                "Semantic content audit blocked the lesson.");
        }

        if (input.SkillResolutionStatus == LessonSkillResolutionStatus.Conflict
            || input.SemanticContentStatus == LessonContentSemanticStatus.MappingConflict)
        {
            return Decision(
                MathematicsGenerationReadiness.MappingConflict,
                "Curriculum/skill evidence contains a mapping conflict.");
        }

        if (input.SemanticContentStatus == LessonContentSemanticStatus.ContentWeak)
        {
            return Decision(
                MathematicsGenerationReadiness.ContentWeak,
                "Lesson content is structurally present but worked examples do not demonstrate the recognized mathematical target.");
        }

        if (input.SemanticContentStatus is LessonContentSemanticStatus.ReviewRequired
            or LessonContentSemanticStatus.Unclassified)
        {
            return Decision(
                MathematicsGenerationReadiness.RequiresAcademicReview,
                "Semantic content evidence is not strong enough for automatic generation readiness.");
        }

        if (input.SkillResolutionStatus == LessonSkillResolutionStatus.Ambiguous)
        {
            return Decision(
                MathematicsGenerationReadiness.SkillAmbiguous,
                "More than one SkillId remains plausible for the lesson.");
        }

        if (input.SkillResolutionStatus == LessonSkillResolutionStatus.ReviewRequired)
        {
            return Decision(
                MathematicsGenerationReadiness.RequiresAcademicReview,
                "Skill evidence exists but requires review before promotion to an approved lesson-skill profile.");
        }

        if (input.SkillResolutionStatus == LessonSkillResolutionStatus.HighConfidenceCandidate)
        {
            return Decision(
                MathematicsGenerationReadiness.RequiresAcademicReview,
                "A high-confidence SkillId candidate is still a candidate and cannot authorize generation until explicitly promoted.");
        }

        if (input.SkillResolutionStatus is LessonSkillResolutionStatus.Unresolved
            or LessonSkillResolutionStatus.OntologyGap)
        {
            return Decision(
                MathematicsGenerationReadiness.SolverCapabilityMissing,
                "The current Skill/Capability ontology cannot yet represent this lesson precisely enough for V2 generation.");
        }

        if (!input.HasApprovedLessonSkillProfile)
        {
            return Decision(
                MathematicsGenerationReadiness.RequiresAcademicReview,
                "No approved lesson-skill profile exists.");
        }

        if (!input.HasQuestionFamily)
        {
            return Decision(
                MathematicsGenerationReadiness.QuestionFamilyMissing,
                "The approved skill has no eligible question family.");
        }

        if (input.HasVerifiedSolverCapability)
        {
            return Decision(
                MathematicsGenerationReadiness.ReadyVerified,
                "Approved mapping, semantic content and independently verifiable mathematical capability are all available.");
        }

        if (input.HasContextualGenerationCapability)
        {
            return Decision(
                MathematicsGenerationReadiness.ReadyContextual,
                "Approved mapping and semantic content are available, but the lesson remains on contextual generation rather than verified V2 solving.");
        }

        return Decision(
            MathematicsGenerationReadiness.SolverCapabilityMissing,
            "The approved skill is not backed by a sufficient solver/verifier capability.");
    }

    private static LessonGenerationReadinessDecision Decision(
        MathematicsGenerationReadiness readiness,
        params string[] reasons) =>
        new(
            readiness,
            reasons
                .Where(reason => !string.IsNullOrWhiteSpace(reason))
                .Select(reason => reason.Trim())
                .Distinct(StringComparer.Ordinal)
                .ToArray());
}
