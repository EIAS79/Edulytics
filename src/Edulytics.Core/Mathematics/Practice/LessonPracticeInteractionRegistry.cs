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
            var lessonRoutable = family.TryGetProperty(
                    "lessonPracticeRouting", out var routing) &&
                routing.ValueKind == JsonValueKind.True;
            // Keep candidate HS kernels available for direct mathematics QA,
            // but do not register a learner-facing interaction for any family
            // whose academic/runtime readiness is still IntegrationPending.
            var integrationPending = family.TryGetProperty(
                    "status", out var status) &&
                string.Equals(status.GetString(), "IntegrationPending",
                    StringComparison.Ordinal);
            if (!lessonRoutable || integrationPending)
                continue;

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
        // Fourth Courses contract models exist for independently reviewed
        // generators but are not available to learners until their owning lesson
        // is READY_VERIFIED. These interaction types never grant capability.
        if (familyId.StartsWith("usccss.hs.matrix.", StringComparison.Ordinal))
            return LessonPracticeInteractionKind.StructuredEntry;
        if (familyId == "usccss.hs.log.compare_order")
            return LessonPracticeInteractionKind.RelationChoice;
        if (familyId is "usccss.hs.log.inverse_power" or
            "solid_geometry.cavalieri_sphere_volume" or
            "coordinates.focal_distance_conics" or "trig.unit_circle_reflection_exact" or
            "trig.principal_inverse")
            return LessonPracticeInteractionKind.NumericEntry;
        if (familyId == "trig.model_all_time_solutions")
            return LessonPracticeInteractionKind.StructuredEntry;

        // U06 responses are keyed LONGER/DIFFERENCE/RESULT/EQUATION,
        // never bare <,>,= choice; center/manual measurement needs a teacher.
        if(familyId.StartsWith("usccss.g1.u06.l",StringComparison.Ordinal) &&
           answerType=="contract_defined")
            return LessonPracticeInteractionKind.ContractDriven;

        // G1 U08 has a keyed DIFFERENCE=number Compare story, not
        // the generic symbol-only relation choice interface.
        if (familyId=="usccss.g1.u08.l06.compare_difference_story" &&
            answerType=="contract_defined")
            return LessonPracticeInteractionKind.ContractDriven;

        // G1 U04 L16-L23 answers are keyed strings (SYMBOL, ORDER, TENS,
        // ONES, COUNT/GAP), not the bare relation-choice widget. Keep the
        // original exact answer contract for the source-goal digital tasks.
        if (familyId.StartsWith("usccss.g1.u04.l",StringComparison.Ordinal) &&
            int.TryParse(familyId.AsSpan("usccss.g1.u04.l".Length,2),out var u04Number) &&
            u04Number>=16&&u04Number<=23&&answerType=="contract_defined")
            return LessonPracticeInteractionKind.ContractDriven;

        // Source-specific G1 two-digit comparison prompts use structured
        // RELATION=GREATER/LESS/EQUAL or SYMBOL=<,>,= entries, not a single
        // RelationChoice response. Preserve that real grading contract.
        if ((familyId.StartsWith("usccss.g1.u04.l14.",StringComparison.Ordinal) ||
             familyId.StartsWith("usccss.g1.u04.l15.",StringComparison.Ordinal)) &&
            answerType == "contract_defined")
            return LessonPracticeInteractionKind.ContractDriven;

        // Structured comparison families may share a ".compare" family prefix
        // without using the < = > response shape. Resolve their declared answer
        // contract before the generic comparison fallback.
        if ((familyId.StartsWith("usccss.g1.u03.l09.",StringComparison.Ordinal) ||
             familyId.StartsWith("usccss.g1.u03.l10.",StringComparison.Ordinal) ||
             familyId.StartsWith("usccss.g1.u03.l11.",StringComparison.Ordinal) ||
             familyId.StartsWith("usccss.g1.u03.l12.",StringComparison.Ordinal) ||
             familyId.StartsWith("usccss.g1.u03.l13.",StringComparison.Ordinal)) &&
            answerType == "contract_defined")
            return LessonPracticeInteractionKind.ContractDriven;

        if ((familyId.StartsWith("usccss.g1.u03.l04.",StringComparison.Ordinal) ||
             familyId.StartsWith("usccss.g1.u03.l05.",StringComparison.Ordinal) ||
             familyId.StartsWith("usccss.g1.u03.l06.",StringComparison.Ordinal) ||
             familyId.StartsWith("usccss.g1.u03.l07.",StringComparison.Ordinal) ||
             familyId.StartsWith("usccss.g1.u03.l08.",StringComparison.Ordinal)) &&
            answerType == "contract_defined")
            return LessonPracticeInteractionKind.ContractDriven;

        if ((familyId.StartsWith("usccss.g1.u02.l21.",StringComparison.Ordinal) ||
             familyId.StartsWith("usccss.g1.u02.l22.",StringComparison.Ordinal) ||
             familyId.StartsWith("usccss.g1.u03.l01.",StringComparison.Ordinal) ||
             familyId.StartsWith("usccss.g1.u03.l02.",StringComparison.Ordinal) ||
             familyId.StartsWith("usccss.g1.u03.l03.",StringComparison.Ordinal)) &&
            answerType == "contract_defined")
            return LessonPracticeInteractionKind.ContractDriven;

        if ((familyId.StartsWith("usccss.g1.u02.story.",StringComparison.Ordinal) ||
             familyId == "usccss.g1.u02.story.compare_unknowns_equations" ||
             familyId.StartsWith("usccss.g1.u02.compare.",StringComparison.Ordinal)) &&
            answerType == "contract_defined")
            return LessonPracticeInteractionKind.ContractDriven;

        // G2 U01 L18 category comparisons ask for an exact count/gap,
        // not a <, = or > relation. Numeric answerType takes precedence.
        if (familyId.StartsWith("usccss.g2.u01.l18.",StringComparison.Ordinal) &&
            answerType=="exact_integer")
            return LessonPracticeInteractionKind.IntegerKeypad;

        // G2 U02 L03 compare-gap answers are integers, not bare <,=,>.
        if(familyId.StartsWith("usccss.g2.u02.l03.",StringComparison.Ordinal) &&
           answerType=="exact_integer")
            return LessonPracticeInteractionKind.IntegerKeypad;

        // Declared numeric answer type is authoritative. Families named compare*
        // may ask for a numerical gap, not a <, =, > relation.
        if (answerType=="exact_integer")
            return LessonPracticeInteractionKind.IntegerKeypad;

        if (answerType is "side_label" or "boolean" or "ordered_labels")
            return LessonPracticeInteractionKind.StructuredEntry;

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
