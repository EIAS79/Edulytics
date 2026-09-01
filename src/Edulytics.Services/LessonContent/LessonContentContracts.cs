using Edulytics.Core.Enums;
using Edulytics.Core.Lessons;
namespace Edulytics.Services.LessonContent;

public enum LessonContentErrorCode { AccessDenied=1, SchoolNotActive=2, LessonNotFound=3 }

public sealed record LessonContentQueryResult<T>(T? Value,LessonContentErrorCode? Error) where T:class
{
    public static LessonContentQueryResult<T> Success(T value)=>new(value,null);
    public static LessonContentQueryResult<T> Failure(LessonContentErrorCode error)=>new(null,error);
}

public sealed record LessonContentSelection(
    Guid? AcademicYearId = null,
    Guid? AcademicProgramId = null,
    Guid? CurriculumAdoptionId = null);

public sealed record LessonContentCurriculumOption(
    Guid CurriculumAdoptionId,
    Guid AcademicYearId,
    string AcademicYearName,
    Guid AcademicProgramId,
    string AcademicProgramName,
    string AcademicProgramCode,
    string FrameworkName,
    string CurriculumLevelKey,
    string CurriculumLevelLabel,
    string? CurriculumPathway);

public sealed record CanonicalLessonLibraryItem(
    Guid LessonId,string LessonCode,string LessonTitle,string UnitTitle,int SortOrder,
    CanonicalLessonContentStatus? Status,DateTime? PublishedAtUtc,bool HasOfficialAlignment)
{
    public bool IsSupporting { get; init; }
}

public sealed record CanonicalCurriculumLibraryGroup(
    Guid FrameworkVersionId,string FrameworkName,string FrameworkVersionName,string SubjectName,string SubjectCode,
    string GradeName,int TotalLessons,int ProductionReadyLessons,IReadOnlyList<CanonicalLessonLibraryItem> Lessons)
{
    public Guid CurriculumAdoptionId { get; init; }
    public Guid AcademicYearId { get; init; }
    public string AcademicYearName { get; init; } = string.Empty;
    public Guid AcademicProgramId { get; init; }
    public string AcademicProgramName { get; init; } = string.Empty;
    public string AcademicProgramCode { get; init; } = string.Empty;
    public string CurriculumLevelKey { get; init; } = string.Empty;
    public string CurriculumLevelLabel { get; init; } = string.Empty;
    public string? CurriculumPathway { get; init; }
}

public sealed record LessonContentDashboard(Guid SchoolId,IReadOnlyList<CanonicalCurriculumLibraryGroup> Curricula)
{
    public IReadOnlyList<LessonContentCurriculumOption> Options { get; init; } = [];
    public Guid? SelectedAcademicYearId { get; init; }
    public Guid? SelectedAcademicProgramId { get; init; }
    public Guid? SelectedCurriculumAdoptionId { get; init; }
}

public sealed record CanonicalLessonDetail(
    Guid LessonId,string LessonCode,string LessonTitle,string UnitTitle,string FrameworkName,string FrameworkVersionName,
    string SubjectName,string SubjectCode,string GradeName,CanonicalLessonContentStatus? Status,DateTime? PublishedAtUtc,
    CanonicalLessonTranslationRecord? Body,IReadOnlyList<LessonOutcomeRecord> Outcomes)
{
    public bool IsSupporting { get; init; }
}

public sealed record StudentLessonSummary(
    Guid Id,string Title,string TopicName,string SubjectName,string SubjectCode,string GradeName,string FrameworkName,int Order,
    bool IsSupporting=false);

public sealed record StudentLessonDetail(
    Guid Id,string Title,string TopicName,string SubjectName,string SubjectCode,string GradeName,string FrameworkName,
    string Explanation,string KeyConceptsAndRules,string WorkedExamples,string StepByStepSolutions,string CommonMistakes,
    string QuickSummary,IReadOnlyList<LessonOutcomeRecord> Outcomes,DateTime PublishedAtUtc,bool IsSupporting=false);
