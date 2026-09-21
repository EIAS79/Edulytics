using System.Text.Json;
using Edulytics.Core.Entities;
using Edulytics.Core.Enums;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Core.Practice;
using Edulytics.Services.Mathematics;
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
    [InlineData(31)]
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
    public async Task Recognizable_mathematics_outcomes_use_contextual_generation_instead_of_failing_closed()
    {
        var ids = Ids.Create();
        var repo = new FakeRepository
        {
            Context = BuildContext(
                ids,
                [Outcome(ids, "GEO.1", "Identify a geometric shape and reason about its area.", 1)],
                [])
        };
        var result = await new StudentPrivatePracticeService(repo).GenerateAsync(
            ids.User,
            new GenerateStudentPrivatePracticeRequest(ids.Adoption, StudentPrivatePracticeScope.WholeCurriculum,
                null, null, StudentPrivatePracticeDifficulty.AtClassLevel, 1, 7));

        Assert.True(result.Succeeded);
        Assert.Null(result.Error);
        Assert.NotNull(repo.SavedAttempt);
        Assert.Single(repo.SavedItems);
        Assert.Equal("CurriculumContextCheck", repo.SavedItems[0].GenerationFamily);
        Assert.Contains("student-private", repo.SavedItems[0].ValidationMetadataJson, StringComparison.Ordinal);
        Assert.False(string.IsNullOrWhiteSpace(repo.SavedItems[0].CorrectAnswer));
        Assert.False(string.IsNullOrWhiteSpace(repo.SavedItems[0].Solution));
    }

    [Fact]
    public async Task Non_mathematics_outcomes_still_fail_closed()
    {
        var ids = Ids.Create();
        var repo = new FakeRepository
        {
            Context = BuildContext(
                ids,
                [Outcome(ids, "HIST.1", "Describe the historical context of a source.", 1)],
                [])
        };
        var result = await new StudentPrivatePracticeService(repo).GenerateAsync(
            ids.User,
            new GenerateStudentPrivatePracticeRequest(ids.Adoption, StudentPrivatePracticeScope.WholeCurriculum,
                null, null, StudentPrivatePracticeDifficulty.AtClassLevel, 1, 7));

        Assert.Equal(StudentPrivatePracticeError.NoSupportedOutcomes, result.Error);
        Assert.Null(repo.SavedAttempt);
        Assert.Empty(repo.SavedItems);
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
    public async Task Lesson_practice_generates_eight_progressive_questions_and_rolls_over_old_exposure()
    {
        const string lessonCode =
            "PED:CAMBRIDGE-INTL-MATH:S6:6NPV-1:APPLY";

        Assert.True(
            LessonPracticeContractRegistry.TryResolve(
                lessonCode,
                out var contract));
        Assert.NotNull(contract);
        Assert.Contains(
            "supporting.indices.power_or_root",
            contract!.AllowedQuestionFamilies);

        var ids = Ids.Create();
        var lessonId = Guid.NewGuid();
        var lesson = Lesson(
            ids,
            lessonId,
            "S6-NPV",
            "Number and Place Value",
            lessonCode,
            "Powers of 10: Reason and Apply",
            1);

        // Standard difficulty currently has a deliberately bounded parameter
        // range. Saturate it to reproduce the Production condition where
        // historical exposure must not block a new lesson Practice attempt.
        var saturatedStandardPool =
            new ExactSkillContractQuestionEngine().Generate(
                "stage18",
                lessonCode,
                contract.AllowedQuestionFamilies,
                ExactSkillQuestionDifficulty.Standard,
                16,
                42017,
                []);

        Assert.Equal(16, saturatedStandardPool.Count);

        var historicalExposures = saturatedStandardPool
            .Select(question => new StudentItemExposure
            {
                Id = Guid.NewGuid(),
                SchoolId = ids.School,
                StudentProfileId = ids.Student,
                AssessmentItemId = Guid.NewGuid(),
                ExposureFingerprint = question.ExposureFingerprint,
                ExposedAtUtc = DateTime.UtcNow.AddMinutes(-10)
            })
            .ToArray();

        var context = BuildContext(
            ids,
            [],
            [lesson]) with
        {
            Exposures = historicalExposures
        };

        var repo = new FakeRepository { Context = context };
        var result = await new StudentPrivatePracticeService(repo)
            .GenerateAsync(
                ids.User,
                new GenerateStudentPrivatePracticeRequest(
                    ids.Adoption,
                    StudentPrivatePracticeScope.Lesson,
                    lessonId,
                    null,
                    StudentPrivatePracticeDifficulty.MyLevel,
                    8,
                    73031,
                    UseLessonDifficultyProgression: true));

        Assert.True(result.Succeeded);
        Assert.Null(result.Error);
        Assert.Equal(8, repo.SavedItems.Count);
        Assert.Equal(
            8,
            repo.SavedItems
                .Select(item => item.ExposureFingerprint)
                .Distinct(StringComparer.Ordinal)
                .Count());

        var expectedDifficulty = new[]
        {
            "Standard", "Standard", "Standard",
            "Stretch", "Stretch", "Stretch",
            "Challenge", "Challenge"
        };

        for (var index = 0; index < repo.SavedItems.Count; index++)
        {
            Assert.False(
                string.IsNullOrWhiteSpace(
                    repo.SavedItems[index].ValidationMetadataJson));

            using var metadata = JsonDocument.Parse(
                repo.SavedItems[index].ValidationMetadataJson!);

            Assert.Equal(
                expectedDifficulty[index],
                metadata.RootElement
                    .GetProperty("difficulty")
                    .GetString());

            Assert.Equal(
                index + 1,
                metadata.RootElement
                    .GetProperty("progressionIndex")
                    .GetInt32());
        }

        Assert.All(
            repo.SavedItems,
            item => Assert.True(
                Stage18SkillContractPracticeEngine.VerifyPersistedItem(
                    contract.ToLegacyStage18Contract(),
                    item)));
    }

    [Fact]
    public async Task Lesson_progression_contract_rejects_any_count_other_than_eight()
    {
        var ids = Ids.Create();
        var lessonId = Guid.NewGuid();
        var repo = new FakeRepository
        {
            Context = BuildContext(
                ids,
                [],
                [
                    Lesson(
                        ids,
                        lessonId,
                        "S6-NPV",
                        "Number and Place Value",
                        "PED:CAMBRIDGE-INTL-MATH:S6:6NPV-1:APPLY",
                        "Powers of 10: Reason and Apply",
                        1)
                ])
        };

        var result = await new StudentPrivatePracticeService(repo)
            .GenerateAsync(
                ids.User,
                new GenerateStudentPrivatePracticeRequest(
                    ids.Adoption,
                    StudentPrivatePracticeScope.Lesson,
                    lessonId,
                    null,
                    StudentPrivatePracticeDifficulty.MyLevel,
                    10,
                    7,
                    UseLessonDifficultyProgression: true));

        Assert.Equal(
            StudentPrivatePracticeError.InvalidQuestionCount,
            result.Error);
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
