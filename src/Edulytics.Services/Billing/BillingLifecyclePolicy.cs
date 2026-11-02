using Edulytics.Core.Enums;
using Edulytics.Services.Subscriptions;

namespace Edulytics.Services.Billing;

/// <summary>
/// Projects invoice aging into the commercial subscription lifecycle without
/// changing invoice calculations or the existing 14-day due / 7-day grace policy.
/// Suspension remains an explicit administrative action; payment recovery can
/// restore a payment-suspended subscription once enforceable delinquency is gone.
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
                or SubscriptionStatus.Trial)
        {
            return currentStatus;
        }

        if (currentStatus == SubscriptionStatus.Suspended &&
            (aging.SuspensionEligible || aging.InGracePeriod))
        {
            return SubscriptionStatus.Suspended;
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

        return nonRenewalRequested
            ? SubscriptionStatus.CancellationPending
            : SubscriptionStatus.Active;
    }
}
