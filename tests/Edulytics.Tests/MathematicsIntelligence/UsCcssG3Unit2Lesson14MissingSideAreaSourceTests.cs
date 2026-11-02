using System.Text.Json;
using System.Text.Json.Serialization;
using Edulytics.Core.Curriculum;
using Edulytics.Data.Seeding;
using Edulytics.Web.Curriculum.Grade3;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Original-source missing-side/area authoring contracts. Deferred Stage D; NOT RUN.</summary>
public sealed class UsCcssG3Unit2Lesson14MissingSideAreaSourceTests
{
    private const string Code = "PED:US-CCSS-MATH:G3:U02:L14";
    private static Grade3CompanionLesson Lesson()
    {
        Assert.True(Grade3SourceCompanionCatalogue.TryGet(Code, out var lesson));
        return Assert.IsType<Grade3CompanionLesson>(lesson);
    }
    private static Grade3OriginalQuestion Task(string suffix) =>
        Lesson().SourceSubtasks.Single(q => q.Id.EndsWith(suffix, StringComparison.Ordinal));

    [Theory]
    [InlineData("WU-HORIZONTAL", "6 cm", true)]
    [InlineData("WU-HORIZONTAL", "6 centimetres", true)]
    [InlineData("WU-HORIZONTAL", "6 m", false)]
    [InlineData("WU-HORIZONTAL", "6 cm²", false)]
    [InlineData("WU-VERTICAL", "7 cm", true)]
    [InlineData("WU-VERTICAL", "6 cm", false)]
    [InlineData("A1-LENGTH", "6 m", true)]
    [InlineData("A1-LENGTH", "6 meters", true)]
    [InlineData("A1-LENGTH", "6.0 m", true)]
    [InlineData("A1-LENGTH", "12/2 m", true)]
    [InlineData("A2-B-WIDTH", "4/2 ft", true)]
    [InlineData("A1-LENGTH", "5 m", false)]
    [InlineData("A1-LENGTH", "6 m²", false)]
    [InlineData("A2-B-WIDTH", "2 ft", true)]
    [InlineData("A2-B-WIDTH", "2 feet", true)]
    [InlineData("A2-B-WIDTH", "4 ft", false)]
    [InlineData("A2-B-HEIGHT", "4 foot", true)]
    [InlineData("A2-B-HEIGHT", "2 ft", false)]
    public void EachMissingSideRequiresItsOwnValueAndLinearUnit(string suffix, string answer, bool correct)
    {
        var result = Grade3SourceCompanionCatalogue.Grade(Task(suffix), answer);
        Assert.Equal(correct, result.NumericCorrect);
        Assert.True(result.TeacherReview);
    }

    [Theory]
    [InlineData("6")]
    [InlineData("0 m")]
    [InlineData("-6 m")]
    [InlineData("6.1 m")]
    [InlineData("6/0 m")]
    [InlineData("6e0 m")]
    [InlineData("NaN m")]
    [InlineData("6 m trailing")]
    [InlineData("99999999999999999999 m")]
    [InlineData("")]
    public void MissingSideContractRejectsWrongValuesAndMalformedNotations(string answer)
    {
        var result = Grade3SourceCompanionCatalogue.Grade(Task("A1-LENGTH"), answer);
        Assert.False(result.NumericCorrect);
        Assert.True(result.TeacherReview);
    }

    [Fact]
    public void InvalidExpectedLengthConfigurationNeverAwardsCredit()
    {
        foreach (var expected in new string?[] { null, "6", "6 m²", "0 m" })
        {
            var result = Grade3SourceCompanionCatalogue.Grade(
                new() { AnswerType = "length-with-unit", Expected = expected }, "6 m");
            Assert.Null(result.NumericCorrect);
            Assert.True(result.TeacherReview);
        }
    }

    [Fact]
    public void SourceLengthsAndIndependentAreaDecompositionsAgree()
    {
        Assert.Equal(6, 10 - 4);
        Assert.Equal(7, 9 - 2);
        Assert.Equal(6, 12 - 6);
        Assert.Equal(2, 7 - 3 - 2);
        Assert.Equal(8 * 6 + 4 * 6, 4 * 6 + 4 * 12);
        Assert.Equal(72, 8 * 6 + 4 * 6);
        Assert.Equal(7 * 2 + 2 * 4, 3 * 2 + 2 * 6 + 2 * 2);
        Assert.Equal(22, 7 * 2 + 2 * 4);
        Assert.True(Grade3SourceCompanionCatalogue.Grade(Lesson().SourceBasedCheck!, "72 m²").NumericCorrect);
        Assert.True(Grade3SourceCompanionCatalogue.Grade(Task("A2-B-AREA"), "22 ft²").NumericCorrect);
        foreach (var wrong in new[] { "72", "72 m", "72 ft²", "60 m²" })
            Assert.False(Grade3SourceCompanionCatalogue.Grade(Lesson().SourceBasedCheck!, wrong).NumericCorrect);
    }

    [Fact]
    public void CritiqueDiscussionAndSectionSummaryStayTeacherReviewed()
    {
        var lesson = Lesson();
        foreach (var q in lesson.SourceSubtasks.Where(q => q.AnswerType == "teacher"))
        foreach (var answer in new[] { "6 m", "72 m²", "22 ft²", "" })
        {
            var result = Grade3SourceCompanionCatalogue.Grade(q, answer);
            Assert.Null(result.NumericCorrect);
            Assert.True(result.TeacherReview);
        }
        Assert.Contains("identical SVG bytes", lesson.TeacherReviewNote);
        Assert.Contains("same/different-method", lesson.TeacherReviewNote);
        Assert.True(lesson.OriginalStudentTaskPdfRead);
        Assert.True(lesson.ReadyForStageD);
    }

    [Fact]
    public void EquivalentCoolDownMeasuresAreaAfterRecoveringDifferentMissingSides()
    {
        var q = Lesson().OriginalEdulyticsCoolDown!;
        Assert.Equal(6, 9 - 3);
        Assert.Equal(5, 7 - 2);
        Assert.Equal(9 * 2 + 3 * 5, 6 * 2 + 3 * 7);
        Assert.True(Grade3SourceCompanionCatalogue.Grade(q, "33 cm²").NumericCorrect);
        foreach (var wrong in new[] { "33", "33 cm", "33 m²", "6 cm", "48 cm²" })
            Assert.False(Grade3SourceCompanionCatalogue.Grade(q, wrong).NumericCorrect);
        Assert.True(Grade3SourceCompanionCatalogue.Grade(q, "33 cm²").TeacherReview);
    }

    [Fact]
    public void GuardedCanonicalMaterializationTeachesMissingSpansAndBothAreaMethods()
    {
        var assembly = typeof(CanonicalLessonContentPackDocument).Assembly;
        var resource = assembly.GetManifestResourceNames().Single(n => n.EndsWith(
            "us-ccss-math-g3-phase29-v1.lesson-content-pack.json", StringComparison.Ordinal));
        using var stream = assembly.GetManifestResourceStream(resource)!;
        var options = new JsonSerializerOptions(JsonSerializerDefaults.Web);
        options.Converters.Add(new JsonStringEnumConverter());
        var document = JsonSerializer.Deserialize<CanonicalLessonContentPackDocument>(stream, options)!;
        var lesson = document.Lessons.Single(l => l.LessonCode == Code);
        CommonCoreG3SourceTeachingCorrections.Apply(document);
        var en = lesson.Translations.Single(t => t.CultureCode == "en");
        Assert.Contains("12 − 6 = 6 m", en.Explanation);
        Assert.Contains("72 m²", en.WorkedExamples);
        Assert.Contains("22 ft²", en.WorkedExamples);
        Assert.Contains("three-rectangle", en.Explanation);
        Assert.Contains("I heard you say", en.Explanation);
        Assert.Contains("MLR8", en.Explanation);
        Assert.Contains("33 cm²", en.WorkedExamples);
        Assert.DoesNotContain("classify", en.StepByStepSolutions, StringComparison.OrdinalIgnoreCase);
        lesson.SourceEdition = "2nd Edition";
        Assert.False(CommonCoreG3SourceTeachingCorrections.IsTarget(document, lesson));
        Assert.Throws<InvalidOperationException>(() => CommonCoreG3SourceTeachingCorrections.Apply(document));
    }
}
