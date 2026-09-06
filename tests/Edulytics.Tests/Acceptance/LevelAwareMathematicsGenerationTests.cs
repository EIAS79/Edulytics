using System.Text.Json;
using Edulytics.Core.AssessmentIntelligence;
using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;
using Edulytics.Core.Enums;
using Edulytics.Core.MathematicsGeneration;
using Edulytics.Services.Assessments;
using Edulytics.Services.MathematicsGeneration;

namespace Edulytics.Tests.Acceptance;

public sealed class LevelAwareMathematicsGenerationTests
{
    private readonly MathematicsQuestionGenerationEngine _engine = new();

    [Fact]
    public void Resolver_ExtractsExplicitWithinCeilingFromReviewedOutcome()
    {
        var outcome = new LearningOutcome
        {
            Id = Guid.NewGuid(),
            Code = "CCSS:K.OA.A.5",
            Description = "Fluently add and subtract within 5."
        };

        var profile = NativeMathematicsOutcomeProfileResolver.Resolve(outcome);

        Assert.NotNull(profile);
        Assert.Equal(5, profile!.IntegerComputationMaximum);
        Assert.Contains(
            CanonicalMathematicsSkill.WholeNumberAdditionAndSubtraction,
            profile.CanonicalSkills);
    }

    [Fact]
    public void ExplicitOutcomeCeiling_ConstrainsOperandsAndResultAcrossDifficulties()
    {
        var outcomeId = Guid.NewGuid();
        var profile = IntegerProfile(
            outcomeId,
            CanonicalMathematicsSkill.WholeNumberAddition,
            maximum: 5);
        var levelKey = CurriculumLevelIdentityRegistry.BuildKey(
            MathematicsCurriculumPackRegistry.CommonCoreCode,
            1,
            null);

        var batch = _engine.Generate(new MathematicsGenerationRequest(
            Blueprint(outcomeId, levelKey, questionCount: 12),
            [profile],
            701));

        Assert.All(batch.Items, generated =>
        {
            using var parameters = JsonDocument.Parse(generated.Item.GenerationParametersJson!);
            var root = parameters.RootElement;
            var a = root.GetProperty("A").GetInt32();
            var b = root.GetProperty("B").GetInt32();
            var answer = int.Parse(generated.Item.CorrectAnswer!);

            Assert.Equal("add", root.GetProperty("Operation").GetString());
            Assert.InRange(a, 1, 5);
            Assert.InRange(b, 1, 5);
            Assert.InRange(answer, 0, 5);
            Assert.True(a + b <= 5);
        });
    }

    [Fact]
    public void RegisteredStageOneLevel_AppliesCeilingWhenOutcomeHasNoNumericBound()
    {
        var outcomeId = Guid.NewGuid();
        var profile = IntegerProfile(
            outcomeId,
            CanonicalMathematicsSkill.WholeNumberAddition,
            maximum: null);
        var levelKey = CurriculumLevelIdentityRegistry.BuildKey(
            MathematicsCurriculumPackRegistry.CambridgeCode,
            1,
            null);

        var batch = _engine.Generate(new MathematicsGenerationRequest(
            Blueprint(outcomeId, levelKey, questionCount: 9),
            [profile],
            702));

        Assert.All(batch.Items, generated =>
        {
            using var parameters = JsonDocument.Parse(generated.Item.GenerationParametersJson!);
            var root = parameters.RootElement;
            var a = root.GetProperty("A").GetInt32();
            var b = root.GetProperty("B").GetInt32();
            var answer = int.Parse(generated.Item.CorrectAnswer!);

            Assert.InRange(a, 1, 10);
            Assert.InRange(b, 1, 10);
            Assert.InRange(answer, 0, 10);
        });
    }

    [Theory]
    [InlineData(CanonicalMathematicsSkill.WholeNumberMultiplication, "multiply")]
    [InlineData(CanonicalMathematicsSkill.WholeNumberDivision, "divide")]
    public void ExplicitWithin100_CapsMultiplicationProductAndDivisionDividend(
        CanonicalMathematicsSkill skill,
        string expectedOperation)
    {
        var outcomeId = Guid.NewGuid();
        var profile = IntegerProfile(outcomeId, skill, maximum: 100);
        var levelKey = CurriculumLevelIdentityRegistry.BuildKey(
            MathematicsCurriculumPackRegistry.CommonCoreCode,
            4,
            null);

        var batch = _engine.Generate(new MathematicsGenerationRequest(
            Blueprint(outcomeId, levelKey, questionCount: 12),
            [profile],
            703));

        Assert.All(batch.Items, generated =>
        {
            using var parameters = JsonDocument.Parse(generated.Item.GenerationParametersJson!);
            var root = parameters.RootElement;
            var a = root.GetProperty("A").GetInt32();
            var b = root.GetProperty("B").GetInt32();

            Assert.Equal(expectedOperation, root.GetProperty("Operation").GetString());
            if (expectedOperation == "multiply")
            {
                Assert.True(a * b <= 100);
            }
            else
            {
                Assert.True(a <= 100);
                Assert.True(b > 0);
                Assert.Equal(0, a % b);
            }
        });
    }

    private static MathematicsOutcomeGenerationProfile IntegerProfile(
        Guid outcomeId,
        CanonicalMathematicsSkill skill,
        int? maximum) =>
        new(
            outcomeId,
            $"TEST-{skill}",
            [MathematicsGeneratorFamily.IntegerComputation])
        {
            CanonicalSkills = [skill],
            IntegerComputationMaximum = maximum
        };

    private static AssessmentBlueprint Blueprint(
        Guid outcomeId,
        string levelKey,
        int questionCount)
    {
        var easy = questionCount / 3;
        var medium = questionCount / 3;
        var challenging = questionCount - easy - medium;

        return new AssessmentBlueprint(
            Guid.NewGuid(),
            Guid.NewGuid(),
            levelKey,
            Guid.NewGuid(),
            null,
            AssessmentPurpose.StudentPersonalTest,
            questionCount,
            [new OutcomeBlueprintAllocation(outcomeId, questionCount, 100m, "level-aware reviewed outcome")],
            [
                new DifficultyBlueprintAllocation(AssessmentItemDifficulty.Easy, easy),
                new DifficultyBlueprintAllocation(AssessmentItemDifficulty.Medium, medium),
                new DifficultyBlueprintAllocation(AssessmentItemDifficulty.Challenging, challenging)
            ],
            [new QuestionFamilyBlueprintAllocation(AssessmentQuestionFamily.DirectComputation, questionCount)],
            [new ItemTypeBlueprintAllocation(AssessmentItemType.Numeric, questionCount)],
            [new OutcomeEvidenceRequirement(outcomeId, questionCount, true, true)],
            [],
            "phase32-v1");
    }
}
