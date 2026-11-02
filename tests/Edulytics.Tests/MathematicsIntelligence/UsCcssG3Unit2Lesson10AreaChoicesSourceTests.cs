using System.Text.Json;
using System.Text.Json.Serialization;
using Edulytics.Core.Curriculum;
using Edulytics.Data.Seeding;
using Edulytics.Web.Curriculum.Grade3;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Source-grounded A–C contracts, authored for deferred Stage D. NOT RUN.</summary>
public sealed class UsCcssG3Unit2Lesson10AreaChoicesSourceTests
{
    private const string Code = "PED:US-CCSS-MATH:G3:U02:L10";

    private static Grade3CompanionLesson Lesson()
    {
        Assert.True(Grade3SourceCompanionCatalogue.TryGet(Code, out var lesson));
        return Assert.IsType<Grade3CompanionLesson>(lesson);
    }

    private static Grade3OriginalQuestion Task(string suffix) =>
        Lesson().SourceSubtasks.Single(q => q.Id == "G3-U02-L10-" + suffix);

    [Theory]
    [InlineData("6,0,0", true)]
    [InlineData("0,2,0", true)]
    [InlineData("0,0,1", true)]
    [InlineData("3,1,0", true)]
    [InlineData(" 6 , 0 , 0 ", true)]
    [InlineData("5,0,0", false)]
    [InlineData("0,1,0", false)]
    [InlineData("0,0,0", false)]
    [InlineData("2", false)]
    [InlineData("6,0", false)]
    [InlineData("6,0,0,1", false)]
    [InlineData("-1,2,0", false)]
    [InlineData("0.5,2,0", false)]
    [InlineData("1e2,0,0", false)]
    [InlineData("1000,0,0", false)]
    [InlineData("6,0,0 trailing", false)]
    [InlineData("", false)]
    public void PaintChoiceChecksCoverageWithoutImposingOnePurchase(string answer, bool correct)
    {
        var q = Task("A1-CANS");
        Assert.Equal("coverage-combination", q.AnswerType);
        Assert.Null(q.Expected);
        var result = Grade3SourceCompanionCatalogue.Grade(q, answer);
        Assert.Equal(correct, result.NumericCorrect);
        Assert.True(result.TeacherReview);
    }

    [Fact]
    public void BoundedPaintPurchasesMatchAnIndependentCoverageOracle()
    {
        var q = Task("A1-CANS");
        Assert.Equal(18, q.CoverageConstraint!.RequiredArea);
        Assert.Equal(new[] { 3, 10, 40 }, q.CoverageConstraint.ContainerAreas);
        for (var small = 0; small <= 7; small++)
        for (var medium = 0; medium <= 3; medium++)
        for (var large = 0; large <= 2; large++)
        {
            // Explicit source constants, independently of q's grading rule.
            var sufficient = 3 * small + 10 * medium + 40 * large >= 18;
            var result = Grade3SourceCompanionCatalogue.Grade(q, $"{small},{medium},{large}");
            Assert.Equal(sufficient, result.NumericCorrect);
            Assert.True(result.TeacherReview);
        }
    }

    [Theory]
    [InlineData("7 ft", true)]
    [InlineData("8 feet", true)]
    [InlineData("9 ft", true)]
    [InlineData("10 ft", true)]
    [InlineData("20/3 ft", true)]
    [InlineData("40/6 ft", true)]
    [InlineData("6 2/3 ft", true)]
    [InlineData("7.5 ft", true)]
    [InlineData("6.666667 ft", true)]
    [InlineData("6.666666 ft", false)]
    [InlineData("10.000001 ft", false)]
    [InlineData("6 ft", false)]
    [InlineData("11 ft", false)]
    [InlineData("7 m", false)]
    [InlineData("7 ft²", false)]
    [InlineData("7", false)]
    [InlineData("21 ft²", false)]
    [InlineData("0 ft", false)]
    [InlineData("-7 ft", false)]
    [InlineData("7/0 ft", false)]
    [InlineData("NaN ft", false)]
    [InlineData("7e0 ft", false)]
    [InlineData("7 ft trailing", false)]
    [InlineData("99999999999999999999 ft", false)]
    public void GardenAcceptsMultipleExactLengthsAndBothInclusiveBoundaries(string answer, bool correct)
    {
        var q = Task("A2-LENGTHS");
        Assert.Equal("constrained-side-length", q.AnswerType);
        Assert.Null(q.Expected);
        Assert.Equal(3, q.RectangleConstraint!.FixedSide);
        Assert.Equal(20, q.RectangleConstraint.MinimumArea);
        Assert.Equal(30, q.RectangleConstraint.MaximumArea);
        var result = Grade3SourceCompanionCatalogue.Grade(q, answer);
        Assert.Equal(correct, result.NumericCorrect);
        Assert.True(result.TeacherReview);
    }

    [Theory]
    [InlineData("18 m²", true)]
    [InlineData("18 m2", true)]
    [InlineData("18 square metres", true)]
    [InlineData("18", false)]
    [InlineData("18 m", false)]
    [InlineData("18 ft²", false)]
    [InlineData("9 m²", false)]
    public void WallAreaRequiresBothValueAndSquareMeterUnit(string answer, bool correct)
    {
        var result = Grade3SourceCompanionCatalogue.Grade(Task("A1-WALL-AREA"), answer);
        Assert.Equal(correct, result.NumericCorrect);
        Assert.True(result.TeacherReview);
    }

    [Fact]
    public void NumberTalkAndTeacherOnlyWorkStayDistinct()
    {
        var lesson = Lesson();
        Assert.Equal(14, lesson.SourceSubtasks.Count);
        var values = new[] { "10", "12", "30", "36" };
        for (var i = 0; i < values.Length; i++)
        {
            var q = Task($"WU-{i + 1:00}");
            Assert.True(Grade3SourceCompanionCatalogue.Grade(q, values[i]).NumericCorrect);
            Assert.False(Grade3SourceCompanionCatalogue.Grade(q, "99").NumericCorrect);
        }
        foreach (var suffix in new[] { "WU-REASON", "A1-JUSTIFY", "A2-READS", "A2-POSTER", "A2-GALLERY", "SYNTH", "SUMMARY" })
        foreach (var answer in new[] { "18", "7 ft", "6,0,0", "" })
        {
            var result = Grade3SourceCompanionCatalogue.Grade(Task(suffix), answer);
            Assert.Null(result.NumericCorrect);
            Assert.True(result.TeacherReview);
        }
        Assert.True(lesson.OriginalStudentTaskPdfRead);
        Assert.True(lesson.SourceBasedCheckProvenance!.FigurePixelsInspected);
        Assert.True(lesson.ReadyForStageD); // A–C authoring readiness only.
    }

    [Fact]
    public void NewCoolDownUsesItsOwnConstraintsAndCannotReuseTheSourceAnswerSetBlindly()
    {
        var q = Lesson().OriginalEdulyticsCoolDown!;
        Assert.Equal(2, q.RectangleConstraint!.FixedSide);
        Assert.Equal(12, q.RectangleConstraint.MinimumArea);
        Assert.Equal(18, q.RectangleConstraint.MaximumArea);
        foreach (var answer in new[] { "6 ft", "7 ft", "8 ft", "9 ft", "6.5 ft" })
            Assert.True(Grade3SourceCompanionCatalogue.Grade(q, answer).NumericCorrect);
        foreach (var answer in new[] { "5 ft", "10 ft", "8", "8 m", "1/0 ft" })
            Assert.False(Grade3SourceCompanionCatalogue.Grade(q, answer).NumericCorrect);
        Assert.True(Grade3SourceCompanionCatalogue.Grade(q, "7 ft").TeacherReview);
    }

    [Fact]
    public void MissingOrInconsistentConstraintConfigurationDoesNotAwardNumericCredit()
    {
        foreach (var q in new[] {
            new Grade3OriginalQuestion { AnswerType = "coverage-combination" },
            new Grade3OriginalQuestion { AnswerType = "coverage-combination", CoverageConstraint = new() { RequiredArea = 18, ContainerAreas = [3, 0, 40] } },
            new Grade3OriginalQuestion { AnswerType = "constrained-side-length" },
            new Grade3OriginalQuestion { AnswerType = "constrained-side-length", RectangleConstraint = new() { FixedSide = 3, MinimumArea = 30, MaximumArea = 20, LinearUnit = "ft" } }
        })
        {
            var result = Grade3SourceCompanionCatalogue.Grade(q, "7 ft");
            Assert.Null(result.NumericCorrect);
            Assert.True(result.TeacherReview);
        }
    }

    [Fact]
    public void GuardedCanonicalMaterializationTeachesTheActualAreaProblems()
    {
        var assembly = typeof(CanonicalLessonContentPackDocument).Assembly;
        var resource = assembly.GetManifestResourceNames().Single(name =>
            name.EndsWith("us-ccss-math-g3-phase29-v1.lesson-content-pack.json", StringComparison.Ordinal));
        using var stream = assembly.GetManifestResourceStream(resource)!;
        var options = new JsonSerializerOptions(JsonSerializerDefaults.Web);
        options.Converters.Add(new JsonStringEnumConverter());
        var document = JsonSerializer.Deserialize<CanonicalLessonContentPackDocument>(stream, options)!;
        var lesson = document.Lessons.Single(l => l.LessonCode == Code);
        Assert.True(CommonCoreG3SourceTeachingCorrections.IsTarget(document, lesson));
        CommonCoreG3SourceTeachingCorrections.Apply(document);
        var en = lesson.Translations.Single(t => t.CultureCode == "en");
        Assert.Equal("Solve Area Problems", en.Title);
        Assert.Contains("18 m²", en.Explanation);
        Assert.Contains("no prices", en.Explanation, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("Three Reads", en.Explanation);
        Assert.Contains("7, 8, 9 and 10 ft", en.WorkedExamples);
        Assert.Contains("record three", en.QuickSummary);
        Assert.DoesNotContain("classify", en.StepByStepSolutions, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("30 ft²", en.KeyConceptsAndRules);
        lesson.CanonicalBodySha256 = new string('0', 64);
        Assert.False(CommonCoreG3SourceTeachingCorrections.IsTarget(document, lesson));
        Assert.Throws<InvalidOperationException>(() => CommonCoreG3SourceTeachingCorrections.Apply(document));
    }
}
