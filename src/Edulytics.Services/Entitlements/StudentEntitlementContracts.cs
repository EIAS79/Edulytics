using Edulytics.Core.Enums;

namespace Edulytics.Services.Entitlements;

public sealed record StudentContentEntitlement(
    StudentEntitlementSource Source,
    Guid FrameworkVersionId,
    string FrameworkCode,
    string FrameworkName,
    string FrameworkVersionName,
    string CurriculumLevelKey,
    int CurriculumLogicalLevel,
    string CurriculumLevelLabel,
    string? CurriculumPathway,
    string SubjectCode,
    string SubjectName,
    DateTime? StartsAtUtc,
    DateTime? EndsAtUtc);

public sealed record StudentEntitlementSnapshot(
    Guid StudentUserId,
    IReadOnlyList<StudentContentEntitlement> Items)
{
    public bool HasPersonalAccess =>
        Items.Any(x => x.Source == StudentEntitlementSource.Personal);

    public bool HasSchoolAccess =>
        Items.Any(x => x.Source == StudentEntitlementSource.School);
}
