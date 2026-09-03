using Edulytics.Core.Enums;

namespace Edulytics.Services.Subscriptions;

/// <summary>
/// Central commercial lifecycle semantics for schools and subscriptions.
/// Existing persisted enum values are not renumbered when lifecycle states evolve.
/// </summary>
public static class SubscriptionLifecyclePolicy
{
    public static bool IsOperationalSchoolState(SchoolStatus status) =>
        status is SchoolStatus.Active or SchoolStatus.Trial;

    public static bool IsOperationalSubscriptionState(SubscriptionStatus status) =>
        status is SubscriptionStatus.Trial
            or SubscriptionStatus.Active
            or SubscriptionStatus.PastDue
            or SubscriptionStatus.GracePeriod
            or SubscriptionStatus.CancellationPending;

    public static bool IsCommerciallyBlocked(SubscriptionStatus status) =>
        status is SubscriptionStatus.PendingActivation
            or SubscriptionStatus.Suspended
            or SubscriptionStatus.Expired
            or SubscriptionStatus.Cancelled;

    public static bool IsTerminal(SubscriptionStatus status) =>
        status is SubscriptionStatus.Expired
            or SubscriptionStatus.Cancelled;

    public static bool IsPaymentRecoveryState(SubscriptionStatus status) =>
        status is SubscriptionStatus.PastDue
            or SubscriptionStatus.GracePeriod
            or SubscriptionStatus.Suspended;

    public static bool CanRemainOperationalWhileAwaitingEndOfTerm(
        SubscriptionStatus status,
        DateTime? currentTermEndsAtUtc,
        DateTime utcNow) =>
        status == SubscriptionStatus.CancellationPending &&
        currentTermEndsAtUtc.HasValue &&
        utcNow < currentTermEndsAtUtc.Value;
}
