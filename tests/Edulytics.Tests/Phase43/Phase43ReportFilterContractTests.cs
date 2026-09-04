using Edulytics.Core.Analytics;
using Edulytics.Core.Constants;
using Edulytics.Core.Entities;
using Edulytics.Core.Enums;
using Edulytics.Core.Interfaces;
using Edulytics.Core.Reports;
using Edulytics.Core.Users;
using Edulytics.Services.Auditing;
using Edulytics.Services.Reports;

namespace Edulytics.Tests.Phase43;

public sealed class Phase43ReportFilterContractTests
{
    [Fact]
    public void Phase43_ReportRequestPolicy_NormalizesFiltersByReportKind()
    {
        var yearId = Guid.NewGuid();
        var classId = Guid.NewGuid();
        var subjectId = Guid.NewGuid();
        var studentId = Guid.NewGuid();
        var outcomeId = Guid.NewGuid();

        var classRequest =
            ReportRequestPolicy.Normalize(
                new ReportRequest(
                    ReportKind.Class,
                    yearId,
                    classId,
                    subjectId,
                    studentId,
                    outcomeId));

        Assert.Equal(yearId, classRequest.AcademicYearId);
        Assert.Equal(classId, classRequest.ClassGroupId);
        Assert.Null(classRequest.SubjectId);
        Assert.Null(classRequest.StudentProfileId);
        Assert.Null(classRequest.LearningOutcomeId);

        var studentRequest =
            ReportRequestPolicy.Normalize(
                new ReportRequest(
                    ReportKind.Student,
                    yearId,
                    classId,
                    subjectId,
                    studentId,
                    outcomeId));

        Assert.Equal(yearId, studentRequest.AcademicYearId);
        Assert.Equal(classId, studentRequest.ClassGroupId);
        Assert.Null(studentRequest.SubjectId);
        Assert.Equal(studentId, studentRequest.StudentProfileId);
        Assert.Null(studentRequest.LearningOutcomeId);

        var outcomeRequest =
            ReportRequestPolicy.Normalize(
                new ReportRequest(
                    ReportKind.LearningOutcome,
                    yearId,
                    classId,
                    subjectId,
                    studentId,
                    outcomeId));

        Assert.Equal(yearId, outcomeRequest.AcademicYearId);
        Assert.Equal(classId, outcomeRequest.ClassGroupId);
        Assert.Null(outcomeRequest.SubjectId);
        Assert.Null(outcomeRequest.StudentProfileId);
        Assert.Equal(outcomeId, outcomeRequest.LearningOutcomeId);
    }

    [Fact]
    public void Phase43_ReportRequestPolicy_RequiresAgreedHierarchy()
    {
        var yearId = Guid.NewGuid();
        var classId = Guid.NewGuid();
        var studentId = Guid.NewGuid();
        var outcomeId = Guid.NewGuid();

        Assert.False(
            ReportRequestPolicy.HasRequiredSelection(
                new ReportRequest(
                    ReportKind.Class,
                    ClassGroupId: classId)));

        Assert.True(
            ReportRequestPolicy.HasRequiredSelection(
                new ReportRequest(
                    ReportKind.Class,
                    yearId,
                    classId)));

        Assert.False(
            ReportRequestPolicy.HasRequiredSelection(
                new ReportRequest(
                    ReportKind.Student,
                    yearId,
                    classId)));

        Assert.True(
            ReportRequestPolicy.HasRequiredSelection(
                new ReportRequest(
                    ReportKind.Student,
                    yearId,
                    classId,
                    StudentProfileId: studentId)));

        Assert.False(
            ReportRequestPolicy.HasRequiredSelection(
                new ReportRequest(
                    ReportKind.LearningOutcome,
                    yearId,
                    classId)));

        Assert.True(
            ReportRequestPolicy.HasRequiredSelection(
                new ReportRequest(
                    ReportKind.LearningOutcome,
                    yearId,
                    classId,
                    LearningOutcomeId: outcomeId)));
    }

    [Fact]
    public async Task Phase43_ClassReport_RejectsClassFromDifferentAcademicYear()
    {
        var fixture = Fixture.Create();

        var result =
            await fixture.Reports.ValidateAsync(
                fixture.Admin.Id,
                new ReportRequest(
                    ReportKind.Class,
                    fixture.YearA.Id,
                    fixture.ClassB.Id));

        Assert.Null(result.Value);
        Assert.Equal(
            ReportErrorCode.InvalidFilter,
            result.Error);
    }

    [Fact]
    public async Task Phase43_StudentReport_RejectsStudentOutsideSelectedClass()
    {
        var fixture = Fixture.Create();

        var result =
            await fixture.Reports.ValidateAsync(
                fixture.Admin.Id,
                new ReportRequest(
                    ReportKind.Student,
                    fixture.YearA.Id,
                    fixture.ClassA.Id,
                    StudentProfileId:
                        fixture.StudentB.Id));

        Assert.Null(result.Value);
        Assert.Equal(
            ReportErrorCode.InvalidFilter,
            result.Error);
    }

    [Fact]
    public async Task Phase43_OutcomeReport_RejectsOutcomeOutsideSelectedClass()
    {
        var fixture = Fixture.Create();

        var result =
            await fixture.Reports.ValidateAsync(
                fixture.Admin.Id,
                new ReportRequest(
                    ReportKind.LearningOutcome,
                    fixture.YearA.Id,
                    fixture.ClassA.Id,
                    LearningOutcomeId:
                        fixture.OutcomeB.Id));

        Assert.Null(result.Value);
        Assert.Equal(
            ReportErrorCode.InvalidFilter,
            result.Error);
    }

    [Fact]
    public async Task Phase43_Export_StoresOnlyNormalizedFilters()
    {
        var fixture = Fixture.Create();

        var result =
            await fixture.Exports.RequestAsync(
                fixture.Admin.Id,
                new ReportRequest(
                    ReportKind.Class,
                    fixture.YearA.Id,
                    fixture.ClassA.Id,
                    fixture.Subject.Id,
                    fixture.StudentA.Id,
                    fixture.OutcomeA.Id),
                ReportExportFormat.Csv,
                "en");

        Assert.True(result.Succeeded);

        var job =
            Assert.Single(
                fixture.ExportRepository.Jobs);

        Assert.Equal(
            fixture.YearA.Id,
            job.AcademicYearId);

        Assert.Equal(
            fixture.ClassA.Id,
            job.ClassGroupId);

        Assert.Null(job.SubjectId);
        Assert.Null(job.StudentProfileId);
        Assert.Null(job.LearningOutcomeId);
    }

    [Fact]
    public void Phase43_ReportView_UsesDynamicFilterContract()
    {
        var root = FindRoot();

        var view =
            File.ReadAllText(
                Path.Combine(
                    root,
                    "src",
                    "Edulytics.Web",
                    "Views",
                    "Reports",
                    "Index.cshtml"));

        Assert.Contains(
            "ShowAcademicYear(Model.Request.Kind)",
            view);

        Assert.Contains(
            "ShowClass(Model.Request.Kind)",
            view);

        Assert.Contains(
            "ShowStudent(Model.Request.Kind)",
            view);

        Assert.Contains(
            "ShowLearningOutcome(Model.Request.Kind)",
            view);

        Assert.Contains(
            "data-report-kind-filter",
            view);

        var siteJs =
            File.ReadAllText(
                Path.Combine(
                    root,
                    "src",
                    "Edulytics.Web",
                    "wwwroot",
                    "js",
                    "site.js"));

        Assert.Contains(
            "wireReportKindFilters",
            siteJs);

        Assert.Contains(
            "form.requestSubmit()",
            siteJs);
    }

    private static string FindRoot()
    {
        var directory =
            new DirectoryInfo(
                AppContext.BaseDirectory);

        while (
            directory is not null &&
            !File.Exists(
                Path.Combine(
                    directory.FullName,
                    "Edulytics.sln")))
        {
            directory = directory.Parent;
        }

        return directory?.FullName
            ?? throw new InvalidOperationException(
                "Repository root not found.");
    }

    private sealed class Fixture
    {
        public required SchoolUserRecord Admin { get; init; }
        public required AcademicYear YearA { get; init; }
        public required AcademicYear YearB { get; init; }
        public required ClassGroup ClassA { get; init; }
        public required ClassGroup ClassB { get; init; }
        public required Subject Subject { get; init; }
        public required StudentProfile StudentA { get; init; }
        public required StudentProfile StudentB { get; init; }
        public required LearningOutcome OutcomeA { get; init; }
        public required LearningOutcome OutcomeB { get; init; }
        public required IReportQueryService Reports { get; init; }
        public required ReportExportService Exports { get; init; }
        public required FakeExportRepository ExportRepository { get; init; }

        public static Fixture Create()
        {
            var now = DateTime.UtcNow;

            var school =
                new School
                {
                    Id = Guid.NewGuid(),
                    Name = "Phase43 Filter School",
                    SchoolCode = "P43F",
                    NormalizedSchoolCode = "P43F",
                    Status = SchoolStatus.Active,
                    CountryCode = "PL",
                    City = "Warsaw",
                    ContactEmail = "phase43-filter@example.com",
                    DefaultCulture = "en",
                    TimeZoneId = "Europe/Warsaw",
                    CreatedAtUtc = now,
                    UpdatedAtUtc = now
                };

            var admin =
                NewUser(
                    school.Id,
                    RoleNames.SchoolAdmin);

            var yearA =
                NewYear(
                    school.Id,
                    "2026/27",
                    new DateOnly(2026, 9, 1),
                    new DateOnly(2027, 6, 30));

            var yearB =
                NewYear(
                    school.Id,
                    "2027/28",
                    new DateOnly(2027, 9, 1),
                    new DateOnly(2028, 6, 30));

            var classA =
                NewClass(
                    school.Id,
                    yearA.Id,
                    "Class A",
                    "A");

            var classB =
                NewClass(
                    school.Id,
                    yearB.Id,
                    "Class B",
                    "B");

            var subject =
                new Subject
                {
                    Id = Guid.NewGuid(),
                    SchoolId = school.Id,
                    Name = "Mathematics",
                    Code = "MATH",
                    NormalizedCode = "MATH",
                    Status = AcademicStructureStatus.Active
                };

            var studentA =
                NewStudent(
                    school.Id,
                    "S-A",
                    "Student A",
                    now);

            var studentB =
                NewStudent(
                    school.Id,
                    "S-B",
                    "Student B",
                    now);

            var outcomeA =
                NewOutcome(
                    school.Id,
                    subject.Id,
                    "OUT-A");

            var outcomeB =
                NewOutcome(
                    school.Id,
                    subject.Id,
                    "OUT-B");

            var masteryA =
                NewMastery(
                    school.Id,
                    yearA.Id,
                    classA.Id,
                    subject.Id,
                    studentA.Id,
                    outcomeA.Id,
                    now);

            var masteryB =
                NewMastery(
                    school.Id,
                    yearB.Id,
                    classB.Id,
                    subject.Id,
                    studentB.Id,
                    outcomeB.Id,
                    now);

            var summaryA =
                NewSummary(
                    school.Id,
                    yearA.Id,
                    classA.Id,
                    subject.Id,
                    outcomeA.Id,
                    now);

            var summaryB =
                NewSummary(
                    school.Id,
                    yearB.Id,
                    classB.Id,
                    subject.Id,
                    outcomeB.Id,
                    now);

            var projection =
                new AnalyticsProjectionSnapshot(
                    [yearA, yearB],
                    [classA, classB],
                    [subject],
                    [studentA, studentB],
                    [],
                    [],
                    [outcomeA, outcomeB],
                    [masteryA, masteryB],
                    [summaryA, summaryB],
                    [],
                    [],
                    []);

            var analytics =
                new FakeAnalyticsRepository(
                    projection);

            var schools =
                new FakeSchoolRepository();

            schools.Seed(school);

            var users =
                new FakeUserRepository();

            users.Seed(admin);

            var inner =
                new ReportQueryService(
                    analytics,
                    schools,
                    users,
                    new FakeAssignmentRepository());

            var reports =
                new Phase43ReportQueryService(
                    inner,
                    analytics);

            var exportRepository =
                new FakeExportRepository();

            var exports =
                new ReportExportService(
                    reports,
                    exportRepository,
                    new FakeAuditService(),
                    new FakeMetadataProvider(),
                    new ReportOptions());

            return new Fixture
            {
                Admin = admin,
                YearA = yearA,
                YearB = yearB,
                ClassA = classA,
                ClassB = classB,
                Subject = subject,
                StudentA = studentA,
                StudentB = studentB,
                OutcomeA = outcomeA,
                OutcomeB = outcomeB,
                Reports = reports,
                Exports = exports,
                ExportRepository = exportRepository
            };
        }
    }

    private static AcademicYear NewYear(
        Guid schoolId,
        string name,
        DateOnly startsOn,
        DateOnly endsOn) =>
        new()
        {
            Id = Guid.NewGuid(),
            SchoolId = schoolId,
            Name = name,
            StartsOn = startsOn,
            EndsOn = endsOn,
            Status = AcademicStructureStatus.Active
        };

    private static ClassGroup NewClass(
        Guid schoolId,
        Guid academicYearId,
        string name,
        string code) =>
        new()
        {
            Id = Guid.NewGuid(),
            SchoolId = schoolId,
            AcademicYearId = academicYearId,
            GradeLevelId = Guid.NewGuid(),
            Name = name,
            Code = code,
            NormalizedCode = code,
            Status = AcademicStructureStatus.Active
        };

    private static StudentProfile NewStudent(
        Guid schoolId,
        string number,
        string displayName,
        DateTime now) =>
        new()
        {
            Id = Guid.NewGuid(),
            SchoolId = schoolId,
            StudentNumber = number,
            NormalizedStudentNumber = number,
            FirstName = displayName,
            LastName = string.Empty,
            DisplayName = displayName,
            Status = AcademicStructureStatus.Active,
            CreatedAtUtc = now,
            UpdatedAtUtc = now
        };

    private static LearningOutcome NewOutcome(
        Guid schoolId,
        Guid subjectId,
        string code) =>
        new()
        {
            Id = Guid.NewGuid(),
            SchoolId = schoolId,
            AcademicProgramId = Guid.NewGuid(),
            FrameworkVersionId = Guid.NewGuid(),
            SubjectId = subjectId,
            GradeLevelId = Guid.NewGuid(),
            TopicId = Guid.NewGuid(),
            Code = code,
            Description = code,
            Order = 1
        };

    private static StudentOutcomeMastery NewMastery(
        Guid schoolId,
        Guid yearId,
        Guid classId,
        Guid subjectId,
        Guid studentId,
        Guid outcomeId,
        DateTime now) =>
        new()
        {
            Id = Guid.NewGuid(),
            SchoolId = schoolId,
            AcademicYearId = yearId,
            ClassGroupId = classId,
            SubjectId = subjectId,
            StudentProfileId = studentId,
            LearningOutcomeId = outcomeId,
            EarnedScore = 8m,
            PossibleScore = 10m,
            MasteryPercentage = 80m,
            EvidenceCount = 1,
            CalculatedAtUtc = now
        };

    private static ClassOutcomeSummary NewSummary(
        Guid schoolId,
        Guid yearId,
        Guid classId,
        Guid subjectId,
        Guid outcomeId,
        DateTime now) =>
        new()
        {
            Id = Guid.NewGuid(),
            SchoolId = schoolId,
            AcademicYearId = yearId,
            ClassGroupId = classId,
            SubjectId = subjectId,
            LearningOutcomeId = outcomeId,
            EarnedScore = 8m,
            PossibleScore = 10m,
            AverageMasteryPercentage = 80m,
            StudentCount = 1,
            AtRiskStudentCount = 0,
            EvidenceCount = 1,
            CalculatedAtUtc = now
        };

    private static SchoolUserRecord NewUser(
        Guid schoolId,
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

    private sealed class FakeAnalyticsRepository
        : IAnalyticsRepository
    {
        private readonly AnalyticsProjectionSnapshot
            _projection;

        public FakeAnalyticsRepository(
            AnalyticsProjectionSnapshot projection)
        {
            _projection = projection;
        }

        public Task<AnalyticsSourceSnapshot>
            GetSourceSnapshotAsync(
                Guid schoolId,
                CancellationToken cancellationToken = default) =>
            Task.FromResult(
                new AnalyticsSourceSnapshot(
                    [], [], [], [], [], [], [],
                    [], [], [], [], [], []));

        public Task<AnalyticsProjectionSnapshot>
            GetProjectionSnapshotAsync(
                Guid schoolId,
                CancellationToken cancellationToken = default) =>
            Task.FromResult(_projection);

        public Task<DateTime?> GetLatestSourceUpdateAsync(
            Guid schoolId,
            CancellationToken cancellationToken = default) =>
            Task.FromResult<DateTime?>(null);

        public Task<AnalyticsPersistenceResult>
            ReplaceProjectionsAsync(
                Guid schoolId,
                AnalyticsProjectionSet projections,
                CancellationToken cancellationToken = default) =>
            Task.FromResult(
                AnalyticsPersistenceResult.Success());
    }

    private sealed class FakeAssignmentRepository
        : ISubjectSupervisorAssignmentRepository
    {
        public Task<IReadOnlyList<SubjectSupervisorAssignment>>
            ListBySchoolAsync(
                Guid schoolId,
                CancellationToken cancellationToken = default) =>
            Task.FromResult<IReadOnlyList<SubjectSupervisorAssignment>>([]);

        public Task<IReadOnlyList<SubjectSupervisorAssignment>>
            ListActiveBySupervisorAsync(
                Guid schoolId,
                Guid supervisorUserId,
                CancellationToken cancellationToken = default) =>
            Task.FromResult<IReadOnlyList<SubjectSupervisorAssignment>>([]);

        public Task<IReadOnlyList<Subject>>
            ListSubjectsAsync(
                Guid schoolId,
                CancellationToken cancellationToken = default) =>
            Task.FromResult<IReadOnlyList<Subject>>([]);

        public Task<Subject?> GetSubjectAsync(
            Guid schoolId,
            Guid subjectId,
            CancellationToken cancellationToken = default) =>
            Task.FromResult<Subject?>(null);

        public Task<SubjectSupervisorAssignment?>
            GetBySchoolAndIdAsync(
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

        public void Remove(
            SubjectSupervisorAssignment assignment)
        {
        }

        public Task<bool> SaveAsync(
            CancellationToken cancellationToken = default) =>
            Task.FromResult(true);
    }

    private sealed class FakeUserRepository
        : ISchoolUserRepository
    {
        private readonly Dictionary<Guid, SchoolUserRecord>
            _users = [];

        public void Seed(
            SchoolUserRecord user) =>
            _users[user.Id] = user;

        public Task<SchoolUserRecord?> GetActorAsync(
            Guid userId,
            CancellationToken cancellationToken = default) =>
            Task.FromResult(
                _users.GetValueOrDefault(userId));

        public Task<IReadOnlyList<SchoolUserRecord>>
            ListBySchoolAsync(
                Guid schoolId,
                CancellationToken cancellationToken = default) =>
            Task.FromResult<IReadOnlyList<SchoolUserRecord>>(
                _users.Values
                    .Where(x => x.SchoolId == schoolId)
                    .ToArray());

        public Task<SchoolUserRecord?>
            GetBySchoolAndIdAsync(
                Guid schoolId,
                Guid userId,
                CancellationToken cancellationToken = default)
        {
            var user =
                _users.GetValueOrDefault(userId);

            return Task.FromResult(
                user?.SchoolId == schoolId
                    ? user
                    : null);
        }

        public Task<SchoolUserPersistenceResult>
            CreateAsync(
                Guid schoolId,
                string email,
                string role,
                CancellationToken cancellationToken = default) =>
            Unsupported();

        public Task<SchoolUserPersistenceResult>
            SetActiveAsync(
                Guid schoolId,
                Guid userId,
                bool isActive,
                CancellationToken cancellationToken = default) =>
            Unsupported();

        public Task<SchoolUserPersistenceResult>
            SetLockedAsync(
                Guid schoolId,
                Guid userId,
                bool isLocked,
                CancellationToken cancellationToken = default) =>
            Unsupported();

        public Task<SchoolUserPersistenceResult>
            SetRoleAsync(
                Guid schoolId,
                Guid userId,
                string role,
                CancellationToken cancellationToken = default) =>
            Unsupported();

        public Task<SchoolUserPersistenceResult>
            GeneratePasswordSetupAsync(
                Guid schoolId,
                Guid userId,
                CancellationToken cancellationToken = default) =>
            Unsupported();

        public Task<SchoolUserPersistenceResult>
            CompletePasswordSetupAsync(
                Guid userId,
                string token,
                string newPassword,
                CancellationToken cancellationToken = default) =>
            Unsupported();

        private static Task<SchoolUserPersistenceResult>
            Unsupported() =>
            Task.FromResult(
                SchoolUserPersistenceResult.Failure(
                    SchoolUserPersistenceError.NotFound));
    }

    private sealed class FakeSchoolRepository
        : ISchoolRepository
    {
        private readonly Dictionary<Guid, School>
            _schools = [];

        public void Seed(
            School school) =>
            _schools[school.Id] = school;

        public Task<IReadOnlyList<School>> ListAsync(
            CancellationToken cancellationToken = default) =>
            Task.FromResult<IReadOnlyList<School>>(
                _schools.Values.ToArray());

        public Task<School?> GetByIdAsync(
            Guid id,
            CancellationToken cancellationToken = default) =>
            Task.FromResult(
                _schools.GetValueOrDefault(id));

        public Task<School?> GetForUpdateAsync(
            Guid id,
            CancellationToken cancellationToken = default) =>
            GetByIdAsync(
                id,
                cancellationToken);

        public Task<bool>
            ExistsByNormalizedCodeAsync(
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

        public Task<SchoolRepositoryWriteResult>
            SaveAsync(
                School school,
                byte[]? expectedRowVersion,
                CancellationToken cancellationToken = default) =>
            Task.FromResult(
                SchoolRepositoryWriteResult.Success);
    }

    private sealed class FakeExportRepository
        : IReportExportRepository
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
                Jobs.SingleOrDefault(
                    x =>
                        x.SchoolId == schoolId &&
                        x.Id == id));

        public Task<ReportExportJob?>
            GetForUpdateAsync(
                Guid schoolId,
                Guid id,
                CancellationToken cancellationToken = default) =>
            GetAsync(
                schoolId,
                id,
                cancellationToken);

        public Task<IReadOnlyList<ReportExportJob>>
            ListRecentAsync(
                Guid schoolId,
                Guid requestedByUserId,
                int maxCount,
                CancellationToken cancellationToken = default) =>
            Task.FromResult<IReadOnlyList<ReportExportJob>>(
                Jobs
                    .Where(
                        x =>
                            x.SchoolId == schoolId &&
                            x.RequestedByUserId ==
                                requestedByUserId)
                    .Take(maxCount)
                    .ToArray());

        public Task<bool> SaveAsync(
            CancellationToken cancellationToken = default) =>
            Task.FromResult(true);
    }

    private sealed class FakeAuditService
        : IAuditService
    {
        public Task QueueAsync(
            AuditEvent auditEvent,
            CancellationToken cancellationToken = default) =>
            Task.CompletedTask;

        public Task RecordAsync(
            AuditEvent auditEvent,
            CancellationToken cancellationToken = default) =>
            Task.CompletedTask;
    }

    private sealed class FakeMetadataProvider
        : IAuditRequestMetadataProvider
    {
        public AuditRequestMetadata GetCurrent() =>
            new(
                null,
                RoleNames.SchoolAdmin,
                "phase43-filter-test",
                "127.0.0.1",
                "Phase43FilterTests",
                "Tests");
    }
}
