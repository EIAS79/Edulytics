using System.Reflection;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Edulytics.Core.Mathematics.Practice;

public sealed record OfficialOutcomePracticeResolution(
    string OutcomeCode,
    string RuleId,
    SupportingPracticeTargetRule TargetRule);

/// <summary>
/// Shared fail-closed authority that maps official curriculum outcome codes to
/// the already reviewed Practice target rules. Resolution is based on explicit
/// code scope plus official outcome text where available. Only the unique
/// highest-priority rule may authorize a mapping; ties are configuration errors.
/// </summary>
public static class OfficialOutcomePracticeRuleRegistry
{
    private const string ResourceName =
        "Edulytics.Core.Mathematics.Curriculum.official-outcome-practice-rules.v1.json";

    private static readonly Lazy<IReadOnlyDictionary<string, OfficialOutcomePracticeResolution>>
        Resolutions = new(Load);

    public static IReadOnlyDictionary<string, OfficialOutcomePracticeResolution> All =>
        Resolutions.Value;

    public static bool TryResolve(
        string? outcomeCode,
        out OfficialOutcomePracticeResolution? resolution)
    {
        if (string.IsNullOrWhiteSpace(outcomeCode))
        {
            resolution = null;
            return false;
        }

        return Resolutions.Value.TryGetValue(outcomeCode.Trim(), out resolution);
    }

    private static IReadOnlyDictionary<string, OfficialOutcomePracticeResolution> Load()
    {
        var assembly = typeof(OfficialOutcomePracticeRuleRegistry).Assembly;
        using var stream = assembly.GetManifestResourceStream(ResourceName)
            ?? throw new InvalidOperationException(
                $"Missing embedded official Practice outcome registry: {ResourceName}.");
        using var document = JsonDocument.Parse(stream);

        var targetRules = SupportingPracticeTargetRuleRegistry.All
            .ToDictionary(x => x.Id, StringComparer.Ordinal);
        var compiled = new List<CompiledRule>();
        var ids = new HashSet<string>(StringComparer.Ordinal);

        foreach (var row in document.RootElement.GetProperty("rules").EnumerateArray())
        {
            var id = Required(row, "id");
            if (!ids.Add(id))
                throw new InvalidOperationException($"Duplicate official Practice rule id: {id}.");

            var priority = row.GetProperty("priority").GetInt32();
            var targetRuleId = Required(row, "targetRuleId");
            if (!targetRules.TryGetValue(targetRuleId, out var targetRule))
            {
                throw new InvalidOperationException(
                    $"Official Practice rule {id} references unknown target rule {targetRuleId}.");
            }

            var codePatterns = Compile(ReadList(row, "codePatterns"), id, "codePatterns");
            var textPatterns = Compile(ReadList(row, "textPatterns"), id, "textPatterns");
            if (codePatterns.Count == 0)
            {
                throw new InvalidOperationException(
                    $"Official Practice rule {id} must have explicit OutcomeCode scope.");
            }

            compiled.Add(new CompiledRule(
                id,
                priority,
                codePatterns,
                textPatterns,
                targetRule));
        }

        var outcomeEvidence = LoadOfficialOutcomeEvidence(assembly);
        var result = new Dictionary<string, OfficialOutcomePracticeResolution>(
            StringComparer.Ordinal);

        foreach (var pair in outcomeEvidence.OrderBy(x => x.Key, StringComparer.Ordinal))
        {
            var code = pair.Key;
            var evidence = pair.Value;
            var matches = compiled
                .Where(rule =>
                    rule.CodePatterns.Any(pattern => pattern.IsMatch(code)) &&
                    (rule.TextPatterns.Count == 0 ||
                     rule.TextPatterns.Any(pattern => pattern.IsMatch(evidence))))
                .ToArray();

            if (matches.Length == 0)
                continue;

            var highestPriority = matches.Max(x => x.Priority);
            var winners = matches
                .Where(x => x.Priority == highestPriority)
                .ToArray();

            if (winners.Length != 1)
            {
                throw new InvalidOperationException(
                    $"Official Practice outcome mapping is ambiguous for {code}: " +
                    string.Join(", ", winners.Select(x => x.Id)));
            }

            var winner = winners[0];
            result[code] = new OfficialOutcomePracticeResolution(
                code,
                winner.Id,
                winner.TargetRule);
        }

        return result;
    }

    private static IReadOnlyDictionary<string, string> LoadOfficialOutcomeEvidence(
        Assembly assembly)
    {
        var result = new Dictionary<string, string>(StringComparer.Ordinal);

        foreach (var resource in assembly.GetManifestResourceNames()
                     .Where(name => name.EndsWith(
                         ".curriculum-pack.json",
                         StringComparison.OrdinalIgnoreCase))
                     .OrderBy(name => name, StringComparer.Ordinal))
        {
            using var stream = assembly.GetManifestResourceStream(resource);
            if (stream is null)
                continue;
            using var document = JsonDocument.Parse(stream);
            if (!TryGetCase(document.RootElement, "Nodes", "nodes", out var nodes) ||
                nodes.ValueKind != JsonValueKind.Array)
            {
                continue;
            }

            foreach (var node in nodes.EnumerateArray())
            {
                var code = ReadCaseString(node, "Code", "code");
                if (string.IsNullOrWhiteSpace(code))
                    continue;

                var kind = ReadCaseString(node, "Kind", "kind");
                if (!string.Equals(kind, "Standard", StringComparison.OrdinalIgnoreCase) &&
                    !string.Equals(kind, "Outcome", StringComparison.OrdinalIgnoreCase) &&
                    !string.Equals(kind, "Reference", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                var evidence = string.Join(
                    " ",
                    ReadCaseString(node, "Title", "title"),
                    ReadCaseString(node, "OfficialText", "officialText"),
                    ReadCaseString(node, "AuthorDescription", "authorDescription"));
                evidence = Regex.Replace(evidence, @"\s+", " ").Trim();

                result.TryAdd(code, evidence);
            }
        }

        return result;
    }

    private static IReadOnlyList<Regex> Compile(
        IReadOnlyList<string> patterns,
        string ruleId,
        string field) =>
        patterns.Select(pattern =>
        {
            try
            {
                return new Regex(
                    pattern,
                    RegexOptions.IgnoreCase |
                    RegexOptions.CultureInvariant |
                    RegexOptions.Compiled);
            }
            catch (ArgumentException ex)
            {
                throw new InvalidOperationException(
                    $"Invalid regex in official Practice rule {ruleId}.{field}: {pattern}",
                    ex);
            }
        }).ToArray();

    private static IReadOnlyList<string> ReadList(JsonElement row, string name) =>
        row.TryGetProperty(name, out var node) && node.ValueKind == JsonValueKind.Array
            ? node.EnumerateArray()
                .Select(value => value.GetString()?.Trim())
                .Where(value => !string.IsNullOrWhiteSpace(value))
                .Cast<string>()
                .ToArray()
            : [];

    private static string Required(JsonElement row, string name)
    {
        if (!row.TryGetProperty(name, out var node))
            throw new InvalidOperationException($"Official Practice rule field missing: {name}.");
        var value = node.GetString()?.Trim();
        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidOperationException($"Official Practice rule field blank: {name}.");
        return value;
    }

    private static bool TryGetCase(
        JsonElement row,
        string upper,
        string lower,
        out JsonElement value)
    {
        if (row.TryGetProperty(upper, out value))
            return true;
        return row.TryGetProperty(lower, out value);
    }

    private static string ReadCaseString(
        JsonElement row,
        string upper,
        string lower)
    {
        if (!TryGetCase(row, upper, lower, out var value) ||
            value.ValueKind is JsonValueKind.Null or JsonValueKind.Undefined)
        {
            return string.Empty;
        }

        return value.ValueKind == JsonValueKind.String
            ? value.GetString()?.Trim() ?? string.Empty
            : value.ToString().Trim();
    }

    private sealed record CompiledRule(
        string Id,
        int Priority,
        IReadOnlyList<Regex> CodePatterns,
        IReadOnlyList<Regex> TextPatterns,
        SupportingPracticeTargetRule TargetRule);
}
