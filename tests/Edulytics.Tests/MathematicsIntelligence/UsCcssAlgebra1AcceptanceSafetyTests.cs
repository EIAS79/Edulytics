using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;
using Edulytics.Core.Enums;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Core.Practice;
using Edulytics.Services.Practice;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Course-specific regression for safe Algebra 1 Practice routing.
/// These synthetic repository tests do NOT demonstrate HTTPS, database
/// persistence, teacher rubric scoring, or production school adoption.
/// </summary>
public sealed class UsCcssAlgebra1AcceptanceSafetyTests
{
    private const string Prefix="PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:";

    [Fact]
    public void All132AreSourceLinkedAndGenericProjectedContractsAreNotAcademicAcceptance()
    {
        var lessonCodes=RichLessonContentV2Registry.AllDocuments
            .SelectMany(d=>d.Lessons)
            .Where(x=>x.LessonCode.StartsWith(Prefix,StringComparison.Ordinal))
            .Select(x=>x.LessonCode).ToArray();
        Assert.Equal(132,lessonCodes.Length);
        Assert.Equal(132,lessonCodes.Distinct(StringComparer.Ordinal).Count());
        foreach(var code in lessonCodes)
        {
            var lesson=RichLessonContentV2Registry.Find(code,"en");
            Assert.NotNull(lesson);
            Assert.True(lesson.WorkedExamples.Count>=3);
        }
        // Runtime actually projects official outcome rules into READY_VERIFIED.
        // Such a registry flag alone does not show activity-level acceptance.
        var projected=lessonCodes
            .Select(code=>LessonPracticeCapabilityResolver.TryResolve(code,out var contract)
                ? contract : null)
            .Where(x=>x is not null).ToArray();
        Assert.NotEmpty(projected);
        Assert.All(projected,contract=>Assert.NotEmpty(contract!.AllowedQuestionFamilies));
    }

    [Theory]
    [InlineData("U01:L01")]
    [InlineData("U03:L07")]
    [InlineData("U04:L15")]
    [InlineData("U05:L16")]
    [InlineData("U06:L15")]
    [InlineData("U07:L24")]
    public async Task UnverifiedExactLessonCannotCreateGenericPrivatePractice(string suffix)
    {
        var school=Guid.NewGuid();
        var student=Guid.NewGuid();
        var adoption=Guid.NewGuid();
        var repo=new SyntheticRepository();
        repo.SetStudent(student,adoption,CreateContext(school,student,adoption,Prefix+suffix));
        var result=await new StudentPrivatePracticeService(repo).GenerateAsync(
            student,new GenerateStudentPrivatePracticeRequest(
                adoption,StudentPrivatePracticeScope.Lesson,
                repo.LessonId,null,StudentPrivatePracticeDifficulty.AtClassLevel,1,29));
        Assert.Equal(StudentPrivatePracticeError.NoSupportedOutcomes,result.Error);
        Assert.Equal(0,repo.SaveCount);
    }

    [Theory]
    [InlineData(StudentPrivatePracticeScope.Unit)]
    [InlineData(StudentPrivatePracticeScope.WholeCurriculum)]
    [InlineData(StudentPrivatePracticeScope.WeakAreas)]
    public async Task UnverifiedAlgebra1BroaderScopesAlsoCannotMasqueradeAsExactPractice(
        StudentPrivatePracticeScope scope)
    {
        var student=Guid.NewGuid();
        var adoption=Guid.NewGuid();
        var repo=new SyntheticRepository();
        repo.SetStudent(student,adoption,CreateContext(Guid.NewGuid(),student,adoption,Prefix+"U02:L01"));
        var result=await new StudentPrivatePracticeService(repo).GenerateAsync(
            student,new GenerateStudentPrivatePracticeRequest(
                adoption,scope,null,scope==StudentPrivatePracticeScope.Unit?"U02":null,
                StudentPrivatePracticeDifficulty.AtClassLevel,1,30));
        Assert.Equal(StudentPrivatePracticeError.NoSupportedOutcomes,result.Error);
        Assert.Equal(0,repo.SaveCount);
    }

    [Fact]
    public async Task SyntheticWrongSchoolStudentAndAdoptionCannotSelectAuthorizedContextOrWrite()
    {
        var schoolA=Guid.NewGuid();
        var studentA=Guid.NewGuid();
        var adoptionA=Guid.NewGuid();
        var schoolB=Guid.NewGuid();
        var studentB=Guid.NewGuid();
        var adoptionB=Guid.NewGuid();
        var repo=new SyntheticRepository();
        repo.SetStudent(studentA,adoptionA,CreateContext(schoolA,studentA,adoptionA,Prefix+"U01:L01"));
        repo.SetStudent(studentB,adoptionB,CreateContext(schoolB,studentB,adoptionB,Prefix+"U01:L01"));
        var service=new StudentPrivatePracticeService(repo);
        foreach(var (actor,adoption) in new[]{
            (studentA,adoptionB),(studentB,adoptionA),(Guid.NewGuid(),adoptionA)})
        {
            var result=await service.GenerateAsync(actor,new GenerateStudentPrivatePracticeRequest(
                adoption,StudentPrivatePracticeScope.Lesson,repo.LessonId,null,
                StudentPrivatePracticeDifficulty.AtClassLevel,1,101));
            Assert.Equal(StudentPrivatePracticeError.CurriculumNotAvailable,result.Error);
        }
        Assert.Equal(0,repo.SaveCount);
    }

    [Fact]
    public void Unit1Lesson1RequiresGenuineFifteenClassmateSurveyOrHonestNoncompletion()
    {
        var lesson=RichLessonContentV2Registry.Find(Prefix+"U01:L01","en");
        Assert.NotNull(lesson);
        var text=string.Join(" ",lesson.ExplanationParagraphs);
        Assert.Contains("15 classmates",text,StringComparison.Ordinal);
        Assert.Contains("four",text,StringComparison.OrdinalIgnoreCase);
        Assert.Contains("preserve",text,StringComparison.OrdinalIgnoreCase);
        Assert.Contains("not completed",text,StringComparison.OrdinalIgnoreCase);
        Assert.Contains("/HS/students/1/1/1/index.html",text,StringComparison.Ordinal);
    }

    [Fact]
    public void CulminatingQuadraticsUsesOriginalLineParabolaModelWithoutCollisionClaim()
    {
        var lesson=RichLessonContentV2Registry.Find(Prefix+"U07:L24","en");
        Assert.NotNull(lesson);
        var text=string.Join(" ",lesson.ExplanationParagraphs);
        Assert.Contains("parabola",text,StringComparison.OrdinalIgnoreCase);
        Assert.Contains("line",text,StringComparison.OrdinalIgnoreCase);
        Assert.Contains("horizontal",text,StringComparison.OrdinalIgnoreCase);
        Assert.Contains("teacher",text,StringComparison.OrdinalIgnoreCase);
    }

    private static StudentPrivatePracticeContext CreateContext(
        Guid schoolId,Guid studentId,Guid adoptionId,string lessonCode)
    {
        var year=Guid.NewGuid();
        var program=Guid.NewGuid();
        var grade=Guid.NewGuid();
        var framework=Guid.NewGuid();
        var classId=Guid.NewGuid();
        var profileId=Guid.NewGuid();
        return new StudentPrivatePracticeContext(
            new StudentProfile
            {
                Id=profileId,SchoolId=schoolId,UserId=studentId,
                Status=AcademicStructureStatus.Active
            },
            new SchoolCurriculumAdoption
            {
                Id=adoptionId,SchoolId=schoolId,AcademicYearId=year,
                AcademicProgramId=program,GradeLevelId=grade,
                SubjectId=Guid.NewGuid(),FrameworkVersionId=framework,
                CurriculumLevelKey="CCSS-HS-TRAD-ALG1",CurriculumLogicalLevel=10,
                CurriculumLevelLabel="Algebra 1",IsActive=true,IsPrimary=true
            },
            new ClassGroup
            {
                Id=classId,SchoolId=schoolId,AcademicYearId=year,
                AcademicProgramId=program,GradeLevelId=grade,
                CurriculumAdoptionId=adoptionId,Name="Algebra Class",
                Code="ALG1-A",Status=AcademicStructureStatus.Active
            },
            new StudentEnrollment
            {
                Id=Guid.NewGuid(),SchoolId=schoolId,
                StudentProfileId=profileId,ClassGroupId=classId,
                AcademicYearId=year
            },
            [],
            [new CurriculumPedagogicalLesson
            {
                Id=Guid.NewGuid(),FrameworkVersionId=framework,
                Code=lessonCode,UnitKey=lessonCode.Split(':')[6],
                UnitTitle="Mathematics",Title="Algebra 1",
                LogicalLevelFrom=10,LogicalLevelTo=13,
                NativeLevel="High School",SortOrder=1,
                Pathway="Traditional"
            }],
            [],[],[]);
    }

    private sealed class SyntheticRepository:IStudentPrivatePracticeRepository
    {
        private readonly Dictionary<(Guid Student,Guid Adoption),StudentPrivatePracticeContext>
            _contexts=new();
        public Guid LessonId {get;private set;}
        public int SaveCount {get;private set;}

        public void SetStudent(Guid student,Guid adoption,StudentPrivatePracticeContext context)
        {
            _contexts.Add((student,adoption),context);
            LessonId=context.Lessons[0].Id;
        }

        public Task<IReadOnlyList<PrivatePracticeCurriculumOption>> ListCurriculaAsync(
            Guid userId,CancellationToken ct=default)=>
            Task.FromResult<IReadOnlyList<PrivatePracticeCurriculumOption>>([]);

        public Task<StudentPrivatePracticeContext?> GetContextAsync(
            Guid studentId,Guid adoption,CancellationToken ct=default)=>
            Task.FromResult(_contexts.TryGetValue((studentId,adoption),out var value)?value:null);

        public Task<IReadOnlyList<PrivatePracticeAttemptSummary>> ListPrivateAttemptsAsync(
            Guid userId,CancellationToken ct=default)=>
            Task.FromResult<IReadOnlyList<PrivatePracticeAttemptSummary>>([]);

        public Task<IReadOnlyList<PrivatePracticeEvidenceItem>> ListPrivateEvidenceAsync(
            Guid userId,CancellationToken ct=default)=>
            Task.FromResult<IReadOnlyList<PrivatePracticeEvidenceItem>>([]);

        public Task AddGeneratedAttemptAsync(
            IReadOnlyList<AssessmentItem> items,
            IReadOnlyList<AssessmentItemOutcome> links,
            PracticeAttempt attempt,
            IReadOnlyList<PracticeAttemptItem> attemptItems,
            IReadOnlyList<StudentItemExposure> exposures,
            CancellationToken ct=default)
        {
            SaveCount++;
            return Task.CompletedTask;
        }
    }
}
