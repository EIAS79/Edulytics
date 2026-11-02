using Edulytics.Services.AdaptiveAssessment;
using Edulytics.Services.Mathematics.Difficulty;

namespace Edulytics.Web.Extensions;

public static class AdaptiveAssessmentRegistrationExtensions
{
    public static IServiceCollection AddAdaptiveAssessmentPhase35(
        this IServiceCollection services)
    {
        services.AddSingleton<MathematicsDifficultyEngine>();
        services.AddSingleton<AdaptiveDiagnosticAssessmentEngine>();
        return services;
    }
}
