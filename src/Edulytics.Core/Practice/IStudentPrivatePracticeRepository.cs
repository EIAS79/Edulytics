using Edulytics.Core.Entities;
using Edulytics.Core.Enums;

namespace Edulytics.Core.Practice;

public sealed record PrivatePracticeCurriculumOption(
    Guid CurriculumAdoptionId,
    Guid ClassGroupId,
    Guid AcademicYearId,
    string CurriculumLevelLabel,
    string ClassName);

public sealed record StudentPrivatePracticeContext(
    StudentProfile Student,
    SchoolCurriculumAdoption Adoption,
    ClassGroup ClassGroup,
    StudentEnrollment Enrollment,
    IReadOnlyList<LearningOutcome> LearningOutcomes,
    IReadOnlyList<CurriculumPedagogicalLesson> Lessons,
    IReadOnlyList<CurriculumPedagogicalLessonOutcome> LessonOutcomes,
    IReadOnlyList<StudentOutcomeMastery> OfficialMasteries,
    IReadOnlyList<StudentItemExposure> Exposures);

public sealed record PrivatePracticeAttemptSummary(
    Guid AttemptId,
    Guid CurriculumAdoptionId,
    Guid? PedagogicalLessonId,
    PracticeAttemptStatus Status,
    DateTime StartedAtUtc,
    DateTime? SubmittedAtUtc,
    decimal Score,
    decimal MaxScore,
    decimal Percentage);

public interface IStudentPrivatePracticeRepository
{
    Task<IReadOnlyList<PrivatePracticeCurriculumOption>> ListCurriculaAsync(
        Guid studentUserId,
        CancellationToken cancellationToken = default);

    Task<StudentPrivatePracticeContext?> GetContextAsync(
        Guid studentUserId,
        Guid curriculumAdoptionId,
        CancellationToken cancellationToken = default);

    Task AddGeneratedAttemptAsync(
        IReadOnlyList<AssessmentItem> items,
        IReadOnlyList<AssessmentItemOutcome> itemOutcomes,
        PracticeAttempt attempt,
        IReadOnlyList<PracticeAttemptItem> attemptItems,
        IReadOnlyList<StudentItemExposure> exposures,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<PrivatePracticeAttemptSummary>> ListPrivateAttemptsAsync(
        Guid studentUserId,
        CancellationToken cancellationToken = default);
}
