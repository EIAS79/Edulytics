using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Edulytics.Core.AssessmentIntelligence;
using Edulytics.Core.Entities;
using Edulytics.Core.Enums;
using Edulytics.Core.MathematicsGeneration;

namespace Edulytics.Services.MathematicsGeneration;

public sealed class MathematicsQuestionGenerationEngine
{
    public const string GeneratorVersion = "phase33-v1";
    private const string GenerationMethod = "deterministic-reviewed-family";
    private const int MaxExposureRetries = 128;

    public MathematicsGenerationBatch Generate(MathematicsGenerationRequest request)
    {
        ValidateRequest(request);

        var blueprint = request.Blueprint;
        var profiles = request.OutcomeProfiles
            .GroupBy(x => x.LearningOutcomeId)
            .ToDictionary(
                x => x.Key,
                x => x.Single());
        var outcomes = Expand(
            blueprint.OutcomeAllocations,
            x => x.LearningOutcomeId,
            x => x.ItemCount);
        var difficulties = Expand(
            blueprint.DifficultyAllocations,
            x => x.Difficulty,
            x => x.ItemCount);
        var blueprintFamilies = Expand(
            blueprint.QuestionFamilyAllocations,
            x => x.Family,
            x => x.ItemCount);
        var itemTypes = Expand(
            blueprint.ItemTypeAllocations,
            x => x.ItemType,
            x => x.ItemCount);
        var excluded = blueprint.ExcludedExposureFingerprints
            .ToHashSet(StringComparer.Ordinal);
        var generatedFingerprints = new HashSet<string>(StringComparer.Ordinal);
        var items = new List<GeneratedMathematicsItem>(blueprint.QuestionCount);

        for (var index = 0; index < blueprint.QuestionCount; index++)
        {
            var outcomeId = outcomes[index];
            var profile = profiles[outcomeId];
            var blueprintFamily = blueprintFamilies[index];
            var difficulty = difficulties[index];
            var itemType = itemTypes[index];
            var generatorFamily = SelectGeneratorFamily(
                profile,
                blueprintFamily,
                request.Seed,
                index);

            GeneratedMathematicsItem? accepted = null;
            for (var retry = 0; retry < MaxExposureRetries; retry++)
            {
                var candidate = BuildCandidate(
                    blueprint,
                    outcomeId,
                    profile,
                    blueprintFamily,
                    generatorFamily,
                    difficulty,
                    itemType,
                    request.Seed,
                    index,
                    retry);

                ValidateCandidate(candidate, profile, blueprint);

                var fingerprint = candidate.Item.ExposureFingerprint;
                if (excluded.Contains(fingerprint) ||
                    !generatedFingerprints.Add(fingerprint))
                {
                    continue;
                }

                accepted = candidate;
                break;
            }

            if (accepted is null)
            {
                throw new InvalidOperationException(
                    "Unable to generate a non-exposed Mathematics item within the deterministic retry budget.");
            }

            items.Add(accepted);
        }

        if (items.Count != blueprint.QuestionCount)
        {
            throw new InvalidOperationException(
                "Generated Mathematics item count does not match the assessment blueprint.");
        }

        return new MathematicsGenerationBatch(
            blueprint.SchoolId,
            blueprint.CurriculumAdoptionId,
            blueprint.CurriculumLevelKey,
            items,
            GeneratorVersion);
    }

    private static void ValidateRequest(MathematicsGenerationRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(request.Blueprint);
        ArgumentNullException.ThrowIfNull(request.OutcomeProfiles);

        var blueprint = request.Blueprint;
        if (blueprint.SchoolId == Guid.Empty ||
            blueprint.CurriculumAdoptionId == Guid.Empty ||
            string.IsNullOrWhiteSpace(blueprint.CurriculumLevelKey) ||
            blueprint.QuestionCount is < 1 or > 100)
        {
            throw new InvalidOperationException(
                "Mathematics generation requires an explicit valid assessment blueprint scope.");
        }

        EnsureAllocationTotal(
            blueprint.OutcomeAllocations.Sum(x => x.ItemCount),
            blueprint.QuestionCount,
            "Outcome");
        EnsureAllocationTotal(
            blueprint.DifficultyAllocations.Sum(x => x.ItemCount),
            blueprint.QuestionCount,
            "Difficulty");
        EnsureAllocationTotal(
            blueprint.QuestionFamilyAllocations.Sum(x => x.ItemCount),
            blueprint.QuestionCount,
            "Question family");
        EnsureAllocationTotal(
            blueprint.ItemTypeAllocations.Sum(x => x.ItemCount),
            blueprint.QuestionCount,
            "Item type");

        var duplicateProfiles = request.OutcomeProfiles
            .GroupBy(x => x.LearningOutcomeId)
            .Where(x => x.Count() != 1)
            .Select(x => x.Key)
            .ToArray();
        if (duplicateProfiles.Length != 0)
        {
            throw new InvalidOperationException(
                "Mathematics generation requires exactly one trusted profile per Learning Outcome.");
        }

        foreach (var allocation in blueprint.OutcomeAllocations.Where(x => x.ItemCount > 0))
        {
            var profile = request.OutcomeProfiles
                .SingleOrDefault(x => x.LearningOutcomeId == allocation.LearningOutcomeId)
                ?? throw new InvalidOperationException(
                    "Assessment blueprint contains an Outcome without a trusted Mathematics generation profile.");

            if (string.IsNullOrWhiteSpace(profile.OutcomeCode) ||
                profile.AllowedFamilies is null ||
                profile.AllowedFamilies.Count == 0 ||
                profile.AllowedFamilies.Distinct().Count() != profile.AllowedFamilies.Count)
            {
                throw new InvalidOperationException(
                    "Trusted Mathematics generation profile is incomplete or ambiguous.");
            }
        }
    }

    private static void EnsureAllocationTotal(int actual, int expected, string name)
    {
        if (actual != expected)
        {
            throw new InvalidOperationException(
                $"{name} allocation does not match the blueprint question count.");
        }
    }

    private static IReadOnlyList<TValue> Expand<TAllocation, TValue>(
        IReadOnlyList<TAllocation> allocations,
        Func<TAllocation, TValue> value,
        Func<TAllocation, int> count)
    {
        var result = new List<TValue>();
        foreach (var allocation in allocations)
        {
            var amount = count(allocation);
            if (amount < 0)
            {
                throw new InvalidOperationException(
                    "Blueprint allocations cannot contain negative item counts.");
            }

            for (var i = 0; i < amount; i++)
            {
                result.Add(value(allocation));
            }
        }

        return result;
    }

    private static MathematicsGeneratorFamily SelectGeneratorFamily(
        MathematicsOutcomeGenerationProfile profile,
        AssessmentQuestionFamily blueprintFamily,
        int seed,
        int index)
    {
        var eligible = profile.AllowedFamilies
            .Where(x => SupportsBlueprintFamily(x, blueprintFamily))
            .OrderBy(x => x)
            .ToArray();

        if (eligible.Length == 0)
        {
            throw new InvalidOperationException(
                $"Outcome {profile.OutcomeCode} has no trusted generator family for blueprint family {blueprintFamily}.");
        }

        var selected = StableInt(
            $"family|{profile.LearningOutcomeId:N}|{blueprintFamily}|{seed}|{index}",
            eligible.Length);
        return eligible[selected];
    }

    private static bool SupportsBlueprintFamily(
        MathematicsGeneratorFamily family,
        AssessmentQuestionFamily blueprintFamily) =>
        family switch
        {
            MathematicsGeneratorFamily.IntegerComputation =>
                blueprintFamily is AssessmentQuestionFamily.DirectComputation,
            MathematicsGeneratorFamily.OneStepEquation =>
                blueprintFamily is AssessmentQuestionFamily.StructuredMethod or
                    AssessmentQuestionFamily.MathematicalReasoning,
            MathematicsGeneratorFamily.FractionOfQuantity =>
                blueprintFamily is AssessmentQuestionFamily.DirectComputation or
                    AssessmentQuestionFamily.StructuredMethod or
                    AssessmentQuestionFamily.MathematicalReasoning,
            MathematicsGeneratorFamily.PercentageOfQuantity =>
                blueprintFamily is AssessmentQuestionFamily.DirectComputation or
                    AssessmentQuestionFamily.AppliedProblem,
            MathematicsGeneratorFamily.UnitRateWordProblem =>
                blueprintFamily is AssessmentQuestionFamily.AppliedProblem,
            _ => false
        };

    private static GeneratedMathematicsItem BuildCandidate(
        AssessmentBlueprint blueprint,
        Guid outcomeId,
        MathematicsOutcomeGenerationProfile profile,
        AssessmentQuestionFamily blueprintFamily,
        MathematicsGeneratorFamily family,
        AssessmentItemDifficulty difficulty,
        AssessmentItemType itemType,
        int seed,
        int index,
        int retry)
    {
        var key = string.Join(
            '|',
            GeneratorVersion,
            blueprint.SchoolId.ToString("N"),
            blueprint.CurriculumAdoptionId.ToString("N"),
            blueprint.CurriculumLevelKey,
            outcomeId.ToString("N"),
            family,
            difficulty,
            itemType,
            seed,
            index,
            retry);

        var raw = family switch
        {
            MathematicsGeneratorFamily.IntegerComputation =>
                GenerateIntegerComputation(key, difficulty),
            MathematicsGeneratorFamily.OneStepEquation =>
                GenerateOneStepEquation(key, difficulty),
            MathematicsGeneratorFamily.FractionOfQuantity =>
                GenerateFractionOfQuantity(key, difficulty),
            MathematicsGeneratorFamily.PercentageOfQuantity =>
                GeneratePercentageOfQuantity(key, difficulty),
            MathematicsGeneratorFamily.UnitRateWordProblem =>
                GenerateUnitRateWordProblem(key, difficulty),
            _ => throw new InvalidOperationException(
                "Unsupported Mathematics generator family.")
        };

        var prompt = FormatPrompt(raw.Prompt, raw.Answer, itemType, key);
        var fingerprint = Fingerprint(
            blueprint,
            outcomeId,
            family,
            difficulty,
            itemType,
            raw.ParametersJson,
            prompt,
            raw.Answer);

        var item = new AssessmentItem
        {
            Id = Guid.NewGuid(),
            SchoolId = blueprint.SchoolId,
            CurriculumAdoptionId = blueprint.CurriculumAdoptionId,
            CurriculumPedagogicalLessonId = blueprint.CurriculumPedagogicalLessonId,
            CurriculumTopicId = blueprint.CurriculumTopicId,
            Source = AssessmentItemSource.SystemGenerated,
            ItemType = itemType,
            Difficulty = difficulty,
            Prompt = prompt,
            CorrectAnswer = raw.Answer,
            Solution = raw.Solution,
            GenerationMethod = GenerationMethod,
            GenerationFamily = family.ToString(),
            GenerationParametersJson = raw.ParametersJson,
            ExposureFingerprint = fingerprint,
            ValidationMetadataJson = JsonSerializer.Serialize(new
            {
                generatorVersion = GeneratorVersion,
                blueprintFormulaVersion = blueprint.FormulaVersion,
                blueprintFamily = blueprintFamily.ToString(),
                outcomeCode = profile.OutcomeCode,
                scopeValidated = true,
                outcomeProfileValidated = true,
                difficultyValidated = true,
                answerValidated = true,
                solutionValidated = true
            }),
            CreatedAtUtc = DateTime.UtcNow
        };

        var outcomeLink = new AssessmentItemOutcome
        {
            Id = Guid.NewGuid(),
            SchoolId = blueprint.SchoolId,
            AssessmentItemId = item.Id,
            LearningOutcomeId = outcomeId
        };

        return new GeneratedMathematicsItem(
            item,
            outcomeLink,
            blueprintFamily,
            GeneratorVersion);
    }

    private static RawGeneratedItem GenerateIntegerComputation(
        string key,
        AssessmentItemDifficulty difficulty)
    {
        var (min, max) = difficulty switch
        {
            AssessmentItemDifficulty.Easy => (2, 20),
            AssessmentItemDifficulty.Medium => (15, 120),
            AssessmentItemDifficulty.Challenging => (80, 750),
            _ => throw new InvalidOperationException("Unsupported difficulty.")
        };
        var a = StableRange($"{key}|a", min, max);
        var b = StableRange($"{key}|b", min, max);
        var addition = StableInt($"{key}|operation", 2) == 0;
        if (!addition && b > a)
        {
            (a, b) = (b, a);
        }

        var answer = addition ? a + b : a - b;
        var operation = addition ? "+" : "−";
        var parameters = new IntegerParameters(a, b, addition ? "add" : "subtract");
        return new RawGeneratedItem(
            $"Calculate {a} {operation} {b}.",
            answer.ToString(),
            addition
                ? $"Add {a} and {b} to get {answer}."
                : $"Subtract {b} from {a} to get {answer}.",
            JsonSerializer.Serialize(parameters));
    }

    private static RawGeneratedItem GenerateOneStepEquation(
        string key,
        AssessmentItemDifficulty difficulty)
    {
        var (xMax, aMax, bMax) = difficulty switch
        {
            AssessmentItemDifficulty.Easy => (12, 5, 10),
            AssessmentItemDifficulty.Medium => (30, 9, 30),
            AssessmentItemDifficulty.Challenging => (80, 15, 80),
            _ => throw new InvalidOperationException("Unsupported difficulty.")
        };
        var x = StableRange($"{key}|x", 1, xMax);
        var a = StableRange($"{key}|a", 2, aMax);
        var b = StableRange($"{key}|b", 1, bMax);
        var c = a * x + b;
        var parameters = new EquationParameters(a, b, c);
        return new RawGeneratedItem(
            $"Solve for x: {a}x + {b} = {c}.",
            x.ToString(),
            $"Subtract {b}: {a}x = {c - b}. Divide by {a}: x = {x}.",
            JsonSerializer.Serialize(parameters));
    }

    private static RawGeneratedItem GenerateFractionOfQuantity(
        string key,
        AssessmentItemDifficulty difficulty)
    {
        var denominatorMax = difficulty switch
        {
            AssessmentItemDifficulty.Easy => 6,
            AssessmentItemDifficulty.Medium => 10,
            AssessmentItemDifficulty.Challenging => 16,
            _ => throw new InvalidOperationException("Unsupported difficulty.")
        };
        var denominator = StableRange($"{key}|d", 2, denominatorMax);
        var numerator = StableRange($"{key}|n", 1, denominator - 1);
        var multiplier = StableRange(
            $"{key}|m",
            difficulty == AssessmentItemDifficulty.Easy ? 2 : 4,
            difficulty == AssessmentItemDifficulty.Challenging ? 30 : 18);
        var quantity = denominator * multiplier;
        var answer = numerator * multiplier;
        var parameters = new FractionParameters(numerator, denominator, quantity);
        return new RawGeneratedItem(
            $"Find {numerator}/{denominator} of {quantity}.",
            answer.ToString(),
            $"One {denominator}th of {quantity} is {multiplier}; multiply by {numerator} to get {answer}.",
            JsonSerializer.Serialize(parameters));
    }

    private static RawGeneratedItem GeneratePercentageOfQuantity(
        string key,
        AssessmentItemDifficulty difficulty)
    {
        int[] percentages = difficulty switch
        {
            AssessmentItemDifficulty.Easy => [10, 25, 50],
            AssessmentItemDifficulty.Medium => [15, 20, 30, 40, 60, 75],
            AssessmentItemDifficulty.Challenging => [12, 18, 35, 45, 65, 85],
            _ => throw new InvalidOperationException("Unsupported difficulty.")
        };
        var percent = percentages[StableInt($"{key}|p", percentages.Length)];
        var unit = 100 / GreatestCommonDivisor(100, percent);
        var multiplier = StableRange(
            $"{key}|m",
            difficulty == AssessmentItemDifficulty.Easy ? 2 : 5,
            difficulty == AssessmentItemDifficulty.Challenging ? 50 : 30);
        var quantity = unit * multiplier;
        var answer = quantity * percent / 100;
        var parameters = new PercentageParameters(percent, quantity);
        return new RawGeneratedItem(
            $"What is {percent}% of {quantity}?",
            answer.ToString(),
            $"Compute {percent}/100 × {quantity} = {answer}.",
            JsonSerializer.Serialize(parameters));
    }

    private static RawGeneratedItem GenerateUnitRateWordProblem(
        string key,
        AssessmentItemDifficulty difficulty)
    {
        var scenarios = new[]
        {
            new Scenario("box", "pencils"),
            new Scenario("pack", "stickers"),
            new Scenario("tray", "cups"),
            new Scenario("bundle", "notebooks")
        };
        var scenario = scenarios[StableInt($"{key}|scenario", scenarios.Length)];
        var rateMax = difficulty switch
        {
            AssessmentItemDifficulty.Easy => 8,
            AssessmentItemDifficulty.Medium => 18,
            AssessmentItemDifficulty.Challenging => 35,
            _ => throw new InvalidOperationException("Unsupported difficulty.")
        };
        var countMax = difficulty switch
        {
            AssessmentItemDifficulty.Easy => 8,
            AssessmentItemDifficulty.Medium => 15,
            AssessmentItemDifficulty.Challenging => 25,
            _ => throw new InvalidOperationException("Unsupported difficulty.")
        };
        var rate = StableRange($"{key}|rate", 2, rateMax);
        var count = StableRange($"{key}|count", 2, countMax);
        var answer = rate * count;
        var parameters = new UnitRateParameters(
            scenario.Container,
            scenario.Object,
            rate,
            count);
        return new RawGeneratedItem(
            $"Each {scenario.Container} holds {rate} {scenario.Object}. How many {scenario.Object} are in {count} {scenario.Container}s?",
            answer.ToString(),
            $"Multiply the unit rate by the number of {scenario.Container}s: {rate} × {count} = {answer}.",
            JsonSerializer.Serialize(parameters));
    }

    private static string FormatPrompt(
        string prompt,
        string answer,
        AssessmentItemType itemType,
        string key)
    {
        return itemType switch
        {
            AssessmentItemType.Numeric => prompt,
            AssessmentItemType.ShortAnswer => $"{prompt} Give your answer as a number.",
            AssessmentItemType.MultipleChoice => BuildMultipleChoicePrompt(prompt, answer, key),
            _ => throw new InvalidOperationException("Unsupported assessment item type.")
        };
    }

    private static string BuildMultipleChoicePrompt(string prompt, string answer, string key)
    {
        if (!int.TryParse(answer, out var correct))
        {
            throw new InvalidOperationException(
                "Reviewed Mathematics multiple-choice families require an integer answer.");
        }

        var delta = StableRange($"{key}|delta", 1, 5);
        var choices = new HashSet<int> { correct };
        for (var i = 1; choices.Count < 4; i++)
        {
            choices.Add(Math.Max(0, correct + delta * i));
            if (choices.Count < 4)
            {
                choices.Add(Math.Max(0, correct - delta * i));
            }
        }

        var ordered = choices
            .OrderBy(x => StableInt($"{key}|choice|{x}", int.MaxValue))
            .Take(4)
            .ToArray();
        var labels = new[] { "A", "B", "C", "D" };
        var options = string.Join(
            "  ",
            ordered.Select((x, i) => $"{labels[i]}) {x}"));
        return $"{prompt} {options}";
    }

    private static void ValidateCandidate(
        GeneratedMathematicsItem candidate,
        MathematicsOutcomeGenerationProfile profile,
        AssessmentBlueprint blueprint)
    {
        var item = candidate.Item;
        if (item.SchoolId != blueprint.SchoolId ||
            item.CurriculumAdoptionId != blueprint.CurriculumAdoptionId ||
            candidate.OutcomeLink.SchoolId != blueprint.SchoolId ||
            candidate.OutcomeLink.AssessmentItemId != item.Id ||
            candidate.OutcomeLink.LearningOutcomeId != profile.LearningOutcomeId)
        {
            throw new InvalidOperationException(
                "Generated Mathematics item violates school, curriculum or Outcome scope.");
        }

        if (item.Source != AssessmentItemSource.SystemGenerated ||
            !string.Equals(item.GenerationMethod, GenerationMethod, StringComparison.Ordinal) ||
            !Enum.TryParse<MathematicsGeneratorFamily>(item.GenerationFamily, out var family) ||
            !profile.AllowedFamilies.Contains(family) ||
            !SupportsBlueprintFamily(family, candidate.BlueprintFamily))
        {
            throw new InvalidOperationException(
                "Generated Mathematics item has an invalid or untrusted generation family.");
        }

        if (string.IsNullOrWhiteSpace(item.Prompt) ||
            string.IsNullOrWhiteSpace(item.CorrectAnswer) ||
            string.IsNullOrWhiteSpace(item.Solution) ||
            string.IsNullOrWhiteSpace(item.GenerationParametersJson) ||
            string.IsNullOrWhiteSpace(item.ValidationMetadataJson) ||
            string.IsNullOrWhiteSpace(item.ExposureFingerprint))
        {
            throw new InvalidOperationException(
                "Generated Mathematics item is not reconstructable.");
        }

        var expected = RecalculateAnswer(family, item.GenerationParametersJson);
        if (!string.Equals(expected, item.CorrectAnswer, StringComparison.Ordinal))
        {
            throw new InvalidOperationException(
                "Generated Mathematics item failed answer validation.");
        }

        if (!item.Solution.Contains(expected, StringComparison.Ordinal))
        {
            throw new InvalidOperationException(
                "Generated Mathematics item failed solution validation.");
        }

        ValidateDifficulty(family, item.Difficulty, item.GenerationParametersJson);

        var expectedFingerprint = Fingerprint(
            blueprint,
            profile.LearningOutcomeId,
            family,
            item.Difficulty,
            item.ItemType,
            item.GenerationParametersJson,
            item.Prompt,
            item.CorrectAnswer);
        if (!string.Equals(expectedFingerprint, item.ExposureFingerprint, StringComparison.Ordinal))
        {
            throw new InvalidOperationException(
                "Generated Mathematics item failed exposure fingerprint validation.");
        }
    }

    private static string RecalculateAnswer(
        MathematicsGeneratorFamily family,
        string parametersJson) =>
        family switch
        {
            MathematicsGeneratorFamily.IntegerComputation =>
                Answer(JsonSerializer.Deserialize<IntegerParameters>(parametersJson)),
            MathematicsGeneratorFamily.OneStepEquation =>
                Answer(JsonSerializer.Deserialize<EquationParameters>(parametersJson)),
            MathematicsGeneratorFamily.FractionOfQuantity =>
                Answer(JsonSerializer.Deserialize<FractionParameters>(parametersJson)),
            MathematicsGeneratorFamily.PercentageOfQuantity =>
                Answer(JsonSerializer.Deserialize<PercentageParameters>(parametersJson)),
            MathematicsGeneratorFamily.UnitRateWordProblem =>
                Answer(JsonSerializer.Deserialize<UnitRateParameters>(parametersJson)),
            _ => throw new InvalidOperationException("Unsupported Mathematics generator family.")
        };

    private static string Answer(IntegerParameters? p)
    {
        if (p is null || (p.Operation != "add" && p.Operation != "subtract"))
            throw new InvalidOperationException("Invalid integer generation parameters.");
        return (p.Operation == "add" ? p.A + p.B : p.A - p.B).ToString();
    }

    private static string Answer(EquationParameters? p)
    {
        if (p is null || p.A == 0 || (p.C - p.B) % p.A != 0)
            throw new InvalidOperationException("Invalid equation generation parameters.");
        return ((p.C - p.B) / p.A).ToString();
    }

    private static string Answer(FractionParameters? p)
    {
        if (p is null || p.Denominator <= 0 || p.Quantity % p.Denominator != 0)
            throw new InvalidOperationException("Invalid fraction generation parameters.");
        return (p.Quantity / p.Denominator * p.Numerator).ToString();
    }

    private static string Answer(PercentageParameters? p)
    {
        if (p is null || p.Percent <= 0 || p.Percent >= 100 || p.Quantity * p.Percent % 100 != 0)
            throw new InvalidOperationException("Invalid percentage generation parameters.");
        return (p.Quantity * p.Percent / 100).ToString();
    }

    private static string Answer(UnitRateParameters? p)
    {
        if (p is null || p.Rate <= 0 || p.Count <= 0 ||
            string.IsNullOrWhiteSpace(p.Container) || string.IsNullOrWhiteSpace(p.Object))
            throw new InvalidOperationException("Invalid unit-rate generation parameters.");
        return (p.Rate * p.Count).ToString();
    }

    private static void ValidateDifficulty(
        MathematicsGeneratorFamily family,
        AssessmentItemDifficulty difficulty,
        string parametersJson)
    {
        var valid = family switch
        {
            MathematicsGeneratorFamily.IntegerComputation =>
                ValidateIntegerDifficulty(JsonSerializer.Deserialize<IntegerParameters>(parametersJson), difficulty),
            MathematicsGeneratorFamily.OneStepEquation =>
                ValidateEquationDifficulty(JsonSerializer.Deserialize<EquationParameters>(parametersJson), difficulty),
            MathematicsGeneratorFamily.FractionOfQuantity =>
                ValidateFractionDifficulty(JsonSerializer.Deserialize<FractionParameters>(parametersJson), difficulty),
            MathematicsGeneratorFamily.PercentageOfQuantity =>
                ValidatePercentageDifficulty(JsonSerializer.Deserialize<PercentageParameters>(parametersJson), difficulty),
            MathematicsGeneratorFamily.UnitRateWordProblem =>
                ValidateUnitRateDifficulty(JsonSerializer.Deserialize<UnitRateParameters>(parametersJson), difficulty),
            _ => false
        };

        if (!valid)
        {
            throw new InvalidOperationException(
                "Generated Mathematics item failed difficulty validation.");
        }
    }

    private static bool ValidateIntegerDifficulty(IntegerParameters? p, AssessmentItemDifficulty difficulty)
    {
        if (p is null) return false;
        var max = Math.Max(p.A, p.B);
        return difficulty switch
        {
            AssessmentItemDifficulty.Easy => max <= 20,
            AssessmentItemDifficulty.Medium => max is >= 15 and <= 120,
            AssessmentItemDifficulty.Challenging => max is >= 80 and <= 750,
            _ => false
        };
    }

    private static bool ValidateEquationDifficulty(EquationParameters? p, AssessmentItemDifficulty difficulty)
    {
        if (p is null || p.A <= 0) return false;
        var x = (p.C - p.B) / p.A;
        return difficulty switch
        {
            AssessmentItemDifficulty.Easy => x <= 12 && p.A <= 5 && p.B <= 10,
            AssessmentItemDifficulty.Medium => x <= 30 && p.A <= 9 && p.B <= 30,
            AssessmentItemDifficulty.Challenging => x <= 80 && p.A <= 15 && p.B <= 80,
            _ => false
        };
    }

    private static bool ValidateFractionDifficulty(FractionParameters? p, AssessmentItemDifficulty difficulty)
    {
        if (p is null || p.Numerator <= 0 || p.Numerator >= p.Denominator) return false;
        return difficulty switch
        {
            AssessmentItemDifficulty.Easy => p.Denominator <= 6,
            AssessmentItemDifficulty.Medium => p.Denominator <= 10,
            AssessmentItemDifficulty.Challenging => p.Denominator <= 16,
            _ => false
        };
    }

    private static bool ValidatePercentageDifficulty(PercentageParameters? p, AssessmentItemDifficulty difficulty)
    {
        if (p is null) return false;
        int[] allowed = difficulty switch
        {
            AssessmentItemDifficulty.Easy => [10, 25, 50],
            AssessmentItemDifficulty.Medium => [15, 20, 30, 40, 60, 75],
            AssessmentItemDifficulty.Challenging => [12, 18, 35, 45, 65, 85],
            _ => []
        };
        return allowed.Contains(p.Percent);
    }

    private static bool ValidateUnitRateDifficulty(UnitRateParameters? p, AssessmentItemDifficulty difficulty)
    {
        if (p is null) return false;
        return difficulty switch
        {
            AssessmentItemDifficulty.Easy => p.Rate <= 8 && p.Count <= 8,
            AssessmentItemDifficulty.Medium => p.Rate <= 18 && p.Count <= 15,
            AssessmentItemDifficulty.Challenging => p.Rate <= 35 && p.Count <= 25,
            _ => false
        };
    }

    private static string Fingerprint(
        AssessmentBlueprint blueprint,
        Guid outcomeId,
        MathematicsGeneratorFamily family,
        AssessmentItemDifficulty difficulty,
        AssessmentItemType itemType,
        string parametersJson,
        string prompt,
        string answer)
    {
        var material = string.Join(
            '|',
            GeneratorVersion,
            blueprint.SchoolId.ToString("N"),
            blueprint.CurriculumAdoptionId.ToString("N"),
            blueprint.CurriculumLevelKey.Trim(),
            blueprint.CurriculumTopicId?.ToString("N") ?? string.Empty,
            blueprint.CurriculumPedagogicalLessonId?.ToString("N") ?? string.Empty,
            outcomeId.ToString("N"),
            family,
            difficulty,
            itemType,
            parametersJson,
            prompt,
            answer);
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(material));
        return Convert.ToHexString(bytes).ToLowerInvariant();
    }

    private static int StableRange(string key, int minInclusive, int maxInclusive)
    {
        if (maxInclusive < minInclusive)
            throw new InvalidOperationException("Invalid deterministic range.");
        return minInclusive + StableInt(key, maxInclusive - minInclusive + 1);
    }

    private static int StableInt(string key, int maxExclusive)
    {
        if (maxExclusive <= 0)
            throw new InvalidOperationException("Deterministic range must be positive.");
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(key));
        var value = BitConverter.ToUInt32(hash, 0);
        return (int)(value % (uint)maxExclusive);
    }

    private static int GreatestCommonDivisor(int a, int b)
    {
        while (b != 0)
        {
            (a, b) = (b, a % b);
        }
        return Math.Abs(a);
    }

    private sealed record RawGeneratedItem(
        string Prompt,
        string Answer,
        string Solution,
        string ParametersJson);
    private sealed record IntegerParameters(int A, int B, string Operation);
    private sealed record EquationParameters(int A, int B, int C);
    private sealed record FractionParameters(int Numerator, int Denominator, int Quantity);
    private sealed record PercentageParameters(int Percent, int Quantity);
    private sealed record UnitRateParameters(string Container, string Object, int Rate, int Count);
    private sealed record Scenario(string Container, string Object);
}
