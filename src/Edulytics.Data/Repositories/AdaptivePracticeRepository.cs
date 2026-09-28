using Edulytics.Core.AdaptivePractice;
using Edulytics.Core.Entities;
using Edulytics.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Edulytics.Data.Repositories;

public sealed class AdaptivePracticeRepository(
    EdulyticsDbContext context)
    : IAdaptivePracticeRepository
{
    public async Task AddSessionAsync(
        AdaptivePracticeSession session,
        CancellationToken cancellationToken = default)
    {
        context.AdaptivePracticeSessions.Add(session);
        await context.SaveChangesAsync(cancellationToken);
    }

    public Task<AdaptivePracticeSession?> GetSessionAsync(
        Guid schoolId,
        Guid sessionId,
        CancellationToken cancellationToken = default) =>
        context.AdaptivePracticeSessions.SingleOrDefaultAsync(
            x => x.SchoolId == schoolId && x.Id == sessionId,
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

    public async Task AddDecisionAndTurnAsync(
        AdaptiveDecisionSnapshot decision,
        AdaptivePracticeTurn turn,
        CancellationToken cancellationToken = default)
    {
        if (decision.SchoolId != turn.SchoolId ||
            decision.SessionId != turn.SessionId ||
            decision.Sequence != turn.Sequence ||
            decision.Id != turn.DecisionSnapshotId)
        {
            throw new InvalidOperationException(
                "Adaptive Practice decision/turn scope mismatch.");
        }

        context.AdaptiveDecisionSnapshots.Add(decision);
        context.AdaptivePracticeTurns.Add(turn);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task SaveAnsweredTurnAsync(
        AdaptivePracticeTurn turn,
        AdaptivePracticeSession session,
        CancellationToken cancellationToken = default)
    {
        if (turn.SchoolId != session.SchoolId ||
            turn.SessionId != session.Id)
        {
            throw new InvalidOperationException(
                "Adaptive Practice answered turn/session scope mismatch.");
        }

        await context.SaveChangesAsync(cancellationToken);
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
            context.StudentMisconceptionStates.Add(state);
        }
        else
        {
            existing.QuestionFamily = state.QuestionFamily;
            existing.Status = state.Status;
            existing.Confidence = state.Confidence;
            existing.ObservationCount = state.ObservationCount;
            existing.FirstObservedAtUtc = state.FirstObservedAtUtc;
            existing.LastObservedAtUtc = state.LastObservedAtUtc;
            existing.LastRemediationAtUtc = state.LastRemediationAtUtc;
            existing.ResolvedAtUtc = state.ResolvedAtUtc;
            existing.EngineVersion = state.EngineVersion;
        }

        await context.SaveChangesAsync(cancellationToken);
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
            context.StudentRepresentationFluencyStates.Add(state);
        }
        else
        {
            existing.EvidenceCount = state.EvidenceCount;
            existing.SuccessCount = state.SuccessCount;
            existing.WeightedFluency = state.WeightedFluency;
            existing.LatestEvidenceAtUtc = state.LatestEvidenceAtUtc;
            existing.EngineVersion = state.EngineVersion;
        }

        await context.SaveChangesAsync(cancellationToken);
    }
}
