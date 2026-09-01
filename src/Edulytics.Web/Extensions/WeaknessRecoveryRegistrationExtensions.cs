using Edulytics.Services.Recovery;

namespace Edulytics.Web.Extensions;

public static class WeaknessRecoveryRegistrationExtensions
{
    public static IServiceCollection AddWeaknessRecoveryPhase36(
        this IServiceCollection services)
    {
        services.AddSingleton<WeaknessRecoveryEngine>();
        return services;
    }
}
