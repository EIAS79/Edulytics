using System.Text.Json;
using System.Text.Json.Nodes;
using Edulytics.Core.Entities;
using Edulytics.Core.Enums;
using Edulytics.Core.Mathematics.Practice;

namespace Edulytics.Services.Practice;

public enum PracticeSessionReadinessStatus
{
    ReadyBalanced = 1,
    ReadyNarrow = 2,
    BlockedInsufficientVariety = 3,
    BlockedGeneration = 4,
    BlockedPedagogicalQa = 5
}

public sealed record PracticeSessionQualityResult(
    PracticeSessionReadinessStatus Status,
    IReadOnlyList<string> ReasonCodes,
    int RequestedQuestionCount,
    int ActualQuestionCount,
    int DistinctSemanticQuestions,
    int DistinctFamilies,
    int DistinctQuestionForms,
    int DistinctCognitiveOperations)
{
    public bool IsReady =>
        Status is
            PracticeSessionReadinessStatus.ReadyBalanced or
            PracticeSessionReadinessStatus.ReadyNarrow;

    public string StatusCode => Status switch
    {
        PracticeSessionReadinessStatus.ReadyBalanced => "READY_BALANCED",
        PracticeSessionReadinessStatus.ReadyNarrow => "READY_NARROW",
        PracticeSessionReadinessStatus.BlockedInsufficientVariety =>
            "BLOCKED_INSUFFICIENT_VARIETY",
        PracticeSessionReadinessStatus.BlockedGeneration =>
            "BLOCKED_GENERATION",
        _ => "BLOCKED_PEDAGOGICAL_QA"
    };
}

/// <summary>
/// Final session-level certification for exact lesson Practice.
///
/// Individual questions are already solved and independently verified. This
/// validator protects the assembled learner session: semantic uniqueness,
/// difficulty truthfulness, answer leakage, family/form concentration and
/// persisted exact-alignment integrity.
/// </summary>
public sealed class PracticeSessionQualityValidator
{
    public PracticeSessionQualityResult Validate(
        Stage18PracticeSkillContract contract,
        IReadOnlyList<AssessmentItem> items,
        int requestedQuestionCount)
    {
        ArgumentNullException.ThrowIfNull(contract);
        ArgumentNullException.ThrowIfNull(items);

        if (requestedQuestionCount is < 1 or > 30 ||
            items.Count == 0 ||
            items.Count > requestedQuestionCount)
        {
            return Blocked(
                PracticeSessionReadinessStatus.BlockedGeneration,
                "INVALID_SESSION_CARDINALITY",
                requestedQuestionCount,
                items);
        }

        var minimumAcceptableCount = Math.Min(3, requestedQuestionCount);
        if (items.Count < minimumAcceptableCount)
        {
            return Blocked(
                PracticeSessionReadinessStatus.BlockedInsufficientVariety,
                "INSUFFICIENT_VALID_ITEMS",
                requestedQuestionCount,
                items);
        }

        var fingerprints = new HashSet<string>(StringComparer.Ordinal);
        var semanticKeys = new HashSet<string>(StringComparer.Ordinal);
        var families = new HashSet<string>(StringComparer.Ordinal);
        var forms = new HashSet<PracticeQuestionForm>();
        var operations = new HashSet<PracticeCognitiveOperation>();
        var narrowReasons = new HashSet<string>(StringComparer.Ordinal);

        foreach (var item in items)
        {
            if (string.IsNullOrWhiteSpace(item.GenerationFamily) ||
                !contract.AllowedQuestionFamilies.Contains(
                    item.GenerationFamily,
                    StringComparer.Ordinal) ||
                !Stage18SkillContractPracticeEngine.VerifyPersistedItem(
                    contract,
                    item))
            {
                return Blocked(
                    PracticeSessionReadinessStatus.BlockedGeneration,
                    "EXACT_ITEM_VERIFICATION_FAILED",
                    requestedQuestionCount,
                    items);
            }

            var leakCheck = PracticeAnswerLeakValidator.ValidatePrompt(
                item.GenerationFamily,
                item.Prompt,
                item.CorrectAnswer);
            if (!leakCheck.IsSafe)
            {
                return Blocked(
                    PracticeSessionReadinessStatus.BlockedPedagogicalQa,
                    leakCheck.ReasonCode,
                    requestedQuestionCount,
                    items);
            }

            if (string.IsNullOrWhiteSpace(item.ExposureFingerprint) ||
                !fingerprints.Add(item.ExposureFingerprint))
            {
                return Blocked(
                    PracticeSessionReadinessStatus.BlockedInsufficientVariety,
                    "DUPLICATE_EXPOSURE_FINGERPRINT",
                    requestedQuestionCount,
                    items);
            }

            if (!TryReadGeneration(
                    item,
                    out var parameters,
                    out var variantSlot) ||
                !TryReadAssessmentMetadata(
                    item,
                    out var metadataForm,
                    out var metadataOperation,
                    out var metadataDifficulty))
            {
                return Blocked(
                    PracticeSessionReadinessStatus.BlockedPedagogicalQa,
                    "INVALID_COMPOSER_METADATA",
                    requestedQuestionCount,
                    items);
            }

            var semanticIdentity =
                PracticeSemanticQuestionIdentityPolicy.Create(
                    item.GenerationFamily,
                    parameters);
            if (!semanticKeys.Add(semanticIdentity.Key))
            {
                return Blocked(
                    PracticeSessionReadinessStatus.BlockedInsufficientVariety,
                    "DUPLICATE_SEMANTIC_QUESTION",
                    requestedQuestionCount,
                    items);
            }

            var capability =
                PracticeQuestionFormCapabilityRegistry.ResolveForVariant(
                    item.GenerationFamily,
                    variantSlot);
            if (capability is null ||
                capability.Form != metadataForm ||
                capability.CognitiveOperation != metadataOperation ||
                metadataDifficulty < capability.MinimumDifficulty ||
                metadataDifficulty > capability.MaximumDifficulty)
            {
                return Blocked(
                    PracticeSessionReadinessStatus.BlockedPedagogicalQa,
                    "DIFFICULTY_OR_FORM_CAPABILITY_MISMATCH",
                    requestedQuestionCount,
                    items);
            }

            var expectedItemDifficulty =
                metadataDifficulty == PracticeCognitiveDifficulty.Standard
                    ? AssessmentItemDifficulty.Medium
                    : AssessmentItemDifficulty.Challenging;
            if (item.Difficulty != expectedItemDifficulty)
            {
                return Blocked(
                    PracticeSessionReadinessStatus.BlockedPedagogicalQa,
                    "DISPLAY_DIFFICULTY_MISMATCH",
                    requestedQuestionCount,
                    items);
            }

            families.Add(item.GenerationFamily);
            forms.Add(metadataForm);
            operations.Add(metadataOperation);
        }

        if (items.Count < requestedQuestionCount)
            narrowReasons.Add("VALID_SESSION_SHORTER_THAN_REQUESTED");

        if (contract.AllowedQuestionFamilies.Count > 1 &&
            items.Count >= 4)
        {
            if (families.Count == 1)
                narrowReasons.Add("MULTI_FAMILY_CONTRACT_SINGLE_FAMILY_SESSION");

            var largestFamilyCount = items
                .GroupBy(
                    item => item.GenerationFamily!,
                    StringComparer.Ordinal)
                .Max(group => group.Count());
            if (largestFamilyCount * 4 > items.Count * 3)
                narrowReasons.Add("HIGH_FAMILY_CONCENTRATION");
        }

        var availableForms = contract.AllowedQuestionFamilies
            .SelectMany(
                PracticeQuestionFormCapabilityRegistry.Resolve)
            .Select(capability => capability.Form)
            .Distinct()
            .Count();
        if (availableForms > 1 &&
            items.Count >= 4 &&
            forms.Count == 1)
        {
            narrowReasons.Add("AVAILABLE_FORM_DIVERSITY_NOT_REPRESENTED");
        }

        var availableOperations = contract.AllowedQuestionFamilies
            .SelectMany(
                PracticeQuestionFormCapabilityRegistry.Resolve)
            .Select(capability => capability.CognitiveOperation)
            .Distinct()
            .Count();
        if (availableOperations > 1 &&
            items.Count >= 4 &&
            operations.Count == 1)
        {
            narrowReasons.Add("AVAILABLE_COGNITIVE_DIVERSITY_NOT_REPRESENTED");
        }

        var status = narrowReasons.Count == 0
            ? PracticeSessionReadinessStatus.ReadyBalanced
            : PracticeSessionReadinessStatus.ReadyNarrow;

        return new PracticeSessionQualityResult(
            status,
            narrowReasons.OrderBy(x => x, StringComparer.Ordinal).ToArray(),
            requestedQuestionCount,
            items.Count,
            semanticKeys.Count,
            families.Count,
            forms.Count,
            operations.Count);
    }

    public static void StampReadiness(
        IReadOnlyList<AssessmentItem> items,
        PracticeSessionQualityResult result)
    {
        ArgumentNullException.ThrowIfNull(items);
        ArgumentNullException.ThrowIfNull(result);

        foreach (var item in items)
        {
            JsonObject metadata;
            try
            {
                metadata = string.IsNullOrWhiteSpace(
                    item.ValidationMetadataJson)
                    ? new JsonObject()
                    : JsonNode.Parse(item.ValidationMetadataJson!)
                        ?.AsObject() ?? new JsonObject();
            }
            catch (JsonException)
            {
                metadata = new JsonObject();
            }

            metadata["sessionReadiness"] = result.StatusCode;
            metadata["sessionQualityValidated"] = true;
            metadata["sessionRequestedCount"] =
                result.RequestedQuestionCount;
            metadata["sessionActualCount"] =
                result.ActualQuestionCount;
            metadata["sessionSemanticCount"] =
                result.DistinctSemanticQuestions;
            metadata["sessionFamilyCount"] =
                result.DistinctFamilies;
            metadata["sessionQuestionFormCount"] =
                result.DistinctQuestionForms;
            metadata["sessionCognitiveOperationCount"] =
                result.DistinctCognitiveOperations;
            metadata["sessionQualityReasons"] =
                new JsonArray(
                    result.ReasonCodes
                        .Select(reason =>
                            JsonValue.Create(reason))
                        .ToArray());

            item.ValidationMetadataJson =
                metadata.ToJsonString();
        }
    }

    private static PracticeSessionQualityResult Blocked(
        PracticeSessionReadinessStatus status,
        string reasonCode,
        int requestedQuestionCount,
        IReadOnlyList<AssessmentItem> items) =>
        new(
            status,
            [reasonCode],
            requestedQuestionCount,
            items.Count,
            0,
            0,
            0,
            0);

    private static bool TryReadGeneration(
        AssessmentItem item,
        out IReadOnlyDictionary<string, int> parameters,
        out int variantSlot)
    {
        parameters = new Dictionary<string, int>();
        variantSlot = 0;

        if (string.IsNullOrWhiteSpace(item.GenerationParametersJson))
            return false;

        try
        {
            using var document = JsonDocument.Parse(
                item.GenerationParametersJson);
            var root = document.RootElement;
            var node = root.GetProperty("parameters");
            var values = new Dictionary<string, int>(
                StringComparer.Ordinal);
            foreach (var property in node.EnumerateObject())
                values[property.Name] = property.Value.GetInt32();

            parameters = values;
            variantSlot = values.TryGetValue(
                "variant",
                out var variant)
                    ? variant
                    : 0;
            return true;
        }
        catch (Exception exception) when (
            exception is JsonException or
            InvalidOperationException or
            KeyNotFoundException or
            FormatException)
        {
            return false;
        }
    }

    private static bool TryReadAssessmentMetadata(
        AssessmentItem item,
        out PracticeQuestionForm form,
        out PracticeCognitiveOperation operation,
        out PracticeCognitiveDifficulty difficulty)
    {
        form = default;
        operation = default;
        difficulty = default;

        if (string.IsNullOrWhiteSpace(item.ValidationMetadataJson))
            return false;

        try
        {
            using var document = JsonDocument.Parse(
                item.ValidationMetadataJson);
            var root = document.RootElement;

            if (!root.TryGetProperty(
                    "composer",
                    out var composer) ||
                !string.Equals(
                    composer.GetString(),
                    "practice-assessment-v2",
                    StringComparison.Ordinal) ||
                !root.TryGetProperty(
                    "questionForm",
                    out var formNode) ||
                !Enum.TryParse(
                    formNode.GetString(),
                    ignoreCase: false,
                    out form) ||
                !root.TryGetProperty(
                    "cognitiveOperation",
                    out var operationNode) ||
                !Enum.TryParse(
                    operationNode.GetString(),
                    ignoreCase: false,
                    out operation))
            {
                return false;
            }

            string? difficultyText = null;
            if (root.TryGetProperty(
                    "cognitiveDifficulty",
                    out var cognitiveDifficultyNode))
            {
                difficultyText =
                    cognitiveDifficultyNode.GetString();
            }
            else if (root.TryGetProperty(
                         "difficulty",
                         out var legacyDifficultyNode))
            {
                difficultyText =
                    legacyDifficultyNode.GetString();
            }

            return Enum.TryParse(
                difficultyText,
                ignoreCase: false,
                out difficulty);
        }
        catch (JsonException)
        {
            return false;
        }
    }
}
