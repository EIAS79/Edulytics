using System.Text.Json;
using Edulytics.Core.Analytics;
using Edulytics.Core.AssessmentIntelligence;
using Edulytics.Core.Enums;
using Edulytics.Core.Mathematics.Assessment;
using Edulytics.Core.MathematicsGeneration;
using Edulytics.Core.Recovery;
using Edulytics.Services.AssessmentIntelligence;
using Edulytics.Services.Mathematics;
using Edulytics.Services.MathematicsGeneration;
using Edulytics.Services.Recovery;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class Stage21ReassessmentMigrationTests
{
    [Theory]
    [InlineData("CCSS:3.NF.A.3", "fractions.equivalent")]
    [InlineData("CCSS:4.NF.A.1", "fractions.equivalent")]
    [InlineData("CCSS:6.RP.A.2", "ratio.unit_rate")]
    [InlineData("CCSS:6.RP.A.3", "ratio.unit_rate")]
    public void EveryExactOutcomeGeneratesMathematicallyFreshVerifiedReassessment(
        string outcomeCode,
        string skillId)
    {
        var scope = Scope();
        var recovery = new WeaknessRecoveryEngine(new AssessmentBlueprintEngine());
        var plan = recovery.BuildPlan(Request(
            scope,
            outcomeCode,
            previousFingerprints: [],
            previousPrompts: [],
            previousSignatures: []));

        var profile = new MathematicsOutcomeGenerationProfile(
            scope.OutcomeId,
            outcomeCode,
            []);

        var generator = new EquivalentReassessmentGenerator(
            new MathematicsQuestionGenerationEngine(),
            recovery);

        var batch = generator.Generate(plan, [profile], seed: 21021);

        Assert.Equal(Stage21ExactReassessmentEngine.GenerationMethod, batch.GeneratorVersion);
        Assert.Equal(4, batch.Items.Count);

        Assert.True(Stage19AssessmentSkillContracts.TryResolve(outcomeCode, out var contract));
        Assert.NotNull(contract);
        Assert.Equal(skillId, contract!.SkillId);

        var signatures = batch.Items
            .Select(x => Stage21ExactReassessmentEngine.ExtractSignature(x.Item))
            .ToArray();

        Assert.Equal(4, signatures.Select(x => x.CoefficientSignature).Distinct().Count());
        Assert.True(signatures.Select(x => x.Representation).Distinct().Count() >= 2);
        Assert.True(signatures.Select(x => x.Strategy).Distinct().Count() >= 2);
        Assert.True(signatures.Select(x => x.Context).Distinct().Count() >= 2);
        Assert.True(signatures.Select(x => x.MisconceptionTrap).Distinct().Count() >= 2);
        Assert.True(signatures.Select(x => x.CognitiveDemand).Distinct().Count() >= 2);

        foreach (var generated in batch.Items)
        {
            Assert.Equal(scope.OutcomeId, generated.OutcomeLink.LearningOutcomeId);
            Assert.Equal(scope.LessonId, generated.Item.CurriculumPedagogicalLessonId);
            Assert.Contains(generated.Item.GenerationFamily!, contract.AllowedQuestionFamilies);
            Assert.Contains(
                "\"mathematicalFreshnessVerified\":true",
                generated.Item.ValidationMetadataJson,
                StringComparison.Ordinal);
            Assert.Contains(
                "\"wordingOnlyFreshness\":false",
                generated.Item.ValidationMetadataJson,
                StringComparison.Ordinal);

            using var parametersJson = JsonDocument.Parse(
                generated.Item.GenerationParametersJson);
            var values = parametersJson.RootElement
                .GetProperty("parameters")
                .EnumerateObject()
                .ToDictionary(
                    x => x.Name,
                    x => x.Value.GetInt32(),
                    StringComparer.Ordinal);

            Assert.True(ExactSkillContractQuestionEngine.Verify(
                generated.Item.GenerationFamily!,
                values,
                generated.Item.CorrectAnswer));
        }

        recovery.ValidateEquivalentReassessment(plan, batch);
    }

    [Fact]
    public void PriorExactExposureRequiresMathematicalSignatureNotOnlyPromptOrFingerprint()
    {
        var scope = Scope();
        var recovery = new WeaknessRecoveryEngine(new AssessmentBlueprintEngine());
        var plan = recovery.BuildPlan(Request(
            scope,
            "CCSS:4.NF.A.1",
            previousFingerprints: ["prior-exposure"],
            previousPrompts: ["Complete the equivalent fraction: 1/2 = ?/4."],
            previousSignatures: []));

        var generator = new EquivalentReassessmentGenerator(
            new MathematicsQuestionGenerationEngine(),
            recovery);

        var error = Assert.Throws<InvalidOperationException>(() =>
            generator.Generate(
                plan,
                [new MathematicsOutcomeGenerationProfile(
                    scope.OutcomeId,
                    "CCSS:4.NF.A.1",
                    [])],
                seed: 99));

        Assert.Contains(
            "mathematical signatures",
            error.Message,
            StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void ExactReassessmentDoesNotReusePriorCoefficientsAndChangesMultipleMathDimensions()
    {
        var scope = Scope();
        var prior = new ReassessmentMathematicalSignature(
            "prior-stage21",
            "fractions.equivalent.missing_value",
            "baseD=4;baseN=1;targetD=8",
            "symbolic-equation",
            "scale-factor",
            "abstract-equivalence",
            "additive-instead-of-multiplicative",
            ReassessmentCognitiveDemand.Standard);

        var recovery = new WeaknessRecoveryEngine(new AssessmentBlueprintEngine());
        var plan = recovery.BuildPlan(Request(
            scope,
            "CCSS:4.NF.A.1",
            previousFingerprints: ["prior-stage21"],
            previousPrompts: ["Complete the equivalent fraction: 1/4 = ?/8."],
            previousSignatures: [prior]));

        var generator = new EquivalentReassessmentGenerator(
            new MathematicsQuestionGenerationEngine(),
            recovery);

        var batch = generator.Generate(
            plan,
            [new MathematicsOutcomeGenerationProfile(
                scope.OutcomeId,
                "CCSS:4.NF.A.1",
                [])],
            seed: 1);

        foreach (var generated in batch.Items)
        {
            var current = Stage21ExactReassessmentEngine.ExtractSignature(
                generated.Item);

            Assert.NotEqual(
                prior.CoefficientSignature,
                current.CoefficientSignature);

            var changed = 0;
            if (current.QuestionFamily != prior.QuestionFamily) changed++;
            if (current.Representation != prior.Representation) changed++;
            if (current.Strategy != prior.Strategy) changed++;
            if (current.Context != prior.Context) changed++;
            if (current.MisconceptionTrap != prior.MisconceptionTrap) changed++;
            if (current.CognitiveDemand != prior.CognitiveDemand) changed++;

            Assert.True(
                changed >= 2,
                $"Expected at least two non-coefficient mathematical dimensions to change; changed={changed}.");
        }
    }

    [Fact]
    public void RecoveryPlanCarriesOnlyDeclaredReconstructableMathematicalHistory()
    {
        var scope = Scope();
        var signature = new ReassessmentMathematicalSignature(
            "prior-stage21",
            "ratio.unit_rate.direct",
            "quantity=3;total=18",
            "ratio-table",
            "divide-to-one",
            "travel",
            "use-total-as-unit-rate",
            ReassessmentCognitiveDemand.Stretch);

        var recovery = new WeaknessRecoveryEngine(new AssessmentBlueprintEngine());
        var plan = recovery.BuildPlan(Request(
            scope,
            "CCSS:6.RP.A.2",
            previousFingerprints: ["prior-stage21"],
            previousPrompts: ["A vehicle travels 18 km in 3 hours."],
            previousSignatures: [signature]));

        Assert.Single(plan.PreviousMathematicalSignatures!);
        Assert.Equal(signature, plan.PreviousMathematicalSignatures![0]);
    }

    [Fact]
    public void MathematicalHistoryOutsideDeclaredExposureFailsClosed()
    {
        var scope = Scope();
        var signature = new ReassessmentMathematicalSignature(
            "not-declared",
            "ratio.unit_rate.direct",
            "quantity=3;total=18",
            "ratio-table",
            "divide-to-one",
            "travel",
            "use-total-as-unit-rate",
            ReassessmentCognitiveDemand.Stretch);

        var recovery = new WeaknessRecoveryEngine(new AssessmentBlueprintEngine());

        Assert.Throws<InvalidOperationException>(() =>
            recovery.BuildPlan(Request(
                scope,
                "CCSS:6.RP.A.2",
                previousFingerprints: ["different-fingerprint"],
                previousPrompts: [],
                previousSignatures: [signature])));
    }

    [Fact]
    public void NonMigratedOutcomeRetainsLegacyPhase36Generator()
    {
        var scope = Scope();
        var recovery = new WeaknessRecoveryEngine(new AssessmentBlueprintEngine());
        var plan = recovery.BuildPlan(Request(
            scope,
            "MATH.RECOVERY.1",
            previousFingerprints: [],
            previousPrompts: [],
            previousSignatures: []));

        var generator = new EquivalentReassessmentGenerator(
            new MathematicsQuestionGenerationEngine(),
            recovery);

        var batch = generator.Generate(
            plan,
            [
                new MathematicsOutcomeGenerationProfile(
                    scope.OutcomeId,
                    "MATH.RECOVERY.1",
                    [MathematicsGeneratorFamily.IntegerComputation])
            ],
            seed: 97);

        Assert.Equal(MathematicsQuestionGenerationEngine.GeneratorVersion, batch.GeneratorVersion);
        Assert.All(batch.Items, x =>
            Assert.NotEqual(
                Stage21ExactReassessmentEngine.GenerationMethod,
                x.Item.GenerationMethod));
    }

    private static WeaknessRecoveryRequest Request(
        TestScope scope,
        string outcomeCode,
        IReadOnlyCollection<string> previousFingerprints,
        IReadOnlyCollection<string> previousPrompts,
        IReadOnlyCollection<ReassessmentMathematicalSignature> previousSignatures)
    {
        return new WeaknessRecoveryRequest(
            scope.SchoolId,
            scope.AdoptionId,
            "EXACT",
            Guid.NewGuid(),
            scope.LessonId,
            Profile(scope, outcomeCode),
            scope.OutcomeId,
            previousFingerprints,
            previousPrompts,
            AssessmentDifficultyPolicy.Balanced,
            PracticeQuestionCount: 4,
            ReassessmentQuestionCount: 4,
            PreviousMathematicalSignatures: previousSignatures);
    }

    private static StudentLearningProfile Profile(
        TestScope scope,
        string outcomeCode)
    {
        var row = new StudentOutcomeLearningProfile(
            scope.OutcomeId,
            outcomeCode,
            "Stage 21 reassessment outcome.",
            58m,
            MasteryBand.Developing,
            6,
            90m,
            DateTime.UtcNow,
            2,
            2,
            2,
            4m,
            "phase31-v2");

        return new StudentLearningProfile(
            scope.SchoolId,
            scope.StudentId,
            Guid.NewGuid(),
            Guid.NewGuid(),
            scope.AdoptionId,
            58m,
            MasteryBand.Developing,
            6,
            90m,
            DateTime.UtcNow,
            [row],
            "phase31-v2");
    }

    private static TestScope Scope() =>
        new(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid());

    private sealed record TestScope(
        Guid SchoolId,
        Guid AdoptionId,
        Guid StudentId,
        Guid OutcomeId,
        Guid LessonId);
}
