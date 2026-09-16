using Edulytics.Core.Mathematics.Identifiers;

namespace Edulytics.Core.Mathematics.Curriculum;

public enum LessonSkillEvidenceType
{
    LessonTitle = 1,
    TopicTitle = 2,
    UnitTitle = 3,
    Explanation = 4,
    KeyConceptsAndRules = 5,
    WorkedExamples = 6,
    StepByStepSolutions = 7,
    CommonMistakes = 8,
    QuickSummary = 9,
    OfficialOutcome = 10,
    SourceReference = 11,
    ExistingPracticeMechanic = 12
}

public enum LessonSkillResolutionStatus
{
    Unresolved = 0,
    ExistingVerifiedMapping = 1,
    HighConfidenceCandidate = 2,
    ReviewRequired = 3,
    Ambiguous = 4,
    Conflict = 5,
    OntologyGap = 6,
    Blocked = 7
}

/// <summary>
/// One auditable signal used to support or reject a candidate SkillId. The raw
/// excerpt is intentionally short and is metadata evidence, not a replacement for
/// the canonical lesson content.
/// </summary>
public sealed record LessonSkillEvidence
{
    public LessonSkillEvidence(
        LessonSkillEvidenceType type,
        string signal,
        int weight,
        string? ruleId = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(signal);
        if (weight is < -100 or > 100)
        {
            throw new ArgumentOutOfRangeException(nameof(weight));
        }

        Type = type;
        Signal = signal.Trim();
        Weight = weight;
        RuleId = string.IsNullOrWhiteSpace(ruleId) ? null : ruleId.Trim();
    }

    public LessonSkillEvidenceType Type { get; }
    public string Signal { get; }
    public int Weight { get; }
    public string? RuleId { get; }
}

/// <summary>
/// A proposed exact skill for a lesson. Candidates are not production mappings.
/// Promotion requires deterministic validation and, where necessary, academic
/// review.
/// </summary>
public sealed record LessonSkillCandidate
{
    public LessonSkillCandidate(
        SkillId skillId,
        int score,
        IReadOnlyList<LessonSkillEvidence> evidence,
        IReadOnlyList<string>? conflicts = null)
    {
        ArgumentNullException.ThrowIfNull(evidence);
        SkillId = skillId;
        Score = score;
        Evidence = evidence.ToArray();
        Conflicts = CleanStrings(conflicts);
    }

    public SkillId SkillId { get; }
    public int Score { get; }
    public IReadOnlyList<LessonSkillEvidence> Evidence { get; }
    public IReadOnlyList<string> Conflicts { get; }

    private static IReadOnlyList<string> CleanStrings(IReadOnlyList<string>? values) =>
        values is null
            ? []
            : values
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => x.Trim())
                .Distinct(StringComparer.Ordinal)
                .ToArray();
}

/// <summary>
/// Non-authoritative resolution output produced before a LessonSkillProfile is
/// promoted. It lets CI and reviewers distinguish a confident candidate from an
/// exact mapping, an ontology gap, or an ambiguous lesson.
/// </summary>
public sealed record LessonSkillResolution
{
    public LessonSkillResolution(
        string lessonCode,
        MathematicsLessonSourceType sourceType,
        LessonSkillResolutionStatus status,
        IReadOnlyList<LessonSkillCandidate>? candidates = null,
        IReadOnlyList<string>? diagnostics = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(lessonCode);
        LessonCode = lessonCode.Trim();
        SourceType = sourceType;
        Status = status;
        Candidates = (candidates ?? [])
            .OrderByDescending(x => x.Score)
            .ThenBy(x => x.SkillId.Value, StringComparer.Ordinal)
            .ToArray();
        Diagnostics = CleanStrings(diagnostics);

        if (status == LessonSkillResolutionStatus.HighConfidenceCandidate && Candidates.Count == 0)
        {
            throw new ArgumentException(
                "A high-confidence resolution requires at least one candidate.",
                nameof(candidates));
        }
    }

    public string LessonCode { get; }
    public MathematicsLessonSourceType SourceType { get; }
    public LessonSkillResolutionStatus Status { get; }
    public IReadOnlyList<LessonSkillCandidate> Candidates { get; }
    public IReadOnlyList<string> Diagnostics { get; }

    private static IReadOnlyList<string> CleanStrings(IReadOnlyList<string>? values) =>
        values is null
            ? []
            : values
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => x.Trim())
                .Distinct(StringComparer.Ordinal)
                .ToArray();
}
