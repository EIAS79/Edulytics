namespace Edulytics.Core.Enums;

public enum SubscriptionTerm
{
    ThreeMonths = 3,
    SixMonths = 6,
    SchoolYearTenMonths = 10
}

public enum SubscriptionStatus
{
    PendingActivation = 1,
    Trial = 2,
    Active = 3,
    GracePeriod = 4,
    PastDue = 5,
    CancellationPending = 6,
    Suspended = 7,
    Expired = 8,
    Cancelled = 9
}

public enum SubscriptionBillingCadence
{
    MonthlyInstallments = 1,
    FullTermUpfront = 2
}

public enum CommercialCurrency
{
    PLN = 1,
    AED = 2
}

public enum SeatCommitmentChangeType
{
    Initial = 1,
    Increase = 2,
    RenewalAdjustment = 3
}
