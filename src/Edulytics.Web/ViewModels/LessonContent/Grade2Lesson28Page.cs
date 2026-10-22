namespace Edulytics.Web.ViewModels.LessonContent;
public sealed class Grade2Lesson28Answers
{
    public string? SourceWarmup{get;set;}
    public string? WarmupTens{get;set;}
    public string? WarmupOnes{get;set;}
    public string? WhatNoticed{get;set;}
    public string? SourceDiceCurrent{get;set;}="73";
    public string? SourceDiceBorrowChoice{get;set;}="24";
    public string? SourceDiceBorrowAnswer{get;set;}
    public string? SourceDiceSimpleChoice{get;set;}="52";
    public string? SourceDiceSimpleAnswer{get;set;}
    public string? ActualDiceAndRounds{get;set;}
    public string? ActualCenterChosen{get;set;}
    public string? ActualPartnerExplanation{get;set;}
    public string? IndependentA{get;set;}
    public string? IndependentB{get;set;}
    public string? IndependentC{get;set;}
}
public sealed record Grade2Lesson28Page(Guid AdoptionId,Guid LessonId,string Title,bool Submitted,Grade2Lesson28Answers Answers,IReadOnlyList<Grade2Lesson28Feedback> Feedback);
public sealed record Grade2Lesson28Feedback(string Section,bool? Correct,bool TeacherReviewNeeded,string Feedback);
