using System.Reflection;
using System.Text.Json;

namespace Edulytics.Core.Mathematics.Skills;

public sealed record MathematicsSkillMetadata(
    string Id,
    string Domain,
    string CanonicalName,
    IReadOnlyList<string> Prerequisites);

/// <summary>
/// Read-only authority over the embedded curriculum-neutral Mathematics skill
/// registry. Evaluation and intervention code use this registry for names and
/// prerequisite relationships instead of inventing skill dependencies.
/// </summary>
public static class MathematicsSkillMetadataRegistry
{
    private const string ResourceName =
        "Edulytics.Core.Mathematics.Skills.skill-registry.v1.json";

    private static readonly Lazy<IReadOnlyDictionary<string, MathematicsSkillMetadata>>
        ById = new(Load);

    public static IReadOnlyList<MathematicsSkillMetadata> All =>
        ById.Value.Values
            .OrderBy(x => x.Id, StringComparer.Ordinal)
            .ToArray();

    public static bool TryResolve(
        string? skillId,
        out MathematicsSkillMetadata? metadata)
    {
        if (string.IsNullOrWhiteSpace(skillId))
        {
            metadata = null;
            return false;
        }

        return ById.Value.TryGetValue(
            skillId.Trim(),
            out metadata);
    }

    public static int DependentCount(string? skillId)
    {
        if (string.IsNullOrWhiteSpace(skillId))
            return 0;

        var key = skillId.Trim();
        return ById.Value.Values.Count(
            x => x.Prerequisites.Contains(
                key,
                StringComparer.Ordinal));
    }

    private static IReadOnlyDictionary<string, MathematicsSkillMetadata> Load()
    {
        var assembly = typeof(MathematicsSkillMetadataRegistry).Assembly;
        using var stream = assembly.GetManifestResourceStream(ResourceName)
            ?? throw new InvalidOperationException(
                $"Missing embedded Mathematics skill registry: {ResourceName}.");

        using var document = JsonDocument.Parse(stream);
        var result = new Dictionary<string, MathematicsSkillMetadata>(
            StringComparer.Ordinal);

        foreach (var row in document.RootElement
                     .GetProperty("skills")
                     .EnumerateArray())
        {
            var id = Required(row, "id");
            var domain = Required(row, "domain");
            var canonicalName = Required(row, "canonicalName");
            var prerequisites =
                row.TryGetProperty("prerequisites", out var prerequisiteNode) &&
                prerequisiteNode.ValueKind == JsonValueKind.Array
                    ? prerequisiteNode
                        .EnumerateArray()
                        .Select(x => x.GetString()?.Trim())
                        .Where(x => !string.IsNullOrWhiteSpace(x))
                        .Cast<string>()
                        .Distinct(StringComparer.Ordinal)
                        .ToArray()
                    : [];

            if (!result.TryAdd(
                    id,
                    new MathematicsSkillMetadata(
                        id,
                        domain,
                        canonicalName,
                        prerequisites)))
            {
                throw new InvalidOperationException(
                    $"Duplicate Mathematics skill registry id: {id}.");
            }
        }

        return result;
    }

    private static string Required(
        JsonElement row,
        string propertyName)
    {
        if (!row.TryGetProperty(propertyName, out var node))
        {
            throw new InvalidOperationException(
                $"Mathematics skill registry field missing: {propertyName}.");
        }

        var value = node.GetString()?.Trim();
        return string.IsNullOrWhiteSpace(value)
            ? throw new InvalidOperationException(
                $"Mathematics skill registry field blank: {propertyName}.")
            : value;
    }
}
