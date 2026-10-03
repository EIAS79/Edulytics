namespace Edulytics.Core.Entities;

public sealed class PersonalEntitlement
{
    public Guid Id { get; set; }
    public Guid SubscriptionId { get; set; }
    public Guid StudentUserId { get; set; }

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

    public DateTime StartsAtUtc { get; set; }
    public DateTime EndsAtUtc { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public DateTime UpdatedAtUtc { get; set; }
    public byte[] RowVersion { get; set; } = [];
}
