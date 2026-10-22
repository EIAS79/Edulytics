namespace Edulytics.Web.ViewModels.LessonContent;
public sealed class Grade2Lesson47Answers
{
    public string? TalkA{get;set;}public string? TalkB{get;set;}
    public string? TalkC{get;set;}public string? TalkD{get;set;}
    public string? OriginalMentalReasoning{get;set;}
    public string? MeasurementUnit{get;set;}="cm";
    public string? MeasurementTarget{get;set;}="12";
    public string? MeasurementActual{get;set;}="15";
    public string? MeasurementScore{get;set;}
    public string? RealPartnerRounds{get;set;}
    public string? PuzzleAddUnknown{get;set;}
    public string? PuzzleSubtractUnknown{get;set;}
    public string? ActualPrintedPuzzle{get;set;}
    public string? ChoiceCenter{get;set;}
    public string? ExitCm{get;set;}public string? ExitInch{get;set;}
    public string? ExitAdd{get;set;}
}
public sealed record Grade2Lesson47Page(Guid AdoptionId,Guid LessonId,string Title,
    bool Submitted,Grade2Lesson47Answers Answers,IReadOnlyList<Grade2Lesson47Feedback> Feedback);
public sealed record Grade2Lesson47Feedback(string Section,bool? Correct,bool TeacherReviewNeeded,string Message);
