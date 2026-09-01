
using System.Globalization;
using Edulytics.Core.Entities;
using Edulytics.Core.Enums;
using Edulytics.Core.Practice;

namespace Edulytics.Services.Practice;

public sealed class PracticeService(IPracticeRepository repository) : IPracticeService
{
    private const int MaxItemsPerAttempt = 50;

    public async Task<PracticeQueryResult<IReadOnlyList<PracticeItemSummary>>> ListAvailableAsync(
        Guid studentUserId,
        Guid curriculumAdoptionId,
        Guid? lessonId,
        CancellationToken cancellationToken = default)
    {
        var student = await repository.FindStudentByUserIdAsync(studentUserId, cancellationToken);
        if (student is null)
        {
            return PracticeQueryResult<IReadOnlyList<PracticeItemSummary>>.Failure(PracticeErrorCode.StudentNotFound);
        }

        if (!await repository.IsEnrolledInAdoptionAsync(
                student.SchoolId,
                student.Id,
                curriculumAdoptionId,
                cancellationToken))
        {
            return PracticeQueryResult<IReadOnlyList<PracticeItemSummary>>.Failure(PracticeErrorCode.NotEnrolled);
        }

        var items = await repository.ListItemsAsync(student.SchoolId, curriculumAdoptionId, lessonId, cancellationToken);
        var mappings = await repository.GetOutcomeIdsAsync(student.SchoolId, items.Select(x => x.Id).ToArray(), cancellationToken);

        var result = items
            .Where(x => mappings.TryGetValue(x.Id, out var outcomeIds) && outcomeIds.Count > 0)
            .Select(x => new PracticeItemSummary(
                x.Id,
                x.ItemType,
                x.Difficulty,
                x.Prompt,
                x.CurriculumPedagogicalLessonId,
                mappings[x.Id]))
            .ToArray();

        return PracticeQueryResult<IReadOnlyList<PracticeItemSummary>>.Success(result);
    }

    public async Task<PracticeCommandResult> StartAsync(
        Guid studentUserId,
        Guid curriculumAdoptionId,
        IReadOnlyList<Guid> itemIds,
        CancellationToken cancellationToken = default)
    {
        if (itemIds.Count is < 1 or > MaxItemsPerAttempt || itemIds.Distinct().Count() != itemIds.Count)
        {
            return PracticeCommandResult.Failure(PracticeErrorCode.Required);
        }

        var student = await repository.FindStudentByUserIdAsync(studentUserId, cancellationToken);
        if (student is null)
        {
            return PracticeCommandResult.Failure(PracticeErrorCode.StudentNotFound);
        }

        if (!await repository.IsEnrolledInAdoptionAsync(
                student.SchoolId,
                student.Id,
                curriculumAdoptionId,
                cancellationToken))
        {
            return PracticeCommandResult.Failure(PracticeErrorCode.NotEnrolled);
        }

        var items = await repository.GetItemsAsync(student.SchoolId, itemIds, cancellationToken);
        if (items.Count != itemIds.Count)
        {
            return PracticeCommandResult.Failure(PracticeErrorCode.ItemNotFound);
        }

        if (items.Any(x => x.CurriculumAdoptionId != curriculumAdoptionId))
        {
            return PracticeCommandResult.Failure(PracticeErrorCode.ItemScopeMismatch);
        }

        var mappings = await repository.GetOutcomeIdsAsync(student.SchoolId, itemIds, cancellationToken);
        if (items.Any(x => !mappings.TryGetValue(x.Id, out var outcomes) || outcomes.Count == 0))
        {
            return PracticeCommandResult.Failure(PracticeErrorCode.ItemMissingOutcome);
        }

        var allOutcomeIds = mappings.Values.SelectMany(x => x).Distinct().ToArray();
        if (!await repository.OutcomesBelongToAdoptionAsync(
                student.SchoolId,
                curriculumAdoptionId,
                allOutcomeIds,
                cancellationToken))
        {
            return PracticeCommandResult.Failure(PracticeErrorCode.OutcomeScopeMismatch);
        }

        var now = DateTime.UtcNow;
        var attempt = new PracticeAttempt
        {
            Id = Guid.NewGuid(),
            SchoolId = student.SchoolId,
            StudentProfileId = student.Id,
            CurriculumAdoptionId = curriculumAdoptionId,
            CurriculumPedagogicalLessonId = SingleLessonOrNull(items),
            Status = PracticeAttemptStatus.InProgress,
            StartedAtUtc = now,
            Score = 0,
            MaxScore = itemIds.Count,
            Percentage = 0,
            RowVersion = []
        };

        var byId = items.ToDictionary(x => x.Id);
        var attemptItems = itemIds.Select((id, index) => new PracticeAttemptItem
        {
            Id = Guid.NewGuid(),
            SchoolId = student.SchoolId,
            PracticeAttemptId = attempt.Id,
            AssessmentItemId = id,
            Order = index + 1,
            MaxScore = 1m
        }).ToArray();

        var exposures = itemIds.Select(id => new StudentItemExposure
        {
            Id = Guid.NewGuid(),
            SchoolId = student.SchoolId,
            StudentProfileId = student.Id,
            AssessmentItemId = id,
            ExposureFingerprint = byId[id].ExposureFingerprint,
            ExposedAtUtc = now
        }).ToArray();

        await repository.AddAttemptAsync(attempt, attemptItems, exposures, cancellationToken);
        return PracticeCommandResult.Success(attempt.Id);
    }

    public async Task<PracticeQueryResult<PracticeAttemptDetails>> GetAttemptAsync(
        Guid studentUserId,
        Guid attemptId,
        CancellationToken cancellationToken = default)
    {
        var access = await GetOwnedAttemptAsync(studentUserId, attemptId, cancellationToken);
        if (access.Error.HasValue)
        {
            return PracticeQueryResult<PracticeAttemptDetails>.Failure(access.Error.Value);
        }

        return PracticeQueryResult<PracticeAttemptDetails>.Success(
            await BuildDetailsAsync(access.Student!, access.Attempt!, cancellationToken));
    }

    public async Task<PracticeQueryResult<PracticeFeedback>> AnswerAsync(
        Guid studentUserId,
        Guid attemptId,
        Guid attemptItemId,
        string answer,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(answer) || answer.Trim().Length > 2000)
        {
            return PracticeQueryResult<PracticeFeedback>.Failure(PracticeErrorCode.InvalidAnswer);
        }

        var access = await GetOwnedAttemptAsync(studentUserId, attemptId, cancellationToken);
        if (access.Error.HasValue)
        {
            return PracticeQueryResult<PracticeFeedback>.Failure(access.Error.Value);
        }

        var attempt = access.Attempt!;
        if (attempt.Status != PracticeAttemptStatus.InProgress)
        {
            return PracticeQueryResult<PracticeFeedback>.Failure(PracticeErrorCode.AttemptNotInProgress);
        }

        var attemptItems = await repository.GetAttemptItemsAsync(attempt.SchoolId, attempt.Id, cancellationToken);
        var attemptItem = attemptItems.SingleOrDefault(x => x.Id == attemptItemId);
        if (attemptItem is null)
        {
            return PracticeQueryResult<PracticeFeedback>.Failure(PracticeErrorCode.ResponseItemMismatch);
        }

        var itemList = await repository.GetItemsAsync(attempt.SchoolId, [attemptItem.AssessmentItemId], cancellationToken);
        var item = itemList.SingleOrDefault();
        if (item is null)
        {
            return PracticeQueryResult<PracticeFeedback>.Failure(PracticeErrorCode.ItemNotFound);
        }

        var correct = AnswersMatch(answer, item.CorrectAnswer);
        var score = correct ? attemptItem.MaxScore : 0m;
        var response = new PracticeResponse
        {
            Id = Guid.NewGuid(),
            SchoolId = attempt.SchoolId,
            PracticeAttemptItemId = attemptItem.Id,
            Answer = answer.Trim(),
            IsCorrect = correct,
            Score = score,
            Feedback = item.Solution,
            AnsweredAtUtc = DateTime.UtcNow
        };

        await repository.SaveResponseAsync(response, cancellationToken);
        return PracticeQueryResult<PracticeFeedback>.Success(
            new PracticeFeedback(attemptItem.Id, correct, score, item.Solution));
    }

    public async Task<PracticeQueryResult<PracticeAttemptDetails>> SubmitAsync(
        Guid studentUserId,
        Guid attemptId,
        CancellationToken cancellationToken = default)
    {
        var access = await GetOwnedAttemptAsync(studentUserId, attemptId, cancellationToken);
        if (access.Error.HasValue)
        {
            return PracticeQueryResult<PracticeAttemptDetails>.Failure(access.Error.Value);
        }

        var attempt = access.Attempt!;
        if (attempt.Status == PracticeAttemptStatus.Submitted)
        {
            return PracticeQueryResult<PracticeAttemptDetails>.Success(
                await BuildDetailsAsync(access.Student!, attempt, cancellationToken));
        }

        var attemptItems = await repository.GetAttemptItemsAsync(attempt.SchoolId, attempt.Id, cancellationToken);
        var responses = await repository.GetResponsesAsync(attempt.SchoolId, attempt.Id, cancellationToken);
        if (attemptItems.Count == 0 || responses.Count != attemptItems.Count)
        {
            return PracticeQueryResult<PracticeAttemptDetails>.Failure(PracticeErrorCode.AttemptIncomplete);
        }

        var items = await repository.GetItemsAsync(
            attempt.SchoolId,
            attemptItems.Select(x => x.AssessmentItemId).ToArray(),
            cancellationToken);
        var itemById = items.ToDictionary(x => x.Id);
        var mappings = await repository.GetOutcomeIdsAsync(
            attempt.SchoolId,
            items.Select(x => x.Id).ToArray(),
            cancellationToken);
        var responseByAttemptItemId = responses.ToDictionary(x => x.PracticeAttemptItemId);

        var now = DateTime.UtcNow;
        var evidence = new List<LearningEvidence>();
        foreach (var attemptItem in attemptItems)
        {
            var item = itemById[attemptItem.AssessmentItemId];
            var response = responseByAttemptItemId[attemptItem.Id];
            foreach (var outcomeId in mappings[item.Id])
            {
                evidence.Add(new LearningEvidence
                {
                    Id = Guid.NewGuid(),
                    SchoolId = attempt.SchoolId,
                    StudentProfileId = attempt.StudentProfileId,
                    LearningOutcomeId = outcomeId,
                    PracticeAttemptId = attempt.Id,
                    AssessmentItemId = item.Id,
                    EvidenceType = LearningEvidenceType.Practice,
                    Difficulty = item.Difficulty,
                    IsCorrect = response.IsCorrect,
                    Score = response.Score,
                    MaxScore = attemptItem.MaxScore,
                    OccurredAtUtc = now
                });
            }
        }

        attempt.Score = responses.Sum(x => x.Score);
        attempt.MaxScore = attemptItems.Sum(x => x.MaxScore);
        attempt.Percentage = attempt.MaxScore == 0m
            ? 0m
            : Math.Round(attempt.Score / attempt.MaxScore * 100m, 2, MidpointRounding.AwayFromZero);
        attempt.Status = PracticeAttemptStatus.Submitted;
        attempt.SubmittedAtUtc = now;

        await repository.CompleteAttemptAsync(attempt, evidence, cancellationToken);
        return PracticeQueryResult<PracticeAttemptDetails>.Success(
            await BuildDetailsAsync(access.Student!, attempt, cancellationToken));
    }

    private async Task<PracticeAttemptDetails> BuildDetailsAsync(
        StudentProfile student,
        PracticeAttempt attempt,
        CancellationToken cancellationToken)
    {
        var attemptItems = await repository.GetAttemptItemsAsync(attempt.SchoolId, attempt.Id, cancellationToken);
        var items = await repository.GetItemsAsync(
            attempt.SchoolId,
            attemptItems.Select(x => x.AssessmentItemId).ToArray(),
            cancellationToken);
        var itemById = items.ToDictionary(x => x.Id);
        var responses = await repository.GetResponsesAsync(attempt.SchoolId, attempt.Id, cancellationToken);
        var responseByAttemptItem = responses.ToDictionary(x => x.PracticeAttemptItemId);

        var questions = attemptItems.Select(link =>
        {
            var item = itemById[link.AssessmentItemId];
            responseByAttemptItem.TryGetValue(link.Id, out var response);
            return new PracticeAttemptQuestion(
                link.Id,
                item.Id,
                link.Order,
                item.ItemType,
                item.Difficulty,
                item.Prompt,
                response is not null,
                response?.IsCorrect,
                response?.Feedback);
        }).ToArray();

        return new PracticeAttemptDetails(
            attempt.Id,
            attempt.Status,
            attempt.StartedAtUtc,
            attempt.SubmittedAtUtc,
            attempt.Score,
            attempt.MaxScore,
            attempt.Percentage,
            questions);
    }

    private async Task<(StudentProfile? Student, PracticeAttempt? Attempt, PracticeErrorCode? Error)> GetOwnedAttemptAsync(
        Guid studentUserId,
        Guid attemptId,
        CancellationToken cancellationToken)
    {
        var student = await repository.FindStudentByUserIdAsync(studentUserId, cancellationToken);
        if (student is null)
        {
            return (null, null, PracticeErrorCode.StudentNotFound);
        }

        var attempt = await repository.GetAttemptAsync(student.SchoolId, attemptId, cancellationToken);
        if (attempt is null)
        {
            return (student, null, PracticeErrorCode.AttemptNotFound);
        }

        if (attempt.StudentProfileId != student.Id)
        {
            return (student, null, PracticeErrorCode.AccessDenied);
        }

        return (student, attempt, null);
    }

    private static Guid? SingleLessonOrNull(IReadOnlyList<AssessmentItem> items)
    {
        var values = items.Select(x => x.CurriculumPedagogicalLessonId).Distinct().ToArray();
        return values.Length == 1 ? values[0] : null;
    }

    public static bool AnswersMatch(string submitted, string expected)
    {
        var left = submitted.Trim();
        var right = expected.Trim();
        if (string.Equals(left, right, StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        static bool TryNumber(string value, out decimal number) =>
            decimal.TryParse(
                value.Replace(',', '.'),
                NumberStyles.Number | NumberStyles.AllowLeadingSign,
                CultureInfo.InvariantCulture,
                out number);

        return TryNumber(left, out var leftNumber) &&
               TryNumber(right, out var rightNumber) &&
               leftNumber == rightNumber;
    }
}
