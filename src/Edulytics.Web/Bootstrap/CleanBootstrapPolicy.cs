using Microsoft.Extensions.Configuration;

namespace Edulytics.Web.Bootstrap;

/// <summary>
/// Opt-in, fail-closed account/demo provisioning guard for a newly rebuilt
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

        // Refuse contradictory configuration rather than silently creating a
        // demo school, synthetic student or a platform admin during clean init.
        var wantsPresentationDemo =
            configuration.GetValue<bool>("Edulytics:PresentationDemo:Provision");
        var wantsMeetingDemo =
            configuration.GetValue<bool>("Edulytics:MeetingDemo:ResetAndSeed");
        var hasSuperAdminEmail =
            !string.IsNullOrWhiteSpace(configuration["Edulytics:SuperAdmin:Email"]);
        var hasSuperAdminPassword =
            !string.IsNullOrWhiteSpace(configuration["Edulytics:SuperAdmin:Password"]);

        if (wantsPresentationDemo || wantsMeetingDemo ||
            hasSuperAdminEmail || hasSuperAdminPassword)
        {
            throw new InvalidOperationException(
                "Edulytics:Deployment:CleanBootstrap prohibits demo seeding " +
                "and automatic SuperAdmin provisioning. Disable demo flags " +
                "and remove SuperAdmin bootstrap credentials before clean init.");
        }

        return true;
    }
}
