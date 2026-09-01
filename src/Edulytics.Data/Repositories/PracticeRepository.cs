
using Edulytics.Core.Entities;
using Edulytics.Core.Practice;
using Edulytics.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Edulytics.Data.Repositories;

public sealed class PracticeRepository(EdulyticsDbContext context) : IPracticeRepository
{
    public Task<StudentProfile?> FindStudentByUserIdAsync(Guid userId, CancellationToken cancellationToken = default) =>
        context.StudentProfiles.AsNoTracking()
            .SingleOrDefaultAsync(x => x.UserId == userId && !x.IsArchived, cancellationToken);

    public Task<bool> IsEnrolledInAdoptionAsync(
        Guid schoolId,
        Guid studentProfileId,
        Guid curriculumAdoptionId,
        CancellationToken cancellationToken = default) =>
        context.StudentEnrollments.AsNoTracking()
            .Where(x => x.SchoolId == schoolId && x.StudentProfileId == studentProfileId)
            .Join(
                context.ClassGroups.AsNoTracking().Where(x => x.SchoolId == schoolId),
                enrollment => enrollment.ClassGroupId,
                classGroup => classGroup.Id,
                (_, classGroup) => classGroup)
            .AnyAsync(x => x.CurriculumAdoptionId == curriculumAdoptionId, cancellationToken);

    public async Task<IReadOnlyList<AssessmentItem>> ListItemsAsync(
        Guid schoolId,
        Guid curriculumAdoptionId,
        Guid? lessonId,
        CancellationToken cancellationToken = default) =>
        await context.AssessmentItems.AsNoTracking()
            .Where(x =>
                x.SchoolId == schoolId &&
                x.CurriculumAdoptionId == curriculumAdoptionId &&
                (!lessonId.HasValue || x.CurriculumPedagogicalLessonId == lessonId))
            .OrderBy(x => x.Difficulty)
            .ThenBy(x => x.CreatedAtUtc)
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<AssessmentItem>> GetItemsAsync(
        Guid schoolId,
        IReadOnlyCollection<Guid> itemIds,
        CancellationToken cancellationToken = default) =>
        await context.AssessmentItems.AsNoTracking()
            .Where(x => x.SchoolId == schoolId && itemIds.Contains(x.Id))
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyDictionary<Guid, IReadOnlyList<Guid>>> GetOutcomeIdsAsync(
        Guid schoolId,
        IReadOnlyCollection<Guid> itemIds,
        CancellationToken cancellationToken = default)
    {
        var rows = await context.AssessmentItemOutcomes.AsNoTracking()
            .Where(x => x.SchoolId == schoolId && itemIds.Contains(x.AssessmentItemId))
            .Select(x => new { x.AssessmentItemId, x.LearningOutcomeId })
            .ToListAsync(cancellationToken);

        return rows
            .GroupBy(x => x.AssessmentItemId)
            .ToDictionary(
                x => x.Key,
                x => (IReadOnlyList<Guid>)x.Select(y => y.LearningOutcomeId).Distinct().ToArray());
    }

    public async Task<bool> OutcomesBelongToAdoptionAsync(
        Guid schoolId,
        Guid curriculumAdoptionId,
        IReadOnlyCollection<Guid> outcomeIds,
        CancellationToken cancellationToken = default)
    {
        var distinct = outcomeIds.Distinct().ToArray();
        if (distinct.Length == 0)
        {
            return false;
        }

        var count = await context.LearningOutcomes.AsNoTracking()
            .CountAsync(
                x =>
                    x.SchoolId == schoolId &&
                    x.CurriculumAdoptionId == curriculumAdoptionId &&
                    distinct.Contains(x.Id),
                cancellationToken);

        return count == distinct.Length;
    }

    public async Task AddAttemptAsync(
        PracticeAttempt attempt,
        IReadOnlyCollection<PracticeAttemptItem> items,
        IReadOnlyCollection<StudentItemExposure> exposures,
        CancellationToken cancellationToken = default)
    {
        context.PracticeAttempts.Add(attempt);
        context.PracticeAttemptItems.AddRange(items);
        context.StudentItemExposures.AddRange(exposures);
        await context.SaveChangesAsync(cancellationToken);
    }

    public Task<PracticeAttempt?> GetAttemptAsync(
        Guid schoolId,
        Guid attemptId,
        CancellationToken cancellationToken = default) =>
        context.PracticeAttempts.SingleOrDefaultAsync(
            x => x.SchoolId == schoolId && x.Id == attemptId,
            cancellationToken);

    public async Task<IReadOnlyList<PracticeAttemptItem>> GetAttemptItemsAsync(
        Guid schoolId,
        Guid attemptId,
        CancellationToken cancellationToken = default) =>
        await context.PracticeAttemptItems.AsNoTracking()
            .Where(x => x.SchoolId == schoolId && x.PracticeAttemptId == attemptId)
            .OrderBy(x => x.Order)
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<PracticeResponse>> GetResponsesAsync(
        Guid schoolId,
        Guid attemptId,
        CancellationToken cancellationToken = default)
    {
        var itemIds = context.PracticeAttemptItems.AsNoTracking()
            .Where(x => x.SchoolId == schoolId && x.PracticeAttemptId == attemptId)
            .Select(x => x.Id);

        return await context.PracticeResponses.AsNoTracking()
            .Where(x => x.SchoolId == schoolId && itemIds.Contains(x.PracticeAttemptItemId))
            .ToListAsync(cancellationToken);
    }

    public async Task SaveResponseAsync(
        PracticeResponse response,
        CancellationToken cancellationToken = default)
    {
        var existing = await context.PracticeResponses.SingleOrDefaultAsync(
            x => x.SchoolId == response.SchoolId && x.PracticeAttemptItemId == response.PracticeAttemptItemId,
            cancellationToken);

        if (existing is null)
        {
            context.PracticeResponses.Add(response);
        }
        else
        {
            existing.Answer = response.Answer;
            existing.IsCorrect = response.IsCorrect;
            existing.Score = response.Score;
            existing.Feedback = response.Feedback;
            existing.AnsweredAtUtc = response.AnsweredAtUtc;
        }

        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task CompleteAttemptAsync(
        PracticeAttempt attempt,
        IReadOnlyCollection<LearningEvidence> evidence,
        CancellationToken cancellationToken = default)
    {
        context.LearningEvidence.AddRange(evidence);
        await context.SaveChangesAsync(cancellationToken);
    }
}
