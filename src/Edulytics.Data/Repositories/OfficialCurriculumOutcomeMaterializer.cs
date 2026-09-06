using Edulytics.Core.Entities;
using Edulytics.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Edulytics.Data.Repositories;

/// <summary>
/// Makes the verified platform curriculum usable by operational assessment/practice flows.
/// The source of truth remains CurriculumPackContentNode; this class only projects active
/// official Standard/Outcome nodes into the school-scoped LearningOutcome model required by
/// assessment, mastery and evidence foreign keys.
/// </summary>
internal static class OfficialCurriculumOutcomeMaterializer
{
    public static async Task EnsureAsync(
        EdulyticsDbContext db,
        SchoolCurriculumAdoption adoption,
        CancellationToken cancellationToken = default)
    {
        if (!adoption.IsActive ||
            !adoption.CurriculumLogicalLevel.HasValue ||
            string.IsNullOrWhiteSpace(adoption.CurriculumLevelKey))
        {
            return;
        }

        var logicalLevel = adoption.CurriculumLogicalLevel.Value;
        var pathway = Normalize(adoption.CurriculumPathway);

        var officialNodes = await db.CurriculumPackContentNodes
            .AsNoTracking()
            .Where(x =>
                x.FrameworkVersionId == adoption.FrameworkVersionId &&
                x.IsOfficial &&
                x.IsActive &&
                (x.NodeKind == "Standard" || x.NodeKind == "Outcome") &&
                x.LogicalLevelFrom <= logicalLevel &&
                x.LogicalLevelTo >= logicalLevel &&
                (pathway == null
                    ? x.Pathway == null || x.Pathway == string.Empty
                    : x.Pathway == pathway))
            .OrderBy(x => x.SortOrder)
            .ThenBy(x => x.Code)
            .ToListAsync(cancellationToken);

        if (officialNodes.Count == 0)
            return;

        // A code represents one official assessable outcome in a curriculum level. De-duplicate
        // defensively without creating synthetic identifiers or text.
        officialNodes = officialNodes
            .GroupBy(x => DisplayCode(x.Code), StringComparer.Ordinal)
            .Select(x => x.OrderBy(n => n.SortOrder).ThenBy(n => n.Id).First())
            .OrderBy(x => x.SortOrder)
            .ThenBy(x => x.Code)
            .ToList();

        var parentIds = officialNodes
            .Where(x => x.ParentId.HasValue)
            .Select(x => x.ParentId!.Value)
            .Distinct()
            .ToArray();
        var parents = await db.CurriculumPackContentNodes
            .AsNoTracking()
            .Where(x => parentIds.Contains(x.Id))
            .ToDictionaryAsync(x => x.Id, cancellationToken);

        var existingTopics = await db.CurriculumTopics
            .Where(x =>
                x.SchoolId == adoption.SchoolId &&
                x.AcademicProgramId == adoption.AcademicProgramId &&
                x.FrameworkVersionId == adoption.FrameworkVersionId &&
                x.SubjectId == adoption.SubjectId &&
                x.GradeLevelId == adoption.GradeLevelId &&
                (x.CurriculumAdoptionId == adoption.Id || x.CurriculumAdoptionId == null))
            .OrderBy(x => x.Order)
            .ToListAsync(cancellationToken);

        var existingOutcomes = await db.LearningOutcomes
            .Where(x =>
                x.SchoolId == adoption.SchoolId &&
                x.AcademicProgramId == adoption.AcademicProgramId &&
                x.FrameworkVersionId == adoption.FrameworkVersionId &&
                x.SubjectId == adoption.SubjectId &&
                x.GradeLevelId == adoption.GradeLevelId &&
                (x.CurriculumAdoptionId == adoption.Id || x.CurriculumAdoptionId == null))
            .OrderBy(x => x.Order)
            .ToListAsync(cancellationToken);

        var topicByName = existingTopics
            .GroupBy(x => NormalizeKey(x.Name), StringComparer.Ordinal)
            .ToDictionary(x => x.Key, x => x.First(), StringComparer.Ordinal);
        var nextTopicOrder = existingTopics.Count == 0 ? 1 : existingTopics.Max(x => x.Order) + 1;

        var nodeGroups = officialNodes
            .GroupBy(node => GroupName(node, parents, adoption))
            .OrderBy(group => group.Min(x => x.SortOrder))
            .ThenBy(group => group.Key, StringComparer.Ordinal)
            .ToArray();

        foreach (var nodeGroup in nodeGroups)
        {
            var topicKey = NormalizeKey(nodeGroup.Key);
            if (!topicByName.TryGetValue(topicKey, out var topic))
            {
                topic = new CurriculumTopic
                {
                    Id = Guid.NewGuid(),
                    SchoolId = adoption.SchoolId,
                    AcademicProgramId = adoption.AcademicProgramId,
                    FrameworkVersionId = adoption.FrameworkVersionId,
                    SubjectId = adoption.SubjectId,
                    GradeLevelId = adoption.GradeLevelId,
                    CurriculumAdoptionId = adoption.Id,
                    Name = Compact(nodeGroup.Key, 200),
                    Order = nextTopicOrder++
                };
                db.CurriculumTopics.Add(topic);
                existingTopics.Add(topic);
                topicByName[topicKey] = topic;
            }

            var nextOutcomeOrder = existingOutcomes
                .Where(x => x.TopicId == topic.Id)
                .Select(x => x.Order)
                .DefaultIfEmpty(0)
                .Max() + 1;

            foreach (var node in nodeGroup.OrderBy(x => x.SortOrder).ThenBy(x => x.Code))
            {
                var code = DisplayCode(node.Code);
                var current = existingOutcomes.FirstOrDefault(x =>
                    string.Equals(x.Code, code, StringComparison.Ordinal));

                if (current is not null)
                {
                    // Compatibility backfill: old rows created before explicit curriculum adoption
                    // keep their IDs/mappings, but become traceable to the authoritative adoption/node.
                    if (!current.CurriculumAdoptionId.HasValue)
                        current.CurriculumAdoptionId = adoption.Id;
                    if (!current.OfficialContentNodeId.HasValue)
                        current.OfficialContentNodeId = node.Id;
                    continue;
                }

                var outcome = new LearningOutcome
                {
                    Id = Guid.NewGuid(),
                    SchoolId = adoption.SchoolId,
                    AcademicProgramId = adoption.AcademicProgramId,
                    FrameworkVersionId = adoption.FrameworkVersionId,
                    SubjectId = adoption.SubjectId,
                    GradeLevelId = adoption.GradeLevelId,
                    CurriculumAdoptionId = adoption.Id,
                    TopicId = topic.Id,
                    OfficialContentNodeId = node.Id,
                    Code = code,
                    Description = Compact(
                        node.OfficialText ?? node.AuthorDescription ?? node.Title,
                        1000),
                    Weight = 1m,
                    Order = nextOutcomeOrder++
                };
                db.LearningOutcomes.Add(outcome);
                existingOutcomes.Add(outcome);
            }
        }

        if (!db.ChangeTracker.HasChanges())
            return;

        await db.SaveChangesAsync(cancellationToken);
    }

    private static string GroupName(
        CurriculumPackContentNode node,
        IReadOnlyDictionary<Guid, CurriculumPackContentNode> parents,
        SchoolCurriculumAdoption adoption)
    {
        if (node.ParentId.HasValue &&
            parents.TryGetValue(node.ParentId.Value, out var parent) &&
            !string.IsNullOrWhiteSpace(parent.Title))
        {
            return Compact(parent.Title, 200);
        }

        if (!string.IsNullOrWhiteSpace(node.NativeLevel))
            return Compact(node.NativeLevel, 200);

        return Compact(
            adoption.CurriculumLevelLabel ?? adoption.CurriculumLevelKey ?? "Mathematics",
            200);
    }

    private static string DisplayCode(string value) =>
        value
            .Replace("UAE:STD:", string.Empty, StringComparison.Ordinal)
            .Replace("UK:STD:", string.Empty, StringComparison.Ordinal)
            .Replace("CCSS:", string.Empty, StringComparison.Ordinal)
            .Replace("PL:REQ:", string.Empty, StringComparison.Ordinal)
            .Trim();

    private static string? Normalize(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value.Trim();

    private static string NormalizeKey(string value) =>
        string.Join(" ", value.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries))
            .ToUpperInvariant();

    private static string Compact(string value, int maximumLength)
    {
        var compact = string.Join(
            " ",
            (value ?? string.Empty).Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries));
        if (compact.Length <= maximumLength)
            return compact;
        return compact[..(maximumLength - 1)].TrimEnd() + "…";
    }
}
