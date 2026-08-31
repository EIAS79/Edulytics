using Edulytics.Core.Enums;
using Edulytics.Core.Interfaces;

namespace Edulytics.Services.Curriculum;

public sealed record ExplicitCurriculumOutcomeUiItem(
    Guid Id,
    Guid TopicId,
    string Code,
    string Description,
    int Order,
    bool IsOfficial);

public sealed record ExplicitOfficialOutcomeUiOption(
    Guid ContentNodeId,
    Guid? LessonNodeId,
    string Code,
    string Description,
    string SelectionLabel,
    string? GroupLabel,
    int SortOrder)
{
    public string SelectionKey =>
        $"{ContentNodeId}|{(LessonNodeId.HasValue ? LessonNodeId.Value.ToString() : string.Empty)}";
}

public sealed record ExplicitCurriculumTopicUiItem(
    Guid Id,
    Guid CurriculumAdoptionId,
    Guid AcademicYearId,
    string AcademicYearName,
    Guid AcademicProgramId,
    string AcademicProgramName,
    string AcademicProgramCode,
    string CurriculumLevelKey,
    int CurriculumLogicalLevel,
    string CurriculumLevelLabel,
    string CurriculumStage,
    string? CurriculumPathway,
    string FrameworkCode,
    string FrameworkName,
    string Name,
    int Order,
    IReadOnlyList<ExplicitCurriculumOutcomeUiItem> Outcomes,
    IReadOnlyList<ExplicitOfficialOutcomeUiOption> OfficialOutcomes);

public interface IExplicitCurriculumContentUiQuery
{
    Task<IReadOnlyList<ExplicitCurriculumTopicUiItem>> ListTopicsAsync(
        Guid actorUserId,
        CancellationToken cancellationToken = default);
}

public sealed class ExplicitCurriculumContentUiQuery : IExplicitCurriculumContentUiQuery
{
    private readonly ICurriculumRepository _curriculum;
    private readonly ISchoolRepository _schools;
    private readonly ISchoolUserRepository _users;

    public ExplicitCurriculumContentUiQuery(
        ICurriculumRepository curriculum,
        ISchoolRepository schools,
        ISchoolUserRepository users)
    {
        _curriculum = curriculum;
        _schools = schools;
        _users = users;
    }

    public async Task<IReadOnlyList<ExplicitCurriculumTopicUiItem>> ListTopicsAsync(
        Guid actorUserId,
        CancellationToken cancellationToken = default)
    {
        var actor = await _users.GetActorAsync(actorUserId, cancellationToken);
        if (actor is null ||
            !actor.IsActive ||
            actor.IsLocked ||
            !actor.SchoolId.HasValue)
        {
            return [];
        }

        var school = await _schools.GetByIdAsync(
            actor.SchoolId.Value,
            cancellationToken);
        if (school is null || school.Status != SchoolStatus.Active)
            return [];

        var snapshot = await _curriculum.GetSnapshotAsync(
            school.Id,
            cancellationToken);
        var contexts = await _curriculum.GetAdoptedCurriculumContextsAsync(
            school.Id,
            cancellationToken);

        var explicitContexts = contexts
            .Where(x =>
                x.AdoptionId != Guid.Empty &&
                x.AcademicYearId.HasValue &&
                !string.IsNullOrWhiteSpace(x.CurriculumLevelKey) &&
                x.CurriculumLogicalLevel.HasValue)
            .ToArray();

        if (explicitContexts.Length == 0)
            return [];

        var outcomesByTopic = snapshot.Outcomes
            .Where(x => x.CurriculumAdoptionId.HasValue)
            .GroupBy(x => x.TopicId)
            .ToDictionary(
                x => x.Key,
                x => (IReadOnlyList<ExplicitCurriculumOutcomeUiItem>)x
                    .OrderBy(y => y.Order)
                    .Select(y => new ExplicitCurriculumOutcomeUiItem(
                        y.Id,
                        y.TopicId,
                        y.Code,
                        y.Description,
                        y.Order,
                        y.OfficialContentNodeId.HasValue))
                    .ToArray());

        var result = new List<ExplicitCurriculumTopicUiItem>();
        foreach (var context in explicitContexts)
        {
            var official = await _curriculum.GetOfficialOutcomeSourcesAsync(
                context.FrameworkVersionId,
                context.CurriculumLogicalLevel!.Value,
                context.CurriculumPathway,
                cancellationToken);

            var officialOptions = official
                .Select(x => new ExplicitOfficialOutcomeUiOption(
                    x.ContentNodeId,
                    x.LessonNodeId,
                    x.Code,
                    x.Description,
                    x.SelectionLabel,
                    x.GroupLabel,
                    x.SortOrder))
                .ToArray();

            foreach (var topic in snapshot.Topics
                         .Where(x => x.CurriculumAdoptionId == context.AdoptionId)
                         .OrderBy(x => x.Order))
            {
                result.Add(new ExplicitCurriculumTopicUiItem(
                    topic.Id,
                    context.AdoptionId,
                    context.AcademicYearId!.Value,
                    snapshot.AcademicYears
                        .FirstOrDefault(x => x.Id == context.AcademicYearId.Value)?.Name ??
                        string.Empty,
                    context.AcademicProgramId,
                    context.AcademicProgramName,
                    context.AcademicProgramCode,
                    context.CurriculumLevelKey!,
                    context.CurriculumLogicalLevel.Value,
                    context.CurriculumLevelLabel ?? string.Empty,
                    context.CurriculumStage ?? string.Empty,
                    context.CurriculumPathway,
                    context.FrameworkCode,
                    context.FrameworkName,
                    topic.Name,
                    topic.Order,
                    outcomesByTopic.GetValueOrDefault(
                        topic.Id,
                        Array.Empty<ExplicitCurriculumOutcomeUiItem>()),
                    officialOptions));
            }
        }

        return result
            .OrderByDescending(x => x.AcademicYearName)
            .ThenBy(x => x.AcademicProgramName)
            .ThenBy(x => x.CurriculumLogicalLevel)
            .ThenBy(x => x.CurriculumPathway ?? string.Empty)
            .ThenBy(x => x.Order)
            .ToArray();
    }
}
