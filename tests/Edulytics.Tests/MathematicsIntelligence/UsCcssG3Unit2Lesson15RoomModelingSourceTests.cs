using System.Text.Json;
using System.Text.Json.Serialization;
using Edulytics.Core.Curriculum;
using Edulytics.Data.Seeding;
using Edulytics.Web.Curriculum.Grade3;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Source-grounded open modeling and numeric anchor contracts. NOT RUN; deferred Stage D.</summary>
public sealed class UsCcssG3Unit2Lesson15RoomModelingSourceTests
{
    private const string Code = "PED:US-CCSS-MATH:G3:U02:L15";
    private static Grade3CompanionLesson Lesson()
    {
        Assert.True(Grade3SourceCompanionCatalogue.TryGet(Code, out var lesson));
        return Assert.IsType<Grade3CompanionLesson>(lesson);
    }
    private static Grade3OriginalQuestion Task(string suffix) =>
        Lesson().SourceSubtasks.Single(q => q.Id.EndsWith(suffix, StringComparison.Ordinal));

    [Fact]
    public void SourceDimensionsGiveIndependentOuterAreaAndFurnitureFootprints()
    {
        Assert.Equal(10, 15 - 5);
        Assert.Equal(10, 6 + 4);
        Assert.Equal(110, 6 * 15 + 4 * 5);
        Assert.Equal(6 * 15 + 4 * 5, 6 * 10 + 10 * 5);
        Assert.Equal(18, 3 * 6);
        Assert.Equal(6, 3 * 2);
        var anchors = new[] {
            ("A2-MISSING-HEIGHT", "10 ft"), ("A2-FULL-WIDTH", "10 ft"),
            ("A2-ROOM-AREA", "110 ft²"), ("A2-BED", "18 ft²"), ("A2-DESK", "6 ft²") };
        foreach (var (suffix, answer) in anchors)
        {
            var result = Grade3SourceCompanionCatalogue.Grade(Task(suffix), answer);
            Assert.True(result.NumericCorrect);
            Assert.True(result.TeacherReview);
        }
    }

    [Theory]
    [InlineData("A2-ROOM-AREA", "86 ft²")]
    [InlineData("A2-ROOM-AREA", "110")]
    [InlineData("A2-ROOM-AREA", "110 ft")]
    [InlineData("A2-ROOM-AREA", "110 m²")]
    [InlineData("A2-BED", "24 ft²")]
    [InlineData("A2-DESK", "6")]
    [InlineData("A2-MISSING-HEIGHT", "10 ft²")]
    [InlineData("A2-MISSING-HEIGHT", "5 ft")]
    [InlineData("A2-FULL-WIDTH", "4 ft")]
    public void NumericAnchorsRequireTheRequestedQuantityAndUnit(string suffix, string wrong)
    {
        var result = Grade3SourceCompanionCatalogue.Grade(Task(suffix), wrong);
        Assert.False(result.NumericCorrect);
        Assert.True(result.TeacherReview);
    }

    [Fact]
    public void AnUnexplainedEightySixCannotAutoCompleteTheOriginalProject()
    {
        var lesson = Lesson();
        Assert.Equal(86, 110 - 18 - 6); // Conditional remainder, not a full design verdict.
        Assert.Equal("teacher", lesson.SourceBasedCheck!.AnswerType);
        Assert.Equal("teacher", lesson.IndependentPractice!.AnswerType);
        var openTasks = lesson.SourceSubtasks.Where(q => q.AnswerType == "teacher")
            .Concat(new[] { lesson.SourceBasedCheck!, lesson.IndependentPractice! });
        foreach (var q in openTasks)
        foreach (var answer in new[] { "86", "86 ft²", "110 ft²", "18+6=24", "" })
        {
            var result = Grade3SourceCompanionCatalogue.Grade(q, answer);
            Assert.Null(result.NumericCorrect);
            Assert.True(result.TeacherReview);
        }
        Assert.Contains("Closet dimensions are absent", lesson.TeacherReviewNote);
        Assert.Contains("Many layouts can work", lesson.TeacherReviewNote);
        Assert.Contains("own grid-paper diagram", lesson.TeacherReviewNote);
        Assert.True(lesson.OriginalStudentTaskPdfRead);
        Assert.False(lesson.OriginalTeacherBlacklineMasterPdfRead);
        Assert.True(lesson.ReadyForStageD);
    }

    [Fact]
    public void IndependentExitTaskHasExplicitFootprintsAndStillNeedsDesignReview()
    {
        var q = Lesson().OriginalEdulyticsCoolDown!;
        Assert.Equal(64, 8 * 10 - 2 * 5 - 2 * 3);
        var correct = Grade3SourceCompanionCatalogue.Grade(q, "64 ft²");
        Assert.True(correct.NumericCorrect);
        Assert.True(correct.TeacherReview);
        foreach (var wrong in new[] { "64", "64 ft", "64 m²", "86 ft²", "80 ft²", "-64 ft²", "64 ft² trailing" })
            Assert.False(Grade3SourceCompanionCatalogue.Grade(q, wrong).NumericCorrect);
    }

    [Fact]
    public void GuardedCanonicalMaterializationKeepsOpenModelingAndExplicitAssumptions()
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
        Assert.Contains("110 ft²", en.WorkedExamples);
        Assert.Contains("18 ft²", en.WorkedExamples);
        Assert.Contains("6 ft²", en.WorkedExamples);
        Assert.Contains("two-item-only assumption", en.WorkedExamples);
        Assert.Contains("MLR5", en.Explanation);
        Assert.Contains("swap roles", en.Explanation);
        Assert.Contains("no listed publisher Cool-down", en.Explanation);
        Assert.Contains("64 ft²", en.WorkedExamples);
        Assert.DoesNotContain("classify", en.StepByStepSolutions, StringComparison.OrdinalIgnoreCase);
        lesson.CanonicalBodySha256 = new string('0', 64);
        Assert.False(CommonCoreG3SourceTeachingCorrections.IsTarget(document, lesson));
        Assert.Throws<InvalidOperationException>(() => CommonCoreG3SourceTeachingCorrections.Apply(document));
    }
}
