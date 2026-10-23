namespace Edulytics.Web.ViewModels.LessonContent;

/// <summary>Independent source-goal learner activity for Grade 2 Unit 1 Lesson 2.</summary>
public sealed record Grade2Lesson2PracticePage(
    Guid CurriculumAdoptionId,
    Guid LessonId,
    string Title,
    bool Submitted,
    string? Hidden,
    string? Removed,
    string? Addition,
    string? Subtraction,
    string? Tower,
    string? Reasoning,
    IReadOnlyList<Grade2Lesson2Feedback> Feedback);

public sealed record Grade2Lesson2Feedback(
    string Task,bool? Correct,bool TeacherReviewNeeded,string Message);
