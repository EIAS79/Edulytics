namespace Edulytics.Web.ViewModels.LessonContent;

public sealed class Grade2Lesson19Answers
{
    public string? WarmupReason {get;set;}
    public string? SnackSum {get;set;}
    public string? SnackDifference {get;set;}
    public string? SnackAdditionEquation {get;set;}
    public string? SnackDifferenceEquation {get;set;}
    public string? SnackRepresentation {get;set;}
    public string? CubeTotal {get;set;}
    public string? CubeDifference {get;set;}
    public string? CubeInverseEquation {get;set;}
    public string? CubeDrawingExplanation {get;set;}
    public string? CoolAdd {get;set;}
    public string? CoolSubtract {get;set;}
    public string? CoolCompare {get;set;}
    public string? PartnerReflection {get;set;}
}
public sealed record Grade2Lesson19Page(Guid AdoptionId,Guid LessonId,
    string LessonTitle,bool Submitted,Grade2Lesson19Answers Answers,
    IReadOnlyList<Grade2Lesson19Feedback> Feedback);
public sealed record Grade2Lesson19Feedback(string Activity,bool? Correct,
    bool TeacherReviewNeeded,string Explanation);
