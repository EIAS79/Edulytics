using Edulytics.Core.Constants;
using Edulytics.Services.LearningIntelligence;

namespace Edulytics.Web.Extensions;

public static class LearningIntelligenceRegistrationExtensions
{
    public static IServiceCollection AddLearningIntelligencePhase37(
        this IServiceCollection services)
    {
        services.AddSingleton<LearningIntelligenceEngine>();
        services.AddAuthorization(options =>
        {
            options.AddPolicy(
                "LearningIntelligenceRead",
                policy => policy.RequireRole(
                    RoleNames.SchoolAdmin,
                    RoleNames.SubjectSupervisor));
        });
        return services;
    }
}
