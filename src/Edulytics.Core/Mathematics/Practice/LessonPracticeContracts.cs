using System.Reflection;
using System.Text.Json;

namespace Edulytics.Core.Mathematics.Practice;

/// <summary>
/// General lesson-scoped Practice contract used by the runtime capability layer.
/// Official curriculum mappings remain separate; a Supporting lesson may be
/// READY_VERIFIED for Practice without an official OutcomeCode.
/// </summary>
public sealed record LessonPracticeContract(
    string LessonCode,
    string SkillId,
    string Mechanic,
    IReadOnlyList<string> AllowedQuestionFamilies,
    string SourceType,
    string Readiness,
    string ContractVersion)
{
    public Stage18PracticeSkillContract ToLegacyStage18Contract() =>
        new(LessonCode, SkillId, Mechanic, AllowedQuestionFamilies);
}

/// <summary>
/// Data-driven runtime projection of approved lesson-skill mappings into exact
/// Practice contracts. The mapping JSON is the academic authority; this loader
/// only exposes rows that explicitly declare READY_VERIFIED Practice metadata.
/// </summary>
public static class LessonPracticeContractRegistry
{
    public const string Version = "supporting-lesson-practice-v5";

    private static readonly Lazy<IReadOnlyList<LessonPracticeContract>> LazyEntries =
        new(Load, LazyThreadSafetyMode.ExecutionAndPublication);

    private static readonly Lazy<IReadOnlyDictionary<string, LessonPracticeContract>> LazyByLessonCode =
        new(() => LazyEntries.Value.ToDictionary(x => x.LessonCode, StringComparer.Ordinal),
            LazyThreadSafetyMode.ExecutionAndPublication);

    public static IReadOnlyList<LessonPracticeContract> All => LazyEntries.Value;

    public static bool TryResolve(string? lessonCode, out LessonPracticeContract? contract)
    {
        if (string.IsNullOrWhiteSpace(lessonCode))
        {
            contract = null;
            return false;
        }

        return LazyByLessonCode.Value.TryGetValue(lessonCode.Trim(), out contract);
    }

    private static IReadOnlyList<LessonPracticeContract> Load()
    {
        var assembly = typeof(LessonPracticeContractRegistry).Assembly;
        var resourceName = assembly.GetManifestResourceNames()
            .Single(name => name.EndsWith(
                "Mathematics.Curriculum.lesson-skill-mappings.v1.json",
                StringComparison.Ordinal));

        using var stream = assembly.GetManifestResourceStream(resourceName)
            ?? throw new InvalidOperationException("Lesson-skill mapping resource is unavailable.");
        using var document = JsonDocument.Parse(stream);

        var entries = new List<LessonPracticeContract>();
        var seen = new HashSet<string>(StringComparer.Ordinal);

        foreach (var row in document.RootElement.GetProperty("mappings").EnumerateArray())
        {
            if (!row.TryGetProperty("practiceReadiness", out var readinessElement) ||
                !string.Equals(readinessElement.GetString(), "READY_VERIFIED", StringComparison.Ordinal) ||
                !row.TryGetProperty("practiceMechanic", out var mechanicElement) ||
                !row.TryGetProperty("allowedQuestionFamilies", out var familiesElement))
            {
                continue;
            }

            var lessonCode = row.GetProperty("lessonCode").GetString()?.Trim();
            var primarySkills = row.GetProperty("primarySkills")
                .EnumerateArray()
                .Select(x => x.GetString()?.Trim())
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Cast<string>()
                .ToArray();
            var families = familiesElement
                .EnumerateArray()
                .Select(x => x.GetString()?.Trim())
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Cast<string>()
                .Distinct(StringComparer.Ordinal)
                .ToArray();

            if (string.IsNullOrWhiteSpace(lessonCode) ||
                primarySkills.Length != 1 ||
                families.Length == 0 ||
                !seen.Add(lessonCode))
            {
                throw new InvalidOperationException(
                    $"Invalid or duplicate READY_VERIFIED lesson Practice mapping: {lessonCode ?? "<missing>"}.");
            }

            var sourceType = row.TryGetProperty("sourceType", out var sourceElement)
                ? sourceElement.GetString() ?? "SupportingLesson"
                : "SupportingLesson";
            var contractVersion = row.TryGetProperty("practiceContractVersion", out var versionElement)
                ? versionElement.GetString() ?? Version
                : Version;

            entries.Add(new LessonPracticeContract(
                lessonCode,
                primarySkills[0],
                mechanicElement.GetString() ?? throw new InvalidOperationException(
                    $"Practice mechanic missing for {lessonCode}."),
                families,
                sourceType,
                "READY_VERIFIED",
                contractVersion));
        }

        return entries
            .OrderBy(x => x.LessonCode, StringComparer.Ordinal)
            .ToArray();
    }
}
