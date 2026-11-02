using Edulytics.Core.Enums;
using Edulytics.Services.Billing;

namespace Edulytics.Tests.Phase25D;

public sealed class BillingLifecyclePolicyTests
{
    [Fact]
    public void GraceWindow_ProjectsToGracePeriod()
    {
        var aging = new BillingAgingState(
            BillingInvoiceStatus.Overdue,
            InGracePeriod: true,
            SuspensionEligible: false,
            OutstandingAmount: 100m);

        Assert.Equal(
            SubscriptionStatus.GracePeriod,
            BillingLifecyclePolicy.ResolveOperationalStatus(
                SubscriptionStatus.Active,
                aging,
                nonRenewalRequested: false));
    }

    [Fact]
    public void BeyondGrace_ProjectsToPastDueButDoesNotSuspendAutomatically()
    {
        var aging = new BillingAgingState(
            BillingInvoiceStatus.Overdue,
            InGracePeriod: false,
            SuspensionEligible: true,
            OutstandingAmount: 100m);

        Assert.Equal(
            SubscriptionStatus.PastDue,
            BillingLifecyclePolicy.ResolveOperationalStatus(
                SubscriptionStatus.Active,
                aging,
                nonRenewalRequested: false));
    }

    [Fact]
    public void SuspendedWithDelinquency_RemainsSuspended()
    {
        var aging = new BillingAgingState(
            BillingInvoiceStatus.Overdue,
            InGracePeriod: false,
            SuspensionEligible: true,
            OutstandingAmount: 100m);

        Assert.Equal(
            SubscriptionStatus.Suspended,
            BillingLifecyclePolicy.ResolveOperationalStatus(
                SubscriptionStatus.Suspended,
                aging,
                nonRenewalRequested: false));
    }

    [Theory]
    [InlineData(SubscriptionStatus.PastDue)]
    [InlineData(SubscriptionStatus.Suspended)]
    public void ClearedBalance_ReturnsToActive(SubscriptionStatus currentStatus)
    {
        var aging = new BillingAgingState(
            BillingInvoiceStatus.Paid,
            InGracePeriod: false,
            SuspensionEligible: false,
            OutstandingAmount: 0m);

        Assert.Equal(
            SubscriptionStatus.Active,
            BillingLifecyclePolicy.ResolveOperationalStatus(
                currentStatus,
                aging,
                nonRenewalRequested: false));
    }

    [Fact]
    public void ClearedBalanceWithNonRenewal_ReturnsToCancellationPending()
    {
        var aging = new BillingAgingState(
            BillingInvoiceStatus.Paid,
            InGracePeriod: false,
            SuspensionEligible: false,
            OutstandingAmount: 0m);

        Assert.Equal(
            SubscriptionStatus.CancellationPending,
            BillingLifecyclePolicy.ResolveOperationalStatus(
                SubscriptionStatus.GracePeriod,
                aging,
                nonRenewalRequested: true));
    }

    [Fact]
    public void CurrentDueBalanceWithNonRenewal_RemainsCancellationPending()
    {
        var aging = new BillingAgingState(
            BillingInvoiceStatus.Due,
            InGracePeriod: false,
            SuspensionEligible: false,
            OutstandingAmount: 100m);

        Assert.Equal(
            SubscriptionStatus.CancellationPending,
            BillingLifecyclePolicy.ResolveOperationalStatus(
                SubscriptionStatus.Active,
                aging,
                nonRenewalRequested: true));
    }

    [Theory]
    [InlineData(SubscriptionStatus.Expired)]
    [InlineData(SubscriptionStatus.Cancelled)]
    public void BillingProjection_DoesNotBypassTerminalStates(
        SubscriptionStatus status)
    {
        var aging = new BillingAgingState(
            BillingInvoiceStatus.Paid,
            InGracePeriod: false,
            SuspensionEligible: false,
            OutstandingAmount: 0m);

        Assert.Equal(
            status,
            BillingLifecyclePolicy.ResolveOperationalStatus(
                status,
                aging,
                nonRenewalRequested: false));
    }
}
