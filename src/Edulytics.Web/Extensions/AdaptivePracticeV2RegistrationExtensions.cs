using Edulytics.Core.AdaptivePractice;
using Edulytics.Data.Repositories;
using Edulytics.Services.AdaptivePractice;
using Edulytics.Services.Mathematics.Difficulty;
using Edulytics.Web.AdaptivePractice;
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

                    return options.AllowedSchoolIds.Any(
                               x => x != Guid.Empty) &&
                           AdaptivePrimaryRolloutPlan.IsValidCanaryScope(
                               options.AllowedCurriculumLevelKeys,
                               options.AllowedLessonCodes);
                },
                "AdaptivePracticeV2 Canary requires explicit curriculum-level, lesson, and school allow-lists.")
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
        services.AddSingleton<AdaptivePracticeEvidenceProjector>();
        services.AddScoped<IAdaptivePracticeRepository, AdaptivePracticeRepository>();
        services.AddScoped<IAdaptivePracticeV2Service, AdaptivePracticeV2Service>();
        services.AddScoped<IAdaptivePracticeShadowObserver, AdaptivePracticeShadowObserver>();

        return services;
    }
}
