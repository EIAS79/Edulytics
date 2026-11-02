
using Edulytics.Core.Entities;
using Edulytics.Core.Enums;
using Edulytics.Data.Contexts;
using Edulytics.Data.Repositories;
using Edulytics.Services.Practice;
using Microsoft.EntityFrameworkCore;

namespace Edulytics.Tests.Phase30;

public sealed class Phase30PracticeEngineTests
{
    [Fact]
    public void NumericAnswerComparison_IsCultureTolerantAndDeterministic()
    {
        Assert.True(PracticeService.AnswersMatch(" 2,50 ", "2.5"));
        Assert.True(PracticeService.AnswersMatch("X = 7", "x = 7"));
        Assert.False(PracticeService.AnswersMatch("7", "8"));
    }

    [Fact]
    public async Task PracticeAttempt_RecordsExposureResponseAndOutcomeEvidence_WithoutOfficialGrade()
    {
        await using var context = CreateContext();
        var fixture = await SeedAsync(context);
        var service = new PracticeService(new PracticeRepository(context));

        var start = await service.StartAsync(
            fixture.StudentUserId,
            fixture.AdoptionId,
            [fixture.ItemId]);

        Assert.True(start.Succeeded);
        var attemptId = Assert.IsType<Guid>(start.EntityId);

        var details = await service.GetAttemptAsync(fixture.StudentUserId, attemptId);
        Assert.NotNull(details.Value);
        var question = Assert.Single(details.Value!.Questions);

        var feedback = await service.AnswerAsync(
            fixture.StudentUserId,
            attemptId,
            question.AttemptItemId,
            "6");

        Assert.NotNull(feedback.Value);
        Assert.True(feedback.Value!.IsCorrect);
        Assert.Equal("Divide both sides by 3.", feedback.Value.Solution);

        var submitted = await service.SubmitAsync(fixture.StudentUserId, attemptId);
        Assert.NotNull(submitted.Value);
        Assert.Equal(PracticeAttemptStatus.Submitted, submitted.Value!.Status);
        Assert.Equal(100m, submitted.Value.Percentage);

        var evidence = await context.LearningEvidence.SingleAsync();
        Assert.Equal(fixture.OutcomeId, evidence.LearningOutcomeId);
        Assert.Equal(LearningEvidenceType.Practice, evidence.EvidenceType);
        Assert.Equal(AssessmentItemDifficulty.Medium, evidence.Difficulty);
        Assert.True(evidence.IsCorrect);

        var exposure = await context.StudentItemExposures.SingleAsync();
        Assert.Equal("linear-equation-family:a1:b0:c6", exposure.ExposureFingerprint);

        Assert.Empty(await context.AssessmentResults.ToListAsync());
    }

    [Fact]
    public async Task PracticeAttempt_RejectsItemsFromAnotherCurriculumAdoption()
    {
        await using var context = CreateContext();
        var fixture = await SeedAsync(context);
        var foreignItem = await AddItemAsync(
            context,
            fixture.SchoolId,
            Guid.NewGuid(),
            fixture.OutcomeId,
            "foreign-fingerprint");

        var service = new PracticeService(new PracticeRepository(context));
        var result = await service.StartAsync(
            fixture.StudentUserId,
            fixture.AdoptionId,
            [foreignItem.Id]);

        Assert.False(result.Succeeded);
        Assert.Equal(PracticeErrorCode.ItemScopeMismatch, result.Error);
        Assert.Empty(await context.PracticeAttempts.ToListAsync());
    }

    [Fact]
    public async Task Student_CannotReadAnotherStudentsPracticeAttempt()
    {
        await using var context = CreateContext();
        var fixture = await SeedAsync(context);
        var service = new PracticeService(new PracticeRepository(context));
        var start = await service.StartAsync(fixture.StudentUserId, fixture.AdoptionId, [fixture.ItemId]);
        var attemptId = Assert.IsType<Guid>(start.EntityId);

        var otherUser = Guid.NewGuid();
        var otherStudent = NewStudent(fixture.SchoolId, otherUser, "S-OTHER");
        context.StudentProfiles.Add(otherStudent);
        context.StudentEnrollments.Add(new StudentEnrollment
        {
            Id = Guid.NewGuid(),
            SchoolId = fixture.SchoolId,
            StudentProfileId = otherStudent.Id,
            ClassGroupId = fixture.ClassGroupId,
            AcademicYearId = fixture.AcademicYearId,
            EnrolledAtUtc = DateTime.UtcNow
        });
        await context.SaveChangesAsync();

        var result = await service.GetAttemptAsync(otherUser, attemptId);
        Assert.Null(result.Value);
        Assert.Equal(PracticeErrorCode.AccessDenied, result.Error);
    }

    [Fact]
    public async Task SubmittedAttempt_IsIdempotentAndDoesNotDuplicateEvidence()
    {
        await using var context = CreateContext();
        var fixture = await SeedAsync(context);
        var service = new PracticeService(new PracticeRepository(context));
        var start = await service.StartAsync(fixture.StudentUserId, fixture.AdoptionId, [fixture.ItemId]);
        var attemptId = Assert.IsType<Guid>(start.EntityId);
        var details = (await service.GetAttemptAsync(fixture.StudentUserId, attemptId)).Value!;
        await service.AnswerAsync(fixture.StudentUserId, attemptId, details.Questions[0].AttemptItemId, "6");

        var first = await service.SubmitAsync(fixture.StudentUserId, attemptId);
        var second = await service.SubmitAsync(fixture.StudentUserId, attemptId);

        Assert.NotNull(first.Value);
        Assert.NotNull(second.Value);
        Assert.Equal(1, await context.LearningEvidence.CountAsync());
    }

    [Fact]
    public async Task AssessmentItem_PreservesReconstructionAndValidationMetadata()
    {
        await using var context = CreateContext();
        var fixture = await SeedAsync(context);
        var item = await context.AssessmentItems.SingleAsync(x => x.Id == fixture.ItemId);

        Assert.Equal(AssessmentItemSource.SystemGenerated, item.Source);
        Assert.Equal(AssessmentItemType.Numeric, item.ItemType);
        Assert.Equal(AssessmentItemDifficulty.Medium, item.Difficulty);
        Assert.Equal("3x = 18. Find x.", item.Prompt);
        Assert.Equal("6", item.CorrectAnswer);
        Assert.Equal("Divide both sides by 3.", item.Solution);
        Assert.Equal("native-template-v1", item.GenerationMethod);
        Assert.Equal("linear-equation-family", item.GenerationFamily);
        Assert.Contains("coefficient", item.GenerationParametersJson);
        Assert.Contains("validated", item.ValidationMetadataJson);
    }

    private static EdulyticsDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<EdulyticsDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString("N"))
            .Options;
        return new EdulyticsDbContext(options);
    }

    private static async Task<Fixture> SeedAsync(EdulyticsDbContext context)
    {
        var schoolId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var student = NewStudent(schoolId, userId, "S-001");
        var academicYearId = Guid.NewGuid();
        var classGroupId = Guid.NewGuid();
        var adoptionId = Guid.NewGuid();
        var outcomeId = Guid.NewGuid();

        context.StudentProfiles.Add(student);
        context.ClassGroups.Add(new ClassGroup
        {
            Id = classGroupId,
            SchoolId = schoolId,
            AcademicYearId = academicYearId,
            AcademicProgramId = Guid.NewGuid(),
            GradeLevelId = Guid.NewGuid(),
            CurriculumAdoptionId = adoptionId,
            Name = "Class A",
            NormalizedName = "CLASS A",
            Code = "A",
            NormalizedCode = "A",
            Status = AcademicStructureStatus.Active,
            RowVersion = []
        });
        context.StudentEnrollments.Add(new StudentEnrollment
        {
            Id = Guid.NewGuid(),
            SchoolId = schoolId,
            StudentProfileId = student.Id,
            ClassGroupId = classGroupId,
            AcademicYearId = academicYearId,
            EnrolledAtUtc = DateTime.UtcNow
        });
        context.LearningOutcomes.Add(new LearningOutcome
        {
            Id = outcomeId,
            SchoolId = schoolId,
            AcademicProgramId = Guid.NewGuid(),
            FrameworkVersionId = Guid.NewGuid(),
            SubjectId = Guid.NewGuid(),
            GradeLevelId = Guid.NewGuid(),
            CurriculumAdoptionId = adoptionId,
            TopicId = Guid.NewGuid(),
            Code = "M.TEST.1",
            Description = "Solve a linear equation.",
            Weight = 1,
            Order = 1
        });
        await context.SaveChangesAsync();

        var item = await AddItemAsync(
            context,
            schoolId,
            adoptionId,
            outcomeId,
            "linear-equation-family:a1:b0:c6");

        return new Fixture(
            schoolId,
            userId,
            student.Id,
            academicYearId,
            classGroupId,
            adoptionId,
            outcomeId,
            item.Id);
    }

    private static StudentProfile NewStudent(Guid schoolId, Guid userId, string number) => new()
    {
        Id = Guid.NewGuid(),
        SchoolId = schoolId,
        UserId = userId,
        StudentNumber = number,
        NormalizedStudentNumber = number,
        FirstName = "Test",
        LastName = "Student",
        DisplayName = "Test Student",
        Status = AcademicStructureStatus.Active,
        CreatedAtUtc = DateTime.UtcNow,
        UpdatedAtUtc = DateTime.UtcNow,
        RowVersion = []
    };

    private static async Task<AssessmentItem> AddItemAsync(
        EdulyticsDbContext context,
        Guid schoolId,
        Guid adoptionId,
        Guid outcomeId,
        string fingerprint)
    {
        var item = new AssessmentItem
        {
            Id = Guid.NewGuid(),
            SchoolId = schoolId,
            CurriculumAdoptionId = adoptionId,
            Source = AssessmentItemSource.SystemGenerated,
            ItemType = AssessmentItemType.Numeric,
            Difficulty = AssessmentItemDifficulty.Medium,
            Prompt = "3x = 18. Find x.",
            CorrectAnswer = "6",
            Solution = "Divide both sides by 3.",
            GenerationMethod = "native-template-v1",
            GenerationFamily = "linear-equation-family",
            GenerationParametersJson = "{\"coefficient\":3,\"result\":18}",
            ExposureFingerprint = fingerprint,
            ValidationMetadataJson = "{\"validated\":true}",
            CreatedAtUtc = DateTime.UtcNow,
            RowVersion = []
        };
        context.AssessmentItems.Add(item);
        context.AssessmentItemOutcomes.Add(new AssessmentItemOutcome
        {
            Id = Guid.NewGuid(),
            SchoolId = schoolId,
            AssessmentItemId = item.Id,
            LearningOutcomeId = outcomeId
        });
        await context.SaveChangesAsync();
        return item;
    }

    private sealed record Fixture(
        Guid SchoolId,
        Guid StudentUserId,
        Guid StudentProfileId,
        Guid AcademicYearId,
        Guid ClassGroupId,
        Guid AdoptionId,
        Guid OutcomeId,
        Guid ItemId);
}
