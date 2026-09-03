using Edulytics.Core.Enums;
using Edulytics.Services.Subscriptions;

namespace Edulytics.Tests.Phase25C;

public sealed class CommercialLifecyclePolicyTests
{
    [Fact]
    public void SubscriptionLifecycle_HasNoLegacyEndedState()
    {
        Assert.Equal(1, (int)SubscriptionStatus.PendingActivation);
        Assert.Equal(2, (int)SubscriptionStatus.Trial);
        Assert.Equal(3, (int)SubscriptionStatus.Active);
        Assert.Equal(4, (int)SubscriptionStatus.GracePeriod);
        Assert.Equal(5, (int)SubscriptionStatus.PastDue);
        Assert.Equal(6, (int)SubscriptionStatus.CancellationPending);
        Assert.Equal(7, (int)SubscriptionStatus.Suspended);
        Assert.Equal(8, (int)SubscriptionStatus.Expired);
        Assert.Equal(9, (int)SubscriptionStatus.Cancelled);

        Assert.DoesNotContain("Ended", Enum.GetNames<SubscriptionStatus>());
    }

    [Theory]
    [InlineData(SubscriptionStatus.Trial)]
    [InlineData(SubscriptionStatus.Active)]
    [InlineData(SubscriptionStatus.PastDue)]
    [InlineData(SubscriptionStatus.GracePeriod)]
    [InlineData(SubscriptionStatus.CancellationPending)]
    public void OperationalSubscriptionStates_RemainUsable(SubscriptionStatus status)
    {
        Assert.True(SubscriptionLifecyclePolicy.IsOperationalSubscriptionState(status));
        Assert.False(SubscriptionLifecyclePolicy.IsCommerciallyBlocked(status));
    }

    [Theory]
    [InlineData(SubscriptionStatus.PendingActivation)]
    [InlineData(SubscriptionStatus.Suspended)]
    [InlineData(SubscriptionStatus.Expired)]
    [InlineData(SubscriptionStatus.Cancelled)]
    public void BlockedSubscriptionStates_DoNotAllowOperationalUse(SubscriptionStatus status)
    {
        Assert.False(SubscriptionLifecyclePolicy.IsOperationalSubscriptionState(status));
        Assert.True(SubscriptionLifecyclePolicy.IsCommerciallyBlocked(status));
    }

    [Theory]
    [InlineData(SubscriptionStatus.Expired)]
    [InlineData(SubscriptionStatus.Cancelled)]
    public void TerminalStates_AreExplicit(SubscriptionStatus status)
    {
        Assert.True(SubscriptionLifecyclePolicy.IsTerminal(status));
    }

    [Fact]
    public void CancellationPending_IsOperationalOnlyBeforeTermEnd()
    {
        var now = new DateTime(2026, 9, 3, 6, 0, 0, DateTimeKind.Utc);

        Assert.True(
            SubscriptionLifecyclePolicy.CanRemainOperationalWhileAwaitingEndOfTerm(
                SubscriptionStatus.CancellationPending,
                now.AddDays(1),
                now));

        Assert.False(
            SubscriptionLifecyclePolicy.CanRemainOperationalWhileAwaitingEndOfTerm(
                SubscriptionStatus.CancellationPending,
                now,
                now));
    }
}
