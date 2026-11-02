using Edulytics.Core.Constants;
using Edulytics.Core.Entities;
using Edulytics.Core.Enums;
using Edulytics.Core.Interfaces;
using Edulytics.Core.Users;
using Edulytics.Services.Users;

namespace Edulytics.Tests.Phase05;

public sealed class RoleHierarchyWorkflowTests
{
    [Fact]
    public async Task SchoolAdmin_CanCreateSubjectSupervisor()
    {
        var fixture = NewFixture(RoleNames.SchoolAdmin);

        var result = await fixture.Service.CreateAsync(
            fixture.Actor.Id,
            fixture.School.Id,
            new CreateSchoolUserRequest(
                "supervisor@example.com",
                RoleNames.SubjectSupervisor));

        Assert.True(result.Succeeded);

        var created = Assert.Single(
            fixture.Users.Users.Values,
            x => x.Id == result.UserId);

        Assert.Equal(
            RoleNames.SubjectSupervisor,
            Assert.Single(created.Roles));
    }

    [Theory]
    [InlineData(RoleNames.Teacher)]
    [InlineData(RoleNames.Student)]
    [InlineData(RoleNames.SchoolAdmin)]
    public async Task SchoolAdmin_CannotCreateOtherRoles(
        string role)
    {
        var fixture = NewFixture(RoleNames.SchoolAdmin);

        var result = await fixture.Service.CreateAsync(
            fixture.Actor.Id,
            fixture.School.Id,
            new CreateSchoolUserRequest(
                "blocked@example.com",
                role));

        Assert.False(result.Succeeded);
        Assert.Contains(
            result.Errors,
            x => x.Code == SchoolUserErrorCode.UserAccessDenied);
    }

    [Fact]
    public async Task SchoolAdmin_CanManageSubjectSupervisorAccount()
    {
        var fixture = NewFixture(RoleNames.SchoolAdmin);
        var supervisor = NewUser(
            fixture.School.Id,
            RoleNames.SubjectSupervisor);

        fixture.Users.Seed(supervisor);

        var result = await fixture.Service.SetLockedAsync(
            fixture.Actor.Id,
            fixture.School.Id,
            supervisor.Id,
            true);

        Assert.True(result.Succeeded);
        Assert.True(fixture.Users.Users[supervisor.Id].IsLocked);
    }

    [Theory]
    [InlineData(RoleNames.Teacher)]
    [InlineData(RoleNames.Student)]
    [InlineData(RoleNames.SchoolAdmin)]
    public async Task SchoolAdmin_CannotManageNonSupervisorAccounts(
        string targetRole)
    {
        var fixture = NewFixture(RoleNames.SchoolAdmin);
        var target = NewUser(
            fixture.School.Id,
            targetRole);

        fixture.Users.Seed(target);

        var result = await fixture.Service.SetLockedAsync(
            fixture.Actor.Id,
            fixture.School.Id,
            target.Id,
            true);

        Assert.False(result.Succeeded);
        Assert.Contains(
            result.Errors,
            x => x.Code == SchoolUserErrorCode.UserAccessDenied);
    }

    [Fact]
    public async Task SchoolAdmin_CannotChangeSubjectSupervisorRole()
    {
        var fixture = NewFixture(RoleNames.SchoolAdmin);
        var supervisor = NewUser(
            fixture.School.Id,
            RoleNames.SubjectSupervisor);

        fixture.Users.Seed(supervisor);

        var result = await fixture.Service.ChangeRoleAsync(
            fixture.Actor.Id,
            fixture.School.Id,
            supervisor.Id,
            RoleNames.Teacher);

        Assert.False(result.Succeeded);
        Assert.Contains(
            result.Errors,
            x => x.Code == SchoolUserErrorCode.UserAccessDenied);
    }

    [Theory]
    [InlineData(RoleNames.Teacher)]
    [InlineData(RoleNames.Student)]
    public async Task SubjectSupervisor_CanCreateOperationalUsers(
        string role)
    {
        var fixture = NewFixture(RoleNames.SubjectSupervisor);

        var result = await fixture.Service.CreateAsync(
            fixture.Actor.Id,
            fixture.School.Id,
            new CreateSchoolUserRequest(
                $"{role.ToLowerInvariant()}@example.com",
                role));

        Assert.True(result.Succeeded);
    }

    [Fact]
    public async Task SubjectSupervisor_CannotCreateSubjectSupervisor()
    {
        var fixture = NewFixture(RoleNames.SubjectSupervisor);

        var result = await fixture.Service.CreateAsync(
            fixture.Actor.Id,
            fixture.School.Id,
            new CreateSchoolUserRequest(
                "another-supervisor@example.com",
                RoleNames.SubjectSupervisor));

        Assert.False(result.Succeeded);
        Assert.Contains(
            result.Errors,
            x => x.Code == SchoolUserErrorCode.UserAccessDenied);
    }

    [Fact]
    public async Task SubjectSupervisor_CannotManageAnotherSupervisor()
    {
        var fixture = NewFixture(RoleNames.SubjectSupervisor);
        var target = NewUser(
            fixture.School.Id,
            RoleNames.SubjectSupervisor);

        fixture.Users.Seed(target);

        var result = await fixture.Service.SetActiveAsync(
            fixture.Actor.Id,
            fixture.School.Id,
            target.Id,
            false);

        Assert.False(result.Succeeded);
        Assert.Contains(
            result.Errors,
            x => x.Code == SchoolUserErrorCode.UserAccessDenied);
    }

    private static Fixture NewFixture(string actorRole)
    {
        var school = NewSchool();
        var actor = NewUser(school.Id, actorRole);
        var users = new FakeUserRepository();
        var schools = new FakeSchoolRepository();

        users.Seed(actor);
        schools.Seed(school);

        return new Fixture(
            school,
            actor,
            users,
            new SchoolUserManagementService(users, schools));
    }

    private static School NewSchool() =>
        new()
        {
            Id = Guid.NewGuid(),
            Name = "Hierarchy Test School",
            SchoolCode = "HIER",
            NormalizedSchoolCode = "HIER",
            Status = SchoolStatus.Active,
            CountryCode = "PL",
            City = "Warsaw",
            ContactEmail = "school@example.com",
            DefaultCulture = "en",
            TimeZoneId = "Europe/Warsaw",
            CreatedAtUtc = DateTime.UtcNow,
            UpdatedAtUtc = DateTime.UtcNow,
            RowVersion = BitConverter.GetBytes(1L)
        };

    private static SchoolUserRecord NewUser(
        Guid? schoolId,
        string role) =>
        new(
            Guid.NewGuid(),
            schoolId,
            $"{Guid.NewGuid():N}@example.com",
            true,
            false,
            DateTime.UtcNow,
            DateTime.UtcNow,
            [role]);

    private sealed record Fixture(
        School School,
        SchoolUserRecord Actor,
        FakeUserRepository Users,
        SchoolUserManagementService Service);

    private sealed class FakeUserRepository
        : ISchoolUserRepository
    {
        public Dictionary<Guid, SchoolUserRecord> Users { get; } = [];

        public void Seed(SchoolUserRecord user) =>
            Users[user.Id] = user;

        public Task<SchoolUserRecord?> GetActorAsync(
            Guid userId,
            CancellationToken cancellationToken = default) =>
            Task.FromResult(Users.GetValueOrDefault(userId));

        public Task<IReadOnlyList<SchoolUserRecord>> ListBySchoolAsync(
            Guid schoolId,
            CancellationToken cancellationToken = default) =>
            Task.FromResult<IReadOnlyList<SchoolUserRecord>>(
                Users.Values
                    .Where(x => x.SchoolId == schoolId)
                    .ToArray());

        public Task<SchoolUserRecord?> GetBySchoolAndIdAsync(
            Guid schoolId,
            Guid userId,
            CancellationToken cancellationToken = default)
        {
            var user = Users.GetValueOrDefault(userId);

            return Task.FromResult(
                user?.SchoolId == schoolId ? user : null);
        }

        public Task<SchoolUserPersistenceResult> CreateAsync(
            Guid schoolId,
            string email,
            string role,
            CancellationToken cancellationToken = default)
        {
            var user = new SchoolUserRecord(
                Guid.NewGuid(),
                schoolId,
                email,
                true,
                false,
                DateTime.UtcNow,
                DateTime.UtcNow,
                [role]);

            Users[user.Id] = user;

            return Task.FromResult(
                SchoolUserPersistenceResult.Success(
                    user,
                    "test-token"));
        }

        public Task<SchoolUserPersistenceResult> SetActiveAsync(
            Guid schoolId,
            Guid userId,
            bool isActive,
            CancellationToken cancellationToken = default) =>
            Update(
                schoolId,
                userId,
                user => user with
                {
                    IsActive = isActive,
                    UpdatedAtUtc = DateTime.UtcNow
                });

        public Task<SchoolUserPersistenceResult> SetLockedAsync(
            Guid schoolId,
            Guid userId,
            bool isLocked,
            CancellationToken cancellationToken = default) =>
            Update(
                schoolId,
                userId,
                user => user with
                {
                    IsLocked = isLocked,
                    UpdatedAtUtc = DateTime.UtcNow
                });

        public Task<SchoolUserPersistenceResult> SetRoleAsync(
            Guid schoolId,
            Guid userId,
            string role,
            CancellationToken cancellationToken = default) =>
            Update(
                schoolId,
                userId,
                user => user with
                {
                    Roles = [role],
                    UpdatedAtUtc = DateTime.UtcNow
                });

        public async Task<SchoolUserPersistenceResult>
            GeneratePasswordSetupAsync(
                Guid schoolId,
                Guid userId,
                CancellationToken cancellationToken = default)
        {
            var user = await GetBySchoolAndIdAsync(
                schoolId,
                userId,
                cancellationToken);

            return user is null
                ? SchoolUserPersistenceResult.Failure(
                    SchoolUserPersistenceError.NotFound)
                : SchoolUserPersistenceResult.Success(
                    user,
                    "test-token");
        }

        public Task<SchoolUserPersistenceResult> CompletePasswordSetupAsync(
            Guid userId,
            string token,
            string newPassword,
            CancellationToken cancellationToken = default)
        {
            var user = Users.GetValueOrDefault(userId);

            return Task.FromResult(
                user is null
                    ? SchoolUserPersistenceResult.Failure(
                        SchoolUserPersistenceError.InvalidToken)
                    : SchoolUserPersistenceResult.Success(user));
        }

        private Task<SchoolUserPersistenceResult> Update(
            Guid schoolId,
            Guid userId,
            Func<SchoolUserRecord, SchoolUserRecord> update)
        {
            var user = Users.GetValueOrDefault(userId);

            if (user is null || user.SchoolId != schoolId)
            {
                return Task.FromResult(
                    SchoolUserPersistenceResult.Failure(
                        SchoolUserPersistenceError.NotFound));
            }

            var changed = update(user);
            Users[userId] = changed;

            return Task.FromResult(
                SchoolUserPersistenceResult.Success(changed));
        }
    }

    private sealed class FakeSchoolRepository
        : ISchoolRepository
    {
        private readonly List<School> _schools = [];

        public void Seed(School school) =>
            _schools.Add(school);

        public Task<IReadOnlyList<School>> ListAsync(
            CancellationToken cancellationToken = default) =>
            Task.FromResult<IReadOnlyList<School>>(
                _schools.ToArray());

        public Task<School?> GetByIdAsync(
            Guid id,
            CancellationToken cancellationToken = default) =>
            Task.FromResult(
                _schools.SingleOrDefault(x => x.Id == id));

        public Task<School?> GetForUpdateAsync(
            Guid id,
            CancellationToken cancellationToken = default) =>
            GetByIdAsync(id, cancellationToken);

        public Task<bool> ExistsByNormalizedCodeAsync(
            string normalizedSchoolCode,
            CancellationToken cancellationToken = default) =>
            Task.FromResult(
                _schools.Any(
                    x => x.NormalizedSchoolCode == normalizedSchoolCode));

        public Task AddAsync(
            School school,
            CancellationToken cancellationToken = default)
        {
            _schools.Add(school);
            return Task.CompletedTask;
        }

        public Task<SchoolRepositoryWriteResult> SaveAsync(
            School school,
            byte[]? expectedRowVersion,
            CancellationToken cancellationToken = default) =>
            Task.FromResult(SchoolRepositoryWriteResult.Success);
    }
}
