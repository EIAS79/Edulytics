using System.Text.Json;
using System.Text.Json.Serialization;
using Edulytics.Core.Curriculum;
using Edulytics.Data.Seeding;
using Edulytics.Web.Curriculum.Grade3;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Independent source/math authoring contracts. Deferred Stage D; NOT RUN.</summary>
public sealed class UsCcssG3Unit2Lesson12AdditiveAreaSourceTests
{
    private const string Code = "PED:US-CCSS-MATH:G3:U02:L12";
    private static Grade3CompanionLesson Lesson()
    {
        Assert.True(Grade3SourceCompanionCatalogue.TryGet(Code, out var lesson));
        return Assert.IsType<Grade3CompanionLesson>(lesson);
    }

    [Theory]
    [InlineData("42 ft²", true)]
    [InlineData("42 square feet", true)]
    [InlineData("42 ft2", true)]
    [InlineData("35 ft²", false)]
    [InlineData("42", false)]
    [InlineData("42 ft", false)]
    [InlineData("42 cm²", false)]
    [InlineData("42 square units", false)]
    [InlineData("30 ft²", false)]
    [InlineData("12 ft²", false)]
    [InlineData("42 ft² trailing", false)]
    public void OriginalSevenColumnSixRowGardenKeepsItsCorrectAreaAndUnits(string answer, bool correct)
    {
        var result = Grade3SourceCompanionCatalogue.Grade(Lesson().SourceBasedCheck!, answer);
        Assert.Equal(correct, result.NumericCorrect);
        Assert.True(result.TeacherReview);
    }

    [Theory]
    [InlineData("70 square units", true)]
    [InlineData("70 SQUARE UNITS", true)]
    [InlineData("70", false)]
    [InlineData("70 ft²", false)]
    [InlineData("90 square units", false)]
    [InlineData("20 square units", false)]
    [InlineData("50 square units", false)]
    [InlineData("-70 square units", false)]
    [InlineData("7e1 square units", false)]
    [InlineData("70.0 square units", false)]
    [InlineData("70 square units trailing", false)]
    public void CompositeSourceFigureNeedsAllAndOnlyItsSquaresAndTheStatedUnit(string answer, bool correct)
    {
        var q = Lesson().SourceSubtasks.Single(q => q.Id.EndsWith("A2-AREA"));
        var result = Grade3SourceCompanionCatalogue.Grade(q, answer);
        Assert.Equal(correct, result.NumericCorrect);
        Assert.True(result.TeacherReview);
    }

    [Fact]
    public void IndependentCellEnumerationAndBothSourcePartitionsGiveSeventy()
    {
        var cells = (from row in Enumerable.Range(0, 9)
                     from column in Enumerable.Range(0, 10)
                     where row >= 4 || column < 5
                     select (row, column)).ToHashSet();
        Assert.Equal(70, cells.Count);
        Assert.Equal(4 * 5 + 5 * 10, cells.Count);
        Assert.Equal(9 * 5 + 5 * 5, cells.Count);
        Assert.DoesNotContain((0, 9), cells);
        var garden = (from row in Enumerable.Range(0, 6)
                      from column in Enumerable.Range(0, 7)
                      select (row, column)).ToArray();
        Assert.Equal(42, garden.Length);
        Assert.Equal(30, garden.Count(c => c.column < 5));
        Assert.Equal(12, garden.Count(c => c.column >= 5));
    }

    [Fact]
    public void NumberTalkAndStudentAuthorshipAreDifferentContracts()
    {
        var lesson = Lesson();
        var answers = new[] { 15, 35, 65, 57 };
        for (var i = 0; i < answers.Length; i++)
        {
            var q = lesson.SourceSubtasks.Single(q => q.Id.EndsWith($"WU-{i + 1}"));
            Assert.True(Grade3SourceCompanionCatalogue.Grade(q, answers[i].ToString()).NumericCorrect);
            Assert.False(Grade3SourceCompanionCatalogue.Grade(q, (answers[i] + 1).ToString()).NumericCorrect);
        }
        foreach (var q in lesson.SourceSubtasks.Where(q => q.AnswerType == "teacher"))
        foreach (var answer in new[] { "42 ft²", "70 square units", "38 square units", "" })
        {
            var result = Grade3SourceCompanionCatalogue.Grade(q, answer);
            Assert.Null(result.NumericCorrect);
            Assert.True(result.TeacherReview);
        }
        Assert.Contains("own garden", lesson.TeacherReviewNote);
        Assert.Contains("diagonal", lesson.TeacherReviewNote);
        Assert.True(lesson.OriginalStudentTaskPdfRead);
        Assert.True(lesson.ReadyForStageD);
    }

    [Fact]
    public void IndependentCoolDownUsesADifferentFigureAndTeacherReviewedExpression()
    {
        var q = Lesson().OriginalEdulyticsCoolDown!;
        // Explicit source-independent new figure, not a repetition of 42 or 70.
        Assert.Equal(2 * 3 + 4 * 8, 6 * 3 + 4 * 5);
        Assert.True(Grade3SourceCompanionCatalogue.Grade(q, "38 square units").NumericCorrect);
        foreach (var answer in new[] { "42 square units", "70 square units", "38", "38 ft²" })
            Assert.False(Grade3SourceCompanionCatalogue.Grade(q, answer).NumericCorrect);
        Assert.True(Grade3SourceCompanionCatalogue.Grade(q, "38 square units").TeacherReview);
    }

    [Fact]
    public void GuardedCanonicalMaterializationTeachesAdditiveAreaInsteadOfShapeClassification()
    {
        var assembly = typeof(CanonicalLessonContentPackDocument).Assembly;
        var resource = assembly.GetManifestResourceNames().Single(n => n.EndsWith(
            "us-ccss-math-g3-phase29-v1.lesson-content-pack.json", StringComparison.Ordinal));
        using var stream = assembly.GetManifestResourceStream(resource)!;
        var options = new JsonSerializerOptions(JsonSerializerDefaults.Web);
        options.Converters.Add(new JsonStringEnumConverter());
        var document = JsonSerializer.Deserialize<CanonicalLessonContentPackDocument>(stream, options)!;
        var lesson = document.Lessons.Single(l => l.LessonCode == Code);
        Assert.True(CommonCoreG3SourceTeachingCorrections.IsTarget(document, lesson));
        CommonCoreG3SourceTeachingCorrections.Apply(document);
        var en = lesson.Translations.Single(t => t.CultureCode == "en");
        Assert.Contains("42 ft²", en.WorkedExamples);
        Assert.Contains("70 square units", en.WorkedExamples);
        Assert.Contains("(4 × 5) + (5 × 10)", en.StepByStepSolutions);
        Assert.Contains("(9 × 5) + (5 × 5)", en.StepByStepSolutions);
        Assert.Contains("own garden", en.Explanation);
        Assert.Contains("diagonally", en.Explanation);
        Assert.Contains("Parentheses", en.Explanation);
        Assert.DoesNotContain("classify", en.StepByStepSolutions, StringComparison.OrdinalIgnoreCase);
        lesson.CanonicalBodySha256 = new string('0', 64);
        Assert.False(CommonCoreG3SourceTeachingCorrections.IsTarget(document, lesson));
        Assert.Throws<InvalidOperationException>(() => CommonCoreG3SourceTeachingCorrections.Apply(document));
    }
}
