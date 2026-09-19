using System.Reflection;
using System.Text.Json;

namespace Edulytics.Core.Mathematics.Practice;

/// <summary>
/// Projects unresolved Supporting lesson targets into unique target-specific exact
/// Practice contracts. Explicit READY_VERIFIED mappings always win. This
/// projection never creates official curriculum OutcomeCodes.
/// </summary>
internal static class SupportingPracticeTargetContractProjection
{
    public const string ContractVersion = "supporting-target-manifest-v1";

    public static IReadOnlyList<LessonPracticeContract> Load()
    {
        var assembly = typeof(SupportingPracticeTargetContractProjection).Assembly;
        var projected = new Dictionary<string, LessonPracticeContract>(StringComparer.Ordinal);

        foreach (var resourceName in assembly
            .GetManifestResourceNames()
            .Where(name => name.EndsWith(".lesson-content-pack.json", StringComparison.Ordinal))
            .OrderBy(name => name, StringComparer.Ordinal))
        {
            using var stream = assembly.GetManifestResourceStream(resourceName);
            if (stream is null)
                continue;

            using var document = JsonDocument.Parse(stream);
            if (!TryProperty(document.RootElement, "Lessons", "lessons", out var lessons) ||
                lessons.ValueKind != JsonValueKind.Array)
            {
                continue;
            }

            foreach (var lesson in lessons.EnumerateArray())
            {
                if (!TryString(lesson, "LessonCode", "lessonCode", out var lessonCode) ||
                    lessonCode.Length == 0 ||
                    SupportingPracticeTargetResolver.IsExplicitReadyLesson(lessonCode))
                {
                    continue;
                }

                if (HasOfficialOutcomes(lesson))
                    continue;

                var title = ReadPreferredTitle(lesson);
                if (!SupportingPracticeTargetResolver.TryResolve(title, out var spec) ||
                    spec is null)
                {
                    continue;
                }

                projected[lessonCode] = new LessonPracticeContract(
                    lessonCode,
                    spec.SkillId,
                    spec.Mechanic,
                    [spec.FamilyId],
                    "SupportingTargetManifest",
                    "READY_VERIFIED",
                    ContractVersion);
            }
        }

        return projected.Values
            .OrderBy(contract => contract.LessonCode, StringComparer.Ordinal)
            .ToArray();
    }

    private static bool HasOfficialOutcomes(JsonElement lesson)
    {
        if (!TryProperty(lesson, "OutcomeCodes", "outcomeCodes", out var outcomes) ||
            outcomes.ValueKind != JsonValueKind.Array)
        {
            return false;
        }

        return outcomes.EnumerateArray().Any(value =>
            value.ValueKind == JsonValueKind.String &&
            !string.IsNullOrWhiteSpace(value.GetString()));
    }

    private static string ReadPreferredTitle(JsonElement lesson)
    {
        if (TryProperty(lesson, "Translations", "translations", out var translations) &&
            translations.ValueKind == JsonValueKind.Array)
        {
            JsonElement? fallback = null;
            foreach (var translation in translations.EnumerateArray())
            {
                fallback ??= translation;
                if (TryString(translation, "CultureCode", "cultureCode", out var culture) &&
                    culture.StartsWith("en", StringComparison.OrdinalIgnoreCase) &&
                    TryString(translation, "Title", "title", out var englishTitle))
                {
                    return englishTitle;
                }
            }

            if (fallback is JsonElement first &&
                TryString(first, "Title", "title", out var fallbackTitle))
            {
                return fallbackTitle;
            }
        }

        return TryString(lesson, "Title", "title", out var title)
            ? title
            : string.Empty;
    }

    private static bool TryString(
        JsonElement element,
        string first,
        string second,
        out string value)
    {
        value = string.Empty;
        if (!TryProperty(element, first, second, out var node) ||
            node.ValueKind != JsonValueKind.String)
        {
            return false;
        }

        value = node.GetString()?.Trim() ?? string.Empty;
        return true;
    }

    private static bool TryProperty(
        JsonElement element,
        string first,
        string second,
        out JsonElement value)
    {
        if (element.TryGetProperty(first, out value))
            return true;
        return element.TryGetProperty(second, out value);
    }
}
