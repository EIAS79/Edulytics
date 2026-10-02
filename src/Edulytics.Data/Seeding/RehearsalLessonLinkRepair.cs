using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;
using Edulytics.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Edulytics.Data.Seeding;

/// <summary>Removes unsupported lesson labels without changing learner evidence.</summary>
public static class RehearsalLessonLinkRepair
{
    public static async Task<(int Items, int Attempts)> RunAsync(
        EdulyticsDbContext db,
        string[] schoolCodes,
        CancellationToken ct = default)
    {
        var schoolIds = await db.Schools.AsNoTracking()
            .Where(x => schoolCodes.Contains(x.SchoolCode))
            .Select(x => x.Id).ToArrayAsync(ct);
        var items = await db.AssessmentItems
            .Where(x => schoolIds.Contains(x.SchoolId)).ToArrayAsync(ct);
        var attempts = await db.PracticeAttempts
            .Where(x => schoolIds.Contains(x.SchoolId) && x.CurriculumPedagogicalLessonId != null)
            .ToArrayAsync(ct);
        var adoptions = await db.SchoolCurriculumAdoptions.AsNoTracking()
            .Where(x => schoolIds.Contains(x.SchoolId)).ToDictionaryAsync(x => x.Id, ct);
        var outcomes = await db.LearningOutcomes.AsNoTracking()
            .Where(x => schoolIds.Contains(x.SchoolId)).ToDictionaryAsync(x => x.Id, ct);
        var links = await db.AssessmentItemOutcomes.AsNoTracking()
            .Where(x => schoolIds.Contains(x.SchoolId)).ToArrayAsync(ct);
        var attemptLinks = await db.PracticeAttemptItems.AsNoTracking()
            .Where(x => schoolIds.Contains(x.SchoolId)).ToArrayAsync(ct);
        var lessonIds = items.Select(x => x.CurriculumPedagogicalLessonId)
            .Concat(attempts.Select(x => x.CurriculumPedagogicalLessonId))
            .Where(x => x.HasValue).Select(x => x!.Value).Distinct().ToArray();
        var lessons = await db.CurriculumPedagogicalLessons.AsNoTracking()
            .Where(x => lessonIds.Contains(x.Id)).ToDictionaryAsync(x => x.Id, ct);
        var mappings = (await db.CurriculumPedagogicalLessonOutcomes.AsNoTracking()
            .Where(x => lessonIds.Contains(x.PedagogicalLessonId)).ToArrayAsync(ct))
            .ToLookup(x => x.PedagogicalLessonId);
        var itemOutcomes = links.ToLookup(x => x.AssessmentItemId);
        var attemptItems = attemptLinks.ToLookup(x => x.PracticeAttemptId);
        var itemsById = items.ToDictionary(x => x.Id);

        bool Covers(Guid schoolId, Guid adoptionId, Guid lessonId, Guid[] itemIds)
        {
            if (itemIds.Length == 0 || !adoptions.TryGetValue(adoptionId, out var adoption) ||
                adoption.SchoolId != schoolId || !adoption.CurriculumLogicalLevel.HasValue ||
                !lessons.TryGetValue(lessonId, out var lesson) ||
                lesson.FrameworkVersionId != adoption.FrameworkVersionId ||
                lesson.LogicalLevelFrom > adoption.CurriculumLogicalLevel.Value ||
                lesson.LogicalLevelTo < adoption.CurriculumLogicalLevel.Value ||
                !CurriculumPathwayCompatibility.Matches(adoption.CurriculumPathway, lesson.Pathway))
                return false;

            var officialIds = mappings[lessonId]
                .Where(x => x.FrameworkVersionId == adoption.FrameworkVersionId)
                .Select(x => x.OutcomeNodeId).ToHashSet();
            return itemIds.All(id => itemsById.TryGetValue(id, out var item) &&
                item.SchoolId == schoolId && item.CurriculumAdoptionId == adoptionId &&
                itemOutcomes[id].Any() && itemOutcomes[id].All(link =>
                    link.SchoolId == schoolId && outcomes.TryGetValue(link.LearningOutcomeId, out var outcome) &&
                    outcome.SchoolId == schoolId && outcome.CurriculumAdoptionId == adoptionId &&
                    outcome.OfficialContentNodeId.HasValue && officialIds.Contains(outcome.OfficialContentNodeId.Value)));
        }

        var repairedItems = 0;
        foreach (var item in items.Where(x => x.CurriculumPedagogicalLessonId.HasValue))
        {
            if (Covers(item.SchoolId, item.CurriculumAdoptionId, item.CurriculumPedagogicalLessonId!.Value, [item.Id]))
                continue;
            item.CurriculumPedagogicalLessonId = null;
            repairedItems++;
        }

        var repairedAttempts = 0;
        foreach (var attempt in attempts)
        {
            var ids = attemptItems[attempt.Id].Select(x => x.AssessmentItemId).ToArray();
            if (Covers(attempt.SchoolId, attempt.CurriculumAdoptionId, attempt.CurriculumPedagogicalLessonId!.Value, ids))
                continue;
            attempt.CurriculumPedagogicalLessonId = null;
            repairedAttempts++;
        }

        if (repairedItems + repairedAttempts > 0)
            await db.SaveChangesAsync(ct);
        return (repairedItems, repairedAttempts);
    }
}
