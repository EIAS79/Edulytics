namespace Edulytics.Web.ViewModels.LessonContent;
public sealed class Grade2Lesson47Answers
{
    public string? TalkA{get;set;}
    public string? TalkB{get;set;}
    public string? TalkC{get;set;}
    public string? TalkD{get;set;}
    public string? MakeTenExplanation{get;set;}
    public string? Unit{get;set;}="cm";
    public string? TargetLength{get;set;}="7";
    public string? ActualLength{get;set;}="9";
    public string? AbsoluteError{get;set;}
    public string? SourceRealDrawnLine{get;set;}
    public string? SourceEightRounds{get;set;}
    public string? PuzzleSum{get;set;}
    public string? PuzzleUnknownAddend{get;set;}
    public string? PuzzleDifference{get;set;}
    public string? PuzzleUnknownSubtrahend{get;set;}
    public string? OriginalPuzzleCardExplanation{get;set;}
    public string? ExitA{get;set;}
    public string? ExitB{get;set;}
    public string? ExitC{get;set;}
}
public sealed record Grade2Lesson47Page(Guid AdoptionId,Guid LessonId,string Title,
    bool Submitted,Grade2Lesson47Answers Answers,IReadOnlyList<Grade2Lesson47Feedback> Feedback);
public sealed record Grade2Lesson47Feedback(string Section,bool? Correct,bool TeacherReviewNeeded,string Explanation);
