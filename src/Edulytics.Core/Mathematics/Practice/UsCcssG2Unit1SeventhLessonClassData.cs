using System.Globalization;

namespace Edulytics.Core.Mathematics.Practice;

/// <summary>
/// Original Edulytics Grade 2 U01 L07 class data organizer and Cool-down.
/// Uses ONLY classroom observations supplied by the educator/learner; it
/// NEVER manufactures classmates, responses, dates, or source figures.
/// Exactly one transport choice per observed response and no names/IDs.
/// </summary>
public static class UsCcssG2Unit1SeventhLessonClassData
{
    public const string LessonCode = "PED:US-CCSS-MATH:G2:U01:L07";
    public const string Version = "edulytics-g2-u01-l07-authentic-class-data-v1";

    public static readonly IReadOnlyList<string> Categories =
        ["Walk", "Car", "Train", "School bus"];

    public sealed record RecordResult(
        bool Accepted, string Feedback,
        IReadOnlyList<string> OriginalResponses,
        IReadOnlyDictionary<string, int> Counts);

    public sealed record Review(
        bool MachineScorable, bool Correct, bool TeacherReviewNeeded, string Reason);

    /// <summary>
    /// Source's initial class display is intentionally allowed to be
    /// unordered. Preserve the exact category sequence for comparison with
    /// the organized representation, but never accept student names.
    /// </summary>
    public static RecordResult ParseActualResponses(string? text)
    {
        var empty = new Dictionary<string, int>(StringComparer.Ordinal);
        foreach (var category in Categories) empty[category] = 0;
        if (string.IsNullOrWhiteSpace(text) || text.Length > 2000)
            return new(false, "Enter genuine anonymous classroom travel categories, up to 60 responses.", [], empty);
        var parts = text.Split([',', '\n', ';'], StringSplitOptions.TrimEntries |
            StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length is < 1 or > 60)
            return new(false, "Supply between 1 and 60 observed class responses.", [], empty);
        var original = new List<string>(parts.Length);
        foreach (var part in parts)
        {
            var normalized = part.Trim().ToLowerInvariant() switch
            {
                "walk" or "walking" or "on foot" => "Walk",
                "car" => "Car",
                "train" => "Train",
                "bus" or "school bus" => "School bus",
                _ => null
            };
            if (normalized is null)
                return new(false,
                    "Only anonymous observed Walk, Car, Train or School bus categories are accepted; don't include pupil names or invented travel categories.",
                    [], empty);
            original.Add(normalized);
            empty[normalized]++;
        }
        return new(true, "Class response categories grouped without changing the original observations.",
            original, empty);
    }

    public static Review VerifyFourCategoryRepresentation(
        RecordResult source, string? walking, string? cars,
        string? trains, string? buses)
    {
        if (!source.Accepted)
            return new(false, false, true, "Real class responses must be entered before regrouping.");
        var raw = new[] { walking, cars, trains, buses };
        if (raw.Any(x => !int.TryParse(x, NumberStyles.None,
            CultureInfo.InvariantCulture, out _)))
            return new(true, false, true, "Write one non-negative count for every category.");
        var counts = raw.Select(x => int.Parse(x!, CultureInfo.InvariantCulture)).ToArray();
        if (counts.Any(x => x > 60))
            return new(true, false, true, "A category count cannot exceed 60.");
        var allMatch = Categories.Select((name, i) =>
            counts[i] == source.Counts[name]).All(correct => correct);
        return new(true, allMatch, true, allMatch
            ? "The grouped visual has exactly the same category totals as the initial unorganized responses."
            : "Recount each real response exactly once, without dropping or duplicating an observation.");
    }

    public static Review VerifyOriginalCoolDown(
        RecordResult source, string? total, string? bus, string? carTrainDifference)
    {
        if (!source.Accepted)
            return new(false, false, true, "No authentic class observations were supplied.");
        var raw = new[] { total, bus, carTrainDifference };
        if (raw.Any(x => !int.TryParse(x, NumberStyles.None,
            CultureInfo.InvariantCulture, out _)))
            return new(true, false, false, "Enter three non-negative whole numbers.");
        var values = raw.Select(x => int.Parse(x!, CultureInfo.InvariantCulture)).ToArray();
        var correct = values[0] == source.OriginalResponses.Count &&
            values[1] == source.Counts["School bus"] &&
            values[2] == Math.Abs(source.Counts["Car"] - source.Counts["Train"]);
        return new(true, correct, false, correct
            ? "These answers agree with the authentic classroom travel display."
            : "Count all responses, the buses, then compare car and train category counts.");
    }

    public static readonly IReadOnlyList<string> TeacherRubric =
    [
        "Each student chooses an honest way of arriving today; the teacher collects one real response per participating learner. No sample data masquerades as class results.",
        "The initial free-form class display is accepted without forced category grouping, just as in the original activity.",
        "The student reorganizes the SAME observations into groups, pictures, tally marks or labeled symbols while preserving each original category total.",
        "The student explains why the new representation is easier to read, and compares it with another student's valid representation.",
        "Teacher evaluates real collaboration, picture/tally placement, student-created evidence and explanation; numeric checks alone do not certify this."
    ];
}
