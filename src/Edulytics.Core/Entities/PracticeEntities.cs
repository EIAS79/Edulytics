
using Edulytics.Core.Enums;
using Edulytics.Core.Interfaces;

namespace Edulytics.Core.Entities;

public sealed class AssessmentItem : ISchoolScoped
{
    public Guid Id { get; set; }
    public Guid SchoolId { get; set; }
    public Guid CurriculumAdoptionId { get; set; }
    public Guid? CurriculumPedagogicalLessonId { get; set; }
    public Guid? CurriculumTopicId { get; set; }
    public AssessmentItemSource Source { get; set; }
    public AssessmentItemType ItemType { get; set; }
    public AssessmentItemDifficulty Difficulty { get; set; }
    public string Prompt { get; set; } = string.Empty;
    public string CorrectAnswer { get; set; } = string.Empty;
    public string Solution { get; set; } = string.Empty;
    public Guid? CreatedByUserId { get; set; }
    public string? GenerationMethod { get; set; }
    public string? GenerationFamily { get; set; }
    public string? GenerationParametersJson { get; set; }
    public string ExposureFingerprint { get; set; } = string.Empty;
    public string? ValidationMetadataJson { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public byte[] RowVersion { get; set; } = [];
}

public sealed class AssessmentItemOutcome : ISchoolScoped
{
    public Guid Id { get; set; }
    public Guid SchoolId { get; set; }
    public Guid AssessmentItemId { get; set; }
    public Guid LearningOutcomeId { get; set; }
}

public sealed class PracticeAttempt : ISchoolScoped
{
    public Guid Id { get; set; }
    public Guid SchoolId { get; set; }
    public Guid StudentProfileId { get; set; }
    public Guid CurriculumAdoptionId { get; set; }
    public Guid? CurriculumPedagogicalLessonId { get; set; }

    // Student-owned AI/self-test attempts are private by contract. Private attempts
    // never participate in school/teacher official analytics or reports.
    public bool IsPrivate { get; set; }

    public PracticeAttemptStatus Status { get; set; }
    public DateTime StartedAtUtc { get; set; }
    public DateTime? SubmittedAtUtc { get; set; }
    public decimal Score { get; set; }
    public decimal MaxScore { get; set; }
    public decimal Percentage { get; set; }
    public byte[] RowVersion { get; set; } = [];
}

public sealed class PracticeAttemptItem : ISchoolScoped
{
    public Guid Id { get; set; }
    public Guid SchoolId { get; set; }
    public Guid PracticeAttemptId { get; set; }
    public Guid AssessmentItemId { get; set; }
    public int Order { get; set; }
    public decimal MaxScore { get; set; }
}

public sealed class PracticeResponse : ISchoolScoped
{
    public Guid Id { get; set; }
    public Guid SchoolId { get; set; }
    public Guid PracticeAttemptItemId { get; set; }
    public string Answer { get; set; } = string.Empty;
    public bool IsCorrect { get; set; }
    public decimal Score { get; set; }
    public string Feedback { get; set; } = string.Empty;
    public DateTime AnsweredAtUtc { get; set; }
}

public sealed class LearningEvidence : ISchoolScoped
{
    public Guid Id { get; set; }
    public Guid SchoolId { get; set; }
    public Guid StudentProfileId { get; set; }
    public Guid LearningOutcomeId { get; set; }
    public Guid PracticeAttemptId { get; set; }
    public Guid AssessmentItemId { get; set; }
    public LearningEvidenceType EvidenceType { get; set; }
    public AssessmentItemDifficulty Difficulty { get; set; }
    public bool IsCorrect { get; set; }
    public decimal Score { get; set; }
    public decimal MaxScore { get; set; }
    public DateTime OccurredAtUtc { get; set; }
}

public sealed class StudentItemExposure : ISchoolScoped
{
    public Guid Id { get; set; }
    public Guid SchoolId { get; set; }
    public Guid StudentProfileId { get; set; }
    public Guid AssessmentItemId { get; set; }
    public string ExposureFingerprint { get; set; } = string.Empty;
    public DateTime ExposedAtUtc { get; set; }
}
