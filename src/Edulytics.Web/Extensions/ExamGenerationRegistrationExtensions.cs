using Edulytics.Services.ExamGeneration;

namespace Edulytics.Web.Extensions;

public static class ExamGenerationRegistrationExtensions
{
    public static IServiceCollection AddExamGenerationPhase34(
        this IServiceCollection services)
    {
        services.AddSingleton<ExamGenerationEngine>();
        services.AddAdaptiveAssessmentPhase35();
        return services;
    }
}
