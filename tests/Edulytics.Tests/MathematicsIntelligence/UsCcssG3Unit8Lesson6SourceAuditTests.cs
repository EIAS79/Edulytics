using System;
using System.Linq;
using System.Text.Json;
using Edulytics.Web.Curriculum.Grade3;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UsCcssG3Unit8Lesson6SourceAuditTests
{
    private const string Code="PED:US-CCSS-MATH:G3:U08:L06";
    [Fact]
    public void AuthoredSourceSurveyStagesRemainTeacherReviewed()
    {
        Assert.True(Grade3SourceCompanionCatalogue.TryGet(Code,out var lesson));
        Assert.NotNull(lesson);
        Assert.Equal(4,lesson!.SourceSubtasks.Count);
        foreach(var q in lesson.SourceSubtasks){
            Assert.Equal("teacher",q.AnswerType);Assert.Null(q.Expected);
            var result=Grade3SourceCompanionCatalogue.Grade(q,"12");
            Assert.Null(result.NumericCorrect);Assert.True(result.TeacherReview);
        }
    }
    [Fact]
    public void RichV2KeepsCategoryLimitAndOriginalSurveyAuthenticity()
    {
        var assembly=System.Reflection.Assembly.Load("Edulytics.Data");
        using var stream=assembly.GetManifestResourceStream("Edulytics.Data.Grade3.Unit8Lesson6RichV2");
        Assert.NotNull(stream);
        using var doc=JsonDocument.Parse(stream!);
        Assert.Equal("g3-u08-l06-2021-authentic-six-category-large-group-survey-data-v1",doc.RootElement.GetProperty("ContentVersion").GetString());
        var l=doc.RootElement.GetProperty("Lessons")[0];
        Assert.Equal(Code,l.GetProperty("LessonCode").GetString());
        var text=string.Join("\n",l.GetProperty("ExplanationParagraphs").EnumerateArray().Select(x=>x.GetString()));
        Assert.Contains("UP TO SIX",text);
        Assert.Contains("SAME STUDENT",text);
        Assert.Contains("NEXT lesson",text);
    }
    [Fact]
    public void MaterializerReferencesUnit8Lesson6Resource()
    {
        var t=System.Reflection.Assembly.Load("Edulytics.Data").GetType("Edulytics.Data.Seeding.CommonCoreG3Unit8Lesson6RichV2");
        Assert.NotNull(t);
        var f=t!.GetField("ResourceName",System.Reflection.BindingFlags.NonPublic|System.Reflection.BindingFlags.Static);
        Assert.Equal("Edulytics.Data.Grade3.Unit8Lesson6RichV2",f!.GetRawConstantValue());
    }
}
