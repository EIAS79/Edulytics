using Edulytics.Core.Enums;
using Edulytics.Core.Interfaces;
using Edulytics.Services.Academics;

namespace Edulytics.Services.StudentSetup;

public interface IStudentCreationClassCatalog
{
    Task<IReadOnlyList<StudentRoleClassOption>?> ListAsync(
        Guid actorUserId,
        Guid schoolId,
        CancellationToken cancellationToken = default);
}

public sealed class StudentCreationClassCatalog
    : IStudentCreationClassCatalog
{
    private readonly IAcademicStructureService _academic;
    private readonly ICurriculumRepository _curriculum;

    public StudentCreationClassCatalog(
        IAcademicStructureService academic,
        ICurriculumRepository curriculum)
    {
        _academic = academic;
        _curriculum = curriculum;
    }

    public async Task<IReadOnlyList<StudentRoleClassOption>?> ListAsync(
        Guid actorUserId,
        Guid schoolId,
        CancellationToken cancellationToken = default)
    {
        var dashboard =
            await _academic.GetDashboardAsync(
                actorUserId,
                cancellationToken);

        if (dashboard.Value is null ||
            dashboard.Value.SchoolId != schoolId)
        {
            return null;
        }

        var contexts = await _curriculum.GetAdoptedCurriculumContextsAsync(
            schoolId,
            cancellationToken);
        var contextsByAdoptionId = contexts
            .Where(x => x.AdoptionId != Guid.Empty)
            .GroupBy(x => x.AdoptionId)
            .ToDictionary(x => x.Key, x => x.First());

        return dashboard.Value.ClassGroups
            .Where(
                x =>
                    x.Status ==
                    AcademicStructureStatus.Active)
            .OrderByDescending(x => x.AcademicYearName)
            .ThenBy(x => x.AcademicProgramName)
            .ThenBy(x => x.GradeLevelName)
            .ThenBy(x => x.Name)
            .Select(x =>
            {
                var displayLabel =
                    $"{x.AcademicYearName} · {x.GradeLevelName} · {x.Name}";

                if (x.CurriculumAdoptionId.HasValue &&
                    contextsByAdoptionId.TryGetValue(
                        x.CurriculumAdoptionId.Value,
                        out var context))
                {
                    var level = string.IsNullOrWhiteSpace(
                        context.CurriculumPathway)
                        ? context.CurriculumLevelLabel
                        : $"{context.CurriculumLevelLabel} — " +
                          context.CurriculumPathway;
                    displayLabel =
                        $"{context.AcademicProgramName} · {level} · {x.Name}";
                }

                return new StudentRoleClassOption(
                    x.Id,
                    x.AcademicYearId,
                    x.AcademicYearName,
                    x.GradeLevelName,
                    x.Name,
                    x.Code)
                {
                    DisplayLabel = displayLabel
                };
            })
            .ToArray();
    }
}