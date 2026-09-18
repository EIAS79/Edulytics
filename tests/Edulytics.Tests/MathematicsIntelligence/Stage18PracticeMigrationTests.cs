using System.Text.Json;
using Edulytics.Core.Entities;
using Edulytics.Core.Enums;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Core.Practice;
using Edulytics.Services.Practice;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class Stage18PracticeMigrationTests
{
    [Fact]
    public void PracticeSkillContractsMatchStage17VerifiedLessonsAndStage18Manifest()
    {
        var root = FindRoot();
        using var stage17 = JsonDocument.Parse(File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Core/Mathematics/Curriculum/stage17-grade1-6-production-manifest.v1.json")));
        using var stage18 = JsonDocument.Parse(File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Core/Mathematics/Curriculum/stage18-practice-migration-manifest.v1.json")));

        var stage17ByCode = stage17.RootElement
            .GetProperty("entries")
            .EnumerateArray()
            .ToDictionary(
                x => x.GetProperty("lessonCode").GetString()!,
                x => x,
                StringComparer.Ordinal);

        var stage18ByCode = stage18.RootElement
            .GetProperty("entries")
            .EnumerateArray()
            .ToDictionary(
                x => x.GetProperty("lessonCode").GetString()!,
                x => x,
                StringComparer.Ordinal);

        Assert.Equal(14, stage17ByCode.Count);
        Assert.Equal(stage17ByCode.Keys.OrderBy(x => x), stage18ByCode.Keys.OrderBy(x => x));
        Assert.Equal(stage17ByCode.Count, Stage18PracticeSkillContracts.All.Count);

        foreach (var contract in Stage18PracticeSkillContracts.All)
        {
            Assert.True(stage17ByCode.TryGetValue(contract.LessonCode, out var stage17Row));
            Assert.True(stage18ByCode.TryGetValue(contract.LessonCode, out var stage18Row));
            Assert.Equal(stage17Row.GetProperty("skillId").GetString(), contract.SkillId);
            Assert.Equal(stage17Row.GetProperty("mechanic").GetString(), contract.Mechanic);
            Assert.Equal(stage18Row.GetProperty("skillId").GetString(), contract.SkillId);
            Assert.Equal(stage18Row.GetProperty("mechanic").GetString(), contract.Mechanic);

            var manifestFamilies = stage18Row
                .GetProperty("allowedQuestionFamilies")
                .EnumerateArray()
                .Select(x => x.GetString()!)
                .ToArray();

            Assert.NotEmpty(contract.AllowedQuestionFamilies);
            Assert.Equal(
                contract.AllowedQuestionFamilies.OrderBy(x => x),
                manifestFamilies.OrderBy(x => x));
        }
    }

    [Fact]
    public void EveryAllowedQuestionFamilyGeneratesSolverVerifiedPersistablePractice()
    {
        var engine = new Stage18SkillContractPracticeEngine();
        var generatedFamilies = new HashSet<string>(StringComparer.Ordinal);
        var expectedFamilies = Stage18PracticeSkillContracts.All
            .SelectMany(x => x.AllowedQuestionFamilies)
            .ToHashSet(StringComparer.Ordinal);

        var seed = 100;
        foreach (var contract in Stage18PracticeSkillContracts.All)
        {
            var items = engine.Generate(
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                contract,
                StudentPrivatePracticeDifficulty.Stretch,
                contract.AllowedQuestionFamilies.Count,
                seed++,
                [],
                Guid.NewGuid());

            Assert.Equal(contract.AllowedQuestionFamilies.Count, items.Count);
            foreach (var item in items)
            {
                Assert.Equal(Stage18PracticeSkillContracts.GenerationMethod, item.GenerationMethod);
                Assert.Contains(item.GenerationFamily!, contract.AllowedQuestionFamilies);
                Assert.True(Stage18SkillContractPracticeEngine.VerifyPersistedItem(contract, item));
                Assert.False(string.IsNullOrWhiteSpace(item.CorrectAnswer));
                Assert.False(string.IsNullOrWhiteSpace(item.Solution));
                Assert.Contains("\"solverVerified\":true", item.ValidationMetadataJson, StringComparison.Ordinal);
                Assert.Contains("\"broadFallbackUsed\":false", item.ValidationMetadataJson, StringComparison.Ordinal);
                generatedFamilies.Add(item.GenerationFamily!);
            }
        }

        Assert.Equal(expectedFamilies.OrderBy(x => x), generatedFamilies.OrderBy(x => x));
    }

    [Fact]
    public async Task ReadyVerifiedLessonUsesSkillContractPracticeInsteadOfContextualFallback()
    {
        var schoolId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var studentId = Guid.NewGuid();
        var adoptionId = Guid.NewGuid();
        var lessonId = Guid.NewGuid();
        var frameworkId = Guid.NewGuid();
        var gradeId = Guid.NewGuid();
        var programId = Guid.NewGuid();
        var subjectId = Guid.NewGuid();
        var yearId = Guid.NewGuid();
        var classId = Guid.NewGuid();

        var lesson = new CurriculumPedagogicalLesson
        {
            Id = lessonId,
            FrameworkVersionId = frameworkId,
            Code = "PED:CAMBRIDGE-INTL-MATH:S5:5F-2:BUILD",
            UnitKey = "FRACTIONS",
            UnitTitle = "Fractions",
            Title = "Find equivalent fractions: Build the Idea",
            LogicalLevelFrom = 5,
            LogicalLevelTo = 5,
            NativeLevel = "Stage 5",
            SortOrder = 1
        };

        var context = new StudentPrivatePracticeContext(
            new StudentProfile
            {
                Id = studentId,
                SchoolId = schoolId,
                UserId = userId,
                Status = AcademicStructureStatus.Active
            },
            new SchoolCurriculumAdoption
            {
                Id = adoptionId,
                SchoolId = schoolId,
                AcademicYearId = yearId,
                AcademicProgramId = programId,
                GradeLevelId = gradeId,
                SubjectId = subjectId,
                FrameworkVersionId = frameworkId,
                CurriculumLevelKey = "CAMBRIDGE-S5",
                CurriculumLogicalLevel = 5,
                CurriculumLevelLabel = "Stage 5",
                IsActive = true,
                IsPrimary = true
            },
            new ClassGroup
            {
                Id = classId,
                SchoolId = schoolId,
                AcademicYearId = yearId,
                AcademicProgramId = programId,
                GradeLevelId = gradeId,
                CurriculumAdoptionId = adoptionId,
                Name = "Class",
                Code = "C",
                Status = AcademicStructureStatus.Active
            },
            new StudentEnrollment
            {
                Id = Guid.NewGuid(),
                SchoolId = schoolId,
                StudentProfileId = studentId,
                ClassGroupId = classId,
                AcademicYearId = yearId
            },
            [],
            [lesson],
            [],
            [],
            []);

        var repository = new FakeRepository(context);
        var service = new StudentPrivatePracticeService(repository);

        var result = await service.GenerateAsync(
            userId,
            new GenerateStudentPrivatePracticeRequest(
                adoptionId,
                StudentPrivatePracticeScope.Lesson,
                lessonId,
                null,
                StudentPrivatePracticeDifficulty.MyLevel,
                4,
                404));

        Assert.True(result.Succeeded);
        Assert.NotNull(repository.SavedAttempt);
        Assert.Equal(4, repository.SavedItems.Count);
        Assert.Empty(repository.SavedOutcomes);

        foreach (var item in repository.SavedItems)
        {
            Assert.Equal(Stage18PracticeSkillContracts.GenerationMethod, item.GenerationMethod);
            Assert.NotEqual("CurriculumContextCheck", item.GenerationFamily);
            Assert.Contains("\"alignment\":\"skill-contract-verified\"", item.ValidationMetadataJson, StringComparison.Ordinal);
            Assert.Contains("\"broadFallbackUsed\":false", item.ValidationMetadataJson, StringComparison.Ordinal);
        }
    }

    [Fact]
    public void PracticeScoringUsesSharedMathematicsEquivalence()
    {
        Assert.True(PracticeService.AnswersMatch("1/2", "50%"));
        Assert.True(PracticeService.AnswersMatch("0,5", "1/2"));
        Assert.True(PracticeService.AnswersMatch("x = 7", "7"));
        Assert.False(PracticeService.AnswersMatch("2/3", "3/4"));
    }

    private static string FindRoot()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);
        while (directory is not null)
        {
            if (File.Exists(Path.Combine(directory.FullName, "Edulytics.sln")))
                return directory.FullName;
            directory = directory.Parent;
        }

        throw new DirectoryNotFoundException("Edulytics solution root not found.");
    }

    private sealed class FakeRepository(StudentPrivatePracticeContext context)
        : IStudentPrivatePracticeRepository
    {
        public PracticeAttempt? SavedAttempt { get; private set; }
        public List<AssessmentItem> SavedItems { get; } = [];
        public List<AssessmentItemOutcome> SavedOutcomes { get; } = [];

        public Task<IReadOnlyList<PrivatePracticeCurriculumOption>> ListCurriculaAsync(
            Guid studentUserId,
            CancellationToken cancellationToken = default) =>
            Task.FromResult<IReadOnlyList<PrivatePracticeCurriculumOption>>([]);

        public Task<StudentPrivatePracticeContext?> GetContextAsync(
            Guid studentUserId,
            Guid curriculumAdoptionId,
            CancellationToken cancellationToken = default) =>
            Task.FromResult<StudentPrivatePracticeContext?>(context);

        public Task AddGeneratedAttemptAsync(
            IReadOnlyList<AssessmentItem> items,
            IReadOnlyList<AssessmentItemOutcome> itemOutcomes,
            PracticeAttempt attempt,
            IReadOnlyList<PracticeAttemptItem> attemptItems,
            IReadOnlyList<StudentItemExposure> exposures,
            CancellationToken cancellationToken = default)
        {
            SavedAttempt = attempt;
            SavedItems.AddRange(items);
            SavedOutcomes.AddRange(itemOutcomes);
            return Task.CompletedTask;
        }

        public Task<IReadOnlyList<PrivatePracticeAttemptSummary>> ListPrivateAttemptsAsync(
            Guid studentUserId,
            CancellationToken cancellationToken = default) =>
            Task.FromResult<IReadOnlyList<PrivatePracticeAttemptSummary>>([]);
    }
}
