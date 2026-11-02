namespace Edulytics.Web.ViewModels.LessonContent;
public sealed class Grade2Lesson35Answers
{
    public string? WhatIsMeasurement{get;set;}
    public string? SourceTenCmStraws{get;set;}
    public string? SourceFiveCmStraws{get;set;}
    public string? SourceCubes{get;set;}
    public string? ActualStrawComparison{get;set;}
    public string? ActualCubeMeasurement{get;set;}
    public string? IndependentTenCm{get;set;}
    public string? IndependentFiveCm{get;set;}
    public string? IndependentCubes{get;set;}
    public string? UnitReflection{get;set;}
}
public sealed record Grade2Lesson35Page(Guid AdoptionId,Guid LessonId,string Title,bool Submitted,Grade2Lesson35Answers Answers,IReadOnlyList<Grade2Lesson35Feedback> Feedback);
public sealed record Grade2Lesson35Feedback(string Section,bool? Correct,bool TeacherReviewNeeded,string Message);
