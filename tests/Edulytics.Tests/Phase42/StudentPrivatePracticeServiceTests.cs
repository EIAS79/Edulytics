using Edulytics.Core.Entities;
using Edulytics.Core.Enums;
using Edulytics.Core.Practice;
using Edulytics.Services.Practice;
using Xunit;

namespace Edulytics.Tests.Phase42;

public sealed class StudentPrivatePracticeServiceTests
{
    [Fact]
    public async Task Workspace_projects_curricula_lessons_units_and_history()
    {
        var ids = Ids.Create();
        var repo = new FakeRepository
        {
            Curricula = [new PrivatePracticeCurriculumOption(ids.Adoption, ids.Class, ids.Year, "Grade 1", "1A")],
            Context = BuildContext(ids,
                [Outcome(ids, "CCSS:1.OA.A.1", "Add whole numbers", 1)],
                [
                    Lesson(ids, Guid.NewGuid(), "U1", "Number", "L1", "Addition", 1),
                    Lesson(ids, Guid.NewGuid(), "U1", "Number", "L2", "Subtraction", 2),
                    Lesson(ids, Guid.NewGuid(), "U2", "Fractions", "L3", "Fractions", 3)
                ]),
            Attempts = [new PrivatePracticeAttemptSummary(Guid.NewGuid(), ids.Adoption, null, PracticeAttemptStatus.Submitted, DateTime.UtcNow, DateTime.UtcNow, 4, 5, 80)]
        };

        var service = new StudentPrivatePracticeService(repo);
        var result = await service.GetWorkspaceAsync(ids.User, ids.Adoption);

        Assert.Single(result.Curricula);
        Assert.Equal(3, result.Lessons.Count);
        Assert.Equal(2, result.UnitKeys.Count);
        Assert.Single(result.Attempts);
        Assert.Equal(ids.Adoption, result.SelectedCurriculumAdoptionId);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(21)]
    public async Task Generate_rejects_invalid_question_count_before_repository_lookup(int count)
    {
        var repo = new FakeRepository();
        var service = new StudentPrivatePracticeService(repo);
        var result = await service.GenerateAsync(Guid.NewGuid(), new GenerateStudentPrivatePracticeRequest(
            Guid.NewGuid(), StudentPrivatePracticeScope.WholeCurriculum, null, null,
            StudentPrivatePracticeDifficulty.MyLevel, count, 1));

        Assert.Equal(StudentPrivatePracticeError.InvalidQuestionCount, result.Error);
        Assert.Equal(0, repo.ContextCalls);
    }

    [Fact]
    public async Task Generate_fails_closed_for_unavailable_curriculum()
    {
        var repo = new FakeRepository();
        var result = await new StudentPrivatePracticeService(repo).GenerateAsync(
            Guid.NewGuid(),
            new GenerateStudentPrivatePracticeRequest(Guid.NewGuid(), StudentPrivatePracticeScope.WholeCurriculum,
                null, null, StudentPrivatePracticeDifficulty.MyLevel, 1, 1));

        Assert.Equal(StudentPrivatePracticeError.CurriculumNotAvailable, result.Error);
    }

    [Fact]
    public async Task Lesson_scope_requires_a_lesson()
    {
        var ids = Ids.Create();
        var repo = new FakeRepository { Context = BuildContext(ids, [Outcome(ids, "CCSS:1.OA.A.1", "Add whole numbers", 1)], []) };
        var result = await new StudentPrivatePracticeService(repo).GenerateAsync(
            ids.User,
            new GenerateStudentPrivatePracticeRequest(ids.Adoption, StudentPrivatePracticeScope.Lesson,
                null, null, StudentPrivatePracticeDifficulty.MyLevel, 1, 1));

        Assert.Equal(StudentPrivatePracticeError.InvalidScope, result.Error);
    }

    [Fact]
    public async Task Unsupported_outcomes_fail_closed_instead_of_generating_untrusted_questions()
    {
        var ids = Ids.Create();
        var repo = new FakeRepository { Context = BuildContext(ids, [Outcome(ids, "GEO.1", "Identify a shape", 1)], []) };
        var result = await new StudentPrivatePracticeService(repo).GenerateAsync(
            ids.User,
            new GenerateStudentPrivatePracticeRequest(ids.Adoption, StudentPrivatePracticeScope.WholeCurriculum,
                null, null, StudentPrivatePracticeDifficulty.AtClassLevel, 1, 7));

        Assert.Equal(StudentPrivatePracticeError.NoSupportedOutcomes, result.Error);
        Assert.Null(repo.SavedAttempt);
    }

    [Fact]
    public async Task Whole_curriculum_generation_creates_private_native_attempt()
    {
        var ids = Ids.Create();
        var outcome = Outcome(ids, "CCSS:1.OA.A.1", "Add whole numbers", 1);
        var repo = new FakeRepository { Context = BuildContext(ids, [outcome], []) };
        var result = await new StudentPrivatePracticeService(repo).GenerateAsync(
            ids.User,
            new GenerateStudentPrivatePracticeRequest(ids.Adoption, StudentPrivatePracticeScope.WholeCurriculum,
                null, null, StudentPrivatePracticeDifficulty.AtClassLevel, 1, 123));

        Assert.True(result.Succeeded);
        Assert.NotNull(repo.SavedAttempt);
        Assert.True(repo.SavedAttempt!.IsPrivate);
        Assert.Equal(PracticeAttemptStatus.InProgress, repo.SavedAttempt.Status);
        Assert.Single(repo.SavedItems);
        Assert.Single(repo.SavedOutcomes);
        Assert.Equal(outcome.Id, repo.SavedOutcomes[0].LearningOutcomeId);
        Assert.Contains("student-private", repo.SavedItems[0].ValidationMetadataJson, StringComparison.Ordinal);
        Assert.Single(repo.SavedExposures);
    }

    [Fact]
    public async Task Weak_area_generation_prefers_low_official_mastery()
    {
        var ids = Ids.Create();
        var weak = Outcome(ids, "CCSS:1.OA.A.1", "Add whole numbers", 1);
        var strong = Outcome(ids, "CCSS:1.OA.A.2", "Add whole numbers", 2);
        var context = BuildContext(ids, [weak, strong], []);
        context = context with
        {
            OfficialMasteries =
            [
                Mastery(ids, weak.Id, 25m),
                Mastery(ids, strong.Id, 95m)
            ]
        };
        var repo = new FakeRepository { Context = context };
        var result = await new StudentPrivatePracticeService(repo).GenerateAsync(
            ids.User,
            new GenerateStudentPrivatePracticeRequest(ids.Adoption, StudentPrivatePracticeScope.WeakAreas,
                null, null, StudentPrivatePracticeDifficulty.MyLevel, 1, 456));

        Assert.True(result.Succeeded);
        Assert.Single(repo.SavedOutcomes);
        Assert.Equal(weak.Id, repo.SavedOutcomes[0].LearningOutcomeId);
    }

    private static StudentPrivatePracticeContext BuildContext(
        Ids ids,
        IReadOnlyList<LearningOutcome> outcomes,
        IReadOnlyList<CurriculumPedagogicalLesson> lessons) =>
        new(
            new StudentProfile { Id = ids.Student, SchoolId = ids.School, UserId = ids.User, Status = AcademicStructureStatus.Active },
            new SchoolCurriculumAdoption
            {
                Id = ids.Adoption, SchoolId = ids.School, AcademicYearId = ids.Year,
                AcademicProgramId = ids.Program, GradeLevelId = ids.Grade, SubjectId = ids.Subject,
                FrameworkVersionId = ids.Framework, CurriculumLevelKey = "CCSS-G1",
                CurriculumLogicalLevel = 1, CurriculumLevelLabel = "Grade 1", IsActive = true, IsPrimary = true
            },
            new ClassGroup
            {
                Id = ids.Class, SchoolId = ids.School, AcademicYearId = ids.Year,
                AcademicProgramId = ids.Program, GradeLevelId = ids.Grade,
                CurriculumAdoptionId = ids.Adoption, Name = "1A", Code = "1A", Status = AcademicStructureStatus.Active
            },
            new StudentEnrollment
            {
                Id = Guid.NewGuid(), SchoolId = ids.School, StudentProfileId = ids.Student,
                ClassGroupId = ids.Class, AcademicYearId = ids.Year
            },
            outcomes,
            lessons,
            [],
            [],
            []);

    private static LearningOutcome Outcome(Ids ids, string code, string description, int order) => new()
    {
        Id = Guid.NewGuid(), SchoolId = ids.School, AcademicProgramId = ids.Program,
        FrameworkVersionId = ids.Framework, SubjectId = ids.Subject, GradeLevelId = ids.Grade,
        CurriculumAdoptionId = ids.Adoption, TopicId = ids.Topic, Code = code,
        Description = description, Order = order
    };

    private static CurriculumPedagogicalLesson Lesson(Ids ids, Guid id, string unitKey, string unitTitle, string code, string title, int order) => new()
    {
        Id = id, FrameworkVersionId = ids.Framework, Code = code, UnitKey = unitKey,
        UnitTitle = unitTitle, Title = title, LogicalLevelFrom = 1, LogicalLevelTo = 1,
        NativeLevel = "Grade 1", SortOrder = order
    };

    private static StudentOutcomeMastery Mastery(Ids ids, Guid outcomeId, decimal percentage) => new()
    {
        Id = Guid.NewGuid(), SchoolId = ids.School, AcademicYearId = ids.Year,
        ClassGroupId = ids.Class, SubjectId = ids.Subject, StudentProfileId = ids.Student,
        LearningOutcomeId = outcomeId, MasteryPercentage = percentage
    };

    private sealed class FakeRepository : IStudentPrivatePracticeRepository
    {
        public IReadOnlyList<PrivatePracticeCurriculumOption> Curricula { get; init; } = [];
        public StudentPrivatePracticeContext? Context { get; init; }
        public IReadOnlyList<PrivatePracticeAttemptSummary> Attempts { get; init; } = [];
        public int ContextCalls { get; private set; }
        public PracticeAttempt? SavedAttempt { get; private set; }
        public List<AssessmentItem> SavedItems { get; } = [];
        public List<AssessmentItemOutcome> SavedOutcomes { get; } = [];
        public List<StudentItemExposure> SavedExposures { get; } = [];

        public Task<IReadOnlyList<PrivatePracticeCurriculumOption>> ListCurriculaAsync(Guid studentUserId, CancellationToken cancellationToken = default) => Task.FromResult(Curricula);
        public Task<StudentPrivatePracticeContext?> GetContextAsync(Guid studentUserId, Guid curriculumAdoptionId, CancellationToken cancellationToken = default)
        {
            ContextCalls++;
            return Task.FromResult(Context);
        }
        public Task<IReadOnlyList<PrivatePracticeAttemptSummary>> ListPrivateAttemptsAsync(Guid studentUserId, CancellationToken cancellationToken = default) => Task.FromResult(Attempts);
        public Task AddGeneratedAttemptAsync(IReadOnlyList<AssessmentItem> items, IReadOnlyList<AssessmentItemOutcome> itemOutcomes, PracticeAttempt attempt, IReadOnlyList<PracticeAttemptItem> attemptItems, IReadOnlyList<StudentItemExposure> exposures, CancellationToken cancellationToken = default)
        {
            SavedAttempt = attempt;
            SavedItems.AddRange(items);
            SavedOutcomes.AddRange(itemOutcomes);
            SavedExposures.AddRange(exposures);
            return Task.CompletedTask;
        }
    }

    private sealed record Ids(Guid School, Guid User, Guid Student, Guid Adoption, Guid Class, Guid Year, Guid Program, Guid Grade, Guid Subject, Guid Framework, Guid Topic)
    {
        public static Ids Create() => new(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
    }
}
