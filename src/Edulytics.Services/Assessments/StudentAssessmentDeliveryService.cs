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
        var now = DateTime.UtcNow;
        var resolved = await ResolveAsync(actorUserId, assessmentId, now, cancellationToken);
        if (resolved.Error.HasValue)
            return StudentAssessmentDeliveryResult<StudentAssessmentAttempt>.Failure(resolved.Error.Value);

        var assessment = resolved.Assessment!;
        var existingAttempt = await assessments.GetAttemptAsync(
            resolved.SchoolId,
            assessmentId,
            resolved.Profile!.Id,
            cancellationToken);

        if (assessment.AssessmentType == AssessmentType.Exam &&
            resolved.Snapshot!.Results.Any(x =>
                x.AssessmentId == assessmentId &&
                x.StudentProfileId == resolved.Profile.Id))
        {
            return StudentAssessmentDeliveryResult<StudentAssessmentAttempt>.Failure(
                StudentAssessmentDeliveryErrorCode.AlreadySubmitted);
        }

        if (assessment.AssessmentType == AssessmentType.Homework &&
            existingAttempt?.Status == AssessmentAttemptStatus.Submitted)
        {
            return StudentAssessmentDeliveryResult<StudentAssessmentAttempt>.Failure(
                StudentAssessmentDeliveryErrorCode.AlreadySubmitted);
        }

        var attempt = existingAttempt ?? await assessments.GetOrCreateAttemptAsync(
            resolved.SchoolId,
            assessmentId,
            resolved.Profile.Id,
            now,
            cancellationToken);
        if (attempt is null)
            return StudentAssessmentDeliveryResult<StudentAssessmentAttempt>.Failure(
                StudentAssessmentDeliveryErrorCode.PersistenceError);

        var effectiveDeadline = ResolveEffectiveAttemptDeadline(assessment, attempt);
        if (assessment.AssessmentType == AssessmentType.Exam &&
            effectiveDeadline.HasValue &&
            now >= effectiveDeadline.Value)
        {
            return StudentAssessmentDeliveryResult<StudentAssessmentAttempt>.Failure(
                StudentAssessmentDeliveryErrorCode.AttemptExpired);
        }

        var context = await builder.GetContextAsync(resolved.SchoolId, assessmentId, cancellationToken);
        if (context is null)
            return StudentAssessmentDeliveryResult<StudentAssessmentAttempt>.Failure(
                StudentAssessmentDeliveryErrorCode.AssessmentNotFound);

        var itemMap = context.Items.ToDictionary(x => x.Id);
        if (context.Questions.Count == 0 || context.Questions.Any(x => !itemMap.ContainsKey(x.Id)))
            return StudentAssessmentDeliveryResult<StudentAssessmentAttempt>.Failure(
                StudentAssessmentDeliveryErrorCode.AssessmentNotFound);

        var existingResponses = assessment.AssessmentType == AssessmentType.Exam
            ? new Dictionary<Guid, string>()
            : (await assessments.ListTaskResponsesAsync(
                    resolved.SchoolId,
                    attempt.Id,
                    cancellationToken))
                .ToDictionary(x => x.AssessmentQuestionId, x => x.ResponseText);

        return StudentAssessmentDeliveryResult<StudentAssessmentAttempt>.Success(
            new StudentAssessmentAttempt(
                context.Assessment.Id,
                context.Assessment.Title,
                context.Assessment.AssessmentDate,
                context.Assessment.MaxScore,
                context.Assessment.DifficultyBand,
                context.Questions
                    .OrderBy(x => x.Order)
                    .Select(x =>
                    {
                        var item = itemMap[x.Id];
                        return new StudentAssessmentQuestion(x.Id, x.Order, x.Prompt, x.MaxScore)
                        {
                            ItemType = item.ItemType,
                            Choices = ReadChoices(item),
                            CurrentResponse = existingResponses.GetValueOrDefault(x.Id, string.Empty)
                        };
                    })
                    .ToArray())
            {
                AssessmentType = context.Assessment.AssessmentType,
                AvailableFromUtc = context.Assessment.AvailableFromUtc,
                DueAtUtc = context.Assessment.DueAtUtc,
                AttemptExpiresAtUtc = context.Assessment.AssessmentType == AssessmentType.Exam
                    ? effectiveDeadline
                    : null
            });
    }

    public async Task<StudentAssessmentDeliveryResult<StudentAssessmentSubmission>> SubmitAsync(
        Guid actorUserId,
        Guid assessmentId,
        IReadOnlyList<StudentAssessmentResponse> responses,
        CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;
        var resolved = await ResolveAsync(actorUserId, assessmentId, now, cancellationToken);
        if (resolved.Error.HasValue)
            return StudentAssessmentDeliveryResult<StudentAssessmentSubmission>.Failure(resolved.Error.Value);

        var assessment = resolved.Assessment!;
        var profile = resolved.Profile!;

        if (assessment.AssessmentType == AssessmentType.Exam &&
            resolved.Snapshot!.Results.Any(x =>
                x.AssessmentId == assessmentId &&
                x.StudentProfileId == profile.Id))
        {
            return StudentAssessmentDeliveryResult<StudentAssessmentSubmission>.Failure(
                StudentAssessmentDeliveryErrorCode.AlreadySubmitted);
        }

        var attempt = await assessments.GetOrCreateAttemptAsync(
            resolved.SchoolId,
            assessmentId,
            profile.Id,
            now,
            cancellationToken);
        if (attempt is null)
            return StudentAssessmentDeliveryResult<StudentAssessmentSubmission>.Failure(
                StudentAssessmentDeliveryErrorCode.PersistenceError);

        if (assessment.AssessmentType == AssessmentType.Homework &&
            attempt.Status == AssessmentAttemptStatus.Submitted)
        {
            return StudentAssessmentDeliveryResult<StudentAssessmentSubmission>.Failure(
                StudentAssessmentDeliveryErrorCode.AlreadySubmitted);
        }

        var effectiveDeadline = ResolveEffectiveAttemptDeadline(assessment, attempt);
        if (assessment.AssessmentType == AssessmentType.Exam &&
            effectiveDeadline.HasValue &&
            now >= effectiveDeadline.Value)
        {
            return StudentAssessmentDeliveryResult<StudentAssessmentSubmission>.Failure(
                StudentAssessmentDeliveryErrorCode.AttemptExpired);
        }

        var context = await builder.GetContextAsync(resolved.SchoolId, assessmentId, cancellationToken);
        if (context is null)
            return StudentAssessmentDeliveryResult<StudentAssessmentSubmission>.Failure(
                StudentAssessmentDeliveryErrorCode.AssessmentNotFound);

        var questions = context.Questions.OrderBy(x => x.Order).ToArray();
        if (questions.Length == 0 ||
            responses.Count != questions.Length ||
            responses.Select(x => x.QuestionId).Distinct().Count() != questions.Length)
        {
            return StudentAssessmentDeliveryResult<StudentAssessmentSubmission>.Failure(
                StudentAssessmentDeliveryErrorCode.InvalidSubmission);
        }

        var responseMap = responses.ToDictionary(x => x.QuestionId);
        var itemMap = context.Items.ToDictionary(x => x.Id);
        if (questions.Any(x => !responseMap.ContainsKey(x.Id) || !itemMap.ContainsKey(x.Id)))
        {
            return StudentAssessmentDeliveryResult<StudentAssessmentSubmission>.Failure(
                StudentAssessmentDeliveryErrorCode.InvalidSubmission);
        }

        var normalizedResponses = new List<(AssessmentQuestion Question, AssessmentItem Item, string Response)>();
        foreach (var question in questions)
        {
            var response = (responseMap[question.Id].ResponseText ?? string.Empty).Trim();
            if (response.Length > 4000)
                return StudentAssessmentDeliveryResult<StudentAssessmentSubmission>.Failure(
                    StudentAssessmentDeliveryErrorCode.InvalidSubmission);

            var item = itemMap[question.Id];
            var choices = ReadChoices(item);
            if (item.ItemType == AssessmentItemType.MultipleChoice &&
                choices.Count > 0 &&
                response.Length > 0 &&
                !choices.Contains(response, StringComparer.Ordinal))
            {
                return StudentAssessmentDeliveryResult<StudentAssessmentSubmission>.Failure(
                    StudentAssessmentDeliveryErrorCode.InvalidSubmission);
            }

            normalizedResponses.Add((question, item, response));
        }

        return assessment.AssessmentType == AssessmentType.Exam
            ? await SubmitExamAsync(
                actorUserId,
                resolved,
                attempt,
                normalizedResponses,
                now,
                cancellationToken)
            : await SubmitLearningTaskAsync(
                actorUserId,
                resolved,
                attempt,
                normalizedResponses,
                now,
                cancellationToken);
    }

    private async Task<StudentAssessmentDeliveryResult<StudentAssessmentSubmission>> SubmitExamAsync(
        Guid actorUserId,
        ResolvedDelivery resolved,
        AssessmentAttempt attempt,
        IReadOnlyList<(AssessmentQuestion Question, AssessmentItem Item, string Response)> rows,
        DateTime now,
        CancellationToken cancellationToken)
    {
        var assessment = resolved.Assessment!;
        decimal score = 0m;
        var scored = new List<(AssessmentQuestion Question, string Response, decimal Score)>();

        foreach (var row in rows)
        {
            var earned = MathematicsAnswerEquivalence.AreEquivalent(
                row.Response,
                row.Item.CorrectAnswer)
                ? row.Question.MaxScore
                : 0m;
            earned = Round(earned);
            score += earned;
            scored.Add((row.Question, row.Response, earned));
        }

        score = Round(score);
        var percentage = decimal.Round(
            score / assessment.MaxScore * 100m,
            2,
            MidpointRounding.AwayFromZero);

        var result = new AssessmentResult
        {
            Id = Guid.NewGuid(),
            SchoolId = resolved.SchoolId,
            AssessmentId = assessment.Id,
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

        attempt.Status = AssessmentAttemptStatus.Submitted;
        attempt.SubmittedAtUtc = now;
        attempt.UpdatedAtUtc = now;

        var eventId = Guid.NewGuid();
        var changed = new AssessmentResultChangedEvent(
            eventId,
            resolved.SchoolId,
            assessment.Id,
            result.Id,
            assessment.ClassGroupId,
            assessment.SubjectId,
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

        await QueueStudentAuditAsync(
            actorUserId,
            resolved,
            "StudentAssessment.Submitted",
            "AssessmentResult",
            result.Id,
            scored.Count,
            "Online exam/test submitted by student.",
            cancellationToken);

        var saved = await assessments.SaveAsync(cancellationToken);
        if (!saved.Succeeded)
            return StudentAssessmentDeliveryResult<StudentAssessmentSubmission>.Failure(
                StudentAssessmentDeliveryErrorCode.PersistenceError);

        return StudentAssessmentDeliveryResult<StudentAssessmentSubmission>.Success(
            new StudentAssessmentSubmission(
                assessment.Id,
                assessment.Title,
                score,
                assessment.MaxScore,
                percentage,
                now)
            {
                AssessmentType = AssessmentType.Exam
            });
    }

    private async Task<StudentAssessmentDeliveryResult<StudentAssessmentSubmission>> SubmitLearningTaskAsync(
        Guid actorUserId,
        ResolvedDelivery resolved,
        AssessmentAttempt attempt,
        IReadOnlyList<(AssessmentQuestion Question, AssessmentItem Item, string Response)> rows,
        DateTime now,
        CancellationToken cancellationToken)
    {
        var assessment = resolved.Assessment!;
        if (assessment.AssessmentType is not (AssessmentType.Homework or AssessmentType.Worksheet))
        {
            return StudentAssessmentDeliveryResult<StudentAssessmentSubmission>.Failure(
                StudentAssessmentDeliveryErrorCode.InvalidSubmission);
        }

        foreach (var row in rows)
        {
            var response = await assessments.GetTaskResponseAsync(
                resolved.SchoolId,
                attempt.Id,
                row.Question.Id,
                cancellationToken);

            if (response is null)
            {
                await assessments.AddAsync(
                    new AssessmentTaskResponse
                    {
                        Id = Guid.NewGuid(),
                        SchoolId = resolved.SchoolId,
                        AssessmentAttemptId = attempt.Id,
                        AssessmentQuestionId = row.Question.Id,
                        ResponseText = row.Response,
                        UpdatedAtUtc = now
                    },
                    cancellationToken);
            }
            else
            {
                response.ResponseText = row.Response;
                response.UpdatedAtUtc = now;
            }
        }

        if (assessment.AssessmentType == AssessmentType.Homework)
        {
            attempt.Status = AssessmentAttemptStatus.Submitted;
            attempt.SubmittedAtUtc = now;
        }
        else
        {
            attempt.Status = AssessmentAttemptStatus.Completed;
            attempt.CompletedAtUtc = now;
        }

        attempt.UpdatedAtUtc = now;

        await QueueStudentAuditAsync(
            actorUserId,
            resolved,
            assessment.AssessmentType == AssessmentType.Homework
                ? "StudentHomework.Submitted"
                : "StudentWorksheet.Completed",
            "AssessmentAttempt",
            attempt.Id,
            rows.Count,
            assessment.AssessmentType == AssessmentType.Homework
                ? "Homework submitted without numeric grading."
                : "Worksheet completed without numeric grading.",
            cancellationToken);

        var saved = await assessments.SaveAsync(cancellationToken);
        if (!saved.Succeeded)
            return StudentAssessmentDeliveryResult<StudentAssessmentSubmission>.Failure(
                StudentAssessmentDeliveryErrorCode.PersistenceError);

        return StudentAssessmentDeliveryResult<StudentAssessmentSubmission>.Success(
            new StudentAssessmentSubmission(
                assessment.Id,
                assessment.Title,
                0m,
                0m,
                0m,
                now)
            {
                AssessmentType = assessment.AssessmentType
            });
    }

    private async Task<ResolvedDelivery> ResolveAsync(
        Guid actorUserId,
        Guid assessmentId,
        DateTime nowUtc,
        CancellationToken cancellationToken)
    {
        var actor = await users.GetActorAsync(actorUserId, cancellationToken);
        if (actor is null || !actor.IsActive || actor.IsLocked || !actor.SchoolId.HasValue ||
            actor.Roles.Count != 1 || actor.Roles[0] != RoleNames.Student)
        {
            return ResolvedDelivery.Fail(StudentAssessmentDeliveryErrorCode.AccessDenied);
        }

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
        {
            return ResolvedDelivery.Fail(StudentAssessmentDeliveryErrorCode.NotTargeted);
        }

        if (assessment.AssessmentType == AssessmentType.Exam)
        {
            if (assessment.AvailableFromUtc.HasValue &&
                nowUtc < assessment.AvailableFromUtc.Value)
            {
                return ResolvedDelivery.Fail(StudentAssessmentDeliveryErrorCode.NotYetAvailable);
            }

            if (assessment.DueAtUtc.HasValue &&
                nowUtc >= assessment.DueAtUtc.Value)
            {
                return ResolvedDelivery.Fail(StudentAssessmentDeliveryErrorCode.DeadlinePassed);
            }
        }
        else if (assessment.AssessmentType == AssessmentType.Homework)
        {
            if (!assessment.DueAtUtc.HasValue ||
                nowUtc >= assessment.DueAtUtc.Value)
            {
                return ResolvedDelivery.Fail(StudentAssessmentDeliveryErrorCode.DeadlinePassed);
            }
        }

        return ResolvedDelivery.Ok(school.Id, profile, assessment, snapshot);
    }

    private static DateTime? ResolveEffectiveAttemptDeadline(
        Assessment assessment,
        AssessmentAttempt attempt)
    {
        if (assessment.AssessmentType != AssessmentType.Exam)
            return null;

        DateTime? deadline = assessment.DueAtUtc;

        if (assessment.AttemptTimeLimitMinutes.HasValue)
        {
            var personal = attempt.StartedAtUtc.AddMinutes(
                assessment.AttemptTimeLimitMinutes.Value);
            deadline = !deadline.HasValue || personal < deadline.Value
                ? personal
                : deadline;
        }

        return deadline;
    }

    private async Task QueueStudentAuditAsync(
        Guid actorUserId,
        ResolvedDelivery resolved,
        string action,
        string entityType,
        Guid entityId,
        int answerCount,
        string summary,
        CancellationToken cancellationToken)
    {
        if (audit is null)
            return;

        await audit.QueueAsync(
            new AuditEvent(
                SchoolId: resolved.SchoolId,
                Action: action,
                EntityType: entityType,
                EntityId: entityId.ToString("D"),
                Feature: "Assessments",
                NewValues: new Dictionary<string, object?>
                {
                    ["assessmentId"] = resolved.Assessment!.Id,
                    ["assessmentType"] = resolved.Assessment.AssessmentType.ToString(),
                    ["studentProfileId"] = resolved.Profile!.Id,
                    ["answerCount"] = answerCount
                },
                ResultSummary: summary,
                ActorUserIdOverride: actorUserId,
                ActorRoleOverride: RoleNames.Student),
            cancellationToken);
    }

    private static IReadOnlyList<string> ReadChoices(AssessmentItem item)
    {
        if (item.ItemType != AssessmentItemType.MultipleChoice ||
            string.IsNullOrWhiteSpace(item.ValidationMetadataJson))
        {
            return [];
        }

        try
        {
            using var document = JsonDocument.Parse(item.ValidationMetadataJson);
            if (!document.RootElement.TryGetProperty("choices", out var choices) ||
                choices.ValueKind != JsonValueKind.Array)
            {
                return [];
            }

            return choices
                .EnumerateArray()
                .Where(x => x.ValueKind == JsonValueKind.String)
                .Select(x => x.GetString()?.Trim() ?? string.Empty)
                .Where(x => x.Length > 0)
                .Distinct(StringComparer.Ordinal)
                .Take(12)
                .ToArray();
        }
        catch (JsonException)
        {
            return [];
        }
    }

    private static decimal Round(decimal value) =>
        decimal.Round(value, 2, MidpointRounding.AwayFromZero);

    private sealed record ResolvedDelivery(
        Guid SchoolId,
        StudentProfile? Profile,
        Assessment? Assessment,
        AssessmentSnapshot? Snapshot,
        StudentAssessmentDeliveryErrorCode? Error)
    {
        public static ResolvedDelivery Ok(
            Guid schoolId,
            StudentProfile profile,
            Assessment assessment,
            AssessmentSnapshot snapshot) =>
            new(schoolId, profile, assessment, snapshot, null);

        public static ResolvedDelivery Fail(StudentAssessmentDeliveryErrorCode error) =>
            new(Guid.Empty, null, null, null, error);
    }
}
