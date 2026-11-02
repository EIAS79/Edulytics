using System.Reflection;
using System.Text.Json;

namespace Edulytics.Core.Mathematics.Practice;

public sealed record OfficialOutcomePracticeMapEntry(
    string OutcomeCode,
    string TargetRuleId,
    string ResolutionRuleId,
    int Priority,
    string Evidence);

/// <summary>
/// Deterministic, reviewed mapping from accepted official curriculum outcomes
/// to reusable Practice target rules. The materialized map is generated only
/// from accepted curriculum-pack identities/text and reviewed rule definitions.
/// Missing outcomes fail closed.
/// </summary>
public static class OfficialOutcomePracticeMapRegistry
{
    private const string ResourceName =
        "Edulytics.Core.Mathematics.Curriculum.official-outcome-practice-map.v1.json";

    private static readonly Lazy<IReadOnlyDictionary<string, OfficialOutcomePracticeMapEntry>>
        ByOutcomeCode = new(Load);

    public static IReadOnlyList<OfficialOutcomePracticeMapEntry> All =>
        ByOutcomeCode.Value.Values
            .OrderBy(x => x.OutcomeCode, StringComparer.Ordinal)
            .ToArray();

    public static bool TryResolve(
        string? outcomeCode,
        out OfficialOutcomePracticeMapEntry? entry)
    {
        if (string.IsNullOrWhiteSpace(outcomeCode))
        {
            entry = null;
            return false;
        }

        return ByOutcomeCode.Value.TryGetValue(
            outcomeCode.Trim(),
            out entry);
    }

    private static IReadOnlyDictionary<string, OfficialOutcomePracticeMapEntry> Load()
    {
        var assembly = typeof(OfficialOutcomePracticeMapRegistry).Assembly;
        using var stream = assembly.GetManifestResourceStream(ResourceName)
            ?? throw new InvalidOperationException(
                $"Missing embedded official outcome Practice map: {ResourceName}.");

        using var document = JsonDocument.Parse(stream);
        var result = new Dictionary<string, OfficialOutcomePracticeMapEntry>(
            StringComparer.Ordinal);

        foreach (var row in document.RootElement.GetProperty("entries").EnumerateArray())
        {
            var outcomeCode = Required(row, "outcomeCode");
            var targetRuleId = Required(row, "targetRuleId");
            var resolutionRuleId = Required(row, "resolutionRuleId");
            var priority = row.GetProperty("priority").GetInt32();
            var evidence = Required(row, "evidence");

            if (!SupportingPracticeTargetRuleRegistry.TryGetById(
                    targetRuleId,
                    out _))
            {
                throw new InvalidOperationException(
                    $"Official outcome Practice map references unknown target rule {targetRuleId}.");
            }

            if (!result.TryAdd(
                    outcomeCode,
                    new OfficialOutcomePracticeMapEntry(
                        outcomeCode,
                        targetRuleId,
                        resolutionRuleId,
                        priority,
                        evidence)))
            {
                throw new InvalidOperationException(
                    $"Duplicate official outcome Practice mapping: {outcomeCode}.");
            }
        }

        return result;
    }

    private static string Required(JsonElement row, string propertyName)
    {
        var value = row.GetProperty(propertyName).GetString()?.Trim();
        return string.IsNullOrWhiteSpace(value)
            ? throw new InvalidOperationException(
                $"Official outcome Practice map has blank {propertyName}.")
            : value;
    }
}
