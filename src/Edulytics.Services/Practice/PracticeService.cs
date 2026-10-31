
using System.Globalization;
using System.Text.Json;
using Edulytics.Core.Entities;
using Edulytics.Core.Enums;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Core.Practice;
using Edulytics.Services.Assessments;
using Edulytics.Services.Mathematics;

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

        // A constructed graph is a typed count vector, never a scalar.
        // Do not accept ambiguous locale/grouping numeric equivalence.
        var correct = string.Equals(item.GenerationFamily,
                UsCcssT001ConstructShapeGraph.Family, StringComparison.Ordinal)
            ? string.Equals(answer.Trim(), item.CorrectAnswer, StringComparison.Ordinal)
             : UsCcssG1TeenFiveQuestions.Supports(item.GenerationFamily)
                ? UsCcssG1TeenFiveQuestions.GradePersistedAnswer(
                    item.GenerationFamily,item.CorrectAnswer,answer)
             : UsCcssG1SourceFiveNextQuestions.Supports(item.GenerationFamily)
                ? UsCcssG1SourceFiveNextQuestions.GradePersistedAnswer(
                    item.GenerationFamily,item.CorrectAnswer,answer)
             : UsCcssG1NextSourceFiveQuestions.Supports(item.GenerationFamily)
                ? UsCcssG1NextSourceFiveQuestions.GradePersistedAnswer(
                    item.GenerationFamily,item.CorrectAnswer,answer)
             : UsCcssG1Unit2EquationStoryFiveQuestions.Supports(item.GenerationFamily)
                ? UsCcssG1Unit2EquationStoryFiveQuestions.GradePersistedAnswer(
                    item.GenerationFamily,item.CorrectAnswer,answer)
             : UsCcssG1Unit2CompareFiveQuestions.Supports(item.GenerationFamily)
                ? UsCcssG1Unit2CompareFiveQuestions.GradePersistedAnswer(
                    item.GenerationFamily,item.CorrectAnswer,answer)
             : UsCcssG1Unit2NextFiveQuestions.Supports(item.GenerationFamily)
                ? UsCcssG1Unit2NextFiveQuestions.GradePersistedAnswer(
                    item.GenerationFamily,item.CorrectAnswer,answer)
             : UsCcssG1Unit2Part2Questions.IsStoryFamily(item.GenerationFamily)
                ? UsCcssG1Unit2Part2Questions.GradePersistedStoryAnswer(
                    item.GenerationFamily, item.CorrectAnswer, answer)
             : UsCcssG1Unit2Part2Questions.IsAuthoredFamily(item.GenerationFamily)
                ? UsCcssG1Unit2Part2Questions.GradePersistedAuthoredAnswer(
                    item.GenerationFamily, item.CorrectAnswer, answer)
                : UsCcssG1Unit2StoryQuestions.IsAuthoredFamily(item.GenerationFamily)
                ? UsCcssG1Unit2StoryQuestions.GradePersistedAuthoredAnswer(
                    item.GenerationFamily, item.CorrectAnswer, answer)
                : UsCcssT001AuthoredActivities.Supports(item.GenerationFamily)
                ? UsCcssT001AuthoredActivities.GradePersistedAnswer(
                    item.GenerationFamily, item.CorrectAnswer, answer,
                    item.GenerationParametersJson)
                : string.Equals(item.GenerationFamily,
                    "usccss.alg1.u01.l01.statistical_questions_p3",
                    StringComparison.Ordinal)
                ? GradeAlg1P3SelectionAnswer(item.CorrectAnswer, answer)
                : UsCcssAlg2U01L01PracticeAdapter.SupportsFamily(item.GenerationFamily)
                ? GradeAlg2L01PersistedAnswer(item, attempt, answer)
                : AnswersMatch(answer, item.CorrectAnswer);
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

        var now = DateTime.UtcNow;
        IReadOnlyList<LearningEvidence> evidence = [];

        // Student private AI practice is deliberately separated from official
        // assessment/mastery evidence. It can be generated from official outcomes
        // or pedagogical context, but finishing it must never alter official mastery.
        if (!attempt.IsPrivate)
        {
            var items = await repository.GetItemsAsync(
                attempt.SchoolId,
                attemptItems.Select(x => x.AssessmentItemId).ToArray(),
                cancellationToken);
            if (items.Count != attemptItems.Count)
                return PracticeQueryResult<PracticeAttemptDetails>.Failure(PracticeErrorCode.ItemNotFound);

            var itemById = items.ToDictionary(x => x.Id);
            var mappings = await repository.GetOutcomeIdsAsync(
                attempt.SchoolId,
                items.Select(x => x.Id).ToArray(),
                cancellationToken);
            var responseByAttemptItemId = responses.ToDictionary(x => x.PracticeAttemptItemId);
            var officialEvidence = new List<LearningEvidence>();

            foreach (var attemptItem in attemptItems)
            {
                if (!itemById.TryGetValue(attemptItem.AssessmentItemId, out var item))
                    return PracticeQueryResult<PracticeAttemptDetails>.Failure(PracticeErrorCode.ItemNotFound);
                if (!responseByAttemptItemId.TryGetValue(attemptItem.Id, out var response))
                    return PracticeQueryResult<PracticeAttemptDetails>.Failure(PracticeErrorCode.AttemptIncomplete);
                if (!mappings.TryGetValue(item.Id, out var outcomeIds) || outcomeIds.Count == 0)
                    return PracticeQueryResult<PracticeAttemptDetails>.Failure(PracticeErrorCode.ItemMissingOutcome);

                foreach (var outcomeId in outcomeIds)
                {
                    officialEvidence.Add(new LearningEvidence
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

            evidence = officialEvidence;
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
                item.GenerationFamily,
                item.GenerationParametersJson,
                response is not null,
                response?.IsCorrect,
                response?.Feedback)
            {
                SubmittedAnswer = response?.Answer
            };
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

    // Preserve ALG1 P3's released unordered {A,C,E} contract from PR #589.
    // Its course-specific class is absent from this divergent ALG2 branch;
    // reproduce only its exact persisted-set acceptance rule here, without
    // a missing assembly reference or any global set-comparison change.
    private static bool GradeAlg1P3SelectionAnswer(string? expected, string? answer) =>
        TryNormalizeAlg1P3Choices(expected, out var expectedSet) &&
        string.Equals(expectedSet, "ACE", StringComparison.Ordinal) &&
        TryNormalizeAlg1P3Choices(answer, out var answerSet) &&
        string.Equals(answerSet, expectedSet, StringComparison.Ordinal);

    private static bool TryNormalizeAlg1P3Choices(
        string? response, out string normalized)
    {
        normalized = "";
        if (string.IsNullOrWhiteSpace(response))
            return false;
        var tokens = response.Split(',', StringSplitOptions.None);
        if (tokens.Length != 3)
            return false;
        var choices = new HashSet<char>();
        foreach (var token in tokens)
        {
            var candidate = token.Trim().ToUpperInvariant();
            if (candidate.Length != 1 || candidate[0] is < 'A' or > 'F' ||
                !choices.Add(candidate[0]))
                return false;
        }
        normalized = new string(choices.OrderBy(x => x).ToArray());
        return true;
    }

    // Issue #605: ALG2 U01:L01 ONLY. The generic scalar comparator cannot
    // assess exact Hanoi powers or checker expressions. Recreate the issued
    // canonical answer from trusted, bounded generation parameters; never use
    // the learner response or stored answer as the source of mathematical truth.
    // An invalid/foreign/teacher-only item receives no permissive fallback.
    internal static bool GradeAlg2L01PersistedAnswer(
        AssessmentItem item, PracticeAttempt attempt, string submittedAnswer)
    {
        if (!UsCcssAlg2U01L01PracticeAdapter.SupportsFamily(item.GenerationFamily) ||
            !attempt.IsPrivate ||
            attempt.CurriculumPedagogicalLessonId is not Guid issuedLessonId ||
            item.CurriculumPedagogicalLessonId != issuedLessonId ||
            item.SchoolId != attempt.SchoolId ||
            item.CurriculumAdoptionId != attempt.CurriculumAdoptionId ||
            item.Source != AssessmentItemSource.SystemGenerated ||
            item.ItemType != AssessmentItemType.ShortAnswer ||
            !string.Equals(item.GenerationMethod,
                Stage18PracticeSkillContracts.GenerationMethod, StringComparison.Ordinal) ||
            string.IsNullOrWhiteSpace(submittedAnswer) ||
            submittedAnswer.Length > 2000 ||
            string.IsNullOrWhiteSpace(item.GenerationParametersJson) ||
            item.GenerationParametersJson.Length > 4096 ||
            string.IsNullOrWhiteSpace(item.ValidationMetadataJson) ||
            item.ValidationMetadataJson.Length > 4096)
            return false;

        try
        {
            using var issued = JsonDocument.Parse(
                item.GenerationParametersJson, new JsonDocumentOptions { MaxDepth = 12 });
            using var attestation = JsonDocument.Parse(
                item.ValidationMetadataJson, new JsonDocumentOptions { MaxDepth = 12 });
            var envelope = issued.RootElement;
            var metadata = attestation.RootElement;
            const string skillId = "sequences.core.recursive_nth_term";
            if (envelope.ValueKind != JsonValueKind.Object ||
                metadata.ValueKind != JsonValueKind.Object ||
                envelope.EnumerateObject().Count() != 4 ||
                !envelope.TryGetProperty("skillId", out var storedSkill) ||
                storedSkill.ValueKind != JsonValueKind.String ||
                storedSkill.GetString() != skillId ||
                !envelope.TryGetProperty("questionFamily", out var storedFamily) ||
                storedFamily.ValueKind != JsonValueKind.String ||
                storedFamily.GetString() != item.GenerationFamily ||
                !envelope.TryGetProperty("questionVariant", out var displayedVariant) ||
                displayedVariant.ValueKind != JsonValueKind.String ||
                string.IsNullOrWhiteSpace(displayedVariant.GetString()) ||
                !envelope.TryGetProperty("parameters", out var parameters) ||
                parameters.ValueKind != JsonValueKind.Object ||
                parameters.EnumerateObject().Count() != 2 ||
                !parameters.TryGetProperty("n", out var term) ||
                term.ValueKind != JsonValueKind.Number ||
                !term.TryGetInt32(out int n) ||
                !parameters.TryGetProperty("variant", out var variant) ||
                variant.ValueKind != JsonValueKind.Number ||
                !variant.TryGetInt32(out int variantValue) ||
                !metadata.TryGetProperty("stage", out var stage) ||
                stage.ValueKind != JsonValueKind.Number ||
                !stage.TryGetInt32(out int stageNumber) || stageNumber != 18 ||
                !metadata.TryGetProperty("readiness", out var readiness) ||
                readiness.ValueKind != JsonValueKind.String ||
                readiness.GetString() != "READY_VERIFIED" ||
                !metadata.TryGetProperty("alignment", out var alignment) ||
                alignment.ValueKind != JsonValueKind.String ||
                alignment.GetString() != "skill-contract-verified" ||
                !metadata.TryGetProperty("skillContract", out var alignedSkill) ||
                alignedSkill.ValueKind != JsonValueKind.String ||
                alignedSkill.GetString() != skillId ||
                !metadata.TryGetProperty("allowedFamily", out var alignedFamily) ||
                alignedFamily.ValueKind != JsonValueKind.String ||
                alignedFamily.GetString() != item.GenerationFamily ||
                !metadata.TryGetProperty("questionVariant", out var attestedVariant) ||
                attestedVariant.ValueKind != JsonValueKind.String ||
                attestedVariant.GetString() != displayedVariant.GetString() ||
                !metadata.TryGetProperty("solverVerified", out var solverVerified) ||
                solverVerified.ValueKind != JsonValueKind.True ||
                !metadata.TryGetProperty("broadFallbackUsed", out var broadFallback) ||
                broadFallback.ValueKind != JsonValueKind.False ||
                !metadata.TryGetProperty("officialMasteryEvidence", out var officialEvidence) ||
                officialEvidence.ValueKind != JsonValueKind.False)
                return false;

            // Adapter Variant enum values are 0=Hanoi, 1=Checker,
            // 2..5=the four exact recurrence variants. Teacher-only
            // variants 6..8, and any family mismatch, fail closed.
            int sourceSlot = item.GenerationFamily switch
            {
                "usccss.alg2.u01.l01.hanoi_min_moves" when variantValue == 0 => 0,
                "usccss.alg2.u01.l01.checker_jump_optional" when variantValue == 1 => 0,
                "usccss.alg2.u01.l01.recursive_sequence"
                    when variantValue is >= 2 and <= 5 => variantValue - 2,
                _ => -1
            };
            if (sourceSlot < 0 ||
                !UsCcssAlg2U01L01PracticeAdapter.TryPrepareExactNumericQuestion(
                    item.GenerationFamily, sourceSlot, n,
                    out _, out var canonicalAnswer, out var canonicalParameters) ||
                !canonicalParameters.TryGetValue("variant", out int canonicalVariant) ||
                canonicalVariant != variantValue ||
                !string.Equals(item.CorrectAnswer, canonicalAnswer, StringComparison.Ordinal))
                return false;

            return UsCcssAlg2U01L01PracticeAdapter.VerifyExactNumericQuestion(
                item.GenerationFamily, canonicalParameters, submittedAnswer);
        }
        catch (JsonException)
        {
            // Malformed persisted metadata is NOT a reason to use AnswersMatch.
            return false;
        }
    }

    public static bool AnswersMatch(string submitted, string expected) =>
        MathematicsAnswerEquivalence.AreEquivalent(submitted, expected);

}
