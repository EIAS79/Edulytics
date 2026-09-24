using Edulytics.Core.Interfaces;
using Edulytics.Core.Users;
using Edulytics.Data.Contexts;
using Edulytics.Data.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Edulytics.Data.Repositories;

public sealed class IdentitySchoolUserRepository
    : ISchoolUserRepository
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly EdulyticsDbContext _context;

    public IdentitySchoolUserRepository(
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager,
        EdulyticsDbContext context)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _context = context;
    }

    public async Task<SchoolUserRecord?> GetActorAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        var user = await _userManager.Users
            .AsNoTracking()
            .SingleOrDefaultAsync(
                x => x.Id == userId,
                cancellationToken);

        if (user is null)
        {
            return null;
        }

        var tracked = await _userManager.FindByIdAsync(
            user.Id.ToString());

        if (tracked is null)
        {
            return null;
        }

        var roles = await _userManager.GetRolesAsync(
            tracked);

        return ToRecord(
            tracked,
            roles);
    }

    public async Task<IReadOnlyList<SchoolUserRecord>>
        ListBySchoolAsync(
            Guid schoolId,
            CancellationToken cancellationToken = default)
    {
        var users = await _context.Users
            .AsNoTracking()
            .Where(x => x.SchoolId == schoolId)
            .OrderBy(x => x.Email)
            .ToArrayAsync(cancellationToken);

        if (users.Length == 0)
        {
            return [];
        }

        var userIds = users
            .Select(x => x.Id)
            .ToArray();

        var roleRows =
            await (
                from userRole in _context.UserRoles
                join role in _context.Roles
                    on userRole.RoleId equals role.Id
                where userIds.Contains(userRole.UserId)
                select new
                {
                    userRole.UserId,
                    RoleName = role.Name!
                })
            .AsNoTracking()
            .ToArrayAsync(cancellationToken);

        var rolesByUser = roleRows
            .GroupBy(x => x.UserId)
            .ToDictionary(
                group => group.Key,
                group => group
                    .Select(x => x.RoleName)
                    .OrderBy(x => x)
                    .ToArray());

        return users
            .Select(
                user => ToRecord(
                    user,
                    rolesByUser.TryGetValue(
                        user.Id,
                        out var roles)
                        ? roles
                        : []))
            .ToArray();
    }

    public async Task<SchoolUserDirectoryFilterOptions>
        GetDirectoryFilterOptionsAsync(
            Guid schoolId,
            CancellationToken cancellationToken = default)
    {
        var years = await _context.AcademicYears
            .AsNoTracking()
            .Where(x => x.SchoolId == schoolId)
            .OrderByDescending(x => x.StartsOn)
            .ThenByDescending(x => x.Name)
            .Select(x => new SchoolUserDirectoryFilterOption(
                x.Id,
                x.Name))
            .ToArrayAsync(cancellationToken);

        var programs = await _context.AcademicPrograms
            .AsNoTracking()
            .Where(x =>
                x.SchoolId == schoolId &&
                !x.IsDefault)
            .OrderBy(x => x.Name)
            .Select(x => new SchoolUserDirectoryFilterOption(
                x.Id,
                x.Name))
            .ToArrayAsync(cancellationToken);

        var classes = await (
                from classGroup in _context.ClassGroups.AsNoTracking()
                join year in _context.AcademicYears.AsNoTracking()
                    on classGroup.AcademicYearId equals year.Id
                join program in _context.AcademicPrograms.AsNoTracking()
                    on classGroup.AcademicProgramId equals program.Id
                where classGroup.SchoolId == schoolId
                orderby year.StartsOn descending,
                    program.Name,
                    classGroup.Name
                select new SchoolUserClassFilterOption(
                    classGroup.Id,
                    classGroup.Name + " · " + year.Name + " · " + program.Name,
                    year.Id,
                    program.Id))
            .ToArrayAsync(cancellationToken);

        return new SchoolUserDirectoryFilterOptions(
            years,
            programs,
            classes);
    }

    public async Task<SchoolUserPage> QueryBySchoolAsync(
        Guid schoolId,
        SchoolUserListQuery query,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(query);

        var page = Math.Max(1, query.Page);
        var pageSize = Math.Clamp(query.PageSize, 1, 100);
        var search = query.Search?.Trim();
        var role = query.Role?.Trim();
        var name = query.Name?.Trim();
        var userId = query.UserId?.Trim();
        var email = query.Email?.Trim();

        var usersQuery = _context.Users
            .AsNoTracking()
            .Where(x => x.SchoolId == schoolId);

        if (!string.IsNullOrWhiteSpace(search))
        {
            usersQuery = usersQuery.Where(x =>
                x.Email != null &&
                EF.Functions.Like(x.Email, $"%{search}%"));
        }

        if (!string.IsNullOrWhiteSpace(email))
        {
            usersQuery = usersQuery.Where(x =>
                x.Email != null &&
                EF.Functions.Like(x.Email, $"%{email}%"));
        }

        if (!string.IsNullOrWhiteSpace(name))
        {
            var namedStudentUserIds = _context.StudentProfiles
                .AsNoTracking()
                .Where(profile =>
                    profile.SchoolId == schoolId &&
                    profile.UserId.HasValue &&
                    (EF.Functions.Like(profile.DisplayName, $"%{name}%") ||
                     EF.Functions.Like(profile.FirstName, $"%{name}%") ||
                     EF.Functions.Like(profile.LastName, $"%{name}%")))
                .Select(profile => profile.UserId.GetValueOrDefault());

            usersQuery = usersQuery.Where(x =>
                namedStudentUserIds.Contains(x.Id));
        }

        if (!string.IsNullOrWhiteSpace(userId))
        {
            if (Guid.TryParse(userId, out var parsedUserId))
            {
                usersQuery = usersQuery.Where(x => x.Id == parsedUserId);
            }
            else
            {
                usersQuery = usersQuery.Where(_ => false);
            }
        }

        if (query.IsActive.HasValue)
            usersQuery = usersQuery.Where(x => x.IsActive == query.IsActive.Value);

        if (query.IsLocked.HasValue)
        {
            var now = DateTimeOffset.UtcNow;
            usersQuery = query.IsLocked.Value
                ? usersQuery.Where(x =>
                    x.LockoutEnabled &&
                    x.LockoutEnd.HasValue &&
                    x.LockoutEnd.Value > now)
                : usersQuery.Where(x =>
                    !x.LockoutEnabled ||
                    !x.LockoutEnd.HasValue ||
                    x.LockoutEnd.Value <= now);
        }

        if (!string.IsNullOrWhiteSpace(role))
        {
            var roleUserIds =
                from userRole in _context.UserRoles
                join roleRow in _context.Roles
                    on userRole.RoleId equals roleRow.Id
                where roleRow.Name == role
                select userRole.UserId;

            usersQuery = usersQuery.Where(x => roleUserIds.Contains(x.Id));
        }

        if (query.AcademicYearId.HasValue ||
            query.AcademicProgramId.HasValue ||
            query.ClassGroupId.HasValue)
        {
            var studentAcademicUserIds =
                from profile in _context.StudentProfiles.AsNoTracking()
                join enrollment in _context.StudentEnrollments.AsNoTracking()
                    on profile.Id equals enrollment.StudentProfileId
                join classGroup in _context.ClassGroups.AsNoTracking()
                    on enrollment.ClassGroupId equals classGroup.Id
                where
                    profile.SchoolId == schoolId &&
                    enrollment.SchoolId == schoolId &&
                    classGroup.SchoolId == schoolId &&
                    profile.UserId.HasValue &&
                    (!query.AcademicYearId.HasValue ||
                     classGroup.AcademicYearId == query.AcademicYearId.Value) &&
                    (!query.AcademicProgramId.HasValue ||
                     classGroup.AcademicProgramId == query.AcademicProgramId.Value) &&
                    (!query.ClassGroupId.HasValue ||
                     classGroup.Id == query.ClassGroupId.Value)
                select profile.UserId.GetValueOrDefault();

            var teacherAcademicUserIds =
                from assignment in _context.TeacherAssignments.AsNoTracking()
                join classGroup in _context.ClassGroups.AsNoTracking()
                    on assignment.ClassGroupId equals classGroup.Id
                where
                    assignment.SchoolId == schoolId &&
                    classGroup.SchoolId == schoolId &&
                    (!query.AcademicYearId.HasValue ||
                     classGroup.AcademicYearId == query.AcademicYearId.Value) &&
                    (!query.AcademicProgramId.HasValue ||
                     classGroup.AcademicProgramId == query.AcademicProgramId.Value) &&
                    (!query.ClassGroupId.HasValue ||
                     classGroup.Id == query.ClassGroupId.Value)
                select assignment.TeacherUserId;

            var academicUserIds =
                studentAcademicUserIds.Union(teacherAcademicUserIds);

            usersQuery = usersQuery.Where(x =>
                academicUserIds.Contains(x.Id));
        }

        var total = await usersQuery.CountAsync(cancellationToken);
        var totalPages = Math.Max(
            1,
            (int)Math.Ceiling(total / (double)pageSize));
        page = Math.Min(page, totalPages);

        var users = await usersQuery
            .OrderBy(x => x.Email)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToArrayAsync(cancellationToken);

        if (users.Length == 0)
            return new SchoolUserPage([], page, pageSize, total);

        var userIds = users.Select(x => x.Id).ToArray();

        var roleRows = await (
                from userRole in _context.UserRoles
                join roleRow in _context.Roles
                    on userRole.RoleId equals roleRow.Id
                where userIds.Contains(userRole.UserId)
                select new
                {
                    userRole.UserId,
                    RoleName = roleRow.Name!
                })
            .AsNoTracking()
            .ToArrayAsync(cancellationToken);

        var studentRows = await _context.StudentProfiles
            .AsNoTracking()
            .Where(profile =>
                profile.SchoolId == schoolId &&
                profile.UserId.HasValue &&
                userIds.Contains(profile.UserId.GetValueOrDefault()))
            .Select(profile => new
            {
                UserId = profile.UserId.GetValueOrDefault(),
                profile.DisplayName,
                profile.StudentNumber
            })
            .ToArrayAsync(cancellationToken);

        var studentContexts = await (
                from profile in _context.StudentProfiles.AsNoTracking()
                join enrollment in _context.StudentEnrollments.AsNoTracking()
                    on profile.Id equals enrollment.StudentProfileId
                join classGroup in _context.ClassGroups.AsNoTracking()
                    on enrollment.ClassGroupId equals classGroup.Id
                join year in _context.AcademicYears.AsNoTracking()
                    on classGroup.AcademicYearId equals year.Id
                join program in _context.AcademicPrograms.AsNoTracking()
                    on classGroup.AcademicProgramId equals program.Id
                where
                    profile.SchoolId == schoolId &&
                    profile.UserId.HasValue &&
                    userIds.Contains(profile.UserId.GetValueOrDefault())
                select new
                {
                    UserId = profile.UserId.GetValueOrDefault(),
                    YearId = year.Id,
                    YearName = year.Name,
                    ProgramId = program.Id,
                    ProgramName = program.Name,
                    ClassId = classGroup.Id,
                    ClassName = classGroup.Name
                })
            .ToArrayAsync(cancellationToken);

        var teacherContexts = await (
                from assignment in _context.TeacherAssignments.AsNoTracking()
                join classGroup in _context.ClassGroups.AsNoTracking()
                    on assignment.ClassGroupId equals classGroup.Id
                join year in _context.AcademicYears.AsNoTracking()
                    on classGroup.AcademicYearId equals year.Id
                join program in _context.AcademicPrograms.AsNoTracking()
                    on classGroup.AcademicProgramId equals program.Id
                where
                    assignment.SchoolId == schoolId &&
                    userIds.Contains(assignment.TeacherUserId)
                select new
                {
                    UserId = assignment.TeacherUserId,
                    YearId = year.Id,
                    YearName = year.Name,
                    ProgramId = program.Id,
                    ProgramName = program.Name,
                    ClassId = classGroup.Id,
                    ClassName = classGroup.Name
                })
            .ToArrayAsync(cancellationToken);

        var rolesByUser = roleRows
            .GroupBy(x => x.UserId)
            .ToDictionary(
                group => group.Key,
                group => group
                    .Select(x => x.RoleName)
                    .OrderBy(x => x)
                    .ToArray());

        var studentsByUser = studentRows
            .GroupBy(x => x.UserId)
            .ToDictionary(
                group => group.Key,
                group => group.First());

        var contextsByUser = studentContexts
            .Concat(teacherContexts)
            .GroupBy(x => x.UserId)
            .ToDictionary(
                group => group.Key,
                group => group
                    .GroupBy(x => new
                    {
                        x.YearId,
                        x.ProgramId,
                        x.ClassId
                    })
                    .Select(context => context.First())
                    .OrderBy(x => x.YearName)
                    .ThenBy(x => x.ProgramName)
                    .ThenBy(x => x.ClassName)
                    .Select(x => new SchoolUserAcademicContext(
                        x.YearId,
                        x.YearName,
                        x.ProgramId,
                        x.ProgramName,
                        x.ClassId,
                        x.ClassName))
                    .ToArray());

        return new SchoolUserPage(
            users.Select(user =>
            {
                var record = ToRecord(
                    user,
                    rolesByUser.TryGetValue(user.Id, out var roles)
                        ? roles
                        : []);

                var student = studentsByUser.GetValueOrDefault(user.Id);

                return record with
                {
                    DisplayName = student?.DisplayName,
                    StudentNumber = student?.StudentNumber,
                    AcademicContexts =
                        contextsByUser.TryGetValue(user.Id, out var contexts)
                            ? contexts
                            : []
                };
            }).ToArray(),
            page,
            pageSize,
            total);
    }

    public async Task<IReadOnlyList<SchoolUserRecord>>
        ListBySchoolAndIdsAsync(
            Guid schoolId,
            IReadOnlyCollection<Guid> userIds,
            CancellationToken cancellationToken = default)
    {
        if (userIds.Count == 0)
            return [];

        var ids = userIds
            .Where(x => x != Guid.Empty)
            .Distinct()
            .ToArray();
        if (ids.Length == 0)
            return [];

        var users = await _context.Users
            .AsNoTracking()
            .Where(x => x.SchoolId == schoolId && ids.Contains(x.Id))
            .OrderBy(x => x.Email)
            .ToArrayAsync(cancellationToken);

        if (users.Length == 0)
            return [];

        var foundIds = users.Select(x => x.Id).ToArray();
        var roleRows = await (
                from userRole in _context.UserRoles
                join role in _context.Roles
                    on userRole.RoleId equals role.Id
                where foundIds.Contains(userRole.UserId)
                select new
                {
                    userRole.UserId,
                    RoleName = role.Name!
                })
            .AsNoTracking()
            .ToArrayAsync(cancellationToken);

        var rolesByUser = roleRows
            .GroupBy(x => x.UserId)
            .ToDictionary(
                group => group.Key,
                group => group.Select(x => x.RoleName).OrderBy(x => x).ToArray());

        return users.Select(user => ToRecord(
                user,
                rolesByUser.TryGetValue(user.Id, out var roles)
                    ? roles
                    : []))
            .ToArray();
    }

    public async Task<SchoolUserRecord?>
        GetBySchoolAndIdAsync(
            Guid schoolId,
            Guid userId,
            CancellationToken cancellationToken = default)
    {
        var user = await _userManager.Users
            .SingleOrDefaultAsync(
                x =>
                    x.Id == userId &&
                    x.SchoolId == schoolId,
                cancellationToken);

        if (user is null)
        {
            return null;
        }

        var roles = await _userManager.GetRolesAsync(
            user);

        return ToRecord(
            user,
            roles);
    }

    public async Task<SchoolUserPersistenceResult>
        CreateAsync(
            Guid schoolId,
            string email,
            string role,
            CancellationToken cancellationToken = default)
    {
        if (!await _roleManager.RoleExistsAsync(role))
        {
            return SchoolUserPersistenceResult.Failure(
                SchoolUserPersistenceError.RoleFailure);
        }

        var now = DateTime.UtcNow;

        var user = new ApplicationUser
        {
            Id = Guid.NewGuid(),
            SchoolId = schoolId,
            Email = email,
            UserName = email,
            IsActive = true,
            LockoutEnabled = true,
            CreatedAtUtc = now,
            UpdatedAtUtc = now
        };

        var create =
            await _userManager.CreateAsync(user);

        if (!create.Succeeded)
        {
            return SchoolUserPersistenceResult.Failure(
                MapIdentityErrors(create));
        }

        var roleResult =
            await _userManager.AddToRoleAsync(
                user,
                role);

        if (!roleResult.Succeeded)
        {
            await _userManager.DeleteAsync(user);

            return SchoolUserPersistenceResult.Failure(
                SchoolUserPersistenceError.RoleFailure);
        }

        var token =
            await _userManager
                .GeneratePasswordResetTokenAsync(user);

        var roles =
            await _userManager.GetRolesAsync(user);

        return SchoolUserPersistenceResult.Success(
            ToRecord(user, roles),
            token);
    }

    public async Task<SchoolUserPersistenceResult>
        SetActiveAsync(
            Guid schoolId,
            Guid userId,
            bool isActive,
            CancellationToken cancellationToken = default)
    {
        var user = await FindTenantUserAsync(
            schoolId,
            userId,
            cancellationToken);

        if (user is null)
        {
            return SchoolUserPersistenceResult.Failure(
                SchoolUserPersistenceError.NotFound);
        }

        user.IsActive = isActive;
        user.UpdatedAtUtc = DateTime.UtcNow;

        var update =
            await _userManager.UpdateAsync(user);

        if (!update.Succeeded)
        {
            return SchoolUserPersistenceResult.Failure(
                MapIdentityErrors(update));
        }

        if (!isActive)
        {
            var stamp =
                await _userManager
                    .UpdateSecurityStampAsync(user);

            if (!stamp.Succeeded)
            {
                return SchoolUserPersistenceResult.Failure(
                    MapIdentityErrors(stamp));
            }
        }

        return await SuccessForUserAsync(user);
    }

    public async Task<SchoolUserPersistenceResult>
        SetLockedAsync(
            Guid schoolId,
            Guid userId,
            bool isLocked,
            CancellationToken cancellationToken = default)
    {
        var user = await FindTenantUserAsync(
            schoolId,
            userId,
            cancellationToken);

        if (user is null)
        {
            return SchoolUserPersistenceResult.Failure(
                SchoolUserPersistenceError.NotFound);
        }

        IdentityResult lockResult;

        if (isLocked)
        {
            lockResult =
                await _userManager.SetLockoutEndDateAsync(
                    user,
                    DateTimeOffset.UtcNow.AddYears(100));
        }
        else
        {
            lockResult =
                await _userManager.SetLockoutEndDateAsync(
                    user,
                    null);

            if (lockResult.Succeeded)
            {
                lockResult =
                    await _userManager
                        .ResetAccessFailedCountAsync(user);
            }
        }

        if (!lockResult.Succeeded)
        {
            return SchoolUserPersistenceResult.Failure(
                MapIdentityErrors(lockResult));
        }

        user.UpdatedAtUtc = DateTime.UtcNow;

        var update =
            await _userManager.UpdateAsync(user);

        if (!update.Succeeded)
        {
            return SchoolUserPersistenceResult.Failure(
                MapIdentityErrors(update));
        }

        if (isLocked)
        {
            var stamp =
                await _userManager
                    .UpdateSecurityStampAsync(user);

            if (!stamp.Succeeded)
            {
                return SchoolUserPersistenceResult.Failure(
                    MapIdentityErrors(stamp));
            }
        }

        return await SuccessForUserAsync(user);
    }

    public async Task<SchoolUserPersistenceResult>
        SetRoleAsync(
            Guid schoolId,
            Guid userId,
            string role,
            CancellationToken cancellationToken = default)
    {
        if (!await _roleManager.RoleExistsAsync(role))
        {
            return SchoolUserPersistenceResult.Failure(
                SchoolUserPersistenceError.RoleFailure);
        }

        var user = await FindTenantUserAsync(
            schoolId,
            userId,
            cancellationToken);

        if (user is null)
        {
            return SchoolUserPersistenceResult.Failure(
                SchoolUserPersistenceError.NotFound);
        }

        var oldRoles =
            await _userManager.GetRolesAsync(user);

        if (oldRoles.Count == 1 &&
            oldRoles[0] == role)
        {
            return SchoolUserPersistenceResult.Success(
                ToRecord(user, oldRoles));
        }

        if (oldRoles.Count > 0)
        {
            var remove =
                await _userManager.RemoveFromRolesAsync(
                    user,
                    oldRoles);

            if (!remove.Succeeded)
            {
                return SchoolUserPersistenceResult.Failure(
                    MapIdentityErrors(remove));
            }
        }

        var add =
            await _userManager.AddToRoleAsync(
                user,
                role);

        if (!add.Succeeded)
        {
            foreach (var oldRole in oldRoles)
            {
                await _userManager.AddToRoleAsync(
                    user,
                    oldRole);
            }

            return SchoolUserPersistenceResult.Failure(
                SchoolUserPersistenceError.RoleFailure);
        }

        user.UpdatedAtUtc = DateTime.UtcNow;

        var update =
            await _userManager.UpdateAsync(user);

        if (!update.Succeeded)
        {
            return SchoolUserPersistenceResult.Failure(
                MapIdentityErrors(update));
        }

        var stamp =
            await _userManager.UpdateSecurityStampAsync(
                user);

        if (!stamp.Succeeded)
        {
            return SchoolUserPersistenceResult.Failure(
                MapIdentityErrors(stamp));
        }

        return await SuccessForUserAsync(user);
    }

    public async Task<SchoolUserPersistenceResult>
        GeneratePasswordSetupAsync(
            Guid schoolId,
            Guid userId,
            CancellationToken cancellationToken = default)
    {
        var user = await FindTenantUserAsync(
            schoolId,
            userId,
            cancellationToken);

        if (user is null)
        {
            return SchoolUserPersistenceResult.Failure(
                SchoolUserPersistenceError.NotFound);
        }

        // Generating a password setup/reset email must not
        // disable an existing password. The current password
        // remains valid until ResetPasswordAsync succeeds.
        user.UpdatedAtUtc = DateTime.UtcNow;

        var update =
            await _userManager.UpdateAsync(user);

        if (!update.Succeeded)
        {
            return SchoolUserPersistenceResult.Failure(
                MapIdentityErrors(update));
        }

        var token =
            await _userManager
                .GeneratePasswordResetTokenAsync(user);

        var roles =
            await _userManager.GetRolesAsync(user);

        return SchoolUserPersistenceResult.Success(
            ToRecord(user, roles),
            token);
    }

    public async Task<SchoolUserPersistenceResult>
        CompletePasswordSetupAsync(
            Guid userId,
            string token,
            string newPassword,
            CancellationToken cancellationToken = default)
    {
        var user =
            await _userManager.FindByIdAsync(
                userId.ToString());

        if (user is null ||
            !user.SchoolId.HasValue)
        {
            return SchoolUserPersistenceResult.Failure(
                SchoolUserPersistenceError.InvalidToken);
        }

        var reset =
            await _userManager.ResetPasswordAsync(
                user,
                token,
                newPassword);

        if (!reset.Succeeded)
        {
            var invalidToken =
                reset.Errors.Any(
                    x => x.Code.Contains(
                        "InvalidToken",
                        StringComparison.OrdinalIgnoreCase));

            return SchoolUserPersistenceResult.Failure(
                invalidToken
                    ? SchoolUserPersistenceError.InvalidToken
                    : SchoolUserPersistenceError.PasswordPolicy);
        }

        user.UpdatedAtUtc = DateTime.UtcNow;

        var update =
            await _userManager.UpdateAsync(user);

        if (!update.Succeeded)
        {
            return SchoolUserPersistenceResult.Failure(
                MapIdentityErrors(update));
        }

        return await SuccessForUserAsync(user);
    }

    private Task<ApplicationUser?>
        FindTenantUserAsync(
            Guid schoolId,
            Guid userId,
            CancellationToken cancellationToken) =>
        _userManager.Users.SingleOrDefaultAsync(
            x =>
                x.Id == userId &&
                x.SchoolId == schoolId,
            cancellationToken);

    private async Task<SchoolUserPersistenceResult>
        SuccessForUserAsync(
            ApplicationUser user)
    {
        var roles =
            await _userManager.GetRolesAsync(user);

        return SchoolUserPersistenceResult.Success(
            ToRecord(user, roles));
    }

    private static SchoolUserRecord ToRecord(
        ApplicationUser user,
        IEnumerable<string> roles) =>
        new(
            user.Id,
            user.SchoolId,
            user.Email ?? string.Empty,
            user.IsActive,
            user.LockoutEnabled &&
            user.LockoutEnd.HasValue &&
            user.LockoutEnd.Value > DateTimeOffset.UtcNow,
            user.CreatedAtUtc,
            user.UpdatedAtUtc,
            roles.ToArray());

    private static SchoolUserPersistenceError
        MapIdentityErrors(
            IdentityResult result)
    {
        if (result.Errors.Any(
                x =>
                    x.Code.Equals(
                        "DuplicateEmail",
                        StringComparison.OrdinalIgnoreCase) ||
                    x.Code.Equals(
                        "DuplicateUserName",
                        StringComparison.OrdinalIgnoreCase)))
        {
            return SchoolUserPersistenceError.DuplicateEmail;
        }

        if (result.Errors.Any(
                x => x.Code.Equals(
                    "ConcurrencyFailure",
                    StringComparison.OrdinalIgnoreCase)))
        {
            return SchoolUserPersistenceError.Conflict;
        }

        return SchoolUserPersistenceError.IdentityFailure;
    }
}
