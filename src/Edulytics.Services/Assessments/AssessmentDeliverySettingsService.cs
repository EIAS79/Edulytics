using Edulytics.Core.Constants;
using Edulytics.Core.Enums;
using Edulytics.Core.Interfaces;

namespace Edulytics.Services.Assessments;

public sealed record AssessmentDeliverySettingsWorkspace(
    AssessmentListItem Assessment,
    IReadOnlyList<AssessmentTargetStudentOption> TargetStudents);

public interface IAssessmentDeliverySettingsService
{
    Task<AssessmentQueryResult<AssessmentDeliverySettingsWorkspace>> GetAsync(
        Guid actorUserId,
        Guid assessmentId,
        CancellationToken cancellationToken = default);

    Task<AssessmentCommandResult> UpdateAsync(
        Guid actorUserId,
        UpdateAssessmentDeliverySettingsRequest request,
        CancellationToken cancellationToken = default);
}

public sealed class AssessmentDeliverySettingsService(
    IAssessmentService assessments,
    IAssessmentBuilderRepository repository,
    ISchoolUserRepository users) : IAssessmentDeliverySettingsService
{
    public async Task<AssessmentQueryResult<AssessmentDeliverySettingsWorkspace>> GetAsync(
        Guid actorUserId,
        Guid assessmentId,
        CancellationToken cancellationToken = default)
    {
        var actor = await users.GetActorAsync(actorUserId, cancellationToken);
        if (actor is null || !actor.IsActive || actor.IsLocked || !actor.SchoolId.HasValue ||
            actor.Roles.Count != 1 || actor.Roles[0] != RoleNames.Teacher)
            return AssessmentQueryResult<AssessmentDeliverySettingsWorkspace>.Failure(
                AssessmentErrorCode.AccessDenied);

        var details = await assessments.GetDetailsAsync(actorUserId, assessmentId, cancellationToken);
        if (details.Value is null)
            return AssessmentQueryResult<AssessmentDeliverySettingsWorkspace>.Failure(
                details.Error ?? AssessmentErrorCode.AccessDenied);

        var students = await repository.ListTargetStudentsAsync(
            actor.SchoolId.Value, assessmentId, cancellationToken);

        return AssessmentQueryResult<AssessmentDeliverySettingsWorkspace>.Success(
            new AssessmentDeliverySettingsWorkspace(
                details.Value.Assessment,
                students
                    .Select(x => new AssessmentTargetStudentOption(x.Id, x.StudentNumber, x.DisplayName))
                    .ToArray()));
    }

    public async Task<AssessmentCommandResult> UpdateAsync(
        Guid actorUserId,
        UpdateAssessmentDeliverySettingsRequest request,
        CancellationToken cancellationToken = default)
    {
        var actor = await users.GetActorAsync(actorUserId, cancellationToken);
        if (actor is null || !actor.IsActive || actor.IsLocked || !actor.SchoolId.HasValue ||
            actor.Roles.Count != 1 || actor.Roles[0] != RoleNames.Teacher)
            return AssessmentCommandResult.Failure(string.Empty, AssessmentErrorCode.AccessDenied);

        var details = await assessments.GetDetailsAsync(actorUserId, request.AssessmentId, cancellationToken);
        if (details.Value is null)
            return AssessmentCommandResult.Failure(
                string.Empty, details.Error ?? AssessmentErrorCode.AccessDenied);
        if (details.Value.Assessment.Status != AssessmentStatus.Draft)
            return AssessmentCommandResult.Failure(string.Empty, AssessmentErrorCode.AssessmentNotDraft);

        if (!Enum.IsDefined(request.TargetType) ||
            !Enum.IsDefined(request.DeliveryMode) ||
            !Enum.IsDefined(request.DifficultyBand))
            return AssessmentCommandResult.Failure(string.Empty, AssessmentErrorCode.Required);

        var context = await repository.GetContextAsync(
            actor.SchoolId.Value, request.AssessmentId, cancellationToken);
        if (context is null)
            return AssessmentCommandResult.Failure(string.Empty, AssessmentErrorCode.AssessmentNotFound);

        Guid? targetStudentId = null;
        if (request.TargetType == AssessmentTargetType.Student)
        {
            if (!request.TargetStudentProfileId.HasValue || request.TargetStudentProfileId.Value == Guid.Empty)
                return AssessmentCommandResult.Failure(string.Empty, AssessmentErrorCode.StudentNotFound);

            var eligible = await repository.ListTargetStudentsAsync(
                actor.SchoolId.Value, request.AssessmentId, cancellationToken);
            if (eligible.All(x => x.Id != request.TargetStudentProfileId.Value))
                return AssessmentCommandResult.Failure(string.Empty, AssessmentErrorCode.StudentNotEnrolled);

            targetStudentId = request.TargetStudentProfileId.Value;
        }

        context.Assessment.TargetType = request.TargetType;
        context.Assessment.TargetStudentProfileId = targetStudentId;
        context.Assessment.DeliveryMode = request.DeliveryMode;
        context.Assessment.DifficultyBand = request.DifficultyBand;
        context.Assessment.UpdatedAtUtc = DateTime.UtcNow;

        var saved = await repository.SaveAsync(
            context.Assessment, request.AssessmentRowVersion, cancellationToken);

        return saved.Succeeded
            ? AssessmentCommandResult.Success(context.Assessment.Id)
            : AssessmentCommandResult.Failure(
                string.Empty,
                saved.Error == Edulytics.Core.Assessments.AssessmentPersistenceError.Conflict
                    ? AssessmentErrorCode.ConcurrencyConflict
                    : AssessmentErrorCode.PersistenceError);
    }
}
