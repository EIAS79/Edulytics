using Edulytics.Core.Enums;

namespace Edulytics.Core.Entities;

public sealed class PersonalSubscription
{
    public Guid Id { get; set; }
    public Guid StudentUserId { get; set; }
    public DirectStudentPlanType PlanType { get; set; }
    public DirectStudentSubscriptionStatus Status { get; set; }

    public Guid FrameworkVersionId { get; set; }
    public string FrameworkCode { get; set; } = string.Empty;
    public string FrameworkName { get; set; } = string.Empty;
    public string FrameworkVersionName { get; set; } = string.Empty;
    public string CurriculumLevelKey { get; set; } = string.Empty;
    public int CurriculumLogicalLevel { get; set; }
    public string CurriculumLevelLabel { get; set; } = string.Empty;
    public string? CurriculumPathway { get; set; }
    public string SubjectCode { get; set; } = string.Empty;
    public string SubjectName { get; set; } = string.Empty;

    public decimal BaseAmount { get; set; }
    public decimal DiscountPercent { get; set; }
    public decimal PaidAmount { get; set; }
    public string CurrencyCode { get; set; } = "AED";

    public string PaymentProvider { get; set; } = string.Empty;
    public string? ExternalCustomerReference { get; set; }
    public string? ExternalPaymentReference { get; set; }

    public DateTime? ActivatedAtUtc { get; set; }
    public DateTime? StartsAtUtc { get; set; }
    public DateTime? EndsAtUtc { get; set; }
    public DateTime? CancelledAtUtc { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public DateTime UpdatedAtUtc { get; set; }
    public byte[] RowVersion { get; set; } = [];
}
