using Edulytics.Core.AssessmentIntelligence;
using Edulytics.Core.Constants;
using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;
using Edulytics.Core.Enums;
using Edulytics.Core.MathematicsGeneration;
using Edulytics.Data.Contexts;
using Edulytics.Data.Identity;
using Edulytics.Data.Repositories;
using Edulytics.Data.Seeding;
using Edulytics.Services.Analytics;
using Edulytics.Services.AssessmentIntelligence;
using Edulytics.Services.MathematicsGeneration;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Edulytics.Web.Bootstrap;

/// <summary>
/// One-shot production-rehearsal dataset provisioner for the staging service.
/// It resets school-scoped operational data while preserving the platform
/// curriculum catalogue, EF migrations, Identity roles and data-protection keys.
/// A platform-scoped idempotency marker prevents an accidental second reset.
/// </summary>
internal static class MeetingDemoProvisioner
{
    private const string TargetRenderServiceId = "srv-dakq5n2fngtc73a62i10";
    private const string SeedVersion = "production-rehearsal-2026-10-01-v1";
    private const string MarkerOperation = "ProductionRehearsalSeed";
    private const string RepairMarkerOperation = "ProductionRehearsalRepair";
    private const string RepairVersion = "production-rehearsal-repair-2026-10-02-v2";

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
        string StageKey,
        int MinimumLogicalLevel,
        int MaximumLogicalLevel,
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
            "cambridge-primary",
            "Horizon British Primary School",
            "REHEARSAL-GB-PRIMARY",
            "GB",
            "London",
            "en",
            "Europe/London",
            MathematicsCurriculumPackRegistry.CambridgeCode,
            "British Programme",
            "BRITISH",
            "primary",
            1,
            6,
            4,
            5,
            null),
        new(
            "cambridge-middle",
            "Horizon British Middle School",
            "REHEARSAL-GB-MIDDLE",
            "GB",
            "London",
            "en",
            "Europe/London",
            MathematicsCurriculumPackRegistry.CambridgeCode,
            "British Programme",
            "BRITISH",
            "middle",
            7,
            9,
            8,
            9,
            null),
        new(
            "cambridge-secondary",
            "Horizon British Secondary School",
            "REHEARSAL-GB-SECONDARY",
            "GB",
            "London",
            "en",
            "Europe/London",
            MathematicsCurriculumPackRegistry.CambridgeCode,
            "British Programme",
            "BRITISH",
            "secondary",
            10,
            13,
            11,
            12,
            "Extended"),

        new(
            "commoncore-primary",
            "Liberty American Primary School",
            "REHEARSAL-US-PRIMARY",
            "US",
            "Boston",
            "en",
            "America/New_York",
            MathematicsCurriculumPackRegistry.CommonCoreCode,
            "American Programme",
            "AMERICAN",
            "primary",
            1,
            6,
            5,
            6,
            null),
        new(
            "commoncore-middle",
            "Liberty American Middle School",
            "REHEARSAL-US-MIDDLE",
            "US",
            "Boston",
            "en",
            "America/New_York",
            MathematicsCurriculumPackRegistry.CommonCoreCode,
            "American Programme",
            "AMERICAN",
            "middle",
            7,
            9,
            8,
            9,
            null),
        new(
            "commoncore-secondary",
            "Liberty American Secondary School",
            "REHEARSAL-US-SECONDARY",
            "US",
            "Boston",
            "en",
            "America/New_York",
            MathematicsCurriculumPackRegistry.CommonCoreCode,
            "American Programme",
            "AMERICAN",
            "secondary",
            10,
            13,
            11,
            13,
            null),

        new(
            "uae-primary",
            "Emirates Future Primary Academy",
            "REHEARSAL-AE-PRIMARY",
            "AE",
            "Dubai",
            "en",
            "Asia/Dubai",
            MathematicsCurriculumPackRegistry.UaeCode,
            "UAE Programme",
            "UAE",
            "primary",
            1,
            4,
            3,
            4,
            null),
        new(
            "uae-middle",
            "Emirates Future Middle Academy",
            "REHEARSAL-AE-MIDDLE",
            "AE",
            "Dubai",
            "en",
            "Asia/Dubai",
            MathematicsCurriculumPackRegistry.UaeCode,
            "UAE Programme",
            "UAE",
            "middle",
            5,
            8,
            7,
            8,
            "Advanced"),
        new(
            "uae-secondary",
            "Emirates Future Secondary Academy",
            "REHEARSAL-AE-SECONDARY",
            "AE",
            "Dubai",
            "en",
            "Asia/Dubai",
            MathematicsCurriculumPackRegistry.UaeCode,
            "UAE Programme",
            "UAE",
            "secondary",
            9,
            12,
            11,
            12,
            "Advanced"),

        new(
            "polish-primary",
            "Akademia Vistula Szkoła Podstawowa",
            "REHEARSAL-PL-PRIMARY",
            "PL",
            "Warsaw",
            "pl",
            "Europe/Warsaw",
            MathematicsCurriculumPackRegistry.PolandCode,
            "Polish Programme",
            "POLISH",
            "primary",
            1,
            6,
            4,
            6,
            null),
        new(
            "polish-middle",
            "Akademia Vistula Middle School",
            "REHEARSAL-PL-MIDDLE",
            "PL",
            "Warsaw",
            "pl",
            "Europe/Warsaw",
            MathematicsCurriculumPackRegistry.PolandCode,
            "Polish Programme",
            "POLISH",
            "middle",
            7,
            8,
            7,
            8,
            null),
        new(
            "polish-secondary",
            "Akademia Vistula Liceum",
            "REHEARSAL-PL-SECONDARY",
            "PL",
            "Warsaw",
            "pl",
            "Europe/Warsaw",
            MathematicsCurriculumPackRegistry.PolandCode,
            "Polish Programme",
            "POLISH",
            "secondary",
            9,
            13,
            11,
            12,
            "Liceum ogólnokształcące")
    ];

    public static async Task RunAsync(
        EdulyticsDbContext db,
        UserManager<ApplicationUser> userManager,
        IConfiguration configuration,
        CancellationToken cancellationToken = default)
    {
        // Disposable CI rehearsal is separate from the production-only reset path.
        // Four independent guards prevent this option from touching Neon or a
        // long-lived demo database, even if an environment flag is misconfigured.
        var disposableCi =
            string.Equals(Environment.GetEnvironmentVariable("GITHUB_ACTIONS"),
                "true", StringComparison.OrdinalIgnoreCase) &&
            string.Equals(Environment.GetEnvironmentVariable("EDULYTICS_CI_REHEARSAL"),
                "true", StringComparison.OrdinalIgnoreCase) &&
            string.Equals(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"),
                "Staging", StringComparison.Ordinal) &&
            string.Equals(db.Database.GetDbConnection().Database,
                "edulytics_clean_ci", StringComparison.Ordinal);

        if (!disposableCi && !string.Equals(
                Environment.GetEnvironmentVariable("RENDER_SERVICE_ID"),
                TargetRenderServiceId,
                StringComparison.Ordinal))
        {
            return;
        }

        if (!configuration.GetValue<bool>("Edulytics:MeetingDemo:ResetAndSeed"))
            return;

        var activeSchools = disposableCi ? Schools.Take(2).ToArray() : Schools;

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

                foreach (var definition in activeSchools)
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
        if (schoolCount != activeSchools.Length)
        {
            throw new InvalidOperationException(
                $"Production rehearsal verification failed: expected {activeSchools.Length} schools, found {schoolCount}.");
        }

        var seededCodes = await db.Schools
            .AsNoTracking()
            .Select(x => x.SchoolCode)
            .ToArrayAsync(cancellationToken);

        var missingCodes = activeSchools
            .Select(x => x.SchoolCode)
            .Except(seededCodes, StringComparer.OrdinalIgnoreCase)
            .ToArray();

        if (missingCodes.Length > 0)
        {
            throw new InvalidOperationException(
                $"Production rehearsal verification failed: missing schools {string.Join(',', missingCodes)}.");
        }

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

    public static async Task RepairExistingAsync(
        EdulyticsDbContext db,
        CancellationToken cancellationToken = default)
    {
        if (!string.Equals(
                Environment.GetEnvironmentVariable("RENDER_SERVICE_ID"),
                TargetRenderServiceId,
                StringComparison.Ordinal))
        {
            return;
        }

        var alreadyCompleted = await db.IdempotencyRecords
            .AsNoTracking()
            .AnyAsync(
                x =>
                    x.SchoolId == null &&
                    x.ActorUserId == Guid.Empty &&
                    x.Operation == RepairMarkerOperation &&
                    x.IdempotencyKey == RepairVersion &&
                    x.Status == IdempotencyStatus.Completed,
                cancellationToken);

        if (alreadyCompleted)
        {
            Console.WriteLine(
                $"MEETING_DEMO_REPAIR_SKIPPED version={RepairVersion} reason=already-completed");
            return;
        }

        Console.WriteLine($"MEETING_DEMO_REPAIR_BEGIN version={RepairVersion}");

        var repairedClasses = 0;
        var stillUnsupportedClasses = 0;
        var completeClasses = 0;
        var touchedSchools = new List<Guid>();

        foreach (var definition in Schools)
        {
            var school = await db.Schools
                .SingleOrDefaultAsync(
                    x => x.SchoolCode == definition.SchoolCode,
                    cancellationToken);

            if (school is null)
            {
                Console.WriteLine(
                    $"MEETING_DEMO_REPAIR_SCHOOL_SKIPPED key={definition.Key} reason=school-not-found");
                continue;
            }

            var academicYear = await db.AcademicYears
                .Where(x => x.SchoolId == school.Id)
                .OrderByDescending(x => x.StartsOn)
                .FirstOrDefaultAsync(cancellationToken);
            var subject = await db.Subjects
                .SingleOrDefaultAsync(
                    x => x.SchoolId == school.Id && x.NormalizedCode == "MATH",
                    cancellationToken);

            if (academicYear is null || subject is null)
            {
                Console.WriteLine(
                    $"MEETING_DEMO_REPAIR_SCHOOL_SKIPPED key={definition.Key} reason=academic-structure-missing");
                continue;
            }

            var term1 = await db.Terms
                .Where(
                    x =>
                        x.SchoolId == school.Id &&
                        x.AcademicYearId == academicYear.Id)
                .OrderBy(x => x.StartsOn)
                .FirstOrDefaultAsync(cancellationToken);

            if (term1 is null)
            {
                Console.WriteLine(
                    $"MEETING_DEMO_REPAIR_SCHOOL_SKIPPED key={definition.Key} reason=term-missing");
                continue;
            }

            var adoptions = await db.SchoolCurriculumAdoptions
                .Where(
                    x =>
                        x.SchoolId == school.Id &&
                        x.AcademicYearId == academicYear.Id &&
                        x.SubjectId == subject.Id &&
                        x.IsActive)
                .ToListAsync(cancellationToken);

            foreach (var adoption in adoptions)
            {
                await OfficialCurriculumOutcomeMaterializer.EnsureAsync(
                    db,
                    adoption,
                    cancellationToken);
            }

            var adoptionById = adoptions.ToDictionary(x => x.Id);
            var grades = await db.GradeLevels
                .Where(x => x.SchoolId == school.Id)
                .ToDictionaryAsync(x => x.Id, cancellationToken);
            var classGroups = await db.ClassGroups
                .Where(
                    x =>
                        x.SchoolId == school.Id &&
                        x.AcademicYearId == academicYear.Id &&
                        x.CurriculumAdoptionId.HasValue)
                .ToListAsync(cancellationToken);

            var seededClasses = new List<SeededClass>();
            foreach (var classGroup in classGroups)
            {
                if (!classGroup.CurriculumAdoptionId.HasValue ||
                    !adoptionById.TryGetValue(
                        classGroup.CurriculumAdoptionId.Value,
                        out var adoption) ||
                    !grades.TryGetValue(classGroup.GradeLevelId, out var grade))
                {
                    continue;
                }

                var level = CurriculumLevelIdentityRegistry.Find(
                    adoption.CurriculumLevelKey);
                if (level is null)
                    continue;

                seededClasses.Add(
                    new SeededClass(
                        classGroup,
                        grade,
                        adoption,
                        level));
            }

            var deepClasses = SelectDeepClasses(
                definition,
                seededClasses);

            foreach (var deepClass in deepClasses)
            {
                var hasAssessments = await db.Assessments
                    .AsNoTracking()
                    .AnyAsync(
                        x =>
                            x.SchoolId == school.Id &&
                            x.ClassGroupId == deepClass.ClassGroup.Id,
                        cancellationToken);

                if (hasAssessments)
                {
                    completeClasses++;
                    Console.WriteLine(
                        $"MEETING_DEMO_REPAIR_CLASS_SKIPPED school={school.Id:D} class={deepClass.ClassGroup.Id:D} logicalLevel={deepClass.Level.LogicalLevel} reason=already-seeded");
                    continue;
                }

                var outcomeCount = await db.LearningOutcomes
                    .AsNoTracking()
                    .CountAsync(
                        x =>
                            x.SchoolId == school.Id &&
                            x.CurriculumAdoptionId == deepClass.Adoption.Id,
                        cancellationToken);

                if (outcomeCount == 0)
                {
                    stillUnsupportedClasses++;
                    Console.WriteLine(
                        $"MEETING_DEMO_REPAIR_UNSUPPORTED school={school.Id:D} class={deepClass.ClassGroup.Id:D} logicalLevel={deepClass.Level.LogicalLevel} pathway={deepClass.Level.Pathway ?? "shared"} reason=insufficient-materialized-outcomes count={outcomeCount}");
                    continue;
                }

                var studentIds = await db.StudentEnrollments
                    .AsNoTracking()
                    .Where(
                        x =>
                            x.SchoolId == school.Id &&
                            x.AcademicYearId == academicYear.Id &&
                            x.ClassGroupId == deepClass.ClassGroup.Id)
                    .Select(x => x.StudentProfileId)
                    .ToArrayAsync(cancellationToken);

                var students = await db.StudentProfiles
                    .Where(x => studentIds.Contains(x.Id))
                    .OrderBy(x => x.StudentNumber)
                    .ToListAsync(cancellationToken);

                var teacherUserId = await db.TeacherAssignments
                    .AsNoTracking()
                    .Where(
                        x =>
                            x.SchoolId == school.Id &&
                            x.AcademicYearId == academicYear.Id &&
                            x.SubjectId == subject.Id &&
                            x.ClassGroupId == deepClass.ClassGroup.Id)
                    .Select(x => x.TeacherUserId)
                    .FirstOrDefaultAsync(cancellationToken);

                if (students.Count == 0 || teacherUserId == Guid.Empty)
                {
                    Console.WriteLine(
                        $"MEETING_DEMO_REPAIR_CLASS_SKIPPED school={school.Id:D} class={deepClass.ClassGroup.Id:D} logicalLevel={deepClass.Level.LogicalLevel} reason=operational-links-missing");
                    continue;
                }

                await using var classTransaction = await db.Database.BeginTransactionAsync(cancellationToken);
                await SeedDeepClassDataAsync(
                    db,
                    school,
                    academicYear,
                    term1,
                    subject,
                    deepClass,
                    students,
                    teacherUserId,
                    students.Any(x => x.UserId.HasValue),
                    cancellationToken);

                await classTransaction.CommitAsync(cancellationToken);

                repairedClasses++;
                completeClasses++;
                if (!touchedSchools.Contains(school.Id))
                    touchedSchools.Add(school.Id);

                Console.WriteLine(
                    $"MEETING_DEMO_REPAIR_CLASS_COMPLETED school={school.Id:D} class={deepClass.ClassGroup.Id:D} logicalLevel={deepClass.Level.LogicalLevel} outcomes={outcomeCount}");
            }
        }

        var analytics = new AnalyticsProjectionRefreshService(
            new AnalyticsRepository(db),
            new AnalyticsProjectionBuilder());

        foreach (var schoolId in touchedSchools)
        {
            var refresh = await analytics.RefreshSchoolAsync(
                schoolId,
                cancellationToken);

            if (!refresh.Succeeded)
            {
                throw new InvalidOperationException(
                    $"Meeting demo repair analytics refresh failed for school {schoolId:D}: {refresh.Error}.");
            }
        }

        if (stillUnsupportedClasses > 0 || completeClasses != Schools.Length * 2)
        {
            Console.WriteLine(
                $"MEETING_DEMO_REPAIR_INCOMPLETE version={RepairVersion} completeClasses={completeClasses} expectedClasses={Schools.Length * 2} unsupportedClasses={stillUnsupportedClasses}");
            return;
        }

        db.IdempotencyRecords.Add(
            new IdempotencyRecord
            {
                Id = Guid.NewGuid(),
                SchoolId = null,
                ActorUserId = Guid.Empty,
                Operation = RepairMarkerOperation,
                IdempotencyKey = RepairVersion,
                RequestHash = new string('0', 64),
                Status = IdempotencyStatus.Completed,
                ResultStatusCode = 200,
                CreatedAtUtc = DateTime.UtcNow,
                CompletedAtUtc = DateTime.UtcNow,
                ExpiresAtUtc = DateTime.UtcNow.AddYears(10),
                RowVersion = []
            });

        await db.SaveChangesAsync(cancellationToken);

        Console.WriteLine(
            $"MEETING_DEMO_REPAIR_COMPLETED version={RepairVersion} repairedClasses={repairedClasses} unsupportedClasses={stillUnsupportedClasses} touchedSchools={touchedSchools.Count}");
    }

    public static async Task RepairLessonLinksAsync(
        EdulyticsDbContext db,
        CancellationToken cancellationToken = default)
    {
        if (!string.Equals(Environment.GetEnvironmentVariable("RENDER_SERVICE_ID"),
                TargetRenderServiceId, StringComparison.Ordinal))
            return;

        const string operation = "ProductionRehearsalLessonLinkRepair";
        const string version = "production-rehearsal-lesson-links-2026-10-02-v1";
        if (await db.IdempotencyRecords.AsNoTracking().AnyAsync(x =>
                x.SchoolId == null && x.ActorUserId == Guid.Empty &&
                x.Operation == operation && x.IdempotencyKey == version &&
                x.Status == IdempotencyStatus.Completed, cancellationToken))
            return;

        await using var transaction = db.Database.IsRelational()
            ? await db.Database.BeginTransactionAsync(cancellationToken)
            : null;
        var result = await RehearsalLessonLinkRepair.RunAsync(
            db, Schools.Select(x => x.SchoolCode).ToArray(), cancellationToken);
        db.IdempotencyRecords.Add(new IdempotencyRecord {
            Id = Guid.NewGuid(), SchoolId = null, ActorUserId = Guid.Empty,
            Operation = operation, IdempotencyKey = version, RequestHash = new string('0', 64),
            Status = IdempotencyStatus.Completed, ResultStatusCode = 200,
            CreatedAtUtc = DateTime.UtcNow, CompletedAtUtc = DateTime.UtcNow,
            ExpiresAtUtc = DateTime.UtcNow.AddYears(10), RowVersion = []
        });
        await db.SaveChangesAsync(cancellationToken);
        if (transaction is not null)
            await transaction.CommitAsync(cancellationToken);
        Console.WriteLine($"MEETING_DEMO_LESSON_LINK_REPAIR items={result.Items} attempts={result.Attempts}");
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

DELETE FROM "AdaptivePracticeTurns";
DELETE FROM "AdaptiveDecisionSnapshots";
DELETE FROM "AdaptivePracticeShadowObservations";
DELETE FROM "StudentMisconceptionStates";
DELETE FROM "StudentRepresentationFluencyStates";
DELETE FROM "AdaptivePracticeSessions";

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

DELETE FROM "AssessmentTaskResponses";
DELETE FROM "AssessmentAttempts";
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

