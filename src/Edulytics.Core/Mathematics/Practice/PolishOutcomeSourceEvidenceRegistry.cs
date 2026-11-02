using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace Edulytics.Core.Mathematics.Practice;

public sealed record PolishOutcomeSourceEvidence(
    string OutcomeCode,
    string OfficialText,
    string OfficialTextSha256);

/// <summary>
/// Pinned official Polish Mathematics outcome evidence reconstructed from the
/// accepted ELI sources. Raw legal text is evidence, not learner-facing prose.
/// </summary>
public static class PolishOutcomeSourceEvidenceRegistry
{
    private const string ResourceName =
        "Edulytics.Core.Mathematics.Curriculum.polish-outcome-source-evidence.v1.json";

    private static readonly Lazy<IReadOnlyDictionary<string, PolishOutcomeSourceEvidence>>
        Entries = new(Load);

    public static IReadOnlyDictionary<string, PolishOutcomeSourceEvidence> All =>
        Entries.Value;

    public static bool TryResolve(
        string? outcomeCode,
        out PolishOutcomeSourceEvidence? evidence)
    {
        if (string.IsNullOrWhiteSpace(outcomeCode))
        {
            evidence = null;
            return false;
        }

        return Entries.Value.TryGetValue(outcomeCode.Trim(), out evidence);
    }

    private static IReadOnlyDictionary<string, PolishOutcomeSourceEvidence> Load()
    {
        var assembly = typeof(PolishOutcomeSourceEvidenceRegistry).Assembly;
        using var stream = assembly.GetManifestResourceStream(ResourceName)
            ?? throw new InvalidOperationException(
                $"Missing embedded Polish Outcome source evidence: {ResourceName}.");
        using var document = JsonDocument.Parse(stream);

        var declaredCount = document.RootElement
            .GetProperty("outcomeCount")
            .GetInt32();
        var result = new Dictionary<string, PolishOutcomeSourceEvidence>(
            StringComparer.Ordinal);

        foreach (var row in document.RootElement.GetProperty("entries").EnumerateArray())
        {
            var code = Required(row, "outcomeCode");
            var text = Required(row, "officialText");
            var expectedHash = Required(row, "officialTextSha256").ToLowerInvariant();
            var actualHash = Convert
                .ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(text)))
                .ToLowerInvariant();

            if (!string.Equals(expectedHash, actualHash, StringComparison.Ordinal))
            {
                throw new InvalidOperationException(
                    $"Polish Outcome evidence hash mismatch for {code}.");
            }

            if (!result.TryAdd(
                    code,
                    new PolishOutcomeSourceEvidence(code, text, expectedHash)))
            {
                throw new InvalidOperationException(
                    $"Duplicate Polish Outcome source evidence: {code}.");
            }
        }

        if (declaredCount != 306 || result.Count != 306)
        {
            throw new InvalidOperationException(
                $"Polish Outcome source evidence must contain exactly 306 outcomes; " +
                $"declared={declaredCount}, loaded={result.Count}.");
        }

        var mappedCodes = PolishOutcomePracticeMapRegistry.All.Keys
            .OrderBy(x => x, StringComparer.Ordinal)
            .ToArray();
        var evidenceCodes = result.Keys
            .OrderBy(x => x, StringComparer.Ordinal)
            .ToArray();
        if (!mappedCodes.SequenceEqual(evidenceCodes, StringComparer.Ordinal))
        {
            throw new InvalidOperationException(
                "Polish Practice map and official source evidence do not cover the same OutcomeCodes.");
        }

        return result;
    }

    private static string Required(JsonElement row, string name)
    {
        if (!row.TryGetProperty(name, out var node))
            throw new InvalidOperationException(
                $"Polish Outcome evidence field missing: {name}.");

        var value = node.GetString()?.Trim();
        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidOperationException(
                $"Polish Outcome evidence field blank: {name}.");

        return value;
    }
}
