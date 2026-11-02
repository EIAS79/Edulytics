using Edulytics.Services.Mathematics.Runtime;

namespace Edulytics.Services.Mathematics.Rollout;

public enum MathematicsRolloutMode
{
    LegacyOnly = 1,
    ShadowV2 = 2,
    V2VerifiedOnly = 3,
    V2Preferred = 4,
    V2Only = 5
}

public enum MathematicsRolloutRoute
{
    Legacy = 1,
    ShadowV2 = 2,
    V2 = 3,
    Blocked = 4
}

public sealed record MathematicsRolloutScope(
    string Domain,
    string SkillId,
    string Curriculum,
    int Grade);

public sealed record MathematicsRolloutConfiguration(
    MathematicsRolloutMode Mode,
    IReadOnlyList<string> Domains,
    IReadOnlyList<string> Skills,
    IReadOnlyList<string> Curricula,
    IReadOnlyList<int> Grades,
    bool ExplicitMode)
{
    public bool HasSelectors =>
        Domains.Count > 0 || Skills.Count > 0 || Curricula.Count > 0 || Grades.Count > 0;
}

public sealed record MathematicsRolloutDecision(
    MathematicsRolloutRoute Route,
    MathematicsRolloutMode Mode,
    string Reason);

/// <summary>
/// Stage 28 production rollout control plane. New explicit rollout modes must be
/// scoped by domain, skill, curriculum and/or grade. The pre-existing Grade 1-6
/// boolean gate remains compatible and maps to V2VerifiedOnly for the already
/// allow-listed exact production routes.
/// </summary>
public static class MathematicsProductionRolloutPolicy
{
    public const string ModeEnvironmentVariable = "EDULYTICS_MATH_V2_ROLLOUT_MODE";
    public const string DomainsEnvironmentVariable = "EDULYTICS_MATH_V2_ROLLOUT_DOMAINS";
    public const string SkillsEnvironmentVariable = "EDULYTICS_MATH_V2_ROLLOUT_SKILLS";
    public const string CurriculaEnvironmentVariable = "EDULYTICS_MATH_V2_ROLLOUT_CURRICULA";
    public const string GradesEnvironmentVariable = "EDULYTICS_MATH_V2_ROLLOUT_GRADES";

    public static MathematicsRolloutConfiguration FromEnvironment(bool legacyGrade16Enabled) =>
        FromValues(
            Environment.GetEnvironmentVariable(ModeEnvironmentVariable),
            Environment.GetEnvironmentVariable(DomainsEnvironmentVariable),
            Environment.GetEnvironmentVariable(SkillsEnvironmentVariable),
            Environment.GetEnvironmentVariable(CurriculaEnvironmentVariable),
            Environment.GetEnvironmentVariable(GradesEnvironmentVariable),
            legacyGrade16Enabled);

    public static MathematicsRolloutConfiguration FromValues(
        string? mode,
        string? domains,
        string? skills,
        string? curricula,
        string? grades,
        bool legacyGrade16Enabled)
    {
        var explicitMode = !string.IsNullOrWhiteSpace(mode);
        var resolvedMode = explicitMode
            ? ParseMode(mode)
            : legacyGrade16Enabled
                ? MathematicsRolloutMode.V2VerifiedOnly
                : MathematicsRolloutMode.LegacyOnly;

        return new MathematicsRolloutConfiguration(
            resolvedMode,
            ParseStrings(domains),
            ParseStrings(skills),
            ParseStrings(curricula),
            ParseGrades(grades),
            explicitMode);
    }

    public static MathematicsRolloutDecision Evaluate(
        MathematicsRolloutScope scope,
        MathematicsRolloutConfiguration configuration,
        bool isReadyVerified,
        bool hasProductionCapability)
    {
        ArgumentNullException.ThrowIfNull(scope);
        ArgumentNullException.ThrowIfNull(configuration);

        if (scope.Grade < 1 ||
            string.IsNullOrWhiteSpace(scope.Domain) ||
            string.IsNullOrWhiteSpace(scope.SkillId) ||
            string.IsNullOrWhiteSpace(scope.Curriculum))
        {
            MathematicsObservability.Record(MathematicsMetricKind.AlignmentRejection);
            return new MathematicsRolloutDecision(
                MathematicsRolloutRoute.Blocked,
                configuration.Mode,
                "Rollout scope is incomplete.");
        }

        if (configuration.ExplicitMode &&
            configuration.Mode != MathematicsRolloutMode.LegacyOnly &&
            !configuration.HasSelectors)
        {
            MathematicsObservability.Record(MathematicsMetricKind.AlignmentRejection);
            return new MathematicsRolloutDecision(
                MathematicsRolloutRoute.Blocked,
                configuration.Mode,
                "Explicit non-legacy rollout modes require at least one scope selector.");
        }

        if (!Matches(scope, configuration))
        {
            MathematicsObservability.Record(MathematicsMetricKind.Fallback);
            return new MathematicsRolloutDecision(
                MathematicsRolloutRoute.Legacy,
                configuration.Mode,
                "Scope is outside the selected rollout boundary.");
        }

        return configuration.Mode switch
        {
            MathematicsRolloutMode.LegacyOnly =>
                Legacy(configuration.Mode, "LegacyOnly selected."),

            MathematicsRolloutMode.ShadowV2 =>
                new MathematicsRolloutDecision(
                    MathematicsRolloutRoute.ShadowV2,
                    configuration.Mode,
                    "V2 may execute for observation but cannot own learner-facing output."),

            MathematicsRolloutMode.V2VerifiedOnly =>
                isReadyVerified && hasProductionCapability
                    ? V2(configuration.Mode, "READY_VERIFIED production capability is approved.")
                    : Legacy(configuration.Mode, "Target is not READY_VERIFIED with production capability."),

            MathematicsRolloutMode.V2Preferred =>
                isReadyVerified && hasProductionCapability
                    ? V2(configuration.Mode, "Verified V2 is preferred inside the selected scope.")
                    : Legacy(configuration.Mode, "Verified V2 is unavailable; preserve legacy route."),

            MathematicsRolloutMode.V2Only =>
                isReadyVerified && hasProductionCapability
                    ? V2(configuration.Mode, "V2Only target is explicitly verified and capable.")
                    : Blocked(configuration.Mode, "V2Only forbids silent legacy fallback for unsupported targets."),

            _ => Legacy(MathematicsRolloutMode.LegacyOnly, "Unknown rollout mode failed closed.")
        };
    }

    private static MathematicsRolloutDecision Legacy(MathematicsRolloutMode mode, string reason)
    {
        MathematicsObservability.Record(MathematicsMetricKind.Fallback);
        return new MathematicsRolloutDecision(MathematicsRolloutRoute.Legacy, mode, reason);
    }

    private static MathematicsRolloutDecision V2(MathematicsRolloutMode mode, string reason) =>
        new(MathematicsRolloutRoute.V2, mode, reason);

    private static MathematicsRolloutDecision Blocked(MathematicsRolloutMode mode, string reason)
    {
        MathematicsObservability.Record(MathematicsMetricKind.Unsupported);
        return new MathematicsRolloutDecision(MathematicsRolloutRoute.Blocked, mode, reason);
    }

    private static bool Matches(
        MathematicsRolloutScope scope,
        MathematicsRolloutConfiguration configuration) =>
        MatchesText(configuration.Domains, scope.Domain) &&
        MatchesText(configuration.Skills, scope.SkillId) &&
        MatchesText(configuration.Curricula, scope.Curriculum) &&
        (configuration.Grades.Count == 0 || configuration.Grades.Contains(scope.Grade));

    private static bool MatchesText(IReadOnlyList<string> values, string candidate) =>
        values.Count == 0 || values.Contains(candidate, StringComparer.OrdinalIgnoreCase);

    private static MathematicsRolloutMode ParseMode(string? value) =>
        Enum.TryParse<MathematicsRolloutMode>(value?.Trim(), true, out var mode)
            ? mode
            : MathematicsRolloutMode.LegacyOnly;

    private static IReadOnlyList<string> ParseStrings(string? value) =>
        string.IsNullOrWhiteSpace(value)
            ? []
            : value.Split([',', ';'], StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToArray();

    private static IReadOnlyList<int> ParseGrades(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return [];

        return value.Split([',', ';'], StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(x => int.TryParse(x, out var grade) && grade is >= 1 and <= 20 ? grade : 0)
            .Where(x => x > 0)
            .Distinct()
            .Order()
            .ToArray();
    }
}
