namespace Edulytics.Web.ViewModels.LessonContent;
public sealed class Grade2Lesson18Answers
{
    public string? WarmUp {get;set;}
    public string? SurveyQuestion {get;set;}
    public string? GraphTitle {get;set;}
    public string? GraphType {get;set;}="Bar";
    public string? NumberCategories {get;set;}="3";
    public List<string?> Categories {get;set;}=["","","",""];
    public List<string?> Counts {get;set;}=["","","",""];
    public string? PictureKey {get;set;}="1";
    public string? AxisStart {get;set;}="0";
    public string? AxisStep {get;set;}="1";
    public string? CategoryOne {get;set;}="1";
    public string? CategoryTwo {get;set;}="2";
    public string? PartnerQuestionCombine {get;set;}
    public string? PartnerQuestionCompare {get;set;}
    public string? CombineAnswer {get;set;}
    public string? CombineEquation {get;set;}
    public string? CompareAnswer {get;set;}
    public string? CompareEquation {get;set;}
    public string? TapeDifference {get;set;}
    public string? TapeEquation {get;set;}
    public string? TapeDrawingExplanation {get;set;}
    public string? CoolTotal {get;set;}
    public string? CoolCombine {get;set;}
    public string? CoolCompare {get;set;}
    public string? CoolTape {get;set;}
    public string? GraphVersusTapeReflection {get;set;}
}
public sealed record Grade2Lesson18Page(Guid AdoptionId,Guid LessonId,
    string LessonTitle,bool Submitted,Grade2Lesson18Answers Answers,
    IReadOnlyList<Grade2Lesson18Feedback> Feedback,
    Edulytics.Core.Mathematics.Practice.UsCcssG2Unit1EighteenthLessonClassSurveys.Survey? EnteredSurvey);
public sealed record Grade2Lesson18Feedback(string Activity,bool? Correct,
    bool TeacherReviewNeeded,string Message);
