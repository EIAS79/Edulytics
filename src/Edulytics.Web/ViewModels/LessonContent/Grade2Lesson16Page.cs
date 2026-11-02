namespace Edulytics.Web.ViewModels.LessonContent;

public sealed class Grade2Lesson16StoryAnswer
{
    public string? SourceId {get;set;}
    public string? UnknownPosition {get;set;}
    public string? LargerPerson {get;set;}
    public string? NumericalAnswer {get;set;}
    public string? Equation {get;set;}
}
public sealed class Grade2Lesson16Answers
{
    public string? TrueOne {get;set;}
    public string? TrueTwo {get;set;}
    public string? FalseThree {get;set;}
    public List<Grade2Lesson16StoryAnswer> SourceAnswers {get;set;}=
    [
        new(){SourceId="LibraryBooks"},new(){SourceId="LibraryPages"},
        new(){SourceId="ThreeReads1"},new(){SourceId="ThreeReads2"},
        new(){SourceId="ThreeReads3"},new(){SourceId="ThreeReads4"}
    ];
    public string? CoolLarger {get;set;}
    public string? CoolSmaller {get;set;}
    public string? CoolDifference {get;set;}
    public string? RepresentationExplanation {get;set;}
}
public sealed record Grade2Lesson16Page(
    Guid AdoptionId,Guid LessonId,string LessonTitle,bool Submitted,
    Grade2Lesson16Answers Answers,
    IReadOnlyList<Grade2Lesson16Feedback> Feedback);
public sealed record Grade2Lesson16Feedback(
    string Activity,bool? Correct,bool TeacherReviewNeeded,string Message);
