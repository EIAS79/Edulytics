namespace Edulytics.Web.ViewModels.LessonContent;
public sealed class Grade2Lesson37Answers
{
    public string? TalkA{get;set;} ;public string? TalkB{get;set;} ;
    public string? TalkC{get;set;} ;public string? TalkD{get;set;}
    public string? TalkExplain{get;set;}
    public string? RulerTickZero{get;set;}="0";public string? RulerTickOne{get;set;}="1";
    public string? RulerTickTen{get;set;}="10";
    public string? RulerStart{get;set;}="3";public string? RulerEnd{get;set;}="18";
    public string? RulerLength{get;set;} ;
    public string? RealRulerObservation{get;set;} ;
    public string? ActualRectangleMeasures{get;set;} ;
    public string? ActualRectangleDifferences{get;set;} ;
    public string? FirstPartyLong{get;set;}="23";public string? FirstPartyShort{get;set;}="17";
    public string? FirstPartyGap{get;set;} ;public string? FirstPartyJoin{get;set;}
    public string? ExitLength{get;set;} ;public string? PartnerReasoning{get;set;}
}
public sealed record Grade2Lesson37Page(Guid AdoptionId,Guid LessonId,string Title,bool Submitted,Grade2Lesson37Answers Answers,IReadOnlyList<Grade2Lesson37Feedback> Feedback);
public sealed record Grade2Lesson37Feedback(string Section,bool? Correct,bool TeacherReviewNeeded,string Feedback);
