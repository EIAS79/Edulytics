using Edulytics.Core.Constants;
using Edulytics.Core.Entities;
using Edulytics.Core.Enums;
using Edulytics.Core.Interfaces;
using Edulytics.Core.Users;

namespace Edulytics.Services.Academics;

public sealed record StudentPlacementFailure(
    Guid StudentProfileId,
    string Code);

public sealed record StudentPlacementResult(
    bool Succeeded,
    int Enrolled,
    int Moved,
    int Unchanged,
    IReadOnlyList<StudentPlacementFailure> Failures)
{
    public static StudentPlacementResult Denied(string code) =>
        new(false, 0, 0, 0, [new StudentPlacementFailure(Guid.Empty, code)]);
}

public interface IStudentPlacementService
{
    Task<StudentPlacementResult> PlaceStudentsAsync(
        Guid actorUserId,
        Guid targetClassGroupId,
        IReadOnlyCollection<Guid> studentProfileIds,
        CancellationToken cancellationToken = default);
}

public sealed class StudentPlacementService : IStudentPlacementService
{
    private readonly IAcademicStructureRepository _academic;
    private readonly IStudentPlacementRepository _placements;
    private readonly ISchoolRepository _schools;
    private readonly ISchoolUserRepository _users;

    public StudentPlacementService(
        IAcademicStructureRepository academic,
        IStudentPlacementRepository placements,
        ISchoolRepository schools,
        ISchoolUserRepository users)
    {
        _academic = academic;
        _placements = placements;
        _schools = schools;
        _users = users;
    }

    public async Task<StudentPlacementResult> PlaceStudentsAsync(
        Guid actorUserId,
        Guid targetClassGroupId,
        IReadOnlyCollection<Guid> studentProfileIds,
        CancellationToken cancellationToken = default)
    {
        var actor = await _users.GetActorAsync(actorUserId, cancellationToken);
        if (actor is null ||
            !actor.IsActive ||
            actor.IsLocked ||
            !actor.SchoolId.HasValue ||
            actor.Roles.Count != 1 ||
            !actor.Roles.Contains(RoleNames.SubjectSupervisor, StringComparer.Ordinal))
        {
            return StudentPlacementResult.Denied("AccessDenied");
        }

        var school = await _schools.GetByIdAsync(actor.SchoolId.Value, cancellationToken);
        if (school is null || school.Status != SchoolStatus.Active)
            return StudentPlacementResult.Denied("SchoolNotActive");

        var schoolId = school.Id;
        var targetClass = await _academic.GetClassGroupAsync(
            schoolId,
            targetClassGroupId,
            cancellationToken);

        if (targetClass is null || targetClass.Status != AcademicStructureStatus.Active)
            return StudentPlacementResult.Denied("ClassGroupNotFound");

        var ids = studentProfileIds
            .Where(x => x != Guid.Empty)
            .Distinct()
            .ToArray();

        if (ids.Length == 0)
            return StudentPlacementResult.Denied("Required");

        var failures = new List<StudentPlacementFailure>();
        var enrolled = 0;
        var moved = 0;
        var unchanged = 0;

        foreach (var studentId in ids)
        {
            var profile = await _academic.GetStudentProfileAsync(
                schoolId,
                studentId,
                cancellationToken);

            if (profile is null)
            {
                failures.Add(new StudentPlacementFailure(studentId, "StudentProfileNotFound"));
                continue;
            }

            if (profile.IsArchived || profile.Status != AcademicStructureStatus.Active)
            {
                failures.Add(new StudentPlacementFailure(studentId, "StudentInactive"));
                continue;
            }

            var existing = await _placements.GetEnrollmentAsync(
                schoolId,
                targetClass.AcademicYearId,
                studentId,
                cancellationToken);

            if (existing is null)
            {
                await _placements.AddEnrollmentAsync(
                    new StudentEnrollment
                    {
                        Id = Guid.NewGuid(),
                        SchoolId = schoolId,
                        StudentProfileId = studentId,
                        ClassGroupId = targetClass.Id,
                        AcademicYearId = targetClass.AcademicYearId,
                        EnrolledAtUtc = DateTime.UtcNow
                    },
                    cancellationToken);

                enrolled++;
                continue;
            }

            if (existing.ClassGroupId == targetClass.Id)
            {
                unchanged++;
                continue;
            }

            var sourceClass = await _academic.GetClassGroupAsync(
                schoolId,
                existing.ClassGroupId,
                cancellationToken);

            if (sourceClass is null)
            {
                failures.Add(new StudentPlacementFailure(studentId, "ExistingClassNotFound"));
                continue;
            }

            if (sourceClass.GradeLevelId != targetClass.GradeLevelId)
            {
                failures.Add(new StudentPlacementFailure(studentId, "CrossGradeMoveNotAllowed"));
                continue;
            }

            existing.ClassGroupId = targetClass.Id;
            moved++;
        }

        if (enrolled == 0 && moved == 0)
        {
            return new StudentPlacementResult(
                failures.Count == 0,
                enrolled,
                moved,
                unchanged,
                failures);
        }

        var saved = await _placements.SaveAsync(cancellationToken);
        if (!saved.Succeeded)
        {
            failures.Add(new StudentPlacementFailure(Guid.Empty, "PersistenceError"));
            return new StudentPlacementResult(false, 0, 0, unchanged, failures);
        }

        return new StudentPlacementResult(
            failures.Count == 0,
            enrolled,
            moved,
            unchanged,
            failures);
    }
}
