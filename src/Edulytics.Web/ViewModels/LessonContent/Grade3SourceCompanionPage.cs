using Edulytics.Web.Curriculum.Grade3;

namespace Edulytics.Web.ViewModels.LessonContent;

public sealed class Grade3SourceCompanionAnswers
{
    public string? PracticeAnswer { get; set; }
    public string? PracticeReasoning { get; set; }
    public string? SourceWorkReflection { get; set; }
    public string? CoolDownAnswer { get; set; }
    public string? CoolDownReasoning { get; set; }
}

public sealed record Grade3SourceCompanionPage(
    Guid CurriculumAdoptionId,
    Guid LessonId,
    Grade3CompanionLesson Lesson,
    Grade3SourceCompanionAnswers Answers,
    bool Submitted,
    Grade3CompanionFeedback? PracticeFeedback,
    Grade3CompanionFeedback? CoolDownFeedback);
