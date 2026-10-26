using System;
using System.Linq;
using System.Text.Json;
using Edulytics.Web.Curriculum.Grade3;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class UsCcssG3Unit8Lesson2SourceAuditTests
{
    private const string Code = "PED:US-CCSS-MATH:G3:U08:L02";
    private const string Version = "g3-u08-l02-2021-floor-number-line-thirteen-fractions-six-comparisons-v1";

    [Fact]
    public void OpenSourceTasksRemainHumanReviewedNotScalarAutograded()
    {
        Assert.True(Grade3SourceCompanionCatalogue.TryGet(Code, out var lesson));
        Assert.NotNull(lesson);
        Assert.Equal(4, lesson!.SourceSubtasks.Count);
        foreach (var q in lesson.SourceSubtasks)
        {
            Assert.StartsWith("G3-U08-L02-SOURCE-", q.Id);
            Assert.Equal("teacher", q.AnswerType);
            Assert.Null(q.Expected);
            var result = Grade3SourceCompanionCatalogue.Grade(q, "4");
            Assert.Null(result.NumericCorrect);
            Assert.True(result.TeacherReview);
        }
    }

    [Fact]
    public void SourceSpecificRichV2CoversSourceThirteenFractionsAndSixSymbolRule()
    {
        var assembly = System.Reflection.Assembly.Load("Edulytics.Data");
        using var stream = assembly.GetManifestResourceStream("Edulytics.Data.Grade3.Unit8Lesson2RichV2");
        Assert.NotNull(stream);
        using var doc = JsonDocument.Parse(stream!);
        Assert.Equal(Version, doc.RootElement.GetProperty("ContentVersion").GetString());
        var lesson = doc.RootElement.GetProperty("Lessons")[0];
        Assert.Equal(Code, lesson.GetProperty("LessonCode").GetString());
        var body = string.Join("\n", lesson.GetProperty("ExplanationParagraphs").EnumerateArray().Select(x => x.GetString()));
        foreach (var fraction in new[] {"1/2", "1/3", "6/2", "12/3", "1/4", "5/4", "6/6", "5/6", "9/8", "15/8", "5/3", "18/6", "2/8"})
            Assert.Contains(fraction, body);
        Assert.Contains("SIX comparison statements", body);
        Assert.Contains("from 0 to 2 would be too short", body);
        Assert.Contains("teacher", body, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void SourceSpecificMaterializerSelectsItsOwnResourceNotUnit8Lesson1()
    {
        var assembly = System.Reflection.Assembly.Load("Edulytics.Data");
        var type = assembly.GetType("Edulytics.Data.Seeding.CommonCoreG3Unit8Lesson2RichV2");
        Assert.NotNull(type);
        var field = type!.GetField("ResourceName", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);
        Assert.NotNull(field);
        Assert.Equal("Edulytics.Data.Grade3.Unit8Lesson2RichV2", field!.GetRawConstantValue());
    }
}
