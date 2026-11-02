using Edulytics.Core.Curriculum;
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

        // Pedagogical lesson titles are a semantic bridge only for curriculum
        // packs where the official wording is reference-linked and therefore not
        // reproduced in LearningOutcome.Description. Full-official-text packs
        // such as Common Core must be classified from their official text itself;
        // appending lesson titles there can broaden or contaminate the meaning of
        // an otherwise precise standard.
        var outcomeNodeScopes = await db.CurriculumPackContentNodes.AsNoTracking()
            .Where(x => outcomeNodeIds.Contains(x.Id))
            .Select(x => new { x.Id, x.FrameworkCode })
            .ToListAsync(cancellationToken);
        var semanticHintEligibleNodeIds = outcomeNodeScopes
            .Where(x => AllowsSemanticHints(x.FrameworkCode))
            .Select(x => x.Id)
            .ToHashSet();
        if (semanticHintEligibleNodeIds.Count == 0)
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
                semanticHintEligibleNodeIds.Contains(x.OutcomeNodeId))
            .OrderBy(x => x.SortOrder)
            .ToListAsync(cancellationToken);

        Apply(outcomes, lessons, mappings, semanticHintEligibleNodeIds);
    }

    public static void Apply(
        IReadOnlyList<LearningOutcome> outcomes,
        IReadOnlyList<CurriculumPedagogicalLesson> lessons,
        IReadOnlyList<CurriculumPedagogicalLessonOutcome> mappings,
        IReadOnlySet<Guid>? semanticHintEligibleOutcomeNodeIds = null)
    {
        if (outcomes.Count == 0 || lessons.Count == 0 || mappings.Count == 0)
            return;

        // Some existing repository paths already hold the lessons/mappings and
        // call this in-memory overload directly. When an explicit eligibility set
        // is absent, infer only the known OfficialSourceLinked pack identities.
        // This keeps Teacher and Student paths consistent and prevents Common Core
        // full official text from being contaminated by broad lesson titles.
        var effectiveEligibleOutcomeNodeIds = semanticHintEligibleOutcomeNodeIds ??
            outcomes
                .Where(IsReferenceLinkedOutcome)
                .Where(x => x.OfficialContentNodeId.HasValue)
                .Select(x => x.OfficialContentNodeId!.Value)
                .ToHashSet();
        if (effectiveEligibleOutcomeNodeIds.Count == 0)
            return;

        var lessonById = lessons.ToDictionary(x => x.Id);
        var hintsByOutcomeNodeId = mappings
            .Where(x =>
                lessonById.ContainsKey(x.PedagogicalLessonId) &&
                effectiveEligibleOutcomeNodeIds.Contains(x.OutcomeNodeId))
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
                !effectiveEligibleOutcomeNodeIds.Contains(outcome.OfficialContentNodeId.Value) ||
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

    private static bool AllowsSemanticHints(string? frameworkCode)
    {
        if (string.IsNullOrWhiteSpace(frameworkCode))
            return false;

        var definition = MathematicsCurriculumPackRegistry.All.SingleOrDefault(x =>
            string.Equals(x.Code, frameworkCode.Trim(), StringComparison.OrdinalIgnoreCase));
        return definition?.TextMode == CurriculumTextMode.OfficialSourceLinked;
    }

    private static bool IsReferenceLinkedOutcome(LearningOutcome outcome)
    {
        var code = outcome.Code?.Trim() ?? string.Empty;
        return code.StartsWith("CAM:", StringComparison.OrdinalIgnoreCase) ||
            code.StartsWith("PL:", StringComparison.OrdinalIgnoreCase) ||
            code.StartsWith("UAE:", StringComparison.OrdinalIgnoreCase) ||
            code.StartsWith("MAT.", StringComparison.OrdinalIgnoreCase);
    }
}
