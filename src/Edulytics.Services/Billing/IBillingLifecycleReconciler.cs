namespace Edulytics.Services.Billing;

public interface IBillingLifecycleReconciler
{
    Task<BillingCommandResult> ReconcileAsync(
        Guid actorUserId,
        CancellationToken cancellationToken = default);
}
