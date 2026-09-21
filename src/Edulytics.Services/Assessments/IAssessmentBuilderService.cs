using Edulytics.Core.Assessments;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Assessments;

public enum AssessmentGenerationScopeType
{
    Outcomes = 0,
    Lessons = 1,
    Units = 2,
    Curriculum = 3
}

public sealed record AssessmentBuilderLessonOption(
    Guid Id,
    string Code,
    string UnitKey,
    string UnitTitle,
    string Title,
    bool HasOfficialOutcome,
    bool AiSupported,
    IReadOnlyList<Guid> OutcomeIds);

public sealed record AssessmentBuilderUnitOption(
    string UnitKey,
    string UnitTitle,
    int LessonCount,
    int AiSupportedLessonCount);

public sealed record AssessmentBuilderQuestion(
    Guid Id,
    int Order,
    string Prompt,
    decimal MaxScore,
    AssessmentItemSource? Source,
    AssessmentItemDifficulty? Difficulty,
    AssessmentBuilderQuestionStatus Status,
    string CorrectAnswer,
    string Solution,
    IReadOnlyList<Guid> OutcomeIds)
{
    public Guid? LessonId { get; init; }
    public string? LessonTitle { get; init; }
};

public sealed record AssessmentTargetStudentOption(
    Guid Id,
    string StudentNumber,
    string DisplayName);

public sealed record AssessmentBuilderWorkspace(
    AssessmentDetails Details,
    IReadOnlyList<AssessmentBuilderQuestion> Questions,
    decimal CurrentMarks,
    decimal RemainingMarks,
    decimal? ClassMasteryPercentage,
    bool CanGenerateNatively,
    bool ReadyToPublish,
    string ReadinessMessage,
    IReadOnlyList<AssessmentTargetStudentOption>? TargetStudents = null,
    IReadOnlyList<Guid>? AiSupportedOutcomeIds = null)
{
    public IReadOnlyList<AssessmentBuilderLessonOption> Lessons { get; init; } = [];
    public IReadOnlyList<AssessmentBuilderUnitOption> Units { get; init; } = [];
};

public sealed record UpdateAssessmentDeliverySettingsRequest(
    Guid AssessmentId,
    AssessmentTargetType TargetType,
    Guid? TargetStudentProfileId,
    AssessmentDeliveryMode DeliveryMode,
    AssessmentDifficultyBand DifficultyBand,
    byte[] AssessmentRowVersion);

public sealed record CreateManualBuilderQuestionRequest(
    Guid AssessmentId,
    string Prompt,
    string CorrectAnswer,
    string Solution,
    decimal MaxScore,
    int Order,
    AssessmentItemDifficulty Difficulty,
    IReadOnlyList<Guid> OutcomeIds,
    byte[] AssessmentRowVersion);

public sealed record EditBuilderQuestionRequest(
    Guid AssessmentId,
    Guid QuestionId,
    string Prompt,
    string CorrectAnswer,
    string Solution,
    decimal MaxScore,
    int Order,
    AssessmentItemDifficulty Difficulty,
    IReadOnlyList<Guid> OutcomeIds,
    byte[] AssessmentRowVersion);

public sealed record GenerateBuilderQuestionsRequest(
    Guid AssessmentId,
    int QuestionCount,
    decimal MaxScorePerQuestion,
    AssessmentBuilderDifficulty Difficulty,
    IReadOnlyList<Guid> OutcomeIds,
    byte[] AssessmentRowVersion,
    int Seed = 0)
{
    public AssessmentGenerationScopeType ScopeType { get; init; } = AssessmentGenerationScopeType.Outcomes;
    public IReadOnlyList<Guid> LessonIds { get; init; } = [];
    public IReadOnlyList<string> UnitKeys { get; init; } = [];
};

public interface IAssessmentBuilderService
{
    Task<AssessmentQueryResult<AssessmentBuilderWorkspace>> GetWorkspaceAsync(Guid actorUserId, Guid assessmentId, CancellationToken cancellationToken = default);
    Task<AssessmentCommandResult> CreateManualQuestionAsync(Guid actorUserId, CreateManualBuilderQuestionRequest request, CancellationToken cancellationToken = default);
    Task<AssessmentCommandResult> EditQuestionAsync(Guid actorUserId, EditBuilderQuestionRequest request, CancellationToken cancellationToken = default);
    Task<AssessmentCommandResult> GenerateQuestionsAsync(Guid actorUserId, GenerateBuilderQuestionsRequest request, CancellationToken cancellationToken = default);
    Task<AssessmentCommandResult> RegenerateQuestionAsync(Guid actorUserId, Guid assessmentId, Guid questionId, int seed, byte[] assessmentRowVersion, CancellationToken cancellationToken = default);
    Task<AssessmentCommandResult> ApproveQuestionAsync(Guid actorUserId, Guid assessmentId, Guid questionId, byte[] assessmentRowVersion, CancellationToken cancellationToken = default);
    Task<AssessmentCommandResult> DeleteQuestionAsync(Guid actorUserId, Guid assessmentId, Guid questionId, byte[] assessmentRowVersion, CancellationToken cancellationToken = default);
    Task<AssessmentCommandResult> PublishAsync(Guid actorUserId, Guid assessmentId, byte[] assessmentRowVersion, CancellationToken cancellationToken = default);
}
