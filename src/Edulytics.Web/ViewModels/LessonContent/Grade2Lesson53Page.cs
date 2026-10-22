namespace Edulytics.Web.ViewModels.LessonContent;

public sealed class Grade2Lesson53Answers
{
    public string? MissingThree{get;set;}
    public string? MissingFour{get;set;}
    public string? MissingSix{get;set;}
    public string? MissingSeven{get;set;}
    public string? FirstPointSix{get;set;}
    public string? GreaterThanSix{get;set;}
    public string? SecondPointNine{get;set;}
    public string? LessThanNine{get;set;}
    public string? NewGapThreeThirteen{get;set;}
    public string? NewFromZeroThirteen{get;set;}
    public string? NewTotalTickMarks{get;set;}
    public string? RealChosenUnit{get;set;}
    public string? RealLineZeroToTwenty{get;set;}
    public string? RealPointsThreeThirteen{get;set;}
    public string? RealPartnerComparison{get;set;}
}
public sealed record Grade2Lesson53Page(Guid AdoptionId,Guid LessonId,string Title,bool Submitted,
    Grade2Lesson53Answers Answers,IReadOnlyList<Grade2Lesson53Feedback> Feedback);
public sealed record Grade2Lesson53Feedback(string Section,bool? Correct,bool TeacherReviewNeeded,string Feedback);
