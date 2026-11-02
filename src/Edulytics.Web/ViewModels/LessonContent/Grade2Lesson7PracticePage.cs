namespace Edulytics.Web.ViewModels.LessonContent;

/// <summary>Source-goal classroom survey and student-authored representation.</summary>
public sealed record Grade2Lesson7PracticePage(
    Guid AdoptionId,Guid LessonId,string LessonTitle,
    bool Submitted,bool HasValidSurvey,
    string? RawResponses,
    string? WalkCount,string? CarCount,string? TrainCount,string? BusCount,
    string? TotalAnswer,string? BusAnswer,string? ComparisonAnswer,
    string? Explanation,
    IReadOnlyList<string> OriginalSequence,
    IReadOnlyDictionary<string,int> GroupedCounts,
    IReadOnlyList<Grade2Lesson7Feedback> Feedback);

public sealed record Grade2Lesson7Feedback(
    string Task,bool? Correct,bool TeacherReviewNeeded,string Explanation);
