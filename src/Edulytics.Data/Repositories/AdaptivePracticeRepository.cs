using Edulytics.Core.AdaptivePractice;
using Edulytics.Core.Entities;
using Edulytics.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Edulytics.Data.Repositories;

public sealed class AdaptivePracticeRepository(
    EdulyticsDbContext context)
    : IAdaptivePracticeRepository
{
    private static readonly HashSet<string> DuplicateSubmissionConstraints =
        new(StringComparer.Ordinal)
        {
            "IX_AdaptivePracticeTurns_SchoolId_SessionId_Sequence",
            "IX_AdaptiveDecisionSnapshots_SchoolId_SessionId_Sequence"
        };

    public async Task CreateSessionWithFirstTurnAsync(
        AdaptivePracticeSession session,
        AssessmentItem item,
        IReadOnlyList<AssessmentItemOutcome> itemOutcomes,
        StudentItemExposure exposure,
        AdaptiveDecisionSnapshot decision,
        AdaptivePracticeTurn turn,
        CancellationToken cancellationToken = default)
    {
        ValidateGeneratedTurn(
            session,
            item,
            exposure,
            decision,
            turn);

        // One SaveChanges call is the atomic unit. Relational EF providers
        // (including PostgreSQL/Npgsql) wrap the save in a transaction.
        Stamp(session);
        Stamp(turn);

        context.AdaptivePracticeSessions.Add(session);
        context.AssessmentItems.Add(item);
        context.AssessmentItemOutcomes.AddRange(itemOutcomes);
        context.StudentItemExposures.Add(exposure);
        context.AdaptiveDecisionSnapshots.Add(decision);
        context.AdaptivePracticeTurns.Add(turn);

        await SaveChangesWithConflictMappingAsync(
            "Adaptive Practice session creation conflicted with another request.",
            cancellationToken);
    }

    public Task<AdaptivePracticeSession?> GetSessionAsync(
        Guid schoolId,
        Guid sessionId,
        CancellationToken cancellationToken = default) =>
        context.AdaptivePracticeSessions.SingleOrDefaultAsync(
            x => x.SchoolId == schoolId && x.Id == sessionId,
            cancellationToken);

    public Task<AdaptivePracticeTurn?> GetTurnAsync(
        Guid schoolId,
        Guid sessionId,
        int sequence,
        CancellationToken cancellationToken = default) =>
        context.AdaptivePracticeTurns.SingleOrDefaultAsync(
            x =>
                x.SchoolId == schoolId &&
                x.SessionId == sessionId &&
                x.Sequence == sequence,
            cancellationToken);

    public async Task<IReadOnlyList<AdaptivePracticeTurn>> GetTurnsAsync(
        Guid schoolId,
        Guid sessionId,
        CancellationToken cancellationToken = default) =>
        await context.AdaptivePracticeTurns.AsNoTracking()
            .Where(x =>
                x.SchoolId == schoolId &&
                x.SessionId == sessionId)
            .OrderBy(x => x.Sequence)
            .ToListAsync(cancellationToken);

    public Task<AssessmentItem?> GetItemAsync(
        Guid schoolId,
        Guid assessmentItemId,
        CancellationToken cancellationToken = default) =>
        context.AssessmentItems.AsNoTracking()
            .SingleOrDefaultAsync(
                x =>
                    x.SchoolId == schoolId &&
                    x.Id == assessmentItemId,
                cancellationToken);

    public async Task<IReadOnlyList<AssessmentItem>> GetItemsAsync(
        Guid schoolId,
        IReadOnlyCollection<Guid> assessmentItemIds,
        CancellationToken cancellationToken = default)
    {
        if (assessmentItemIds.Count == 0)
            return [];

        var ids = assessmentItemIds.ToHashSet();

        return await context.AssessmentItems.AsNoTracking()
            .Where(x =>
                x.SchoolId == schoolId &&
                ids.Contains(x.Id))
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<AdaptivePracticeSession>> GetSessionsForStudentAsync(
        Guid schoolId,
        Guid studentProfileId,
        int take = 50,
        CancellationToken cancellationToken = default) =>
        await context.AdaptivePracticeSessions.AsNoTracking()
            .Where(x =>
                x.SchoolId == schoolId &&
                x.StudentProfileId == studentProfileId)
            .OrderByDescending(x => x.StartedAtUtc)
            .ThenByDescending(x => x.Id)
            .Take(Math.Clamp(take, 1, 200))
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<AdaptivePracticeSession>> GetRecentSessionsAsync(
        Guid schoolId,
        IReadOnlyCollection<Guid> studentProfileIds,
        DateTime sinceUtc,
        int take = 500,
        CancellationToken cancellationToken = default)
    {
        if (studentProfileIds.Count == 0)
            return [];

        var ids = studentProfileIds.ToHashSet();

        return await context.AdaptivePracticeSessions.AsNoTracking()
            .Where(x =>
                x.SchoolId == schoolId &&
                ids.Contains(x.StudentProfileId) &&
                x.StartedAtUtc >= sinceUtc)
            .OrderByDescending(x => x.StartedAtUtc)
            .ThenByDescending(x => x.Id)
            .Take(Math.Clamp(take, 1, 2000))
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<AdaptivePracticeTurn>> GetTurnsForSessionsAsync(
        Guid schoolId,
        IReadOnlyCollection<Guid> sessionIds,
        CancellationToken cancellationToken = default)
    {
        if (sessionIds.Count == 0)
            return [];

        var ids = sessionIds.ToHashSet();

        return await context.AdaptivePracticeTurns.AsNoTracking()
            .Where(x =>
                x.SchoolId == schoolId &&
                ids.Contains(x.SessionId))
            .OrderBy(x => x.SessionId)
            .ThenBy(x => x.Sequence)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<AdaptiveDecisionSnapshot>> GetDecisionSnapshotsForSessionsAsync(
        Guid schoolId,
        IReadOnlyCollection<Guid> sessionIds,
        CancellationToken cancellationToken = default)
    {
        if (sessionIds.Count == 0)
            return [];

        var ids = sessionIds.ToHashSet();

        return await context.AdaptiveDecisionSnapshots.AsNoTracking()
            .Where(x =>
                x.SchoolId == schoolId &&
                ids.Contains(x.SessionId))
            .OrderBy(x => x.SessionId)
            .ThenBy(x => x.Sequence)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<StudentMisconceptionState>> GetMisconceptionStatesForStudentsAsync(
        Guid schoolId,
        IReadOnlyCollection<Guid> studentProfileIds,
        CancellationToken cancellationToken = default)
    {
        if (studentProfileIds.Count == 0)
            return [];

        var ids = studentProfileIds.ToHashSet();

        return await context.StudentMisconceptionStates.AsNoTracking()
            .Where(x =>
                x.SchoolId == schoolId &&
                ids.Contains(x.StudentProfileId))
            .OrderByDescending(x => x.LastObservedAtUtc)
            .ToListAsync(cancellationToken);
    }

    public async Task CommitAnsweredTurnAsync(
        AdaptivePracticeSession session,
        AdaptivePracticeTurn answeredTurn,
        StudentMisconceptionState? misconceptionState,
        StudentRepresentationFluencyState? representationState,
        AssessmentItem? nextItem,
        IReadOnlyList<AssessmentItemOutcome> nextItemOutcomes,
        StudentItemExposure? nextExposure,
        AdaptiveDecisionSnapshot? nextDecision,
        AdaptivePracticeTurn? nextTurn,
        CancellationToken cancellationToken = default)
    {
        if (session.SchoolId != answeredTurn.SchoolId ||
            session.Id != answeredTurn.SessionId)
        {
            throw new InvalidOperationException(
                "Adaptive Practice answered turn/session scope mismatch.");
        }

        var hasNext =
            nextItem is not null ||
            nextExposure is not null ||
            nextDecision is not null ||
            nextTurn is not null ||
            nextItemOutcomes.Count > 0;

        if (hasNext)
        {
            if (nextItem is null ||
                nextExposure is null ||
                nextDecision is null ||
                nextTurn is null)
            {
                throw new InvalidOperationException(
                    "Adaptive Practice next turn persistence must be atomic.");
            }

            ValidateGeneratedTurn(
                session,
                nextItem,
                nextExposure,
                nextDecision,
                nextTurn);

            if (nextTurn.Sequence != answeredTurn.Sequence + 1)
            {
                throw new InvalidOperationException(
                    "Adaptive Practice next sequence must be contiguous.");
            }
        }

        if (misconceptionState is not null)
        {
            await ApplyMisconceptionStateAsync(
                misconceptionState,
                cancellationToken);
        }

        if (representationState is not null)
        {
            await ApplyRepresentationStateAsync(
                representationState,
                cancellationToken);
        }

        // The answered turn/session mutations, learner-state updates and
        // optional next-turn bundle are committed by the same SaveChanges transaction.
        if (nextItem is not null &&
            nextExposure is not null &&
            nextDecision is not null &&
            nextTurn is not null)
        {
            context.AssessmentItems.Add(nextItem);
            context.AssessmentItemOutcomes.AddRange(nextItemOutcomes);
            context.StudentItemExposures.Add(nextExposure);
            context.AdaptiveDecisionSnapshots.Add(nextDecision);
            context.AdaptivePracticeTurns.Add(nextTurn);
        }

        Stamp(session);
        Stamp(answeredTurn);
        if (nextTurn is not null)
            Stamp(nextTurn);

        await SaveChangesWithConflictMappingAsync(
            "Adaptive Practice answer conflicted with another submission.",
            cancellationToken);
    }

    public async Task<IReadOnlyList<StudentMisconceptionState>>
        GetMisconceptionStatesAsync(
            Guid schoolId,
            Guid studentProfileId,
            Guid curriculumAdoptionId,
            string skillId,
            CancellationToken cancellationToken = default) =>
        await context.StudentMisconceptionStates.AsNoTracking()
            .Where(x =>
                x.SchoolId == schoolId &&
                x.StudentProfileId == studentProfileId &&
                x.CurriculumAdoptionId == curriculumAdoptionId &&
                x.SkillId == skillId)
            .OrderByDescending(x => x.LastObservedAtUtc)
            .ToListAsync(cancellationToken);

    public async Task UpsertMisconceptionStateAsync(
        StudentMisconceptionState state,
        CancellationToken cancellationToken = default)
    {
        await ApplyMisconceptionStateAsync(
            state,
            cancellationToken);
        await SaveChangesWithConflictMappingAsync(
            "Adaptive misconception state conflicted with another write.",
            cancellationToken);
    }

    public async Task<IReadOnlyList<StudentRepresentationFluencyState>>
        GetRepresentationStatesAsync(
            Guid schoolId,
            Guid studentProfileId,
            string skillId,
            CancellationToken cancellationToken = default) =>
        await context.StudentRepresentationFluencyStates.AsNoTracking()
            .Where(x =>
                x.SchoolId == schoolId &&
                x.StudentProfileId == studentProfileId &&
                x.SkillId == skillId)
            .OrderBy(x => x.Representation)
            .ToListAsync(cancellationToken);

    public async Task UpsertRepresentationStateAsync(
        StudentRepresentationFluencyState state,
        CancellationToken cancellationToken = default)
    {
        await ApplyRepresentationStateAsync(
            state,
            cancellationToken);
        await SaveChangesWithConflictMappingAsync(
            "Adaptive representation state conflicted with another write.",
            cancellationToken);
    }

    private async Task ApplyMisconceptionStateAsync(
        StudentMisconceptionState state,
        CancellationToken cancellationToken)
    {
        var existing =
            await context.StudentMisconceptionStates.SingleOrDefaultAsync(
                x =>
                    x.SchoolId == state.SchoolId &&
                    x.StudentProfileId == state.StudentProfileId &&
                    x.CurriculumAdoptionId == state.CurriculumAdoptionId &&
                    x.SkillId == state.SkillId &&
                    x.MisconceptionId == state.MisconceptionId,
                cancellationToken);

        if (existing is null)
        {
            Stamp(state);
            context.StudentMisconceptionStates.Add(state);
            return;
        }

        existing.QuestionFamily = state.QuestionFamily;
        existing.Status = state.Status;
        existing.Confidence = state.Confidence;
        existing.ObservationCount = state.ObservationCount;
        existing.FirstObservedAtUtc = state.FirstObservedAtUtc;
        existing.LastObservedAtUtc = state.LastObservedAtUtc;
        existing.LastRemediationAtUtc = state.LastRemediationAtUtc;
        existing.ResolvedAtUtc = state.ResolvedAtUtc;
        existing.EngineVersion = state.EngineVersion;
        Stamp(existing);
    }

    private async Task ApplyRepresentationStateAsync(
        StudentRepresentationFluencyState state,
        CancellationToken cancellationToken)
    {
        var existing =
            await context.StudentRepresentationFluencyStates
                .SingleOrDefaultAsync(
                    x =>
                        x.SchoolId == state.SchoolId &&
                        x.StudentProfileId == state.StudentProfileId &&
                        x.SkillId == state.SkillId &&
                        x.Representation == state.Representation,
                    cancellationToken);

        if (existing is null)
        {
            Stamp(state);
            context.StudentRepresentationFluencyStates.Add(state);
            return;
        }

        existing.EvidenceCount = state.EvidenceCount;
        existing.SuccessCount = state.SuccessCount;
        existing.WeightedFluency = state.WeightedFluency;
        existing.LatestEvidenceAtUtc = state.LatestEvidenceAtUtc;
        existing.EngineVersion = state.EngineVersion;
        Stamp(existing);
    }

    public async Task AddShadowObservationIfMissingAsync(
        AdaptivePracticeShadowObservation observation,
        CancellationToken cancellationToken = default)
    {
        var exists =
            await context.AdaptivePracticeShadowObservations
                .AnyAsync(
                    x =>
                        x.SchoolId == observation.SchoolId &&
                        x.V1AttemptId == observation.V1AttemptId &&
                        x.V1AttemptItemId == observation.V1AttemptItemId,
                    cancellationToken);

        if (exists)
            return;

        context.AdaptivePracticeShadowObservations.Add(
            observation);

        await context.SaveChangesAsync(cancellationToken);
    }

    private async Task SaveChangesWithConflictMappingAsync(
        string message,
        CancellationToken cancellationToken)
    {
        try
        {
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException exception)
        {
            throw new AdaptivePracticeWriteConflictException(
                message,
                exception);
        }
        catch (DbUpdateException exception)
            when (exception.InnerException is PostgresException postgres &&
                  postgres.SqlState == PostgresErrorCodes.UniqueViolation &&
                  !string.IsNullOrWhiteSpace(postgres.ConstraintName) &&
                  DuplicateSubmissionConstraints.Contains(
                      postgres.ConstraintName))
        {
            throw new AdaptivePracticeWriteConflictException(
                message,
                exception);
        }
    }

    private static void Stamp(AdaptivePracticeSession entity) =>
        entity.RowVersion = Guid.NewGuid().ToByteArray();

    private static void Stamp(AdaptivePracticeTurn entity) =>
        entity.RowVersion = Guid.NewGuid().ToByteArray();

    private static void Stamp(StudentMisconceptionState entity) =>
        entity.RowVersion = Guid.NewGuid().ToByteArray();

    private static void Stamp(StudentRepresentationFluencyState entity) =>
        entity.RowVersion = Guid.NewGuid().ToByteArray();

    private static void ValidateGeneratedTurn(
        AdaptivePracticeSession session,
        AssessmentItem item,
        StudentItemExposure exposure,
        AdaptiveDecisionSnapshot decision,
        AdaptivePracticeTurn turn)
    {
        if (session.SchoolId == Guid.Empty ||
            session.Id == Guid.Empty ||
            item.SchoolId != session.SchoolId ||
            item.CurriculumAdoptionId !=
                session.CurriculumAdoptionId ||
            item.CurriculumPedagogicalLessonId !=
                session.CurriculumPedagogicalLessonId ||
            exposure.SchoolId != session.SchoolId ||
            exposure.StudentProfileId !=
                session.StudentProfileId ||
            exposure.AssessmentItemId != item.Id ||
            !string.Equals(
                exposure.ExposureFingerprint,
                item.ExposureFingerprint,
                StringComparison.Ordinal) ||
            decision.SchoolId != session.SchoolId ||
            decision.SessionId != session.Id ||
            turn.SchoolId != session.SchoolId ||
            turn.SessionId != session.Id ||
            turn.AssessmentItemId != item.Id ||
            turn.DecisionSnapshotId != decision.Id ||
            turn.Sequence != decision.Sequence)
        {
            throw new InvalidOperationException(
                "Adaptive Practice generated turn scope mismatch.");
        }
    }
}
