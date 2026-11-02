using System.Reflection;
using System.Text.Json;

namespace Edulytics.Core.Mathematics.Practice;

public sealed record PolishOutcomePracticeMapping(
    string OutcomeCode,
    int Serial,
    IReadOnlyList<string> TargetRuleIds,
    IReadOnlyList<SupportingPracticeTargetRule> TargetRules,
    string SourceUrl,
    string SourceLocator,
    string SourceContentHash,
    int LogicalLevelFrom,
    int LogicalLevelTo,
    string? Pathway,
    string OfficialDomain);

/// <summary>
/// Exact, fail-closed authority for the Polish 2025/2026 outcome-backed
/// learner lessons. The mapping key is the accepted official OutcomeCode;
/// generated Phase-29 lesson titles are never mapping authority.
/// </summary>
public static class PolishOutcomePracticeMapRegistry
{
    private const string ResourceName =
        "Edulytics.Core.Mathematics.Curriculum.polish-outcome-practice-map.v1.json";

    private static readonly Lazy<IReadOnlyDictionary<string, PolishOutcomePracticeMapping>>
        Mappings = new(Load);

    public static IReadOnlyDictionary<string, PolishOutcomePracticeMapping> All =>
        Mappings.Value;

    public static bool TryResolve(
        string? outcomeCode,
        out PolishOutcomePracticeMapping? mapping)
    {
        if (string.IsNullOrWhiteSpace(outcomeCode))
        {
            mapping = null;
            return false;
        }

        return Mappings.Value.TryGetValue(outcomeCode.Trim(), out mapping);
    }

    private static IReadOnlyDictionary<string, PolishOutcomePracticeMapping> Load()
    {
        var assembly = typeof(PolishOutcomePracticeMapRegistry).Assembly;
        using var stream = assembly.GetManifestResourceStream(ResourceName)
            ?? throw new InvalidOperationException(
                $"Missing embedded Polish Practice map: {ResourceName}.");
        using var document = JsonDocument.Parse(stream);

        var root = document.RootElement;
        var declaredCount = root.GetProperty("outcomeCount").GetInt32();
        var targetById = SupportingPracticeTargetRuleRegistry.All
            .ToDictionary(x => x.Id, StringComparer.Ordinal);
        var result = new Dictionary<string, PolishOutcomePracticeMapping>(
            StringComparer.Ordinal);
        var serials = new HashSet<int>();

        foreach (var row in root.GetProperty("entries").EnumerateArray())
        {
            var code = Required(row, "outcomeCode");
            var serial = row.GetProperty("serial").GetInt32();
            if (!serials.Add(serial))
                throw new InvalidOperationException(
                    $"Duplicate Polish Practice outcome serial: {serial}.");
            if (serial is < 1 or > 306)
                throw new InvalidOperationException(
                    $"Invalid Polish Practice outcome serial {serial} for {code}.");

            var targetIds = row.GetProperty("targetRuleIds")
                .EnumerateArray()
                .Select(x => x.GetString()?.Trim())
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Cast<string>()
                .Distinct(StringComparer.Ordinal)
                .ToArray();
            if (targetIds.Length == 0)
                throw new InvalidOperationException(
                    $"Polish Practice outcome {code} has no target rules.");

            var targetRules = targetIds.Select(id =>
            {
                if (!targetById.TryGetValue(id, out var rule))
                    throw new InvalidOperationException(
                        $"Polish Practice outcome {code} references unknown target rule {id}.");
                return rule;
            }).ToArray();

            var mapping = new PolishOutcomePracticeMapping(
                code,
                serial,
                targetIds,
                targetRules,
                Required(row, "sourceUrl"),
                Required(row, "sourceLocator"),
                Required(row, "sourceContentHash"),
                row.GetProperty("logicalLevelFrom").GetInt32(),
                row.GetProperty("logicalLevelTo").GetInt32(),
                Optional(row, "pathway"),
                Required(row, "officialDomain"));

            if (!result.TryAdd(code, mapping))
                throw new InvalidOperationException(
                    $"Duplicate Polish Practice OutcomeCode: {code}.");
        }

        if (declaredCount != 306 ||
            result.Count != declaredCount ||
            serials.Count != declaredCount)
        {
            throw new InvalidOperationException(
                $"Polish Practice map must contain exactly 306 unique outcomes; " +
                $"declared={declaredCount}, loaded={result.Count}, serials={serials.Count}.");
        }

        for (var serial = 1; serial <= 306; serial++)
        {
            if (!serials.Contains(serial))
                throw new InvalidOperationException(
                    $"Polish Practice map is missing serial {serial}.");
        }

        return result;
    }

    private static string Required(JsonElement row, string name)
    {
        if (!row.TryGetProperty(name, out var node))
            throw new InvalidOperationException(
                $"Polish Practice map field missing: {name}.");
        var value = node.GetString()?.Trim();
        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidOperationException(
                $"Polish Practice map field blank: {name}.");
        return value;
    }

    private static string? Optional(JsonElement row, string name)
    {
        if (!row.TryGetProperty(name, out var node) ||
            node.ValueKind is JsonValueKind.Null or JsonValueKind.Undefined)
        {
            return null;
        }

        var value = node.GetString()?.Trim();
        return string.IsNullOrWhiteSpace(value) ? null : value;
    }
}
