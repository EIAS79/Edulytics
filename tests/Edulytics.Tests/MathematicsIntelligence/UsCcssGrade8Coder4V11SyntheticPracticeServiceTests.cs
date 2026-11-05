using System.Text.Json;
using Edulytics.Core.Entities;
using Edulytics.Core.Enums;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Core.Practice;
using Edulytics.Services.Mathematics;
using Edulytics.Services.Practice;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Two synthetic Grade 8 school contexts exercise the actual private-practice
/// service and its approved U04:L05 family without any real database.
/// Mock repository identity denial is NOT authenticated persisted Stage D.
/// Shared U03:L12 membership generator belongs to Supervisor 2 PR #597.
/// </summary>
public sealed class UsCcssGrade8Coder4V11SyntheticPracticeServiceTests
{
    private const string LessonCode = "PED:US-CCSS-MATH:G8:U04:L05";
    private const string Family = "algebra.linear.ax_plus_b_equals_c";

    [Fact]
    public async Task TwoSeparateSyntheticSchoolsGenerateRealGrade8ExactQuestionsAndRetainSchoolScopedAttemptOwnership()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(LessonCode, out var contract));
        Assert.NotNull(contract);
        Assert.Contains(Family, contract.AllowedQuestionFamilies);
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(Family));

        var first = SyntheticSchool.Create();
        var second = SyntheticSchool.Create();
        Assert.NotEqual(first.School, second.School);
        Assert.NotEqual(first.User, second.User);
        Assert.NotEqual(first.Adoption, second.Adoption);
        Assert.NotEqual(first.Lesson, second.Lesson);

        var repo = new IdentityScopedFakeRepository(first, second);
        var service = new StudentPrivatePracticeService(repo);
        foreach (var (user,school,seed) in new[]
        {
            (first.User, first, 20261029),
            (second.User, second, 20261030)
        })
        {
            var result = await service.GenerateAsync(
                user,
                new GenerateStudentPrivatePracticeRequest(
                    school.Adoption, StudentPrivatePracticeScope.Lesson,
                    school.Lesson, null, StudentPrivatePracticeDifficulty.MyLevel, 4, seed));
            Assert.True(result.Succeeded, result.Error?.ToString());
            Assert.True(repo.Attempts.TryGetValue(user, out var attempts));
            var batch = Assert.Single(attempts);
            Assert.Equal(school.School, batch.Attempt.SchoolId);
            Assert.Equal(school.Student, batch.Attempt.StudentProfileId);
            Assert.Equal(school.Adoption, batch.Attempt.CurriculumAdoptionId);
            Assert.Equal(school.Lesson, batch.Attempt.CurriculumPedagogicalLessonId);
            Assert.True(batch.Attempt.IsPrivate);
            Assert.Equal(PracticeAttemptStatus.InProgress, batch.Attempt.Status);
            Assert.Equal(4, batch.Items.Length);
            Assert.All(batch.Items, item =>
            {
                Assert.Equal(school.School, item.SchoolId);
                Assert.Equal(school.Adoption, item.CurriculumAdoptionId);
                Assert.Equal(school.Lesson, item.CurriculumPedagogicalLessonId);
                Assert.Equal(user, item.CreatedByUserId);
                Assert.Equal(Family, item.GenerationFamily);
                Assert.True(!string.IsNullOrWhiteSpace(item.GenerationParametersJson));
                Assert.Contains("\"broadFallbackUsed\":false", item.ValidationMetadataJson);
                using var parameters = JsonDocument.Parse(item.GenerationParametersJson!);
                Assert.True(Stage18SkillContractPracticeEngine.VerifyPersistedItem(
                    contract!.ToLegacyStage18Contract(), item));
                var values = parameters.RootElement.GetProperty("parameters").EnumerateObject()
                    .Where(p => p.Value.ValueKind == JsonValueKind.Number)
                    .ToDictionary(p => p.Name, p => p.Value.GetInt32(), StringComparer.Ordinal);
                Assert.True(ExactSkillContractQuestionEngine.Verify(
                    Family, values, item.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(
                    Family, values, "not-an-equation-answer"));
                Assert.False(ExactSkillContractQuestionEngine.Verify(
                    Family, values, "999999999"));
            });
            Assert.All(batch.AttemptItems, i =>
            {
                Assert.Equal(school.School, i.SchoolId);
                Assert.Equal(batch.Attempt.Id, i.PracticeAttemptId);
                Assert.Contains(batch.Items, q => q.Id == i.AssessmentItemId);
            });
            Assert.All(batch.Exposures, e =>
            {
                Assert.Equal(school.School, e.SchoolId);
                Assert.Equal(school.Student, e.StudentProfileId);
            });
        }
        Assert.Equal(2, repo.Attempts.Count);
        var savedSchoolIds = repo.Attempts.Values
            .SelectMany(x => x.Select(y => y.Attempt.SchoolId))
            .Distinct().ToArray();
        Assert.Equal(2, savedSchoolIds.Length);
    }

    [Fact]
    public async Task SyntheticRepositoryIdentityGatesDenyCrossSchoolAdoptionAndUnknownUsersWithoutPersisting()
    {
        var first = SyntheticSchool.Create();
        var second = SyntheticSchool.Create();
        var repo = new IdentityScopedFakeRepository(first, second);
        var service = new StudentPrivatePracticeService(repo);

        foreach (var (user,adoption,lesson) in new[]
        {
            (first.User, second.Adoption, second.Lesson),
            (second.User, first.Adoption, first.Lesson),
            (Guid.NewGuid(), first.Adoption, first.Lesson),
            (first.User, Guid.NewGuid(), first.Lesson),
            (second.User, second.Adoption, first.Lesson)
        })
        {
            var result = await service.GenerateAsync(user,
                new GenerateStudentPrivatePracticeRequest(
                    adoption, StudentPrivatePracticeScope.Lesson, lesson,
                    null, StudentPrivatePracticeDifficulty.MyLevel, 4, 20261029));
            Assert.False(result.Succeeded);
        }
        Assert.Empty(repo.Attempts);
        // These denies rely on the stub's explicit repository scope checks.
        // Actual authenticated DB/HTTP tenant isolation must be independently tested.
    }

    private sealed record SyntheticSchool(
        Guid School, Guid User, Guid Student, Guid Adoption, Guid Lesson,
        StudentPrivatePracticeContext Context)
    {
        public static SyntheticSchool Create()
        {
            var school = Guid.NewGuid();
            var user = Guid.NewGuid();
            var student = Guid.NewGuid();
            var adoption = Guid.NewGuid();
            var lesson = Guid.NewGuid();
            var framework = Guid.NewGuid();
            var year = Guid.NewGuid();
            var program = Guid.NewGuid();
            var grade = Guid.NewGuid();
            var subject = Guid.NewGuid();
            var classId = Guid.NewGuid();

            var context = new StudentPrivatePracticeContext(
                new StudentProfile
                {
                    Id = student, SchoolId = school, UserId = user,
                    Status = AcademicStructureStatus.Active
                },
                new SchoolCurriculumAdoption
                {
                    Id = adoption, SchoolId = school, AcademicYearId = year,
                    AcademicProgramId = program, GradeLevelId = grade,
                    SubjectId = subject, FrameworkVersionId = framework,
                    CurriculumLevelKey = "US-CCSS-G8",
                    CurriculumLogicalLevel = 9,
                    CurriculumLevelLabel = "Grade 8",
                    IsActive = true, IsPrimary = true
                },
                new ClassGroup
                {
                    Id = classId, SchoolId = school, AcademicYearId = year,
                    AcademicProgramId = program, GradeLevelId = grade,
                    CurriculumAdoptionId = adoption, Name = "8A", Code = "8A",
                    Status = AcademicStructureStatus.Active
                },
                new StudentEnrollment
                {
                    Id = Guid.NewGuid(), SchoolId = school,
                    StudentProfileId = student, ClassGroupId = classId,
                    AcademicYearId = year
                },
                [],
                [
                    new CurriculumPedagogicalLesson
                    {
                        Id = lesson, FrameworkVersionId = framework,
                        Code = LessonCode, UnitKey = "U04",
                        UnitTitle = "Linear Equations and Linear Systems",
                        Title = "Solving Any Linear Equation",
                        LogicalLevelFrom = 9, LogicalLevelTo = 9,
                        NativeLevel = "Grade 8", SortOrder = 5
                    }
                ],
                [], [], []);
            return new SyntheticSchool(school,user,student,adoption,lesson,context);
        }
    }

    private sealed class IdentityScopedFakeRepository(params SyntheticSchool[] schools)
        : IStudentPrivatePracticeRepository
    {
        public Dictionary<Guid, List<SyntheticSavedBatch>> Attempts { get; } = [];

        public Task<IReadOnlyList<PrivatePracticeCurriculumOption>> ListCurriculaAsync(
            Guid studentUserId, CancellationToken cancellationToken = default) =>
            Task.FromResult<IReadOnlyList<PrivatePracticeCurriculumOption>>([]);

        public Task<StudentPrivatePracticeContext?> GetContextAsync(
            Guid studentUserId, Guid curriculumAdoptionId,
            CancellationToken cancellationToken = default) =>
            Task.FromResult(schools.SingleOrDefault(s =>
                s.User == studentUserId && s.Adoption == curriculumAdoptionId)?.Context);

        public Task AddGeneratedAttemptAsync(
            IReadOnlyList<AssessmentItem> items,
            IReadOnlyList<AssessmentItemOutcome> itemOutcomes,
            PracticeAttempt attempt,
            IReadOnlyList<PracticeAttemptItem> attemptItems,
            IReadOnlyList<StudentItemExposure> exposures,
            CancellationToken cancellationToken = default)
        {
            var school = schools.Single(s => s.Student == attempt.StudentProfileId);
            Assert.Equal(school.School, attempt.SchoolId);
            Assert.Equal(school.Adoption, attempt.CurriculumAdoptionId);
            Assert.Equal(school.Lesson, attempt.CurriculumPedagogicalLessonId);
            Assert.All(items, item => Assert.Equal(school.School, item.SchoolId));
            Assert.All(attemptItems, item => Assert.Equal(school.School, item.SchoolId));
            Assert.All(exposures, item => Assert.Equal(school.School, item.SchoolId));
            var batch = new SyntheticSavedBatch(attempt, [..items], [..attemptItems], [..exposures]);
            if (!Attempts.TryGetValue(school.User, out var batches))
                Attempts[school.User] = batches = [];
            batches.Add(batch);
            return Task.CompletedTask;
        }

        public Task<IReadOnlyList<PrivatePracticeAttemptSummary>> ListPrivateAttemptsAsync(
            Guid studentUserId, CancellationToken cancellationToken = default) =>
            Task.FromResult<IReadOnlyList<PrivatePracticeAttemptSummary>>([]);

        public Task<IReadOnlyList<PrivatePracticeEvidenceItem>> ListPrivateEvidenceAsync(
            Guid studentUserId, CancellationToken cancellationToken = default) =>
            Task.FromResult<IReadOnlyList<PrivatePracticeEvidenceItem>>([]);
    }

    private sealed record SyntheticSavedBatch(
        PracticeAttempt Attempt, AssessmentItem[] Items,
        PracticeAttemptItem[] AttemptItems, StudentItemExposure[] Exposures);
}
