using Edulytics.Core.AdaptivePractice;
using Edulytics.Data.Repositories;
using Edulytics.Services.AdaptivePractice;
using Edulytics.Services.AdaptiveAssessment;
using Edulytics.Services.Mathematics.Difficulty;
using Edulytics.Web.AdaptivePractice;
using Edulytics.Web.Health;
using Microsoft.Extensions.Options;

namespace Edulytics.Web.Extensions;

public static class AdaptivePracticeV2RegistrationExtensions
{
    public static IServiceCollection AddAdaptivePracticeV2(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        services
            .AddOptions<AdaptivePracticeV2Options>()
            .Bind(configuration.GetSection(AdaptivePracticeV2Options.SectionName))
            .Validate(
                options =>
                    options.ShadowSamplingPercentage is >= 0 and <= 100,
                "AdaptivePracticeV2 ShadowSamplingPercentage must be between 0 and 100.")
            .Validate(
                options =>
                    options.MaxLessonQuestions is >= 1 and <= 30,
                "AdaptivePracticeV2 MaxLessonQuestions must be between 1 and 30.")
            .Validate(
                options =>
                    options.MinimumPsychometricResponses is >= 10 and <= 10000 &&
                    options.MinimumPsychometricStudents is >= 5 and <= 1000 &&
                    options.MinimumResearchCohortSize is >= 10 and <= 10000 &&
                    options.MaximumIntelligenceRows is >= 50 and <= 2000,
                "AdaptivePracticeV2 intelligence thresholds or resource limits are invalid.")
            .Validate(
                options =>
                    !options.EnableLiveGroupSession ||
                    options.EnableLiveClassroom,
                "AdaptivePracticeV2 Live Group Session requires Live Classroom Intelligence.")
            .Validate(
                options =>
                    Enum.TryParse<AdaptivePracticeV2Mode>(
                        options.Mode,
                        ignoreCase: true,
                        out _),
                "AdaptivePracticeV2 Mode must be Off, Shadow, Canary, or On.")
            .Validate(
                options =>
                {
                    if (!options.Enabled ||
                        !Enum.TryParse<AdaptivePracticeV2Mode>(
                            options.Mode,
                            ignoreCase: true,
                            out var mode) ||
                        mode != AdaptivePracticeV2Mode.Canary)
                    {
                        return true;
                    }

                    var validCurriculumScope =
                        options.RouteAllReadyVerifiedLessons
                            ? AdaptivePrimaryRolloutPlan.IsValidReadyVerifiedScope(
                                options.AllowedCurriculumLevelKeys)
                            : AdaptivePrimaryRolloutPlan.IsValidCanaryScope(
                                options.AllowedCurriculumLevelKeys,
                                options.AllowedLessonCodes);

                    return options.AllowedSchoolIds.Any(
                               x => x != Guid.Empty) &&
                           validCurriculumScope;
                },
                "AdaptivePracticeV2 Canary requires an explicit Primary curriculum scope and school allow-list; legacy lesson allow-lists are required only when unified READY_VERIFIED routing is disabled.")
            .ValidateOnStart();

        services.AddSingleton(
            static serviceProvider =>
                serviceProvider
                    .GetRequiredService<IOptions<AdaptivePracticeV2Options>>()
                    .Value
                    .ToPolicy());

        services.AddSingleton<IAdaptivePracticeEligibilityResolver,
            AdaptivePracticeEligibilityResolver>();

        services.AddSingleton<MathematicsDifficultyEngine>();
        services.AddSingleton<AdaptiveNextItemDecisionEngine>();
        services.AddSingleton<AdaptiveRemediationStateMachine>();
        services.AddSingleton<AdaptiveLearningStateAssembler>();
        services.AddSingleton<AdaptivePracticeShadowEvaluator>();
        services.AddSingleton<AdaptiveVerifiedItemGenerator>();
        services.AddSingleton<AdaptiveMisconceptionClassifier>();
        services.AddSingleton<AdaptiveRemediationGuidanceEngine>();
        services.AddSingleton<AdaptivePracticeEvidenceProjector>();
        services.AddSingleton<AdaptiveDiagnosticAssessmentEngine>();
        services.AddScoped<IAdaptivePracticeRepository, AdaptivePracticeRepository>();
        services.AddScoped<IAdaptivePracticeV2Service, AdaptivePracticeV2Service>();
        services.AddScoped<IAdaptiveIntelligenceV2Service, AdaptiveIntelligenceV2Service>();
        services.AddScoped<IAdaptiveProgrammeClosureService, AdaptiveProgrammeClosureService>();
        services.AddScoped<IAdaptivePracticeShadowObserver, AdaptivePracticeShadowObserver>();

        services.AddHealthChecks()
            .AddCheck<AdaptivePracticeV2ReadinessHealthCheck>(
                "adaptive-v2",
                tags: ["ready"]);

        return services;
    }
}
