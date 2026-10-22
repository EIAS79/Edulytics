namespace Edulytics.Web.ViewModels.LessonContent;
public sealed class Grade2Lesson40Answers
{
    public string? TalkA{get;set;}
    public string? TalkB{get;set;}
    public string? TalkC{get;set;}
    public string? TalkD{get;set;}
    public string? TalkStrategy{get;set;}
    public string? JadaLizard{get;set;}
    public string? DiegoSnake{get;set;}
    public string? DiagramDescription{get;set;}
    public string? DayGecko{get;set;}
    public string? Komodo{get;set;}
    public string? BabyCobra{get;set;}
    public string? Iguana{get;set;}
    public string? Ribbon{get;set;}
    public string? Gila{get;set;}
    public string? Alligator{get;set;}
    public string? Ringneck{get;set;}
    public string? SecretPartnerReptiles{get;set;}
    public string? RealPartnerClue{get;set;}
    public string? FirstPartyLonger{get;set;}
    public string? FirstPartyShorter{get;set;}
    public string? FirstPartyGap{get;set;}
}
public sealed record Grade2Lesson40Page(Guid AdoptionId,Guid LessonId,string Title,bool Submitted,Grade2Lesson40Answers Answers,IReadOnlyList<Grade2Lesson40Feedback> Feedback);
public sealed record Grade2Lesson40Feedback(string Section,bool? Correct,bool TeacherReviewNeeded,string Feedback);
