using System.Security.Cryptography;
using System.Text.Json;
using Edulytics.Core.AdaptivePractice;
using Edulytics.Core.Entities;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Core.Practice;
using Edulytics.Services.Assessments;

namespace Edulytics.Services.AdaptivePractice;

/// <summary>
/// Learner-facing Adaptive Practice V2 orchestrator.
///
/// This service is additive. It never mutates a V1 PracticeAttempt and may be
/// entered only through the fail-closed AdaptivePracticeEligibilityResolver.
/// </summary>
public sealed class AdaptivePracticeV2Service(
    IPracticeRepository practiceRepository,
    IStudentPrivatePracticeRepository privatePracticeRepository,
    IAdaptivePracticeRepository adaptiveRepository,
    IAdaptivePracticeEligibilityResolver eligibilityResolver,
    AdaptiveLearningStateAssembler stateAssembler,
    AdaptiveNextItemDecisionEngine decisionEngine,
    AdaptiveVerifiedItemGenerator itemGenerator,
    AdaptivePracticeEvidenceProjector evidenceProjector,
    AdaptiveRemediationGuidanceEngine guidanceEngine,
    AdaptivePracticeV2Policy policy)
    : IAdaptivePracticeV2Service
{
    public async Task<AdaptivePracticeStartResult> StartLessonAsync(
        Guid studentUserId,
        Guid curriculumAdoptionId,
        Guid lessonId,
        CancellationToken cancellationToken = default)
    {
        // Off/Shadow must not add database work or alter the V1 learner path.
        // Shadow observation is wired separately against V1 answer events.
        if (!policy.Enabled ||
            policy.Mode is
                AdaptivePracticeV2Mode.Off or
                AdaptivePracticeV2Mode.Shadow)
        {
            return AdaptivePracticeStartResult.Failure(
                AdaptivePracticeV2Error.NotEligible);
        }

        if (studentUserId == Guid.Empty ||
            curriculumAdoptionId == Guid.Empty ||
            lessonId == Guid.Empty)
        {
            return AdaptivePracticeStartResult.Failure(
                AdaptivePracticeV2Error.AccessDenied);
        }

        var context =
            await privatePracticeRepository.GetContextAsync(
                studentUserId,
                curriculumAdoptionId,
                cancellationToken);

        if (context is null)
        {
            return AdaptivePracticeStartResult.Failure(
                AdaptivePracticeV2Error.CurriculumNotAvailable);
        }

        var lesson = context.Lessons.SingleOrDefault(
            x => x.Id == lessonId);

        if (lesson is null)
        {
            return AdaptivePracticeStartResult.Failure(
                AdaptivePracticeV2Error.LessonNotAvailable);
        }

        var eligibility = eligibilityResolver.Resolve(
            new AdaptivePracticeEligibilityRequest(
                context.Student.SchoolId,
                context.Adoption.CurriculumLevelKey ?? string.Empty,
                lesson.Code,
                IsMathematics: true));

        if (!eligibility.IsLearnerFacing)
        {
            return AdaptivePracticeStartResult.Failure(
                AdaptivePracticeV2Error.NotEligible);
        }

        if (!LessonPracticeCapabilityResolver.TryResolve(
                lesson.Code,
                out var contract) ||
            contract is null)
        {
            return AdaptivePracticeStartResult.Failure(
                AdaptivePracticeV2Error.NotEligible);
        }

        var misconceptionStates =
            await adaptiveRepository.GetMisconceptionStatesAsync(
                context.Student.SchoolId,
                context.Student.Id,
                context.Adoption.Id,
                contract.SkillId,
                cancellationToken);

        var representationStates =
            await adaptiveRepository.GetRepresentationStatesAsync(
                context.Student.SchoolId,
                context.Student.Id,
                contract.SkillId,
                cancellationToken);

        var learningState = stateAssembler.Build(
            context,
            lessonId,
            contract,
            [],
            misconceptionStates,
            representationStates);

        var proposed = decisionEngine.Decide(learningState);
        var decision = proposed with
        {
            ReasonCode =
                AdaptivePracticeDecisionReasonCodes.SessionBaseline,
            RemediationLockActive = false,
            ConfirmationRequired = false,
            IsIndependentConfirmation = false,
            ProgressionEligible = false
        };

        AssessmentItem item;
        try
        {
            item = itemGenerator.GenerateOne(
                context.Student.SchoolId,
                context.Adoption.Id,
                lessonId,
                studentUserId,
                contract,
                decision,
                RandomNumberGenerator.GetInt32(1, int.MaxValue),
                context.Exposures
                    .Select(x => x.ExposureFingerprint)
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .Distinct(StringComparer.Ordinal)
                    .ToArray(),
                excludedSemanticIdentityKeys: []);
        }
        catch (InvalidOperationException)
        {
            return AdaptivePracticeStartResult.Failure(
                AdaptivePracticeV2Error.GenerationFailed);
        }

        var now = DateTime.UtcNow;
        var session = new AdaptivePracticeSession
        {
            Id = Guid.NewGuid(),
            SchoolId = context.Student.SchoolId,
            StudentProfileId = context.Student.Id,
            CurriculumAdoptionId = context.Adoption.Id,
            CurriculumLevelKey =
                context.Adoption.CurriculumLevelKey ?? string.Empty,
            CurriculumPedagogicalLessonId = lessonId,
            LessonCode = lesson.Code,
            PrimarySkillId = contract.SkillId,
            Purpose = AdaptivePracticePurpose.PrivatePractice,
            Status = AdaptivePracticeSessionStatus.InProgress,
            EngineVersion = decision.EngineVersion,
            PolicyVersion = decision.PolicyVersion,
            CapabilityVersion = contract.ContractVersion,
            FeatureFlagSnapshotJson = JsonSerializer.Serialize(new
            {
                policy.Enabled,
                Mode = policy.Mode.ToString(),
                policy.MaxLessonQuestions,
                policy.EnableMisconceptionLoop,
                policy.RouteAllReadyVerifiedLessons
            }),
            TargetQuestionCount = policy.MaxLessonQuestions,
            CurrentSequence = 1,
            StartedAtUtc = now,
            RowVersion = []
        };

        var decisionSnapshot = CreateDecisionSnapshot(
            session,
            sequence: 1,
            learningState,
            decision,
            now);

        var turn = CreateTurn(
            session,
            item,
            decisionSnapshot,
            decision,
            now);

        var exposure = CreateExposure(
            context.Student,
            item,
            now);

        var outcomeLinks = BuildOutcomeLinks(
            context,
            lessonId,
            item);

        await adaptiveRepository.CreateSessionWithFirstTurnAsync(
            session,
            item,
            outcomeLinks,
            exposure,
            decisionSnapshot,
            turn,
            cancellationToken);

        return AdaptivePracticeStartResult.Success(
            await BuildSessionViewAsync(
                session,
                turn,
                item,
                cancellationToken));
    }

    public async Task<AdaptivePracticeStartResult> GetSessionAsync(
        Guid studentUserId,
        Guid sessionId,
        CancellationToken cancellationToken = default)
    {
        var access = await GetOwnedSessionAsync(
            studentUserId,
            sessionId,
            cancellationToken);

        if (access.Error.HasValue)
        {
            return AdaptivePracticeStartResult.Failure(
                access.Error.Value);
        }

        var session = access.Session!;
        if (session.Status != AdaptivePracticeSessionStatus.InProgress)
        {
            return AdaptivePracticeStartResult.Success(
                BuildCompletedView(session));
        }

        var turn = await adaptiveRepository.GetTurnAsync(
            session.SchoolId,
            session.Id,
            session.CurrentSequence,
            cancellationToken);

        if (turn is null)
        {
            return AdaptivePracticeStartResult.Failure(
                AdaptivePracticeV2Error.TurnNotFound);
        }

        var item = await adaptiveRepository.GetItemAsync(
            session.SchoolId,
            turn.AssessmentItemId,
            cancellationToken);

        if (item is null)
        {
            return AdaptivePracticeStartResult.Failure(
                AdaptivePracticeV2Error.TurnNotFound);
        }

        return AdaptivePracticeStartResult.Success(
            await BuildSessionViewAsync(
                session,
                turn,
                item,
                cancellationToken));
    }

    public async Task<AdaptivePracticeAnswerResult> AnswerAsync(
        Guid studentUserId,
        Guid sessionId,
        int sequence,
        string answer,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(answer) ||
            answer.Trim().Length > 2000 ||
            sequence <= 0)
        {
            return AdaptivePracticeAnswerResult.Failure(
                AdaptivePracticeV2Error.InvalidAnswer);
        }

        var access = await GetOwnedSessionAsync(
            studentUserId,
            sessionId,
            cancellationToken);

        if (access.Error.HasValue)
        {
            return AdaptivePracticeAnswerResult.Failure(
                access.Error.Value);
        }

        var session = access.Session!;
        if (session.Status != AdaptivePracticeSessionStatus.InProgress)
        {
            return AdaptivePracticeAnswerResult.Failure(
                AdaptivePracticeV2Error.SessionNotInProgress);
        }

        if (sequence != session.CurrentSequence)
        {
            return AdaptivePracticeAnswerResult.Failure(
                AdaptivePracticeV2Error.TurnNotFound);
        }

        var turn = await adaptiveRepository.GetTurnAsync(
            session.SchoolId,
            session.Id,
            sequence,
            cancellationToken);

        if (turn is null)
        {
            return AdaptivePracticeAnswerResult.Failure(
                AdaptivePracticeV2Error.TurnNotFound);
        }

        if (turn.AnsweredAtUtc.HasValue)
        {
            return AdaptivePracticeAnswerResult.Failure(
                AdaptivePracticeV2Error.TurnAlreadyAnswered);
        }

        var item = await adaptiveRepository.GetItemAsync(
            session.SchoolId,
            turn.AssessmentItemId,
            cancellationToken);

        if (item is null)
        {
            return AdaptivePracticeAnswerResult.Failure(
                AdaptivePracticeV2Error.TurnNotFound);
        }

        var now = DateTime.UtcNow;
        var trimmed = answer.Trim();
        var correct = MathematicsAnswerEquivalence.AreEquivalent(
            trimmed,
            item.CorrectAnswer);

        var existingMisconceptionStates =
            await adaptiveRepository.GetMisconceptionStatesAsync(
                session.SchoolId,
                session.StudentProfileId,
                session.CurriculumAdoptionId,
                session.PrimarySkillId,
                cancellationToken);

        var existingRepresentationStates =
            await adaptiveRepository.GetRepresentationStatesAsync(
                session.SchoolId,
                session.StudentProfileId,
                session.PrimarySkillId,
                cancellationToken);

        AdaptivePracticeEvidenceUpdate evidenceUpdate;
        AdaptiveRemediationGuidance? remediationGuidance = null;

        if (!correct)
        {
            turn.IncorrectAttemptCount++;
            turn.LastIncorrectAnswer = trimmed;
            turn.LastIncorrectAtUtc = now;

            remediationGuidance = guidanceEngine.Build(
                item,
                trimmed,
                turn.IncorrectAttemptCount);

            var observation = CreateEvidenceObservation(
                turn,
                trimmed,
                isCorrect: false,
                remediationGuidance.Hint,
                now);

            evidenceUpdate = evidenceProjector.Project(
                session,
                observation,
                item,
                existingMisconceptionStates,
                existingRepresentationStates,
                policy.EnableMisconceptionLoop,
                now);

            if (evidenceUpdate.MisconceptionState is not null)
            {
                turn.MisconceptionFocusId =
                    evidenceUpdate.MisconceptionState.MisconceptionId;
            }

            if (turn.IncorrectAttemptCount == 1)
            {
                // C0/C2: exactly one retry of the exact same item is allowed.
                // Keep the turn open, persist the evidence, and return the
                // same sequence with a targeted answer-aware hint.
                turn.Feedback = remediationGuidance.Hint;

                await adaptiveRepository.CommitAnsweredTurnAsync(
                    session,
                    turn,
                    evidenceUpdate.MisconceptionState,
                    evidenceUpdate.RepresentationState,
                    null,
                    [],
                    null,
                    null,
                    null,
                    cancellationToken);

                return AdaptivePracticeAnswerResult.Success(
                    await BuildSessionViewAsync(
                        session,
                        turn,
                        item,
                        cancellationToken),
                    false,
                    remediationGuidance.Hint);
            }

            // C0/C2: Wrong #2 closes this exact item. No third retry is
            // permitted. The decision engine will generate a fresh bounded
            // remediation item from this completed incorrect evidence.
            turn.SubmittedAnswer = trimmed;
            turn.IsCorrect = false;
            turn.Score = 0m;
            turn.Feedback =
                remediationGuidance.WorkedExample ??
                remediationGuidance.Hint;
            turn.AnsweredAtUtc = now;
            turn.ResponseDurationMs = Math.Max(
                0L,
                (long)(now - turn.PresentedAtUtc).TotalMilliseconds);
        }
        else
        {
            turn.SubmittedAnswer = trimmed;
            turn.IsCorrect = true;
            turn.Score = 1m;
            turn.Feedback = item.Solution;
            turn.AnsweredAtUtc = now;
            turn.ResponseDurationMs = Math.Max(
                0L,
                (long)(now - turn.PresentedAtUtc).TotalMilliseconds);

            evidenceUpdate = evidenceProjector.Project(
                session,
                turn,
                item,
                existingMisconceptionStates,
                existingRepresentationStates,
                policy.EnableMisconceptionLoop,
                now);
        }

        if (!string.Equals(
                session.EngineVersion,
                AdaptivePracticeV2Versions.EngineVersion,
                StringComparison.Ordinal) ||
            !string.Equals(
                session.PolicyVersion,
                AdaptivePracticeV2Versions.PolicyVersion,
                StringComparison.Ordinal))
        {
            session.Status = AdaptivePracticeSessionStatus.Paused;
            session.StopReason = "PINNED_ENGINE_VERSION_UNAVAILABLE";

            await adaptiveRepository.CommitAnsweredTurnAsync(
                session,
                turn,
                evidenceUpdate.MisconceptionState,
                evidenceUpdate.RepresentationState,
                null,
                [],
                null,
                null,
                null,
                cancellationToken);

            return AdaptivePracticeAnswerResult.Failure(
                AdaptivePracticeV2Error.SessionNotInProgress);
        }

        var context =
            await privatePracticeRepository.GetContextAsync(
                studentUserId,
                session.CurriculumAdoptionId,
                cancellationToken);

        if (context is null)
        {
            return AdaptivePracticeAnswerResult.Failure(
                AdaptivePracticeV2Error.CurriculumNotAvailable);
        }

        if (!LessonPracticeCapabilityResolver.TryResolve(
                session.LessonCode,
                out var contract) ||
            contract is null ||
            !string.Equals(
                contract.ContractVersion,
                session.CapabilityVersion,
                StringComparison.Ordinal) ||
            !string.Equals(
                contract.SkillId,
                session.PrimarySkillId,
                StringComparison.Ordinal))
        {
            return AdaptivePracticeAnswerResult.Failure(
                AdaptivePracticeV2Error.NotEligible);
        }

        var storedTurns = await adaptiveRepository.GetTurnsAsync(
            session.SchoolId,
            session.Id,
            cancellationToken);

        var replayTurns = storedTurns
            .Where(x => x.Sequence != turn.Sequence)
            .Append(turn)
            .OrderBy(x => x.Sequence)
            .ToArray();

        var misconceptionStates = MergeMisconceptionState(
            existingMisconceptionStates,
            evidenceUpdate.MisconceptionState);
        var representationStates = MergeRepresentationState(
            existingRepresentationStates,
            evidenceUpdate.RepresentationState);

        var learningState = stateAssembler.Build(
            context,
            session.CurriculumPedagogicalLessonId,
            contract,
            replayTurns,
            misconceptionStates,
            representationStates);

        var decision = decisionEngine.Decide(learningState);

        if (sequence >= session.TargetQuestionCount)
        {
            var unresolvedAdaptiveWork =
                decision.RemediationLockActive ||
                decision.ConfirmationRequired ||
                !correct;

            if (unresolvedAdaptiveWork &&
                session.TargetQuestionCount < 30)
            {
                // The ordinary item budget cannot end a session while a
                // remediation/confirmation contract is still open.
                session.TargetQuestionCount++;
            }
            else
            {
                session.Status =
                    AdaptivePracticeSessionStatus.Completed;
                session.CompletedAtUtc = now;
                session.StopReason = unresolvedAdaptiveWork
                    ? "MAX_REMEDIATION_BUDGET_REACHED"
                    : "QUESTION_BUDGET_REACHED";

                await adaptiveRepository.CommitAnsweredTurnAsync(
                    session,
                    turn,
                    evidenceUpdate.MisconceptionState,
                    evidenceUpdate.RepresentationState,
                    null,
                    [],
                    null,
                    null,
                    null,
                    cancellationToken);

                return AdaptivePracticeAnswerResult.Success(
                    BuildCompletedView(session),
                    correct,
                    turn.Feedback ?? item.Solution);
            }
        }

        AssessmentItem nextItem;
        try
        {
            nextItem = itemGenerator.GenerateOne(
                session.SchoolId,
                session.CurriculumAdoptionId,
                session.CurriculumPedagogicalLessonId,
                studentUserId,
                contract,
                decision,
                RandomNumberGenerator.GetInt32(1, int.MaxValue),
                context.Exposures
                    .Select(x => x.ExposureFingerprint)
                    .Concat(
                        replayTurns.Select(x =>
                            x.ExposureFingerprint))
                    .Where(x =>
                        !string.IsNullOrWhiteSpace(x))
                    .Distinct(StringComparer.Ordinal)
                    .ToArray(),
                replayTurns
                    .Select(x => x.SemanticIdentityKey)
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .Distinct(StringComparer.Ordinal)
                    .ToArray());
        }
        catch (InvalidOperationException)
        {
            session.Status =
                AdaptivePracticeSessionStatus.Paused;
            session.StopReason =
                "VERIFIED_NEXT_ITEM_GENERATION_FAILED";

            await adaptiveRepository.CommitAnsweredTurnAsync(
                session,
                turn,
                evidenceUpdate.MisconceptionState,
                evidenceUpdate.RepresentationState,
                null,
                [],
                null,
                null,
                null,
                cancellationToken);

            return AdaptivePracticeAnswerResult.Failure(
                AdaptivePracticeV2Error.GenerationFailed);
        }

        var nextSequence = sequence + 1;
        var nextDecisionSnapshot = CreateDecisionSnapshot(
            session,
            nextSequence,
            learningState,
            decision,
            now);

        var nextTurn = CreateTurn(
            session,
            nextItem,
            nextDecisionSnapshot,
            decision,
            now);

        nextTurn.Sequence = nextSequence;
        nextDecisionSnapshot.Sequence = nextSequence;

        if (!correct &&
            remediationGuidance is not null)
        {
            // Carry the stronger scaffold onto the fresh remediation item.
            // The learner sees the worked example before attempting the new
            // numbers; the current item's verified answer is never exposed.
            nextTurn.Feedback =
                remediationGuidance.WorkedExample ??
                remediationGuidance.Hint;
        }

        var nextExposure = CreateExposure(
            context.Student,
            nextItem,
            now);

        var nextOutcomeLinks = BuildOutcomeLinks(
            context,
            session.CurriculumPedagogicalLessonId,
            nextItem);

        session.CurrentSequence = nextSequence;

        await adaptiveRepository.CommitAnsweredTurnAsync(
            session,
            turn,
            evidenceUpdate.MisconceptionState,
            evidenceUpdate.RepresentationState,
            nextItem,
            nextOutcomeLinks,
            nextExposure,
            nextDecisionSnapshot,
            nextTurn,
            cancellationToken);

        return AdaptivePracticeAnswerResult.Success(
            await BuildSessionViewAsync(
                session,
                nextTurn,
                nextItem,
                cancellationToken),
            correct,
            item.Solution);
    }

    private async Task<(
        StudentProfile? Student,
        AdaptivePracticeSession? Session,
        AdaptivePracticeV2Error? Error)> GetOwnedSessionAsync(
            Guid studentUserId,
            Guid sessionId,
            CancellationToken cancellationToken)
    {
        var student =
            await practiceRepository.FindStudentByUserIdAsync(
                studentUserId,
                cancellationToken);

        if (student is null)
        {
            return (
                null,
                null,
                AdaptivePracticeV2Error.AccessDenied);
        }

        var session = await adaptiveRepository.GetSessionAsync(
            student.SchoolId,
            sessionId,
            cancellationToken);

        if (session is null)
        {
            return (
                student,
                null,
                AdaptivePracticeV2Error.SessionNotFound);
        }

        if (session.StudentProfileId != student.Id)
        {
            return (
                student,
                session,
                AdaptivePracticeV2Error.AccessDenied);
        }

        return (student, session, null);
    }

    private static IReadOnlyList<AssessmentItemOutcome>
        BuildOutcomeLinks(
            StudentPrivatePracticeContext context,
            Guid lessonId,
            AssessmentItem item)
    {
        var nodeIds = context.LessonOutcomes
            .Where(x => x.PedagogicalLessonId == lessonId)
            .Select(x => x.OutcomeNodeId)
            .ToHashSet();

        return context.LearningOutcomes
            .Where(x =>
                x.OfficialContentNodeId.HasValue &&
                nodeIds.Contains(
                    x.OfficialContentNodeId.Value))
            .Select(x =>
                new AssessmentItemOutcome
                {
                    Id = Guid.NewGuid(),
                    SchoolId = context.Student.SchoolId,
                    AssessmentItemId = item.Id,
                    LearningOutcomeId = x.Id
                })
            .ToArray();
    }

    private static StudentItemExposure CreateExposure(
        StudentProfile student,
        AssessmentItem item,
        DateTime now) =>
        new()
        {
            Id = Guid.NewGuid(),
            SchoolId = student.SchoolId,
            StudentProfileId = student.Id,
            AssessmentItemId = item.Id,
            ExposureFingerprint = item.ExposureFingerprint,
            ExposedAtUtc = now
        };

    private static AdaptiveDecisionSnapshot CreateDecisionSnapshot(
        AdaptivePracticeSession session,
        int sequence,
        AdaptivePracticeLearningState state,
        AdaptiveNextItemDecision decision,
        DateTime now) =>
        new()
        {
            Id = Guid.NewGuid(),
            SchoolId = session.SchoolId,
            SessionId = session.Id,
            Sequence = sequence,
            EngineVersion = decision.EngineVersion,
            PolicyVersion = decision.PolicyVersion,
            SkillMasteryBefore = state.SkillMastery,
            PrerequisiteMasteryBefore =
                state.PrerequisiteMastery,
            RepresentationFluencyJson =
                JsonSerializer.Serialize(
                    state.RepresentationFluency),
            ActiveMisconceptionsJson =
                JsonSerializer.Serialize(
                    state.Misconceptions),
            CurrentComplexity =
                state.CurrentComplexityScore,
            TargetComplexity =
                decision.TargetComplexityScore,
            SelectedFamily =
                decision.TargetQuestionFamily,
            SelectedRepresentation =
                decision.TargetRepresentation,
            MisconceptionFocusId =
                decision.MisconceptionFocusId,
            FreshnessConstraintsJson =
                JsonSerializer.Serialize(new
                {
                    decision.RequiresFreshExposure
                }),
            DecisionReasonCode = decision.ReasonCode,
            DecisionTraceJson =
                JsonSerializer.Serialize(new
                {
                    decision.RemediationLockActive,
                    decision.ConfirmationRequired,
                    decision.IsIndependentConfirmation,
                    decision.ProgressionEligible
                }),
            CreatedAtUtc = now
        };

    private static AdaptivePracticeTurn CreateTurn(
        AdaptivePracticeSession session,
        AssessmentItem item,
        AdaptiveDecisionSnapshot snapshot,
        AdaptiveNextItemDecision decision,
        DateTime now) =>
        new()
        {
            Id = Guid.NewGuid(),
            SchoolId = session.SchoolId,
            SessionId = session.Id,
            Sequence = snapshot.Sequence,
            AssessmentItemId = item.Id,
            DecisionSnapshotId = snapshot.Id,
            SkillId = decision.TargetSkillId,
            QuestionFamily =
                item.GenerationFamily ??
                decision.TargetQuestionFamily,
            Representation =
                ResolveRepresentation(
                    item,
                    decision),
            MathematicalComplexityScore =
                decision.TargetComplexityScore,
            UiDifficultyBand = item.Difficulty,
            MisconceptionFocusId =
                decision.MisconceptionFocusId,
            IsIndependentConfirmation =
                decision.IsIndependentConfirmation,
            PresentedAtUtc = now,
            ExposureFingerprint =
                item.ExposureFingerprint,
            SemanticIdentityKey =
                AdaptivePracticeSemanticIdentity.Resolve(item),
            RowVersion = []
        };

    private static string? ResolveRepresentation(
        AssessmentItem item,
        AdaptiveNextItemDecision decision)
    {
        if (!string.IsNullOrWhiteSpace(
                decision.TargetRepresentation))
        {
            return decision.TargetRepresentation;
        }

        if (!string.IsNullOrWhiteSpace(
                item.GenerationFamily) &&
            LessonPracticeInteractionRegistry.TryResolve(
                item.GenerationFamily,
                out var interaction) &&
            interaction is not null)
        {
            return interaction.Representations
                .FirstOrDefault();
        }

        return null;
    }

    private static IReadOnlyList<StudentMisconceptionState>
        MergeMisconceptionState(
            IReadOnlyList<StudentMisconceptionState> source,
            StudentMisconceptionState? update)
    {
        if (update is null)
            return source;

        return source
            .Where(x =>
                !string.Equals(
                    x.MisconceptionId,
                    update.MisconceptionId,
                    StringComparison.Ordinal))
            .Append(update)
            .ToArray();
    }

    private static IReadOnlyList<StudentRepresentationFluencyState>
        MergeRepresentationState(
            IReadOnlyList<StudentRepresentationFluencyState> source,
            StudentRepresentationFluencyState? update)
    {
        if (update is null)
            return source;

        return source
            .Where(x =>
                !string.Equals(
                    x.Representation,
                    update.Representation,
                    StringComparison.Ordinal))
            .Append(update)
            .ToArray();
    }

    private static AdaptivePracticeTurn CreateEvidenceObservation(
        AdaptivePracticeTurn source,
        string submittedAnswer,
        bool isCorrect,
        string feedback,
        DateTime answeredAtUtc) =>
        new()
        {
            Id = source.Id,
            SchoolId = source.SchoolId,
            SessionId = source.SessionId,
            Sequence = source.Sequence,
            AssessmentItemId = source.AssessmentItemId,
            DecisionSnapshotId = source.DecisionSnapshotId,
            SkillId = source.SkillId,
            QuestionFamily = source.QuestionFamily,
            Representation = source.Representation,
            MathematicalComplexityScore =
                source.MathematicalComplexityScore,
            UiDifficultyBand = source.UiDifficultyBand,
            MisconceptionFocusId =
                source.MisconceptionFocusId,
            IsIndependentConfirmation =
                source.IsIndependentConfirmation,
            PresentedAtUtc = source.PresentedAtUtc,
            AnsweredAtUtc = answeredAtUtc,
            SubmittedAnswer = submittedAnswer,
            IsCorrect = isCorrect,
            Score = isCorrect ? 1m : 0m,
            Feedback = feedback,
            ResponseDurationMs = Math.Max(
                0L,
                (long)(answeredAtUtc -
                    source.PresentedAtUtc).TotalMilliseconds),
            ExposureFingerprint =
                source.ExposureFingerprint,
            SemanticIdentityKey =
                source.SemanticIdentityKey,
            IncorrectAttemptCount =
                source.IncorrectAttemptCount,
            LastIncorrectAnswer =
                source.LastIncorrectAnswer,
            LastIncorrectAtUtc =
                source.LastIncorrectAtUtc,
            RowVersion = source.RowVersion
        };

    private Task<AdaptivePracticeSessionView>
        BuildSessionViewAsync(
            AdaptivePracticeSession session,
            AdaptivePracticeTurn turn,
            AssessmentItem item,
            CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return Task.FromResult(
            new AdaptivePracticeSessionView(
                session.Id,
                session.CurriculumAdoptionId,
                session.CurriculumPedagogicalLessonId,
                session.LessonCode,
                session.PrimarySkillId,
                session.CurrentSequence,
                session.TargetQuestionCount,
                session.Status ==
                    AdaptivePracticeSessionStatus.Completed,
                session.Status ==
                    AdaptivePracticeSessionStatus.Paused,
                session.StopReason,
                new AdaptivePracticeQuestionView(
                    turn.Id,
                    turn.Sequence,
                    item.Id,
                    item.ItemType,
                    item.Difficulty,
                    item.Prompt,
                    item.GenerationFamily,
                    item.GenerationParametersJson,
                    turn.Representation,
                    turn.MathematicalComplexityScore,
                    turn.IsIndependentConfirmation,
                    turn.IncorrectAttemptCount,
                    turn.LastIncorrectAnswer)));
    }

    private static AdaptivePracticeSessionView BuildCompletedView(
        AdaptivePracticeSession session) =>
        new(
            session.Id,
            session.CurriculumAdoptionId,
            session.CurriculumPedagogicalLessonId,
            session.LessonCode,
            session.PrimarySkillId,
            session.CurrentSequence,
            session.TargetQuestionCount,
            session.Status ==
                AdaptivePracticeSessionStatus.Completed,
            session.Status ==
                AdaptivePracticeSessionStatus.Paused,
            session.StopReason,
            null);
}
