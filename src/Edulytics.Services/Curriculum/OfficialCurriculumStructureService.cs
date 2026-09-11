using Edulytics.Core.Entities;
using Edulytics.Core.Enums;
using Edulytics.Core.Interfaces;
using Edulytics.Core.Users;

namespace Edulytics.Services.Curriculum;

public sealed record OfficialCurriculumStructureResult(
    bool Succeeded,
    int TopicsCreated,
    int OutcomesCreated,
    string? Error)
{
    public static OfficialCurriculumStructureResult Success(int topics, int outcomes) =>
        new(true, topics, outcomes, null);

    public static OfficialCurriculumStructureResult Failure(string error) =>
        new(false, 0, 0, error);
}

public interface IOfficialCurriculumStructureService
{
    Task<OfficialCurriculumStructureResult> InitializeAsync(
        Guid actorUserId,
        Guid curriculumAdoptionId,
        CancellationToken cancellationToken = default);
}

public sealed class OfficialCurriculumStructureService(
    ICurriculumRepository curriculum,
    ISchoolRepository schools,
    ISchoolUserRepository users) : IOfficialCurriculumStructureService
{
    public async Task<OfficialCurriculumStructureResult> InitializeAsync(
        Guid actorUserId,
        Guid curriculumAdoptionId,
        CancellationToken cancellationToken = default)
    {
        var actor = await users.GetActorAsync(actorUserId, cancellationToken);
        if (actor is null ||
            !actor.IsActive ||
            actor.IsLocked ||
            !actor.SchoolId.HasValue ||
            !actor.Roles.Contains(RoleNames.SubjectSupervisor, StringComparer.Ordinal))
        {
            return OfficialCurriculumStructureResult.Failure("AccessDenied");
        }

        var school = await schools.GetByIdAsync(actor.SchoolId.Value, cancellationToken);
        if (school is null || school.Status != SchoolStatus.Active)
            return OfficialCurriculumStructureResult.Failure("SchoolNotActive");

        var contexts = await curriculum.GetAdoptedCurriculumContextsAsync(school.Id, cancellationToken);
        var adoption = contexts.SingleOrDefault(x =>
            x.AdoptionId == curriculumAdoptionId &&
            x.CurriculumLogicalLevel.HasValue);
        if (adoption is null)
            return OfficialCurriculumStructureResult.Failure("CurriculumAdoptionNotFound");

        var official = await curriculum.GetOfficialOutcomeSourcesAsync(
            adoption.FrameworkVersionId,
            adoption.CurriculumLogicalLevel!.Value,
            adoption.CurriculumPathway,
            cancellationToken);
        if (official.Count == 0)
            return OfficialCurriculumStructureResult.Failure("OfficialStructureUnavailable");

        var snapshot = await curriculum.GetSnapshotAsync(school.Id, cancellationToken);
        var existingTopics = snapshot.Topics
            .Where(x => x.CurriculumAdoptionId == adoption.AdoptionId)
            .OrderBy(x => x.Order)
            .ToList();
        var existingCodes = snapshot.Outcomes
            .Where(x => x.CurriculumAdoptionId == adoption.AdoptionId)
            .Select(x => x.Code)
            .ToHashSet(StringComparer.Ordinal);
        var nextOutcomeOrder = snapshot.Outcomes
            .Where(x => x.CurriculumAdoptionId == adoption.AdoptionId)
            .GroupBy(x => x.TopicId)
            .ToDictionary(x => x.Key, x => x.Max(y => y.Order) + 1);

        var groups = official
            .GroupBy(x => CleanGroup(x.GroupLabel))
            .OrderBy(x => x.Min(y => y.SortOrder))
            .ToArray();

        var nextTopicOrder = existingTopics.Count == 0 ? 1 : existingTopics.Max(x => x.Order) + 1;
        var topicsCreated = 0;
        var outcomesCreated = 0;

        foreach (var group in groups)
        {
            var topicName = group.Key;
            var topic = existingTopics.FirstOrDefault(x =>
                string.Equals(x.Name, topicName, StringComparison.OrdinalIgnoreCase));

            if (topic is null)
            {
                topic = new CurriculumTopic
                {
                    Id = Guid.NewGuid(),
                    SchoolId = school.Id,
                    AcademicProgramId = adoption.AcademicProgramId,
                    FrameworkVersionId = adoption.FrameworkVersionId,
                    SubjectId = adoption.SubjectId,
                    GradeLevelId = adoption.GradeLevelId,
                    CurriculumAdoptionId = adoption.AdoptionId,
                    Name = topicName,
                    Order = nextTopicOrder++
                };
                await curriculum.AddTopicAsync(topic, cancellationToken);
                existingTopics.Add(topic);
                nextOutcomeOrder[topic.Id] = 1;
                topicsCreated++;
            }

            if (!nextOutcomeOrder.TryGetValue(topic.Id, out var outcomeOrder))
                outcomeOrder = 1;

            foreach (var source in group.OrderBy(x => x.SortOrder))
            {
                if (!existingCodes.Add(source.Code))
                    continue;

                var outcome = new LearningOutcome
                {
                    Id = Guid.NewGuid(),
                    SchoolId = school.Id,
                    AcademicProgramId = adoption.AcademicProgramId,
                    FrameworkVersionId = adoption.FrameworkVersionId,
                    SubjectId = adoption.SubjectId,
                    GradeLevelId = adoption.GradeLevelId,
                    CurriculumAdoptionId = adoption.AdoptionId,
                    TopicId = topic.Id,
                    OfficialContentNodeId = source.ContentNodeId,
                    Code = source.Code,
                    Description = source.Description,
                    Weight = 1m,
                    Order = outcomeOrder++
                };
                await curriculum.AddOutcomeAsync(outcome, cancellationToken);
                outcomesCreated++;
            }

            nextOutcomeOrder[topic.Id] = outcomeOrder;
        }

        if (topicsCreated == 0 && outcomesCreated == 0)
            return OfficialCurriculumStructureResult.Success(0, 0);

        var saved = await curriculum.SaveAsync(cancellationToken);
        return saved.Succeeded
            ? OfficialCurriculumStructureResult.Success(topicsCreated, outcomesCreated)
            : OfficialCurriculumStructureResult.Failure("PersistenceError");
    }

    private static string CleanGroup(string? groupLabel) =>
        string.IsNullOrWhiteSpace(groupLabel)
            ? "Official curriculum"
            : string.Join(
                " ",
                groupLabel.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries));
}
