using Edulytics.Services.LearningIntelligence;

namespace Edulytics.Web.Extensions;

public static class LearningIntelligenceRegistrationExtensions
{
    public static IServiceCollection AddLearningIntelligencePhase37(
        this IServiceCollection services)
    {
        services.AddSingleton<LearningIntelligenceEngine>();
        return services;
    }
}
