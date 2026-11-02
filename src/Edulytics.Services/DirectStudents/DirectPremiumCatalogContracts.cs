namespace Edulytics.Services.DirectStudents;

public sealed record DirectPremiumPlan(
    string Code,
    string Name,
    int DurationMonths,
    decimal BaseAmountAed,
    decimal DiscountPercent,
    decimal FinalAmountAed);

public sealed record DirectCurriculumOption(
    Guid FrameworkVersionId,
    string FrameworkCode,
    string FrameworkName,
    string FrameworkVersionName,
    string CountryCode,
    IReadOnlyList<DirectCurriculumLevelOption> Levels);

public sealed record DirectCurriculumLevelOption(
    string Key,
    int LogicalLevel,
    string Label,
    string Stage,
    string? Pathway);

public sealed record DirectSubjectOption(
    string Code,
    string Name);

public sealed record DirectPremiumCatalog(
    IReadOnlyList<DirectPremiumPlan> Plans,
    IReadOnlyList<DirectCurriculumOption> Curricula,
    IReadOnlyList<DirectSubjectOption> Subjects);
