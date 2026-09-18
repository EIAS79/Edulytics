using System.Collections.Concurrent;
using System.Diagnostics.Metrics;

namespace Edulytics.Services.Mathematics.Runtime;

public enum MathematicsMetricKind
{
    GenerationSuccess = 1,
    SolverSuccess = 2,
    VerificationFailure = 3,
    AlignmentRejection = 4,
    Fallback = 5,
    Unsupported = 6,
    Timeout = 7,
    AnswerEquivalenceDisagreement = 8,
    ContentMappingConflict = 9
}

/// <summary>
/// Low-overhead Mathematics Intelligence observability surface.
/// Metrics are emitted through System.Diagnostics.Metrics and mirrored into
/// process-local monotonic totals so acceptance/runtime diagnostics can verify
/// that instrumentation is active without depending on a vendor SDK.
/// </summary>
public static class MathematicsObservability
{
    public const string MeterName = "Edulytics.Mathematics";

    private static readonly Meter Meter = new(MeterName, "1.0.0");
    private static readonly ConcurrentDictionary<MathematicsMetricKind, long> Totals = new();
    private static readonly IReadOnlyDictionary<MathematicsMetricKind, Counter<long>> Counters =
        Enum.GetValues<MathematicsMetricKind>()
            .ToDictionary(
                kind => kind,
                kind => Meter.CreateCounter<long>(MetricName(kind)));

    public static void Record(MathematicsMetricKind kind, long value = 1)
    {
        if (value <= 0)
            return;

        Totals.AddOrUpdate(kind, value, (_, current) => checked(current + value));
        Counters[kind].Add(value);
    }

    public static long Current(MathematicsMetricKind kind) =>
        Totals.TryGetValue(kind, out var value) ? value : 0L;

    public static IReadOnlyDictionary<string, long> Snapshot() =>
        Enum.GetValues<MathematicsMetricKind>()
            .ToDictionary(
                MetricName,
                kind => Current(kind),
                StringComparer.Ordinal);

    public static string MetricName(MathematicsMetricKind kind) => kind switch
    {
        MathematicsMetricKind.GenerationSuccess => "edulytics.math.generation.success",
        MathematicsMetricKind.SolverSuccess => "edulytics.math.solver.success",
        MathematicsMetricKind.VerificationFailure => "edulytics.math.verification.failure",
        MathematicsMetricKind.AlignmentRejection => "edulytics.math.alignment.rejection",
        MathematicsMetricKind.Fallback => "edulytics.math.fallback",
        MathematicsMetricKind.Unsupported => "edulytics.math.unsupported",
        MathematicsMetricKind.Timeout => "edulytics.math.timeout",
        MathematicsMetricKind.AnswerEquivalenceDisagreement => "edulytics.math.answer_equivalence.disagreement",
        MathematicsMetricKind.ContentMappingConflict => "edulytics.math.content_mapping.conflict",
        _ => throw new ArgumentOutOfRangeException(nameof(kind))
    };
}
