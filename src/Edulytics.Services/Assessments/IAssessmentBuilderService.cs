using Edulytics.Core.Assessments;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Assessments;

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
    IReadOnlyList<Guid> OutcomeIds);

public sealed record AssessmentBuilderWorkspace(
    AssessmentDetails Details,
    IReadOnlyList<AssessmentBuilderQuestion> Questions,
    decimal CurrentMarks,
    decimal RemainingMarks,
    decimal? ClassMasteryPercentage,
    bool CanGenerateNatively,
    bool ReadyToPublish,
    string ReadinessMessage);

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

public sealed record GenerateBuilderQuestionsRequest(
    Guid AssessmentId,
    int QuestionCount,
    decimal MaxScorePerQuestion,
    AssessmentBuilderDifficulty Difficulty,
    IReadOnlyList<Guid> OutcomeIds,
    byte[] AssessmentRowVersion,
    int Seed = 0);

public interface IAssessmentBuilderService
{
    Task<AssessmentQueryResult<AssessmentBuilderWorkspace>> GetWorkspaceAsync(
        Guid actorUserId,
        Guid assessmentId,
        CancellationToken cancellationToken = default);

    Task<AssessmentCommandResult> CreateManualQuestionAsync(
        Guid actorUserId,
        CreateManualBuilderQuestionRequest request,
        CancellationToken cancellationToken = default);

    Task<AssessmentCommandResult> GenerateQuestionsAsync(
        Guid actorUserId,
        GenerateBuilderQuestionsRequest request,
        CancellationToken cancellationToken = default);

    Task<AssessmentCommandResult> ApproveQuestionAsync(
        Guid actorUserId,
        Guid assessmentId,
        Guid questionId,
        byte[] assessmentRowVersion,
        CancellationToken cancellationToken = default);

    Task<AssessmentCommandResult> DeleteQuestionAsync(
        Guid actorUserId,
        Guid assessmentId,
        Guid questionId,
        byte[] assessmentRowVersion,
        CancellationToken cancellationToken = default);

    Task<AssessmentCommandResult> PublishAsync(
        Guid actorUserId,
        Guid assessmentId,
        byte[] assessmentRowVersion,
        CancellationToken cancellationToken = default);
}
