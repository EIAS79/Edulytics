namespace Edulytics.Core.Mathematics.Curriculum;

/// <summary>
/// Edulytics-authored scaled pictograph data for G3 U01 L03. These are
/// independent illustration/example values, NOT claims about the exact
/// numerals or artwork in the publisher's 2021 sports/flowers figures.
/// </summary>
public static class UsCcssG3Unit1Lesson3ScaledPictureGraph
{
    public const string LessonCode = "PED:US-CCSS-MATH:G3:U01:L03";
    public sealed record CategorySymbols(string Category, int Symbols);
    public sealed record Diagram(string Title, int StudentsPerSymbol, IReadOnlyList<CategorySymbols> Rows);
    public sealed record Question(string Id, string Prompt, int? Expected, bool TeacherReview);

    public static Diagram Sports { get; } = new("Favorite Sports", 5,
        Array.AsReadOnly(new[] {
            new CategorySymbols("Soccer", 3),
            new CategorySymbols("Basketball", 4),
            new CategorySymbols("Tennis", 2)
        }));
    public static Diagram Flowers { get; } = new("Flowers Seen", 2,
        Array.AsReadOnly(new[] {
            new CategorySymbols("Roses", 4),
            new CategorySymbols("Tulips", 3),
            new CategorySymbols("Daisies", 5)
        }));
    public static Diagram IndependentCoolDown { get; } = new("Library Book Choices", 5,
        Array.AsReadOnly(new[] {
            new CategorySymbols("Mystery", 2),
            new CategorySymbols("Nature", 4),
            new CategorySymbols("Sports", 3)
        }));
    public static IReadOnlyList<Question> Questions { get; } = Array.AsReadOnly(new[] {
        new Question("SPORTS-TOTAL", "How many students are represented in the Favorite Sports graph?",45,false),
        new Question("SPORTS-BASKETBALL", "How many chose basketball?",20,false),
        new Question("SPORTS-COMPARE", "How many more chose basketball than tennis?",10,false),
        new Question("FLOWERS-ROSES", "How many roses were counted?",8,false),
        new Question("FLOWERS-TOTAL", "How many flowers were counted in all?",24,false),
        new Question("FLOWERS-COMPARE", "How many more daisies than tulips?",4,false),
        new Question("COOL-NATURE", "Independent Cool-down: how many chose nature books?",20,false),
        new Question("COOL-DIFFERENCE", "Independent Cool-down: nature minus mystery?",10,false),
        new Question("COOL-WRITE", "Write and answer a different graph question. Explain how the key determines the answer.",null,true)
    });

    public static bool TryGradeNumeric(string? lessonCode, string? questionId,
        int learnerAnswer, out bool correct)
    {
        correct = false;
        if (!string.Equals(lessonCode, LessonCode, StringComparison.Ordinal))
            return false;
        var task = Questions.SingleOrDefault(x => string.Equals(x.Id, questionId,
            StringComparison.Ordinal));
        if (task is null || task.TeacherReview || task.Expected is not int expected)
            return false;
        correct = learnerAnswer == expected;
        return true;
    }
    public static int CategoryCount(Diagram d, string name) =>
        d.Rows.Single(x => x.Category == name).Symbols * d.StudentsPerSymbol;

    public static int Total(Diagram d) => d.Rows.Sum(x => x.Symbols) * d.StudentsPerSymbol;

    public static bool RequiresTeacherReview(string? task) =>
        task is "compare-two-diagram-keys" or "invent-scaled-picture-question"
            or "partner-reasoning" or "interpret-source-drawing"
            or "original-cool-down-explanation" or "class-math-community";
}
