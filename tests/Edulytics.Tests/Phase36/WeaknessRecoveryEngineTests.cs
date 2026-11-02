using Edulytics.Core.Analytics;
using Edulytics.Core.AssessmentIntelligence;
using Edulytics.Core.Entities;
using Edulytics.Core.Enums;
using Edulytics.Core.MathematicsGeneration;
using Edulytics.Core.Recovery;
using Edulytics.Services.Recovery;

namespace Edulytics.Tests.Phase36;

public sealed class WeaknessRecoveryEngineTests
{
    private readonly WeaknessRecoveryEngine _engine = new();

    [Fact]
    public void BuildPlan_RequiresNewExposureAndEquivalentCoverage()
    {
        var scope = Scope();
        var request = Request(scope, previousFingerprints: ["old-1", "old-2"]);

        var plan = _engine.BuildPlan(request);

        Assert.True(plan.ExcludePreviouslySeenQuestions);
        Assert.Equal(AssessmentPurpose.Practice, plan.TargetedPracticeBlueprint.Purpose);
        Assert.Equal(AssessmentPurpose.EquivalentReassessment, plan.EquivalentReassessmentBlueprint.Purpose);
        Assert.Equal(["old-1", "old-2"], plan.EquivalentReassessmentBlueprint.ExcludedExposureFingerprints);
        Assert.Single(plan.EquivalentReassessmentBlueprint.OutcomeAllocations);
        Assert.Equal(scope.OutcomeId, plan.EquivalentReassessmentBlueprint.OutcomeAllocations[0].LearningOutcomeId);
        Assert.Equal(scope.LessonId, plan.EquivalentReassessmentBlueprint.CurriculumPedagogicalLessonId);
        Assert.Equal("phase36-v1", plan.FormulaVersion);
    }

    [Fact]
    public void BuildPlan_RejectsAlreadySecureOutcome()
    {
        var scope = Scope();
        var profile = Profile(scope, 84m, MasteryBand.Secure);
        var request = Request(scope, profile: profile);

        Assert.Throws<InvalidOperationException>(() => _engine.BuildPlan(request));
    }

    [Fact]
    public void EquivalentReassessment_RejectsPreviouslySeenFingerprint()
    {
        var scope = Scope();
        var plan = _engine.BuildPlan(Request(scope, previousFingerprints: ["old-1"]));
        var batch = Batch(plan, fingerprintOverride: "old-1");

        Assert.Throws<InvalidOperationException>(() =>
            _engine.ValidateEquivalentReassessment(plan, batch));
    }

    [Fact]
    public void EquivalentReassessment_RejectsTrivialNumberOnlyNearDuplicate()
    {
        var scope = Scope();
        var plan = _engine.BuildPlan(Request(
            scope,
            previousPrompts: ["Mia has 12 apples and buys 3 more. How many apples now?"]));
        var batch = Batch(
            plan,
            promptOverride: "Mia has 27 apples and buys 8 more. How many apples now?");

        Assert.Throws<InvalidOperationException>(() =>
            _engine.ValidateEquivalentReassessment(plan, batch));
    }

    [Fact]
    public void EquivalentReassessment_AcceptsFreshComparableBatch()
    {
        var scope = Scope();
        var plan = _engine.BuildPlan(Request(scope));
        var batch = Batch(plan);

        _engine.ValidateEquivalentReassessment(plan, batch);
    }

    [Fact]
    public void Evaluate_ClassifiesImprovementAndMastery()
    {
        var scope = Scope();
        var plan = _engine.BuildPlan(Request(scope));

        var improved = _engine.Evaluate(plan, Profile(scope, 63m, MasteryBand.Developing));
        var mastered = _engine.Evaluate(plan, Profile(scope, 84m, MasteryBand.Secure));

        Assert.Equal(RecoveryOutcome.Improved, improved.Outcome);
        Assert.Equal(5m, improved.Delta);
        Assert.Equal(RecoveryOutcome.Mastered, mastered.Outcome);
    }

    private static WeaknessRecoveryRequest Request(
        TestScope scope,
        StudentLearningProfile? profile = null,
        IReadOnlyCollection<string>? previousFingerprints = null,
        IReadOnlyCollection<string>? previousPrompts = null) =>
        new(
            scope.SchoolId,
            scope.AdoptionId,
            "US-G7",
            Guid.NewGuid(),
            scope.LessonId,
            profile ?? Profile(scope, 58m, MasteryBand.Developing),
            scope.OutcomeId,
            previousFingerprints ?? [],
            previousPrompts ?? [],
            AssessmentDifficultyPolicy.Balanced,
            PracticeQuestionCount: 4,
            ReassessmentQuestionCount: 4);

    private static StudentLearningProfile Profile(
        TestScope scope,
        decimal mastery,
        MasteryBand band)
    {
        var row = new StudentOutcomeLearningProfile(
            scope.OutcomeId,
            "7.NS.A.1",
            "Apply and extend previous understandings of addition and subtraction.",
            mastery,
            band,
            6,
            90m,
            DateTime.UtcNow,
            2,
            2,
            2,
            4m,
            "phase31-v1");

        return new StudentLearningProfile(
            scope.SchoolId,
            scope.StudentId,
            Guid.NewGuid(),
            Guid.NewGuid(),
            scope.AdoptionId,
            mastery,
            band,
            6,
            90m,
            DateTime.UtcNow,
            [row],
            "phase31-v1");
    }

    private static MathematicsGenerationBatch Batch(
        WeaknessRecoveryPlan plan,
        string? fingerprintOverride = null,
        string? promptOverride = null)
    {
        var rows = new List<GeneratedMathematicsItem>();
        var index = 0;

        foreach (var allocation in plan.EquivalentReassessmentBlueprint.DifficultyAllocations)
        {
            for (var i = 0; i < allocation.ItemCount; i++)
            {
                index++;
                var itemId = Guid.NewGuid();
                var item = new AssessmentItem
                {
                    Id = itemId,
                    SchoolId = plan.SchoolId,
                    CurriculumAdoptionId = plan.CurriculumAdoptionId,
                    CurriculumPedagogicalLessonId = plan.CurriculumPedagogicalLessonId,
                    Source = AssessmentItemSource.SystemGenerated,
                    ItemType = AssessmentItemType.Numeric,
                    Difficulty = allocation.Difficulty,
                    Prompt = promptOverride ?? $"Fresh recovery model {index} asks for the value of variable alpha.",
                    CorrectAnswer = index.ToString(),
                    Solution = $"Verified solution {index}.",
                    GenerationMethod = "Phase36Test",
                    GenerationFamily = "IntegerComputation",
                    GenerationParametersJson = "{}",
                    ExposureFingerprint = fingerprintOverride ?? $"fresh-{index}",
                    ValidationMetadataJson = "{\"valid\":true}",
                    CreatedAtUtc = DateTime.UtcNow
                };
                var link = new AssessmentItemOutcome
                {
                    Id = Guid.NewGuid(),
                    SchoolId = plan.SchoolId,
                    AssessmentItemId = itemId,
                    LearningOutcomeId = plan.LearningOutcomeId
                };

                rows.Add(new GeneratedMathematicsItem(
                    item,
                    link,
                    AssessmentQuestionFamily.DirectComputation,
                    "phase33-v1"));
            }
        }

        return new MathematicsGenerationBatch(
            plan.SchoolId,
            plan.CurriculumAdoptionId,
            plan.CurriculumLevelKey,
            rows,
            "phase33-v1");
    }

    private static TestScope Scope() =>
        new(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());

    private sealed record TestScope(
        Guid SchoolId,
        Guid AdoptionId,
        Guid StudentId,
        Guid OutcomeId,
        Guid LessonId);
}
