using Edulytics.Core.AdaptivePractice;
using Edulytics.Web.Health;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Edulytics.Tests.MathematicsIntelligence.AdaptivePractice;

public sealed class AdaptivePracticeV2ReadinessHealthCheckTests
{
    [Fact]
    public async Task FullCatalogueCanary_WithSchoolAllowList_IsReady()
    {
        var policy =
            new AdaptivePracticeV2Policy(
                Enabled: true,
                Mode: AdaptivePracticeV2Mode.Canary,
                AllowedCurriculumLevelKeys:
                    new HashSet<string>(
                        StringComparer.OrdinalIgnoreCase),
                AllowedLessonCodes:
                    new HashSet<string>(
                        StringComparer.Ordinal),
                AllowedSchoolIds:
                    new HashSet<Guid>
                    {
                        Guid.Parse(
                            "15151515-1515-1515-1515-151515151515")
                    },
                ShadowSamplingPercentage: 100,
                MaxLessonQuestions: 8,
                EnableMisconceptionLoop: true,
                EnableDirectNextSteps: false,
                EnableQuestionLog: false,
                EnableLiveClassroom: false,
                EnableDiagnosticV2: false)
            {
                RouteAllReadyVerifiedCatalogue = true
            };

        var result =
            await new AdaptivePracticeV2ReadinessHealthCheck(
                    policy)
                .CheckHealthAsync(
                    new HealthCheckContext());

        Assert.Equal(
            HealthStatus.Healthy,
            result.Status);
    }

    [Fact]
    public async Task LegacyCanary_StillRequiresReviewedPrimaryScope()
    {
        var policy =
            new AdaptivePracticeV2Policy(
                Enabled: true,
                Mode: AdaptivePracticeV2Mode.Canary,
                AllowedCurriculumLevelKeys:
                    new HashSet<string>(
                        StringComparer.OrdinalIgnoreCase),
                AllowedLessonCodes:
                    new HashSet<string>(
                        StringComparer.Ordinal),
                AllowedSchoolIds:
                    new HashSet<Guid>
                    {
                        Guid.Parse(
                            "16161616-1616-1616-1616-161616161616")
                    },
                ShadowSamplingPercentage: 100,
                MaxLessonQuestions: 8,
                EnableMisconceptionLoop: true,
                EnableDirectNextSteps: false,
                EnableQuestionLog: false,
                EnableLiveClassroom: false,
                EnableDiagnosticV2: false);

        var result =
            await new AdaptivePracticeV2ReadinessHealthCheck(
                    policy)
                .CheckHealthAsync(
                    new HealthCheckContext());

        Assert.Equal(
            HealthStatus.Unhealthy,
            result.Status);
    }
}
