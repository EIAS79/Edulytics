using System.Text.Json;
using Edulytics.Core.AssessmentIntelligence;
using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;
using Edulytics.Core.Enums;
using Edulytics.Core.MathematicsGeneration;
using Edulytics.Services.Assessments;
using Edulytics.Services.MathematicsGeneration;

namespace Edulytics.Tests.Acceptance;

public sealed class UniversalMathematicsGenerationTests
{
    private readonly UniversalMathematicsQuestionGenerationEngine _engine = new();

    [Fact]
    public void MixedNativeAndContextualOutcomes_GenerateInOneSharedBatch()
    {
        var nativeOutcome = new LearningOutcome
        {
            Id = Guid.NewGuid(),
            Code = "CCSS:3.NBT.A.2",
            Description = "Fluently add and subtract within 1000 using strategies and algorithms based on place value."
        };
        var contextualOutcome = new LearningOutcome
        {
            Id = Guid.NewGuid(),
            Code = "CCSS:3.OA.C.7",
            Description = "Fluently multiply and divide within 100 using strategies such as the relationship between multiplication and division."
        };

        var nativeProfile = Assert.IsType<MathematicsOutcomeGenerationProfile>(
            NativeMathematicsOutcomeProfileResolver.Resolve(nativeOutcome));
        var contextualProfile = Assert.IsType<MathematicsOutcomeGenerationProfile>(
            NativeMathematicsOutcomeProfileResolver.Resolve(contextualOutcome));

        Assert.False(nativeProfile.IsContextualAssisted);
        Assert.True(contextualProfile.IsContextualAssisted);

        var levelKey = CurriculumLevelIdentityRegistry.BuildKey(
            MathematicsCurriculumPackRegistry.CommonCoreCode,
            4,
            null);
        var blueprint = Blueprint(
            levelKey,
            [nativeOutcome.Id, contextualOutcome.Id],
            [
                new OutcomeBlueprintAllocation(nativeOutcome.Id, 1, 100m, "native"),
                new OutcomeBlueprintAllocation(contextualOutcome.Id, 1, 100m, "contextual")
            ],
            2,
            AssessmentQuestionFamily.DirectComputation,
            AssessmentItemType.Numeric);

        var batch = _engine.Generate(new MathematicsGenerationRequest(
            blueprint,
            [nativeProfile, contextualProfile],
            901));

        Assert.Equal(2, batch.Items.Count);
        Assert.Equal(2, batch.Items.Select(x => x.OutcomeLink.LearningOutcomeId).Distinct().Count());
        Assert.Contains(batch.Items, x =>
            x.Item.GenerationFamily == MathematicsGeneratorFamily.IntegerComputation.ToString());
        Assert.Contains(batch.Items, x =>
            x.Item.GenerationFamily == MathematicsGeneratorFamily.CurriculumContextCheck.ToString());
        Assert.All(batch.Items, x =>
        {
            Assert.False(string.IsNullOrWhiteSpace(x.Item.CorrectAnswer));
            Assert.False(string.IsNullOrWhiteSpace(x.Item.Solution));
            Assert.False(string.IsNullOrWhiteSpace(x.Item.ExposureFingerprint));
        });
    }

    [Fact]
    public void ContextualOneStepEquation_IsGradeableAndReconstructable()
    {
        var outcome = new LearningOutcome
        {
            Id = Guid.NewGuid(),
            Code = "MATH-ONE-STEP",
            Description = "Solve a one-step equation."
        };
        var profile = Assert.IsType<MathematicsOutcomeGenerationProfile>(
            NativeMathematicsOutcomeProfileResolver.Resolve(outcome));
        Assert.True(profile.IsContextualAssisted);

        var levelKey = CurriculumLevelIdentityRegistry.BuildKey(
            MathematicsCurriculumPackRegistry.CommonCoreCode,
            7,
            null);
        var blueprint = Blueprint(
            levelKey,
            [outcome.Id],
            [new OutcomeBlueprintAllocation(outcome.Id, 1, 100m, "one-step contextual")],
            1,
            AssessmentQuestionFamily.MathematicalReasoning,
            AssessmentItemType.Numeric);

        var generated = Assert.Single(_engine.Generate(new MathematicsGenerationRequest(
            blueprint,
            [profile],
            902)).Items);

        Assert.Equal(
            MathematicsGeneratorFamily.CurriculumContextCheck.ToString(),
            generated.Item.GenerationFamily);
        Assert.Contains("Solve for x", generated.Item.Prompt, StringComparison.OrdinalIgnoreCase);
        Assert.True(int.TryParse(generated.Item.CorrectAnswer, out _));
        Assert.Contains(generated.Item.CorrectAnswer!, generated.Item.Solution!, StringComparison.Ordinal);
        Assert.Contains("AiAssisted", generated.Item.ValidationMetadataJson!, StringComparison.Ordinal);

        using var json = JsonDocument.Parse(generated.Item.GenerationParametersJson!);
        var operation = json.RootElement.GetProperty("Operation").GetString();
        Assert.Contains(operation, new[] { "equation-add", "equation-multiply" });
    }

    [Fact]
    public void ContextualGeneration_IsDeterministicForSameScopeAndSeed()
    {
        var outcome = new LearningOutcome
        {
            Id = Guid.NewGuid(),
            Code = "CCSS:7.G.A.1",
            Description = "Solve mathematical problems involving scale drawings of geometric figures and area."
        };
        var profile = Assert.IsType<MathematicsOutcomeGenerationProfile>(
            NativeMathematicsOutcomeProfileResolver.Resolve(outcome));
        var levelKey = CurriculumLevelIdentityRegistry.BuildKey(
            MathematicsCurriculumPackRegistry.CommonCoreCode,
            8,
            null);
        var blueprint = Blueprint(
            levelKey,
            [outcome.Id],
            [new OutcomeBlueprintAllocation(outcome.Id, 1, 100m, "geometry contextual")],
            1,
            AssessmentQuestionFamily.AppliedProblem,
            AssessmentItemType.ShortAnswer);
        var request = new MathematicsGenerationRequest(blueprint, [profile], 903);

        var first = Assert.Single(_engine.Generate(request).Items).Item;
        var second = Assert.Single(_engine.Generate(request).Items).Item;

        Assert.Equal(first.Prompt, second.Prompt);
        Assert.Equal(first.CorrectAnswer, second.CorrectAnswer);
        Assert.Equal(first.Solution, second.Solution);
        Assert.Equal(first.GenerationParametersJson, second.GenerationParametersJson);
        Assert.Equal(first.ExposureFingerprint, second.ExposureFingerprint);
        Assert.Equal(
            MathematicsGeneratorFamily.CurriculumContextCheck.ToString(),
            first.GenerationFamily);
    }

    [Fact]
    public void ContextualMultipleChoice_PreservesNumericCorrectAnswerForExistingPracticeGrader()
    {
        var outcome = new LearningOutcome
        {
            Id = Guid.NewGuid(),
            Code = "CAM:OUT:0096:1Ni.02",
            Description = "Cambridge Mathematics reference objective 1Ni.02",
            GenerationSemanticHint = "Addition, Subtraction and Doubles :: Join Groups to Add"
        };
        var profile = Assert.IsType<MathematicsOutcomeGenerationProfile>(
            NativeMathematicsOutcomeProfileResolver.Resolve(outcome));
        var levelKey = CurriculumLevelIdentityRegistry.BuildKey(
            MathematicsCurriculumPackRegistry.CambridgeCode,
            1,
            null);
        var blueprint = Blueprint(
            levelKey,
            [outcome.Id],
            [new OutcomeBlueprintAllocation(outcome.Id, 1, 100m, "Cambridge contextual")],
            1,
            AssessmentQuestionFamily.MathematicalReasoning,
            AssessmentItemType.MultipleChoice);

        var item = Assert.Single(_engine.Generate(new MathematicsGenerationRequest(
            blueprint,
            [profile],
            904)).Items).Item;

        Assert.Equal(AssessmentItemType.MultipleChoice, item.ItemType);
        Assert.True(int.TryParse(item.CorrectAnswer, out var answer));
        Assert.Contains(answer.ToString(), item.Prompt, StringComparison.Ordinal);
        Assert.Contains("A)", item.Prompt, StringComparison.Ordinal);
        Assert.Contains("D)", item.Prompt, StringComparison.Ordinal);
    }

    private static AssessmentBlueprint Blueprint(
        string levelKey,
        IReadOnlyList<Guid> outcomeIds,
        IReadOnlyList<OutcomeBlueprintAllocation> allocations,
        int questionCount,
        AssessmentQuestionFamily family,
        AssessmentItemType itemType)
    {
        return new AssessmentBlueprint(
            Guid.NewGuid(),
            Guid.NewGuid(),
            levelKey,
            Guid.NewGuid(),
            null,
            AssessmentPurpose.StudentPersonalTest,
            questionCount,
            allocations,
            [new DifficultyBlueprintAllocation(AssessmentItemDifficulty.Easy, questionCount)],
            [new QuestionFamilyBlueprintAllocation(family, questionCount)],
            [new ItemTypeBlueprintAllocation(itemType, questionCount)],
            outcomeIds.Select(x => new OutcomeEvidenceRequirement(x, 1, true, true)).ToArray(),
            [],
            "round2-universal-test");
    }
}
