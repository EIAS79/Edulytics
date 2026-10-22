namespace Edulytics.Web.ViewModels.LessonContent;
public sealed class Grade2Lesson43Answers
{
    public string? WarmupInitialLow{get;set;}
    public string? WarmupAboutRight{get;set;}
    public string? WarmupHigh{get;set;}
    public string? WarmupRevisedAfterBoy48{get;set;}
    public string? TapeAFeet{get;set;}
    public string? TapeBFeet{get;set;}
    public string? TapeCFeet{get;set;}
    public string? TapeDFeet{get;set;}
    public string? TapeDExtraInches{get;set;}
    public string? TapeAInches{get;set;}
    public string? TapeBInches{get;set;}
    public string? TapeCInches{get;set;}
    public string? TapeDInches{get;set;}
    public string? RealToolAndMeasurement{get;set;}
    public string? RealEstimateBeforeMeasuring{get;set;}
    public string? RealInchOrFeetChoice{get;set;}
    public string? RealPartnerExplanation{get;set;}
    public string? FirstPartyOneSpan{get;set;}
    public string? FirstPartyThreeSpans{get;set;}
    public string? FirstPartyMixed{get;set;}
}
public sealed record Grade2Lesson43Page(Guid AdoptionId,Guid LessonId,string Title,bool Submitted,Grade2Lesson43Answers Answers,IReadOnlyList<Grade2Lesson43Feedback> Feedback);
public sealed record Grade2Lesson43Feedback(string Section,bool? Correct,bool TeacherReviewNeeded,string Feedback);
