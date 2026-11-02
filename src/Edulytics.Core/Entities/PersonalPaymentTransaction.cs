using Edulytics.Core.Enums;

namespace Edulytics.Core.Entities;

public sealed class PersonalPaymentTransaction
{
    public Guid Id { get; set; }
    public Guid SubscriptionId { get; set; }
    public Guid StudentUserId { get; set; }
    public string Provider { get; set; } = string.Empty;
    public string ProviderCheckoutSessionId { get; set; } = string.Empty;
    public string? ProviderPaymentId { get; set; }
    public string? ProviderEventId { get; set; }
    public decimal Amount { get; set; }
    public string CurrencyCode { get; set; } = "AED";
    public DirectPaymentStatus Status { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public DateTime UpdatedAtUtc { get; set; }
    public byte[] RowVersion { get; set; } = [];
}
