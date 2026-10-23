namespace Edulytics.Web.ViewModels.LessonContent;
public sealed class Grade2Unit7Answers
{
    public string? SourceA{get;set;}public string? SourceB{get;set;}
    public string? SourceC{get;set;}public string? SourceD{get;set;}
    public string? OwnA{get;set;}public string? OwnB{get;set;}
    public string? OwnC{get;set;}public string? OwnD{get;set;}
    public string? ActualSourceShapeOrImage{get;set;}public string? StudentCreatedDrawingOrWords{get;set;}
    public string? RealPhysicalGameOrCoins{get;set;}public string? TeacherComments{get;set;}
}
public sealed record Grade2Unit7Page(Guid AdoptionId,Guid LessonId,string Title,int LessonNumber,bool Submitted,
    string AuthenticSourceRequirement,Grade2Unit7Answers Answers,
    IReadOnlyList<Grade2Unit7Feedback> Feedback);
public sealed record Grade2Unit7Feedback(string Section,bool? Correct,bool NeedsHumanReview,string Comment);
