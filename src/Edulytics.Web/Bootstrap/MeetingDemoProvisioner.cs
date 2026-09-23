using Edulytics.Core.AssessmentIntelligence;
using Edulytics.Core.Constants;
using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;
using Edulytics.Core.Enums;
using Edulytics.Core.MathematicsGeneration;
using Edulytics.Data.Contexts;
using Edulytics.Data.Identity;
using Edulytics.Data.Repositories;
using Edulytics.Services.Analytics;
using Edulytics.Services.MathematicsGeneration;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Edulytics.Web.Bootstrap;

/// <summary>
/// One-shot meeting dataset provisioner for the production demo service.
/// It resets school-scoped operational data while preserving the platform
/// curriculum catalogue, EF migrations, Identity roles and data-protection keys.
/// A platform-scoped idempotency marker prevents an accidental second reset.
/// </summary>
internal static class MeetingDemoProvisioner
{
    private const string TargetRenderServiceId = "srv-dakq5n2fngtc73a62i10";
    private const string SeedVersion = "meeting-demo-2026-09-23-v1";
    private const string MarkerOperation = "MeetingDemoSeed";

    private static readonly int[] DeepLogicalLevels = [2, 4, 5, 10, 11, 12];

    private static readonly string[] FirstNames =
    [
        "Adam", "Aisha", "Alex", "Amelia", "Daniel",
        "Emma", "Ethan", "Fatima", "Hana", "Jacob",
        "Julia", "Karim", "Lena", "Leo", "Maya",
        "Noah", "Omar", "Sara", "Sofia", "Tomasz",
        "Victor", "Yara", "Zain", "Nadia", "Lucas"
    ];

    private static readonly string[] LastNames =
    [
        "Anderson", "Bennett", "Carter", "Davis", "Evans",
        "Garcia", "Hassan", "Ibrahim", "Johnson", "Kowalski",
        "Lewis", "Martin", "Nowak", "Patel", "Roberts",
        "Silva", "Smith", "Taylor", "Walker", "Williams",
        "Zielinski", "Morgan", "Khan", "Brown", "Clark"
    ];

    private sealed record SchoolDefinition(
        string Key,
        string Name,
        string SchoolCode,
        string CountryCode,
        string City,
        string DefaultCulture,
        string TimeZoneId,
        string PackCode,
        string ProgramName,
        string ProgramCode,
        int PrimaryLoginLogicalLevel,
        int SecondaryLoginLogicalLevel,
        string? SecondaryLoginPathway);

    private sealed record DemoAccount(
        string SchoolKey,
        string Role,
        string Email,
        string? ClassName);

    private sealed record SeededClass(
        ClassGroup ClassGroup,
        GradeLevel Grade,
        SchoolCurriculumAdoption Adoption,
        CurriculumLevelIdentity Level);

    private sealed record SeededSchool(
        Guid SchoolId,
        IReadOnlyList<DemoAccount> Accounts);

    private static readonly SchoolDefinition[] Schools =
    [
        new(
            "cambridge",
            "Horizon Cambridge Demo School",
            "DEMO-CAMBRIDGE",
            "GB",
            "London",
            "en",
            "Europe/London",
            MathematicsCurriculumPackRegistry.CambridgeCode,
            "British Programme",
            "BRITISH",
            4,
            12,
            null),
        new(
            "uae",
            "Emirates Future Demo Academy",
            "DEMO-UAE",
            "AE",
            "Dubai",
            "en",
            "Asia/Dubai",
            MathematicsCurriculumPackRegistry.UaeCode,
            "UAE Programme",
            "UAE",
            4,
            11,
            "Advanced"),
        new(
            "commoncore",
            "Liberty Common Core Demo School",
            "DEMO-US",
            "US",
            "Boston",
            "en",
            "America/New_York",
            MathematicsCurriculumPackRegistry.CommonCoreCode,
            "American Programme",
            "AMERICAN",
            5,
            12,
            null),
        new(
            "polish",
            "Akademia Vistula Demo School",
            "DEMO-PL",
            "PL",
            "Warsaw",
            "pl",
            "Europe/Warsaw",
            MathematicsCurriculumPackRegistry.PolandCode,
            "Polish Programme",
            "POLISH",
            4,
            11,
            "Liceum ogólnokształcące")
    ];

    public static async Task RunAsync(
        EdulyticsDbContext db,
        UserManager<ApplicationUser> userManager,
        IConfiguration configuration,
        CancellationToken cancellationToken = default)
    {
        if (!string.Equals(
                Environment.GetEnvironmentVariable("RENDER_SERVICE_ID"),
                TargetRenderServiceId,
                StringComparison.Ordinal))
        {
            return;
        }

        if (!configuration.GetValue<bool>("Edulytics:MeetingDemo:ResetAndSeed"))
            return;

        var password = configuration["Edulytics:MeetingDemo:Password"];
        if (string.IsNullOrWhiteSpace(password))
        {
            throw new InvalidOperationException(
                "Meeting demo reset was enabled without Edulytics:MeetingDemo:Password.");
        }

        var alreadyCompleted = await db.IdempotencyRecords
            .AsNoTracking()
            .AnyAsync(
                x =>
                    x.SchoolId == null &&
                    x.ActorUserId == Guid.Empty &&
                    x.Operation == MarkerOperation &&
                    x.IdempotencyKey == SeedVersion &&
                    x.Status == IdempotencyStatus.Completed,
                cancellationToken);

        if (alreadyCompleted)
        {
            Console.WriteLine(
                $"MEETING_DEMO_SEED_SKIPPED version={SeedVersion} reason=already-completed");
            return;
        }

        Console.WriteLine($"MEETING_DEMO_SEED_BEGIN version={SeedVersion}");

        var seededSchools = new List<SeededSchool>();

        await using (var transaction =
                     await db.Database.BeginTransactionAsync(cancellationToken))
        {
            try
            {
                await ResetSchoolScopedDataAsync(db, cancellationToken);
                db.ChangeTracker.Clear();

                foreach (var definition in Schools)
                {
                    seededSchools.Add(
                        await SeedSchoolAsync(
                            db,
                            userManager,
                            definition,
                            password,
                            cancellationToken));
                }

                await transaction.CommitAsync(cancellationToken);
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                Console.WriteLine("MEETING_DEMO_SEED_ROLLED_BACK stage=base-data");
                throw;
            }
        }

        db.ChangeTracker.Clear();

        var analytics = new AnalyticsProjectionRefreshService(
            new AnalyticsRepository(db),
            new AnalyticsProjectionBuilder());

        foreach (var school in seededSchools)
        {
            var refresh = await analytics.RefreshSchoolAsync(
                school.SchoolId,
                cancellationToken);

            if (!refresh.Succeeded)
            {
                throw new InvalidOperationException(
                    $"Meeting demo analytics refresh failed for school {school.SchoolId:D}: {refresh.Error}.");
            }
        }

        db.IdempotencyRecords.Add(
            new IdempotencyRecord
            {
                Id = Guid.NewGuid(),
                SchoolId = null,
                ActorUserId = Guid.Empty,
                Operation = MarkerOperation,
                IdempotencyKey = SeedVersion,
                RequestHash = new string('0', 64),
                Status = IdempotencyStatus.Completed,
                ResultStatusCode = 200,
                CreatedAtUtc = DateTime.UtcNow,
                CompletedAtUtc = DateTime.UtcNow,
                ExpiresAtUtc = DateTime.UtcNow.AddYears(10),
                RowVersion = []
            });

        await db.SaveChangesAsync(cancellationToken);

        var schoolCount = await db.Schools.CountAsync(cancellationToken);
        var classCount = await db.ClassGroups.CountAsync(cancellationToken);
        var studentCount = await db.StudentProfiles.CountAsync(cancellationToken);
        var enrollmentCount = await db.StudentEnrollments.CountAsync(cancellationToken);
        var assessmentCount = await db.Assessments.CountAsync(cancellationToken);
        var resultCount = await db.AssessmentResults.CountAsync(cancellationToken);
        var practiceCount = await db.PracticeAttempts.CountAsync(cancellationToken);
        var masteryCount = await db.StudentOutcomeMasteries.CountAsync(cancellationToken);
        var snapshotCount = await db.SchoolAnalyticsSnapshots.CountAsync(cancellationToken);

        foreach (var account in seededSchools.SelectMany(x => x.Accounts))
        {
            Console.WriteLine(
                $"MEETING_DEMO_ACCOUNT school={account.SchoolKey} role={account.Role} email={account.Email} class={account.ClassName ?? "-"}");
        }

        Console.WriteLine(
            "MEETING_DEMO_SEED_COMPLETED " +
            $"version={SeedVersion} schools={schoolCount} classes={classCount} " +
            $"students={studentCount} enrollments={enrollmentCount} " +
            $"assessments={assessmentCount} results={resultCount} " +
            $"practiceAttempts={practiceCount} masteries={masteryCount} " +
            $"schoolSnapshots={snapshotCount}");
    }

    private static async Task ResetSchoolScopedDataAsync(
        EdulyticsDbContext db,
        CancellationToken cancellationToken)
    {
        const string sql = """
DELETE FROM "OutboxRequeueAudits";
DELETE FROM "NotificationDeliveryJobs";
DELETE FROM "UserNotifications";
DELETE FROM "ReportExportJobs";
DELETE FROM "ImportValidationErrors";
DELETE FROM "ImportBatches";
DELETE FROM "BillingRefunds";
DELETE FROM "BankTransferPayments";
DELETE FROM "BillingInvoiceLines";
DELETE FROM "BillingInvoices";
DELETE FROM "SchoolBillingProfiles";
DELETE FROM "SubscriptionSeatChanges";
DELETE FROM "SchoolSubscriptions";
DELETE FROM "DemoAccesses";
DELETE FROM "DemoRequests";
DELETE FROM "AuditLogs" WHERE "SchoolId" IS NOT NULL;
DELETE FROM "IdempotencyRecords" WHERE "SchoolId" IS NOT NULL;
DELETE FROM "OutboxMessages" WHERE "SchoolId" IS NOT NULL;
DELETE FROM "AnalyticsRefreshStates";

DELETE FROM "PracticeResponses";
DELETE FROM "LearningEvidence";
DELETE FROM "StudentItemExposures";
DELETE FROM "PracticeAttemptItems";
DELETE FROM "PracticeAttempts";
DELETE FROM "AssessmentItemOutcomes";
DELETE FROM "AssessmentItems";

DELETE FROM "StudentOutcomeMasteries";
DELETE FROM "ClassOutcomeSummaries";
DELETE FROM "ClassTopicSummaries";
DELETE FROM "ClassAssessmentTrends";
DELETE FROM "SchoolAnalyticsSnapshots";

DELETE FROM "StudentAnswers";
DELETE FROM "AssessmentResults";
DELETE FROM "QuestionLearningOutcomes";
DELETE FROM "AssessmentQuestions";
DELETE FROM "Assessments";

DELETE FROM "LearningLessonTranslations";
DELETE FROM "LearningLessonOutcomes";
DELETE FROM "LearningLessons";

DELETE FROM "TeacherAssignments";
DELETE FROM "SubjectSupervisorAssignments";
DELETE FROM "StudentEnrollments";
DELETE FROM "LearningOutcomes";
DELETE FROM "CurriculumTopics";
DELETE FROM "ClassGroups";
DELETE FROM "SchoolCurriculumAdoptions";
DELETE FROM "AcademicYearProgramOfferings";
DELETE FROM "Terms";
DELETE FROM "StudentProfiles";
DELETE FROM "GradeLevels";
DELETE FROM "Subjects";
DELETE FROM "AcademicPrograms";
DELETE FROM "AcademicYears";

DELETE FROM "AspNetUserTokens"
WHERE "UserId" IN (SELECT "Id" FROM "AspNetUsers" WHERE "SchoolId" IS NOT NULL);
DELETE FROM "AspNetUserLogins"
WHERE "UserId" IN (SELECT "Id" FROM "AspNetUsers" WHERE "SchoolId" IS NOT NULL);
DELETE FROM "AspNetUserClaims"
WHERE "UserId" IN (SELECT "Id" FROM "AspNetUsers" WHERE "SchoolId" IS NOT NULL);
DELETE FROM "AspNetUserRoles"
WHERE "UserId" IN (SELECT "Id" FROM "AspNetUsers" WHERE "SchoolId" IS NOT NULL);
DELETE FROM "AspNetUsers" WHERE "SchoolId" IS NOT NULL;

DELETE FROM "CurriculumLessonContentTranslations"
WHERE "CurriculumLessonContentId" IN (
    SELECT c."Id"
    FROM "CurriculumLessonContents" c
    JOIN "CurriculumFrameworkVersions" v ON v."Id" = c."FrameworkVersionId"
    JOIN "CurriculumFrameworks" f ON f."Id" = v."FrameworkId"
    WHERE f."OwnerSchoolId" IS NOT NULL
);
DELETE FROM "CurriculumLessonContents"
WHERE "FrameworkVersionId" IN (
    SELECT v."Id"
    FROM "CurriculumFrameworkVersions" v
    JOIN "CurriculumFrameworks" f ON f."Id" = v."FrameworkId"
    WHERE f."OwnerSchoolId" IS NOT NULL
);
DELETE FROM "CurriculumPedagogicalLessonOutcomes"
WHERE "FrameworkVersionId" IN (
    SELECT v."Id"
    FROM "CurriculumFrameworkVersions" v
    JOIN "CurriculumFrameworks" f ON f."Id" = v."FrameworkId"
    WHERE f."OwnerSchoolId" IS NOT NULL
);
DELETE FROM "CurriculumPedagogicalLessons"
WHERE "FrameworkVersionId" IN (
    SELECT v."Id"
    FROM "CurriculumFrameworkVersions" v
    JOIN "CurriculumFrameworks" f ON f."Id" = v."FrameworkId"
    WHERE f."OwnerSchoolId" IS NOT NULL
);
DELETE FROM "CurriculumPackNodeLinks"
WHERE "FrameworkVersionId" IN (
    SELECT v."Id"
    FROM "CurriculumFrameworkVersions" v
    JOIN "CurriculumFrameworks" f ON f."Id" = v."FrameworkId"
    WHERE f."OwnerSchoolId" IS NOT NULL
);
DELETE FROM "CurriculumPackContentNodes"
WHERE "FrameworkVersionId" IN (
    SELECT v."Id"
    FROM "CurriculumFrameworkVersions" v
    JOIN "CurriculumFrameworks" f ON f."Id" = v."FrameworkId"
    WHERE f."OwnerSchoolId" IS NOT NULL
);
DELETE FROM "CurriculumPackImportStates"
WHERE "FrameworkVersionId" IN (
    SELECT v."Id"
    FROM "CurriculumFrameworkVersions" v
    JOIN "CurriculumFrameworks" f ON f."Id" = v."FrameworkId"
    WHERE f."OwnerSchoolId" IS NOT NULL
);
DELETE FROM "CurriculumFrameworkVersions"
WHERE "FrameworkId" IN (
    SELECT "Id" FROM "CurriculumFrameworks" WHERE "OwnerSchoolId" IS NOT NULL
);
DELETE FROM "CurriculumFrameworks" WHERE "OwnerSchoolId" IS NOT NULL;

DELETE FROM "Schools";
""";

        await db.Database.ExecuteSqlRawAsync(sql, cancellationToken);

        Console.WriteLine(
            "MEETING_DEMO_RESET_COMPLETED scope=school-operational-data globalCurriculum=preserved roles=preserved");
    }

    private static async Task<SeededSchool> SeedSchoolAsync(
        EdulyticsDbContext db,
        UserManager<ApplicationUser> userManager,
        SchoolDefinition definition,
        string password,
        CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;
        var school = new School
        {
            Id = Guid.NewGuid(),
            Name = definition.Name,
            SchoolCode = definition.SchoolCode,
            NormalizedSchoolCode = definition.SchoolCode.ToUpperInvariant(),
            Status = SchoolStatus.Active,
            CountryCode = definition.CountryCode,
            City = definition.City,
            ContactEmail = $"{definition.Key}.school@edulytiks.com",
            DefaultCulture = definition.DefaultCulture,
            TimeZoneId = definition.TimeZoneId,
            CreatedAtUtc = now.AddMonths(-8),
            UpdatedAtUtc = now,
            RowVersion = []
        };

        db.Schools.Add(school);

        var academicYear = new AcademicYear
        {
            Id = Guid.NewGuid(),
            SchoolId = school.Id,
            Name = "2026/2027",
            StartsOn = new DateOnly(2026, 9, 1),
            EndsOn = new DateOnly(2027, 6, 30),
            Status = AcademicStructureStatus.Active,
            CreatedAtUtc = now.AddMonths(-3),
            UpdatedAtUtc = now,
            RowVersion = []
        };

        var program = new AcademicProgram
        {
            Id = Guid.NewGuid(),
            SchoolId = school.Id,
            Name = definition.ProgramName,
            Code = definition.ProgramCode,
            NormalizedCode = definition.ProgramCode.ToUpperInvariant(),
            Status = AcademicStructureStatus.Active,
            IsDefault = true,
            CreatedAtUtc = now.AddMonths(-3),
            UpdatedAtUtc = now,
            RowVersion = []
        };

        var subject = new Subject
        {
            Id = Guid.NewGuid(),
            SchoolId = school.Id,
            Name = "Mathematics",
            Code = "MATH",
            NormalizedCode = "MATH",
            Status = AcademicStructureStatus.Active,
            RowVersion = []
        };

        db.AcademicYears.Add(academicYear);
        db.AcademicPrograms.Add(program);
        db.Subjects.Add(subject);

        db.Terms.AddRange(
            new Term
            {
                Id = Guid.NewGuid(),
                SchoolId = school.Id,
                AcademicYearId = academicYear.Id,
                Name = "Term 1",
                StartsOn = new DateOnly(2026, 9, 1),
                EndsOn = new DateOnly(2026, 12, 18),
                Status = AcademicStructureStatus.Active
            },
            new Term
            {
                Id = Guid.NewGuid(),
                SchoolId = school.Id,
                AcademicYearId = academicYear.Id,
                Name = "Term 2",
                StartsOn = new DateOnly(2027, 1, 4),
                EndsOn = new DateOnly(2027, 3, 26),
                Status = AcademicStructureStatus.Active
            },
            new Term
            {
                Id = Guid.NewGuid(),
                SchoolId = school.Id,
                AcademicYearId = academicYear.Id,
                Name = "Term 3",
                StartsOn = new DateOnly(2027, 4, 5),
                EndsOn = new DateOnly(2027, 6, 30),
                Status = AcademicStructureStatus.Active
            });

        db.AcademicYearProgramOfferings.Add(
            new AcademicYearProgramOffering
            {
                Id = Guid.NewGuid(),
                SchoolId = school.Id,
                AcademicYearId = academicYear.Id,
                AcademicProgramId = program.Id,
                IsOffered = true,
                CreatedAtUtc = now,
                UpdatedAtUtc = now,
                RowVersion = []
            });

        await db.SaveChangesAsync(cancellationToken);

        var framework = await db.CurriculumFrameworks
            .AsNoTracking()
            .SingleAsync(
                x =>
                    x.OwnerSchoolId == null &&
                    x.Code == definition.PackCode &&
                    x.IsActive,
                cancellationToken);

        var version = await db.CurriculumFrameworkVersions
            .AsNoTracking()
            .Where(x => x.FrameworkId == framework.Id && x.IsActive)
            .OrderByDescending(x => x.UpdatedAtUtc)
            .FirstAsync(cancellationToken);

        var levels = CurriculumLevelIdentityRegistry.ForPack(definition.PackCode);
        if (levels.Count == 0)
        {
            throw new InvalidOperationException(
                $"No curriculum levels registered for {definition.PackCode}.");
        }

        var gradeByLogicalLevel = new Dictionary<int, GradeLevel>();
        var usedGradeNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (var logicalLevel in levels.Select(x => x.LogicalLevel).Distinct().OrderBy(x => x))
        {
            var representative = levels.First(x => x.LogicalLevel == logicalLevel);
            var gradeName = representative.Label;

            if (!usedGradeNames.Add(gradeName))
            {
                gradeName = $"{representative.Label} — Level {logicalLevel}";
                if (!usedGradeNames.Add(gradeName))
                {
                    throw new InvalidOperationException(
                        $"Unable to create a unique grade name for {definition.PackCode} logical level {logicalLevel}.");
                }
            }

            var grade = new GradeLevel
            {
                Id = Guid.NewGuid(),
                SchoolId = school.Id,
                Name = gradeName,
                Order = logicalLevel
            };
            gradeByLogicalLevel[logicalLevel] = grade;
            db.GradeLevels.Add(grade);
        }

        await db.SaveChangesAsync(cancellationToken);

        var classes = new List<SeededClass>();
        foreach (var level in levels.OrderBy(x => x.LogicalLevel).ThenBy(x => x.Pathway ?? string.Empty))
        {
            var grade = gradeByLogicalLevel[level.LogicalLevel];
            var adoption = new SchoolCurriculumAdoption
            {
                Id = Guid.NewGuid(),
                SchoolId = school.Id,
                AcademicYearId = academicYear.Id,
                AcademicProgramId = program.Id,
                GradeLevelId = grade.Id,
                SubjectId = subject.Id,
                FrameworkVersionId = version.Id,
                CurriculumLevelKey = level.Key,
                CurriculumLogicalLevel = level.LogicalLevel,
                CurriculumLevelLabel = level.Label,
                CurriculumStage = level.Stage,
                CurriculumPathway = level.Pathway,
                IsPrimary = true,
                IsActive = true,
                CreatedAtUtc = now,
                UpdatedAtUtc = now,
                RowVersion = []
            };

            var pathToken = PathToken(level.Pathway);
            var classCode = $"L{level.LogicalLevel:D2}-{pathToken}-A";
            var className = string.IsNullOrWhiteSpace(level.Pathway)
                ? $"{level.Label} — A"
                : $"{level.Label} — {level.Pathway} — A";

            var classGroup = new ClassGroup
            {
                Id = Guid.NewGuid(),
                SchoolId = school.Id,
                AcademicYearId = academicYear.Id,
                AcademicProgramId = program.Id,
                GradeLevelId = grade.Id,
                CurriculumAdoptionId = adoption.Id,
                Name = Compact(className, 150),
                NormalizedName = Compact(className, 150).ToUpperInvariant(),
                Code = classCode,
                NormalizedCode = classCode.ToUpperInvariant(),
                Status = AcademicStructureStatus.Active,
                RowVersion = []
            };

            db.SchoolCurriculumAdoptions.Add(adoption);
            db.ClassGroups.Add(classGroup);
            classes.Add(new SeededClass(classGroup, grade, adoption, level));
        }

        await db.SaveChangesAsync(cancellationToken);

        var adminEmail = $"{definition.Key}.admin@edulytiks.com";
        var supervisorEmail = $"{definition.Key}.supervisor@edulytiks.com";
        var primaryTeacherEmail = $"{definition.Key}.teacher.primary@edulytiks.com";
        var secondaryTeacherEmail = $"{definition.Key}.teacher.secondary@edulytiks.com";
        var primaryStudentEmail = $"{definition.Key}.student.primary@edulytiks.com";
        var secondaryStudentEmail = $"{definition.Key}.student.secondary@edulytiks.com";

        var admin = await CreateDemoUserAsync(
            userManager, school.Id, adminEmail, RoleNames.SchoolAdmin, password);
        var supervisor = await CreateDemoUserAsync(
            userManager, school.Id, supervisorEmail, RoleNames.SubjectSupervisor, password);
        var primaryTeacher = await CreateDemoUserAsync(
            userManager, school.Id, primaryTeacherEmail, RoleNames.Teacher, password);
        var secondaryTeacher = await CreateDemoUserAsync(
            userManager, school.Id, secondaryTeacherEmail, RoleNames.Teacher, password);
        var primaryStudent = await CreateDemoUserAsync(
            userManager, school.Id, primaryStudentEmail, RoleNames.Student, password);
        var secondaryStudent = await CreateDemoUserAsync(
            userManager, school.Id, secondaryStudentEmail, RoleNames.Student, password);

        // SubjectSupervisor role assignment is authoritative here. The PostgreSQL
        // trigger installed by AutoAssignMathSupervisorsAndRefreshMastery creates
        // the Mathematics SubjectSupervisorAssignment automatically.
        foreach (var seededClass in classes)
        {
            db.TeacherAssignments.Add(
                new TeacherAssignment
                {
                    Id = Guid.NewGuid(),
                    SchoolId = school.Id,
                    TeacherUserId = seededClass.Level.LogicalLevel <= 6
                        ? primaryTeacher.Id
                        : secondaryTeacher.Id,
                    ClassGroupId = seededClass.ClassGroup.Id,
                    SubjectId = subject.Id,
                    AcademicYearId = academicYear.Id,
                    CreatedAtUtc = now
                });
        }

        var primaryLoginClass = SelectLoginClass(
            classes,
            definition.PrimaryLoginLogicalLevel,
            null);

        var secondaryLoginClass = SelectLoginClass(
            classes,
            definition.SecondaryLoginLogicalLevel,
            definition.SecondaryLoginPathway);

        var studentsByClass = new Dictionary<Guid, List<StudentProfile>>();
        var schoolStudentSequence = 0;

        foreach (var seededClass in classes)
        {
            var students = new List<StudentProfile>(25);

            for (var index = 0; index < 25; index++)
            {
                schoolStudentSequence++;

                var userId = index == 0 && seededClass.ClassGroup.Id == primaryLoginClass.ClassGroup.Id
                    ? primaryStudent.Id
                    : index == 0 && seededClass.ClassGroup.Id == secondaryLoginClass.ClassGroup.Id
                        ? secondaryStudent.Id
                        : (Guid?)null;

                var firstName = FirstNames[index % FirstNames.Length];
                var lastName = LastNames[(index + seededClass.Level.LogicalLevel) % LastNames.Length];
                var number = $"{definition.Key[..Math.Min(definition.Key.Length, 4)].ToUpperInvariant()}-{schoolStudentSequence:D4}";

                var profile = new StudentProfile
                {
                    Id = Guid.NewGuid(),
                    SchoolId = school.Id,
                    UserId = userId,
                    StudentNumber = number,
                    NormalizedStudentNumber = number,
                    FirstName = firstName,
                    LastName = lastName,
                    DisplayName = $"{firstName} {lastName}",
                    Status = AcademicStructureStatus.Active,
                    IsArchived = false,
                    CreatedAtUtc = now.AddMonths(-2),
                    UpdatedAtUtc = now,
                    RowVersion = []
                };

                students.Add(profile);
                db.StudentProfiles.Add(profile);
                db.StudentEnrollments.Add(
                    new StudentEnrollment
                    {
                        Id = Guid.NewGuid(),
                        SchoolId = school.Id,
                        StudentProfileId = profile.Id,
                        ClassGroupId = seededClass.ClassGroup.Id,
                        AcademicYearId = academicYear.Id,
                        EnrolledAtUtc = now.AddMonths(-1)
                    });
            }

            studentsByClass[seededClass.ClassGroup.Id] = students;
        }

        await db.SaveChangesAsync(cancellationToken);

        foreach (var seededClass in classes)
        {
            await OfficialCurriculumOutcomeMaterializer.EnsureAsync(
                db,
                seededClass.Adoption,
                cancellationToken);
        }

        var term1 = await db.Terms
            .SingleAsync(
                x =>
                    x.SchoolId == school.Id &&
                    x.AcademicYearId == academicYear.Id &&
                    x.Name == "Term 1",
                cancellationToken);

        foreach (var deepClass in SelectDeepClasses(definition, classes))
        {
            var teacher = deepClass.Level.LogicalLevel <= 6
                ? primaryTeacher
                : secondaryTeacher;

            await SeedDeepClassDataAsync(
                db,
                school,
                academicYear,
                term1,
                subject,
                deepClass,
                studentsByClass[deepClass.ClassGroup.Id],
                teacher.Id,
                deepClass.ClassGroup.Id == primaryLoginClass.ClassGroup.Id ||
                deepClass.ClassGroup.Id == secondaryLoginClass.ClassGroup.Id,
                cancellationToken);
        }

        db.AuditLogs.AddRange(
            new AuditLog
            {
                Id = Guid.NewGuid(),
                SchoolId = school.Id,
                ActorUserId = supervisor.Id,
                ActorRole = RoleNames.SubjectSupervisor,
                Action = "Demo.CurriculumConfigured",
                EntityType = "School",
                EntityId = school.Id.ToString("D"),
                OccurredAtUtc = now.AddDays(-21),
                CorrelationId = $"demo-{definition.Key}-curriculum",
                IpAddress = "127.0.0.1",
                UserAgent = "Edulytics Demo Seeder",
                OldValuesJson = "{}",
                NewValuesJson = "{\"status\":\"configured\"}",
                ResultSummary = "Curriculum, grades and classes configured for the meeting dataset.",
                Source = "MeetingDemoSeed",
                Feature = "Curriculum"
            },
            new AuditLog
            {
                Id = Guid.NewGuid(),
                SchoolId = school.Id,
                ActorUserId = primaryTeacher.Id,
                ActorRole = RoleNames.Teacher,
                Action = "Demo.AssessmentsPublished",
                EntityType = "School",
                EntityId = school.Id.ToString("D"),
                OccurredAtUtc = now.AddDays(-3),
                CorrelationId = $"demo-{definition.Key}-assessment",
                IpAddress = "127.0.0.1",
                UserAgent = "Edulytics Demo Seeder",
                OldValuesJson = "{}",
                NewValuesJson = "{\"status\":\"published\"}",
                ResultSummary = "Representative assessments and results published.",
                Source = "MeetingDemoSeed",
                Feature = "Assessments"
            });

        await db.SaveChangesAsync(cancellationToken);

        Console.WriteLine(
            $"MEETING_DEMO_SCHOOL_COMPLETED key={definition.Key} schoolId={school.Id:D} classes={classes.Count} students={studentsByClass.Values.Sum(x => x.Count)}");

        var accounts = new[]
        {
            new DemoAccount(definition.Key, RoleNames.SchoolAdmin, adminEmail, null),
            new DemoAccount(definition.Key, RoleNames.SubjectSupervisor, supervisorEmail, null),
            new DemoAccount(definition.Key, $"{RoleNames.Teacher}-Primary", primaryTeacherEmail, primaryLoginClass.ClassGroup.Name),
            new DemoAccount(definition.Key, $"{RoleNames.Teacher}-Secondary", secondaryTeacherEmail, secondaryLoginClass.ClassGroup.Name),
            new DemoAccount(definition.Key, $"{RoleNames.Student}-Primary", primaryStudentEmail, primaryLoginClass.ClassGroup.Name),
            new DemoAccount(definition.Key, $"{RoleNames.Student}-Secondary", secondaryStudentEmail, secondaryLoginClass.ClassGroup.Name)
        };

        return new SeededSchool(school.Id, accounts);
    }

    private static async Task SeedDeepClassDataAsync(
        EdulyticsDbContext db,
        School school,
        AcademicYear academicYear,
        Term term,
        Subject subject,
        SeededClass seededClass,
        IReadOnlyList<StudentProfile> students,
        Guid teacherUserId,
        bool seedPrivateHistory,
        CancellationToken cancellationToken)
    {
        var outcomes = await db.LearningOutcomes
            .AsNoTracking()
            .Where(
                x =>
                    x.SchoolId == school.Id &&
                    x.CurriculumAdoptionId == seededClass.Adoption.Id)
            .OrderBy(x => x.TopicId)
            .ThenBy(x => x.Order)
            .ToListAsync(cancellationToken);

        if (outcomes.Count < 5)
        {
            throw new InvalidOperationException(
                $"Deep demo class '{seededClass.ClassGroup.Name}' has only {outcomes.Count} materialized outcomes.");
        }

        var selectedOutcomes = outcomes
            .GroupBy(x => x.TopicId)
            .OrderByDescending(x => x.Count())
            .ThenBy(x => x.Key)
            .FirstOrDefault(x => x.Count() >= 5)?
            .Take(5)
            .ToArray()
            ?? outcomes.Take(5).ToArray();

        var topicId = selectedOutcomes[0].TopicId;
        var lessonId = await ResolveLessonIdAsync(
            db,
            seededClass.Adoption,
            selectedOutcomes,
            cancellationToken);

        var generatedByAssessment = new List<IReadOnlyList<GeneratedMathematicsItem>>();
        var assessmentDates = new[]
        {
            DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-28)),
            DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-14)),
            DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-3)),
            DateOnly.FromDateTime(DateTime.UtcNow.AddDays(3))
        };
        var titles = new[]
        {
            "Baseline Diagnostic",
            "Unit Checkpoint",
            "Term 1 Mathematics Assessment",
            "Next Learning Check"
        };

        for (var assessmentIndex = 0; assessmentIndex < 4; assessmentIndex++)
        {
            var generated = GenerateQuestionBatch(
                school.Id,
                seededClass.Adoption,
                topicId,
                lessonId,
                selectedOutcomes,
                AssessmentPurpose.TeacherAssessment,
                StableSeed($"{school.Id:N}|{seededClass.ClassGroup.Id:N}|assessment|{assessmentIndex}"));

            generatedByAssessment.Add(generated);

            var assessment = new Assessment
            {
                Id = Guid.NewGuid(),
                SchoolId = school.Id,
                SubjectId = subject.Id,
                ClassGroupId = seededClass.ClassGroup.Id,
                AcademicYearId = academicYear.Id,
                TermId = term.Id,
                Title = titles[assessmentIndex],
                AssessmentDate = assessmentDates[assessmentIndex],
                MaxScore = 50m,
                Status = assessmentIndex == 3
                    ? AssessmentStatus.Open
                    : AssessmentStatus.Closed,
                TargetType = AssessmentTargetType.Class,
                TargetStudentProfileId = null,
                DeliveryMode = assessmentIndex is 1 or 3
                    ? AssessmentDeliveryMode.Online
                    : AssessmentDeliveryMode.Offline,
                DifficultyBand = assessmentIndex == 3
                    ? AssessmentDifficultyBand.Stretch
                    : AssessmentDifficultyBand.AtClassLevel,
                CreatedByUserId = teacherUserId,
                CreatedAtUtc = assessmentDates[assessmentIndex].ToDateTime(new TimeOnly(8, 0)).AddDays(-3),
                UpdatedAtUtc = DateTime.UtcNow,
                RowVersion = []
            };

            db.Assessments.Add(assessment);

            foreach (var pair in generated.Select((value, index) => (value, index)))
            {
                var item = pair.value.Item;
                var question = new AssessmentQuestion
                {
                    Id = item.Id,
                    SchoolId = school.Id,
                    AssessmentId = assessment.Id,
                    Prompt = item.Prompt,
                    MaxScore = 10m,
                    Order = pair.index + 1
                };

                db.AssessmentItems.Add(item);
                db.AssessmentItemOutcomes.Add(pair.value.OutcomeLink);
                db.AssessmentQuestions.Add(question);
                db.QuestionLearningOutcomes.Add(
                    new QuestionLearningOutcome
                    {
                        Id = Guid.NewGuid(),
                        SchoolId = school.Id,
                        AssessmentQuestionId = question.Id,
                        LearningOutcomeId = pair.value.OutcomeLink.LearningOutcomeId
                    });
            }

            if (assessment.Status == AssessmentStatus.Closed)
            {
                foreach (var studentPair in students.Select((value, index) => (value, index)))
                {
                    var resultId = Guid.NewGuid();
                    var questionScores = new decimal[generated.Count];

                    for (var questionIndex = 0; questionIndex < generated.Count; questionIndex++)
                    {
                        questionScores[questionIndex] = FormalQuestionScore(
                            studentPair.index,
                            questionIndex,
                            assessmentIndex,
                            seededClass.Level.LogicalLevel);
                    }

                    var score = questionScores.Sum();
                    var percentage = decimal.Round(score / 50m * 100m, 2);

                    db.AssessmentResults.Add(
                        new AssessmentResult
                        {
                            Id = resultId,
                            SchoolId = school.Id,
                            AssessmentId = assessment.Id,
                            StudentProfileId = studentPair.value.Id,
                            Score = score,
                            Percentage = percentage,
                            EnteredByUserId = teacherUserId,
                            EnteredAtUtc = assessment.AssessmentDate.ToDateTime(new TimeOnly(15, 30)),
                            UpdatedAtUtc = assessment.AssessmentDate.ToDateTime(new TimeOnly(15, 30)),
                            RowVersion = []
                        });

                    for (var questionIndex = 0; questionIndex < generated.Count; questionIndex++)
                    {
                        var item = generated[questionIndex].Item;
                        var qScore = questionScores[questionIndex];

                        db.StudentAnswers.Add(
                            new StudentAnswer
                            {
                                Id = Guid.NewGuid(),
                                SchoolId = school.Id,
                                AssessmentResultId = resultId,
                                AssessmentQuestionId = item.Id,
                                ResponseText = qScore >= 8m
                                    ? item.CorrectAnswer
                                    : "Demo learner response",
                                Score = qScore,
                                UpdatedAtUtc = assessment.AssessmentDate.ToDateTime(new TimeOnly(15, 25))
                            });
                    }
                }
            }
        }

        await db.SaveChangesAsync(cancellationToken);

        var practiceItems = generatedByAssessment[1];
        foreach (var studentPair in students.Select((value, index) => (value, index)))
        {
            await AddPracticeAttemptAsync(
                db,
                school.Id,
                seededClass.Adoption.Id,
                lessonId,
                studentPair.value,
                practiceItems,
                studentPair.index,
                seededClass.Level.LogicalLevel,
                isPrivate: false,
                attemptOffsetDays: 7 + studentPair.index % 5,
                cancellationToken);
        }

        if (seedPrivateHistory)
        {
            var linkedStudent = students.Single(x => x.UserId.HasValue);

            await AddPracticeAttemptAsync(
                db,
                school.Id,
                seededClass.Adoption.Id,
                lessonId,
                linkedStudent,
                practiceItems,
                2,
                seededClass.Level.LogicalLevel,
                isPrivate: true,
                attemptOffsetDays: 5,
                cancellationToken);

            await AddPracticeAttemptAsync(
                db,
                school.Id,
                seededClass.Adoption.Id,
                lessonId,
                linkedStudent,
                practiceItems,
                8,
                seededClass.Level.LogicalLevel,
                isPrivate: true,
                attemptOffsetDays: 1,
                cancellationToken);
        }

        await db.SaveChangesAsync(cancellationToken);
    }

    private static async Task AddPracticeAttemptAsync(
        EdulyticsDbContext db,
        Guid schoolId,
        Guid adoptionId,
        Guid? lessonId,
        StudentProfile student,
        IReadOnlyList<GeneratedMathematicsItem> items,
        int studentIndex,
        int logicalLevel,
        bool isPrivate,
        int attemptOffsetDays,
        CancellationToken cancellationToken)
    {
        var started = DateTime.UtcNow
            .AddDays(-attemptOffsetDays)
            .AddMinutes(-(studentIndex % 17));

        var attempt = new PracticeAttempt
        {
            Id = Guid.NewGuid(),
            SchoolId = schoolId,
            StudentProfileId = student.Id,
            CurriculumAdoptionId = adoptionId,
            CurriculumPedagogicalLessonId = lessonId,
            IsPrivate = isPrivate,
            Status = PracticeAttemptStatus.Submitted,
            StartedAtUtc = started,
            SubmittedAtUtc = started.AddMinutes(18),
            Score = 0m,
            MaxScore = items.Count,
            Percentage = 0m,
            RowVersion = []
        };

        db.PracticeAttempts.Add(attempt);

        var earned = 0m;
        foreach (var pair in items.Select((value, index) => (value, index)))
        {
            var item = pair.value.Item;
            var roll = (
                studentIndex * 17 +
                pair.index * 23 +
                logicalLevel * 11 +
                (isPrivate ? 13 : 0)) % 100;

            var target = Math.Clamp(
                55 + (studentIndex * 7 % 35) + (isPrivate ? 5 : 0),
                40,
                95);

            var correct = roll < target;
            var score = correct ? 1m : 0m;
            earned += score;

            var attemptItem = new PracticeAttemptItem
            {
                Id = Guid.NewGuid(),
                SchoolId = schoolId,
                PracticeAttemptId = attempt.Id,
                AssessmentItemId = item.Id,
                Order = pair.index + 1,
                MaxScore = 1m
            };

            db.PracticeAttemptItems.Add(attemptItem);
            db.PracticeResponses.Add(
                new PracticeResponse
                {
                    Id = Guid.NewGuid(),
                    SchoolId = schoolId,
                    PracticeAttemptItemId = attemptItem.Id,
                    Answer = correct ? item.CorrectAnswer : "Demo learner response",
                    IsCorrect = correct,
                    Score = score,
                    Feedback = correct
                        ? "Correct. The method is secure."
                        : "Review the worked solution and try an equivalent problem.",
                    AnsweredAtUtc = started.AddMinutes(4 + pair.index * 2)
                });

            db.StudentItemExposures.Add(
                new StudentItemExposure
                {
                    Id = Guid.NewGuid(),
                    SchoolId = schoolId,
                    StudentProfileId = student.Id,
                    AssessmentItemId = item.Id,
                    ExposureFingerprint = item.ExposureFingerprint,
                    ExposedAtUtc = started.AddMinutes(pair.index)
                });

            if (!isPrivate)
            {
                db.LearningEvidence.Add(
                    new LearningEvidence
                    {
                        Id = Guid.NewGuid(),
                        SchoolId = schoolId,
                        StudentProfileId = student.Id,
                        LearningOutcomeId = pair.value.OutcomeLink.LearningOutcomeId,
                        PracticeAttemptId = attempt.Id,
                        AssessmentItemId = item.Id,
                        EvidenceType = LearningEvidenceType.Practice,
                        Difficulty = item.Difficulty,
                        IsCorrect = correct,
                        Score = score,
                        MaxScore = 1m,
                        OccurredAtUtc = started.AddMinutes(4 + pair.index * 2)
                    });
            }
        }

        attempt.Score = earned;
        attempt.Percentage = items.Count == 0
            ? 0m
            : decimal.Round(earned / items.Count * 100m, 2);

        await Task.CompletedTask;
    }

    private static IReadOnlyList<GeneratedMathematicsItem> GenerateQuestionBatch(
        Guid schoolId,
        SchoolCurriculumAdoption adoption,
        Guid topicId,
        Guid? lessonId,
        IReadOnlyList<LearningOutcome> outcomes,
        AssessmentPurpose purpose,
        int seed)
    {
        var profiles = outcomes
            .Select(
                outcome =>
                    new MathematicsOutcomeGenerationProfile(
                        outcome.Id,
                        outcome.Code,
                        [MathematicsGeneratorFamily.CurriculumContextCheck])
                    {
                        GenerationContext = outcome.Description,
                        IsContextualAssisted = true
                    })
            .ToArray();

        var blueprint = new AssessmentBlueprint(
            schoolId,
            adoption.Id,
            adoption.CurriculumLevelKey!,
            topicId,
            lessonId,
            purpose,
            outcomes.Count,
            outcomes
                .Select(
                    outcome =>
                        new OutcomeBlueprintAllocation(
                            outcome.Id,
                            1,
                            1m,
                            "MeetingDemoCoverage"))
                .ToArray(),
            [
                new DifficultyBlueprintAllocation(AssessmentItemDifficulty.Easy, 2),
                new DifficultyBlueprintAllocation(AssessmentItemDifficulty.Medium, 2),
                new DifficultyBlueprintAllocation(AssessmentItemDifficulty.Challenging, 1)
            ],
            [
                new QuestionFamilyBlueprintAllocation(AssessmentQuestionFamily.DirectComputation, 1),
                new QuestionFamilyBlueprintAllocation(AssessmentQuestionFamily.StructuredMethod, 1),
                new QuestionFamilyBlueprintAllocation(AssessmentQuestionFamily.AppliedProblem, 2),
                new QuestionFamilyBlueprintAllocation(AssessmentQuestionFamily.MathematicalReasoning, 1)
            ],
            [
                new ItemTypeBlueprintAllocation(AssessmentItemType.Numeric, 2),
                new ItemTypeBlueprintAllocation(AssessmentItemType.ShortAnswer, 2),
                new ItemTypeBlueprintAllocation(AssessmentItemType.MultipleChoice, 1)
            ],
            outcomes
                .Select(
                    outcome =>
                        new OutcomeEvidenceRequirement(
                            outcome.Id,
                            1,
                            true,
                            true))
                .ToArray(),
            [],
            "meeting-demo-v1");

        return new UniversalMathematicsQuestionGenerationEngine()
            .Generate(
                new MathematicsGenerationRequest(
                    blueprint,
                    profiles,
                    seed))
            .Items;
    }

    private static async Task<Guid?> ResolveLessonIdAsync(
        EdulyticsDbContext db,
        SchoolCurriculumAdoption adoption,
        IReadOnlyList<LearningOutcome> outcomes,
        CancellationToken cancellationToken)
    {
        var officialNodeIds = outcomes
            .Where(x => x.OfficialContentNodeId.HasValue)
            .Select(x => x.OfficialContentNodeId!.Value)
            .ToArray();

        var logicalLevel = adoption.CurriculumLogicalLevel!.Value;
        var pathway = adoption.CurriculumPathway;

        var compatibleLessonIds = await db.CurriculumPedagogicalLessons
            .AsNoTracking()
            .Where(
                x =>
                    x.FrameworkVersionId == adoption.FrameworkVersionId &&
                    x.LogicalLevelFrom <= logicalLevel &&
                    logicalLevel <= x.LogicalLevelTo &&
                    (string.IsNullOrWhiteSpace(pathway)
                        ? x.Pathway == null || x.Pathway == ""
                        : x.Pathway == pathway))
            .OrderBy(x => x.SortOrder)
            .Select(x => x.Id)
            .ToArrayAsync(cancellationToken);

        if (compatibleLessonIds.Length == 0)
            return null;

        if (officialNodeIds.Length > 0)
        {
            var mapped = await db.CurriculumPedagogicalLessonOutcomes
                .AsNoTracking()
                .Where(
                    x =>
                        compatibleLessonIds.Contains(x.PedagogicalLessonId) &&
                        officialNodeIds.Contains(x.OutcomeNodeId))
                .OrderBy(x => x.SortOrder)
                .Select(x => (Guid?)x.PedagogicalLessonId)
                .FirstOrDefaultAsync(cancellationToken);

            if (mapped.HasValue)
                return mapped.Value;
        }

        return compatibleLessonIds[0];
    }

    private static SeededClass SelectLoginClass(
        IReadOnlyList<SeededClass> classes,
        int logicalLevel,
        string? pathway)
    {
        var candidates = classes
            .Where(x => x.Level.LogicalLevel == logicalLevel)
            .ToArray();

        if (!string.IsNullOrWhiteSpace(pathway))
        {
            var exact = candidates.SingleOrDefault(
                x => string.Equals(
                    x.Level.Pathway,
                    pathway,
                    StringComparison.OrdinalIgnoreCase));

            if (exact is not null)
                return exact;
        }

        return candidates
            .OrderBy(x => x.Level.Pathway ?? string.Empty)
            .First();
    }

    private static IReadOnlyList<SeededClass> SelectDeepClasses(
        SchoolDefinition definition,
        IReadOnlyList<SeededClass> classes)
    {
        return DeepLogicalLevels
            .Select(
                logicalLevel =>
                {
                    var candidates = classes
                        .Where(x => x.Level.LogicalLevel == logicalLevel)
                        .ToArray();

                    var preferred = PreferredPathway(
                        definition.PackCode,
                        logicalLevel);

                    if (!string.IsNullOrWhiteSpace(preferred))
                    {
                        var exact = candidates.SingleOrDefault(
                            x => string.Equals(
                                x.Level.Pathway,
                                preferred,
                                StringComparison.OrdinalIgnoreCase));

                        if (exact is not null)
                            return exact;
                    }

                    return candidates
                        .OrderBy(x => x.Level.Pathway ?? string.Empty)
                        .First();
                })
            .ToArray();
    }

    private static string? PreferredPathway(
        string packCode,
        int logicalLevel) =>
        packCode switch
        {
            MathematicsCurriculumPackRegistry.CambridgeCode
                when logicalLevel is 10 or 11 => "Extended",
            MathematicsCurriculumPackRegistry.UaeCode
                when logicalLevel >= 5 => "Advanced",
            MathematicsCurriculumPackRegistry.PolandCode
                when logicalLevel is >= 9 and <= 12 => "Liceum ogólnokształcące",
            MathematicsCurriculumPackRegistry.PolandCode
                when logicalLevel == 13 => "Technikum",
            _ => null
        };

    private static async Task<ApplicationUser> CreateDemoUserAsync(
        UserManager<ApplicationUser> userManager,
        Guid schoolId,
        string email,
        string role,
        string password)
    {
        var user = new ApplicationUser
        {
            Id = Guid.NewGuid(),
            UserName = email,
            Email = email,
            EmailConfirmed = true,
            IsActive = true,
            SchoolId = schoolId,
            CreatedAtUtc = DateTime.UtcNow,
            UpdatedAtUtc = DateTime.UtcNow,
            LockoutEnd = null,
            AccessFailedCount = 0
        };

        var create = await userManager.CreateAsync(user, password);
        EnsureIdentitySucceeded(create, $"create {email}");

        var addRole = await userManager.AddToRoleAsync(user, role);
        EnsureIdentitySucceeded(addRole, $"add role {role} to {email}");

        return user;
    }

    private static decimal FormalQuestionScore(
        int studentIndex,
        int questionIndex,
        int assessmentIndex,
        int logicalLevel)
    {
        var basePercentage =
            52 +
            (studentIndex * 11 + logicalLevel * 3) % 37;

        if (studentIndex % 9 == 0)
            basePercentage -= 18;

        var trend = assessmentIndex switch
        {
            0 => -6,
            1 => 2,
            2 => 9,
            _ => 0
        };

        var questionVariation =
            ((questionIndex * 7 + studentIndex * 3) % 11) - 5;

        var percentage = Math.Clamp(
            basePercentage + trend + questionVariation,
            25,
            99);

        return decimal.Round(
            percentage / 10m,
            1,
            MidpointRounding.AwayFromZero);
    }

    private static int StableSeed(string value)
    {
        unchecked
        {
            uint hash = 2166136261;
            foreach (var character in value)
            {
                hash ^= character;
                hash *= 16777619;
            }

            return (int)(hash & 0x7fffffff);
        }
    }

    private static string PathToken(string? pathway)
    {
        if (string.IsNullOrWhiteSpace(pathway))
            return "SHARED";

        var token = new string(
            pathway
                .ToUpperInvariant()
                .Where(char.IsLetterOrDigit)
                .Take(12)
                .ToArray());

        return string.IsNullOrWhiteSpace(token)
            ? "PATH"
            : token;
    }

    private static string Compact(string value, int maximumLength) =>
        value.Length <= maximumLength
            ? value
            : value[..maximumLength];

    private static void EnsureIdentitySucceeded(
        IdentityResult result,
        string operation)
    {
        if (result.Succeeded)
            return;

        throw new InvalidOperationException(
            $"Meeting demo provisioning failed to {operation}: " +
            string.Join("; ", result.Errors.Select(x => x.Description)));
    }
}
