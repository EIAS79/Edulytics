using System.Globalization;
using System.Text.Json;
using Edulytics.Core.Assessments;
using Edulytics.Core.Constants;
using Edulytics.Core.Entities;
using Edulytics.Core.Enums;
using Edulytics.Core.Interfaces;
using Edulytics.Core.Realtime;
using Edulytics.Services.Auditing;

namespace Edulytics.Services.Assessments;

public sealed class StudentAssessmentDeliveryService(
    IAssessmentRepository assessments,
    IAssessmentBuilderRepository builder,
    ISchoolUserRepository users,
    ISchoolRepository schools,
    IAuditService? audit = null) : IStudentAssessmentDeliveryService
{
    public async Task<StudentAssessmentDeliveryResult<StudentAssessmentAttempt>> GetAttemptAsync(
        Guid actorUserId,
        Guid assessmentId,
        CancellationToken cancellationToken = default)
    {
        var resolved = await ResolveAsync(actorUserId, assessmentId, cancellationToken);
        if (resolved.Error.HasValue)
            return StudentAssessmentDeliveryResult<StudentAssessmentAttempt>.Failure(resolved.Error.Value);

        if (resolved.Snapshot!.Results.Any(x =>
                x.AssessmentId == assessmentId &&
                x.StudentProfileId == resolved.Profile!.Id))
            return StudentAssessmentDeliveryResult<StudentAssessmentAttempt>.Failure(
                StudentAssessmentDeliveryErrorCode.AlreadySubmitted);

        var context = await builder.GetContextAsync(resolved.SchoolId, assessmentId, cancellationToken);
        if (context is null)
            return StudentAssessmentDeliveryResult<StudentAssessmentAttempt>.Failure(
                StudentAssessmentDeliveryErrorCode.AssessmentNotFound);

        var itemIds = context.Items.Select(x => x.Id).ToHashSet();
        if (context.Questions.Count == 0 || context.Questions.Any(x => !itemIds.Contains(x.Id)))
            return StudentAssessmentDeliveryResult<StudentAssessmentAttempt>.Failure(
                StudentAssessmentDeliveryErrorCode.AssessmentNotFound);

        return StudentAssessmentDeliveryResult<StudentAssessmentAttempt>.Success(
            new StudentAssessmentAttempt(
                context.Assessment.Id,
                context.Assessment.Title,
                context.Assessment.AssessmentDate,
                context.Assessment.MaxScore,
                context.Assessment.DifficultyBand,
                context.Questions
                    .OrderBy(x => x.Order)
                    .Select(x => new StudentAssessmentQuestion(x.Id, x.Order, x.Prompt, x.MaxScore))
                    .ToArray()));
    }

    public async Task<StudentAssessmentDeliveryResult<StudentAssessmentSubmission>> SubmitAsync(
        Guid actorUserId,
        Guid assessmentId,
        IReadOnlyList<StudentAssessmentResponse> responses,
        CancellationToken cancellationToken = default)
    {
        var resolved = await ResolveAsync(actorUserId, assessmentId, cancellationToken);
        if (resolved.Error.HasValue)
            return StudentAssessmentDeliveryResult<StudentAssessmentSubmission>.Failure(resolved.Error.Value);

        if (resolved.Snapshot!.Results.Any(x =>
                x.AssessmentId == assessmentId &&
                x.StudentProfileId == resolved.Profile!.Id))
            return StudentAssessmentDeliveryResult<StudentAssessmentSubmission>.Failure(
                StudentAssessmentDeliveryErrorCode.AlreadySubmitted);

        var context = await builder.GetContextAsync(resolved.SchoolId, assessmentId, cancellationToken);
        if (context is null)
            return StudentAssessmentDeliveryResult<StudentAssessmentSubmission>.Failure(
                StudentAssessmentDeliveryErrorCode.AssessmentNotFound);

        var questions = context.Questions.OrderBy(x => x.Order).ToArray();
        if (questions.Length == 0 ||
            responses.Count != questions.Length ||
            responses.Select(x => x.QuestionId).Distinct().Count() != questions.Length)
            return StudentAssessmentDeliveryResult<StudentAssessmentSubmission>.Failure(
                StudentAssessmentDeliveryErrorCode.InvalidSubmission);

        var responseMap = responses.ToDictionary(x => x.QuestionId);
        var itemMap = context.Items.ToDictionary(x => x.Id);
        if (questions.Any(x => !responseMap.ContainsKey(x.Id) || !itemMap.ContainsKey(x.Id)))
            return StudentAssessmentDeliveryResult<StudentAssessmentSubmission>.Failure(
                StudentAssessmentDeliveryErrorCode.InvalidSubmission);

        decimal score = 0m;
        var scored = new List<(AssessmentQuestion Question, string Response, decimal Score)>();
        foreach (var question in questions)
        {
            var response = (responseMap[question.Id].ResponseText ?? string.Empty).Trim();
            if (response.Length > 4000)
                return StudentAssessmentDeliveryResult<StudentAssessmentSubmission>.Failure(
                    StudentAssessmentDeliveryErrorCode.InvalidSubmission);

            var earned = AnswersEquivalent(response, itemMap[question.Id].CorrectAnswer)
                ? question.MaxScore
                : 0m;
            earned = Round(earned);
            score += earned;
            scored.Add((question, response, earned));
        }

        score = Round(score);
        var percentage = decimal.Round(
            score / context.Assessment.MaxScore * 100m,
            2,
            MidpointRounding.AwayFromZero);
        var now = DateTime.UtcNow;

        var result = new AssessmentResult
        {
            Id = Guid.NewGuid(),
            SchoolId = resolved.SchoolId,
            AssessmentId = assessmentId,
            StudentProfileId = resolved.Profile!.Id,
            Score = score,
            Percentage = percentage,
            EnteredByUserId = actorUserId,
            EnteredAtUtc = now,
            UpdatedAtUtc = now
        };
        await assessments.AddAsync(result, cancellationToken);

        foreach (var row in scored)
        {
            await assessments.AddAsync(
                new StudentAnswer
                {
                    Id = Guid.NewGuid(),
                    SchoolId = resolved.SchoolId,
                    AssessmentResultId = result.Id,
                    AssessmentQuestionId = row.Question.Id,
                    ResponseText = row.Response,
                    Score = row.Score,
                    UpdatedAtUtc = now
                },
                cancellationToken);
        }

        var eventId = Guid.NewGuid();
        var changed = new AssessmentResultChangedEvent(
            eventId,
            resolved.SchoolId,
            assessmentId,
            result.Id,
            context.Assessment.ClassGroupId,
            context.Assessment.SubjectId,
            resolved.Profile.Id,
            now);

        await assessments.AddOutboxAsync(
            new OutboxMessage
            {
                Id = eventId,
                SchoolId = resolved.SchoolId,
                EventType = RealtimeEventTypes.AssessmentResultEntered,
                PayloadJson = JsonSerializer.Serialize(changed),
                OccurredAtUtc = now,
                AvailableAtUtc = now,
                ProcessingAttempts = 0,
                CorrelationId = $"assessment-result:{eventId:N}"
            },
            cancellationToken);

        if (audit is not null)
        {
            await audit.QueueAsync(
                new AuditEvent(
                    SchoolId: resolved.SchoolId,
                    Action: "StudentAssessment.Submitted",
                    EntityType: "AssessmentResult",
                    EntityId: result.Id.ToString("D"),
                    Feature: "Assessments",
                    NewValues: new Dictionary<string, object?>
                    {
                        ["assessmentId"] = assessmentId,
                        ["studentProfileId"] = resolved.Profile.Id,
                        ["answerCount"] = scored.Count
                    },
                    ResultSummary: "Online assessment submitted by student.",
                    ActorUserIdOverride: actorUserId,
                    ActorRoleOverride: RoleNames.Student),
                cancellationToken);
        }

        var saved = await assessments.SaveAsync(cancellationToken);
        if (!saved.Succeeded)
            return StudentAssessmentDeliveryResult<StudentAssessmentSubmission>.Failure(
                StudentAssessmentDeliveryErrorCode.PersistenceError);

        return StudentAssessmentDeliveryResult<StudentAssessmentSubmission>.Success(
            new StudentAssessmentSubmission(
                assessmentId,
                context.Assessment.Title,
                score,
                context.Assessment.MaxScore,
                percentage,
                now));
    }

    private async Task<ResolvedDelivery> ResolveAsync(
        Guid actorUserId,
        Guid assessmentId,
        CancellationToken cancellationToken)
    {
        var actor = await users.GetActorAsync(actorUserId, cancellationToken);
        if (actor is null || !actor.IsActive || actor.IsLocked || !actor.SchoolId.HasValue ||
            actor.Roles.Count != 1 || actor.Roles[0] != RoleNames.Student)
            return ResolvedDelivery.Fail(StudentAssessmentDeliveryErrorCode.AccessDenied);

        var school = await schools.GetByIdAsync(actor.SchoolId.Value, cancellationToken);
        if (school is null || school.Status != SchoolStatus.Active)
            return ResolvedDelivery.Fail(StudentAssessmentDeliveryErrorCode.SchoolNotActive);

        var snapshot = await assessments.GetSnapshotAsync(school.Id, cancellationToken);
        var profile = snapshot.StudentProfiles.SingleOrDefault(x =>
            x.UserId == actorUserId &&
            !x.IsArchived &&
            x.Status == AcademicStructureStatus.Active);
        if (profile is null)
            return ResolvedDelivery.Fail(StudentAssessmentDeliveryErrorCode.ProfileNotLinked);

        var assessment = snapshot.Assessments.SingleOrDefault(x => x.Id == assessmentId);
        if (assessment is null)
            return ResolvedDelivery.Fail(StudentAssessmentDeliveryErrorCode.AssessmentNotFound);
        if (assessment.Status != AssessmentStatus.Open)
            return ResolvedDelivery.Fail(StudentAssessmentDeliveryErrorCode.AssessmentNotOpen);
        if (assessment.DeliveryMode != AssessmentDeliveryMode.Online)
            return ResolvedDelivery.Fail(StudentAssessmentDeliveryErrorCode.AssessmentOffline);

        var enrolled = snapshot.StudentEnrollments.Any(x =>
            x.StudentProfileId == profile.Id &&
            x.AcademicYearId == assessment.AcademicYearId &&
            x.ClassGroupId == assessment.ClassGroupId);
        if (!enrolled)
            return ResolvedDelivery.Fail(StudentAssessmentDeliveryErrorCode.NotTargeted);

        if (assessment.TargetType == AssessmentTargetType.Student &&
            assessment.TargetStudentProfileId != profile.Id)
            return ResolvedDelivery.Fail(StudentAssessmentDeliveryErrorCode.NotTargeted);

        return ResolvedDelivery.Ok(school.Id, profile, snapshot);
    }

    private static bool AnswersEquivalent(string actual, string expected)
    {
        actual = actual.Trim();
        expected = (expected ?? string.Empty).Trim();

        if (decimal.TryParse(actual, NumberStyles.Number, CultureInfo.InvariantCulture, out var actualNumber) &&
            decimal.TryParse(expected, NumberStyles.Number, CultureInfo.InvariantCulture, out var expectedNumber))
            return actualNumber == expectedNumber;

        return string.Equals(actual, expected, StringComparison.OrdinalIgnoreCase);
    }

    private static decimal Round(decimal value) =>
        decimal.Round(value, 2, MidpointRounding.AwayFromZero);

    private sealed record ResolvedDelivery(
        Guid SchoolId,
        StudentProfile? Profile,
        AssessmentSnapshot? Snapshot,
        StudentAssessmentDeliveryErrorCode? Error)
    {
        public static ResolvedDelivery Ok(Guid schoolId, StudentProfile profile, AssessmentSnapshot snapshot) =>
            new(schoolId, profile, snapshot, null);

        public static ResolvedDelivery Fail(StudentAssessmentDeliveryErrorCode error) =>
            new(Guid.Empty, null, null, error);
    }
}
