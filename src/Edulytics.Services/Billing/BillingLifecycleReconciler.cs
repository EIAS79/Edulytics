using Edulytics.Core.Billing;
using Edulytics.Core.Constants;
using Edulytics.Core.Entities;
using Edulytics.Core.Enums;
using Edulytics.Core.Interfaces;
using Edulytics.Core.Subscriptions;
using Edulytics.Core.Users;
using Edulytics.Services.Auditing;
using Edulytics.Services.Subscriptions;

namespace Edulytics.Services.Billing;

public sealed class BillingLifecycleReconciler : IBillingLifecycleReconciler
{
    private readonly IBillingRepository _billing;
    private readonly ISchoolSubscriptionRepository _subscriptions;
    private readonly ISchoolRepository _schools;
    private readonly ISchoolUserRepository _users;
    private readonly IAuditService _audit;
    private readonly IApplicationTransactionManager _transactions;

    public BillingLifecycleReconciler(
        IBillingRepository billing,
        ISchoolSubscriptionRepository subscriptions,
        ISchoolRepository schools,
        ISchoolUserRepository users,
        IAuditService audit,
        IApplicationTransactionManager transactions)
    {
        _billing = billing;
        _subscriptions = subscriptions;
        _schools = schools;
        _users = users;
        _audit = audit;
        _transactions = transactions;
    }

    public async Task<BillingCommandResult> ReconcileAsync(
        Guid actorUserId,
        CancellationToken cancellationToken = default)
    {
        var actor = await _users.GetActorAsync(actorUserId, cancellationToken);
        if (actor is null ||
            !actor.IsActive ||
            actor.IsLocked ||
            actor.SchoolId.HasValue ||
            actor.Roles.Count != 1 ||
            actor.Roles[0] != RoleNames.SuperAdmin)
        {
            return BillingCommandResult.Failure(BillingErrorCode.AccessDenied);
        }

        var subscriptions = await _subscriptions.ListAsync(cancellationToken);
        foreach (var listed in subscriptions)
        {
            var result = await ReconcileOneAsync(
                actor.Id,
                listed.SchoolId,
                cancellationToken);

            if (!result.Succeeded)
                return result;
        }

        return BillingCommandResult.Success();
    }

    private async Task<BillingCommandResult> ReconcileOneAsync(
        Guid actorUserId,
        Guid schoolId,
        CancellationToken cancellationToken)
    {
        await using var transaction =
            await _transactions.BeginAsync(cancellationToken);

        var subscription =
            await _subscriptions.GetForUpdateBySchoolAsync(
                schoolId,
                cancellationToken);

        if (subscription is null)
        {
            await transaction.RollbackAsync(cancellationToken);
            return BillingCommandResult.Failure(
                BillingErrorCode.SubscriptionNotFound);
        }

        if (subscription.Status is SubscriptionStatus.PendingActivation or
            SubscriptionStatus.Trial ||
            SubscriptionLifecyclePolicy.IsTerminal(subscription.Status))
        {
            await transaction.CommitAsync(cancellationToken);
            return BillingCommandResult.Success(subscription.Id);
        }

        var invoices = await _billing.ListInvoicesAsync(
            schoolId,
            cancellationToken);
        var aggregate = AggregateAging(invoices, DateTime.UtcNow);
        var nonRenewalRequested =
            !subscription.AutoRenew ||
            subscription.NonRenewalRequestedAtUtc.HasValue;

        var targetStatus = BillingLifecyclePolicy.ResolveOperationalStatus(
            subscription.Status,
            aggregate,
            nonRenewalRequested);

        if (targetStatus == subscription.Status)
        {
            await transaction.CommitAsync(cancellationToken);
            return BillingCommandResult.Success(subscription.Id);
        }

        var previousStatus = subscription.Status;
        var expectedSubscriptionVersion = subscription.RowVersion.ToArray();
        var now = DateTime.UtcNow;

        subscription.Status = targetStatus;
        subscription.UpdatedAtUtc = now;
        if (targetStatus != SubscriptionStatus.Suspended)
            subscription.SuspendedAtUtc = null;

        School? school = null;
        byte[]? expectedSchoolVersion = null;
        var restoringOperationalSchool =
            previousStatus == SubscriptionStatus.Suspended &&
            SubscriptionLifecyclePolicy.IsOperationalSubscriptionState(targetStatus);

        if (restoringOperationalSchool)
        {
            school = await _schools.GetForUpdateAsync(
                schoolId,
                cancellationToken);

            if (school is null)
            {
                await transaction.RollbackAsync(cancellationToken);
                return BillingCommandResult.Failure(BillingErrorCode.SchoolNotFound);
            }

            if (school.Status == SchoolStatus.Archived)
            {
                await transaction.CommitAsync(cancellationToken);
                return BillingCommandResult.Success(subscription.Id);
            }

            expectedSchoolVersion = school.RowVersion.ToArray();
            school.Status = targetStatus == SubscriptionStatus.Trial
                ? SchoolStatus.Trial
                : SchoolStatus.Active;
            school.UpdatedAtUtc = now;
        }

        await _audit.QueueAsync(
            new AuditEvent(
                SchoolId: schoolId,
                Action: "Billing.SubscriptionLifecycleReconciled",
                EntityType: "SchoolSubscription",
                EntityId: subscription.Id.ToString("D"),
                Feature: "Billing",
                OldValues: new Dictionary<string, object?>
                {
                    ["status"] = previousStatus.ToString()
                },
                NewValues: new Dictionary<string, object?>
                {
                    ["status"] = targetStatus.ToString(),
                    ["outstandingAmount"] = aggregate.OutstandingAmount,
                    ["inGracePeriod"] = aggregate.InGracePeriod,
                    ["suspensionEligible"] = aggregate.SuspensionEligible
                },
                ResultSummary:
                    "Subscription lifecycle reconciled from invoice aging.",
                ActorUserIdOverride: actorUserId,
                ActorRoleOverride: RoleNames.SuperAdmin),
            cancellationToken);

        SubscriptionPersistenceResult saved;
        if (school is not null && expectedSchoolVersion is not null)
        {
            saved = await _subscriptions.SaveWithSchoolAsync(
                subscription,
                expectedSubscriptionVersion,
                school,
                expectedSchoolVersion,
                cancellationToken: cancellationToken);
        }
        else
        {
            saved = await _subscriptions.SaveAsync(
                subscription,
                expectedSubscriptionVersion,
                cancellationToken: cancellationToken);
        }

        if (!saved.Succeeded)
        {
            await transaction.RollbackAsync(cancellationToken);
            return BillingCommandResult.Failure(
                saved.Error == SubscriptionPersistenceError.Concurrency
                    ? BillingErrorCode.ConcurrencyConflict
                    : BillingErrorCode.PersistenceError);
        }

        await transaction.CommitAsync(cancellationToken);
        return BillingCommandResult.Success(subscription.Id);
    }

    private static BillingAgingState AggregateAging(
        IReadOnlyList<BillingInvoice> invoices,
        DateTime utcNow)
    {
        var enforceable = invoices
            .Select(invoice => BillingCommercialPolicy.Aging(invoice, utcNow))
            .Where(state =>
                state.EffectiveStatus is not (
                    BillingInvoiceStatus.Cancelled or
                    BillingInvoiceStatus.Refunded or
                    BillingInvoiceStatus.PartiallyRefunded))
            .ToArray();

        var outstanding = BillingCommercialPolicy.RoundMoney(
            enforceable.Sum(x => x.OutstandingAmount));

        if (outstanding <= 0m)
        {
            return new BillingAgingState(
                BillingInvoiceStatus.Paid,
                InGracePeriod: false,
                SuspensionEligible: false,
                OutstandingAmount: 0m);
        }

        if (enforceable.Any(x => x.SuspensionEligible))
        {
            return new BillingAgingState(
                BillingInvoiceStatus.Overdue,
                InGracePeriod: false,
                SuspensionEligible: true,
                OutstandingAmount: outstanding);
        }

        if (enforceable.Any(x => x.InGracePeriod))
        {
            return new BillingAgingState(
                BillingInvoiceStatus.Overdue,
                InGracePeriod: true,
                SuspensionEligible: false,
                OutstandingAmount: outstanding);
        }

        var partiallyPaid = enforceable.Any(
            x => x.EffectiveStatus == BillingInvoiceStatus.PartiallyPaid);

        return new BillingAgingState(
            partiallyPaid
                ? BillingInvoiceStatus.PartiallyPaid
                : BillingInvoiceStatus.Due,
            InGracePeriod: false,
            SuspensionEligible: false,
            OutstandingAmount: outstanding);
    }
}
