using Edulytics.Services.AssessmentIntelligence;

namespace Edulytics.Web.Extensions;

public static class AssessmentIntelligenceRegistrationExtensions
{
    public static IServiceCollection AddAssessmentIntelligencePhase32(
        this IServiceCollection services)
    {
        services.AddSingleton<AssessmentBlueprintEngine>();
        return services;
    }
}
