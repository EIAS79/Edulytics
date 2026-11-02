namespace Edulytics.Web.ViewModels.LessonContent;
public sealed class Grade2Lesson30Answers
{
    public string? MancalaNotice{get;set;}
    public string? Mlr6FirstRead{get;set;}
    public string? Mlr6SecondRead{get;set;}
    public string? Mlr6ThirdRead{get;set;}
    public string? OriginalSourceDiagramMatches{get;set;}
    public string? DiagramExplanation{get;set;}
    public string? OriginalCardPairMatches{get;set;}
    public string? OriginalChosenTwoCards{get;set;}
    public string? SourceSynthesisCardsLAndN{get;set;}
    public string? OptionalSourceSmaller{get;set;}
    public string? OptionalSourceTotal{get;set;}
    public string? OptionalSourceBoard{get;set;}
    public string? OptionalSourceSide{get;set;}
    public string? FirstPartySmaller{get;set;}
    public string? FirstPartyTotal{get;set;}
    public string? FirstPartyBoard{get;set;}
    public string? FirstPartySide{get;set;}
}
public sealed record Grade2Lesson30Page(Guid AdoptionId,Guid LessonId,string Title,bool Submitted,Grade2Lesson30Answers Answers,IReadOnlyList<Grade2Lesson30Feedback> Feedback);
public sealed record Grade2Lesson30Feedback(string Section,bool? Correct,bool TeacherReviewNeeded,string Feedback);
