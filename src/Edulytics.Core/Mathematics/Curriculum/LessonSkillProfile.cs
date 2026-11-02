using Edulytics.Core.Mathematics.Identifiers;

namespace Edulytics.Core.Mathematics.Curriculum;

public enum MathematicsLessonSourceType
{
    OfficialOutcomeLesson = 1,
    OfficialMappedPedagogicalLesson = 2,
    SupportingLesson = 3,
    EdulyticsAuthoredLesson = 4,
    ImportedNonOfficialLesson = 5,
    LegacyLesson = 6,
    Unknown = 7
}

public enum MathematicsMappingConfidence
{
    Low = 1,
    Medium = 2,
    High = 3,
    Conflict = 4
}

public enum MathematicsContentAuditStatus
{
    NotAudited = 0,
    Pass = 1,
    PassWithWarnings = 2,
    Fail = 3,
    Ambiguous = 4
}

public enum MathematicsGenerationReadiness
{
    Unclassified = 0,
    ReadyVerified = 1,
    ReadyContextual = 2,
    ContentWeak = 3,
    SkillAmbiguous = 4,
    MappingConflict = 5,
    QuestionFamilyMissing = 6,
    SolverCapabilityMissing = 7,
    RepresentationMissing = 8,
    RequiresAcademicReview = 9,
    Blocked = 10
}

/// <summary>
/// Edulytics-owned mapping between a lesson and exact curriculum-neutral skills.
/// Official curriculum identifiers remain separate and are never synthesized here.
/// </summary>
public sealed record LessonSkillProfile
{
    public LessonSkillProfile(
        string lessonCode,
        MathematicsLessonSourceType sourceType,
        IReadOnlyList<SkillId> primarySkills,
        IReadOnlyList<SkillId>? secondarySkills = null,
        IReadOnlyList<SkillId>? prerequisites = null,
        IReadOnlyList<string>? allowedQuestionFamilies = null,
        IReadOnlyList<string>? forbiddenQuestionFamilies = null,
        MathematicsMappingConfidence mappingConfidence = MathematicsMappingConfidence.Low,
        MathematicsContentAuditStatus contentAuditStatus = MathematicsContentAuditStatus.NotAudited,
        MathematicsGenerationReadiness generationReadiness = MathematicsGenerationReadiness.Unclassified,
        IReadOnlyList<string>? evidence = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(lessonCode);
        ArgumentNullException.ThrowIfNull(primarySkills);

        var primary = primarySkills.Distinct().ToArray();
        if (primary.Length == 0)
        {
            throw new ArgumentException(
                "A lesson skill profile requires at least one primary skill.",
                nameof(primarySkills));
        }

        var secondary = (secondarySkills ?? []).Distinct().ToArray();
        if (secondary.Intersect(primary).Any())
        {
            throw new ArgumentException(
                "Primary and secondary skills must not overlap.",
                nameof(secondarySkills));
        }

        var allowed = CleanStrings(allowedQuestionFamilies);
        var forbidden = CleanStrings(forbiddenQuestionFamilies);
        if (allowed.Intersect(forbidden, StringComparer.Ordinal).Any())
        {
            throw new ArgumentException(
                "A question family cannot be both allowed and forbidden.");
        }

        LessonCode = lessonCode.Trim();
        SourceType = sourceType;
        PrimarySkills = primary;
        SecondarySkills = secondary;
        Prerequisites = (prerequisites ?? []).Distinct().ToArray();
        AllowedQuestionFamilies = allowed;
        ForbiddenQuestionFamilies = forbidden;
        MappingConfidence = mappingConfidence;
        ContentAuditStatus = contentAuditStatus;
        GenerationReadiness = generationReadiness;
        Evidence = CleanStrings(evidence);
    }

    public string LessonCode { get; }
    public MathematicsLessonSourceType SourceType { get; }
    public IReadOnlyList<SkillId> PrimarySkills { get; }
    public IReadOnlyList<SkillId> SecondarySkills { get; }
    public IReadOnlyList<SkillId> Prerequisites { get; }
    public IReadOnlyList<string> AllowedQuestionFamilies { get; }
    public IReadOnlyList<string> ForbiddenQuestionFamilies { get; }
    public MathematicsMappingConfidence MappingConfidence { get; }
    public MathematicsContentAuditStatus ContentAuditStatus { get; }
    public MathematicsGenerationReadiness GenerationReadiness { get; }
    public IReadOnlyList<string> Evidence { get; }

    private static IReadOnlyList<string> CleanStrings(IReadOnlyList<string>? values) =>
        values is null
            ? []
            : values
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => x.Trim())
                .Distinct(StringComparer.Ordinal)
                .ToArray();
}
