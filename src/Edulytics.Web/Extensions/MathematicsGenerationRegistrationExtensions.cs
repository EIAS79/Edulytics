using Edulytics.Services.MathematicsGeneration;

namespace Edulytics.Web.Extensions;

public static class MathematicsGenerationRegistrationExtensions
{
    public static IServiceCollection AddMathematicsGenerationPhase33(
        this IServiceCollection services)
    {
        services.AddSingleton<MathematicsQuestionGenerationEngine>();
        return services;
    }
}
