using Edulytics.Core.AdaptivePractice;
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
        services.AddSingleton<AdaptivePracticeShadowEvaluator>();
        services.AddSingleton<AdaptiveVerifiedItemGenerator>();

        return services;
    }
}
