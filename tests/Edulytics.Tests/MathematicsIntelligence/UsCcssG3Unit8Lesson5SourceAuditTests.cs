using System;
using System.Linq;
using System.Text.Json;
using Edulytics.Web.Curriculum.Grade3;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class UsCcssG3Unit8Lesson5SourceAuditTests
{
    private const string Code = "PED:US-CCSS-MATH:G3:U08:L05";
    private const string Version = "g3-u08-l05-2021-one-room-one-thousand-budget-independent-cost-example-v1";
    [Fact]
    public void SourceOpenRoomBudgetTasksRequireTeacherOriginalPriceReview()
    {
        Assert.True(Grade3SourceCompanionCatalogue.TryGet(Code, out var lesson));
        Assert.NotNull(lesson);
        Assert.Equal(3, lesson!.SourceSubtasks.Count);
        foreach (var q in lesson.SourceSubtasks)
        {
            Assert.StartsWith("G3-U08-L05-SOURCE-", q.Id);
            Assert.Equal("teacher", q.AnswerType);
            Assert.Null(q.Expected);
            var result=Grade3SourceCompanionCatalogue.Grade(q,"930");
            Assert.Null(result.NumericCorrect);
            Assert.True(result.TeacherReview);
        }
    }
    [Fact]
    public void SourceSpecificRichV2KeepsBudgetAndIndependentExamplesSeparate()
    {
        var assembly=System.Reflection.Assembly.Load("Edulytics.Data");
        using var stream=assembly.GetManifestResourceStream("Edulytics.Data.Grade3.Unit8Lesson5RichV2");
        Assert.NotNull(stream);
        using var doc=JsonDocument.Parse(stream!);
        Assert.Equal(Version,doc.RootElement.GetProperty("ContentVersion").GetString());
        var lesson=doc.RootElement.GetProperty("Lessons")[0];
        Assert.Equal(Code,lesson.GetProperty("LessonCode").GetString());
        var body=string.Join("\n",lesson.GetProperty("ExplanationParagraphs").EnumerateArray().Select(x=>x.GetString()));
        Assert.Contains("$1,000",body);
        Assert.Contains("$930",body);
        Assert.Contains("NOT original item prices",body);
        Assert.Contains("cost sheet",body);
    }
    [Fact]
    public void MaterializerReferencesItsOwnEmbeddedResource()
    {
        var assembly=System.Reflection.Assembly.Load("Edulytics.Data");
        var type=assembly.GetType("Edulytics.Data.Seeding.CommonCoreG3Unit8Lesson5RichV2");
        Assert.NotNull(type);
        var field=type!.GetField("ResourceName",System.Reflection.BindingFlags.Static|System.Reflection.BindingFlags.NonPublic);
        Assert.NotNull(field);
        Assert.Equal("Edulytics.Data.Grade3.Unit8Lesson5RichV2",field!.GetRawConstantValue());
    }
}
