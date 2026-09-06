using Edulytics.Core.Entities;
using Edulytics.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Edulytics.Data.Repositories;

internal static class MathematicsOutcomeSemanticHintLoader
{
    public static async Task ApplyAsync(
        EdulyticsDbContext db,
        SchoolCurriculumAdoption? adoption,
        IReadOnlyList<LearningOutcome> outcomes,
        CancellationToken cancellationToken = default)
    {
        if (adoption is null ||
            !adoption.CurriculumLogicalLevel.HasValue ||
            outcomes.Count == 0)
        {
            return;
        }

        var outcomeNodeIds = outcomes
            .Where(x => x.OfficialContentNodeId.HasValue)
            .Select(x => x.OfficialContentNodeId!.Value)
            .Distinct()
            .ToArray();
        if (outcomeNodeIds.Length == 0)
            return;

        var logicalLevel = adoption.CurriculumLogicalLevel.Value;
        var pathway = adoption.CurriculumPathway;
        var lessons = await db.CurriculumPedagogicalLessons.AsNoTracking()
            .Where(x =>
                x.FrameworkVersionId == adoption.FrameworkVersionId &&
                x.LogicalLevelFrom <= logicalLevel && logicalLevel <= x.LogicalLevelTo &&
                (string.IsNullOrWhiteSpace(pathway)
                    ? x.Pathway == null || x.Pathway == string.Empty
                    : x.Pathway == pathway))
            .OrderBy(x => x.SortOrder)
            .ToListAsync(cancellationToken);
        if (lessons.Count == 0)
            return;

        var lessonIds = lessons.Select(x => x.Id).ToArray();
        var mappings = await db.CurriculumPedagogicalLessonOutcomes.AsNoTracking()
            .Where(x =>
                x.FrameworkVersionId == adoption.FrameworkVersionId &&
                lessonIds.Contains(x.PedagogicalLessonId) &&
                outcomeNodeIds.Contains(x.OutcomeNodeId))
            .OrderBy(x => x.SortOrder)
            .ToListAsync(cancellationToken);

        Apply(outcomes, lessons, mappings);
    }

    public static void Apply(
        IReadOnlyList<LearningOutcome> outcomes,
        IReadOnlyList<CurriculumPedagogicalLesson> lessons,
        IReadOnlyList<CurriculumPedagogicalLessonOutcome> mappings)
    {
        if (outcomes.Count == 0 || lessons.Count == 0 || mappings.Count == 0)
            return;

        var lessonById = lessons.ToDictionary(x => x.Id);
        var hintsByOutcomeNodeId = mappings
            .Where(x => lessonById.ContainsKey(x.PedagogicalLessonId))
            .GroupBy(x => x.OutcomeNodeId)
            .ToDictionary(
                group => group.Key,
                group => string.Join(
                    " | ",
                    group.Select(mapping => lessonById[mapping.PedagogicalLessonId])
                        .Select(lesson => $"{lesson.UnitTitle} :: {lesson.Title}")
                        .Where(value => !string.IsNullOrWhiteSpace(value))
                        .Distinct(StringComparer.OrdinalIgnoreCase)));

        foreach (var outcome in outcomes)
        {
            if (!outcome.OfficialContentNodeId.HasValue ||
                !hintsByOutcomeNodeId.TryGetValue(
                    outcome.OfficialContentNodeId.Value,
                    out var hint) ||
                string.IsNullOrWhiteSpace(hint))
            {
                continue;
            }

            outcome.GenerationSemanticHint = hint;
        }
    }
}
