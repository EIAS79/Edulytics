namespace Edulytics.Core.Mathematics.Curriculum;

public enum LessonContentSemanticStatus
{
    Unclassified = 0,
    PassTargeted = 1,
    PassWithWarnings = 2,
    ReviewRequired = 3,
    ContentWeak = 4,
    MappingConflict = 5,
    Blocked = 6
}

public enum LessonContentSemanticFindingType
{
    WorkedExampleTargetEvidencePresent = 1,
    WorkedExampleTargetEvidenceMissing = 2,
    ExplanationTargetEvidenceMissing = 3,
    KeyConceptTargetEvidenceMissing = 4,
    ReusedWorkedExampleAcrossTargets = 5,
    ReusedSolutionTemplateAcrossTargets = 6,
    ConflictingDomainEvidence = 7,
    UnclassifiedTarget = 8
}

/// <summary>
/// A deterministic semantic-content audit finding. These records are evidence for
/// content review and must not silently rewrite curriculum or learner-facing text.
/// </summary>
public sealed record LessonContentSemanticFinding
{
    public LessonContentSemanticFinding(
        LessonContentSemanticFindingType type,
        string message,
        string? ruleId = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(message);
        Type = type;
        Message = message.Trim();
        RuleId = string.IsNullOrWhiteSpace(ruleId) ? null : ruleId.Trim();
    }

    public LessonContentSemanticFindingType Type { get; }
    public string Message { get; }
    public string? RuleId { get; }
}

public sealed record LessonContentSemanticAuditResult
{
    public LessonContentSemanticAuditResult(
        string lessonCode,
        LessonContentSemanticStatus status,
        IReadOnlyList<LessonContentSemanticFinding>? findings = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(lessonCode);
        LessonCode = lessonCode.Trim();
        Status = status;
        Findings = (findings ?? []).ToArray();
    }

    public string LessonCode { get; }
    public LessonContentSemanticStatus Status { get; }
    public IReadOnlyList<LessonContentSemanticFinding> Findings { get; }
}
