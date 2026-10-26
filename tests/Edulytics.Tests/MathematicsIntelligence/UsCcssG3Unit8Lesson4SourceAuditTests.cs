using System;
using System.Linq;
using System.Text.Json;
using Edulytics.Web.Curriculum.Grade3;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class UsCcssG3Unit8Lesson4SourceAuditTests
{
    private const string Code = "PED:US-CCSS-MATH:G3:U08:L04";
    private const string Version = "g3-u08-l04-2021-tiny-house-three-options-area-perimeter-revision-v1";

    [Fact]
    public void SourceOpenDesignAndPartnerTasksAreNotAutoGraded()
    {
        Assert.True(Grade3SourceCompanionCatalogue.TryGet(Code, out var lesson));
        Assert.NotNull(lesson);
        Assert.Equal(4, lesson!.SourceSubtasks.Count);
        foreach (var q in lesson.SourceSubtasks)
        {
            Assert.StartsWith("G3-U08-L04-SOURCE-", q.Id);
            Assert.Equal("teacher", q.AnswerType);
            Assert.Null(q.Expected);
            var result = Grade3SourceCompanionCatalogue.Grade(q, "160");
            Assert.Null(result.NumericCorrect);
            Assert.True(result.TeacherReview);
        }
    }

    [Fact]
    public void MaterializedSourceCoversThreeExact2021LayoutsAndDistinctUnits()
    {
        var assembly=System.Reflection.Assembly.Load("Edulytics.Data");
        using var stream=assembly.GetManifestResourceStream("Edulytics.Data.Grade3.Unit8Lesson4RichV2");
        Assert.NotNull(stream);
        using var doc=JsonDocument.Parse(stream!);
        Assert.Equal(Version,doc.RootElement.GetProperty("ContentVersion").GetString());
        var lesson=doc.RootElement.GetProperty("Lessons")[0];
        Assert.Equal(Code,lesson.GetProperty("LessonCode").GetString());
        var body=string.Join("\n",lesson.GetProperty("ExplanationParagraphs").EnumerateArray().Select(x=>x.GetString()));
        foreach(var n in new[]{"8 feet by 20 feet","8 by 10 feet","8 by 8 feet","7 by 14 feet","160 square feet","144 square feet","98 square feet"})
            Assert.Contains(n,body);
        Assert.Contains("NEW partner",body);
        Assert.Contains("no single",body);
    }

    [Fact]
    public void SeedPointsToOwnUnit8Lesson4NotEarlierResource()
    {
        var assembly=System.Reflection.Assembly.Load("Edulytics.Data");
        var type=assembly.GetType("Edulytics.Data.Seeding.CommonCoreG3Unit8Lesson4RichV2");
        Assert.NotNull(type);
        var field=type!.GetField("ResourceName",System.Reflection.BindingFlags.Static|System.Reflection.BindingFlags.NonPublic);
        Assert.NotNull(field);
        Assert.Equal("Edulytics.Data.Grade3.Unit8Lesson4RichV2",field!.GetRawConstantValue());
    }
}
