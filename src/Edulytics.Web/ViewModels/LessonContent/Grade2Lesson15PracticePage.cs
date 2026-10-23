using Edulytics.Core.Mathematics.Practice;

namespace Edulytics.Web.ViewModels.LessonContent;

public sealed class Grade2Lesson15CardAnswers
{
    public string? StoryId {get;set;}
    public string? DiagramKey {get;set;}
    public string? UnknownPosition {get;set;}
    public string? Answer {get;set;}
    public string? EquationA {get;set;}
    public string? EquationB {get;set;}
}
public sealed class Grade2Lesson15Answers
{
    public string? WarmA {get;set;}
    public string? WarmB {get;set;}
    public string? WarmC {get;set;}
    public string? WarmD {get;set;}
    public List<Grade2Lesson15CardAnswers> CardMatches {get;set;}=
        [new(){StoryId="A"},new(){StoryId="B"},new(){StoryId="C"}];
    public string? CoolSmaller {get;set;}
    public string? CoolLarger {get;set;}
    public string? CoolDifference {get;set;}
    public string? ExplainUnknownPositions {get;set;}
}
public sealed record Grade2Lesson15PracticePage(
    Guid AdoptionId,Guid LessonId,string LessonTitle,bool Submitted,
    Grade2Lesson15Answers Answers,
    IReadOnlyList<Grade2Lesson15Feedback> Feedback);
public sealed record Grade2Lesson15Feedback(
    string Task,bool? Correct,bool TeacherReviewNeeded,string Feedback);
