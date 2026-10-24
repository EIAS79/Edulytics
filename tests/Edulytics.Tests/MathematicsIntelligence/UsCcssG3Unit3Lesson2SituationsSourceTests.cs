using System.Text.Json;
using System.Text.Json.Serialization;
using Edulytics.Core.Curriculum;
using Edulytics.Data.Seeding;
using Edulytics.Web.Curriculum.Grade3;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade 3 U03 L02 authoring regressions: written but not run; no Stage D acceptance.</summary>
public sealed class UsCcssG3Unit3Lesson2SituationsSourceTests
{
    private const string Code = "PED:US-CCSS-MATH:G3:U03:L02";
    private static Grade3CompanionLesson Lesson()
    {
        Assert.True(Grade3SourceCompanionCatalogue.TryGet(Code, out var lesson));
        return Assert.IsType<Grade3CompanionLesson>(lesson);
    }
    private static Grade3OriginalQuestion Task(string suffix) =>
        Lesson().SourceSubtasks.Single(q => q.Id.EndsWith(suffix, StringComparison.Ordinal));

    [Theory]
    [InlineData("WU-TABLE-1", "60", true)]
    [InlineData("WU-TABLE-1", "70", false)]
    [InlineData("WU-TABLE-2", "60", true)]
    [InlineData("WU-TABLE-2", "100", false)]
    [InlineData("A1-FALLS", "246 ft", true)]
    [InlineData("A1-FALLS", "246 feet", true)]
    [InlineData("A1-FALLS", "246", false)]
    [InlineData("A1-FALLS", "456 ft", false)]
    [InlineData("A1-MONUMENTS", "456 ft", true)]
    [InlineData("A1-MONUMENTS", "555 ft", false)]
    [InlineData("A1-MONUMENTS", "456 cm", false)]
    [InlineData("A1-EIFFEL", "346", true)]
    [InlineData("A1-EIFFEL", "674", false)]
    [InlineData("A1-EIFFEL", "346 ft", false)]
    [InlineData("A1-EIFFEL", "-346", false)]
    [InlineData("A1-EIFFEL", "346.0", false)]
    public void SourceNumericAnchorsPreserveContextAndUnits(string id, string answer, bool expected)
    {
        var result = Grade3SourceCompanionCatalogue.Grade(Task(id), answer);
        Assert.Equal(expected, result.NumericCorrect);
        Assert.True(result.TeacherReview);
    }

    [Fact]
    public void AllOpenTasksRemainTeacherReviewed()
    {
        var lesson = Lesson();
        Assert.Equal(14, lesson.SourceSubtasks.Count);
        foreach (var task in lesson.SourceSubtasks.Where(t => t.AnswerType == "teacher"))
        {
            Assert.Null(Grade3SourceCompanionCatalogue.Grade(task, "60").NumericCorrect);
            Assert.True(Grade3SourceCompanionCatalogue.Grade(task, "I used base-ten blocks").TeacherReview);
        }
        Assert.False(lesson.ReadyForStageD);
        Assert.False(lesson.OriginalStudentTaskPdfRead);
    }

    [Fact]
    public void IndependentPracticeAndPublisherCoolDownAreNotSourceEquivalents()
    {
        var lesson = Lesson();
        Assert.Equal("281", lesson.IndependentPractice!.Expected);
        Assert.Equal("409", lesson.OriginalEdulyticsCoolDown!.Expected);
        Assert.Equal("246 ft", lesson.SourceBasedCheck!.Expected);
        Assert.True(Grade3SourceCompanionCatalogue.Grade(lesson.SourceBasedCheck, "246 feet").NumericCorrect);
        Assert.False(Grade3SourceCompanionCatalogue.Grade(lesson.SourceBasedCheck, "246").NumericCorrect);
    }

    [Fact]
    public void MaterializedLessonContainsAllThreeSourceStoriesAndNoMandatoryAlgorithm()
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
        Assert.Contains("115 feet and 131 feet", en.Explanation);
        Assert.Contains("456 feet", en.Explanation);
        Assert.Contains("346 steps", en.Explanation);
        Assert.Contains("Journal About Connections", en.Explanation);
        Assert.Contains("MLR8", en.Explanation);
        Assert.Contains("NOT the goal", en.Explanation);
        Assert.Contains("publisher's gated", en.Explanation);
        lesson.CanonicalBodySha256 = new string('0',64);
        Assert.False(CommonCoreG3SourceTeachingCorrections.IsTarget(document,lesson));
    }
}
