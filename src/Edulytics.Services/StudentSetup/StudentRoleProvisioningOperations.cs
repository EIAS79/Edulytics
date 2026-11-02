using System.Text.RegularExpressions;
using Edulytics.Core.Academics;
using Edulytics.Core.Constants;
using Edulytics.Core.Entities;
using Edulytics.Core.Enums;
using Edulytics.Core.Interfaces;
using Edulytics.Services.Academics;
using Edulytics.Services.Auditing;
using Edulytics.Services.Users;

namespace Edulytics.Services.StudentSetup;

public sealed class StudentRoleProvisioningOperations
    : IStudentRoleProvisioningOperations
{
    private static readonly Regex CodePattern = new(
        "^[A-Z0-9-]+$",
        RegexOptions.Compiled | RegexOptions.CultureInvariant);

    private readonly ISchoolUserManagementService _users;
    private readonly IAcademicStructureService _academic;
    private readonly IAcademicStructureRepository _academicRepository;
    private readonly IAuditService _audit;

    public StudentRoleProvisioningOperations(
        ISchoolUserManagementService users,
        IAcademicStructureService academic,
        IAcademicStructureRepository academicRepository,
        IAuditService audit)
    {
        _users = users;
        _academic = academic;
        _academicRepository = academicRepository;
        _audit = audit;
    }

    public async Task<StudentRoleProvisioningContext?> ReadContextAsync(
        Guid actorUserId,
        Guid schoolId,
        Guid targetUserId,
        CancellationToken cancellationToken = default)
    {
        var userResult =
            await _users.GetAsync(
                actorUserId,
                schoolId,
                targetUserId,
                cancellationToken);

        if (userResult.Value is null)
            return null;

        if (await IsPlatformActorForActiveSchoolAsync(
                actorUserId,
                schoolId,
                cancellationToken))
        {
            var snapshot =
                await _academicRepository.GetSnapshotAsync(
                    schoolId,
                    cancellationToken);

            return BuildPlatformContext(
                schoolId,
                userResult.Value,
                snapshot);
        }

        var dashboardResult =
            await _academic.GetDashboardAsync(
                actorUserId,
                cancellationToken);

        if (dashboardResult.Value is null ||
            dashboardResult.Value.SchoolId != schoolId)
        {
            return null;
        }

        var user = userResult.Value;
        var dashboard = dashboardResult.Value;

        var profile =
            dashboard.StudentProfiles
                .FirstOrDefault(
                    x =>
                        string.Equals(
                            x.UserEmail,
                            user.Email,
                            StringComparison.OrdinalIgnoreCase));

        var classes =
            dashboard.ClassGroups
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

        IReadOnlyList<StudentRoleEnrollmentState> enrollments = [];

        if (profile is not null)
        {
            enrollments =
                dashboard.StudentEnrollments
                    .Where(
                        x =>
                            x.StudentProfileId ==
                            profile.Id)
                    .Select(
                        enrollment =>
                        {
                            var match =
                                classes.FirstOrDefault(
                                    x =>
                                        string.Equals(
                                            x.AcademicYearName,
                                            enrollment.AcademicYearName,
                                            StringComparison.Ordinal) &&
                                        string.Equals(
                                            x.Code,
                                            enrollment.ClassCode,
                                            StringComparison.OrdinalIgnoreCase));

                            return new StudentRoleEnrollmentState(
                                match?.Id,
                                match?.AcademicYearId,
                                enrollment.AcademicYearName,
                                enrollment.ClassName,
                                enrollment.ClassCode);
                        })
                    .ToArray();
        }

        return new StudentRoleProvisioningContext(
            schoolId,
            user.Id,
            user.Email,
            user.Role,
            user.IsActive,
            user.IsLocked,
            profile?.Id,
            profile?.StudentNumber,
            profile?.FirstName,
            profile?.LastName,
            profile?.Status ==
                AcademicStructureStatus.Active,
            profile?.IsArchived ?? false,
            profile?.RowVersion?.ToArray(),
            enrollments,
            classes);
    }

    public async Task<StudentRoleProvisioningOperationResult>
        ChangeRoleAsync(
            Guid actorUserId,
            Guid schoolId,
            Guid targetUserId,
            string role,
            CancellationToken cancellationToken = default)
    {
        var result =
            await _users.ChangeRoleAsync(
                actorUserId,
                schoolId,
                targetUserId,
                role,
                cancellationToken);

        return result.Succeeded
            ? StudentRoleProvisioningOperationResult.Success()
            : StudentRoleProvisioningOperationResult.Failure(
                result.Errors.FirstOrDefault()?.Code.ToString());
    }

    public async Task<StudentRoleProvisioningOperationResult>
        CreateProfileAsync(
            Guid actorUserId,
            Guid schoolId,
            Guid targetUserId,
            string studentNumber,
            string firstName,
            string lastName,
            CancellationToken cancellationToken = default)
    {
        if (!await IsPlatformActorForActiveSchoolAsync(
                actorUserId,
                schoolId,
                cancellationToken))
        {
            var result =
                await _academic.CreateStudentProfileAsync(
                    actorUserId,
                    new CreateStudentProfileRequest(
                        studentNumber,
                        firstName,
                        lastName,
                        targetUserId,
                        AcademicStructureStatus.Active),
                    cancellationToken);

            return Map(result);
        }

        var normalizedStudentNumber =
            NormalizeCode(studentNumber);

        if (!ValidCode(normalizedStudentNumber))
        {
            return Failure(
                AcademicStructureErrorCode.InvalidCode);
        }

        firstName = Clean(firstName);
        lastName = Clean(lastName);

        if (firstName.Length == 0 ||
            lastName.Length == 0)
        {
            return Failure(
                AcademicStructureErrorCode.Required);
        }

        if (firstName.Length > 100 ||
            lastName.Length > 100)
        {
            return Failure(
                AcademicStructureErrorCode.InvalidName);
        }

        var userResult =
            await _users.GetAsync(
                actorUserId,
                schoolId,
                targetUserId,
                cancellationToken);

        if (userResult.Value is null ||
            !userResult.Value.IsActive ||
            userResult.Value.IsLocked ||
            userResult.Value.Role != RoleNames.Student)
        {
            return Failure(
                AcademicStructureErrorCode.InvalidStudentAccount);
        }

        if (await _academicRepository.StudentNumberExistsAsync(
                schoolId,
                normalizedStudentNumber,
                cancellationToken))
        {
            return Failure(
                AcademicStructureErrorCode.DuplicateStudentNumber);
        }

        if (await _academicRepository.StudentUserLinkExistsAsync(
                schoolId,
                targetUserId,
                cancellationToken))
        {
            return Failure(
                AcademicStructureErrorCode.DuplicateStudentUserLink);
        }

        var now = DateTime.UtcNow;
        var entity = new StudentProfile
        {
            Id = Guid.NewGuid(),
            SchoolId = schoolId,
            UserId = targetUserId,
            StudentNumber = normalizedStudentNumber,
            NormalizedStudentNumber = normalizedStudentNumber,
            FirstName = firstName,
            LastName = lastName,
            DisplayName = $"{firstName} {lastName}".Trim(),
            Status = AcademicStructureStatus.Active,
            IsArchived = false,
            ArchivedAtUtc = null,
            CreatedAtUtc = now,
            UpdatedAtUtc = now,
            RowVersion = []
        };

        await QueueAuditAsync(
            actorUserId,
            schoolId,
            "StudentProfile.Created",
            "StudentProfile",
            entity.Id,
            oldValues: null,
            new Dictionary<string, object?>
            {
                ["studentNumber"] = entity.StudentNumber,
                ["displayName"] = entity.DisplayName,
                ["status"] = entity.Status.ToString(),
                ["isArchived"] = false
            },
            "Student profile created by platform administrator.",
            cancellationToken);

        return Map(
            await _academicRepository
                .AddStudentProfileWithSeatGuardAsync(
                    entity,
                    cancellationToken));
    }

    public async Task<StudentRoleProvisioningOperationResult>
        ArchiveProfileAsync(
            Guid actorUserId,
            Guid schoolId,
            Guid studentProfileId,
            byte[] expectedRowVersion,
            CancellationToken cancellationToken = default)
    {
        if (!await IsPlatformActorForActiveSchoolAsync(
                actorUserId,
                schoolId,
                cancellationToken))
        {
            var result =
                await _academic.ArchiveStudentProfileAsync(
                    actorUserId,
                    studentProfileId,
                    expectedRowVersion,
                    cancellationToken);

            return Map(result);
        }

        if (expectedRowVersion.Length == 0)
        {
            return Failure(
                AcademicStructureErrorCode.ConcurrencyConflict);
        }

        var entity =
            await _academicRepository.GetStudentProfileAsync(
                schoolId,
                studentProfileId,
                cancellationToken);

        if (entity is null)
        {
            return Failure(
                AcademicStructureErrorCode.StudentProfileNotFound);
        }

        if (entity.IsArchived)
        {
            return Failure(
                AcademicStructureErrorCode.StudentAlreadyArchived);
        }

        entity.IsArchived = true;
        entity.ArchivedAtUtc = DateTime.UtcNow;
        entity.UpdatedAtUtc = entity.ArchivedAtUtc.Value;

        await QueueAuditAsync(
            actorUserId,
            schoolId,
            "StudentProfile.Archived",
            "StudentProfile",
            entity.Id,
            new Dictionary<string, object?>
            {
                ["isArchived"] = false
            },
            new Dictionary<string, object?>
            {
                ["isArchived"] = true,
                ["archivedAtUtc"] = entity.ArchivedAtUtc
            },
            "Student profile archived by platform administrator.",
            cancellationToken);

        return Map(
            await _academicRepository
                .SaveStudentArchiveStateWithSeatGuardAsync(
                    entity,
                    expectedRowVersion,
                    restoring: false,
                    cancellationToken));
    }

    public async Task<StudentRoleProvisioningOperationResult>
        RestoreProfileAsync(
            Guid actorUserId,
            Guid schoolId,
            Guid studentProfileId,
            byte[] expectedRowVersion,
            CancellationToken cancellationToken = default)
    {
        if (!await IsPlatformActorForActiveSchoolAsync(
                actorUserId,
                schoolId,
                cancellationToken))
        {
            var result =
                await _academic.RestoreStudentProfileAsync(
                    actorUserId,
                    studentProfileId,
                    expectedRowVersion,
                    cancellationToken);

            return Map(result);
        }

        if (expectedRowVersion.Length == 0)
        {
            return Failure(
                AcademicStructureErrorCode.ConcurrencyConflict);
        }

        var entity =
            await _academicRepository.GetStudentProfileAsync(
                schoolId,
                studentProfileId,
                cancellationToken);

        if (entity is null)
        {
            return Failure(
                AcademicStructureErrorCode.StudentProfileNotFound);
        }

        if (!entity.IsArchived)
        {
            return Failure(
                AcademicStructureErrorCode.StudentNotArchived);
        }

        entity.IsArchived = false;
        entity.ArchivedAtUtc = null;
        entity.UpdatedAtUtc = DateTime.UtcNow;

        await QueueAuditAsync(
            actorUserId,
            schoolId,
            "StudentProfile.Restored",
            "StudentProfile",
            entity.Id,
            new Dictionary<string, object?>
            {
                ["isArchived"] = true
            },
            new Dictionary<string, object?>
            {
                ["isArchived"] = false
            },
            "Student profile restored by platform administrator.",
            cancellationToken);

        return Map(
            await _academicRepository
                .SaveStudentArchiveStateWithSeatGuardAsync(
                    entity,
                    expectedRowVersion,
                    restoring: true,
                    cancellationToken));
    }

    public async Task<StudentRoleProvisioningOperationResult>
        CreateEnrollmentAsync(
            Guid actorUserId,
            Guid schoolId,
            Guid studentProfileId,
            Guid classGroupId,
            CancellationToken cancellationToken = default)
    {
        if (!await IsPlatformActorForActiveSchoolAsync(
                actorUserId,
                schoolId,
                cancellationToken))
        {
            var result =
                await _academic.CreateStudentEnrollmentAsync(
                    actorUserId,
                    new CreateStudentEnrollmentRequest(
                        studentProfileId,
                        classGroupId),
                    cancellationToken);

            return Map(result);
        }

        var profile =
            await _academicRepository.GetStudentProfileAsync(
                schoolId,
                studentProfileId,
                cancellationToken);

        if (profile is null)
        {
            return Failure(
                AcademicStructureErrorCode.StudentProfileNotFound);
        }

        if (profile.IsArchived)
        {
            return Failure(
                AcademicStructureErrorCode.StudentAlreadyArchived);
        }

        var classGroup =
            await _academicRepository.GetClassGroupAsync(
                schoolId,
                classGroupId,
                cancellationToken);

        if (classGroup is null)
        {
            return Failure(
                AcademicStructureErrorCode.ClassGroupNotFound);
        }

        if (await _academicRepository.StudentEnrollmentExistsAsync(
                schoolId,
                classGroup.AcademicYearId,
                profile.Id,
                cancellationToken))
        {
            return Failure(
                AcademicStructureErrorCode.DuplicateEnrollment);
        }

        var entity = new StudentEnrollment
        {
            Id = Guid.NewGuid(),
            SchoolId = schoolId,
            StudentProfileId = profile.Id,
            ClassGroupId = classGroup.Id,
            AcademicYearId = classGroup.AcademicYearId,
            EnrolledAtUtc = DateTime.UtcNow
        };

        await _academicRepository.AddAsync(
            entity,
            cancellationToken);

        await QueueAuditAsync(
            actorUserId,
            schoolId,
            "StudentEnrollment.Created",
            "StudentEnrollment",
            entity.Id,
            oldValues: null,
            new Dictionary<string, object?>
            {
                ["studentProfileId"] = entity.StudentProfileId,
                ["classGroupId"] = entity.ClassGroupId,
                ["academicYearId"] = entity.AcademicYearId
            },
            "Student enrollment created by platform administrator.",
            cancellationToken);

        return Map(
            await _academicRepository.SaveAsync(
                cancellationToken));
    }

    private async Task<bool> IsPlatformActorForActiveSchoolAsync(
        Guid actorUserId,
        Guid schoolId,
        CancellationToken cancellationToken)
    {
        var management =
            await _users.GetManagementContextAsync(
                actorUserId,
                schoolId,
                cancellationToken);

        return management.Value is
        {
            IsPlatformActor: true,
            SchoolStatus: SchoolStatus.Active
        };
    }

    private static StudentRoleProvisioningContext BuildPlatformContext(
        Guid schoolId,
        SchoolUserDetails user,
        AcademicStructureSnapshot snapshot)
    {
        var years =
            snapshot.AcademicYears.ToDictionary(x => x.Id);
        var grades =
            snapshot.GradeLevels.ToDictionary(x => x.Id);
        var classesById =
            snapshot.ClassGroups.ToDictionary(x => x.Id);

        var profile =
            snapshot.StudentProfiles
                .FirstOrDefault(
                    x => x.UserId == user.Id);

        var classes =
            snapshot.ClassGroups
                .Where(
                    x =>
                        x.Status ==
                        AcademicStructureStatus.Active)
                .OrderByDescending(
                    x =>
                        years.GetValueOrDefault(x.AcademicYearId)
                            ?.Name ?? string.Empty)
                .ThenBy(
                    x =>
                        grades.GetValueOrDefault(x.GradeLevelId)
                            ?.Name ?? string.Empty)
                .ThenBy(x => x.Name)
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
                            x.Code))
                .ToArray();

        IReadOnlyList<StudentRoleEnrollmentState> enrollments = [];

        if (profile is not null)
        {
            enrollments =
                snapshot.StudentEnrollments
                    .Where(
                        x =>
                            x.StudentProfileId == profile.Id)
                    .Select(
                        enrollment =>
                        {
                            var classGroup =
                                classesById.GetValueOrDefault(
                                    enrollment.ClassGroupId);

                            return new StudentRoleEnrollmentState(
                                classGroup?.Id,
                                enrollment.AcademicYearId,
                                years.GetValueOrDefault(
                                        enrollment.AcademicYearId)
                                    ?.Name ?? string.Empty,
                                classGroup?.Name ?? string.Empty,
                                classGroup?.Code ?? string.Empty);
                        })
                    .ToArray();
        }

        return new StudentRoleProvisioningContext(
            schoolId,
            user.Id,
            user.Email,
            user.Role,
            user.IsActive,
            user.IsLocked,
            profile?.Id,
            profile?.StudentNumber,
            profile?.FirstName,
            profile?.LastName,
            profile?.Status ==
                AcademicStructureStatus.Active,
            profile?.IsArchived ?? false,
            profile?.RowVersion?.ToArray(),
            enrollments,
            classes);
    }

    private async Task QueueAuditAsync(
        Guid actorUserId,
        Guid schoolId,
        string action,
        string entityType,
        Guid entityId,
        IReadOnlyDictionary<string, object?>? oldValues,
        IReadOnlyDictionary<string, object?>? newValues,
        string resultSummary,
        CancellationToken cancellationToken)
    {
        await _audit.QueueAsync(
            new AuditEvent(
                SchoolId: schoolId,
                Action: action,
                EntityType: entityType,
                EntityId: entityId.ToString("D"),
                Feature: "AcademicStructure",
                OldValues: oldValues,
                NewValues: newValues,
                ResultSummary: resultSummary,
                ActorUserIdOverride: actorUserId,
                ActorRoleOverride: RoleNames.SuperAdmin),
            cancellationToken);
    }

    private static StudentRoleProvisioningOperationResult Map(
        AcademicCommandResult result)
    {
        if (result.Succeeded)
            return StudentRoleProvisioningOperationResult.Success();

        var first = result.Errors.FirstOrDefault();

        return StudentRoleProvisioningOperationResult.Failure(
            first?.Code.ToString(),
            first?.Code);
    }

    private static StudentRoleProvisioningOperationResult Map(
        AcademicPersistenceResult result)
    {
        if (result.Succeeded)
            return StudentRoleProvisioningOperationResult.Success();

        var error = result.Error switch
        {
            AcademicPersistenceError.Conflict =>
                AcademicStructureErrorCode.ConcurrencyConflict,
            AcademicPersistenceError.SeatLimit =>
                AcademicStructureErrorCode.StudentSeatLimitReached,
            _ =>
                AcademicStructureErrorCode.PersistenceError
        };

        return Failure(error);
    }

    private static StudentRoleProvisioningOperationResult Failure(
        AcademicStructureErrorCode error) =>
        StudentRoleProvisioningOperationResult.Failure(
            error.ToString(),
            error);

    private static bool ValidCode(string value) =>
        value.Length is > 0 and <= 50 &&
        CodePattern.IsMatch(value);

    private static string Clean(string? value) =>
        value?.Trim() ?? string.Empty;

    private static string NormalizeCode(string? value) =>
        Clean(value).ToUpperInvariant();
}
