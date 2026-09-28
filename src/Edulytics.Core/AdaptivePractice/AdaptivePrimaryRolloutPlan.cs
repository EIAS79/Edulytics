using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;

namespace Edulytics.Core.AdaptivePractice;

public sealed record AdaptivePrimaryRolloutEntry(
    string Wave,
    string CurriculumLevelKey,
    string LessonCode);

/// <summary>
/// Audited learner-facing rollout pairs for Adaptive Practice V2.
///
/// Canary configuration must stay inside this registry. The runtime Practice
/// repository independently constrains lessons to the active adoption's
/// framework/version/logical-level/pathway, while this registry prevents a
/// configuration-only expansion beyond reviewed rollout pairs.
///
/// Advanced/secondary levels are intentionally absent.
/// </summary>
public static class AdaptivePrimaryRolloutPlan
{
    public const string InternalCanaryWave = "INTERNAL_CANARY";
    public const string Phase9Stage2Wave = "PHASE9_STAGE2";
    public const string Phase9Stage3Wave = "PHASE9_STAGE3";
    public const string Phase9Stage4Wave = "PHASE9_STAGE4";
    public const string Phase9Stage5Wave = "PHASE9_STAGE5";
    public const string Phase9Stage6Wave = "PHASE9_STAGE6";

    public static IReadOnlyList<AdaptivePrimaryRolloutEntry> All { get; } =
    [
        // Existing production Canary preserved exactly.
        new(
            InternalCanaryWave,
            "US-CCSS-MATH:L05:SHARED",
            "PED:US-CCSS-MATH:G4:U02:L07"),

        // Phase 9 — controlled Primary Stage 2-6 expansion.
        new(
            Phase9Stage2Wave,
            "CAMBRIDGE-INTL-MATH:L02:SHARED",
            "PED:CAMBRIDGE-INTL-MATH:S2:2AS-1:APPLY"),
        new(
            Phase9Stage3Wave,
            "CAMBRIDGE-INTL-MATH:L03:SHARED",
            "PED:CAMBRIDGE-INTL-MATH:S3:3AS-2:APPLY"),
        new(
            Phase9Stage4Wave,
            "CAMBRIDGE-INTL-MATH:L04:SHARED",
            "PED:CAMBRIDGE-INTL-MATH:S4:4NF-1:APPLY"),
        new(
            Phase9Stage5Wave,
            "CAMBRIDGE-INTL-MATH:L05:SHARED",
            "PED:CAMBRIDGE-INTL-MATH:S5:5F-2:APPLY"),
        new(
            Phase9Stage6Wave,
            "CAMBRIDGE-INTL-MATH:L06:SHARED",
            "PED:CAMBRIDGE-INTL-MATH:S6:6F-3:APPLY")
    ];

    public static bool IsValidCanaryScope(
        IEnumerable<string>? curriculumLevelKeys,
        IEnumerable<string>? lessonCodes)
    {
        var levels = (curriculumLevelKeys ?? [])
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Select(x => x.Trim())
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        var lessons = (lessonCodes ?? [])
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Select(x => x.Trim())
            .ToHashSet(StringComparer.Ordinal);

        if (levels.Count == 0 || lessons.Count == 0)
            return false;

        var approvedLevels = All
            .Select(x => x.CurriculumLevelKey)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);
        var approvedLessons = All
            .Select(x => x.LessonCode)
            .ToHashSet(StringComparer.Ordinal);

        if (!levels.IsSubsetOf(approvedLevels) ||
            !lessons.IsSubsetOf(approvedLessons))
        {
            return false;
        }

        // Every configured level must have at least one configured lesson from
        // the same reviewed rollout pair, and every configured lesson must have
        // its reviewed level configured. This prevents accidental partial waves.
        if (levels.Any(level =>
                !All.Any(entry =>
                    string.Equals(
                        entry.CurriculumLevelKey,
                        level,
                        StringComparison.OrdinalIgnoreCase) &&
                    lessons.Contains(entry.LessonCode))))
        {
            return false;
        }

        if (lessons.Any(lesson =>
                !All.Any(entry =>
                    string.Equals(
                        entry.LessonCode,
                        lesson,
                        StringComparison.Ordinal) &&
                    levels.Contains(entry.CurriculumLevelKey))))
        {
            return false;
        }

        return true;
    }

    public static void Validate()
    {
        if (All.Count != All
                .Select(x => (x.CurriculumLevelKey, x.LessonCode))
                .Distinct()
                .Count())
        {
            throw new InvalidOperationException(
                "Adaptive Primary rollout plan contains duplicate level/lesson pairs.");
        }

        foreach (var entry in All)
        {
            var level = CurriculumLevelIdentityRegistry.Find(
                entry.CurriculumLevelKey);

            if (level is null ||
                level.LogicalLevel is < 1 or > 6)
            {
                throw new InvalidOperationException(
                    $"Adaptive Primary rollout level is invalid: {entry.CurriculumLevelKey}.");
            }

            if (!LessonPracticeCapabilityResolver.TryResolve(
                    entry.LessonCode,
                    out var contract) ||
                contract is null)
            {
                throw new InvalidOperationException(
                    $"Adaptive Primary rollout lesson is not READY_VERIFIED: {entry.LessonCode}.");
            }
        }

        var phase9 = All
            .Where(x => x.Wave.StartsWith(
                "PHASE9_STAGE",
                StringComparison.Ordinal))
            .Select(x =>
                CurriculumLevelIdentityRegistry
                    .Find(x.CurriculumLevelKey)!
                    .LogicalLevel)
            .OrderBy(x => x)
            .ToArray();

        if (!phase9.SequenceEqual([2, 3, 4, 5, 6]))
        {
            throw new InvalidOperationException(
                "Adaptive V2 Phase 9 rollout must cover logical Primary levels 2 through 6 exactly once.");
        }
    }
}
