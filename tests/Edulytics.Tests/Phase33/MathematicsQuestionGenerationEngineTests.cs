using Edulytics.Core.AssessmentIntelligence;
using Edulytics.Core.Enums;
using Edulytics.Core.MathematicsGeneration;
using Edulytics.Services.MathematicsGeneration;

namespace Edulytics.Tests.Phase33;

public sealed class MathematicsQuestionGenerationEngineTests
{
    private readonly MathematicsQuestionGenerationEngine _engine = new();

    [Fact]
    public void Generate_ProducesReconstructableValidatedItems()
    {
        var request = Request(questionCount: 10);

        var batch = _engine.Generate(request);

        Assert.Equal(10, batch.Items.Count);
        Assert.All(batch.Items, generated =>
        {
            Assert.Equal(AssessmentItemSource.SystemGenerated, generated.Item.Source);
            Assert.Equal("deterministic-reviewed-family", generated.Item.GenerationMethod);
            Assert.False(string.IsNullOrWhiteSpace(generated.Item.GenerationFamily));
            Assert.False(string.IsNullOrWhiteSpace(generated.Item.GenerationParametersJson));
            Assert.False(string.IsNullOrWhiteSpace(generated.Item.ValidationMetadataJson));
            Assert.False(string.IsNullOrWhiteSpace(generated.Item.ExposureFingerprint));
            Assert.False(string.IsNullOrWhiteSpace(generated.Item.CorrectAnswer));
            Assert.Contains(generated.Item.CorrectAnswer, generated.Item.Solution, StringComparison.Ordinal);
            Assert.Equal(generated.Item.Id, generated.OutcomeLink.AssessmentItemId);
        });
        Assert.Equal(
            10,
            batch.Items.Select(x => x.Item.ExposureFingerprint).Distinct(StringComparer.Ordinal).Count());
    }

    [Fact]
    public void SameInput_ProducesSameExposureIdentities()
    {
        var request = Request(questionCount: 12, seed: 77);

        var first = _engine.Generate(request);
        var second = _engine.Generate(request);

        Assert.Equal(
            first.Items.Select(x => x.Item.ExposureFingerprint),
            second.Items.Select(x => x.Item.ExposureFingerprint));
        Assert.Equal(
            first.Items.Select(x => x.Item.Prompt),
            second.Items.Select(x => x.Item.Prompt));
        Assert.Equal(
            first.Items.Select(x => x.Item.CorrectAnswer),
            second.Items.Select(x => x.Item.CorrectAnswer));
    }

    [Fact]
    public void PreviouslyExposedFingerprint_IsNeverReturnedAgain()
    {
        var original = Request(questionCount: 6, seed: 19);
        var first = _engine.Generate(original);
        var excluded = first.Items[0].Item.ExposureFingerprint;
        var blueprint = original.Blueprint with
        {
            ExcludedExposureFingerprints = [excluded]
        };

        var regenerated = _engine.Generate(original with { Blueprint = blueprint });

        Assert.DoesNotContain(
            regenerated.Items,
            x => string.Equals(
                x.Item.ExposureFingerprint,
                excluded,
                StringComparison.Ordinal));
        Assert.Equal(6, regenerated.Items.Count);
    }

    [Fact]
    public void OutcomeWithoutTrustedProfile_IsRejected()
    {
        var request = Request(questionCount: 3);
        var profiles = request.OutcomeProfiles.Skip(1).ToArray();

        Assert.Throws<InvalidOperationException>(() =>
            _engine.Generate(request with { OutcomeProfiles = profiles }));
    }

    [Fact]
    public void DuplicateOutcomeProfiles_AreRejected()
    {
        var request = Request(questionCount: 3);
        var duplicate = request.OutcomeProfiles[0];

        Assert.Throws<InvalidOperationException>(() =>
            _engine.Generate(request with
            {
                OutcomeProfiles = [.. request.OutcomeProfiles, duplicate]
            }));
    }

    [Fact]
    public void UnsupportedFamilyForOutcome_IsRejectedFailClosed()
    {
        var outcome = Guid.NewGuid();
        var blueprint = Blueprint(
            [outcome],
            questionCount: 1,
            familyAllocations:
            [
                new QuestionFamilyBlueprintAllocation(
                    AssessmentQuestionFamily.AppliedProblem,
                    1)
            ]);
        var profile = new MathematicsOutcomeGenerationProfile(
            outcome,
            "MATH.TEST",
            [MathematicsGeneratorFamily.OneStepEquation]);

        Assert.Throws<InvalidOperationException>(() =>
            _engine.Generate(new MathematicsGenerationRequest(
                blueprint,
                [profile],
                1)));
    }

    [Fact]
    public void GeneratedItems_PreserveBlueprintSchoolCurriculumAndOutcomeScope()
    {
        var request = Request(questionCount: 9);

        var batch = _engine.Generate(request);

        Assert.All(batch.Items, generated =>
        {
            Assert.Equal(request.Blueprint.SchoolId, generated.Item.SchoolId);
            Assert.Equal(
                request.Blueprint.CurriculumAdoptionId,
                generated.Item.CurriculumAdoptionId);
            Assert.Equal(request.Blueprint.SchoolId, generated.OutcomeLink.SchoolId);
            Assert.Contains(
                generated.OutcomeLink.LearningOutcomeId,
                request.Blueprint.OutcomeAllocations.Select(x => x.LearningOutcomeId));
        });
    }

    [Fact]
    public void GeneratedDimensions_MatchBlueprintAllocations()
    {
        var request = Request(questionCount: 10);
        var batch = _engine.Generate(request);

        foreach (var allocation in request.Blueprint.DifficultyAllocations)
        {
            Assert.Equal(
                allocation.ItemCount,
                batch.Items.Count(x => x.Item.Difficulty == allocation.Difficulty));
        }

        foreach (var allocation in request.Blueprint.ItemTypeAllocations)
        {
            Assert.Equal(
                allocation.ItemCount,
                batch.Items.Count(x => x.Item.ItemType == allocation.ItemType));
        }

        foreach (var allocation in request.Blueprint.QuestionFamilyAllocations)
        {
            Assert.Equal(
                allocation.ItemCount,
                batch.Items.Count(x => x.BlueprintFamily == allocation.Family));
        }

        foreach (var allocation in request.Blueprint.OutcomeAllocations)
        {
            Assert.Equal(
                allocation.ItemCount,
                batch.Items.Count(x =>
                    x.OutcomeLink.LearningOutcomeId == allocation.LearningOutcomeId));
        }
    }

    [Fact]
    public void MultipleChoicePrompt_ContainsCorrectAnswerAsAnOption()
    {
        var outcome = Guid.NewGuid();
        var blueprint = Blueprint(
            [outcome],
            questionCount: 1,
            typeAllocations:
            [new ItemTypeBlueprintAllocation(AssessmentItemType.MultipleChoice, 1)]);
        var profile = Profile(outcome);

        var generated = Assert.Single(
            _engine.Generate(new MathematicsGenerationRequest(
                blueprint,
                [profile],
                25)).Items);

        Assert.Contains(generated.Item.CorrectAnswer, generated.Item.Prompt, StringComparison.Ordinal);
    }

    [Fact]
    public void AdditionOnlyCanonicalSkill_NeverGeneratesSubtraction()
    {
        var outcome = Guid.NewGuid();
        var blueprint = Blueprint(
            [outcome],
            questionCount: 8,
            familyAllocations:
            [new QuestionFamilyBlueprintAllocation(AssessmentQuestionFamily.DirectComputation, 8)],
            typeAllocations:
            [new ItemTypeBlueprintAllocation(AssessmentItemType.Numeric, 8)]);
        var profile = new MathematicsOutcomeGenerationProfile(
            outcome,
            "ADD-ONLY",
            [MathematicsGeneratorFamily.IntegerComputation])
        {
            CanonicalSkills = [CanonicalMathematicsSkill.WholeNumberAddition]
        };

        var batch = _engine.Generate(new MathematicsGenerationRequest(
            blueprint,
            [profile],
            91));

        Assert.All(batch.Items, generated =>
            Assert.Contains(
                "\"Operation\":\"add\"",
                generated.Item.GenerationParametersJson,
                StringComparison.Ordinal));
    }

    [Fact]
    public void SubtractionOnlyCanonicalSkill_NeverGeneratesAddition()
    {
        var outcome = Guid.NewGuid();
        var blueprint = Blueprint(
            [outcome],
            questionCount: 8,
            familyAllocations:
            [new QuestionFamilyBlueprintAllocation(AssessmentQuestionFamily.DirectComputation, 8)],
            typeAllocations:
            [new ItemTypeBlueprintAllocation(AssessmentItemType.Numeric, 8)]);
        var profile = new MathematicsOutcomeGenerationProfile(
            outcome,
            "SUBTRACT-ONLY",
            [MathematicsGeneratorFamily.IntegerComputation])
        {
            CanonicalSkills = [CanonicalMathematicsSkill.WholeNumberSubtraction]
        };

        var batch = _engine.Generate(new MathematicsGenerationRequest(
            blueprint,
            [profile],
            92));

        Assert.All(batch.Items, generated =>
            Assert.Contains(
                "\"Operation\":\"subtract\"",
                generated.Item.GenerationParametersJson,
                StringComparison.Ordinal));
    }

    private static MathematicsGenerationRequest Request(
        int questionCount,
        int seed = 3)
    {
        var outcomes = new[] { Guid.NewGuid(), Guid.NewGuid() };
        return new MathematicsGenerationRequest(
            Blueprint(outcomes, questionCount),
            outcomes.Select(Profile).ToArray(),
            seed);
    }

    private static MathematicsOutcomeGenerationProfile Profile(Guid outcomeId) =>
        new(
            outcomeId,
            $"OUT-{outcomeId:N}",
            [
                MathematicsGeneratorFamily.IntegerComputation,
                MathematicsGeneratorFamily.OneStepEquation,
                MathematicsGeneratorFamily.FractionOfQuantity,
                MathematicsGeneratorFamily.PercentageOfQuantity,
                MathematicsGeneratorFamily.UnitRateWordProblem
            ]);

    private static AssessmentBlueprint Blueprint(
        IReadOnlyList<Guid> outcomes,
        int questionCount,
        IReadOnlyList<QuestionFamilyBlueprintAllocation>? familyAllocations = null,
        IReadOnlyList<ItemTypeBlueprintAllocation>? typeAllocations = null)
    {
        var firstCount = (questionCount + 1) / 2;
        var secondCount = questionCount - firstCount;
        var outcomeAllocations = outcomes.Count == 1
            ? [new OutcomeBlueprintAllocation(outcomes[0], questionCount, 100m, "test")]
            : new[]
            {
                new OutcomeBlueprintAllocation(outcomes[0], firstCount, 100m, "test"),
                new OutcomeBlueprintAllocation(outcomes[1], secondCount, 90m, "test")
            };

        var easy = questionCount * 3 / 10;
        var challenging = questionCount * 2 / 10;
        var medium = questionCount - easy - challenging;

        familyAllocations ??= AllocateFamilies(questionCount);
        typeAllocations ??= AllocateTypes(questionCount);

        return new AssessmentBlueprint(
            Guid.NewGuid(),
            Guid.NewGuid(),
            "GRADE-6",
            Guid.NewGuid(),
            null,
            AssessmentPurpose.StudentPersonalTest,
            questionCount,
            outcomeAllocations,
            [
                new DifficultyBlueprintAllocation(AssessmentItemDifficulty.Easy, easy),
                new DifficultyBlueprintAllocation(AssessmentItemDifficulty.Medium, medium),
                new DifficultyBlueprintAllocation(AssessmentItemDifficulty.Challenging, challenging)
            ],
            familyAllocations,
            typeAllocations,
            outcomeAllocations.Select(x =>
                new OutcomeEvidenceRequirement(
                    x.LearningOutcomeId,
                    x.ItemCount,
                    true,
                    true)).ToArray(),
            [],
            "phase32-v1");
    }

    private static IReadOnlyList<QuestionFamilyBlueprintAllocation> AllocateFamilies(int count)
    {
        var direct = (count + 3) / 4;
        var structured = (count + 2) / 4;
        var applied = (count + 1) / 4;
        var reasoning = count - direct - structured - applied;
        return
        [
            new(AssessmentQuestionFamily.DirectComputation, direct),
            new(AssessmentQuestionFamily.StructuredMethod, structured),
            new(AssessmentQuestionFamily.AppliedProblem, applied),
            new(AssessmentQuestionFamily.MathematicalReasoning, reasoning)
        ];
    }

    private static IReadOnlyList<ItemTypeBlueprintAllocation> AllocateTypes(int count)
    {
        var numeric = (count + 2) / 3;
        var shortAnswer = (count + 1) / 3;
        var multipleChoice = count - numeric - shortAnswer;
        return
        [
            new(AssessmentItemType.Numeric, numeric),
            new(AssessmentItemType.ShortAnswer, shortAnswer),
            new(AssessmentItemType.MultipleChoice, multipleChoice)
        ];
    }
}
