using System.Text.Json;
using System.Text.Json.Serialization;
using Edulytics.Core.Curriculum;
using Edulytics.Data.Seeding;
using Edulytics.Web.Curriculum.Grade3;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Authored source-specific contracts for deferred Stage D. NOT RUN.</summary>
public sealed class UsCcssG3Unit2Lesson13UngridAreaSourceTests
{
    private const string Code = "PED:US-CCSS-MATH:G3:U02:L13";
    private static Grade3CompanionLesson Lesson()
    {
        Assert.True(Grade3SourceCompanionCatalogue.TryGet(Code, out var lesson));
        return Assert.IsType<Grade3CompanionLesson>(lesson);
    }
    private static Grade3OriginalQuestion Task(string suffix) =>
        Lesson().SourceSubtasks.Single(q => q.Id.EndsWith(suffix, StringComparison.Ordinal));

    [Fact]
    public void ThreeSourceFiguresHaveDifferentIndependentAreaOracles()
    {
        var partialFigure = 0;
        for (var row = 0; row < 8; row++)
        for (var column = 0; column < 7; column++)
            if (row < 5 || column >= 4) partialFigure++;
        Assert.Equal(44, partialFigure);
        Assert.Equal(7 * 5 + 3 * 3, partialFigure);
        Assert.Equal(4 * 5 + 3 * 8, partialFigure);
        var tee = 0;
        for (var row = 0; row < 7; row++)
        for (var column = 0; column < 8; column++)
            if (row < 3 || (column >= 3 && column < 5)) tee++;
        Assert.Equal(32, tee);
        var patio = 9 * 4 + 3 * 2;
        Assert.Equal(42, patio);
        Assert.Equal(3 * 6 + 6 * 4, patio);
        Assert.True(Grade3SourceCompanionCatalogue.Grade(Task("A1-PARTIAL"), "44 square units").NumericCorrect);
        Assert.True(Grade3SourceCompanionCatalogue.Grade(Lesson().SourceBasedCheck!, "32 square units").NumericCorrect);
        Assert.True(Grade3SourceCompanionCatalogue.Grade(Task("A2-PAVERS"), "42").NumericCorrect);
        Assert.True(Grade3SourceCompanionCatalogue.Grade(Task("A2-AREA"), "42 ft²").NumericCorrect);
    }

    [Theory]
    [InlineData("A1-PARTIAL", "32 square units")]
    [InlineData("A1-PARTIAL", "44")]
    [InlineData("A1-PARTIAL", "44 ft²")]
    [InlineData("A2-PAVERS", "32")]
    [InlineData("A2-PAVERS", "42 ft²")]
    [InlineData("A2-AREA", "32 ft²")]
    [InlineData("A2-AREA", "42")]
    [InlineData("A2-AREA", "42 ft")]
    [InlineData("A2-AREA", "42 square units")]
    [InlineData("A2-AREA", "42 m²")]
    [InlineData("A2-AREA", "42 ft² trailing")]
    public void CountsAreasAndDifferentFiguresAreNotInterchangeable(string suffix, string wrong)
    {
        var result = Grade3SourceCompanionCatalogue.Grade(Task(suffix), wrong);
        Assert.False(result.NumericCorrect);
        Assert.True(result.TeacherReview);
    }

    [Fact]
    public void NumberTalkAndTeacherOnlyMethodsKeepTheirBoundaries()
    {
        var lesson = Lesson();
        var answers = new[] { 113, 123, 243, 253 };
        for (var i = 0; i < answers.Length; i++)
        {
            var q = Task($"WU-{i + 1}");
            Assert.True(Grade3SourceCompanionCatalogue.Grade(q, answers[i].ToString()).NumericCorrect);
            Assert.False(Grade3SourceCompanionCatalogue.Grade(q, (answers[i] + 1).ToString()).NumericCorrect);
        }
        foreach (var q in lesson.SourceSubtasks.Where(q => q.AnswerType == "teacher"))
        foreach (var answer in new[] { "44 square units", "32 square units", "42 ft²", "" })
        {
            var result = Grade3SourceCompanionCatalogue.Grade(q, answer);
            Assert.Null(result.NumericCorrect);
            Assert.True(result.TeacherReview);
        }
        Assert.Contains("no determined numerical area", lesson.TeacherReviewNote);
        Assert.Contains("Count intervals", lesson.TeacherReviewNote);
        Assert.True(lesson.OriginalStudentTaskPdfRead);
        Assert.True(lesson.ReadyForStageD);
    }

    [Fact]
    public void IndependentPatioAndNewCoolDownRemainDistinctFromNoah()
    {
        var lesson = Lesson();
        Assert.True(Grade3SourceCompanionCatalogue.Grade(lesson.IndependentPractice!, "32").NumericCorrect);
        Assert.False(Grade3SourceCompanionCatalogue.Grade(lesson.IndependentPractice!, "42").NumericCorrect);
        Assert.Equal(9 * 3 + 2 * 4, 2 * 7 + 7 * 3);
        Assert.True(Grade3SourceCompanionCatalogue.Grade(lesson.OriginalEdulyticsCoolDown!, "35 square units").NumericCorrect);
        foreach (var wrong in new[] { "35", "35 ft²", "32 square units", "42 square units", "-35 square units" })
            Assert.False(Grade3SourceCompanionCatalogue.Grade(lesson.OriginalEdulyticsCoolDown!, wrong).NumericCorrect);
    }

    [Fact]
    public void GuardedCanonicalMaterializationTeachesTheProgressionToUnGriddedArea()
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
        Assert.Contains("44 square units", en.WorkedExamples);
        Assert.Contains("32 square units", en.WorkedExamples);
        Assert.Contains("42 pavers", en.WorkedExamples);
        Assert.Contains("42 ft²", en.WorkedExamples);
        Assert.Contains("seven interior top marks", en.Explanation);
        Assert.Contains("MLR7", en.Explanation);
        Assert.Contains("no determined numerical area", en.Explanation);
        Assert.DoesNotContain("classify", en.StepByStepSolutions, StringComparison.OrdinalIgnoreCase);
        lesson.CanonicalBodySha256 = new string('0', 64);
        Assert.False(CommonCoreG3SourceTeachingCorrections.IsTarget(document, lesson));
        Assert.Throws<InvalidOperationException>(() => CommonCoreG3SourceTeachingCorrections.Apply(document));
    }
}
