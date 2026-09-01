using Edulytics.Core.Analytics;
using Edulytics.Core.AssessmentIntelligence;
using Edulytics.Core.Enums;
using Edulytics.Core.MathematicsGeneration;
using Edulytics.Core.Recovery;
using Edulytics.Services.AssessmentIntelligence;
using Edulytics.Services.MathematicsGeneration;
using Edulytics.Services.Recovery;

namespace Edulytics.Tests.Phase36;

public sealed class EquivalentReassessmentGeneratorTests
{
    [Fact]
    public void Generate_RetriesDeterministicallyUntilReassessmentIsFreshByFingerprintAndPromptShape()
    {
        var schoolId = Guid.NewGuid();
        var adoptionId = Guid.NewGuid();
        var studentId = Guid.NewGuid();
        var outcomeId = Guid.NewGuid();
        var topicId = Guid.NewGuid();
        var lessonId = Guid.NewGuid();
        var profile = Profile(schoolId, adoptionId, studentId, outcomeId);
        var recovery = new WeaknessRecoveryEngine(new AssessmentBlueprintEngine());
        var generator = new MathematicsQuestionGenerationEngine();
        var profileDefinition = GenerationProfile(outcomeId);

        var baselinePlan = recovery.BuildPlan(new WeaknessRecoveryRequest(
            schoolId,
            adoptionId,
            "US-G7",
            topicId,
            lessonId,
            profile,
            outcomeId,
            [],
            [],
            AssessmentDifficultyPolicy.Balanced,
            PracticeQuestionCount: 2,
            ReassessmentQuestionCount: 4));

        var seenBatch = generator.Generate(new MathematicsGenerationRequest(
            baselinePlan.EquivalentReassessmentBlueprint,
            [profileDefinition],
            97));
        var previousFingerprints = seenBatch.Items
            .Select(x => x.Item.ExposureFingerprint)
            .ToArray();
        var previousPrompts = seenBatch.Items
            .Select(x => x.Item.Prompt)
            .ToArray();

        var recoveryPlan = recovery.BuildPlan(new WeaknessRecoveryRequest(
            schoolId,
            adoptionId,
            "US-G7",
            topicId,
            lessonId,
            profile,
            outcomeId,
            previousFingerprints,
            previousPrompts,
            AssessmentDifficultyPolicy.Balanced,
            PracticeQuestionCount: 2,
            ReassessmentQuestionCount: 4));

        var orchestrator = new EquivalentReassessmentGenerator(generator, recovery);
        var freshBatch = orchestrator.Generate(recoveryPlan, [profileDefinition], seed: 97);

        // This is the authoritative Phase 36 gate. It verifies comparable scope,
        // difficulty, prior exposure exclusion and prompt-shape freshness.
        recovery.ValidateEquivalentReassessment(recoveryPlan, freshBatch);

        var previousFingerprintSet = previousFingerprints.ToHashSet(StringComparer.Ordinal);
        Assert.All(freshBatch.Items, generated =>
            Assert.DoesNotContain(generated.Item.ExposureFingerprint, previousFingerprintSet));
    }

    [Fact]
    public void RecalculateExposureFingerprint_MatchesPhase33GeneratorFingerprint()
    {
        var schoolId = ScopeGuid(101, 1);
        var adoptionId = ScopeGuid(101, 2);
        var studentId = ScopeGuid(101, 3);
        var outcomeId = ScopeGuid(101, 4);
        var topicId = ScopeGuid(101, 5);
        var lessonId = ScopeGuid(101, 6);
        var recovery = new WeaknessRecoveryEngine(new AssessmentBlueprintEngine());
        var generator = new MathematicsQuestionGenerationEngine();
        var profile = Profile(schoolId, adoptionId, studentId, outcomeId);
        var profileDefinition = GenerationProfile(outcomeId);
        var plan = recovery.BuildPlan(new WeaknessRecoveryRequest(
            schoolId,
            adoptionId,
            "US-G7",
            topicId,
            lessonId,
            profile,
            outcomeId,
            [],
            [],
            AssessmentDifficultyPolicy.Balanced,
            PracticeQuestionCount: 2,
            ReassessmentQuestionCount: 4));
        var batch = generator.Generate(new MathematicsGenerationRequest(
            plan.EquivalentReassessmentBlueprint,
            [profileDefinition],
            97));

        Assert.All(batch.Items, generated =>
            Assert.Equal(
                generated.Item.ExposureFingerprint,
                EquivalentReassessmentGenerator.RecalculateExposureFingerprint(
                    plan.EquivalentReassessmentBlueprint,
                    generated.OutcomeLink.LearningOutcomeId,
                    generated.Item,
                    generated.Item.Prompt)));
    }

    [Fact]
    public void Generate_IsStableAcrossManyDeterministicRecoveryScopes()
    {
        for (var scope = 1; scope <= 48; scope++)
        {
            var schoolId = ScopeGuid(scope, 1);
            var adoptionId = ScopeGuid(scope, 2);
            var studentId = ScopeGuid(scope, 3);
            var outcomeId = ScopeGuid(scope, 4);
            var topicId = ScopeGuid(scope, 5);
            var lessonId = ScopeGuid(scope, 6);
            var recovery = new WeaknessRecoveryEngine(new AssessmentBlueprintEngine());
            var generator = new MathematicsQuestionGenerationEngine();
            var profile = Profile(schoolId, adoptionId, studentId, outcomeId);
            var profileDefinition = GenerationProfile(outcomeId);

            var baselinePlan = recovery.BuildPlan(new WeaknessRecoveryRequest(
                schoolId,
                adoptionId,
                "US-G7",
                topicId,
                lessonId,
                profile,
                outcomeId,
                [],
                [],
                AssessmentDifficultyPolicy.Balanced,
                PracticeQuestionCount: 2,
                ReassessmentQuestionCount: 4));
            var seenBatch = generator.Generate(new MathematicsGenerationRequest(
                baselinePlan.EquivalentReassessmentBlueprint,
                [profileDefinition],
                97));
            var previousFingerprints = seenBatch.Items
                .Select(x => x.Item.ExposureFingerprint)
                .ToArray();
            var previousPrompts = seenBatch.Items
                .Select(x => x.Item.Prompt)
                .ToArray();

            var recoveryPlan = recovery.BuildPlan(new WeaknessRecoveryRequest(
                schoolId,
                adoptionId,
                "US-G7",
                topicId,
                lessonId,
                profile,
                outcomeId,
                previousFingerprints,
                previousPrompts,
                AssessmentDifficultyPolicy.Balanced,
                PracticeQuestionCount: 2,
                ReassessmentQuestionCount: 4));
            var orchestrator = new EquivalentReassessmentGenerator(generator, recovery);
            var freshBatch = orchestrator.Generate(
                recoveryPlan,
                [profileDefinition],
                seed: 97);

            recovery.ValidateEquivalentReassessment(recoveryPlan, freshBatch);

            var previousFingerprintSet = previousFingerprints
                .ToHashSet(StringComparer.Ordinal);
            var previousShapeSet = recoveryPlan.PreviousPromptShapes
                .ToHashSet(StringComparer.Ordinal);
            Assert.All(freshBatch.Items, generated =>
            {
                Assert.DoesNotContain(
                    generated.Item.ExposureFingerprint,
                    previousFingerprintSet);
                Assert.DoesNotContain(
                    WeaknessRecoveryEngine.NormalizePromptShape(generated.Item.Prompt),
                    previousShapeSet);
            });
        }
    }

    private static MathematicsOutcomeGenerationProfile GenerationProfile(Guid outcomeId) =>
        new(
            outcomeId,
            "MATH.RECOVERY.1",
            [
                MathematicsGeneratorFamily.IntegerComputation,
                MathematicsGeneratorFamily.OneStepEquation,
                MathematicsGeneratorFamily.FractionOfQuantity,
                MathematicsGeneratorFamily.PercentageOfQuantity,
                MathematicsGeneratorFamily.UnitRateWordProblem
            ]);

    private static StudentLearningProfile Profile(
        Guid schoolId,
        Guid adoptionId,
        Guid studentId,
        Guid outcomeId)
    {
        var row = new StudentOutcomeLearningProfile(
            outcomeId,
            "MATH.RECOVERY.1",
            "Solve a scoped mathematics recovery outcome.",
            58m,
            MasteryBand.Developing,
            6,
            90m,
            DateTime.UtcNow,
            2,
            2,
            2,
            4m,
            "phase31-v1");

        return new StudentLearningProfile(
            schoolId,
            studentId,
            Guid.NewGuid(),
            Guid.NewGuid(),
            adoptionId,
            58m,
            MasteryBand.Developing,
            6,
            90m,
            DateTime.UtcNow,
            [row],
            "phase31-v1");
    }

    private static Guid ScopeGuid(int scope, byte salt) =>
        new(
            scope,
            salt,
            0,
            salt,
            0,
            0,
            0,
            0,
            0,
            0,
            1);
}
