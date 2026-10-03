using Edulytics.Core.Constants;
using Edulytics.Core.Enums;
using Edulytics.Core.Interfaces;

namespace Edulytics.Services.Entitlements;

public sealed class StudentEntitlementService
    : IStudentEntitlementService
{
    private readonly ISchoolUserRepository _users;
    private readonly IStudentPortalRepository _schoolPortal;
    private readonly IPersonalEntitlementRepository _personal;

    public StudentEntitlementService(
        ISchoolUserRepository users,
        IStudentPortalRepository schoolPortal,
        IPersonalEntitlementRepository personal)
    {
        _users = users;
        _schoolPortal = schoolPortal;
        _personal = personal;
    }

    public async Task<StudentEntitlementSnapshot> GetAsync(
        Guid studentUserId,
        DateTime? asOfUtc = null,
        CancellationToken cancellationToken = default)
    {
        var now = asOfUtc ?? DateTime.UtcNow;
        var result = new List<StudentContentEntitlement>();

        var personal = await _personal.ListActiveByStudentAsync(
            studentUserId,
            now,
            cancellationToken);

        result.AddRange(personal.Select(x =>
            new StudentContentEntitlement(
                StudentEntitlementSource.Personal,
                x.FrameworkVersionId,
                x.FrameworkCode,
                x.FrameworkName,
                x.FrameworkVersionName,
                x.CurriculumLevelKey,
                x.CurriculumLogicalLevel,
                x.CurriculumLevelLabel,
                x.CurriculumPathway,
                x.SubjectCode,
                x.SubjectName,
                x.StartsAtUtc,
                x.EndsAtUtc)));

        var actor = await _users.GetActorAsync(
            studentUserId,
            cancellationToken);

        if (actor is null ||
            !actor.IsActive ||
            actor.IsLocked ||
            !actor.SchoolId.HasValue ||
            !actor.Roles.Contains(RoleNames.Student, StringComparer.Ordinal))
        {
            return new StudentEntitlementSnapshot(studentUserId, result);
        }

        var school = await _schoolPortal.GetSnapshotAsync(
            actor.SchoolId.Value,
            studentUserId,
            cancellationToken);

        var versions = school.FrameworkVersions.ToDictionary(x => x.Id);
        var frameworks = school.Frameworks.ToDictionary(x => x.Id);
        var grades = school.GradeLevels.ToDictionary(x => x.Id);
        var subjects = school.Subjects.ToDictionary(x => x.Id);

        foreach (var adoption in school.CurriculumAdoptions.Where(x => x.IsActive))
        {
            if (!versions.TryGetValue(adoption.FrameworkVersionId, out var version) ||
                !frameworks.TryGetValue(version.FrameworkId, out var framework) ||
                !subjects.TryGetValue(adoption.SubjectId, out var subject))
            {
                continue;
            }

            var gradeLabel =
                adoption.CurriculumLevelLabel ??
                (grades.TryGetValue(adoption.GradeLevelId, out var grade)
                    ? grade.Name
                    : adoption.CurriculumLevelKey ?? "Curriculum level");

            result.Add(new StudentContentEntitlement(
                StudentEntitlementSource.School,
                version.Id,
                framework.Code,
                framework.Name,
                version.Name,
                adoption.CurriculumLevelKey ?? gradeLabel,
                adoption.CurriculumLogicalLevel ?? 0,
                gradeLabel,
                adoption.CurriculumPathway,
                subject.Code,
                subject.Name,
                null,
                null));
        }

        return new StudentEntitlementSnapshot(
            studentUserId,
            result
                .DistinctBy(x => new
                {
                    x.Source,
                    x.FrameworkVersionId,
                    x.CurriculumLevelKey,
                    x.CurriculumPathway,
                    x.SubjectCode
                })
                .ToArray());
    }
}
