namespace Edulytics.Web.ViewModels.LessonContent;
public sealed class Grade2Lesson25Answers
{
    public string? WarmupGrouping{get;set;}
    public string? FirstDifference{get;set;}
    public string? FirstTradedTens{get;set;}
    public string? FirstTradedOnes{get;set;}
    public string? SecondDifference{get;set;}
    public string? SecondTradedTens{get;set;}
    public string? SecondTradedOnes{get;set;}
    public string? PartnerExplanation{get;set;}
    public string? RealPlayerCard{get;set;}
    public string? RealStartingNumber{get;set;}
    public string? RealCardActionOne{get;set;}
    public string? RealCardActionTwo{get;set;}
    public string? RealEndingNumber{get;set;}
    public string? RealGroupTotal{get;set;}
    public string? FirstPartyA{get;set;}
    public string? FirstPartyB{get;set;}
    public string? CoolA{get;set;}
    public string? CoolB{get;set;}
    public string? CoolC{get;set;}
}
public sealed record Grade2Lesson25Page(Guid AdoptionId,Guid LessonId,string Title,bool Submitted,
    Grade2Lesson25Answers Answers,IReadOnlyList<Grade2Lesson25Feedback> Feedback);
public sealed record Grade2Lesson25Feedback(string Section,bool? Correct,bool TeacherReviewNeeded,string Feedback);
