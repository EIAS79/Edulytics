using Edulytics.Core.AdaptivePractice;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Edulytics.Web.Health;

public sealed class AdaptivePracticeV2ReadinessHealthCheck(
    AdaptivePracticeV2Policy policy)
    : IHealthCheck
{
    public Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        if (!policy.Enabled ||
            policy.Mode == AdaptivePracticeV2Mode.Off)
        {
            return Task.FromResult(
                HealthCheckResult.Healthy(
                    "Adaptive Practice V2 is disabled."));
        }

        if (policy.Mode == AdaptivePracticeV2Mode.Canary)
        {
            var validScope =
                policy.RouteAllReadyVerifiedCatalogue
                    ? true
                    : policy.RouteAllReadyVerifiedLessons
                        ? AdaptivePrimaryRolloutPlan.IsValidReadyVerifiedScope(
                            policy.AllowedCurriculumLevelKeys)
                        : AdaptivePrimaryRolloutPlan.IsValidCanaryScope(
                            policy.AllowedCurriculumLevelKeys,
                            policy.AllowedLessonCodes);

            if (policy.AllowedSchoolIds.Count == 0 ||
                !validScope)
            {
                return Task.FromResult(
                    HealthCheckResult.Unhealthy(
                        "Adaptive V2 Canary scope is not fail-closed."));
            }
        }

        if (policy.MaxLessonQuestions is < 1 or > 30 ||
            policy.MinimumPsychometricResponses is < 10 or > 10000 ||
            policy.MinimumPsychometricStudents is < 5 or > 1000 ||
            policy.MinimumResearchCohortSize is < 10 or > 10000 ||
            policy.MaximumIntelligenceRows is < 50 or > 2000)
        {
            return Task.FromResult(
                HealthCheckResult.Unhealthy(
                    "Adaptive V2 resource or evidence limits are invalid."));
        }

        if (policy.EnableLiveGroupSession &&
            !policy.EnableLiveClassroom)
        {
            return Task.FromResult(
                HealthCheckResult.Unhealthy(
                    "Live Group Session requires Live Classroom Intelligence."));
        }

        return Task.FromResult(
            HealthCheckResult.Healthy(
                "Adaptive Practice V2 rollout policy is internally consistent."));
    }
}
