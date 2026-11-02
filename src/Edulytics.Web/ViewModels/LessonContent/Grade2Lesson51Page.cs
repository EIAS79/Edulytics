namespace Edulytics.Web.ViewModels.LessonContent;
public sealed class Grade2Lesson51Answers
{
    public string? TalkA{get;set;}public string? TalkB{get;set;}public string? TalkC{get;set;}public string? TalkD{get;set;}
    public string? RealUnit{get;set;}
    public string? RealMeasuredObjects{get;set;}
    public string? RealPlotTitleAndTicks{get;set;}
    public string? PartnerFirstQuestion{get;set;}
    public string? PartnerSecondQuestion{get;set;}
    public string? PartnerResponses{get;set;}
    public string? RealChoiceCenter{get;set;}
    public string? PartnerPraise{get;set;}
    public string? FictionTotal{get;set;}
    public string? FictionAt10{get;set;}
    public string? FictionAt12{get;set;}
    public string? FictionMin{get;set;}
    public string? FictionMax{get;set;}
    public string? FictionAbove11{get;set;}
    public string? CoolA{get;set;}
    public string? CoolB{get;set;}
}
public sealed record Grade2Lesson51Page(Guid AdoptionId,Guid LessonId,string Title,bool Submitted,Grade2Lesson51Answers Answers,IReadOnlyList<Grade2Lesson51Feedback> Feedback);
public sealed record Grade2Lesson51Feedback(string Section,bool? Correct,bool TeacherReviewNeeded,string Feedback);
