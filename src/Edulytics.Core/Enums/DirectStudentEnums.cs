namespace Edulytics.Core.Enums;

public enum DirectStudentPlanType
{
    Monthly = 1,
    AnnualTenMonths = 2
}

public enum DirectStudentSubscriptionStatus
{
    Pending = 1,
    Active = 2,
    PaymentFailed = 3,
    PastDue = 4,
    Cancelled = 5,
    Expired = 6
}

public enum DirectPaymentStatus
{
    Pending = 1,
    Succeeded = 2,
    Failed = 3,
    Cancelled = 4
}

public enum StudentEntitlementSource
{
    School = 1,
    Personal = 2
}
