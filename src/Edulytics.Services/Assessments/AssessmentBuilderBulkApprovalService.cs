using System.Text.Json;
using System.Text.Json.Nodes;
using Edulytics.Core.Assessments;
using Edulytics.Core.Constants;
using Edulytics.Core.Enums;
using Edulytics.Core.Interfaces;
using Edulytics.Core.Users;

namespace Edulytics.Services.Assessments;

public interface IAssessmentBuilderBulkApprovalService
{
    Task<AssessmentCommandResult> ApproveAllDraftQuestionsAsync(
        Guid actorUserId,
        Guid assessmentId,
        byte[] assessmentRowVersion,
        CancellationToken cancellationToken = default);
}

public sealed class AssessmentBuilderBulkApprovalService(
    IAssessmentBuilderRepository repository,
    IAssessmentService assessments,
    ISchoolUserRepository users) : IAssessmentBuilderBulkApprovalService
{
    public async Task<AssessmentCommandResult> ApproveAllDraftQuestionsAsync(
        Guid actorUserId,
        Guid assessmentId,
        byte[] assessmentRowVersion,
        CancellationToken cancellationToken = default)
    {
        var actor = await users.GetActorAsync(actorUserId, cancellationToken);
        if (actor is null ||
            !actor.IsActive ||
            actor.IsLocked ||
            !actor.SchoolId.HasValue ||
            actor.Roles.Count != 1 ||
            actor.Roles[0] != RoleNames.Teacher)
        {
            return Failure(AssessmentErrorCode.AccessDenied);
        }

        var details = await assessments.GetDetailsAsync(actorUserId, assessmentId, cancellationToken);
        if (details.Value is null)
            return Failure(details.Error ?? AssessmentErrorCode.AccessDenied);

        if (details.Value.Assessment.Status != AssessmentStatus.Draft)
            return Failure(AssessmentErrorCode.AssessmentNotDraft);

        var context = await repository.GetContextAsync(
            actor.SchoolId.Value,
            assessmentId,
            cancellationToken);

        if (context is null)
            return Failure(AssessmentErrorCode.AssessmentNotFound);

        var questionIds = context.Questions.Select(x => x.Id).ToHashSet();
        var draftItems = context.Items
            .Where(x => ReadStatus(x.ValidationMetadataJson) == AssessmentBuilderQuestionStatus.Draft)
            .ToArray();

        if (draftItems.Any(x => !questionIds.Contains(x.Id)))
            return Failure(AssessmentErrorCode.QuestionNotFound);

        if (draftItems.Length == 0)
            return AssessmentCommandResult.Success(assessmentId);

        foreach (var item in draftItems)
        {
            item.ValidationMetadataJson = SetStatus(
                item.ValidationMetadataJson,
                AssessmentBuilderQuestionStatus.Approved);
        }

        // AssessmentBuilderRepository recognizes an approval-only unit of work and
        // persists all modified AssessmentItems in one SaveChanges transaction while
        // validating the submitted assessment row version. This prevents a timeout
        // from committing only the first part of a bulk approval.
        context.Assessment.UpdatedAtUtc = DateTime.UtcNow;
        var saved = await repository.SaveAsync(
            context.Assessment,
            assessmentRowVersion,
            cancellationToken);

        if (saved.Succeeded)
            return AssessmentCommandResult.Success(assessmentId);

        return Failure(
            saved.Error == AssessmentPersistenceError.Conflict
                ? AssessmentErrorCode.ConcurrencyConflict
                : AssessmentErrorCode.PersistenceError);
    }

    private static AssessmentBuilderQuestionStatus ReadStatus(string? json)
    {
        if (string.IsNullOrWhiteSpace(json))
            return AssessmentBuilderQuestionStatus.Draft;

        try
        {
            var value = JsonNode.Parse(json)?["builderStatus"]?.GetValue<string>();
            return Enum.TryParse<AssessmentBuilderQuestionStatus>(value, true, out var status)
                ? status
                : AssessmentBuilderQuestionStatus.Draft;
        }
        catch (JsonException)
        {
            return AssessmentBuilderQuestionStatus.Draft;
        }
    }

    private static string SetStatus(string? json, AssessmentBuilderQuestionStatus status)
    {
        JsonObject root;
        try
        {
            root = string.IsNullOrWhiteSpace(json)
                ? new JsonObject()
                : JsonNode.Parse(json) as JsonObject ?? new JsonObject();
        }
        catch (JsonException)
        {
            root = new JsonObject();
        }

        root["builderStatus"] = status.ToString();
        return root.ToJsonString(new JsonSerializerOptions { WriteIndented = false });
    }

    private static AssessmentCommandResult Failure(AssessmentErrorCode error) =>
        AssessmentCommandResult.Failure(string.Empty, error);
}
