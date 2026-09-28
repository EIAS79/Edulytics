using System.Security.Cryptography;
using System.Text;
using Edulytics.Core.AdaptivePractice;
using Edulytics.Core.Entities;
using Edulytics.Core.Enums;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Core.Practice;

namespace Edulytics.Services.AdaptivePractice;

public interface IAdaptivePracticeShadowObserver
{
    Task ObserveLessonAnswerAsync(
        Guid studentUserId,
        Guid curriculumAdoptionId,
        Guid lessonId,
        Guid v1AttemptId,
        Guid v1AttemptItemId,
        CancellationToken cancellationToken = default);
}

/// <summary>
/// One-step counterfactual observer over legacy V1 Practice.
/// It never changes the V1 attempt and never persists the generated shadow item.
/// </summary>
public sealed class AdaptivePracticeShadowObserver(
    IPracticeRepository practiceRepository,
    IStudentPrivatePracticeRepository privatePracticeRepository,
    IAdaptivePracticeRepository adaptiveRepository,
    IAdaptivePracticeEligibilityResolver eligibilityResolver,
    AdaptiveLearningStateAssembler stateAssembler,
    AdaptiveNextItemDecisionEngine decisionEngine,
    AdaptiveVerifiedItemGenerator itemGenerator,
    AdaptiveMisconceptionClassifier misconceptionClassifier,
    AdaptivePracticeV2Policy policy)
    : IAdaptivePracticeShadowObserver
{
    public async Task ObserveLessonAnswerAsync(
        Guid studentUserId,
        Guid curriculumAdoptionId,
        Guid lessonId,
        Guid v1AttemptId,
        Guid v1AttemptItemId,
        CancellationToken cancellationToken = default)
    {
        if (!policy.Enabled ||
            policy.Mode != AdaptivePracticeV2Mode.Shadow ||
            policy.ShadowSamplingPercentage <= 0 ||
            cancellationToken.IsCancellationRequested ||
            !Sample(
                v1AttemptId,
                policy.ShadowSamplingPercentage))
        {
            return;
        }

        try
        {
            await ObserveCoreAsync(
                studentUserId,
                curriculumAdoptionId,
                lessonId,
                v1AttemptId,
                v1AttemptItemId,
                cancellationToken);
        }
        catch (OperationCanceledException)
            when (cancellationToken.IsCancellationRequested)
        {
            // Shadow must never turn a successfully accepted V1 answer into
            // a learner-facing failure because the request was disconnected.
        }
        catch (Exception)
        {
            // Shadow is deliberately best-effort. Counterfactual observations
            // must never change or fail an already accepted V1 learner answer.
        }
    }

    private async Task ObserveCoreAsync(
        Guid studentUserId,
        Guid curriculumAdoptionId,
        Guid lessonId,
        Guid v1AttemptId,
        Guid v1AttemptItemId,
        CancellationToken cancellationToken)
    {
        var context =
            await privatePracticeRepository.GetContextAsync(
                studentUserId,
                curriculumAdoptionId,
                cancellationToken);

        if (context is null)
            return;

        var lesson = context.Lessons.SingleOrDefault(
            x => x.Id == lessonId);

        if (lesson is null ||
            !LessonPracticeCapabilityResolver.TryResolve(
                lesson.Code,
                out var contract) ||
            contract is null)
        {
            return;
        }

        var eligibility = eligibilityResolver.Resolve(
            new AdaptivePracticeEligibilityRequest(
                context.Student.SchoolId,
                context.Adoption.CurriculumLevelKey ?? string.Empty,
                lesson.Code,
                IsMathematics: true));

        if (!eligibility.IsShadow)
            return;

        var attempt = await practiceRepository.GetAttemptAsync(
            context.Student.SchoolId,
            v1AttemptId,
            cancellationToken);

        if (attempt is null ||
            attempt.StudentProfileId != context.Student.Id ||
            attempt.CurriculumAdoptionId != context.Adoption.Id ||
            attempt.CurriculumPedagogicalLessonId != lessonId ||
            !attempt.IsPrivate)
        {
            return;
        }

        var attemptItems =
            await practiceRepository.GetAttemptItemsAsync(
                context.Student.SchoolId,
                v1AttemptId,
                cancellationToken);

        var currentAttemptItem = attemptItems.SingleOrDefault(
            x => x.Id == v1AttemptItemId);

        if (currentAttemptItem is null)
            return;

        var responses =
            await practiceRepository.GetResponsesAsync(
                context.Student.SchoolId,
                v1AttemptId,
                cancellationToken);

        var currentResponse = responses.SingleOrDefault(
            x => x.PracticeAttemptItemId ==
                v1AttemptItemId);

        if (currentResponse is null)
            return;

        var items =
            await practiceRepository.GetItemsAsync(
                context.Student.SchoolId,
                attemptItems
                    .Select(x => x.AssessmentItemId)
                    .Distinct()
                    .ToArray(),
                cancellationToken);

        var itemById = items.ToDictionary(
            x => x.Id);
        var responseByAttemptItem =
            responses.ToDictionary(
                x => x.PracticeAttemptItemId);

        if (!itemById.TryGetValue(
                currentAttemptItem.AssessmentItemId,
                out var currentItem))
        {
            return;
        }

        var observedMisconception =
            currentResponse.IsCorrect
                ? null
                : misconceptionClassifier.Classify(
                    currentItem,
                    currentResponse.Answer)
                    ?.MisconceptionId;

        var pseudoTurns = attemptItems
            .Where(x =>
                x.Order <= currentAttemptItem.Order &&
                responseByAttemptItem.ContainsKey(x.Id) &&
                itemById.ContainsKey(x.AssessmentItemId))
            .OrderBy(x => x.Order)
            .Select(x =>
            {
                var item = itemById[x.AssessmentItemId];
                var response =
                    responseByAttemptItem[x.Id];
                var isCurrent =
                    x.Id == currentAttemptItem.Id;

                return new AdaptivePracticeTurn
                {
                    Id = Guid.NewGuid(),
                    SchoolId = context.Student.SchoolId,
                    SessionId = Guid.Empty,
                    Sequence = x.Order,
                    AssessmentItemId = item.Id,
                    SkillId = contract.SkillId,
                    QuestionFamily =
                        item.GenerationFamily ??
                        contract.AllowedQuestionFamilies[0],
                    Representation =
                        ResolveRepresentation(item),
                    MathematicalComplexityScore =
                        ComplexityFor(item.Difficulty),
                    UiDifficultyBand = item.Difficulty,
                    MisconceptionFocusId =
                        isCurrent
                            ? observedMisconception
                            : null,
                    IsIndependentConfirmation = false,
                    PresentedAtUtc =
                        response.AnsweredAtUtc,
                    AnsweredAtUtc =
                        response.AnsweredAtUtc,
                    SubmittedAnswer = response.Answer,
                    IsCorrect = response.IsCorrect,
                    Score = response.Score,
                    Feedback = response.Feedback,
                    ResponseDurationMs = null,
                    ExposureFingerprint =
                        item.ExposureFingerprint,
                    SemanticIdentityKey =
                        AdaptivePracticeSemanticIdentity.Resolve(
                            item),
                    RowVersion = []
                };
            })
            .ToArray();

        if (pseudoTurns.Length == 0)
            return;

        var state = stateAssembler.Build(
            context,
            lessonId,
            contract,
            pseudoTurns,
            [],
            []);

        var decision = decisionEngine.Decide(state);

        var generationFeasible = false;
        try
        {
            _ = itemGenerator.GenerateOne(
                context.Student.SchoolId,
                context.Adoption.Id,
                lessonId,
                studentUserId,
                contract,
                decision,
                DeterministicSeed(
                    v1AttemptId,
                    v1AttemptItemId),
                context.Exposures
                    .Select(x => x.ExposureFingerprint)
                    .Concat(
                        pseudoTurns.Select(x =>
                            x.ExposureFingerprint))
                    .Where(x =>
                        !string.IsNullOrWhiteSpace(x))
                    .Distinct(StringComparer.Ordinal)
                    .ToArray(),
                pseudoTurns
                    .Select(x => x.SemanticIdentityKey)
                    .Where(x =>
                        !string.IsNullOrWhiteSpace(x))
                    .Distinct(StringComparer.Ordinal)
                    .ToArray());

            generationFeasible = true;
        }
        catch (InvalidOperationException)
        {
            generationFeasible = false;
        }

        await adaptiveRepository.AddShadowObservationIfMissingAsync(
            new AdaptivePracticeShadowObservation
            {
                Id = Guid.NewGuid(),
                SchoolId = context.Student.SchoolId,
                StudentProfileId = context.Student.Id,
                CurriculumAdoptionId = context.Adoption.Id,
                CurriculumPedagogicalLessonId = lessonId,
                V1AttemptId = v1AttemptId,
                V1AttemptItemId = v1AttemptItemId,
                V1Sequence = currentAttemptItem.Order,
                V1WasCorrect = currentResponse.IsCorrect,
                V1QuestionFamily =
                    currentItem.GenerationFamily,
                V1Difficulty = currentItem.Difficulty,
                ProposedSkillId =
                    decision.TargetSkillId,
                ProposedComplexity =
                    decision.TargetComplexityScore,
                ProposedFamily =
                    decision.TargetQuestionFamily,
                ProposedRepresentation =
                    decision.TargetRepresentation,
                ProposedMisconceptionFocusId =
                    decision.MisconceptionFocusId,
                ObservedMisconceptionId =
                    observedMisconception,
                DecisionReasonCode =
                    decision.ReasonCode,
                GenerationFeasible =
                    generationFeasible,
                EngineVersion =
                    decision.EngineVersion,
                PolicyVersion =
                    decision.PolicyVersion,
                CreatedAtUtc = DateTime.UtcNow
            },
            cancellationToken);
    }

    private static string? ResolveRepresentation(
        AssessmentItem item)
    {
        if (string.IsNullOrWhiteSpace(
                item.GenerationFamily) ||
            !LessonPracticeInteractionRegistry.TryResolve(
                item.GenerationFamily,
                out var interaction) ||
            interaction is null)
        {
            return null;
        }

        return interaction.Representations
            .FirstOrDefault();
    }

    private static int ComplexityFor(
        AssessmentItemDifficulty difficulty) =>
        difficulty switch
        {
            AssessmentItemDifficulty.Easy => 30,
            AssessmentItemDifficulty.Medium => 42,
            AssessmentItemDifficulty.Challenging => 68,
            _ => 42
        };

    private static bool Sample(
        Guid attemptId,
        int percentage)
    {
        if (percentage >= 100)
            return true;

        Span<byte> bytes = stackalloc byte[16];
        attemptId.TryWriteBytes(bytes);
        var value =
            BitConverter.ToUInt32(bytes[..4]);

        return value % 100 < percentage;
    }

    private static int DeterministicSeed(
        Guid attemptId,
        Guid attemptItemId)
    {
        var input =
            Encoding.UTF8.GetBytes(
                attemptId.ToString("N") +
                attemptItemId.ToString("N"));
        var hash = SHA256.HashData(input);
        var value =
            BitConverter.ToInt32(hash, 0);

        return value == 0
            ? 1
            : value == int.MinValue
                ? int.MaxValue
                : Math.Abs(value);
    }
}
