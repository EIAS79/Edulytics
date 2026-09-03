using Edulytics.Core.Enums;
using Edulytics.Services.Subscriptions;

namespace Edulytics.Services.Billing;

/// <summary>
/// Projects invoice aging into the commercial subscription lifecycle without
/// changing invoice calculations or the existing 14-day due / 7-day grace policy.
/// </summary>
public static class BillingLifecyclePolicy
{
    public static SubscriptionStatus ResolveOperationalStatus(
        SubscriptionStatus currentStatus,
        BillingAgingState aging,
        bool nonRenewalRequested)
    {
        if (SubscriptionLifecyclePolicy.IsTerminal(currentStatus) ||
            currentStatus is SubscriptionStatus.PendingActivation
                or SubscriptionStatus.Trial
                or SubscriptionStatus.Suspended)
        {
            return currentStatus;
        }

        if (aging.OutstandingAmount <= 0m)
        {
            return nonRenewalRequested
                ? SubscriptionStatus.CancellationPending
                : SubscriptionStatus.Active;
        }

        if (aging.SuspensionEligible)
            return SubscriptionStatus.PastDue;

        if (aging.InGracePeriod)
            return SubscriptionStatus.GracePeriod;

        return nonRenewalRequested && currentStatus == SubscriptionStatus.CancellationPending
            ? SubscriptionStatus.CancellationPending
            : SubscriptionStatus.Active;
    }
}
