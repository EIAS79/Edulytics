using Microsoft.Extensions.Configuration;

namespace Edulytics.Web.Bootstrap;

/// <summary>
/// Opt-in, fail-closed account provisioning guard for a newly rebuilt
/// PostgreSQL database. This does not change existing deployments unless
/// Edulytics:Deployment:CleanBootstrap is explicitly enabled.
/// </summary>
public static class CleanBootstrapPolicy
{
    public static bool ShouldSkipOptionalProvisioning(IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);

        if (!configuration.GetValue<bool>("Edulytics:Deployment:CleanBootstrap"))
            return false;

        // Prevent accidental platform administrator creation during a clean
        // database bootstrap. Demo provisioning is not part of production.

        var hasSuperAdminEmail =
            !string.IsNullOrWhiteSpace(configuration["Edulytics:SuperAdmin:Email"]);
        var hasSuperAdminPassword =
            !string.IsNullOrWhiteSpace(configuration["Edulytics:SuperAdmin:Password"]);

        if (hasSuperAdminEmail || hasSuperAdminPassword)
        {
            throw new InvalidOperationException(
                "Edulytics:Deployment:CleanBootstrap prohibits automatic " +
                "SuperAdmin provisioning. Remove bootstrap credentials before clean init.");
        }

        return true;
    }
}
