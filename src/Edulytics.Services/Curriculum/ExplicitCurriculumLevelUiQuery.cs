using Edulytics.Core.Constants;
using Edulytics.Core.Entities;
using Edulytics.Core.Enums;
using Edulytics.Core.Interfaces;
using Edulytics.Core.Users;

namespace Edulytics.Services.Curriculum;

public sealed record ExplicitCurriculumClassItem(
    Guid ClassGroupId,
    Guid AcademicYearId,
    string AcademicYearName,
    Guid CurriculumAdoptionId,
    Guid AcademicProgramId,
    string AcademicProgramName,
    string AcademicProgramCode,
    string CurriculumLevelKey,
    int CurriculumLogicalLevel,
    string CurriculumLevelLabel,
    string CurriculumStage,
    string? CurriculumPathway,
    string Name,
    string Code,
    AcademicStructureStatus Status);

public interface IExplicitCurriculumLevelUiQuery
{
    Task<IReadOnlyList<ExplicitCurriculumClassItem>> ListClassesAsync(
        Guid actorUserId,
        CancellationToken cancellationToken = default);
}

public sealed class ExplicitCurriculumLevelUiQuery : IExplicitCurriculumLevelUiQuery
{
    private readonly IAcademicStructureRepository _academic;
    private readonly ICurriculumRepository _curriculum;
    private readonly ISchoolRepository _schools;
    private readonly ISchoolUserRepository _users;

    public ExplicitCurriculumLevelUiQuery(
        IAcademicStructureRepository academic,
        ICurriculumRepository curriculum,
        ISchoolRepository schools,
        ISchoolUserRepository users)
    {
        _academic = academic;
        _curriculum = curriculum;
        _schools = schools;
        _users = users;
    }

    public async Task<IReadOnlyList<ExplicitCurriculumClassItem>> ListClassesAsync(
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

        var snapshot = await _academic.GetSnapshotAsync(
            school.Id,
            cancellationToken);
        var contexts = await _curriculum.GetAdoptedCurriculumContextsAsync(
            school.Id,
            cancellationToken);

        var yearNames = snapshot.AcademicYears.ToDictionary(x => x.Id, x => x.Name);
        var contextsById = contexts
            .Where(x =>
                x.AdoptionId != Guid.Empty &&
                x.AcademicYearId.HasValue &&
                !string.IsNullOrWhiteSpace(x.CurriculumLevelKey) &&
                x.CurriculumLogicalLevel.HasValue)
            .ToDictionary(x => x.AdoptionId);

        var result = new List<ExplicitCurriculumClassItem>();
        foreach (var classGroup in snapshot.ClassGroups)
        {
            if (!classGroup.CurriculumAdoptionId.HasValue ||
                !contextsById.TryGetValue(
                    classGroup.CurriculumAdoptionId.Value,
                    out var context) ||
                context.AcademicYearId != classGroup.AcademicYearId)
            {
                continue;
            }

            result.Add(new ExplicitCurriculumClassItem(
                classGroup.Id,
                classGroup.AcademicYearId,
                yearNames.GetValueOrDefault(classGroup.AcademicYearId) ?? string.Empty,
                context.AdoptionId,
                context.AcademicProgramId,
                context.AcademicProgramName,
                context.AcademicProgramCode,
                context.CurriculumLevelKey!,
                context.CurriculumLogicalLevel!.Value,
                context.CurriculumLevelLabel ?? string.Empty,
                context.CurriculumStage ?? string.Empty,
                context.CurriculumPathway,
                classGroup.Name,
                classGroup.Code,
                classGroup.Status));
        }

        return result
            .OrderByDescending(x => x.AcademicYearName)
            .ThenBy(x => x.AcademicProgramName)
            .ThenBy(x => x.CurriculumLogicalLevel)
            .ThenBy(x => x.CurriculumPathway ?? string.Empty)
            .ThenBy(x => x.Name)
            .ToArray();
    }
}
