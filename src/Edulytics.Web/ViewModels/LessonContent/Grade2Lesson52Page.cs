namespace Edulytics.Web.ViewModels.LessonContent;
public sealed class Grade2Lesson52Answers
{
    public string? TalkA{get;set;}public string? TalkB{get;set;}public string? TalkC{get;set;}public string? TalkD{get;set;}
    public string? OneInch{get;set;}public string? TwoInches{get;set;}
    public string? ElevenInches{get;set;}public string? ThirtySixInches{get;set;}
    public string? RealInchMarks{get;set;}public string? RealPairRoles{get;set;}
    public string? RealDeskHeight{get;set;}public string? ActualDeskLinePlot{get;set;}
    public string? RealRevision{get;set;}
    public string? RealPartnerHandsAndArms{get;set;}
    public string? RealTwoEquations{get;set;}public string? RealContextSentences{get;set;}
    public string? FictionDeskA{get;set;}public string? FictionDeskB{get;set;}
    public string? FictionDeskDifference{get;set;}public string? FictionRemaining{get;set;}
    public string? FictionForearmMinusHand{get;set;}
}
public sealed record Grade2Lesson52Page(Guid AdoptionId,Guid LessonId,string Title,bool Submitted,Grade2Lesson52Answers Answers,IReadOnlyList<Grade2Lesson52Feedback> Feedback);
public sealed record Grade2Lesson52Feedback(string Section,bool? Correct,bool TeacherReviewNeeded,string Feedback);
