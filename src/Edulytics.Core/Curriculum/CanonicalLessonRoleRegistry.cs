using System.Text.Json;
using System.Text.Json.Serialization;

namespace Edulytics.Core.Curriculum;

/// <summary>
/// Runtime lesson-role registry sourced from the reviewed embedded canonical
/// content packs. A lesson can be a primary curriculum lesson without claiming
/// a formal outcome mapping; Supporting is an explicit editorial role.
/// </summary>
public static class CanonicalLessonRoleRegistry
{
    private static readonly Lazy<IReadOnlyDictionary<string, bool>> Roles =
        new(Build, LazyThreadSafetyMode.ExecutionAndPublication);

    public static bool TryGetIsSupporting(
        string? lessonCode,
        out bool isSupporting)
    {
        if (string.IsNullOrWhiteSpace(lessonCode))
        {
            isSupporting = false;
            return false;
        }

        return Roles.Value.TryGetValue(
            lessonCode.Trim(),
            out isSupporting);
    }

    private static IReadOnlyDictionary<string, bool> Build()
    {
        var assembly = typeof(CanonicalLessonRoleRegistry).Assembly;
        var result = new Dictionary<string, bool>(StringComparer.Ordinal);
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        options.Converters.Add(new JsonStringEnumConverter());

        foreach (var resourceName in assembly
            .GetManifestResourceNames()
            .Where(x => x.EndsWith(
                ".lesson-content-pack.json",
                StringComparison.Ordinal)))
        {
            using var stream = assembly.GetManifestResourceStream(resourceName)
                ?? throw new InvalidOperationException(
                    $"Embedded lesson content resource is missing: {resourceName}.");

            var document = JsonSerializer.Deserialize<CanonicalLessonContentPackDocument>(
                stream,
                options)
                ?? throw new InvalidOperationException(
                    $"Embedded lesson content resource is invalid: {resourceName}.");

            foreach (var lesson in document.Lessons)
            {
                if (result.TryGetValue(lesson.LessonCode, out var existing))
                {
                    // A later reviewed content version may supersede a pilot.
                    // Core curriculum wins over Supporting when both versions
                    // intentionally reference the same stable lesson identity.
                    result[lesson.LessonCode] = existing && lesson.IsSupporting;
                }
                else
                {
                    result.Add(lesson.LessonCode, lesson.IsSupporting);
                }
            }
        }

        return result;
    }
}
