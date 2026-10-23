using Edulytics.Core.Mathematics.Curriculum;
namespace Edulytics.Web.ViewModels.LessonContent;
/// <summary>Grade 3 scale-choice practice answers, not saved as official grades.</summary>
public sealed class Grade3Lesson6Answers
{
    public string? Warmup {get;set;}
    public string? PetCat {get;set;}
    public string? PetPython {get;set;}
    public string? PetTotal {get;set;}
    public string? PetsScaleExplanation {get;set;}
    public string? Triangle {get;set;}
    public string? Square {get;set;}
    public string? Trapezoid {get;set;}
    public string? Hexagon {get;set;}
    public string? PatternScale {get;set;}
    public string? PatternExplanation {get;set;}
    public string? Spring {get;set;}
    public string? SeasonTotal {get;set;}
    public string? SeasonDifference {get;set;}
    public string? SeasonScale {get;set;}
    public string? SeasonsExplanation {get;set;}
    public string? CoolMusic {get;set;}
    public string? CoolScience {get;set;}
    public string? CoolDifference {get;set;}
    public string? CoolReflection {get;set;}
}
public sealed record Grade3Lesson6Feedback(string Label,bool? Correct,bool HumanReview,string Message);
public sealed record Grade3Lesson6PracticePage(Guid AdoptionId,Guid LessonId,bool Submitted,
    Grade3Lesson6Answers Answers,
    IReadOnlyList<UsCcssG3Unit1Lesson6ChooseScale.CategoryCount>? EnteredPatternBlocks,
    int PatternScale,int SeasonScale,IReadOnlyList<Grade3Lesson6Feedback> Feedback);
