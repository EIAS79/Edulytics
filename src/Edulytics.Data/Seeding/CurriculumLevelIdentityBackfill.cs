using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;
using Edulytics.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Edulytics.Data.Seeding;

public sealed record CurriculumLevelIdentityBackfillResult(
    int AdoptionRowsResolved,
    int ClassRowsResolved,
    int TopicRowsResolved,
    int OutcomeRowsResolved,
    int UnresolvedAdoptions,
    int UnresolvedClasses,
    int UnresolvedTopics,
    int UnresolvedOutcomes);

/// <summary>
/// Compatibility-only, deterministic backfill for records created before
/// explicit curriculum-level identity existed. Every assignment fails closed
/// when more than one valid target exists. This service is idempotent.
/// </summary>
public sealed class CurriculumLevelIdentityBackfill
{
    private readonly EdulyticsDbContext _db;

    public CurriculumLevelIdentityBackfill(EdulyticsDbContext db)
    {
        _db = db;
    }

    public async Task<CurriculumLevelIdentityBackfillResult> RunAsync(
        CancellationToken cancellationToken = default)
    {
        var adoptions = await _db.SchoolCurriculumAdoptions
            .ToArrayAsync(cancellationToken);
        var grades = await _db.GradeLevels
            .ToArrayAsync(cancellationToken);
        var versions = await _db.CurriculumFrameworkVersions
            .ToArrayAsync(cancellationToken);
        var frameworks = await _db.CurriculumFrameworks
            .ToArrayAsync(cancellationToken);
        var classes = await _db.ClassGroups
            .ToArrayAsync(cancellationToken);
        var topics = await _db.CurriculumTopics
            .ToArrayAsync(cancellationToken);
        var outcomes = await _db.LearningOutcomes
            .ToArrayAsync(cancellationToken);

        var gradeByKey = grades.ToDictionary(x => (x.SchoolId, x.Id));
        var versionById = versions.ToDictionary(x => x.Id);
        var frameworkById = frameworks.ToDictionary(x => x.Id);

        var adoptionCandidates = new List<AdoptionResolution>();

        foreach (var adoption in adoptions.Where(
                     x => string.IsNullOrWhiteSpace(x.CurriculumLevelKey)))
        {
            if (!gradeByKey.TryGetValue(
                    (adoption.SchoolId, adoption.GradeLevelId),
                    out var grade) ||
                !versionById.TryGetValue(
                    adoption.FrameworkVersionId,
                    out var version) ||
                !frameworkById.TryGetValue(
                    version.FrameworkId,
                    out var framework))
            {
                continue;
            }

            var level = CurriculumLevelIdentityRegistry.ResolveLegacy(
                framework.Code,
                grade.Name,
                grade.Order);

            if (level is null)
                continue;

            adoptionCandidates.Add(new AdoptionResolution(adoption, level));
        }

        var existingAdoptionKeys = adoptions
            .Where(x => !string.IsNullOrWhiteSpace(x.CurriculumLevelKey))
            .Select(x => new AdoptionIdentityKey(
                x.SchoolId,
                x.AcademicYearId,
                x.AcademicProgramId,
                x.SubjectId,
                x.CurriculumLevelKey!))
            .ToHashSet();

        var adoptionRowsResolved = 0;
        foreach (var group in adoptionCandidates.GroupBy(x =>
                     new AdoptionIdentityKey(
                         x.Adoption.SchoolId,
                         x.Adoption.AcademicYearId,
                         x.Adoption.AcademicProgramId,
                         x.Adoption.SubjectId,
                         x.Level.Key)))
        {
            // Multiple legacy rows resolving to the same explicit identity are
            // ambiguous. Existing explicit identity also wins over legacy data.
            if (group.Count() != 1 || existingAdoptionKeys.Contains(group.Key))
                continue;

            var resolution = group.Single();
            var adoption = resolution.Adoption;
            var level = resolution.Level;

            adoption.CurriculumLevelKey = level.Key;
            adoption.CurriculumLogicalLevel = level.LogicalLevel;
            adoption.CurriculumLevelLabel = level.Label;
            adoption.CurriculumStage = level.Stage;
            adoption.CurriculumPathway = level.Pathway;
            adoption.UpdatedAtUtc = DateTime.UtcNow;
            adoptionRowsResolved++;
        }

        var resolvedAdoptions = adoptions
            .Where(x =>
                !string.IsNullOrWhiteSpace(x.CurriculumLevelKey) &&
                x.CurriculumLogicalLevel.HasValue)
            .ToArray();
        var adoptionById = resolvedAdoptions.ToDictionary(x => x.Id);

        var classProposals = new List<ClassResolution>();
        foreach (var classGroup in classes)
        {
            SchoolCurriculumAdoption? target = null;

            if (classGroup.CurriculumAdoptionId.HasValue)
            {
                adoptionById.TryGetValue(
                    classGroup.CurriculumAdoptionId.Value,
                    out target);
            }
            else
            {
                var matches = resolvedAdoptions
                    .Where(x =>
                        x.SchoolId == classGroup.SchoolId &&
                        x.AcademicProgramId == classGroup.AcademicProgramId &&
                        x.GradeLevelId == classGroup.GradeLevelId &&
                        x.IsActive &&
                        x.IsPrimary &&
                        (!x.AcademicYearId.HasValue ||
                         x.AcademicYearId.Value == classGroup.AcademicYearId))
                    .ToArray();

                if (matches.Length == 1)
                    target = matches[0];
            }

            if (target is null || target.SchoolId != classGroup.SchoolId)
                continue;

            classProposals.Add(new ClassResolution(
                classGroup,
                target,
                NormalizeName(classGroup.Name)));
        }

        var classIdentityCounts = classProposals
            .GroupBy(x => new ClassIdentityKey(
                x.ClassGroup.SchoolId,
                x.ClassGroup.AcademicYearId,
                x.Adoption.Id,
                x.NormalizedName))
            .ToDictionary(x => x.Key, x => x.Count());

        var classRowsResolved = 0;
        foreach (var proposal in classProposals)
        {
            var key = new ClassIdentityKey(
                proposal.ClassGroup.SchoolId,
                proposal.ClassGroup.AcademicYearId,
                proposal.Adoption.Id,
                proposal.NormalizedName);

            if (classIdentityCounts[key] != 1)
                continue;

            var changed = false;
            if (!proposal.ClassGroup.CurriculumAdoptionId.HasValue)
            {
                proposal.ClassGroup.CurriculumAdoptionId = proposal.Adoption.Id;
                changed = true;
            }

            if (!string.Equals(
                    proposal.ClassGroup.NormalizedName,
                    proposal.NormalizedName,
                    StringComparison.Ordinal))
            {
                proposal.ClassGroup.NormalizedName = proposal.NormalizedName;
                changed = true;
            }

            if (changed)
                classRowsResolved++;
        }

        var topicProposals = new List<TopicResolution>();
        foreach (var topic in topics)
        {
            SchoolCurriculumAdoption? target = null;

            if (topic.CurriculumAdoptionId.HasValue)
            {
                adoptionById.TryGetValue(
                    topic.CurriculumAdoptionId.Value,
                    out target);
            }
            else
            {
                var matches = resolvedAdoptions
                    .Where(x =>
                        x.SchoolId == topic.SchoolId &&
                        x.AcademicProgramId == topic.AcademicProgramId &&
                        x.GradeLevelId == topic.GradeLevelId &&
                        x.SubjectId == topic.SubjectId &&
                        x.FrameworkVersionId == topic.FrameworkVersionId &&
                        x.IsActive &&
                        x.IsPrimary)
                    .ToArray();

                // Topic rows do not carry AcademicYearId. If the same legacy
                // scope exists in more than one year, the row stays unresolved.
                if (matches.Length == 1)
                    target = matches[0];
            }

            if (target is null || target.SchoolId != topic.SchoolId)
                continue;

            topicProposals.Add(new TopicResolution(topic, target));
        }

        var topicNameCounts = topicProposals
            .GroupBy(x => new TopicNameIdentityKey(
                x.Topic.SchoolId,
                x.Adoption.Id,
                x.Topic.Name))
            .ToDictionary(x => x.Key, x => x.Count());
        var topicOrderCounts = topicProposals
            .GroupBy(x => new TopicOrderIdentityKey(
                x.Topic.SchoolId,
                x.Adoption.Id,
                x.Topic.Order))
            .ToDictionary(x => x.Key, x => x.Count());

        var topicRowsResolved = 0;
        foreach (var proposal in topicProposals)
        {
            var nameKey = new TopicNameIdentityKey(
                proposal.Topic.SchoolId,
                proposal.Adoption.Id,
                proposal.Topic.Name);
            var orderKey = new TopicOrderIdentityKey(
                proposal.Topic.SchoolId,
                proposal.Adoption.Id,
                proposal.Topic.Order);

            if (topicNameCounts[nameKey] != 1 || topicOrderCounts[orderKey] != 1)
                continue;

            if (!proposal.Topic.CurriculumAdoptionId.HasValue)
            {
                proposal.Topic.CurriculumAdoptionId = proposal.Adoption.Id;
                topicRowsResolved++;
            }
        }

        var topicById = topics.ToDictionary(x => x.Id);
        var outcomeProposals = new List<OutcomeResolution>();

        foreach (var outcome in outcomes)
        {
            Guid? targetAdoptionId = outcome.CurriculumAdoptionId;

            if (!targetAdoptionId.HasValue &&
                topicById.TryGetValue(outcome.TopicId, out var topic) &&
                topic.SchoolId == outcome.SchoolId &&
                topic.CurriculumAdoptionId.HasValue)
            {
                targetAdoptionId = topic.CurriculumAdoptionId.Value;
            }

            if (!targetAdoptionId.HasValue ||
                !adoptionById.TryGetValue(targetAdoptionId.Value, out var adoption) ||
                adoption.SchoolId != outcome.SchoolId)
            {
                continue;
            }

            outcomeProposals.Add(new OutcomeResolution(outcome, adoption));
        }

        var outcomeCodeCounts = outcomeProposals
            .GroupBy(x => new OutcomeCodeIdentityKey(
                x.Outcome.SchoolId,
                x.Adoption.Id,
                x.Outcome.Code))
            .ToDictionary(x => x.Key, x => x.Count());

        var outcomeRowsResolved = 0;
        foreach (var proposal in outcomeProposals)
        {
            var key = new OutcomeCodeIdentityKey(
                proposal.Outcome.SchoolId,
                proposal.Adoption.Id,
                proposal.Outcome.Code);

            if (outcomeCodeCounts[key] != 1)
                continue;

            if (!proposal.Outcome.CurriculumAdoptionId.HasValue)
            {
                proposal.Outcome.CurriculumAdoptionId = proposal.Adoption.Id;
                outcomeRowsResolved++;
            }
        }

        if (_db.ChangeTracker.HasChanges())
            await _db.SaveChangesAsync(cancellationToken);

        return new CurriculumLevelIdentityBackfillResult(
            adoptionRowsResolved,
            classRowsResolved,
            topicRowsResolved,
            outcomeRowsResolved,
            adoptions.Count(x => string.IsNullOrWhiteSpace(x.CurriculumLevelKey)),
            classes.Count(x => !x.CurriculumAdoptionId.HasValue),
            topics.Count(x => !x.CurriculumAdoptionId.HasValue),
            outcomes.Count(x => !x.CurriculumAdoptionId.HasValue));
    }

    private static string NormalizeName(string value) =>
        string.Join(
                " ",
                value.Split(
                    (char[]?)null,
                    StringSplitOptions.RemoveEmptyEntries))
            .ToUpperInvariant();

    private sealed record AdoptionResolution(
        SchoolCurriculumAdoption Adoption,
        CurriculumLevelIdentity Level);

    private sealed record ClassResolution(
        ClassGroup ClassGroup,
        SchoolCurriculumAdoption Adoption,
        string NormalizedName);

    private sealed record TopicResolution(
        CurriculumTopic Topic,
        SchoolCurriculumAdoption Adoption);

    private sealed record OutcomeResolution(
        LearningOutcome Outcome,
        SchoolCurriculumAdoption Adoption);

    private readonly record struct AdoptionIdentityKey(
        Guid SchoolId,
        Guid? AcademicYearId,
        Guid AcademicProgramId,
        Guid SubjectId,
        string CurriculumLevelKey);

    private readonly record struct ClassIdentityKey(
        Guid SchoolId,
        Guid AcademicYearId,
        Guid CurriculumAdoptionId,
        string NormalizedName);

    private readonly record struct TopicNameIdentityKey(
        Guid SchoolId,
        Guid CurriculumAdoptionId,
        string Name);

    private readonly record struct TopicOrderIdentityKey(
        Guid SchoolId,
        Guid CurriculumAdoptionId,
        int Order);

    private readonly record struct OutcomeCodeIdentityKey(
        Guid SchoolId,
        Guid CurriculumAdoptionId,
        string Code);
}
