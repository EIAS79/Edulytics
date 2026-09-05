using Edulytics.Core.Entities;
using Edulytics.Core.Practice;

namespace Edulytics.Services.Practice;

public enum StudentPrivatePracticeScope
{
    Lesson = 1,
    Unit = 2,
    WholeCurriculum = 3,
    WeakAreas = 4
}

public enum StudentPrivatePracticeDifficulty
{
    MyLevel = 1,
    AtClassLevel = 2,
    Stretch = 3,
    Challenge = 4
}

public enum StudentPrivatePracticeError
{
    AccessDenied = 1,
    CurriculumNotAvailable = 2,
    InvalidScope = 3,
    NoSupportedOutcomes = 4,
    GenerationFailed = 5,
    InvalidQuestionCount = 6
}

public sealed record StudentPrivatePracticeLessonOption(
    Guid LessonId,
    string UnitKey,
    string UnitTitle,
    string LessonCode,
    string LessonTitle);

public sealed record StudentPrivatePracticeUnitOption(
    string UnitKey,
    string UnitTitle);

public sealed record StudentPrivatePracticeWorkspace(
    IReadOnlyList<PrivatePracticeCurriculumOption> Curricula,
    Guid? SelectedCurriculumAdoptionId,
    IReadOnlyList<StudentPrivatePracticeLessonOption> Lessons,
    IReadOnlyList<string> UnitKeys,
    IReadOnlyList<PrivatePracticeAttemptSummary> Attempts,
    IReadOnlyList<StudentPrivatePracticeUnitOption>? Units = null);

public sealed record GenerateStudentPrivatePracticeRequest(
    Guid CurriculumAdoptionId,
    StudentPrivatePracticeScope Scope,
    Guid? LessonId,
    string? UnitKey,
    StudentPrivatePracticeDifficulty Difficulty,
    int QuestionCount,
    int Seed = 0);

public sealed record StudentPrivatePracticeResult(
    Guid? AttemptId,
    StudentPrivatePracticeError? Error)
{
    public bool Succeeded => AttemptId.HasValue && Error is null;
    public static StudentPrivatePracticeResult Success(Guid id) => new(id, null);
    public static StudentPrivatePracticeResult Failure(StudentPrivatePracticeError error) => new(null, error);
}

public interface IStudentPrivatePracticeService
{
    Task<StudentPrivatePracticeWorkspace> GetWorkspaceAsync(
        Guid studentUserId,
        Guid? curriculumAdoptionId = null,
        CancellationToken cancellationToken = default);

    Task<StudentPrivatePracticeResult> GenerateAsync(
        Guid studentUserId,
        GenerateStudentPrivatePracticeRequest request,
        CancellationToken cancellationToken = default);
}
