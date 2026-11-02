using Edulytics.Core.Lessons;

namespace Edulytics.Web.ViewModels.Platform;

public sealed record TemporaryPracticePreviewViewModel(
    string LessonCode,
    string Title,
    string UnitTitle,
    string FrameworkName,
    string GradeLabel,
    string SubjectName,
    string SubjectCode,
    string Explanation,
    string KeyConceptsAndRules,
    string WorkedExamples,
    string StepByStepSolutions,
    string CommonMistakes,
    string QuickSummary,
    DateTime PublishedAtUtc,
    bool IsSupporting,
    IReadOnlyList<LessonOutcomeRecord> Outcomes);
