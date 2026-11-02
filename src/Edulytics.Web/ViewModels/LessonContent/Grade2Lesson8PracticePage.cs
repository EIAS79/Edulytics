namespace Edulytics.Web.ViewModels.LessonContent;

public sealed class Grade2Lesson8Answers
{
    public string? AdultsBasketball {get;set;}
    public string? ChildrenAdultsSoccerGap {get;set;}
    public string? Markers {get;set;}
    public string? CrayonsAndClay {get;set;}
    public string? MoreMarkersThanCrayons {get;set;}
    public string? PaintAndMarkers {get;set;}
    public string? CanAskOne {get;set;}
    public string? CanAskTwo {get;set;}
    public string? CanAskThree {get;set;}
    public string? CanAskFour {get;set;}
    public string? CanAskFive {get;set;}
    public string? CoolTotal {get;set;}
    public string? CoolDifference {get;set;}
    public string? Explanation {get;set;}
}
public sealed record Grade2Lesson8PracticePage(
    Guid AdoptionId,Guid LessonId,string LessonTitle,bool Submitted,
    Grade2Lesson8Answers Answers,IReadOnlyList<Grade2Lesson8Feedback> Feedback);
public sealed record Grade2Lesson8Feedback(
    string Task,bool? Correct,bool TeacherReviewNeeded,string Text);
