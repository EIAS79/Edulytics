using Edulytics.Core.Entities;
using Edulytics.Core.Enums;
using Edulytics.Data.Contexts;
using Edulytics.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Edulytics.Tests.Phase42;

public sealed class StudentPrivatePracticeRepositoryTests
{
    [Fact]
    public async Task Unknown_student_returns_no_curricula_context_or_private_history()
    {
        await using var db = CreateDb();
        var repository = new StudentPrivatePracticeRepository(db);
        var userId = Guid.NewGuid();

        Assert.Empty(await repository.ListCurriculaAsync(userId));
        Assert.Null(await repository.GetContextAsync(userId, Guid.NewGuid()));
        Assert.Empty(await repository.ListPrivateAttemptsAsync(userId));
    }

    [Fact]
    public async Task Owned_student_resolves_curriculum_context_and_only_private_history()
    {
        await using var db = CreateDb();
        var ids = Ids.Create();
        var now = DateTime.UtcNow;
        var officialNodeId = Guid.NewGuid();
        var lessonId = Guid.NewGuid();
        var outcomeId = Guid.NewGuid();
        var privateAttemptId = Guid.NewGuid();

        var student = Student(ids, now);
        var adoption = Adoption(ids, now);
        var classGroup = Class(ids, ids.Adoption);
        var enrollment = Enrollment(ids, ids.Class);
        var outcome = Outcome(ids, outcomeId, officialNodeId);
        var lesson = new CurriculumPedagogicalLesson
        {
            Id = lessonId,
            FrameworkVersionId = ids.Framework,
            Code = "L1",
            UnitKey = "U1",
            UnitTitle = "Number",
            Title = "Addition",
            LogicalLevelFrom = 1,
            LogicalLevelTo = 1,
            NativeLevel = "Grade 1",
            SortOrder = 1,
            CreatedAtUtc = now,
            UpdatedAtUtc = now
        };
        var lessonOutcome = new CurriculumPedagogicalLessonOutcome
        {
            PedagogicalLessonId = lessonId,
            FrameworkVersionId = ids.Framework,
            OutcomeNodeId = officialNodeId,
            SortOrder = 1
        };
        var mastery = new StudentOutcomeMastery
        {
            Id = Guid.NewGuid(),
            SchoolId = ids.School,
            AcademicYearId = ids.Year,
            ClassGroupId = ids.Class,
            SubjectId = ids.Subject,
            StudentProfileId = ids.Student,
            LearningOutcomeId = outcomeId,
            EarnedScore = 6,
            PossibleScore = 10,
            MasteryPercentage = 60,
            EvidenceCount = 2,
            CalculatedAtUtc = now
        };
        var exposure = new StudentItemExposure
        {
            Id = Guid.NewGuid(),
            SchoolId = ids.School,
            StudentProfileId = ids.Student,
            AssessmentItemId = Guid.NewGuid(),
            ExposureFingerprint = "fp-1",
            ExposedAtUtc = now
        };
        var privateAttempt = new PracticeAttempt
        {
            Id = privateAttemptId,
            SchoolId = ids.School,
            StudentProfileId = ids.Student,
            CurriculumAdoptionId = ids.Adoption,
            CurriculumPedagogicalLessonId = lessonId,
            IsPrivate = true,
            Status = PracticeAttemptStatus.Submitted,
            StartedAtUtc = now.AddMinutes(-5),
            SubmittedAtUtc = now,
            Score = 4,
            MaxScore = 5,
            Percentage = 80
        };
        var officialAttempt = new PracticeAttempt
        {
            Id = Guid.NewGuid(),
            SchoolId = ids.School,
            StudentProfileId = ids.Student,
            CurriculumAdoptionId = ids.Adoption,
            IsPrivate = false,
            Status = PracticeAttemptStatus.Submitted,
            StartedAtUtc = now.AddMinutes(-10),
            SubmittedAtUtc = now,
            Score = 5,
            MaxScore = 5,
            Percentage = 100
        };

        await db.AddRangeAsync(student, adoption, classGroup, enrollment, outcome, lesson, lessonOutcome,
            mastery, exposure, privateAttempt, officialAttempt);
        await db.SaveChangesAsync();

        var repository = new StudentPrivatePracticeRepository(db);

        var curricula = await repository.ListCurriculaAsync(ids.User);
        var context = await repository.GetContextAsync(ids.User, ids.Adoption);
        var history = await repository.ListPrivateAttemptsAsync(ids.User);

        var curriculum = Assert.Single(curricula);
        Assert.Equal(ids.Adoption, curriculum.CurriculumAdoptionId);
        Assert.Equal("Grade 1", curriculum.CurriculumLevelLabel);
        Assert.Equal("1A", curriculum.ClassName);

        Assert.NotNull(context);
        Assert.Equal(ids.Student, context!.Student.Id);
        Assert.Equal(ids.Adoption, context.Adoption.Id);
        Assert.Equal(ids.Class, context.ClassGroup.Id);
        Assert.Single(context.LearningOutcomes);
        Assert.Single(context.Lessons);
        Assert.Single(context.LessonOutcomes);
        Assert.Single(context.OfficialMasteries);
        Assert.Single(context.Exposures);

        var historyItem = Assert.Single(history);
        Assert.Equal(privateAttemptId, historyItem.AttemptId);
        Assert.Equal(80m, historyItem.Percentage);
    }

    [Fact]
    public async Task Context_supports_legacy_class_without_explicit_adoption_when_program_and_grade_match()
    {
        await using var db = CreateDb();
        var ids = Ids.Create();
        var now = DateTime.UtcNow;
        await db.AddRangeAsync(
            Student(ids, now),
            Adoption(ids, now),
            Class(ids, null),
            Enrollment(ids, ids.Class));
        await db.SaveChangesAsync();

        var context = await new StudentPrivatePracticeRepository(db).GetContextAsync(ids.User, ids.Adoption);

        Assert.NotNull(context);
        Assert.Equal(ids.Class, context!.ClassGroup.Id);
        Assert.Equal(ids.Adoption, context.Adoption.Id);
    }

    [Fact]
    public async Task Context_fails_closed_when_adoption_has_no_resolved_curriculum_level()
    {
        await using var db = CreateDb();
        var ids = Ids.Create();
        var now = DateTime.UtcNow;
        var adoption = Adoption(ids, now);
        adoption.CurriculumLevelKey = null;
        adoption.CurriculumLogicalLevel = null;
        await db.AddRangeAsync(Student(ids, now), adoption);
        await db.SaveChangesAsync();

        var context = await new StudentPrivatePracticeRepository(db).GetContextAsync(ids.User, ids.Adoption);

        Assert.Null(context);
    }

    [Fact]
    public async Task Add_generated_attempt_persists_items_links_attempt_and_exposures()
    {
        await using var db = CreateDb();
        var ids = Ids.Create();
        var itemId = Guid.NewGuid();
        var attemptId = Guid.NewGuid();
        var item = new AssessmentItem
        {
            Id = itemId,
            SchoolId = ids.School,
            CurriculumAdoptionId = ids.Adoption,
            Prompt = "2 + 3 = ?",
            CorrectAnswer = "5",
            Solution = "2 + 3 = 5",
            ExposureFingerprint = "generated-fp",
            CreatedAtUtc = DateTime.UtcNow
        };
        var outcome = new AssessmentItemOutcome
        {
            Id = Guid.NewGuid(),
            SchoolId = ids.School,
            AssessmentItemId = itemId,
            LearningOutcomeId = Guid.NewGuid()
        };
        var attempt = new PracticeAttempt
        {
            Id = attemptId,
            SchoolId = ids.School,
            StudentProfileId = ids.Student,
            CurriculumAdoptionId = ids.Adoption,
            IsPrivate = true,
            Status = PracticeAttemptStatus.InProgress,
            StartedAtUtc = DateTime.UtcNow,
            MaxScore = 1
        };
        var attemptItem = new PracticeAttemptItem
        {
            Id = Guid.NewGuid(),
            SchoolId = ids.School,
            PracticeAttemptId = attemptId,
            AssessmentItemId = itemId,
            Order = 1,
            MaxScore = 1
        };
        var exposure = new StudentItemExposure
        {
            Id = Guid.NewGuid(),
            SchoolId = ids.School,
            StudentProfileId = ids.Student,
            AssessmentItemId = itemId,
            ExposureFingerprint = "generated-fp",
            ExposedAtUtc = DateTime.UtcNow
        };

        await new StudentPrivatePracticeRepository(db).AddGeneratedAttemptAsync(
            [item], [outcome], attempt, [attemptItem], [exposure]);

        Assert.Single(await db.AssessmentItems.ToListAsync());
        Assert.Single(await db.AssessmentItemOutcomes.ToListAsync());
        Assert.Single(await db.PracticeAttempts.ToListAsync());
        Assert.Single(await db.PracticeAttemptItems.ToListAsync());
        Assert.Single(await db.StudentItemExposures.ToListAsync());
    }

    private static EdulyticsDbContext CreateDb()
    {
        var options = new DbContextOptionsBuilder<EdulyticsDbContext>()
            .UseInMemoryDatabase($"phase42-private-practice-{Guid.NewGuid():N}")
            .Options;
        return new EdulyticsDbContext(options);
    }

    private static StudentProfile Student(Ids ids, DateTime now) => new()
    {
        Id = ids.Student,
        SchoolId = ids.School,
        UserId = ids.User,
        StudentNumber = "S-001",
        NormalizedStudentNumber = "S-001",
        FirstName = "Test",
        LastName = "Student",
        DisplayName = "Test Student",
        Status = AcademicStructureStatus.Active,
        CreatedAtUtc = now,
        UpdatedAtUtc = now
    };

    private static SchoolCurriculumAdoption Adoption(Ids ids, DateTime now) => new()
    {
        Id = ids.Adoption,
        SchoolId = ids.School,
        AcademicYearId = ids.Year,
        AcademicProgramId = ids.Program,
        GradeLevelId = ids.Grade,
        SubjectId = ids.Subject,
        FrameworkVersionId = ids.Framework,
        CurriculumLevelKey = "CCSS-G1",
        CurriculumLogicalLevel = 1,
        CurriculumLevelLabel = "Grade 1",
        IsActive = true,
        IsPrimary = true,
        CreatedAtUtc = now,
        UpdatedAtUtc = now
    };

    private static ClassGroup Class(Ids ids, Guid? curriculumAdoptionId) => new()
    {
        Id = ids.Class,
        SchoolId = ids.School,
        AcademicYearId = ids.Year,
        AcademicProgramId = ids.Program,
        GradeLevelId = ids.Grade,
        CurriculumAdoptionId = curriculumAdoptionId,
        Name = "1A",
        NormalizedName = "1A",
        Code = "1A",
        NormalizedCode = "1A",
        Status = AcademicStructureStatus.Active
    };

    private static StudentEnrollment Enrollment(Ids ids, Guid classId) => new()
    {
        Id = Guid.NewGuid(),
        SchoolId = ids.School,
        StudentProfileId = ids.Student,
        ClassGroupId = classId,
        AcademicYearId = ids.Year,
        EnrolledAtUtc = DateTime.UtcNow
    };

    private static LearningOutcome Outcome(Ids ids, Guid outcomeId, Guid officialNodeId) => new()
    {
        Id = outcomeId,
        SchoolId = ids.School,
        AcademicProgramId = ids.Program,
        FrameworkVersionId = ids.Framework,
        SubjectId = ids.Subject,
        GradeLevelId = ids.Grade,
        CurriculumAdoptionId = ids.Adoption,
        TopicId = ids.Topic,
        OfficialContentNodeId = officialNodeId,
        Code = "CCSS:1.OA.A.1",
        Description = "Add and subtract within 20",
        Order = 1
    };

    private sealed record Ids(
        Guid School,
        Guid User,
        Guid Student,
        Guid Adoption,
        Guid Class,
        Guid Year,
        Guid Program,
        Guid Grade,
        Guid Subject,
        Guid Framework,
        Guid Topic)
    {
        public static Ids Create() => new(
            Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(),
            Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
    }
}
