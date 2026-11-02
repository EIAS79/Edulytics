namespace Edulytics.Core.DirectStudents;

public sealed record DirectCurriculumFrameworkRecord(
    Guid FrameworkVersionId,
    string FrameworkCode,
    string FrameworkName,
    string FrameworkVersionName,
    string CountryCode);
