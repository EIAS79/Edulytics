namespace Edulytics.Web.ViewModels.LessonContent;
public sealed class Grade2Lesson42Answers
{
    public string? WarmupRulers{get;set;}
    public string? InchTileSide{get;set;}
    public string? InchRulerZeroOne{get;set;}
    public string? FirstRealOneInchItem{get;set;}
    public string? SecondRealOneInchItem{get;set;}
    public string? MarkerRealInches{get;set;}
    public string? ColoredPencilRealInches{get;set;}
    public string? ElevenCubesRealInches{get;set;}
    public string? BookRealInches{get;set;}
    public string? ActualZeroEnd{get;set;}
    public string? RectangleEstimateReal{get;set;}
    public string? RectangleMeasuredReal{get;set;}
    public string? SquareEstimateReal{get;set;}
    public string? SquareMeasuredReal{get;set;}
    public string? TriangleEstimateReal{get;set;}
    public string? TriangleMeasuredReal{get;set;}
    public string? MLR8RealReflection{get;set;}
    public string? VirtualTiles{get;set;}
    public string? VirtualTicks{get;set;}
    public string? VirtualBelow{get;set;}
    public string? VirtualAbove{get;set;}
}
public sealed record Grade2Lesson42Page(Guid AdoptionId,Guid LessonId,string Title,bool Submitted,Grade2Lesson42Answers Answers,IReadOnlyList<Grade2Lesson42Feedback> Feedback);
public sealed record Grade2Lesson42Feedback(string Section,bool? Correct,bool TeacherReviewNeeded,string Feedback);
