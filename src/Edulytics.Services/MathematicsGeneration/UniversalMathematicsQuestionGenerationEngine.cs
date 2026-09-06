using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Edulytics.Core.AssessmentIntelligence;
using Edulytics.Core.Entities;
using Edulytics.Core.Enums;
using Edulytics.Core.MathematicsGeneration;

namespace Edulytics.Services.MathematicsGeneration;

/// <summary>
/// Shared Mathematics generator used by Teacher Assessment Builder and Student
/// Private Practice. Reviewed native families are delegated to the Phase33 native
/// engine. Outcomes without a complete native solver are served by a deterministic
/// curriculum-contextual provider and remain explicitly AiAssisted in capability
/// metadata. This keeps broad curriculum coverage without relabelling contextual
/// questions as native verified mathematics.
/// </summary>
public sealed class UniversalMathematicsQuestionGenerationEngine
{
    public const string GeneratorVersion = "round2-universal-context-v1";
    private const string ContextualGenerationMethod = "deterministic-curriculum-contextual";
    private const string ContextualProviderKey = "edulytics-contextual-mathematics";
    private const int MaxRetries = 128;

    private readonly MathematicsQuestionGenerationEngine _native = new();

    public MathematicsGenerationBatch Generate(MathematicsGenerationRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(request.Blueprint);
        ArgumentNullException.ThrowIfNull(request.OutcomeProfiles);

        var blueprint = request.Blueprint;
        if (blueprint.QuestionCount is < 1 or > 100 ||
            blueprint.SchoolId == Guid.Empty ||
            blueprint.CurriculumAdoptionId == Guid.Empty ||
            string.IsNullOrWhiteSpace(blueprint.CurriculumLevelKey))
        {
            throw new InvalidOperationException(
                "Universal Mathematics generation requires a valid scoped blueprint.");
        }

        var profiles = request.OutcomeProfiles
            .GroupBy(x => x.LearningOutcomeId)
            .ToDictionary(x => x.Key, x => x.Single());
        if (profiles.Count != request.OutcomeProfiles.Count)
        {
            throw new InvalidOperationException(
                "Universal Mathematics generation requires one profile per Outcome.");
        }

        var outcomeIds = Expand(
            blueprint.OutcomeAllocations,
            x => x.LearningOutcomeId,
            x => x.ItemCount);
        var difficulties = Expand(
            blueprint.DifficultyAllocations,
            x => x.Difficulty,
            x => x.ItemCount);
        var questionFamilies = Expand(
            blueprint.QuestionFamilyAllocations,
            x => x.Family,
            x => x.ItemCount);
        var itemTypes = Expand(
            blueprint.ItemTypeAllocations,
            x => x.ItemType,
            x => x.ItemCount);

        if (outcomeIds.Count != blueprint.QuestionCount ||
            difficulties.Count != blueprint.QuestionCount ||
            questionFamilies.Count != blueprint.QuestionCount ||
            itemTypes.Count != blueprint.QuestionCount)
        {
            throw new InvalidOperationException(
                "Universal Mathematics blueprint allocations do not match question count.");
        }

        var excluded = blueprint.ExcludedExposureFingerprints
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .ToHashSet(StringComparer.Ordinal);
        var generated = new HashSet<string>(StringComparer.Ordinal);
        var items = new List<GeneratedMathematicsItem>(blueprint.QuestionCount);

        for (var index = 0; index < blueprint.QuestionCount; index++)
        {
            var outcomeId = outcomeIds[index];
            if (!profiles.TryGetValue(outcomeId, out var profile))
            {
                throw new InvalidOperationException(
                    "Universal Mathematics blueprint contains an Outcome without a generation profile.");
            }

            var difficulty = difficulties[index];
            var blueprintFamily = questionFamilies[index];
            var itemType = itemTypes[index];

            GeneratedMathematicsItem? candidate = null;
            if (!profile.IsContextualAssisted &&
                profile.AllowedFamilies.Count > 0 &&
                profile.AllowedFamilies.All(x => x != MathematicsGeneratorFamily.CurriculumContextCheck))
            {
                candidate = TryNative(
                    blueprint,
                    profile,
                    difficulty,
                    blueprintFamily,
                    itemType,
                    request.Seed,
                    index,
                    excluded.Concat(generated).ToArray());
            }

            candidate ??= GenerateContextualUnique(
                blueprint,
                profile,
                difficulty,
                blueprintFamily,
                itemType,
                request.Seed,
                index,
                excluded,
                generated);

            if (!generated.Add(candidate.Item.ExposureFingerprint))
            {
                throw new InvalidOperationException(
                    "Universal Mathematics generation produced a duplicate exposure fingerprint.");
            }

            items.Add(candidate);
        }

        return new MathematicsGenerationBatch(
            blueprint.SchoolId,
            blueprint.CurriculumAdoptionId,
            blueprint.CurriculumLevelKey,
            items,
            GeneratorVersion);
    }

    private GeneratedMathematicsItem? TryNative(
        AssessmentBlueprint blueprint,
        MathematicsOutcomeGenerationProfile profile,
        AssessmentItemDifficulty difficulty,
        AssessmentQuestionFamily blueprintFamily,
        AssessmentItemType itemType,
        int seed,
        int index,
        IReadOnlyList<string> excluded)
    {
        var allocation = blueprint.OutcomeAllocations
            .FirstOrDefault(x => x.LearningOutcomeId == profile.LearningOutcomeId);
        var nativeBlueprint = new AssessmentBlueprint(
            blueprint.SchoolId,
            blueprint.CurriculumAdoptionId,
            blueprint.CurriculumLevelKey,
            blueprint.CurriculumTopicId,
            blueprint.CurriculumPedagogicalLessonId,
            blueprint.Purpose,
            1,
            [new OutcomeBlueprintAllocation(
                profile.LearningOutcomeId,
                1,
                allocation?.PriorityScore ?? 1m,
                allocation?.PriorityReason ?? "UniversalNativeDelegation")],
            [new DifficultyBlueprintAllocation(difficulty, 1)],
            [new QuestionFamilyBlueprintAllocation(blueprintFamily, 1)],
            [new ItemTypeBlueprintAllocation(itemType, 1)],
            [new OutcomeEvidenceRequirement(profile.LearningOutcomeId, 1, true, true)],
            excluded,
            blueprint.FormulaVersion);

        try
        {
            return _native.Generate(new MathematicsGenerationRequest(
                    nativeBlueprint,
                    [profile],
                    StableInt($"native|{seed}|{index}", int.MaxValue - 1) + 1))
                .Items
                .Single();
        }
        catch (InvalidOperationException)
        {
            // A native profile can still be incompatible with a specific blueprint
            // family/item shape. Universal generation falls back to the contextual
            // provider instead of failing the entire assessment/practice request.
            return null;
        }
    }

    private static GeneratedMathematicsItem GenerateContextualUnique(
        AssessmentBlueprint blueprint,
        MathematicsOutcomeGenerationProfile profile,
        AssessmentItemDifficulty difficulty,
        AssessmentQuestionFamily blueprintFamily,
        AssessmentItemType itemType,
        int seed,
        int index,
        IReadOnlySet<string> excluded,
        IReadOnlySet<string> generated)
    {
        var context = CleanContext(profile.GenerationContext, profile.OutcomeCode);
        if (string.IsNullOrWhiteSpace(context))
        {
            throw new InvalidOperationException(
                "Contextual Mathematics generation requires curriculum context.");
        }

        for (var retry = 0; retry < MaxRetries; retry++)
        {
            var key = string.Join(
                '|',
                GeneratorVersion,
                blueprint.SchoolId.ToString("N"),
                blueprint.CurriculumAdoptionId.ToString("N"),
                blueprint.CurriculumLevelKey,
                profile.LearningOutcomeId.ToString("N"),
                difficulty,
                blueprintFamily,
                itemType,
                seed,
                index,
                retry);

            var raw = BuildContextualRaw(context, difficulty, key);
            ValidateRaw(raw);
            var prompt = FormatPrompt(raw.Prompt, raw.Answer, itemType, key);
            var fingerprint = Fingerprint(
                blueprint,
                profile.LearningOutcomeId,
                difficulty,
                itemType,
                raw.ParametersJson,
                prompt,
                raw.Answer);
            if (excluded.Contains(fingerprint) || generated.Contains(fingerprint))
                continue;

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
                GenerationMethod = ContextualGenerationMethod,
                GenerationFamily = MathematicsGeneratorFamily.CurriculumContextCheck.ToString(),
                GenerationParametersJson = raw.ParametersJson,
                ExposureFingerprint = fingerprint,
                ValidationMetadataJson = JsonSerializer.Serialize(new
                {
                    generatorVersion = GeneratorVersion,
                    provider = ContextualProviderKey,
                    capabilityLevel = "AiAssisted",
                    contextualTopic = raw.Topic,
                    outcomeCode = profile.OutcomeCode,
                    blueprintFamily = blueprintFamily.ToString(),
                    scopeValidated = true,
                    answerValidated = true,
                    solutionValidated = true,
                    reconstructable = true
                }),
                CreatedAtUtc = DateTime.UtcNow
            };

            var outcomeLink = new AssessmentItemOutcome
            {
                Id = Guid.NewGuid(),
                SchoolId = blueprint.SchoolId,
                AssessmentItemId = item.Id,
                LearningOutcomeId = profile.LearningOutcomeId
            };

            return new GeneratedMathematicsItem(
                item,
                outcomeLink,
                blueprintFamily,
                GeneratorVersion);
        }

        throw new InvalidOperationException(
            "Unable to generate a unique curriculum-contextual Mathematics item.");
    }

    private static ContextualRaw BuildContextualRaw(
        string context,
        AssessmentItemDifficulty difficulty,
        string key)
    {
        var topic = ResolveTopic(context);
        var scale = difficulty switch
        {
            AssessmentItemDifficulty.Easy => 1,
            AssessmentItemDifficulty.Medium => 2,
            AssessmentItemDifficulty.Challenging => 4,
            _ => throw new InvalidOperationException("Unsupported contextual difficulty.")
        };

        return topic switch
        {
            "fraction" => FractionRaw(key, scale),
            "percentage" => PercentageRaw(key, scale),
            "ratio-rate" => RateRaw(key, scale),
            "equation" => EquationRaw(key, scale),
            "geometry" => GeometryRaw(key, scale),
            "angle" => AngleRaw(key, scale),
            "statistics" => StatisticsRaw(key, scale),
            "probability" => ProbabilityRaw(key, scale),
            "exponent" => ExponentRaw(key, scale),
            "root" => RootRaw(key, scale),
            "sequence" => SequenceRaw(key, scale),
            "function" => FunctionRaw(key, scale),
            "slope" => SlopeRaw(key, scale),
            "pythagorean" => PythagoreanRaw(key, scale),
            "vector" => VectorRaw(key, scale),
            "matrix" => MatrixRaw(key, scale),
            "logarithm" => LogarithmRaw(key, scale),
            "derivative" => DerivativeRaw(key, scale),
            "integral" => IntegralRaw(key, scale),
            "measurement" => MeasurementRaw(key, scale),
            "decimal" => DecimalRaw(key, scale),
            "multiplication" => MultiplicationRaw(key, scale),
            "division" => DivisionRaw(key, scale),
            "subtraction" => SubtractionRaw(key, scale),
            "addition" => AdditionRaw(key, scale),
            _ => GeneralReasoningRaw(key, scale)
        };
    }

    private static ContextualRaw AdditionRaw(string key, int scale)
    {
        var a = StableRange($"{key}|a", 2 * scale, 12 * scale + 8);
        var b = StableRange($"{key}|b", 2 * scale, 10 * scale + 7);
        return Raw("addition", $"Calculate {a} + {b}.", a + b,
            $"Add {a} and {b} to get {a + b}.", "add", a, b);
    }

    private static ContextualRaw SubtractionRaw(string key, int scale)
    {
        var b = StableRange($"{key}|b", scale, 8 * scale + 5);
        var answer = StableRange($"{key}|answer", scale, 10 * scale + 6);
        var a = b + answer;
        return Raw("subtraction", $"Calculate {a} − {b}.", answer,
            $"Subtract {b} from {a} to get {answer}.", "subtract", a, b);
    }

    private static ContextualRaw MultiplicationRaw(string key, int scale)
    {
        var a = StableRange($"{key}|a", 2, 6 + 2 * scale);
        var b = StableRange($"{key}|b", 2, 7 + 2 * scale);
        return Raw("multiplication", $"Calculate {a} × {b}.", a * b,
            $"Multiply {a} by {b}: {a} × {b} = {a * b}.", "multiply", a, b);
    }

    private static ContextualRaw DivisionRaw(string key, int scale)
    {
        var divisor = StableRange($"{key}|divisor", 2, 5 + scale);
        var quotient = StableRange($"{key}|quotient", 2, 7 + 2 * scale);
        var dividend = divisor * quotient;
        return Raw("division", $"Calculate {dividend} ÷ {divisor}.", quotient,
            $"{dividend} divided by {divisor} equals {quotient}.", "divide", dividend, divisor);
    }

    private static ContextualRaw FractionRaw(string key, int scale)
    {
        var denominator = StableRange($"{key}|d", 2, Math.Min(12, 4 + 2 * scale));
        var numerator = StableRange($"{key}|n", 1, denominator - 1);
        var unit = StableRange($"{key}|u", 2, 5 + 2 * scale);
        var quantity = denominator * unit;
        var answer = numerator * unit;
        return Raw("fraction", $"Find {numerator}/{denominator} of {quantity}.", answer,
            $"One {denominator}th of {quantity} is {unit}; multiply by {numerator} to get {answer}.",
            "fraction", numerator, denominator, quantity);
    }

    private static ContextualRaw PercentageRaw(string key, int scale)
    {
        int[] values = [10, 20, 25, 50, 75];
        var percent = values[StableInt($"{key}|p", values.Length)];
        var baseUnit = 100 / GreatestCommonDivisor(100, percent);
        var multiplier = StableRange($"{key}|m", 2, 5 + 2 * scale);
        var quantity = baseUnit * multiplier;
        var answer = quantity * percent / 100;
        return Raw("percentage", $"What is {percent}% of {quantity}?", answer,
            $"Compute {percent}/100 × {quantity} = {answer}.", "percent", percent, quantity);
    }

    private static ContextualRaw RateRaw(string key, int scale)
    {
        var rate = StableRange($"{key}|rate", 2, 5 + 2 * scale);
        var count = StableRange($"{key}|count", 2, 5 + 2 * scale);
        var answer = rate * count;
        return Raw("ratio-rate",
            $"Each pack contains {rate} items. How many items are in {count} packs?",
            answer,
            $"Use the unit rate: {rate} × {count} = {answer}.",
            "multiply", rate, count);
    }

    private static ContextualRaw EquationRaw(string key, int scale)
    {
        var multiplicative = StableInt($"{key}|kind", 2) == 1;
        var x = StableRange($"{key}|x", 2, 7 + 3 * scale);
        if (multiplicative)
        {
            var coefficient = StableRange($"{key}|coef", 2, 4 + scale);
            var result = coefficient * x;
            return Raw("equation", $"Solve for x: {coefficient}x = {result}.", x,
                $"Divide both sides by {coefficient}: x = {result} ÷ {coefficient} = {x}.",
                "equation-multiply", coefficient, result);
        }

        var addend = StableRange($"{key}|addend", 1, 5 + 2 * scale);
        var total = x + addend;
        return Raw("equation", $"Solve for x: x + {addend} = {total}.", x,
            $"Subtract {addend} from both sides: x = {total} − {addend} = {x}.",
            "equation-add", addend, total);
    }

    private static ContextualRaw GeometryRaw(string key, int scale)
    {
        var width = StableRange($"{key}|w", 2, 5 + scale);
        var length = StableRange($"{key}|l", width + 1, width + 4 + scale);
        var area = StableInt($"{key}|kind", 2) == 0;
        if (area)
        {
            var answer = width * length;
            return Raw("geometry", $"A rectangle is {length} units long and {width} units wide. Find its area.", answer,
                $"Area = length × width = {length} × {width} = {answer} square units.",
                "multiply", length, width);
        }

        var perimeter = 2 * (width + length);
        return Raw("geometry", $"A rectangle is {length} units long and {width} units wide. Find its perimeter.", perimeter,
            $"Perimeter = 2 × ({length} + {width}) = {perimeter} units.",
            "perimeter", length, width);
    }

    private static ContextualRaw AngleRaw(string key, int scale)
    {
        var supplement = StableInt($"{key}|kind", 2) == 1;
        var step = 5 * Math.Max(1, scale);
        var angle = supplement
            ? StableRange($"{key}|a", 4, 12) * step
            : StableRange($"{key}|a", 2, Math.Max(3, 8 / Math.Max(1, scale))) * step;
        if (!supplement && angle >= 90) angle = 45;
        if (supplement && angle >= 180) angle = 120;
        var total = supplement ? 180 : 90;
        var answer = total - angle;
        return Raw("angle",
            $"An angle is {angle}°. What angle is needed to make {(supplement ? "a straight angle" : "a right angle")}?",
            answer,
            $"Subtract from {total}°: {total} − {angle} = {answer}°.",
            supplement ? "supplement" : "complement", angle);
    }

    private static ContextualRaw StatisticsRaw(string key, int scale)
    {
        var mean = StableRange($"{key}|mean", 4, 8 + 3 * scale);
        var delta = StableRange($"{key}|delta", 1, Math.Max(1, 2 * scale));
        var a = mean - delta;
        var b = mean;
        var c = mean + delta;
        return Raw("statistics", $"Find the mean of {a}, {b}, and {c}.", mean,
            $"Add the values and divide by 3: ({a} + {b} + {c}) ÷ 3 = {mean}.",
            "mean3", a, b, c);
    }

    private static ContextualRaw ProbabilityRaw(string key, int scale)
    {
        int[] totals = [10, 20, 25, 50];
        var total = totals[StableInt($"{key}|total", totals.Length)];
        var favorable = total / StableRange($"{key}|factor", 2, 5);
        if (favorable < 1) favorable = 1;
        var answer = favorable * 100 / total;
        return Raw("probability",
            $"There are {total} equally likely outcomes and {favorable} are favorable. What percentage of the outcomes are favorable?",
            answer,
            $"Probability percentage = {favorable}/{total} × 100 = {answer}%.",
            "probability-percent", favorable, total);
    }

    private static ContextualRaw ExponentRaw(string key, int scale)
    {
        var baseValue = StableRange($"{key}|base", 2, Math.Min(5, 2 + scale));
        var exponent = StableRange($"{key}|exp", 2, Math.Min(4, 2 + scale));
        var answer = IntPow(baseValue, exponent);
        return Raw("exponent", $"Calculate {baseValue}^{exponent}.", answer,
            $"Multiply {baseValue} by itself {exponent} times to get {answer}.",
            "power", baseValue, exponent);
    }

    private static ContextualRaw RootRaw(string key, int scale)
    {
        var root = StableRange($"{key}|root", 2, 6 + scale);
        var square = root * root;
        return Raw("root", $"Find √{square}.", root,
            $"{root} × {root} = {square}, so √{square} = {root}.",
            "root", square);
    }

    private static ContextualRaw SequenceRaw(string key, int scale)
    {
        var start = StableRange($"{key}|start", 1, 5 + scale);
        var step = StableRange($"{key}|step", 2, 4 + scale);
        var a2 = start + step;
        var a3 = start + 2 * step;
        var answer = start + 3 * step;
        return Raw("sequence", $"Find the next term: {start}, {a2}, {a3}, ...", answer,
            $"The common difference is {step}, so the next term is {a3} + {step} = {answer}.",
            "sequence-next", start, step);
    }

    private static ContextualRaw FunctionRaw(string key, int scale)
    {
        var coefficient = StableRange($"{key}|a", 2, 3 + scale);
        var intercept = StableRange($"{key}|b", 1, 4 + scale);
        var x = StableRange($"{key}|x", 2, 4 + scale);
        var answer = coefficient * x + intercept;
        return Raw("function", $"If f(x) = {coefficient}x + {intercept}, find f({x}).", answer,
            $"Substitute x = {x}: f({x}) = {coefficient} × {x} + {intercept} = {answer}.",
            "function", coefficient, intercept, x);
    }

    private static ContextualRaw SlopeRaw(string key, int scale)
    {
        var run = StableRange($"{key}|run", 1, 2 + scale);
        var slope = StableRange($"{key}|m", 1, 3 + scale);
        var rise = run * slope;
        return Raw("slope", $"A line rises {rise} units while running {run} units to the right. Find its slope.", slope,
            $"Slope = rise/run = {rise}/{run} = {slope}.",
            "slope", rise, run);
    }

    private static ContextualRaw PythagoreanRaw(string key, int scale)
    {
        var multiplier = StableRange($"{key}|m", 1, Math.Max(1, scale));
        var a = 3 * multiplier;
        var b = 4 * multiplier;
        var c = 5 * multiplier;
        return Raw("pythagorean", $"A right triangle has legs {a} and {b}. Find the hypotenuse.", c,
            $"Use a² + b² = c²: {a * a} + {b * b} = {c * c}, so c = {c}.",
            "pythagorean", a, b);
    }

    private static ContextualRaw VectorRaw(string key, int scale)
    {
        var multiplier = StableRange($"{key}|m", 1, Math.Max(1, scale));
        var a = 3 * multiplier;
        var b = 4 * multiplier;
        var answer = 5 * multiplier;
        return Raw("vector", $"Find the magnitude of the vector ({a}, {b}).", answer,
            $"Magnitude = √({a}² + {b}²) = √{answer * answer} = {answer}.",
            "pythagorean", a, b);
    }

    private static ContextualRaw MatrixRaw(string key, int scale)
    {
        var a = StableRange($"{key}|a", 2, 3 + scale);
        var d = StableRange($"{key}|d", 2, 4 + scale);
        var answer = a * d;
        return Raw("matrix", $"Find the determinant of [[{a}, 0], [0, {d}]].", answer,
            $"det = ({a} × {d}) − (0 × 0) = {answer}.",
            "multiply", a, d);
    }

    private static ContextualRaw LogarithmRaw(string key, int scale)
    {
        var exponent = StableRange($"{key}|e", 2, Math.Min(8, 3 + scale));
        var value = IntPow(2, exponent);
        return Raw("logarithm", $"Find log₂({value}).", exponent,
            $"2^{exponent} = {value}, so log₂({value}) = {exponent}.",
            "log2", value);
    }

    private static ContextualRaw DerivativeRaw(string key, int scale)
    {
        var exponent = StableRange($"{key}|e", 2, Math.Min(6, 2 + scale));
        return Raw("derivative", $"For f(x) = x^{exponent}, find f′(1).", exponent,
            $"f′(x) = {exponent}x^{exponent - 1}; at x = 1, f′(1) = {exponent}.",
            "derivative-at-one", exponent);
    }

    private static ContextualRaw IntegralRaw(string key, int scale)
    {
        var constant = StableRange($"{key}|c", 2, 4 + scale);
        var upper = StableRange($"{key}|u", 2, 4 + scale);
        var answer = constant * upper;
        return Raw("integral", $"Evaluate ∫₀^{upper} {constant} dx.", answer,
            $"The area is constant × interval length: {constant} × {upper} = {answer}.",
            "integral-constant", constant, upper);
    }

    private static ContextualRaw MeasurementRaw(string key, int scale)
    {
        var metres = StableRange($"{key}|m", 1, 5 + scale);
        var answer = metres * 100;
        return Raw("measurement", $"Convert {metres} metres to centimetres.", answer,
            $"1 metre = 100 centimetres, so {metres} × 100 = {answer} centimetres.",
            "times-100", metres);
    }

    private static ContextualRaw DecimalRaw(string key, int scale)
    {
        var whole = StableRange($"{key}|w", 1, 5 + scale);
        var tenths = StableRange($"{key}|t", 1, 9);
        var answer = whole * 10 + tenths;
        return Raw("decimal", $"How many tenths are in {whole}.{tenths}?", answer,
            $"{whole}.{tenths} = {answer}/10, so it contains {answer} tenths.",
            "decimal-tenths", whole, tenths);
    }

    private static ContextualRaw GeneralReasoningRaw(string key, int scale)
    {
        // Last-resort Mathematics context check: still a genuine, gradeable
        // mathematical reasoning item, but explicitly remains AiAssisted rather
        // than pretending to be a native solver for an unknown curriculum topic.
        var x = StableRange($"{key}|x", 2, 6 + 2 * scale);
        var addend = StableRange($"{key}|b", 1, 4 + scale);
        var total = x + addend;
        return Raw("general-mathematics", $"Find the missing value: □ + {addend} = {total}.", x,
            $"Use the inverse operation: {total} − {addend} = {x}.",
            "equation-add", addend, total);
    }

    private static ContextualRaw Raw(
        string topic,
        string prompt,
        int answer,
        string solution,
        string operation,
        int a = 0,
        int b = 0,
        int c = 0,
        int d = 0)
    {
        var parameters = new ContextualParameters(topic, operation, a, b, c, d);
        return new ContextualRaw(
            topic,
            prompt,
            answer.ToString(),
            solution,
            JsonSerializer.Serialize(parameters));
    }

    private static void ValidateRaw(ContextualRaw raw)
    {
        var parameters = JsonSerializer.Deserialize<ContextualParameters>(raw.ParametersJson)
            ?? throw new InvalidOperationException("Invalid contextual Mathematics parameters.");
        var expected = Recalculate(parameters);
        if (!string.Equals(expected.ToString(), raw.Answer, StringComparison.Ordinal) ||
            !raw.Solution.Contains(raw.Answer, StringComparison.Ordinal))
        {
            throw new InvalidOperationException(
                "Contextual Mathematics item failed reconstructable answer validation.");
        }
    }

    private static int Recalculate(ContextualParameters p) =>
        p.Operation switch
        {
            "add" => p.A + p.B,
            "subtract" => p.A - p.B,
            "multiply" => p.A * p.B,
            "divide" when p.B > 0 && p.A % p.B == 0 => p.A / p.B,
            "fraction" when p.B > 0 && p.C % p.B == 0 => p.C / p.B * p.A,
            "percent" when p.B * p.A % 100 == 0 => p.B * p.A / 100,
            "equation-add" => p.B - p.A,
            "equation-multiply" when p.A > 0 && p.B % p.A == 0 => p.B / p.A,
            "perimeter" => 2 * (p.A + p.B),
            "complement" => 90 - p.A,
            "supplement" => 180 - p.A,
            "mean3" when (p.A + p.B + p.C) % 3 == 0 => (p.A + p.B + p.C) / 3,
            "probability-percent" when p.B > 0 && p.A * 100 % p.B == 0 => p.A * 100 / p.B,
            "power" => IntPow(p.A, p.B),
            "root" => IntegerSquareRoot(p.A),
            "sequence-next" => p.A + 3 * p.B,
            "function" => p.A * p.C + p.B,
            "slope" when p.B > 0 && p.A % p.B == 0 => p.A / p.B,
            "pythagorean" => IntegerSquareRoot(p.A * p.A + p.B * p.B),
            "log2" => IntegerLog2(p.A),
            "derivative-at-one" => p.A,
            "integral-constant" => p.A * p.B,
            "times-100" => p.A * 100,
            "decimal-tenths" => p.A * 10 + p.B,
            _ => throw new InvalidOperationException("Unsupported contextual Mathematics operation.")
        };

    private static string ResolveTopic(string context)
    {
        var text = context.ToUpperInvariant();
        if (ContainsAny(text, "DERIVATIVE", "DIFFERENTIAT", "POCHODN", "مشتق")) return "derivative";
        if (ContainsAny(text, "INTEGRAL", "INTEGRATION", "CAŁK", "CALK", "تكامل")) return "integral";
        if (ContainsAny(text, "LOGARITH", "LOGARYTM", "لوغاريتم")) return "logarithm";
        if (ContainsAny(text, "MATRIX", "MATRICES", "MACIERZ", "مصفوف")) return "matrix";
        if (ContainsAny(text, "VECTOR", "WEKTOR", "متجه")) return "vector";
        if (ContainsAny(text, "PYTHAG", "TRIGONOMET", "PITAGOR", "مثلث قائم")) return "pythagorean";
        if (ContainsAny(text, "SLOPE", "GRADIENT", "NACHYLEN", "ميل")) return "slope";
        if (ContainsAny(text, "FUNCTION", "FUNKCJ", "دالة")) return "function";
        if (ContainsAny(text, "SEQUENCE", "PATTERN", "CIĄG", "CIAG", "متتالية")) return "sequence";
        if (ContainsAny(text, "PROBABILITY", "PRAWDOPODOB", "احتمال")) return "probability";
        if (ContainsAny(text, "STATISTIC", "MEAN", "AVERAGE", "MEDIAN", "DATA SET", "ŚREDNI", "SREDNI", "متوسط", "بيانات")) return "statistics";
        if (ContainsAny(text, "ANGLE", "KĄT", "KAT", "زاوية")) return "angle";
        if (ContainsAny(text, "GEOMET", "AREA", "PERIMETER", "RECTANGLE", "CIRCLE", "SHAPE", "POLE", "OBWÓD", "OBWOD", "HEND", "هندسة", "مساحة", "محيط", "دائرة")) return "geometry";
        if (ContainsAny(text, "PERCENT", "PROCENT", "مئوية")) return "percentage";
        if (ContainsAny(text, "RATIO", "RATE", "PROPORTION", "PROPORCJ", "نسبة", "تناسب")) return "ratio-rate";
        if (ContainsAny(text, "FRACTION", "UŁAM", "ULAM", "كسر", "كسور")) return "fraction";
        if (ContainsAny(text, "DECIMAL", "DZIESIĘTN", "DZIESIETN", "عشري")) return "decimal";
        if (ContainsAny(text, "SQUARE ROOT", "ROOT", "PIERWIAST", "جذر")) return "root";
        if (ContainsAny(text, "EXPONENT", "POWER", "POTĘG", "POTEG", "أس")) return "exponent";
        if (ContainsAny(text, "EQUATION", "INEQUALITY", "ALGEBRA", "VARIABLE", "RÓWNAN", "ROWNAN", "NIERÓWN", "NIEROWN", "معادلة", "معادلات", "متباينة", "جبر")) return "equation";
        if (ContainsAny(text, "MEASURE", "LENGTH", "VOLUME", "MASS", "UNIT", "MIAR", "DŁUG", "DLUG", "OBJĘTO", "OBJETO", "قياس", "طول", "حجم")) return "measurement";
        if (ContainsAny(text, "DIVID", "DIVISION", "QUOTIENT", "DZIEL", "قسمة")) return "division";
        if (ContainsAny(text, "MULTIP", "PRODUCT", "MNOŻ", "MNOZ", "ضرب")) return "multiplication";
        if (ContainsAny(text, "SUBTRACT", "DIFFERENCE", "ODEJM", "طرح")) return "subtraction";
        if (ContainsAny(text, "ADD", "SUM", "DODAW", "جمع")) return "addition";
        return "general-mathematics";
    }

    private static string CleanContext(string? context, string? outcomeCode)
    {
        var value = string.IsNullOrWhiteSpace(context)
            ? outcomeCode
            : context;
        return value?.Trim() ?? string.Empty;
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
            _ => throw new InvalidOperationException("Unsupported contextual Mathematics item type.")
        };
    }

    private static string BuildMultipleChoicePrompt(string prompt, string answer, string key)
    {
        if (!int.TryParse(answer, out var correct))
            throw new InvalidOperationException("Contextual multiple-choice answer must be an integer.");

        var delta = StableRange($"{key}|choice-delta", 1, 5);
        var choices = new HashSet<int> { correct };
        for (var i = 1; choices.Count < 4; i++)
        {
            choices.Add(correct + delta * i);
            if (choices.Count < 4)
                choices.Add(Math.Max(0, correct - delta * i));
        }

        var ordered = choices
            .OrderBy(x => StableInt($"{key}|choice|{x}", int.MaxValue))
            .Take(4)
            .ToArray();
        var labels = new[] { "A", "B", "C", "D" };
        var options = string.Join("  ", ordered.Select((x, i) => $"{labels[i]}) {x}"));
        return $"{prompt} {options}";
    }

    private static string Fingerprint(
        AssessmentBlueprint blueprint,
        Guid outcomeId,
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
            MathematicsGeneratorFamily.CurriculumContextCheck,
            difficulty,
            itemType,
            parametersJson,
            prompt,
            answer);
        return Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(material)))
            .ToLowerInvariant();
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
                throw new InvalidOperationException("Blueprint allocations cannot be negative.");
            for (var i = 0; i < amount; i++) result.Add(value(allocation));
        }
        return result;
    }

    private static int StableRange(string key, int minInclusive, int maxInclusive)
    {
        if (maxInclusive < minInclusive)
            throw new InvalidOperationException("Invalid deterministic contextual range.");
        return minInclusive + StableInt(key, maxInclusive - minInclusive + 1);
    }

    private static int StableInt(string key, int maxExclusive)
    {
        if (maxExclusive <= 0)
            throw new InvalidOperationException("Deterministic contextual range must be positive.");
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(key));
        var value = BitConverter.ToUInt32(hash, 0);
        return (int)(value % (uint)maxExclusive);
    }

    private static int GreatestCommonDivisor(int a, int b)
    {
        while (b != 0) (a, b) = (b, a % b);
        return Math.Abs(a);
    }

    private static int IntPow(int value, int exponent)
    {
        var result = 1;
        for (var i = 0; i < exponent; i++) result = checked(result * value);
        return result;
    }

    private static int IntegerSquareRoot(int value)
    {
        if (value < 0) throw new InvalidOperationException("Square root requires non-negative value.");
        var root = (int)Math.Sqrt(value);
        if (root * root != value)
            throw new InvalidOperationException("Contextual square-root parameter is not a perfect square.");
        return root;
    }

    private static int IntegerLog2(int value)
    {
        if (value <= 0) throw new InvalidOperationException("Logarithm value must be positive.");
        var exponent = 0;
        var current = value;
        while (current > 1 && current % 2 == 0)
        {
            current /= 2;
            exponent++;
        }
        if (current != 1)
            throw new InvalidOperationException("Contextual logarithm parameter is not a power of two.");
        return exponent;
    }

    private static bool ContainsAny(string text, params string[] values) =>
        values.Any(value => text.Contains(value, StringComparison.Ordinal));

    private sealed record ContextualRaw(
        string Topic,
        string Prompt,
        string Answer,
        string Solution,
        string ParametersJson);

    private sealed record ContextualParameters(
        string Topic,
        string Operation,
        int A,
        int B,
        int C,
        int D);
}
