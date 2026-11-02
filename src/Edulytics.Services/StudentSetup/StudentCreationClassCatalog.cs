using Edulytics.Core.Enums;
using Edulytics.Core.Interfaces;
using Edulytics.Services.Academics;
using Edulytics.Services.Users;

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
    private readonly IAcademicStructureRepository _academicRepository;
    private readonly ISchoolUserManagementService _users;
    private readonly ICurriculumRepository _curriculum;

    public StudentCreationClassCatalog(
        IAcademicStructureService academic,
        IAcademicStructureRepository academicRepository,
        ISchoolUserManagementService users,
        ICurriculumRepository curriculum)
    {
        _academic = academic;
        _academicRepository = academicRepository;
        _users = users;
        _curriculum = curriculum;
    }

    public async Task<IReadOnlyList<StudentRoleClassOption>?> ListAsync(
        Guid actorUserId,
        Guid schoolId,
        CancellationToken cancellationToken = default)
    {
        var management =
            await _users.GetManagementContextAsync(
                actorUserId,
                schoolId,
                cancellationToken);

        if (management.Value?.IsPlatformActor == true)
        {
            if (management.Value.SchoolStatus != SchoolStatus.Active)
            {
                return null;
            }

            var snapshot =
                await _academicRepository.GetSnapshotAsync(
                    schoolId,
                    cancellationToken);

            var years =
                snapshot.AcademicYears.ToDictionary(x => x.Id);
            var grades =
                snapshot.GradeLevels.ToDictionary(x => x.Id);

            return await BuildOptionsAsync(
                schoolId,
                snapshot.ClassGroups
                    .Where(
                        x =>
                            x.Status ==
                            AcademicStructureStatus.Active)
                    .Select(
                        x =>
                            new StudentRoleClassOption(
                                x.Id,
                                x.AcademicYearId,
                                years.GetValueOrDefault(x.AcademicYearId)
                                    ?.Name ?? string.Empty,
                                grades.GetValueOrDefault(x.GradeLevelId)
                                    ?.Name ?? string.Empty,
                                x.Name,
                                x.Code)
                            {
                                CurriculumDisplayLabel = null
                            })
                    .ToArray(),
                snapshot.ClassGroups
                    .ToDictionary(
                        x => x.Id,
                        x => x.CurriculumAdoptionId),
                cancellationToken);
        }

        var dashboard =
            await _academic.GetDashboardAsync(
                actorUserId,
                cancellationToken);

        if (dashboard.Value is null ||
            dashboard.Value.SchoolId != schoolId)
        {
            return null;
        }

        return await BuildOptionsAsync(
            schoolId,
            dashboard.Value.ClassGroups
                .Where(
                    x =>
                        x.Status ==
                        AcademicStructureStatus.Active)
                .Select(
                    x =>
                        new StudentRoleClassOption(
                            x.Id,
                            x.AcademicYearId,
                            x.AcademicYearName,
                            x.GradeLevelName,
                            x.Name,
                            x.Code)
                        {
                            CurriculumDisplayLabel = null
                        })
                .ToArray(),
            dashboard.Value.ClassGroups
                .ToDictionary(
                    x => x.Id,
                    x => x.CurriculumAdoptionId),
            cancellationToken);
    }

    private async Task<IReadOnlyList<StudentRoleClassOption>>
        BuildOptionsAsync(
            Guid schoolId,
            IReadOnlyList<StudentRoleClassOption> classes,
            IReadOnlyDictionary<Guid, Guid?> adoptionByClassId,
            CancellationToken cancellationToken)
    {
        var contexts =
            await _curriculum.GetAdoptedCurriculumContextsAsync(
                schoolId,
                cancellationToken);

        var contextsByAdoptionId = contexts
            .Where(x => x.AdoptionId != Guid.Empty)
            .GroupBy(x => x.AdoptionId)
            .ToDictionary(x => x.Key, x => x.First());

        return classes
            .OrderByDescending(x => x.AcademicYearName)
            .ThenBy(x => x.GradeLevelName)
            .ThenBy(x => x.Name)
            .Select(
                x =>
                {
                    string? curriculumDisplayLabel = null;

                    if (adoptionByClassId.TryGetValue(
                            x.Id,
                            out var adoptionId) &&
                        adoptionId.HasValue &&
                        contextsByAdoptionId.TryGetValue(
                            adoptionId.Value,
                            out var context))
                    {
                        var level =
                            string.IsNullOrWhiteSpace(
                                context.CurriculumPathway)
                                ? context.CurriculumLevelLabel
                                : $"{context.CurriculumLevelLabel} — " +
                                  context.CurriculumPathway;

                        curriculumDisplayLabel =
                            $"{context.AcademicProgramName} · {level} · {x.Name}";
                    }

                    return x with
                    {
                        CurriculumDisplayLabel =
                            curriculumDisplayLabel
                    };
                })
            .ToArray();
    }
}
