using Edulytics.Core.Mathematics.Rollout;
using Edulytics.Web.Mathematics;
using Microsoft.Extensions.Options;

namespace Edulytics.Web.Extensions;

public static class AdvancedMathematicsV3RegistrationExtensions
{
    public static IServiceCollection AddAdvancedMathematicsV3(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        services
            .AddOptions<AdvancedMathematicsV3Options>()
            .Bind(
                configuration.GetSection(
                    AdvancedMathematicsV3Options.SectionName))
            .Validate(
                options =>
                    !options.AdaptiveEnabled ||
                    options.Enabled,
                "Advanced Mathematics adaptive routing cannot be enabled while the programme is disabled.")
            .Validate(
                options =>
                    !options.ExamEnabled ||
                    options.Enabled,
                "Advanced Mathematics exam routing cannot be enabled while the programme is disabled.")
            .ValidateOnStart();

        services.AddSingleton(
            static serviceProvider =>
                serviceProvider
                    .GetRequiredService<
                        IOptions<AdvancedMathematicsV3Options>>()
                    .Value
                    .ToPolicy());

        return services;
    }
}
