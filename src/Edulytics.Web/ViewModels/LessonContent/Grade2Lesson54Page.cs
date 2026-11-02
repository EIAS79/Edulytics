namespace Edulytics.Web.ViewModels.LessonContent;
public sealed class Grade2Lesson54Answers
{
    public string? FiveFirst{get;set;}public string? FiveNext{get;set;}
    public string? FiveFifty{get;set;}public string? FiveLast{get;set;}
    public string? MissingAfterZero{get;set;}public string? MissingFive{get;set;}
    public string? MissingTen{get;set;}public string? FullTickCount{get;set;}
    public string? RealYarnZeroAndSpacing{get;set;}public string? RealRandomCardPlacements{get;set;}
    public string? RealCardRevisions{get;set;}public string? JadaActualImageReview{get;set;}
    public string? AndreActualImageReview{get;set;}public string? ElenaActualImageReview{get;set;}
    public string? RealPeerDiscussion{get;set;}
    public string? FictionCardEighteen{get;set;}
    public string? FictionDistanceSix{get;set;}public string? FictionFiveCountTotal{get;set;}
}
public sealed record Grade2Lesson54Page(Guid AdoptionId,Guid LessonId,string Title,bool Submitted,
    Grade2Lesson54Answers Answers,IReadOnlyList<Grade2Lesson54Feedback> Feedback);
public sealed record Grade2Lesson54Feedback(string Section,bool? Correct,bool TeacherReviewNeeded,string Feedback);
