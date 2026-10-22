namespace Edulytics.Web.ViewModels.LessonContent;
public sealed class Grade2Lesson24Answers
{
    public string? True1{get;set;}="True";public string? True2{get;set;}="True";
    public string? True3{get;set;}="False";public string? TrueFalseExplain{get;set;}
    public string? DiagramTens{get;set;}="7";public string? DiagramOnes{get;set;}="12";
    public string? DiagramAnswer{get;set;}public string? DiagramExplain{get;set;}
    public string? TargetStart{get;set;}="99";
    public string? TargetCard1{get;set;}="4";public string? TargetMode1{get;set;}="Ones";
    public string? TargetAfter1{get;set;}public string? TargetCard2{get;set;}="7";
    public string? TargetMode2{get;set;}="Tens";public string? TargetAfter2{get;set;}
    public string? TargetOriginalRealGameExplanation{get;set;}
    public string? Cool1{get;set;}public string? Cool2{get;set;}public string? Cool3{get;set;}
    public string? Reflection{get;set;}
}
public sealed record Grade2Lesson24Page(Guid AdoptionId,Guid LessonId,string LessonTitle,
    bool Submitted,Grade2Lesson24Answers Answers,IReadOnlyList<Grade2Lesson24Feedback> Feedback);
public sealed record Grade2Lesson24Feedback(string Section,bool? Correct,bool TeacherReviewNeeded,string Explanation);
