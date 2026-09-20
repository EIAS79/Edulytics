using System.Text.Json;

namespace Edulytics.Core.Mathematics.Practice;

public enum LessonPracticeInteractionKind
{
    IntegerKeypad = 1,
    NumericEntry = 2,
    FractionEntry = 3,
    RelationChoice = 4,
    StructuredEntry = 5,
    ContractDriven = 6
}

public sealed record LessonPracticeInteractionContract(
    string FamilyId,
    LessonPracticeInteractionKind InteractionKind,
    string AnswerType,
    IReadOnlyList<string> Representations,
    bool HasVisualRepresentation);

/// <summary>
/// Presentation contract for every question family that may appear in
/// lesson-launched Practice. Mathematical generation remains owned by the
/// question engine; this registry only selects the learner interaction.
/// </summary>
public static class LessonPracticeInteractionRegistry
{
    private const string ResourceName =
        "Edulytics.Core.Mathematics.Generation.question-family-registry.v1.json";

    private static readonly IReadOnlyDictionary<
        string,
        LessonPracticeInteractionContract> ByFamily =
        Load();

    public static IReadOnlyCollection<
        LessonPracticeInteractionContract> All =>
        ByFamily.Values
            .OrderBy(x => x.FamilyId, StringComparer.Ordinal)
            .ToArray();

    public static bool TryResolve(
        string familyId,
        out LessonPracticeInteractionContract? contract)
    {
        contract = null;
        if (string.IsNullOrWhiteSpace(familyId))
            return false;

        return ByFamily.TryGetValue(
            familyId.Trim(),
            out contract);
    }

    private static IReadOnlyDictionary<
        string,
        LessonPracticeInteractionContract> Load()
    {
        var assembly =
            typeof(LessonPracticeInteractionRegistry).Assembly;

        using var stream =
            assembly.GetManifestResourceStream(ResourceName)
            ?? throw new InvalidOperationException(
                $"Missing embedded question-family registry: {ResourceName}.");

        using var document =
            JsonDocument.Parse(stream);

        var result =
            new Dictionary<
                string,
                LessonPracticeInteractionContract>(
                StringComparer.Ordinal);

        foreach (var family in document.RootElement
                     .GetProperty("families")
                     .EnumerateArray())
        {
            if (!family.TryGetProperty(
                    "lessonPracticeRouting",
                    out var routing) ||
                routing.ValueKind != JsonValueKind.True)
            {
                continue;
            }

            var id =
                family.GetProperty("id").GetString()?.Trim()
                ?? string.Empty;

            var answerType =
                family.GetProperty("answerType").GetString()?.Trim()
                ?? string.Empty;

            var representations =
                family.TryGetProperty(
                    "representations",
                    out var representationRows)
                    ? representationRows
                        .EnumerateArray()
                        .Select(x => x.GetString()?.Trim())
                        .Where(x => !string.IsNullOrWhiteSpace(x))
                        .Select(x => x!)
                        .Distinct(StringComparer.Ordinal)
                        .ToArray()
                    : [];

            if (string.IsNullOrWhiteSpace(id))
                throw new InvalidOperationException(
                    "Lesson Practice question family has no id.");

            var interaction =
                ResolveInteraction(id, answerType);

            var hasVisual =
                representations.Any(IsVisualRepresentation);

            if (!result.TryAdd(
                    id,
                    new LessonPracticeInteractionContract(
                        id,
                        interaction,
                        answerType,
                        representations,
                        hasVisual)))
            {
                throw new InvalidOperationException(
                    $"Duplicate Lesson Practice interaction family: {id}.");
            }
        }

        return result;
    }

    private static LessonPracticeInteractionKind ResolveInteraction(
        string familyId,
        string answerType)
    {
        if (string.Equals(
                answerType,
                "relation",
                StringComparison.Ordinal) ||
            familyId.Contains(
                ".compare",
                StringComparison.Ordinal) ||
            familyId.EndsWith(
                ".comparison",
                StringComparison.Ordinal))
        {
            return LessonPracticeInteractionKind.RelationChoice;
        }

        return answerType switch
        {
            "exact_integer" =>
                LessonPracticeInteractionKind.IntegerKeypad,

            "exact_scalar" =>
                LessonPracticeInteractionKind.NumericEntry,

            "exact_rational" =>
                LessonPracticeInteractionKind.FractionEntry,

            "exact_vector" or
            "exact_interval_or_union" or
            "quotient_and_remainder" or
            "enum_text" =>
                LessonPracticeInteractionKind.StructuredEntry,

            "contract_defined" =>
                LessonPracticeInteractionKind.ContractDriven,

            _ =>
                throw new InvalidOperationException(
                    $"Unsupported Lesson Practice answer type " +
                    $"{answerType} for {familyId}.")
        };
    }

    private static bool IsVisualRepresentation(
        string representation) =>
        representation is not (
            "symbolic" or
            "contextual" or
            "textual_constraint");
}
