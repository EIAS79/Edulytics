using Edulytics.Core.Enums;
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

    public StudentCreationClassCatalog(
        IAcademicStructureService academic)
    {
        _academic = academic;
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

        return dashboard.Value.ClassGroups
            .Where(
                x =>
                    x.Status ==
                    AcademicStructureStatus.Active)
            .OrderByDescending(x => x.AcademicYearName)
            .ThenBy(x => x.GradeLevelName)
            .ThenBy(x => x.Name)
            .Select(
                x =>
                    new StudentRoleClassOption(
                        x.Id,
                        x.AcademicYearId,
                        x.AcademicYearName,
                        x.GradeLevelName,
                        x.Name,
                        x.Code))
            .ToArray();
    }
}
