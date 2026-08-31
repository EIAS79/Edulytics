using Edulytics.Core.Entities;

namespace Edulytics.Core.Curriculum;

public sealed record CurriculumSnapshot(
    IReadOnlyList<GradeLevel> GradeLevels,
    IReadOnlyList<Subject> Subjects,
    IReadOnlyList<CurriculumTopic> Topics,
    IReadOnlyList<LearningOutcome> Outcomes)
{
    public IReadOnlyList<AcademicProgram> AcademicPrograms { get; init; } = [];
    public IReadOnlyList<AcademicYear> AcademicYears { get; init; } = [];
    public IReadOnlyList<AcademicYearProgramOffering> AcademicYearProgramOfferings { get; init; } = [];
    public IReadOnlyList<SchoolCurriculumAdoption> CurriculumAdoptions { get; init; } = [];
}

public sealed record AdoptedCurriculumContext(
    Guid GradeLevelId,
    Guid SubjectId,
    Guid FrameworkVersionId,
    string FrameworkCode,
    string FrameworkName)
{
    public Guid AdoptionId { get; init; }
    public Guid? AcademicYearId { get; init; }
    public Guid AcademicProgramId { get; init; }
    public string AcademicProgramName { get; init; } = string.Empty;
    public string AcademicProgramCode { get; init; } = string.Empty;
    public string? CurriculumLevelKey { get; init; }
    public int? CurriculumLogicalLevel { get; init; }
    public string? CurriculumLevelLabel { get; init; }
    public string? CurriculumStage { get; init; }
    public string? CurriculumPathway { get; init; }
}

public sealed record OfficialCurriculumOutcomeSource(
    Guid ContentNodeId,
    Guid? LessonNodeId,
    string Code,
    string Description,
    string SelectionLabel,
    string? GroupLabel,
    int SortOrder);

public enum CurriculumPersistenceError
{
    None = 0,
    Constraint = 1
}

public sealed record CurriculumPersistenceResult(
    bool Succeeded,
    CurriculumPersistenceError Error)
{
    public static CurriculumPersistenceResult Success() =>
        new(true, CurriculumPersistenceError.None);

    public static CurriculumPersistenceResult Failure(
        CurriculumPersistenceError error) =>
        new(false, error);
}
