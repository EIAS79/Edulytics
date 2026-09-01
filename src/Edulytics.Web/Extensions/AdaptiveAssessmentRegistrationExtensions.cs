using Edulytics.Services.AdaptiveAssessment;

namespace Edulytics.Web.Extensions;

public static class AdaptiveAssessmentRegistrationExtensions
{
    public static IServiceCollection AddAdaptiveAssessmentPhase35(
        this IServiceCollection services)
    {
        services.AddSingleton<AdaptiveDiagnosticAssessmentEngine>();
        return services;
    }
}
