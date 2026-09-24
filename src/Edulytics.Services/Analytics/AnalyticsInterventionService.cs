using Edulytics.Core.Constants;
using Edulytics.Core.Enums;
using Edulytics.Core.Interfaces;
using Edulytics.Services.Assessments;

namespace Edulytics.Services.Analytics;

/// <summary>
/// Turns an explainable Student 360 skill gap into a targeted Assessment draft.
/// The draft is intentionally left in teacher review state: generated questions
/// must still be reviewed/approved/published through the existing Assessment
/// Builder workflow.
/// </summary>
public sealed class AnalyticsInterventionService : IAnalyticsInterventionService
{
    private readonly IAnalyticsService _analytics;
    private readonly IAssessmentService _assessments;
    private readonly IAssessmentDeliverySettingsService _delivery;
    private readonly IAssessmentBuilderService _builder;
    private readonly ISchoolUserRepository _users;

    public AnalyticsInterventionService(
        IAnalyticsService analytics,
        IAssessmentService assessments,
        IAssessmentDeliverySettingsService delivery,
        IAssessmentBuilderService builder,
        ISchoolUserRepository users)
    {
        _analytics = analytics;
        _assessments = assessments;
        _delivery = delivery;
        _builder = builder;
        _users = users;
    }

    public async Task<AnalyticsInterventionResult> CreateTargetedCheckAsync(
        Guid actorUserId,
        CreateInterventionCheckRequest request,
        CancellationToken cancellationToken = default)
    {
        if (actorUserId == Guid.Empty ||
            request.StudentProfileId == Guid.Empty ||
            request.AcademicYearId == Guid.Empty ||
            request.ClassGroupId == Guid.Empty ||
            request.SubjectId == Guid.Empty ||
            request.LearningOutcomeId == Guid.Empty ||
            string.IsNullOrWhiteSpace(request.SkillKey) ||
            request.QuestionCount is < 3 or > 10)
        {
            return AnalyticsInterventionResult.Failure(
                AnalyticsInterventionErrorCode.InvalidRequest);
        }

        var actor = await _users.GetActorAsync(
            actorUserId,
            cancellationToken);

        if (actor is null ||
            !actor.IsActive ||
            actor.IsLocked ||
            actor.Roles.Count != 1 ||
            actor.Roles[0] != RoleNames.Teacher)
        {
            return AnalyticsInterventionResult.Failure(
                AnalyticsInterventionErrorCode.AccessDenied);
        }

        var evaluationResult = await _analytics.GetStudentEvaluationAsync(
            actorUserId,
            request.StudentProfileId,
            request.AcademicYearId,
            request.ClassGroupId,
            request.SubjectId,
            cancellationToken);

        if (evaluationResult.Value is null)
        {
            return AnalyticsInterventionResult.Failure(
                evaluationResult.Error == AnalyticsErrorCode.AccessDenied
                    ? AnalyticsInterventionErrorCode.AccessDenied
                    : AnalyticsInterventionErrorCode.EvaluationUnavailable);
        }

        var evaluation = evaluationResult.Value;
        var skill = evaluation.Evaluation.Skills.FirstOrDefault(
            x =>
                x.LearningOutcomeId == request.LearningOutcomeId &&
                string.Equals(
                    x.SkillKey,
                    request.SkillKey.Trim(),
                    StringComparison.Ordinal));

        if (skill is null ||
            !skill.TargetedCheckAvailable ||
            skill.Status is
                Edulytics.Core.Analytics.EvaluationSkillStatus.NotYetAssessed or
                Edulytics.Core.Analytics.EvaluationSkillStatus.InsufficientEvidence)
        {
            return AnalyticsInterventionResult.Failure(
                AnalyticsInterventionErrorCode.SkillNotFound);
        }

        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var term = evaluation.Terms
            .Where(x =>
                today >= x.StartsOn &&
                today <= x.EndsOn)
            .OrderBy(x => x.StartsOn)
            .FirstOrDefault()
            ?? evaluation.Terms
                .Where(x => x.StartsOn >= today)
                .OrderBy(x => x.StartsOn)
                .FirstOrDefault()
            ?? evaluation.Terms
                .OrderByDescending(x => x.EndsOn)
                .FirstOrDefault();

        if (term is null)
        {
            return AnalyticsInterventionResult.Failure(
                AnalyticsInterventionErrorCode.TermNotFound);
        }

        var assessmentDate =
            today < term.StartsOn
                ? term.StartsOn
                : today > term.EndsOn
                    ? term.EndsOn
                    : today;

        var cleanSkillName =
            string.IsNullOrWhiteSpace(skill.SkillName)
                ? skill.SkillKey
                : skill.SkillName.Trim();

        var title =
            $"Intervention check - {cleanSkillName} - {DateTime.UtcNow:yyyyMMdd-HHmmssfff}";

        if (title.Length > 200)
            title = title[..200];

        var create = await _assessments.CreateAssessmentAsync(
            actorUserId,
            new CreateAssessmentRequest(
                request.ClassGroupId,
                request.SubjectId,
                term.TermId,
                title,
                assessmentDate,
                request.QuestionCount),
            cancellationToken);

        if (!create.Succeeded ||
            !create.EntityId.HasValue)
        {
            return AnalyticsInterventionResult.Failure(
                create.Error == AssessmentErrorCode.AccessDenied ||
                create.Error == AssessmentErrorCode.TeacherNotAssigned
                    ? AnalyticsInterventionErrorCode.AccessDenied
                    : AnalyticsInterventionErrorCode.AssessmentCreateFailed);
        }

        var assessmentId = create.EntityId.Value;

        var details = await _assessments.GetDetailsAsync(
            actorUserId,
            assessmentId,
            cancellationToken);

        if (details.Value is null)
        {
            return AnalyticsInterventionResult.Failure(
                AnalyticsInterventionErrorCode.AssessmentCreateFailed);
        }

        var delivery = await _delivery.UpdateAsync(
            actorUserId,
            new UpdateAssessmentDeliverySettingsRequest(
                assessmentId,
                AssessmentTargetType.Student,
                request.StudentProfileId,
                AssessmentDeliveryMode.Online,
                AssessmentDifficultyBand.AtClassLevel,
                details.Value.Assessment.RowVersion),
            cancellationToken);

        if (!delivery.Succeeded)
        {
            await TryDeleteDraftAsync(
                actorUserId,
                assessmentId,
                cancellationToken);

            return AnalyticsInterventionResult.Failure(
                delivery.Error == AssessmentErrorCode.AccessDenied ||
                delivery.Error == AssessmentErrorCode.StudentNotEnrolled
                    ? AnalyticsInterventionErrorCode.AccessDenied
                    : AnalyticsInterventionErrorCode.DeliverySetupFailed);
        }

        var workspace = await _builder.GetWorkspaceAsync(
            actorUserId,
            assessmentId,
            cancellationToken);

        if (workspace.Value is null)
        {
            await TryDeleteDraftAsync(
                actorUserId,
                assessmentId,
                cancellationToken);

            return AnalyticsInterventionResult.Failure(
                AnalyticsInterventionErrorCode.QuestionGenerationFailed);
        }

        var generated = await _builder.GenerateQuestionsAsync(
            actorUserId,
            new GenerateBuilderQuestionsRequest(
                assessmentId,
                request.QuestionCount,
                1m,
                Edulytics.Core.Assessments.AssessmentBuilderDifficulty.AtClassLevel,
                [request.LearningOutcomeId],
                workspace.Value.Details.Assessment.RowVersion,
                request.Seed),
            cancellationToken);

        if (!generated.Succeeded)
        {
            await TryDeleteDraftAsync(
                actorUserId,
                assessmentId,
                cancellationToken);

            return AnalyticsInterventionResult.Failure(
                generated.Error == AssessmentErrorCode.AccessDenied
                    ? AnalyticsInterventionErrorCode.AccessDenied
                    : AnalyticsInterventionErrorCode.QuestionGenerationFailed);
        }

        return AnalyticsInterventionResult.Success(
            new AnalyticsInterventionCheck(
                assessmentId,
                title,
                request.StudentProfileId,
                request.LearningOutcomeId,
                skill.SkillKey,
                cleanSkillName,
                skill.CurrentMasteryPercentage,
                skill.AssessmentMasteryPercentage,
                assessmentDate,
                request.QuestionCount,
                $"/school/assessments/{assessmentId:D}/builder"));
    }

    private async Task TryDeleteDraftAsync(
        Guid actorUserId,
        Guid assessmentId,
        CancellationToken cancellationToken)
    {
        var details = await _assessments.GetDetailsAsync(
            actorUserId,
            assessmentId,
            cancellationToken);

        if (details.Value is null ||
            details.Value.Assessment.Status != AssessmentStatus.Draft)
        {
            return;
        }

        await _assessments.DeleteAssessmentAsync(
            actorUserId,
            new DeleteAssessmentRequest(
                assessmentId,
                details.Value.Assessment.RowVersion),
            cancellationToken);
    }
}
