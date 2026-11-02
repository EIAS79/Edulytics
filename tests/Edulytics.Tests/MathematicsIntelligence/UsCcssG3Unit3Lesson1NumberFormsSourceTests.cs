using System.Text.Json;
using System.Text.Json.Serialization;
using Edulytics.Core.Curriculum;
using Edulytics.Data.Seeding;
using Edulytics.Web.Curriculum.Grade3;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Original deck, equivalent decomposition and open-task authoring contracts. NOT RUN.</summary>
public sealed class UsCcssG3Unit3Lesson1NumberFormsSourceTests
{
    private const string Code = "PED:US-CCSS-MATH:G3:U03:L01";
    private static Grade3CompanionLesson Lesson()
    {
        Assert.True(Grade3SourceCompanionCatalogue.TryGet(Code, out var lesson));
        return Assert.IsType<Grade3CompanionLesson>(lesson);
    }
    private static Grade3OriginalQuestion Task(string suffix) =>
        Lesson().SourceSubtasks.Single(q => q.Id.EndsWith(suffix, StringComparison.Ordinal));

    [Theory]
    [InlineData("5,7,2", true)]
    [InlineData("4,17,2", true)]
    [InlineData("5,6,12", true)]
    [InlineData("0,57,2", true)]
    [InlineData("0,0,572", true)]
    [InlineData(" 4 , 17 , 2 ", true)]
    [InlineData("572", false)]
    [InlineData("5,7,3", false)]
    [InlineData("5,17,2", false)]
    [InlineData("-1,67,2", false)]
    [InlineData("5.0,7,2", false)]
    [InlineData("5,7", false)]
    [InlineData("5,7,2,0", false)]
    [InlineData("5,7,2 trailing", false)]
    [InlineData("1001,0,0", false)]
    [InlineData("999999999999999,0,0", false)]
    [InlineData("", false)]
    public void NewCoolDownAcceptsEquivalentGroupingsWithoutReturningTheGivenWhole(string answer, bool correct)
    {
        var q = Lesson().OriginalEdulyticsCoolDown!;
        Assert.Equal("place-value-parts", q.AnswerType);
        var result = Grade3SourceCompanionCatalogue.Grade(q, answer);
        Assert.Equal(correct, result.NumericCorrect);
        Assert.True(result.TeacherReview);
    }

    [Fact]
    public void BoundedGroupingsMatchAnIndependentWeightedValueOracle()
    {
        var q = Lesson().OriginalEdulyticsCoolDown!;
        for (var hundreds = 0; hundreds <= 6; hundreds++)
        for (var tens = 0; tens <= 60; tens++)
        foreach (var ones in new[] { 0, 1, 2, 12, 22, 72, 572 })
        {
            var wanted = hundreds * 100 + tens * 10 + ones == 572;
            var result = Grade3SourceCompanionCatalogue.Grade(q, $"{hundreds},{tens},{ones}");
            Assert.Equal(wanted, result.NumericCorrect);
            Assert.True(result.TeacherReview);
        }
    }

    [Fact]
    public void OneThousandBoundaryAndInvalidTargetsFailClosed()
    {
        var q = new Grade3OriginalQuestion { AnswerType = "place-value-parts", Expected = "1000" };
        foreach (var answer in new[] { "10,0,0", "0,100,0", "0,0,1000" })
            Assert.True(Grade3SourceCompanionCatalogue.Grade(q, answer).NumericCorrect);
        Assert.False(Grade3SourceCompanionCatalogue.Grade(q, "0,0,1001").NumericCorrect);
        foreach (var invalid in new string?[] { null, "0", "1001", "five hundred", "-1" })
        {
            q.Expected = invalid;
            Assert.Null(Grade3SourceCompanionCatalogue.Grade(q, "5,7,2").NumericCorrect);
        }
    }

    [Fact]
    public void OriginalNumericAnchorsDistinguishWarmupAndCardDiagrams()
    {
        var anchors = new[] { ("WU-A", 370), ("WU-B", 371), ("WU-D", 370),
            ("A1-C", 371), ("A1-J", 299), ("A1-L", 175) };
        foreach (var (suffix, expected) in anchors)
        {
            Assert.True(Grade3SourceCompanionCatalogue.Grade(Task(suffix), expected.ToString()).NumericCorrect);
            Assert.False(Grade3SourceCompanionCatalogue.Grade(Task(suffix), (expected + 1).ToString()).NumericCorrect);
        }
        Assert.True(Grade3SourceCompanionCatalogue.Grade(Lesson().SourceBasedCheck!, "370").NumericCorrect);
        Assert.True(Grade3SourceCompanionCatalogue.Grade(Lesson().IndependentPractice!, "625").NumericCorrect);
        foreach (var answer in new[] { "3,6,5", "2,16,5", "3,5,15" })
            Assert.True(Grade3SourceCompanionCatalogue.Grade(Task("A2-365-PARTS"), answer).NumericCorrect);
    }

    [Fact]
    public void OriginalDeckGroupsIncludePairsAndTriplesAndNeverAutoCertifyTheSort()
    {
        var lesson = Lesson();
        Assert.Equal(12, lesson.SourceCardCount);
        Assert.True(lesson.OriginalTeacherBlacklineMasterPdfRead);
        foreach (var group in new[] { "A–G–L=175", "B–H=813", "C–F=371", "D–I–J=299", "E–K=329" })
            Assert.Contains(group, lesson.TeacherReviewNote);
        foreach (var q in lesson.SourceSubtasks.Where(q => q.AnswerType == "teacher"))
        foreach (var answer in new[] { "175", "370", "3,6,5", "" })
        {
            var result = Grade3SourceCompanionCatalogue.Grade(q, answer);
            Assert.Null(result.NumericCorrect);
            Assert.True(result.TeacherReview);
        }
        Assert.True(lesson.OriginalStudentTaskPdfRead);
        Assert.True(lesson.ReadyForStageD);
    }

    [Fact]
    public void GuardedCanonicalMaterializationExplainsAllFormsExchangesAndOriginalTasks()
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
        Assert.Contains("400 + 80 + 2", en.Explanation);
        Assert.Contains("A–G–L", en.Explanation);
        Assert.Contains("2 hundreds, 16 tens", en.Explanation);
        Assert.Contains("One more pass returns", en.Explanation);
        Assert.Contains("MLR8", en.Explanation);
        Assert.Contains("Box 4", en.Explanation);
        Assert.Contains("not a demand for a new formal algorithm", en.Explanation);
        Assert.DoesNotContain("inverse operation", en.StepByStepSolutions, StringComparison.OrdinalIgnoreCase);
        lesson.CanonicalBodySha256 = new string('0', 64);
        Assert.False(CommonCoreG3SourceTeachingCorrections.IsTarget(document, lesson));
        Assert.Throws<InvalidOperationException>(() => CommonCoreG3SourceTeachingCorrections.Apply(document));
    }
}
