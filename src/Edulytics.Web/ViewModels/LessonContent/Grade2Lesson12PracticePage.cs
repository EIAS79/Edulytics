using Edulytics.Core.Mathematics.Practice;

namespace Edulytics.Web.ViewModels.LessonContent;

public sealed class Grade2Lesson12Answers
{
    public string? CategoryA {get;set;}
    public string? CategoryB {get;set;}
    public string? CategoryC {get;set;}
    public string? CountA {get;set;}
    public string? CountB {get;set;}
    public string? CountC {get;set;}
    public string? CollectionContext {get;set;}
    public string? GraphTitle {get;set;}
    public string? GraphKind {get;set;}
    public string? PictureKey {get;set;}
    public string? BarStart {get;set;}
    public string? BarStep {get;set;}
    public string? QuestionOne {get;set;}
    public string? QuestionTwo {get;set;}
    public string? AnswerOne {get;set;}
    public string? AnswerTwo {get;set;}
    public string? WarmOne {get;set;}
    public string? WarmTwo {get;set;}
    public string? WarmThree {get;set;}
    public string? WarmFour {get;set;}
    public string? CenterChoice {get;set;}
    public string? Explanation {get;set;}
}
public sealed record Grade2Lesson12PracticePage(
    Guid AdoptionId,Guid LessonId,string LessonTitle,bool Submitted,
    Grade2Lesson12Answers Answers,
    UsCcssG2Unit1TwelfthLessonSortAndDisplay.Collection? ValidatedCollection,
    bool CorrectGraph,
    IReadOnlyList<Grade2Lesson12Feedback> Feedback);
public sealed record Grade2Lesson12Feedback(
    string Activity,bool? Correct,bool TeacherReviewNeeded,string Feedback);
