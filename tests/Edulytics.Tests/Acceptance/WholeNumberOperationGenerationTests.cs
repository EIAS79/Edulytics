using System.Text.Json;
using Edulytics.Core.AssessmentIntelligence;
using Edulytics.Core.Enums;
using Edulytics.Core.MathematicsGeneration;
using Edulytics.Services.MathematicsGeneration;

namespace Edulytics.Tests.Acceptance;

public sealed class WholeNumberOperationGenerationTests
{
    private readonly MathematicsQuestionGenerationEngine _engine = new();

    [Theory]
    [InlineData(CanonicalMathematicsSkill.WholeNumberMultiplication, "multiply")]
    [InlineData(CanonicalMathematicsSkill.WholeNumberDivision, "divide")]
    public void ReviewedWholeNumberOperation_GeneratesOnlyAlignedItems(
        CanonicalMathematicsSkill skill,
        string expectedOperation)
    {
        var outcomeId = Guid.NewGuid();
        var request = Request(outcomeId, skill, questionCount: 9, seed: 117);

        var batch = _engine.Generate(request);

        Assert.Equal(9, batch.Items.Count);
        Assert.All(batch.Items, generated =>
        {
            using var parameters = JsonDocument.Parse(generated.Item.GenerationParametersJson!);
            var root = parameters.RootElement;
            var operation = root.GetProperty("Operation").GetString();

            Assert.Equal(expectedOperation, operation);
            Assert.Equal(
                MathematicsGeneratorFamily.IntegerComputation.ToString(),
                generated.Item.GenerationFamily);
            Assert.Contains(
                generated.Item.CorrectAnswer!,
                generated.Item.Solution!,
                StringComparison.Ordinal);
        });
    }

    [Fact]
    public void DivisionGeneration_IsAlwaysExactAndReconstructable()
    {
        var outcomeId = Guid.NewGuid();
        var request = Request(
            outcomeId,
            CanonicalMathematicsSkill.WholeNumberDivision,
            questionCount: 18,
            seed: 209);

        var batch = _engine.Generate(request);

        Assert.All(batch.Items, generated =>
        {
            using var parameters = JsonDocument.Parse(generated.Item.GenerationParametersJson!);
            var root = parameters.RootElement;
            var dividend = root.GetProperty("A").GetInt32();
            var divisor = root.GetProperty("B").GetInt32();

            Assert.True(divisor > 0);
            Assert.Equal(0, dividend % divisor);
            Assert.Equal(
                (dividend / divisor).ToString(),
                generated.Item.CorrectAnswer);
        });
    }

    [Fact]
    public void MixedReviewedOperations_NeverGenerateOutsideCanonicalSet()
    {
        var outcomeId = Guid.NewGuid();
        var blueprint = Blueprint(outcomeId, questionCount: 24);
        var profile = new MathematicsOutcomeGenerationProfile(
            outcomeId,
            "ARITHMETIC-ALL",
            [MathematicsGeneratorFamily.IntegerComputation])
        {
            CanonicalSkills =
            [
                CanonicalMathematicsSkill.WholeNumberAdditionAndSubtraction,
                CanonicalMathematicsSkill.WholeNumberMultiplication,
                CanonicalMathematicsSkill.WholeNumberDivision
            ]
        };

        var batch = _engine.Generate(new MathematicsGenerationRequest(
            blueprint,
            [profile],
            313));
        var allowed = new HashSet<string>(StringComparer.Ordinal)
        {
            "add",
            "subtract",
            "multiply",
            "divide"
        };

        Assert.All(batch.Items, generated =>
        {
            using var parameters = JsonDocument.Parse(generated.Item.GenerationParametersJson!);
            var operation = parameters.RootElement.GetProperty("Operation").GetString();
            Assert.Contains(operation!, allowed);
        });
    }

    private static MathematicsGenerationRequest Request(
        Guid outcomeId,
        CanonicalMathematicsSkill skill,
        int questionCount,
        int seed)
    {
        var profile = new MathematicsOutcomeGenerationProfile(
            outcomeId,
            $"WHOLE-{skill}",
            [MathematicsGeneratorFamily.IntegerComputation])
        {
            CanonicalSkills = [skill]
        };

        return new MathematicsGenerationRequest(
            Blueprint(outcomeId, questionCount),
            [profile],
            seed);
    }

    private static AssessmentBlueprint Blueprint(Guid outcomeId, int questionCount)
    {
        var easy = questionCount / 3;
        var medium = questionCount / 3;
        var challenging = questionCount - easy - medium;

        return new AssessmentBlueprint(
            Guid.NewGuid(),
            Guid.NewGuid(),
            "WHOLE-NUMBER-REVIEWED",
            Guid.NewGuid(),
            null,
            AssessmentPurpose.StudentPersonalTest,
            questionCount,
            [new OutcomeBlueprintAllocation(outcomeId, questionCount, 100m, "reviewed whole-number operation")],
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
