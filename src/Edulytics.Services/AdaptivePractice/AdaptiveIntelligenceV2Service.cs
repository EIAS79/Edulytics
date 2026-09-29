using Edulytics.Core.Entities;
using Edulytics.Core.Analytics;
using Edulytics.Core.AdaptiveAssessment;
using Edulytics.Core.AdaptivePractice;
using Edulytics.Core.AssessmentIntelligence;
using Edulytics.Core.Constants;
using Edulytics.Core.Enums;
using Edulytics.Core.Interfaces;
using Edulytics.Core.Practice;
using Edulytics.Services.AdaptiveAssessment;
using Edulytics.Services.Analytics;

namespace Edulytics.Services.AdaptivePractice;

public sealed class AdaptiveIntelligenceV2Service(
    AdaptivePracticeV2Policy policy,
    IPracticeRepository practiceRepository,
    IStudentPrivatePracticeRepository privatePracticeRepository,
    IAdaptivePracticeRepository adaptiveRepository,
    IStudentSelfEvaluationService studentSelfEvaluation,
    IAnalyticsService analytics,
    ISchoolUserRepository schoolUsers,
    AdaptiveDiagnosticAssessmentEngine diagnosticEngine)
    : IAdaptiveIntelligenceV2Service
{
    private static readonly TimeSpan LiveWindow =
        TimeSpan.FromMinutes(30);

    public async Task<AdaptiveIntelligenceV2Result<AdaptiveNextStepsView>>
        GetNextStepsAsync(
            Guid actorUserId,
            Guid academicYearId,
            Guid classGroupId,
            Guid subjectId,
            CancellationToken cancellationToken = default)
    {
        if (!IsFeatureEnabled(policy.EnableDirectNextSteps))
        {
            return AdaptiveIntelligenceV2Result<AdaptiveNextStepsView>
                .Failure(AdaptiveIntelligenceV2Error.FeatureDisabled);
        }

        var result = await studentSelfEvaluation.GetAsync(
            actorUserId,
            academicYearId,
            classGroupId,
            subjectId,
            cancellationToken);

        if (result.Value is null)
        {
            return AdaptiveIntelligenceV2Result<AdaptiveNextStepsView>
                .Failure(
                    result.Error == StudentSelfEvaluationErrorCode.AccessDenied
                        ? AdaptiveIntelligenceV2Error.AccessDenied
                        : AdaptiveIntelligenceV2Error.ScopeNotAvailable);
        }

        var schoolId =
            result.Value.OfficialEvaluation.SchoolId;

        if (!policy.AllowsSchool(schoolId))
        {
            return AdaptiveIntelligenceV2Result<AdaptiveNextStepsView>
                .Failure(AdaptiveIntelligenceV2Error.AccessDenied);
        }

        return AdaptiveIntelligenceV2Result<AdaptiveNextStepsView>
            .Success(
                new AdaptiveNextStepsView(
                    academicYearId,
                    classGroupId,
                    subjectId,
                    result.Value.NextSteps,
                    DateTime.UtcNow));
    }

    public async Task<AdaptiveIntelligenceV2Result<AdaptiveQuestionLogView>>
        GetQuestionLogAsync(
            Guid actorUserId,
            int take = 100,
            CancellationToken cancellationToken = default)
    {
        if (!IsFeatureEnabled(policy.EnableQuestionLog))
        {
            return AdaptiveIntelligenceV2Result<AdaptiveQuestionLogView>
                .Failure(AdaptiveIntelligenceV2Error.FeatureDisabled);
        }

        var student = await practiceRepository.FindStudentByUserIdAsync(
            actorUserId,
            cancellationToken);

        if (student is null ||
            student.IsArchived ||
            !policy.AllowsSchool(student.SchoolId))
        {
            return AdaptiveIntelligenceV2Result<AdaptiveQuestionLogView>
                .Failure(AdaptiveIntelligenceV2Error.AccessDenied);
        }

        var sessions = await adaptiveRepository.GetSessionsForStudentAsync(
            student.SchoolId,
            student.Id,
            take: 50,
            cancellationToken);

        if (sessions.Count == 0)
        {
            return AdaptiveIntelligenceV2Result<AdaptiveQuestionLogView>
                .Success(
                    new AdaptiveQuestionLogView(
                        0,
                        0,
                        [],
                        DateTime.UtcNow));
        }

        var sessionIds = sessions
            .Select(x => x.Id)
            .ToArray();

        var turns = await adaptiveRepository.GetTurnsForSessionsAsync(
            student.SchoolId,
            sessionIds,
            cancellationToken);

        var decisions =
            await adaptiveRepository.GetDecisionSnapshotsForSessionsAsync(
                student.SchoolId,
                sessionIds,
                cancellationToken);

        var items = await adaptiveRepository.GetItemsAsync(
            student.SchoolId,
            turns.Select(x => x.AssessmentItemId)
                .Distinct()
                .ToArray(),
            cancellationToken);

        var sessionsById =
            sessions.ToDictionary(x => x.Id);
        var decisionsByKey =
            decisions.ToDictionary(
                x => (x.SessionId, x.Sequence));
        var itemsById =
            items.ToDictionary(x => x.Id);

        var rows = turns
            .OrderByDescending(x => x.PresentedAtUtc)
            .ThenByDescending(x => x.Sequence)
            .Take(Math.Clamp(take, 1, 500))
            .Select(turn =>
            {
                var session = sessionsById[turn.SessionId];
                decisionsByKey.TryGetValue(
                    (turn.SessionId, turn.Sequence),
                    out var decision);
                itemsById.TryGetValue(
                    turn.AssessmentItemId,
                    out var item);

                return new AdaptiveQuestionLogRow(
                    session.Id,
                    session.LessonCode,
                    turn.SkillId,
                    turn.Sequence,
                    item?.Prompt ?? string.Empty,
                    turn.UiDifficultyBand,
                    turn.QuestionFamily,
                    turn.Representation,
                    turn.MathematicalComplexityScore,
                    turn.SubmittedAnswer,
                    turn.IncorrectAttemptCount,
                    turn.LastIncorrectAnswer,
                    turn.IsCorrect,
                    turn.Feedback,
                    decision?.DecisionReasonCode ?? string.Empty,
                    turn.MisconceptionFocusId,
                    turn.IsIndependentConfirmation,
                    turn.PresentedAtUtc,
                    turn.AnsweredAtUtc,
                    turn.ResponseDurationMs);
            })
            .ToArray();

        return AdaptiveIntelligenceV2Result<AdaptiveQuestionLogView>
            .Success(
                new AdaptiveQuestionLogView(
                    sessions.Count,
                    rows.Length,
                    rows,
                    DateTime.UtcNow));
    }

    public async Task<AdaptiveIntelligenceV2Result<AdaptiveClassroomSnapshot>>
        GetLiveClassroomAsync(
            Guid actorUserId,
            Guid academicYearId,
            Guid classGroupId,
            Guid subjectId,
            CancellationToken cancellationToken = default)
    {
        if (!IsFeatureEnabled(policy.EnableLiveClassroom))
        {
            return AdaptiveIntelligenceV2Result<AdaptiveClassroomSnapshot>
                .Failure(AdaptiveIntelligenceV2Error.FeatureDisabled);
        }

        var actor = await schoolUsers.GetActorAsync(
            actorUserId,
            cancellationToken);

        if (actor?.SchoolId is not Guid schoolId ||
            !actor.IsActive ||
            actor.IsLocked ||
            !actor.Roles.Any(role =>
                role is RoleNames.SchoolAdmin or
                    RoleNames.SubjectSupervisor or
                    RoleNames.Teacher) ||
            !policy.AllowsSchool(schoolId))
        {
            return AdaptiveIntelligenceV2Result<AdaptiveClassroomSnapshot>
                .Failure(AdaptiveIntelligenceV2Error.AccessDenied);
        }

        var page = await analytics.GetStudentsEvaluationAsync(
            actorUserId,
            academicYearId,
            classGroupId,
            subjectId,
            cancellationToken);

        if (page.Value is null)
        {
            return AdaptiveIntelligenceV2Result<AdaptiveClassroomSnapshot>
                .Failure(
                    page.Error == AnalyticsErrorCode.AccessDenied
                        ? AdaptiveIntelligenceV2Error.AccessDenied
                        : AdaptiveIntelligenceV2Error.ScopeNotAvailable);
        }

        var studentIds = page.Value.Students
            .Select(x => x.StudentProfileId)
            .Distinct()
            .ToArray();

        var since = DateTime.UtcNow - LiveWindow;

        var sessions = await adaptiveRepository.GetRecentSessionsAsync(
            schoolId,
            studentIds,
            since,
            take: 1000,
            cancellationToken);

        var sessionIds = sessions
            .Select(x => x.Id)
            .ToArray();

        var turns = await adaptiveRepository.GetTurnsForSessionsAsync(
            schoolId,
            sessionIds,
            cancellationToken);

        var misconceptions =
            await adaptiveRepository.GetMisconceptionStatesForStudentsAsync(
                schoolId,
                studentIds,
                cancellationToken);

        var latestSessionByStudent = sessions
            .GroupBy(x => x.StudentProfileId)
            .ToDictionary(
                group => group.Key,
                group => group
                    .OrderByDescending(x => x.StartedAtUtc)
                    .ThenByDescending(x => x.Id)
                    .First());

        var turnsBySession = turns
            .GroupBy(x => x.SessionId)
            .ToDictionary(
                group => group.Key,
                group => group
                    .OrderBy(x => x.Sequence)
                    .ToArray());

        var misconceptionCountByStudent = misconceptions
            .Where(x =>
                x.Status != AdaptiveMisconceptionStatus.Resolved)
            .GroupBy(x => x.StudentProfileId)
            .ToDictionary(
                group => group.Key,
                group => group.Count());

        var signals = page.Value.Students
            .Select(student =>
            {
                latestSessionByStudent.TryGetValue(
                    student.StudentProfileId,
                    out var session);

                AdaptivePracticeTurn? latestTurn = null;
                if (session is not null &&
                    turnsBySession.TryGetValue(
                        session.Id,
                        out var sessionTurns))
                {
                    latestTurn = sessionTurns
                        .OrderByDescending(x => x.Sequence)
                        .FirstOrDefault();
                }

                misconceptionCountByStudent.TryGetValue(
                    student.StudentProfileId,
                    out var activeMisconceptions);

                var adaptiveActive =
                    session is not null &&
                    session.Status ==
                        AdaptivePracticeSessionStatus.InProgress;

                var needsIntervention =
                    student.HighestPriority >= EvaluationPriority.High ||
                    latestTurn?.IsCorrect == false ||
                    activeMisconceptions > 0;

                return new AdaptiveClassroomStudentSignal(
                    student.StudentProfileId,
                    student.StudentNumber,
                    student.DisplayName,
                    student.HighestPriority,
                    student.CurrentMasteryPercentage,
                    adaptiveActive,
                    session?.PrimarySkillId,
                    latestTurn?.QuestionFamily,
                    latestTurn?.IsCorrect,
                    activeMisconceptions,
                    needsIntervention);
            })
            .OrderByDescending(x => x.NeedsIntervention)
            .ThenByDescending(x => x.Priority)
            .ThenBy(x => x.DisplayName, StringComparer.OrdinalIgnoreCase)
            .ToArray();

        return AdaptiveIntelligenceV2Result<AdaptiveClassroomSnapshot>
            .Success(
                new AdaptiveClassroomSnapshot(
                    page.Value.AcademicYearId,
                    page.Value.AcademicYearName,
                    page.Value.ClassGroupId,
                    page.Value.ClassName,
                    page.Value.SubjectId,
                    page.Value.SubjectName,
                    signals.Length,
                    signals.Count(x => x.AdaptiveActive),
                    signals.Count(x => x.NeedsIntervention),
                    signals.Sum(x => x.ActiveMisconceptionCount),
                    signals,
                    DateTime.UtcNow));
    }

    public async Task<AdaptiveIntelligenceV2Result<AdaptiveDiagnosticPreview>>
        GetDiagnosticPreviewAsync(
            Guid actorUserId,
            Guid curriculumAdoptionId,
            Guid lessonId,
            CancellationToken cancellationToken = default)
    {
        if (!IsFeatureEnabled(policy.EnableDiagnosticV2))
        {
            return AdaptiveIntelligenceV2Result<AdaptiveDiagnosticPreview>
                .Failure(AdaptiveIntelligenceV2Error.FeatureDisabled);
        }

        var context = await privatePracticeRepository.GetContextAsync(
            actorUserId,
            curriculumAdoptionId,
            cancellationToken);

        if (context is null ||
            !policy.AllowsSchool(context.Student.SchoolId))
        {
            return AdaptiveIntelligenceV2Result<AdaptiveDiagnosticPreview>
                .Failure(AdaptiveIntelligenceV2Error.AccessDenied);
        }

        var lesson = context.Lessons.SingleOrDefault(
            x => x.Id == lessonId);

        if (lesson is null)
        {
            return AdaptiveIntelligenceV2Result<AdaptiveDiagnosticPreview>
                .Failure(AdaptiveIntelligenceV2Error.ScopeNotAvailable);
        }

        var outcomeNodeIds = context.LessonOutcomes
            .Where(x => x.PedagogicalLessonId == lessonId)
            .Select(x => x.OutcomeNodeId)
            .ToHashSet();

        var outcomes = context.LearningOutcomes
            .Where(x =>
                x.OfficialContentNodeId.HasValue &&
                outcomeNodeIds.Contains(
                    x.OfficialContentNodeId.Value))
            .OrderBy(x => x.Order)
            .ThenBy(x => x.Code, StringComparer.Ordinal)
            .ToArray();

        if (outcomes.Length == 0)
        {
            return AdaptiveIntelligenceV2Result<AdaptiveDiagnosticPreview>
                .Failure(AdaptiveIntelligenceV2Error.NotEnoughEvidence);
        }

        AdaptiveAssessmentDecision decision;
        try
        {
            decision = diagnosticEngine.DecideNext(
                new AdaptiveAssessmentRequest(
                    context.Student.SchoolId,
                    context.Adoption.Id,
                    context.Adoption.CurriculumLevelKey ?? string.Empty,
                    outcomes.Select(x => x.Id).ToArray(),
                    StudentProfile: null,
                    AssessmentPurpose.Diagnostic,
                    PreviousResponses: [],
                    MathematicsSkillStates: null));
        }
        catch (InvalidOperationException)
        {
            return AdaptiveIntelligenceV2Result<AdaptiveDiagnosticPreview>
                .Failure(AdaptiveIntelligenceV2Error.NotEnoughEvidence);
        }

        var target = outcomes.SingleOrDefault(
            x => x.Id == decision.TargetLearningOutcomeId);

        if (target is null)
        {
            return AdaptiveIntelligenceV2Result<AdaptiveDiagnosticPreview>
                .Failure(AdaptiveIntelligenceV2Error.NotEnoughEvidence);
        }

        return AdaptiveIntelligenceV2Result<AdaptiveDiagnosticPreview>
            .Success(
                new AdaptiveDiagnosticPreview(
                    context.Adoption.Id,
                    lesson.Id,
                    lesson.Code,
                    target.Id,
                    target.Code,
                    target.Description,
                    decision.NextDifficulty,
                    decision.EvidenceCreditMultiplier,
                    decision.RequiresFreshExposure,
                    decision.Reason,
                    decision.FormulaVersion,
                    DateTime.UtcNow));
    }

    private bool IsFeatureEnabled(bool featureFlag) =>
        policy.Enabled &&
        policy.Mode != AdaptivePracticeV2Mode.Off &&
        featureFlag;
}
