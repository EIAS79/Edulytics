using Edulytics.Core.Analytics;
using Edulytics.Core.Enums;
using Edulytics.Core.LearningIntelligence;

namespace Edulytics.Services.LearningIntelligence;

public sealed class LearningIntelligenceEngine
{
    public const string FormulaVersion = "phase37-v1";
    private const int MaxSnapshots = 50_000;
    private const int MaxRecoveryObservations = 100_000;

    public LearningIntelligenceDashboard Build(LearningIntelligenceRequest request)
    {
        Validate(request);

        var snapshots = request.Snapshots
            .OrderBy(x => x.CapturedAtUtc)
            .ThenBy(x => x.StudentProfileId)
            .ToArray();
        var latestSnapshots = snapshots
            .GroupBy(x => x.StudentProfileId)
            .Select(x => x.OrderByDescending(y => y.CapturedAtUtc).First())
            .OrderBy(x => x.StudentDisplayName, StringComparer.Ordinal)
            .ThenBy(x => x.StudentProfileId)
            .ToArray();

        var studentTrends = BuildStudentTrends(snapshots);
        var classTrends = BuildClassTrends(snapshots);
        var outcomeWeakness = BuildOutcomeWeakness(latestSnapshots);
        var weaknessConcentration = BuildWeaknessConcentration(latestSnapshots);
        var recoveryEffectiveness = BuildRecoveryEffectiveness(request.RecoveryObservations);
        var drilldown = BuildDrilldown(latestSnapshots);

        var schoolMastery = latestSnapshots.Length == 0
            ? 0m
            : Round2(latestSnapshots.Average(x => x.Profile.OverallMasteryPercentage));
        var eligibleForImprovement = studentTrends
            .Where(x => x.Trend.Count >= 2)
            .ToArray();
        var improvementRate = eligibleForImprovement.Length == 0
            ? 0m
            : Round2(eligibleForImprovement.Count(x => x.ChangePercentagePoints > 0m) * 100m /
                     eligibleForImprovement.Length);

        return new LearningIntelligenceDashboard(
            request.SchoolId,
            schoolMastery,
            improvementRate,
            latestSnapshots.Length,
            latestSnapshots.Sum(x => x.Profile.EvidenceCount),
            studentTrends,
            classTrends,
            outcomeWeakness,
            weaknessConcentration,
            recoveryEffectiveness,
            drilldown,
            FormulaVersion);
    }

    private static IReadOnlyList<StudentTrendRow> BuildStudentTrends(
        IReadOnlyCollection<LearningIntelligenceStudentSnapshot> snapshots)
    {
        return snapshots
            .GroupBy(x => x.StudentProfileId)
            .Select(group =>
            {
                var ordered = group
                    .OrderBy(x => x.CapturedAtUtc)
                    .ToArray();
                var first = ordered[0];
                var latest = ordered[^1];
                var trend = ordered
                    .Select(x => new MasteryTrendPoint(
                        x.CapturedAtUtc,
                        Round2(x.Profile.OverallMasteryPercentage),
                        1))
                    .ToArray();

                return new StudentTrendRow(
                    group.Key,
                    latest.StudentDisplayName,
                    Round2(first.Profile.OverallMasteryPercentage),
                    Round2(latest.Profile.OverallMasteryPercentage),
                    Round2(latest.Profile.OverallMasteryPercentage - first.Profile.OverallMasteryPercentage),
                    latest.Profile.OverallBand,
                    trend);
            })
            .OrderBy(x => x.StudentDisplayName, StringComparer.Ordinal)
            .ThenBy(x => x.StudentProfileId)
            .ToArray();
    }

    private static IReadOnlyList<ClassTrendRow> BuildClassTrends(
        IReadOnlyCollection<LearningIntelligenceStudentSnapshot> snapshots)
    {
        return snapshots
            .GroupBy(x => new { x.ClassGroupId, x.ClassName })
            .Select(group =>
            {
                var trend = group
                    .GroupBy(x => x.CapturedAtUtc.Date)
                    .OrderBy(x => x.Key)
                    .Select(day => new MasteryTrendPoint(
                        DateTime.SpecifyKind(day.Key, DateTimeKind.Utc),
                        Round2(day.Average(x => x.Profile.OverallMasteryPercentage)),
                        day.Select(x => x.StudentProfileId).Distinct().Count()))
                    .ToArray();
                var first = trend[0];
                var latest = trend[^1];
                var latestStudentCount = group
                    .GroupBy(x => x.StudentProfileId)
                    .Select(x => x.OrderByDescending(y => y.CapturedAtUtc).First())
                    .Count();

                return new ClassTrendRow(
                    group.Key.ClassGroupId,
                    group.Key.ClassName,
                    first.MasteryPercentage,
                    latest.MasteryPercentage,
                    Round2(latest.MasteryPercentage - first.MasteryPercentage),
                    latestStudentCount,
                    trend);
            })
            .OrderBy(x => x.ClassName, StringComparer.Ordinal)
            .ThenBy(x => x.ClassGroupId)
            .ToArray();
    }

    private static IReadOnlyList<OutcomeWeaknessRow> BuildOutcomeWeakness(
        IReadOnlyCollection<LearningIntelligenceStudentSnapshot> latestSnapshots)
    {
        return latestSnapshots
            .SelectMany(snapshot => snapshot.Profile.Outcomes.Select(outcome => new
            {
                snapshot.StudentProfileId,
                Outcome = outcome
            }))
            .GroupBy(x => new
            {
                x.Outcome.LearningOutcomeId,
                x.Outcome.OutcomeCode,
                x.Outcome.OutcomeDescription
            })
            .Select(group =>
            {
                var measured = group.Count();
                var weak = group.Count(x => IsWeak(x.Outcome.Band));
                return new OutcomeWeaknessRow(
                    group.Key.LearningOutcomeId,
                    group.Key.OutcomeCode,
                    group.Key.OutcomeDescription,
                    measured,
                    weak,
                    measured == 0 ? 0m : Round2(weak * 100m / measured),
                    measured == 0 ? 0m : Round2(group.Average(x => x.Outcome.MasteryPercentage)));
            })
            .OrderByDescending(x => x.WeakStudentPercentage)
            .ThenBy(x => x.OutcomeCode, StringComparer.Ordinal)
            .ThenBy(x => x.LearningOutcomeId)
            .ToArray();
    }

    private static IReadOnlyList<WeaknessConcentrationRow> BuildWeaknessConcentration(
        IReadOnlyCollection<LearningIntelligenceStudentSnapshot> latestSnapshots)
    {
        return latestSnapshots
            .GroupBy(x => new
            {
                x.CurriculumLevelKey,
                x.CurriculumLevelLabel,
                x.ClassGroupId,
                x.ClassName
            })
            .Select(group =>
            {
                var outcomeInstances = group.Sum(x => x.Profile.Outcomes.Count);
                var weakInstances = group.Sum(x => x.Profile.Outcomes.Count(y => IsWeak(y.Band)));
                return new WeaknessConcentrationRow(
                    group.Key.CurriculumLevelKey,
                    group.Key.CurriculumLevelLabel,
                    group.Key.ClassGroupId,
                    group.Key.ClassName,
                    group.Select(x => x.StudentProfileId).Distinct().Count(),
                    weakInstances,
                    outcomeInstances == 0 ? 0m : Round2(weakInstances * 100m / outcomeInstances));
            })
            .OrderByDescending(x => x.WeakOutcomePercentage)
            .ThenBy(x => x.CurriculumLevelKey, StringComparer.Ordinal)
            .ThenBy(x => x.ClassName, StringComparer.Ordinal)
            .ThenBy(x => x.ClassGroupId)
            .ToArray();
    }

    private static IReadOnlyList<RecoveryEffectivenessRow> BuildRecoveryEffectiveness(
        IReadOnlyCollection<RecoveryIntelligenceObservation> observations)
    {
        return observations
            .GroupBy(x => x.LearningOutcomeId)
            .Select(group =>
            {
                var count = group.Count();
                var improved = group.Count(x => x.AfterMastery > x.BeforeMastery);
                var recovered = group.Count(x => x.RecoveredToSecureOrStrong);
                return new RecoveryEffectivenessRow(
                    group.Key,
                    count,
                    improved,
                    recovered,
                    count == 0 ? 0m : Round2(group.Average(x => x.AfterMastery - x.BeforeMastery)),
                    count == 0 ? 0m : Round2(recovered * 100m / count));
            })
            .OrderByDescending(x => x.RecoveryRatePercentage)
            .ThenBy(x => x.LearningOutcomeId)
            .ToArray();
    }

    private static IReadOnlyList<LearningIntelligenceDrilldownRow> BuildDrilldown(
        IReadOnlyCollection<LearningIntelligenceStudentSnapshot> latestSnapshots)
    {
        return latestSnapshots
            .SelectMany(snapshot => snapshot.Profile.Outcomes.Select(outcome =>
                new LearningIntelligenceDrilldownRow(
                    snapshot.CurriculumLevelKey,
                    snapshot.CurriculumLevelLabel,
                    snapshot.ClassGroupId,
                    snapshot.ClassName,
                    snapshot.TeacherUserId,
                    snapshot.TeacherDisplayName,
                    snapshot.StudentProfileId,
                    snapshot.StudentDisplayName,
                    outcome.LearningOutcomeId,
                    outcome.OutcomeCode,
                    Round2(outcome.MasteryPercentage),
                    outcome.Band,
                    Round2(outcome.ConfidencePercentage),
                    outcome.EvidenceCount,
                    outcome.LatestEvidenceAtUtc)))
            .OrderBy(x => x.CurriculumLevelKey, StringComparer.Ordinal)
            .ThenBy(x => x.ClassName, StringComparer.Ordinal)
            .ThenBy(x => x.TeacherDisplayName ?? string.Empty, StringComparer.Ordinal)
            .ThenBy(x => x.StudentDisplayName, StringComparer.Ordinal)
            .ThenBy(x => x.OutcomeCode, StringComparer.Ordinal)
            .ThenBy(x => x.LearningOutcomeId)
            .ToArray();
    }

    private static void Validate(LearningIntelligenceRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(request.Snapshots);
        ArgumentNullException.ThrowIfNull(request.RecoveryObservations);

        if (request.SchoolId == Guid.Empty)
            throw new InvalidOperationException("Learning intelligence requires an explicit school scope.");
        if (request.Snapshots.Count > MaxSnapshots ||
            request.RecoveryObservations.Count > MaxRecoveryObservations)
        {
            throw new InvalidOperationException("Learning intelligence input exceeds the bounded synchronous aggregation limit.");
        }

        var keys = new HashSet<(Guid StudentProfileId, DateTime CapturedAtUtc)>();
        foreach (var snapshot in request.Snapshots)
        {
            ArgumentNullException.ThrowIfNull(snapshot.Profile);
            if (snapshot.SchoolId != request.SchoolId ||
                snapshot.Profile.SchoolId != request.SchoolId ||
                snapshot.Profile.StudentProfileId != snapshot.StudentProfileId ||
                snapshot.Profile.ClassGroupId != snapshot.ClassGroupId ||
                snapshot.StudentProfileId == Guid.Empty ||
                snapshot.ClassGroupId == Guid.Empty ||
                string.IsNullOrWhiteSpace(snapshot.StudentDisplayName) ||
                string.IsNullOrWhiteSpace(snapshot.CurriculumLevelKey) ||
                string.IsNullOrWhiteSpace(snapshot.CurriculumLevelLabel) ||
                string.IsNullOrWhiteSpace(snapshot.ClassName) ||
                snapshot.CapturedAtUtc.Kind != DateTimeKind.Utc ||
                !keys.Add((snapshot.StudentProfileId, snapshot.CapturedAtUtc)))
            {
                throw new InvalidOperationException("Learning intelligence snapshot is invalid, ambiguous or outside the requested school scope.");
            }

            ValidatePercentage(snapshot.Profile.OverallMasteryPercentage);
            ValidatePercentage(snapshot.Profile.ConfidencePercentage);
            foreach (var outcome in snapshot.Profile.Outcomes)
            {
                ValidatePercentage(outcome.MasteryPercentage);
                ValidatePercentage(outcome.ConfidencePercentage);
            }
        }

        foreach (var recovery in request.RecoveryObservations)
        {
            if (recovery.SchoolId != request.SchoolId ||
                recovery.StudentProfileId == Guid.Empty ||
                recovery.LearningOutcomeId == Guid.Empty ||
                recovery.OccurredAtUtc.Kind != DateTimeKind.Utc)
            {
                throw new InvalidOperationException("Recovery intelligence observation is invalid or outside the requested school scope.");
            }
            ValidatePercentage(recovery.BeforeMastery);
            ValidatePercentage(recovery.AfterMastery);
        }
    }

    private static void ValidatePercentage(decimal value)
    {
        if (value is < 0m or > 100m)
            throw new InvalidOperationException("Learning intelligence percentages must be within 0..100.");
    }

    private static bool IsWeak(MasteryBand band) =>
        band is MasteryBand.CriticalGap or MasteryBand.Weak;

    private static decimal Round2(decimal value) =>
        Math.Round(value, 2, MidpointRounding.AwayFromZero);
}
