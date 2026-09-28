using Edulytics.Core.AdaptivePractice;
using Edulytics.Web.Health;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class AdaptiveProgrammePhase14To17Tests
{
    [Fact]
    public async Task Phase17_OffPolicy_IsHealthyBecauseFeatureIsInactive()
    {
        var check = new AdaptivePracticeV2ReadinessHealthCheck(
            AdaptivePracticeV2Policy.Off);

        var result = await check.CheckHealthAsync(
            new HealthCheckContext());

        Assert.Equal(HealthStatus.Healthy, result.Status);
    }

    [Fact]
    public async Task Phase17_LiveGroupRequiresLiveClassroom()
    {
        var policy = Policy() with
        {
            EnableLiveGroupSession = true,
            EnableLiveClassroom = false
        };

        var result =
            await new AdaptivePracticeV2ReadinessHealthCheck(policy)
                .CheckHealthAsync(new HealthCheckContext());

        Assert.Equal(HealthStatus.Unhealthy, result.Status);
        Assert.Contains(
            "requires Live Classroom",
            result.Description,
            StringComparison.Ordinal);
    }

    [Fact]
    public async Task Phase17_ValidClosurePolicy_IsReady()
    {
        var policy = Policy() with
        {
            EnableLiveClassroom = true,
            EnableLiveGroupSession = true,
            EnablePsychometricReadiness = true,
            EnableResearchProgramme = true,
            MinimumPsychometricResponses = 30,
            MinimumPsychometricStudents = 10,
            MinimumResearchCohortSize = 10,
            MaximumIntelligenceRows = 500
        };

        var result =
            await new AdaptivePracticeV2ReadinessHealthCheck(policy)
                .CheckHealthAsync(new HealthCheckContext());

        Assert.Equal(HealthStatus.Healthy, result.Status);
    }

    [Fact]
    public void Phase14_LiveGroupPlan_IsBoundedAndComposesLiveClassroom()
    {
        var root = FindRoot();
        var text = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Services/AdaptivePractice/AdaptiveProgrammeClosureService.cs"));

        Assert.Contains("MaximumLiveGroupSize = 6", text, StringComparison.Ordinal);
        Assert.Contains("GetLiveClassroomAsync", text, StringComparison.Ordinal);
        Assert.Contains("Remediation", text, StringComparison.Ordinal);
        Assert.Contains("Confirmation", text, StringComparison.Ordinal);
        Assert.Contains("Extension", text, StringComparison.Ordinal);
    }

    [Fact]
    public void Phase15_Psychometrics_NeverClaimsCalibrationWithoutSampleGate()
    {
        var root = FindRoot();
        var text = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Services/AdaptivePractice/AdaptiveProgrammeClosureService.cs"));

        Assert.Contains("MinimumPsychometricResponses", text, StringComparison.Ordinal);
        Assert.Contains("MinimumPsychometricStudents", text, StringComparison.Ordinal);
        Assert.Contains("InsufficientEvidence", text, StringComparison.Ordinal);
        Assert.Contains("CalibrationReady", text, StringComparison.Ordinal);
        Assert.Contains("Discrimination(", text, StringComparison.Ordinal);
    }

    [Fact]
    public void Phase16_Research_IsAggregateOnlyAndHasPrivacyThresholds()
    {
        var root = FindRoot();
        var contracts = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Services/AdaptivePractice/AdaptiveProgrammeClosureContracts.cs"));
        var service = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Services/AdaptivePractice/AdaptiveProgrammeClosureService.cs"));

        Assert.Contains("MinimumResearchCohortSize", service, StringComparison.Ordinal);
        Assert.Contains("MinimumResearchFamilyResponses = 5", service, StringComparison.Ordinal);
        Assert.Contains("no learner identifiers", service, StringComparison.OrdinalIgnoreCase);

        var aggregateStart = contracts.IndexOf(
            "public sealed record AdaptiveResearchAggregate",
            StringComparison.Ordinal);
        Assert.True(aggregateStart >= 0);
        var aggregate = contracts[aggregateStart..];
        Assert.DoesNotContain("StudentProfileId", aggregate, StringComparison.Ordinal);
        Assert.DoesNotContain("DisplayName", aggregate, StringComparison.Ordinal);
        Assert.DoesNotContain("StudentNumber", aggregate, StringComparison.Ordinal);
    }

    [Fact]
    public void Phase17_EndpointsUseAnalyticsTimeoutAndConcurrencyLimits()
    {
        var root = FindRoot();
        var controller = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/Controllers/AdaptiveIntelligenceV2Controller.cs"));
        var registration = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/Extensions/AdaptivePracticeV2RegistrationExtensions.cs"));

        Assert.Contains(
            "RequestTimeout(BackendResiliencePolicyNames.Analytics)",
            controller,
            StringComparison.Ordinal);
        Assert.Contains(
            "EnableRateLimiting(BackendResiliencePolicyNames.AnalyticsConcurrency)",
            controller,
            StringComparison.Ordinal);
        Assert.Contains(
            "AdaptivePracticeV2ReadinessHealthCheck",
            registration,
            StringComparison.Ordinal);
        Assert.Contains(
            "\"adaptive-v2\"",
            registration,
            StringComparison.Ordinal);
        Assert.Contains(
            "tags: [\"ready\"]",
            registration,
            StringComparison.Ordinal);
    }

    private static AdaptivePracticeV2Policy Policy() =>
        new(
            Enabled: true,
            Mode: AdaptivePracticeV2Mode.Canary,
            AllowedCurriculumLevelKeys:
                new HashSet<string>(
                    ["US-CCSS-MATH:L05:SHARED"],
                    StringComparer.OrdinalIgnoreCase),
            AllowedLessonCodes:
                new HashSet<string>(
                    ["PED:US-CCSS-MATH:G4:U02:L07"],
                    StringComparer.Ordinal),
            AllowedSchoolIds:
                new HashSet<Guid> { Guid.NewGuid() },
            ShadowSamplingPercentage: 100,
            MaxLessonQuestions: 8,
            EnableMisconceptionLoop: true,
            EnableDirectNextSteps: false,
            EnableQuestionLog: false,
            EnableLiveClassroom: false,
            EnableDiagnosticV2: false);

    private static string FindRoot()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);

        while (directory is not null)
        {
            if (File.Exists(
                    Path.Combine(
                        directory.FullName,
                        "Edulytics.sln")))
            {
                return directory.FullName;
            }

            directory = directory.Parent;
        }

        throw new DirectoryNotFoundException(
            "Edulytics solution root not found.");
    }
}
