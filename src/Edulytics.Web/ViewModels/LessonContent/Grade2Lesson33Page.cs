namespace Edulytics.Web.ViewModels.LessonContent;
public sealed class Grade2Lesson33Answers
{
    public string? TalkA{get;set;}public string? TalkB{get;set;}
    public string? TalkC{get;set;}public string? TalkD{get;set;}
    public string? CompensationExplain{get;set;}
    public string? RealSourceCardDescription{get;set;}
    public string? StudentAuthoredStory{get;set;}
    public string? StudentAuthoredEquation{get;set;}
    public string? ActualPartnerSolution{get;set;}
    public string? ChoiceCenter{get;set;}
    public string? ChoiceReflection{get;set;}
    public string? FirstPartyTotal{get;set;}
    public string? FirstPartyPart{get;set;}
    public string? FirstPartyGap{get;set;}
    public string? ExitCompensate{get;set;}
    public string? ExitAdd{get;set;}
    public string? ExitSubtract{get;set;}
}
public sealed record Grade2Lesson33Page(Guid AdoptionId,Guid LessonId,string Title,bool Submitted,Grade2Lesson33Answers Answers,IReadOnlyList<Grade2Lesson33Feedback> Feedback);
public sealed record Grade2Lesson33Feedback(string Section,bool? Correct,bool TeacherReviewNeeded,string Feedback);
