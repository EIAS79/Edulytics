using Edulytics.Core.Analytics;
using Edulytics.Core.Constants;
using Edulytics.Core.Entities;
using Edulytics.Core.Enums;
using Edulytics.Core.Interfaces;
using Edulytics.Core.Reports;
using Edulytics.Core.Users;
using Edulytics.Data.Contexts;
using Edulytics.Data.Repositories;
using Edulytics.Services.Auditing;
using Edulytics.Services.Reports;
using Microsoft.EntityFrameworkCore;

namespace Edulytics.Tests.Phase43;

public sealed class Phase43PrivacyAndReportBoundaryTests
{
    [Fact]
    public async Task Phase43_PrivatePracticeAttempts_AreExcludedFromOfficialAnalyticsSource()
    {
        await using var db = CreateDb();

        var schoolId = Guid.NewGuid();
        var official = NewPracticeAttempt(schoolId, isPrivate: false);
        var privateAttempt = NewPracticeAttempt(schoolId, isPrivate: true);

        await db.PracticeAttempts.AddRangeAsync(official, privateAttempt);
        await db.SaveChangesAsync();

        var snapshot = await new AnalyticsRepository(db)
            .GetSourceSnapshotAsync(schoolId);

        var attempts = Assert.IsAssignableFrom<IReadOnlyList<PracticeAttempt>>(
            snapshot.PracticeAttempts);

        var returned = Assert.Single(attempts);
        Assert.Equal(official.Id, returned.Id);
        Assert.False(returned.IsPrivate);
        Assert.DoesNotContain(attempts, x => x.Id == privateAttempt.Id);
    }

    [Fact]
    public async Task Phase43_PrivateLearningEvidence_IsExcludedFromOfficialAnalyticsSource()
    {
        await using var db = CreateDb();

        var schoolId = Guid.NewGuid();
        var official = NewPracticeAttempt(schoolId, isPrivate: false);
        var privateAttempt = NewPracticeAttempt(schoolId, isPrivate: true);
        var officialEvidence = NewEvidence(schoolId, official.Id);
        var privateEvidence = NewEvidence(schoolId, privateAttempt.Id);

        await db.PracticeAttempts.AddRangeAsync(official, privateAttempt);
        await db.LearningEvidence.AddRangeAsync(officialEvidence, privateEvidence);
        await db.SaveChangesAsync();

        var snapshot = await new AnalyticsRepository(db)
            .GetSourceSnapshotAsync(schoolId);

        var evidence = Assert.IsAssignableFrom<IReadOnlyList<LearningEvidence>>(
            snapshot.LearningEvidence);

        var returned = Assert.Single(evidence);
        Assert.Equal(officialEvidence.Id, returned.Id);
        Assert.Equal(official.Id, returned.PracticeAttemptId);
        Assert.DoesNotContain(evidence, x => x.Id == privateEvidence.Id);
    }

    [Fact]
    public async Task Phase43_ReportFilter_WithClassIdFromAnotherSchool_IsRejected()
    {
        var fixture = ReportFixture.Create();
        var otherSchoolClassId = Guid.NewGuid();

        var result = await fixture.Reports.ValidateAsync(
            fixture.Admin.Id,
            new ReportRequest(
                ReportKind.Class,
                ClassGroupId: otherSchoolClassId));

        Assert.Null(result.Value);
        Assert.Equal(ReportErrorCode.AccessDenied, result.Error);
    }

    [Fact]
    public async Task Phase43_Export_CannotBypassReportValidation()
    {
        var fixture = ReportFixture.Create();
        var otherSchoolClassId = Guid.NewGuid();

        var result = await fixture.Exports.RequestAsync(
            fixture.Admin.Id,
            new ReportRequest(
                ReportKind.Class,
                ClassGroupId: otherSchoolClassId),
            ReportExportFormat.Csv,
            "en");

        Assert.False(result.Succeeded);
        Assert.Equal(ReportErrorCode.AccessDenied, result.Error);
        Assert.Empty(fixture.ExportRepository.Jobs);
        Assert.Empty(fixture.ExportRepository.Outbox);
        Assert.Empty(fixture.Audit.Events);
    }

    private static EdulyticsDbContext CreateDb()
    {
        var options = new DbContextOptionsBuilder<EdulyticsDbContext>()
            .UseInMemoryDatabase($"phase43-{Guid.NewGuid():N}")
            .Options;

        return new EdulyticsDbContext(options);
    }

    private static PracticeAttempt NewPracticeAttempt(
        Guid schoolId,
        bool isPrivate) =>
        new()
        {
            Id = Guid.NewGuid(),
            SchoolId = schoolId,
            StudentProfileId = Guid.NewGuid(),
            CurriculumAdoptionId = Guid.NewGuid(),
            IsPrivate = isPrivate,
            StartedAtUtc = DateTime.UtcNow
        };

    private static LearningEvidence NewEvidence(
        Guid schoolId,
        Guid practiceAttemptId) =>
        new()
        {
            Id = Guid.NewGuid(),
            SchoolId = schoolId,
            StudentProfileId = Guid.NewGuid(),
            LearningOutcomeId = Guid.NewGuid(),
            PracticeAttemptId = practiceAttemptId,
            AssessmentItemId = Guid.NewGuid(),
            OccurredAtUtc = DateTime.UtcNow
        };

    private sealed class ReportFixture
    {
        public required SchoolUserRecord Admin { get; init; }
        public required ReportQueryService Reports { get; init; }
        public required ReportExportService Exports { get; init; }
        public required FakeExportRepository ExportRepository { get; init; }
        public required FakeAuditService Audit { get; init; }

        public static ReportFixture Create()
        {
            var now = DateTime.UtcNow;
            var school = new School
            {
                Id = Guid.NewGuid(),
                Name = "Phase43 School",
                SchoolCode = "P43",
                NormalizedSchoolCode = "P43",
                Status = SchoolStatus.Active,
                CountryCode = "PL",
                City = "Warsaw",
                ContactEmail = "phase43@example.com",
                DefaultCulture = "en",
                TimeZoneId = "Europe/Warsaw",
                CreatedAtUtc = now,
                UpdatedAtUtc = now
            };

            var admin = NewUser(school.Id, RoleNames.SchoolAdmin);
            var year = new AcademicYear
            {
                Id = Guid.NewGuid(),
                SchoolId = school.Id,
                Name = "2026/27",
                StartsOn = new DateOnly(2026, 9, 1),
                EndsOn = new DateOnly(2027, 6, 30),
                Status = AcademicStructureStatus.Active
            };

            var classGroup = new ClassGroup
            {
                Id = Guid.NewGuid(),
                SchoolId = school.Id,
                AcademicYearId = year.Id,
                GradeLevelId = Guid.NewGuid(),
                Name = "Class A",
                Code = "A",
                NormalizedCode = "A",
                Status = AcademicStructureStatus.Active
            };

            var users = new FakeUserRepository();
            users.Seed(admin);

            var schools = new FakeSchoolRepository();
            schools.Seed(school);

            var projection = new AnalyticsProjectionSnapshot(
                [year],
                [classGroup],
                [],
                [],
                [],
                [],
                [],
                [],
                [],
                [],
                [],
                []);

            var reports = new ReportQueryService(
                new FakeAnalyticsRepository(projection),
                schools,
                users,
                new FakeAssignmentRepository());

            var exportRepository = new FakeExportRepository();
            var audit = new FakeAuditService();
            var exports = new ReportExportService(
                reports,
                exportRepository,
                audit,
                new FakeMetadataProvider(),
                new ReportOptions());

            return new ReportFixture
            {
                Admin = admin,
                Reports = reports,
                Exports = exports,
                ExportRepository = exportRepository,
                Audit = audit
            };
        }
    }

    private static SchoolUserRecord NewUser(Guid schoolId, string role) =>
        new(
            Guid.NewGuid(),
            schoolId,
            $"{Guid.NewGuid():N}@example.com",
            true,
            false,
            DateTime.UtcNow,
            DateTime.UtcNow,
            [role]);

    private sealed class FakeAnalyticsRepository : IAnalyticsRepository
    {
        private readonly AnalyticsProjectionSnapshot _projection;

        public FakeAnalyticsRepository(AnalyticsProjectionSnapshot projection)
        {
            _projection = projection;
        }

        public Task<AnalyticsSourceSnapshot> GetSourceSnapshotAsync(
            Guid schoolId,
            CancellationToken cancellationToken = default) =>
            Task.FromResult(new AnalyticsSourceSnapshot(
                [], [], [], [], [], [], [], [], [], [], [], [], []));

        public Task<AnalyticsProjectionSnapshot> GetProjectionSnapshotAsync(
            Guid schoolId,
            CancellationToken cancellationToken = default) =>
            Task.FromResult(_projection);

        public Task<DateTime?> GetLatestSourceUpdateAsync(
            Guid schoolId,
            CancellationToken cancellationToken = default) =>
            Task.FromResult<DateTime?>(null);

        public Task<AnalyticsPersistenceResult> ReplaceProjectionsAsync(
            Guid schoolId,
            AnalyticsProjectionSet projections,
            CancellationToken cancellationToken = default) =>
            Task.FromResult(AnalyticsPersistenceResult.Success());
    }

    private sealed class FakeAssignmentRepository
        : ISubjectSupervisorAssignmentRepository
    {
        public Task<IReadOnlyList<SubjectSupervisorAssignment>> ListBySchoolAsync(
            Guid schoolId,
            CancellationToken cancellationToken = default) =>
            Task.FromResult<IReadOnlyList<SubjectSupervisorAssignment>>([]);

        public Task<IReadOnlyList<SubjectSupervisorAssignment>> ListActiveBySupervisorAsync(
            Guid schoolId,
            Guid supervisorUserId,
            CancellationToken cancellationToken = default) =>
            Task.FromResult<IReadOnlyList<SubjectSupervisorAssignment>>([]);

        public Task<IReadOnlyList<Subject>> ListSubjectsAsync(
            Guid schoolId,
            CancellationToken cancellationToken = default) =>
            Task.FromResult<IReadOnlyList<Subject>>([]);

        public Task<Subject?> GetSubjectAsync(
            Guid schoolId,
            Guid subjectId,
            CancellationToken cancellationToken = default) =>
            Task.FromResult<Subject?>(null);

        public Task<SubjectSupervisorAssignment?> GetBySchoolAndIdAsync(
            Guid schoolId,
            Guid assignmentId,
            CancellationToken cancellationToken = default) =>
            Task.FromResult<SubjectSupervisorAssignment?>(null);

        public Task<bool> ExistsAsync(
            Guid schoolId,
            Guid supervisorUserId,
            Guid subjectId,
            CancellationToken cancellationToken = default) =>
            Task.FromResult(false);

        public Task AddAsync(
            SubjectSupervisorAssignment assignment,
            CancellationToken cancellationToken = default) =>
            Task.CompletedTask;

        public void Remove(SubjectSupervisorAssignment assignment)
        {
        }

        public Task<bool> SaveAsync(
            CancellationToken cancellationToken = default) =>
            Task.FromResult(true);
    }

    private sealed class FakeUserRepository : ISchoolUserRepository
    {
        private readonly Dictionary<Guid, SchoolUserRecord> _users = [];

        public void Seed(SchoolUserRecord user) => _users[user.Id] = user;

        public Task<SchoolUserRecord?> GetActorAsync(
            Guid userId,
            CancellationToken cancellationToken = default) =>
            Task.FromResult(_users.GetValueOrDefault(userId));

        public Task<IReadOnlyList<SchoolUserRecord>> ListBySchoolAsync(
            Guid schoolId,
            CancellationToken cancellationToken = default) =>
            Task.FromResult<IReadOnlyList<SchoolUserRecord>>(
                _users.Values.Where(x => x.SchoolId == schoolId).ToArray());

        public Task<SchoolUserRecord?> GetBySchoolAndIdAsync(
            Guid schoolId,
            Guid userId,
            CancellationToken cancellationToken = default)
        {
            var user = _users.GetValueOrDefault(userId);
            return Task.FromResult(user?.SchoolId == schoolId ? user : null);
        }

        public Task<SchoolUserPersistenceResult> CreateAsync(
            Guid schoolId,
            string email,
            string role,
            CancellationToken cancellationToken = default) => Unsupported();

        public Task<SchoolUserPersistenceResult> SetActiveAsync(
            Guid schoolId,
            Guid userId,
            bool isActive,
            CancellationToken cancellationToken = default) => Unsupported();

        public Task<SchoolUserPersistenceResult> SetLockedAsync(
            Guid schoolId,
            Guid userId,
            bool isLocked,
            CancellationToken cancellationToken = default) => Unsupported();

        public Task<SchoolUserPersistenceResult> SetRoleAsync(
            Guid schoolId,
            Guid userId,
            string role,
            CancellationToken cancellationToken = default) => Unsupported();

        public Task<SchoolUserPersistenceResult> GeneratePasswordSetupAsync(
            Guid schoolId,
            Guid userId,
            CancellationToken cancellationToken = default) => Unsupported();

        public Task<SchoolUserPersistenceResult> CompletePasswordSetupAsync(
            Guid userId,
            string token,
            string newPassword,
            CancellationToken cancellationToken = default) => Unsupported();

        private static Task<SchoolUserPersistenceResult> Unsupported() =>
            Task.FromResult(
                SchoolUserPersistenceResult.Failure(
                    SchoolUserPersistenceError.NotFound));
    }

    private sealed class FakeSchoolRepository : ISchoolRepository
    {
        private readonly Dictionary<Guid, School> _schools = [];

        public void Seed(School school) => _schools[school.Id] = school;

        public Task<IReadOnlyList<School>> ListAsync(
            CancellationToken cancellationToken = default) =>
            Task.FromResult<IReadOnlyList<School>>(_schools.Values.ToArray());

        public Task<School?> GetByIdAsync(
            Guid id,
            CancellationToken cancellationToken = default) =>
            Task.FromResult(_schools.GetValueOrDefault(id));

        public Task<School?> GetForUpdateAsync(
            Guid id,
            CancellationToken cancellationToken = default) =>
            GetByIdAsync(id, cancellationToken);

        public Task<bool> ExistsByNormalizedCodeAsync(
            string normalizedSchoolCode,
            CancellationToken cancellationToken = default) =>
            Task.FromResult(false);

        public Task AddAsync(
            School school,
            CancellationToken cancellationToken = default)
        {
            Seed(school);
            return Task.CompletedTask;
        }

        public Task<SchoolRepositoryWriteResult> SaveAsync(
            School school,
            byte[]? expectedRowVersion,
            CancellationToken cancellationToken = default) =>
            Task.FromResult(SchoolRepositoryWriteResult.Success);
    }

    private sealed class FakeExportRepository : IReportExportRepository
    {
        public List<ReportExportJob> Jobs { get; } = [];
        public List<OutboxMessage> Outbox { get; } = [];

        public Task AddAsync(
            ReportExportJob job,
            CancellationToken cancellationToken = default)
        {
            Jobs.Add(job);
            return Task.CompletedTask;
        }

        public Task AddOutboxAsync(
            OutboxMessage message,
            CancellationToken cancellationToken = default)
        {
            Outbox.Add(message);
            return Task.CompletedTask;
        }

        public Task<ReportExportJob?> GetAsync(
            Guid schoolId,
            Guid id,
            CancellationToken cancellationToken = default) =>
            Task.FromResult(
                Jobs.SingleOrDefault(x => x.SchoolId == schoolId && x.Id == id));

        public Task<ReportExportJob?> GetForUpdateAsync(
            Guid schoolId,
            Guid id,
            CancellationToken cancellationToken = default) =>
            GetAsync(schoolId, id, cancellationToken);

        public Task<IReadOnlyList<ReportExportJob>> ListRecentAsync(
            Guid schoolId,
            Guid requestedByUserId,
            int maxCount,
            CancellationToken cancellationToken = default) =>
            Task.FromResult<IReadOnlyList<ReportExportJob>>(
                Jobs
                    .Where(x =>
                        x.SchoolId == schoolId &&
                        x.RequestedByUserId == requestedByUserId)
                    .Take(maxCount)
                    .ToArray());

        public Task<bool> SaveAsync(
            CancellationToken cancellationToken = default) =>
            Task.FromResult(true);
    }

    private sealed class FakeAuditService : IAuditService
    {
        public List<AuditEvent> Events { get; } = [];

        public Task QueueAsync(
            AuditEvent auditEvent,
            CancellationToken cancellationToken = default)
        {
            Events.Add(auditEvent);
            return Task.CompletedTask;
        }

        public Task RecordAsync(
            AuditEvent auditEvent,
            CancellationToken cancellationToken = default)
        {
            Events.Add(auditEvent);
            return Task.CompletedTask;
        }
    }

    private sealed class FakeMetadataProvider : IAuditRequestMetadataProvider
    {
        public AuditRequestMetadata GetCurrent() =>
            new(
                null,
                RoleNames.SchoolAdmin,
                "phase43-test-correlation",
                "127.0.0.1",
                "Phase43Tests",
                "Tests");
    }
}
