namespace Edulytics.Core.Mathematics.Curriculum;

/// <summary>
/// Edulytics-authored original Cool-down candidates for Grade 3, Unit 1,
/// Lessons 1 and 2. These are NOT quotations or reconstructions of the
/// publisher's educator-gated Cool-down prompts. The lesson goals and graph
/// interpretation/problem-solving skills are the alignment authority.
/// Neither candidate is wired into live student Practice or certified yet.
/// </summary>
public static class UsCcssG3OriginalCoolDownCandidates
{
    public const string FirstEdition = "Illustrative Mathematics K–5 (2021)";
    public const string Lesson1Code = "PED:US-CCSS-MATH:G3:U01:L01";
    public const string Lesson2Code = "PED:US-CCSS-MATH:G3:U01:L02";

    public sealed record GraphDatum(string Category, int Students);
    public sealed record Question(
        string Id,
        string Prompt,
        int? NumericAnswer,
        bool RequiresTeacherReview);

    // Fully first-party fictional dataset: one mark = one learner. It is
    // intentionally different from the publisher's travel-to-home dataset.
    public static IReadOnlyList<GraphDatum> Lesson1LibraryGraph { get; } =
        Array.AsReadOnly(new[]
        {
            new GraphDatum("Comics", 6),
            new GraphDatum("Animals", 4),
            new GraphDatum("Sports", 2),
            new GraphDatum("Space", 5)
        });

    // Fictional independent assessment data, NOT a replacement for the
    // authentic, variable class survey in source Lesson 2.
    public static IReadOnlyList<GraphDatum> Lesson2ClubGraph { get; } =
        Array.AsReadOnly(new[]
        {
            new GraphDatum("Games", 7),
            new GraphDatum("Puzzles", 4),
            new GraphDatum("Drawing", 3),
            new GraphDatum("Reading", 2)
        });

    public static IReadOnlyList<Question> Lesson1Questions { get; } =
        Array.AsReadOnly(new[]
        {
            new Question(
                "G3-U01-L01-EDU-CD-COUNT",
                "In the fictional Library Reading Corners picture graph, one symbol represents one learner. Comics 6, Animals 4, Sports 2, Space 5. How many learners chose Space?",
                5, false),
            new Question(
                "G3-U01-L01-EDU-CD-COMPARE",
                "How many more learners chose Comics than Sports? Use the same picture graph.",
                4, false),
            new Question(
                "G3-U01-L01-EDU-CD-EXPLAIN",
                "Write one different question that can be answered from these four categories and explain how the graph supports your answer.",
                null, true)
        });

    public static IReadOnlyList<Question> Lesson2Questions { get; } =
        Array.AsReadOnly(new[]
        {
            new Question(
                "G3-U01-L02-EDU-CD-ONE-STEP",
                "An invented example club survey has Games 7, Puzzles 4, Drawing 3, Reading 2. How many more chose Games than Reading?",
                5, false),
            new Question(
                "G3-U01-L02-EDU-CD-TWO-STEP",
                "Using the invented club survey, how many more chose Games or Puzzles together than Drawing or Reading together?",
                6, false),
            new Question(
                "G3-U01-L02-EDU-CD-DRAW-EXPLAIN",
                "Draw a correctly labelled unit-scale bar graph for the invented data and explain the two-step comparison using numbers or an equation.",
                null, true)
        });

    public static bool TryGradeNumeric(
        string lessonCode, string questionId, int learnerAnswer, out bool correct)
    {
        correct = false;
        IReadOnlyList<Question>? questions = lessonCode switch
        {
            Lesson1Code => Lesson1Questions,
            Lesson2Code => Lesson2Questions,
            _ => null
        };
        if (questions is null)
            return false;

        var item = questions.SingleOrDefault(x =>
            string.Equals(x.Id, questionId, StringComparison.Ordinal));
        if (item is null || item.RequiresTeacherReview ||
            item.NumericAnswer is not int expected)
            return false;

        correct = learnerAnswer == expected;
        return true;
    }

    public static bool IsTeacherReviewed(string lessonCode, string questionId) =>
        (lessonCode switch
        {
            Lesson1Code => Lesson1Questions,
            Lesson2Code => Lesson2Questions,
            _ => Array.Empty<Question>()
        }).Any(x =>
            string.Equals(x.Id, questionId, StringComparison.Ordinal) &&
            x.RequiresTeacherReview);
}
