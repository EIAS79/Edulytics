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
}
