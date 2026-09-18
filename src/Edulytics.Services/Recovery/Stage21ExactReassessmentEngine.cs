using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Edulytics.Core.AssessmentIntelligence;
using Edulytics.Core.Entities;
using Edulytics.Core.Enums;
using Edulytics.Core.Mathematics.Assessment;
using Edulytics.Core.MathematicsGeneration;
using Edulytics.Core.Recovery;
using Edulytics.Services.Mathematics;

namespace Edulytics.Services.Recovery;

/// <summary>
/// Stage 21 exact reassessment generator. Freshness is mathematical:
/// coefficients, representation, strategy, context, misconception target and
/// cognitive demand are all part of the reconstructable exposure signature.
/// </summary>
public sealed class Stage21ExactReassessmentEngine
{
    public const string GenerationMethod = "skill-contract-reassessment-math-fresh-v1";
    public const string SolverIdentifier = "stage21-exact-reassessment-solver-v1";
    public const string VerifierIdentifier = "stage21-independent-reassessment-verifier-v1";
    private const int MaxAttemptsPerItem = 192;

    private readonly WeaknessRecoveryEngine recovery;

    public Stage21ExactReassessmentEngine(WeaknessRecoveryEngine recovery)
    {
        this.recovery = recovery ?? throw new ArgumentNullException(nameof(recovery));
    }

    public MathematicsGenerationBatch Generate(
        WeaknessRecoveryPlan plan,
        MathematicsOutcomeGenerationProfile profile,
        Stage19AssessmentSkillContract contract,
        int seed)
    {
        ArgumentNullException.ThrowIfNull(plan);
        ArgumentNullException.ThrowIfNull(profile);
        ArgumentNullException.ThrowIfNull(contract);

        if (profile.LearningOutcomeId != plan.LearningOutcomeId ||
            !string.Equals(profile.OutcomeCode, contract.OutcomeCode, StringComparison.OrdinalIgnoreCase) ||
            contract.AllowedQuestionFamilies.Count == 0)
        {
            throw new InvalidOperationException(
                "Stage 21 exact reassessment requires the approved Outcome/SkillContract.");
        }

        var previous = (plan.PreviousMathematicalSignatures ?? [])
            .ToArray();

        if ((plan.ExcludedExposureFingerprints.Count > 0 ||
             plan.PreviousPromptShapes.Count > 0) &&
            previous.Length == 0)
        {
            throw new InvalidOperationException(
                "Stage 21 exact reassessment requires mathematical signatures for prior exposure; wording-only history is insufficient.");
        }

        var excludedFingerprints = plan.ExcludedExposureFingerprints
            .ToHashSet(StringComparer.Ordinal);
        var selectedFingerprints = new HashSet<string>(StringComparer.Ordinal);
        var selectedCoefficientSignatures = new HashSet<string>(StringComparer.Ordinal);
        var selectedSignatures = new List<ReassessmentMathematicalSignature>();
        var generated = new List<GeneratedMathematicsItem>();

        var sequence = 0;
        foreach (var allocation in plan.EquivalentReassessmentBlueprint.DifficultyAllocations)
        {
            for (var itemIndex = 0; itemIndex < allocation.ItemCount; itemIndex++)
            {
                GeneratedMathematicsItem? accepted = null;
                ReassessmentMathematicalSignature? acceptedSignature = null;

                for (var attempt = 0; attempt < MaxAttemptsPerItem && accepted is null; attempt++)
                {
                    var family = contract.AllowedQuestionFamilies[
                        (sequence + attempt) % contract.AllowedQuestionFamilies.Count];

                    var demand = ResolveDemand(
                        allocation.Difficulty,
                        sequence + attempt);

                    var question = new ExactSkillContractQuestionEngine().Generate(
                        "stage21-base",
                        contract.OutcomeCode,
                        [family],
                        ResolveExactDifficulty(demand),
                        1,
                        unchecked(seed + sequence * 7919 + attempt * 104729),
                        [])[0];

                    if (!ExactSkillContractQuestionEngine.Verify(
                            question.Family,
                            question.Parameters,
                            question.CorrectAnswer))
                    {
                        throw new InvalidOperationException(
                            $"Stage 21 exact verifier rejected {question.Family}.");
                    }

                    var descriptor = BuildDescriptor(
                        question.Family,
                        question.Parameters,
                        demand,
                        sequence + attempt);

                    var coefficientSignature = CoefficientSignature(question.Parameters);
                    var exposureFingerprint = Fingerprint(
                        contract,
                        descriptor,
                        coefficientSignature);

                    var signature = new ReassessmentMathematicalSignature(
                        exposureFingerprint,
                        question.Family,
                        coefficientSignature,
                        descriptor.Representation,
                        descriptor.Strategy,
                        descriptor.Context,
                        descriptor.MisconceptionTrap,
                        demand);

                    if (excludedFingerprints.Contains(exposureFingerprint) ||
                        selectedFingerprints.Contains(exposureFingerprint) ||
                        selectedCoefficientSignatures.Contains(coefficientSignature) ||
                        !IsMathematicallyFresh(signature, previous))
                    {
                        continue;
                    }

                    var itemId = Guid.NewGuid();
                    var prompt = BuildPrompt(
                        question.Family,
                        question.Parameters,
                        descriptor,
                        question.Prompt);

                    var item = new AssessmentItem
                    {
                        Id = itemId,
                        SchoolId = plan.SchoolId,
                        CurriculumAdoptionId = plan.CurriculumAdoptionId,
                        CurriculumPedagogicalLessonId = plan.CurriculumPedagogicalLessonId,
                        Source = AssessmentItemSource.SystemGenerated,
                        ItemType = question.ItemType,
                        Difficulty = allocation.Difficulty,
                        Prompt = prompt,
                        CorrectAnswer = question.CorrectAnswer,
                        Solution = question.Solution,
                        GenerationMethod = GenerationMethod,
                        GenerationFamily = question.Family,
                        GenerationParametersJson = JsonSerializer.Serialize(new
                        {
                            skillId = contract.SkillId,
                            outcomeCode = contract.OutcomeCode,
                            questionFamily = question.Family,
                            parameters = question.Parameters,
                            freshness = new
                            {
                                coefficientSignature,
                                descriptor.Representation,
                                descriptor.Strategy,
                                descriptor.Context,
                                descriptor.MisconceptionTrap,
                                cognitiveDemand = demand.ToString()
                            }
                        }),
                        ExposureFingerprint = exposureFingerprint,
                        ValidationMetadataJson = JsonSerializer.Serialize(new
                        {
                            stage = 21,
                            alignment = "official-outcome-skill-contract-verified",
                            readiness = "READY_VERIFIED",
                            skillContract = contract.SkillId,
                            officialOutcomeCode = contract.OutcomeCode,
                            solver = SolverIdentifier,
                            verifier = VerifierIdentifier,
                            solverVerified = true,
                            mathematicalFreshnessVerified = true,
                            wordingOnlyFreshness = false,
                            freshnessDimensions = new[]
                            {
                                "coefficients",
                                "representation",
                                "strategy",
                                "context",
                                "misconceptionTrap",
                                "cognitiveDemand"
                            }
                        }),
                        CreatedAtUtc = DateTime.UtcNow,
                        RowVersion = []
                    };

                    accepted = new GeneratedMathematicsItem(
                        item,
                        new AssessmentItemOutcome
                        {
                            Id = Guid.NewGuid(),
                            SchoolId = plan.SchoolId,
                            AssessmentItemId = itemId,
                            LearningOutcomeId = plan.LearningOutcomeId
                        },
                        ResolveBlueprintFamily(question.Family),
                        GenerationMethod);
                    acceptedSignature = signature;
                }

                if (accepted is null || acceptedSignature is null)
                {
                    throw new InvalidOperationException(
                        "Unable to produce a mathematically fresh exact reassessment within the retry budget.");
                }

                generated.Add(accepted);
                selectedSignatures.Add(acceptedSignature);
                selectedFingerprints.Add(acceptedSignature.ExposureFingerprint);
                selectedCoefficientSignatures.Add(acceptedSignature.CoefficientSignature);
                sequence++;
            }
        }

        ValidateBatchFreshness(
            selectedSignatures,
            previous,
            contract,
            plan.EquivalentReassessmentBlueprint.QuestionCount);

        var result = new MathematicsGenerationBatch(
            plan.SchoolId,
            plan.CurriculumAdoptionId,
            plan.CurriculumLevelKey,
            generated,
            GenerationMethod);

        recovery.ValidateEquivalentReassessment(plan, result);
        return result;
    }

    public static ReassessmentMathematicalSignature ExtractSignature(
        AssessmentItem item)
    {
        ArgumentNullException.ThrowIfNull(item);

        if (!string.Equals(item.GenerationMethod, GenerationMethod, StringComparison.Ordinal) ||
            string.IsNullOrWhiteSpace(item.GenerationParametersJson) ||
            string.IsNullOrWhiteSpace(item.ExposureFingerprint) ||
            string.IsNullOrWhiteSpace(item.GenerationFamily))
        {
            throw new InvalidOperationException(
                "Assessment item is not a reconstructable Stage 21 exact reassessment item.");
        }

        using var document = JsonDocument.Parse(item.GenerationParametersJson);
        var freshness = document.RootElement.GetProperty("freshness");

        if (!Enum.TryParse<ReassessmentCognitiveDemand>(
                freshness.GetProperty("cognitiveDemand").GetString(),
                out var demand))
        {
            throw new InvalidOperationException(
                "Stage 21 reassessment item has an invalid cognitive-demand signature.");
        }

        return new ReassessmentMathematicalSignature(
            item.ExposureFingerprint,
            item.GenerationFamily,
            freshness.GetProperty("coefficientSignature").GetString()!,
            freshness.GetProperty("Representation").GetString()!,
            freshness.GetProperty("Strategy").GetString()!,
            freshness.GetProperty("Context").GetString()!,
            freshness.GetProperty("MisconceptionTrap").GetString()!,
            demand);
    }

    private static bool IsMathematicallyFresh(
        ReassessmentMathematicalSignature candidate,
        IReadOnlyList<ReassessmentMathematicalSignature> previous)
    {
        foreach (var prior in previous)
        {
            if (string.Equals(
                    candidate.CoefficientSignature,
                    prior.CoefficientSignature,
                    StringComparison.Ordinal))
            {
                return false;
            }

            var changedDimensions = 0;
            if (!string.Equals(candidate.QuestionFamily, prior.QuestionFamily, StringComparison.Ordinal))
                changedDimensions++;
            if (!string.Equals(candidate.Representation, prior.Representation, StringComparison.Ordinal))
                changedDimensions++;
            if (!string.Equals(candidate.Strategy, prior.Strategy, StringComparison.Ordinal))
                changedDimensions++;
            if (!string.Equals(candidate.Context, prior.Context, StringComparison.Ordinal))
                changedDimensions++;
            if (!string.Equals(candidate.MisconceptionTrap, prior.MisconceptionTrap, StringComparison.Ordinal))
                changedDimensions++;
            if (candidate.CognitiveDemand != prior.CognitiveDemand)
                changedDimensions++;

            if (changedDimensions < 2)
                return false;
        }

        return true;
    }

    private static void ValidateBatchFreshness(
        IReadOnlyList<ReassessmentMathematicalSignature> signatures,
        IReadOnlyList<ReassessmentMathematicalSignature> previous,
        Stage19AssessmentSkillContract contract,
        int expectedCount)
    {
        if (signatures.Count != expectedCount)
            throw new InvalidOperationException("Stage 21 reassessment signature count does not match the blueprint.");

        if (signatures.Select(x => x.ExposureFingerprint).Distinct(StringComparer.Ordinal).Count() != signatures.Count ||
            signatures.Select(x => x.CoefficientSignature).Distinct(StringComparer.Ordinal).Count() != signatures.Count)
        {
            throw new InvalidOperationException(
                "Stage 21 reassessment reused a mathematical exposure or coefficient set.");
        }

        if (signatures.Any(x =>
                !contract.AllowedQuestionFamilies.Contains(
                    x.QuestionFamily,
                    StringComparer.Ordinal)))
        {
            throw new InvalidOperationException(
                "Stage 21 reassessment selected a question family outside the exact SkillContract.");
        }

        if (signatures.Any(x => !IsMathematicallyFresh(x, previous)))
        {
            throw new InvalidOperationException(
                "Stage 21 reassessment contains a wording-only or mathematically stale item.");
        }

        if (signatures.Count > 1)
        {
            RequireVariation(signatures.Select(x => x.Representation), "representation");
            RequireVariation(signatures.Select(x => x.Strategy), "strategy");
            RequireVariation(signatures.Select(x => x.Context), "context");
            RequireVariation(signatures.Select(x => x.MisconceptionTrap), "misconception trap");
            RequireVariation(signatures.Select(x => x.CognitiveDemand.ToString()), "cognitive demand");
        }
    }

    private static void RequireVariation(
        IEnumerable<string> values,
        string dimension)
    {
        if (values.Distinct(StringComparer.Ordinal).Count() < 2)
        {
            throw new InvalidOperationException(
                $"Stage 21 reassessment did not vary mathematical {dimension}.");
        }
    }

    private static VariantDescriptor BuildDescriptor(
        string family,
        IReadOnlyDictionary<string, int> parameters,
        ReassessmentCognitiveDemand demand,
        int variant)
    {
        var contextVariant = Math.Abs(variant) % 4;

        return family switch
        {
            "fractions.equivalent.missing_value" => new(
                contextVariant % 2 == 0 ? "symbolic-equation" : "fraction-strip",
                "scale-factor",
                contextVariant % 2 == 0 ? "abstract-equivalence" : "fraction-model",
                "additive-instead-of-multiplicative",
                demand),

            "fractions.equivalent.recognize" => new(
                contextVariant % 2 == 0 ? "equivalence-pair" : "area-model-check",
                "cross-product-check",
                contextVariant % 2 == 0 ? "equivalence-check" : "area-model",
                "denominator-size-only",
                demand),

            "fractions.equivalent.generate_multiple" => new(
                contextVariant % 2 == 0 ? "constructed-equivalence" : "fraction-family",
                "multiplicative-scaling",
                contextVariant % 2 == 0 ? "equivalence-family" : "fraction-pattern",
                "scale-one-term-only",
                demand),

            "fractions.equivalent.number_line" => new(
                "number-line",
                "same-point-equivalence",
                contextVariant % 2 == 0 ? "number-line-position" : "unit-interval",
                "larger-denominator-means-larger-value",
                demand),

            "fractions.equivalent.reduce_common_factor" => new(
                contextVariant % 2 == 0 ? "simplified-form" : "factor-model",
                "common-factor-reduction",
                contextVariant % 2 == 0 ? "simplification" : "factor-structure",
                "subtract-same-number",
                demand),

            "ratio.unit_rate.direct" => new(
                contextVariant switch
                {
                    0 => "rate-statement",
                    1 => "ratio-table",
                    2 => "double-number-line",
                    _ => "unit-rate-equation"
                },
                "divide-to-one",
                contextVariant switch
                {
                    0 => "production",
                    1 => "travel",
                    2 => "cost",
                    _ => "recipe"
                },
                "use-total-as-unit-rate",
                demand),

            "ratio.unit_rate.equivalent_ratio" => new(
                contextVariant switch
                {
                    0 => "equivalent-ratio",
                    1 => "ratio-table",
                    2 => "double-number-line",
                    _ => "proportion-equation"
                },
                "scale-both-terms",
                contextVariant switch
                {
                    0 => "production",
                    1 => "travel",
                    2 => "cost",
                    _ => "recipe"
                },
                "scale-only-one-term",
                demand),

            _ => throw new InvalidOperationException(
                $"Stage 21 has no reviewed mathematical freshness descriptor for {family}.")
        };
    }

    private static string BuildPrompt(
        string family,
        IReadOnlyDictionary<string, int> p,
        VariantDescriptor descriptor,
        string fallback)
    {
        if (family == "ratio.unit_rate.direct")
        {
            return descriptor.Context switch
            {
                "travel" =>
                    $"A vehicle travels {p["total"]} km in {p["quantity"]} hours at a constant rate. What is the distance per hour?",
                "cost" =>
                    $"{p["quantity"]} identical items cost {p["total"]} zł altogether. What is the cost per item?",
                "recipe" =>
                    $"A recipe uses {p["total"]} spoonfuls across {p["quantity"]} equal batches. How many spoonfuls are used per batch?",
                _ => fallback
            };
        }

        if (family == "ratio.unit_rate.equivalent_ratio")
        {
            return descriptor.Context switch
            {
                "travel" =>
                    $"A constant rate is {p["baseFirst"]} km in {p["baseSecond"]} hours. At the same rate, how many kilometres are travelled in {p["targetSecond"]} hours?",
                "cost" =>
                    $"{p["baseSecond"]} identical items cost {p["baseFirst"]} zł. At the same unit price, what is the cost of {p["targetSecond"]} items?",
                "recipe" =>
                    $"A recipe uses {p["baseFirst"]} spoonfuls for {p["baseSecond"]} batches. At the same ratio, how many spoonfuls are needed for {p["targetSecond"]} batches?",
                _ => fallback
            };
        }

        return descriptor.Representation switch
        {
            "fraction-strip" => $"A fraction strip represents the same value on both sides. {fallback}",
            "area-model-check" => $"Use equal-area reasoning to test the equivalence. {fallback}",
            "fraction-family" => $"Build another member of the same equivalent-fraction family. {fallback}",
            "factor-model" => $"Use the common-factor structure of numerator and denominator. {fallback}",
            _ => fallback
        };
    }

    private static ReassessmentCognitiveDemand ResolveDemand(
        AssessmentItemDifficulty uiDifficulty,
        int variant)
    {
        return uiDifficulty switch
        {
            AssessmentItemDifficulty.Easy =>
                variant % 2 == 0
                    ? ReassessmentCognitiveDemand.Standard
                    : ReassessmentCognitiveDemand.Stretch,

            AssessmentItemDifficulty.Medium =>
                variant % 2 == 0
                    ? ReassessmentCognitiveDemand.Stretch
                    : ReassessmentCognitiveDemand.Standard,

            AssessmentItemDifficulty.Challenging =>
                variant % 2 == 0
                    ? ReassessmentCognitiveDemand.Challenge
                    : ReassessmentCognitiveDemand.Stretch,

            _ => ReassessmentCognitiveDemand.Standard
        };
    }

    private static ExactSkillQuestionDifficulty ResolveExactDifficulty(
        ReassessmentCognitiveDemand demand) =>
        demand switch
        {
            ReassessmentCognitiveDemand.Stretch => ExactSkillQuestionDifficulty.Stretch,
            ReassessmentCognitiveDemand.Challenge => ExactSkillQuestionDifficulty.Challenge,
            _ => ExactSkillQuestionDifficulty.Standard
        };

    private static string CoefficientSignature(
        IReadOnlyDictionary<string, int> parameters) =>
        string.Join(
            ";",
            parameters
                .OrderBy(x => x.Key, StringComparer.Ordinal)
                .Select(x => $"{x.Key}={x.Value}"));

    private static string Fingerprint(
        Stage19AssessmentSkillContract contract,
        VariantDescriptor descriptor,
        string coefficientSignature)
    {
        var material = string.Join(
            "|",
            GenerationMethod,
            contract.OutcomeCode,
            contract.SkillId,
            coefficientSignature,
            descriptor.Representation,
            descriptor.Strategy,
            descriptor.Context,
            descriptor.MisconceptionTrap,
            descriptor.CognitiveDemand);

        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(material));
        return "stage21:" + Convert.ToHexString(bytes).ToLowerInvariant();
    }

    private static AssessmentQuestionFamily ResolveBlueprintFamily(string family) =>
        family switch
        {
            "ratio.unit_rate.direct" or
            "ratio.unit_rate.equivalent_ratio" => AssessmentQuestionFamily.AppliedProblem,

            "fractions.equivalent.number_line" or
            "fractions.equivalent.recognize" => AssessmentQuestionFamily.MathematicalReasoning,

            _ => AssessmentQuestionFamily.StructuredMethod
        };

    private sealed record VariantDescriptor(
        string Representation,
        string Strategy,
        string Context,
        string MisconceptionTrap,
        ReassessmentCognitiveDemand CognitiveDemand);
}
