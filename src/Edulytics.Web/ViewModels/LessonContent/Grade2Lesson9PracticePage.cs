namespace Edulytics.Web.ViewModels.LessonContent;

public sealed class Grade2Lesson9Answers
{
    public string? Summer {get;set;}
    public string? SpringAndFall {get;set;}
    public string? PairOfSeasonsMakingTen {get;set;}
    public string? AllSeasonVotes {get;set;}
    public string? ClubFootball {get;set;}
    public string? ClubTotal {get;set;}
    public string? ClubFootballMinusArt {get;set;}
    public string? FirstTrueFieldTripStatement {get;set;}
    public string? SecondTrueFieldTripStatement {get;set;}
    public string? GraphComparisonExplanation {get;set;}
}
public sealed record Grade2Lesson9PracticePage(
    Guid AdoptionId,Guid LessonId,string LessonTitle,bool Submitted,
    Grade2Lesson9Answers Answers,IReadOnlyList<Grade2Lesson9Feedback> Feedback);
public sealed record Grade2Lesson9Feedback(
    string Task,bool? Correct,bool TeacherReviewNeeded,string Feedback);
