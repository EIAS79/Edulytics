using Edulytics.Core.AdaptiveAssessment;
using Edulytics.Core.Analytics;
using Edulytics.Core.AssessmentIntelligence;
using Edulytics.Core.Entities;
using Edulytics.Core.Enums;
using Edulytics.Core.ExamGeneration;
using Edulytics.Core.LearningIntelligence;
using Edulytics.Core.MathematicsGeneration;
using Edulytics.Core.Recovery;
using Edulytics.Data.Contexts;
using Edulytics.Data.Repositories;
using Edulytics.Services.AdaptiveAssessment;
using Edulytics.Services.Analytics;
using Edulytics.Services.AssessmentIntelligence;
using Edulytics.Services.ExamGeneration;
using Edulytics.Services.LearningIntelligence;
using Edulytics.Services.MathematicsGeneration;
using Edulytics.Services.Practice;
using Edulytics.Services.Recovery;
using Microsoft.EntityFrameworkCore;

namespace Edulytics.Tests.FullSystem;

public sealed class FullSystemLearningLifecycleTests
{
    [Fact]
    public async Task StudentLearningLifecycle_PreservesScopeProvenanceAndFreshRecovery_FromGenerationToIntelligence()
    {
        await using var context = CreateContext();
        var fixture = await SeedStudentScopeAsync(context);
        var blueprintEngine = new AssessmentBlueprintEngine();
        var generationEngine = new MathematicsQuestionGenerationEngine();
        var practice = new PracticeService(new PracticeRepository(context));
        var analytics = new AnalyticsProjectionBuilder();
        var adaptive = new AdaptiveDiagnosticAssessmentEngine();
        var recovery = new WeaknessRecoveryEngine(blueprintEngine);
        var intelligence = new LearningIntelligenceEngine();

        var initialBlueprint = blueprintEngine.Build(new AssessmentBlueprintRequest(
            fixture.SchoolId,
            fixture.AdoptionId,
            fixture.CurriculumLevelKey,
            fixture.TopicId,
            fixture.LessonId,
            [fixture.Outcome.Id],
            null,
            AssessmentPurpose.Practice,
            2,
            AssessmentDifficultyPolicy.Balanced,
            []));
        var initialBatch = generationEngine.Generate(new MathematicsGenerationRequest(
            initialBlueprint,
            [GenerationProfile(fixture.Outcome.Id)],
            41));

        Assert.All(initialBatch.Items, generated =>
        {
            Assert.Equal(fixture.SchoolId, generated.Item.SchoolId);
            Assert.Equal(fixture.AdoptionId, generated.Item.CurriculumAdoptionId);
            Assert.Equal(fixture.TopicId, generated.Item.CurriculumTopicId);
            Assert.Equal(fixture.LessonId, generated.Item.CurriculumPedagogicalLessonId);
            Assert.Equal(fixture.Outcome.Id, generated.OutcomeLink.LearningOutcomeId);
            Assert.Equal(generated.Item.Id, generated.OutcomeLink.AssessmentItemId);
            Assert.False(string.IsNullOrWhiteSpace(generated.Item.ValidationMetadataJson));
            Assert.False(string.IsNullOrWhiteSpace(generated.Item.ExposureFingerprint));
        });

        await PersistBatchAsync(context, initialBatch);
        await CompletePracticeAsync(
            practice,
            fixture.Student.UserId!.Value,
            fixture.AdoptionId,
            initialBatch,
            answerCorrectly: false);

        var baselineProfile = await BuildProfileAsync(context, fixture, analytics);
        var baselineOutcome = Assert.Single(baselineProfile.Outcomes);
        Assert.Equal(fixture.SchoolId, baselineProfile.SchoolId);
        Assert.Equal(fixture.Student.Id, baselineProfile.StudentProfileId);
        Assert.Equal(fixture.ClassGroup.Id, baselineProfile.ClassGroupId);
        Assert.Equal(fixture.AdoptionId, baselineProfile.CurriculumAdoptionId);
        Assert.Equal(fixture.Outcome.Id, baselineOutcome.LearningOutcomeId);
        Assert.True(baselineOutcome.Band is MasteryBand.CriticalGap or MasteryBand.Weak or MasteryBand.Developing);
        Assert.Equal(initialBatch.Items.Count, baselineOutcome.EvidenceCount);

        var initialItemIds = initialBatch.Items.Select(x => x.Item.Id).ToHashSet();
        var baselineEvidence = await context.LearningEvidence.AsNoTracking().ToArrayAsync();
        Assert.Equal(initialItemIds.Count, baselineEvidence.Length);
        Assert.All(baselineEvidence, evidence =>
        {
            Assert.Equal(fixture.SchoolId, evidence.SchoolId);
            Assert.Equal(fixture.Student.Id, evidence.StudentProfileId);
            Assert.Equal(fixture.Outcome.Id, evidence.LearningOutcomeId);
            Assert.Contains(evidence.AssessmentItemId, initialItemIds);
            Assert.Equal(LearningEvidenceType.Practice, evidence.EvidenceType);
        });

        var adaptiveDecision = adaptive.DecideNext(new AdaptiveAssessmentRequest(
            fixture.SchoolId,
            fixture.AdoptionId,
            fixture.CurriculumLevelKey,
            [fixture.Outcome.Id],
            baselineProfile,
            AssessmentPurpose.Diagnostic,
            []));
        Assert.Equal(fixture.Outcome.Id, adaptiveDecision.TargetLearningOutcomeId);
        Assert.True(adaptiveDecision.RequiresFreshExposure);

        var priorExposures = await context.StudentItemExposures
            .AsNoTracking()
            .OrderBy(x => x.ExposedAtUtc)
            .ToArrayAsync();
        var priorFingerprints = priorExposures
            .Select(x => x.ExposureFingerprint)
            .ToArray();
        var priorPrompts = initialBatch.Items
            .Select(x => x.Item.Prompt)
            .ToArray();

        var recoveryPlan = recovery.BuildPlan(new WeaknessRecoveryRequest(
            fixture.SchoolId,
            fixture.AdoptionId,
            fixture.CurriculumLevelKey,
            fixture.TopicId,
            fixture.LessonId,
            baselineProfile,
            fixture.Outcome.Id,
            priorFingerprints,
            priorPrompts,
            AssessmentDifficultyPolicy.Balanced,
            PracticeQuestionCount: 2,
            ReassessmentQuestionCount: 4));

        Assert.True(recoveryPlan.ExcludePreviouslySeenQuestions);
        Assert.Equal(
            priorFingerprints.OrderBy(x => x, StringComparer.Ordinal),
            recoveryPlan.ExcludedExposureFingerprints);

        var reassessmentGenerator = new EquivalentReassessmentGenerator(generationEngine, recovery);
        var reassessmentBatch = reassessmentGenerator.Generate(
            recoveryPlan,
            [GenerationProfile(fixture.Outcome.Id)],
            97);

        recovery.ValidateEquivalentReassessment(recoveryPlan, reassessmentBatch);

        var reassessmentFingerprints = reassessmentBatch.Items
            .Select(x => x.Item.ExposureFingerprint)
            .ToArray();
        Assert.Empty(priorFingerprints.Intersect(reassessmentFingerprints, StringComparer.Ordinal));
        Assert.All(reassessmentBatch.Items, generated =>
        {
            Assert.Equal(fixture.SchoolId, generated.Item.SchoolId);
            Assert.Equal(fixture.AdoptionId, generated.Item.CurriculumAdoptionId);
            Assert.Equal(fixture.LessonId, generated.Item.CurriculumPedagogicalLessonId);
            Assert.Equal(fixture.Outcome.Id, generated.OutcomeLink.LearningOutcomeId);
        });

        await PersistBatchAsync(context, reassessmentBatch);
        await CompletePracticeAsync(
            practice,
            fixture.Student.UserId!.Value,
            fixture.AdoptionId,
            reassessmentBatch,
            answerCorrectly: true);

        var updatedProfile = await BuildProfileAsync(context, fixture, analytics);
        var updatedOutcome = Assert.Single(updatedProfile.Outcomes);
        Assert.True(updatedOutcome.MasteryPercentage > baselineOutcome.MasteryPercentage);
        Assert.True(updatedOutcome.EvidenceCount > baselineOutcome.EvidenceCount);

        var recoveryEvaluation = recovery.Evaluate(recoveryPlan, updatedProfile);
        Assert.True(recoveryEvaluation.Delta > 0m);
        Assert.True(recoveryEvaluation.Outcome is RecoveryOutcome.Improved or RecoveryOutcome.Mastered);

        var allGenerated = initialBatch.Items
            .Concat(reassessmentBatch.Items)
            .ToDictionary(x => x.Item.Id);
        var allEvidence = await context.LearningEvidence.AsNoTracking().ToArrayAsync();
        Assert.All(allEvidence, evidence =>
        {
            Assert.True(allGenerated.TryGetValue(evidence.AssessmentItemId, out var generated));
            Assert.Equal(evidence.LearningOutcomeId, generated!.OutcomeLink.LearningOutcomeId);
            Assert.Equal(evidence.SchoolId, generated.Item.SchoolId);
            Assert.False(string.IsNullOrWhiteSpace(generated.Item.GenerationMethod));
            Assert.False(string.IsNullOrWhiteSpace(generated.Item.GenerationParametersJson));
            Assert.False(string.IsNullOrWhiteSpace(generated.Item.ValidationMetadataJson));
        });

        var firstCapturedAt = DateTime.UtcNow.AddMinutes(1);
        var secondCapturedAt = firstCapturedAt.AddMinutes(1);
        var recovered = updatedOutcome.Band is MasteryBand.Secure or MasteryBand.Strong;
        var dashboard = intelligence.Build(new LearningIntelligenceRequest(
            fixture.SchoolId,
            [
                Snapshot(fixture, baselineProfile, firstCapturedAt),
                Snapshot(fixture, updatedProfile, secondCapturedAt)
            ],
            [new RecoveryIntelligenceObservation(
                fixture.SchoolId,
                fixture.Student.Id,
                fixture.Outcome.Id,
                secondCapturedAt,
                baselineOutcome.MasteryPercentage,
                updatedOutcome.MasteryPercentage,
                recovered)]));

        Assert.Equal(1, dashboard.StudentCount);
        Assert.Equal(updatedProfile.OverallMasteryPercentage, dashboard.SchoolMasteryPercentage);
        var studentTrend = Assert.Single(dashboard.StudentTrends);
        Assert.True(studentTrend.ChangePercentagePoints > 0m);
        var recoveryRow = Assert.Single(dashboard.RecoveryEffectiveness);
        Assert.Equal(1, recoveryRow.ImprovedCount);
        var drilldown = Assert.Single(dashboard.Drilldown);
        Assert.Equal(fixture.Student.Id, drilldown.StudentProfileId);
        Assert.Equal(fixture.Outcome.Id, drilldown.LearningOutcomeId);
        Assert.Equal(updatedOutcome.MasteryPercentage, drilldown.MasteryPercentage);
    }

    [Fact]
    public void GeneratedFormalAssessment_MaterializesExactGeneratedItemsAndOutcomeMappings()
    {
        var schoolId = Guid.NewGuid();
        var adoptionId = Guid.NewGuid();
        var topicId = Guid.NewGuid();
        var lessonId = Guid.NewGuid();
        var outcomeId = Guid.NewGuid();
        var blueprint = new AssessmentBlueprintEngine().Build(new AssessmentBlueprintRequest(
            schoolId,
            adoptionId,
            "US-G7",
            topicId,
            lessonId,
            [outcomeId],
            null,
            AssessmentPurpose.TeacherAssessment,
            4,
            AssessmentDifficultyPolicy.Balanced,
            []));
        var batch = new MathematicsQuestionGenerationEngine().Generate(new MathematicsGenerationRequest(
            blueprint,
            [GenerationProfile(outcomeId)],
            73));

        var exam = new ExamGenerationEngine();
        var draft = exam.CreateDraft(new ExamGenerationRequest(
            "Full-system generated assessment",
            blueprint,
            batch,
            new FormalExamContext(
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                new DateOnly(2026, 9, 1))));
        var approved = exam.Approve(exam.MarkReviewed(draft));
        var materialized = exam.MaterializeFormalAssessment(approved);

        Assert.Equal(AssessmentStatus.Draft, materialized.Assessment.Status);
        Assert.Equal(batch.Items.Count, materialized.AssessmentItems.Count);
        Assert.Equal(batch.Items.Count, materialized.Questions.Count);
        Assert.Equal(
            batch.Items.Select(x => x.Item.Id).OrderBy(x => x),
            materialized.AssessmentItems.Select(x => x.Id).OrderBy(x => x));

        foreach (var generated in batch.Items)
        {
            var item = Assert.Single(materialized.AssessmentItems, x => x.Id == generated.Item.Id);
            var question = Assert.Single(materialized.Questions, x => x.Id == generated.Item.Id);
            var itemOutcome = Assert.Single(
                materialized.ItemOutcomeMappings,
                x => x.AssessmentItemId == item.Id);
            var questionOutcome = Assert.Single(
                materialized.OutcomeMappings,
                x => x.AssessmentQuestionId == question.Id);

            Assert.Equal(schoolId, item.SchoolId);
            Assert.Equal(adoptionId, item.CurriculumAdoptionId);
            Assert.Equal(outcomeId, itemOutcome.LearningOutcomeId);
            Assert.Equal(itemOutcome.LearningOutcomeId, questionOutcome.LearningOutcomeId);
            Assert.False(string.IsNullOrWhiteSpace(item.ExposureFingerprint));
            Assert.False(string.IsNullOrWhiteSpace(item.ValidationMetadataJson));
        }
    }

    private static MathematicsOutcomeGenerationProfile GenerationProfile(Guid outcomeId) =>
        new(
            outcomeId,
            "MATH.FULLSYSTEM.1",
            [
                MathematicsGeneratorFamily.IntegerComputation,
                MathematicsGeneratorFamily.OneStepEquation,
                MathematicsGeneratorFamily.FractionOfQuantity,
                MathematicsGeneratorFamily.PercentageOfQuantity,
                MathematicsGeneratorFamily.UnitRateWordProblem
            ]);

    private static async Task PersistBatchAsync(
        EdulyticsDbContext context,
        MathematicsGenerationBatch batch)
    {
        context.AssessmentItems.AddRange(batch.Items.Select(x => x.Item));
        context.AssessmentItemOutcomes.AddRange(batch.Items.Select(x => x.OutcomeLink));
        await context.SaveChangesAsync();
    }

    private static async Task CompletePracticeAsync(
        PracticeService service,
        Guid studentUserId,
        Guid adoptionId,
        MathematicsGenerationBatch batch,
        bool answerCorrectly)
    {
        var itemIds = batch.Items.Select(x => x.Item.Id).ToArray();
        var start = await service.StartAsync(studentUserId, adoptionId, itemIds);
        Assert.True(start.Succeeded);
        var attemptId = Assert.IsType<Guid>(start.EntityId);
        var details = await service.GetAttemptAsync(studentUserId, attemptId);
        Assert.NotNull(details.Value);

        var generatedById = batch.Items.ToDictionary(x => x.Item.Id);
        foreach (var question in details.Value!.Questions.OrderBy(x => x.Order))
        {
            var generated = generatedById[question.AssessmentItemId];
            var answer = answerCorrectly
                ? generated.Item.CorrectAnswer
                : "__full_system_intentionally_wrong__";
            var feedback = await service.AnswerAsync(
                studentUserId,
                attemptId,
                question.AttemptItemId,
                answer);
            Assert.NotNull(feedback.Value);
            Assert.Equal(answerCorrectly, feedback.Value!.IsCorrect);
        }

        var submitted = await service.SubmitAsync(studentUserId, attemptId);
        Assert.NotNull(submitted.Value);
        Assert.Equal(PracticeAttemptStatus.Submitted, submitted.Value!.Status);
        Assert.Equal(answerCorrectly ? 100m : 0m, submitted.Value.Percentage);
    }

    private static async Task<StudentLearningProfile> BuildProfileAsync(
        EdulyticsDbContext context,
        Fixture fixture,
        AnalyticsProjectionBuilder analytics)
    {
        var attempts = await context.PracticeAttempts.AsNoTracking().ToArrayAsync();
        var evidence = await context.LearningEvidence.AsNoTracking().ToArrayAsync();
        var source = new AnalyticsSourceSnapshot(
            [],
            [fixture.ClassGroup],
            [],
            [fixture.Student],
            [fixture.Enrollment],
            [],
            [],
            [fixture.Outcome],
            [],
            [],
            [],
            [],
            [],
            attempts,
            evidence);

        return analytics.BuildStudentLearningProfile(
            source,
            fixture.Student.Id,
            fixture.AdoptionId,
            DateTime.UtcNow.AddMinutes(1));
    }

    private static LearningIntelligenceStudentSnapshot Snapshot(
        Fixture fixture,
        StudentLearningProfile profile,
        DateTime capturedAtUtc) =>
        new(
            fixture.SchoolId,
            fixture.Student.Id,
            fixture.Student.DisplayName,
            fixture.CurriculumLevelKey,
            "Grade 7",
            fixture.ClassGroup.Id,
            fixture.ClassGroup.Name,
            null,
            null,
            capturedAtUtc,
            profile);

    private static EdulyticsDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<EdulyticsDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString("N"))
            .Options;
        return new EdulyticsDbContext(options);
    }

    private static async Task<Fixture> SeedStudentScopeAsync(EdulyticsDbContext context)
    {
        var schoolId = Guid.NewGuid();
        var adoptionId = Guid.NewGuid();
        var academicYearId = Guid.NewGuid();
        var classId = Guid.NewGuid();
        var topicId = Guid.NewGuid();
        var lessonId = Guid.NewGuid();
        var subjectId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        var student = new StudentProfile
        {
            Id = Guid.NewGuid(),
            SchoolId = schoolId,
            UserId = userId,
            StudentNumber = "FS-001",
            NormalizedStudentNumber = "FS-001",
            FirstName = "Full",
            LastName = "System",
            DisplayName = "Full System Student",
            Status = AcademicStructureStatus.Active,
            CreatedAtUtc = DateTime.UtcNow,
            UpdatedAtUtc = DateTime.UtcNow,
            RowVersion = []
        };
        var classGroup = new ClassGroup
        {
            Id = classId,
            SchoolId = schoolId,
            AcademicYearId = academicYearId,
            AcademicProgramId = Guid.NewGuid(),
            GradeLevelId = Guid.NewGuid(),
            CurriculumAdoptionId = adoptionId,
            Name = "7A",
            NormalizedName = "7A",
            Code = "7A",
            NormalizedCode = "7A",
            Status = AcademicStructureStatus.Active,
            RowVersion = []
        };
        var enrollment = new StudentEnrollment
        {
            Id = Guid.NewGuid(),
            SchoolId = schoolId,
            StudentProfileId = student.Id,
            ClassGroupId = classGroup.Id,
            AcademicYearId = academicYearId,
            EnrolledAtUtc = DateTime.UtcNow
        };
        var outcome = new LearningOutcome
        {
            Id = Guid.NewGuid(),
            SchoolId = schoolId,
            AcademicProgramId = classGroup.AcademicProgramId,
            FrameworkVersionId = Guid.NewGuid(),
            SubjectId = subjectId,
            GradeLevelId = classGroup.GradeLevelId,
            CurriculumAdoptionId = adoptionId,
            TopicId = topicId,
            Code = "MATH.FULLSYSTEM.1",
            Description = "Solve and reason about a scoped mathematics outcome.",
            Weight = 1m,
            Order = 1
        };

        context.StudentProfiles.Add(student);
        context.ClassGroups.Add(classGroup);
        context.StudentEnrollments.Add(enrollment);
        context.LearningOutcomes.Add(outcome);
        await context.SaveChangesAsync();

        return new Fixture(
            schoolId,
            adoptionId,
            "US-G7",
            topicId,
            lessonId,
            student,
            classGroup,
            enrollment,
            outcome);
    }

    private sealed record Fixture(
        Guid SchoolId,
        Guid AdoptionId,
        string CurriculumLevelKey,
        Guid TopicId,
        Guid LessonId,
        StudentProfile Student,
        ClassGroup ClassGroup,
        StudentEnrollment Enrollment,
        LearningOutcome Outcome);
}
