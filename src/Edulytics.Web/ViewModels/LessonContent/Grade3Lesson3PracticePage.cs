namespace Edulytics.Web.ViewModels.LessonContent;

/// <summary>Unsaved learner practice; no official assessment outcome.</summary>
public sealed class Grade3Lesson3Answers
{
    public string? Warmup { get; set; }
    public string? DifferenceInKeys { get; set; }
    public string? OriginalQuestion { get; set; }
    public string? MathCommunity { get; set; }
    public string? CoolExplanation { get; set; }
    public string? SportsTotal { get; set; }
    public string? SportsBasketball { get; set; }
    public string? SportsCompare { get; set; }
    public string? FlowersRoses { get; set; }
    public string? FlowersTulips { get; set; }
    public string? FlowersDaisies { get; set; }
    public string? FlowersTotal { get; set; }
    public string? FlowersCompare { get; set; }
    public string? FlowersViolets { get; set; }
    public string? BooksJokes { get; set; }
    public string? BooksComics { get; set; }
    public string? BooksScience { get; set; }
    public string? BooksMystery { get; set; }
    public string? BooksTotal { get; set; }
    public string? SecondOriginalQuestion { get; set; }
    // The public 2021 books activity asks students to explain how they know
    // the counts, not just compute numbers or invent new questions.
    public string? BooksHowYouKnow { get; set; }
    // Original 2021 source Cool-down two skill types, separate from the
    // independently authored fictional library survey.
    public string? SourceCoolDownSparrows { get; set; }
    public string? SourceCoolDownBirdQuestion { get; set; }
    public string? CoolNature { get; set; }
    public string? CoolDifference { get; set; }
}
public sealed record Grade3Lesson3Feedback(string Name, bool? MachineCorrect, bool TeacherReview,
    string Message);
public sealed record Grade3Lesson3PracticePage(Guid AdoptionId, Guid LessonId,
    bool Submitted, Grade3Lesson3Answers Answers,
    IReadOnlyList<Grade3Lesson3Feedback> Feedback);
