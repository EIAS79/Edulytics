using System;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using Edulytics.Web.Curriculum.Grade3;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UsCcssG3Unit8Lesson7SourceAuditTests
{
    private const string Code = "PED:US-CCSS-MATH:G3:U08:L07";
    [Fact] public void OpenSourceTasksRequireHumanJudgement()
    {
        Assert.True(Grade3SourceCompanionCatalogue.TryGet(Code,out var lesson));
        Assert.NotNull(lesson);
        Assert.Equal(4,lesson!.SourceSubtasks.Count);
        foreach(var item in lesson.SourceSubtasks) {
            Assert.Equal("teacher",item.AnswerType);
            Assert.Null(item.Expected);
            var grade=Grade3SourceCompanionCatalogue.Grade(item,"16");
            Assert.True(grade.TeacherReview);
            Assert.Null(grade.NumericCorrect);
        }
    }
    [Fact] public void OriginalGraphAndRealSurveyRemainClearlyDistinctFromFiction()
    {
        var lesson=Load();
        var explanation=string.Join("\\n",lesson.GetProperty("ExplanationParagraphs").EnumerateArray().Select(x=>x.GetString()));
        Assert.Contains("genuine class or school survey",explanation);
        Assert.Contains("FICTIONAL",explanation);
        Assert.Contains("12+8-4=16",explanation);
        Assert.Contains("warm-up artwork",explanation);
        Assert.Contains("gated publisher",explanation);
    }
    [Fact] public void IndependentScaledExampleIsMathematicallyCorrect()
    {
        var lesson=Load();
        var e=lesson.GetProperty("WorkedExamples")[0];
        var steps=string.Join("\\n",e.GetProperty("Steps").EnumerateArray().Select(x=>x.GetString()));
        Assert.Contains("12/2=6",steps);
        Assert.Contains("8/2=4",steps);
        Assert.Contains("4/2=2",steps);
        Assert.Contains("12-8=4",steps);
        Assert.Contains("12+8=20",steps);
        Assert.Contains("20-4=16",steps);
        Assert.Equal((12+8)-4,16);
    }
    [Fact] public void SourceSpecificResourceIsVersionGuardedAndLoadable()
    {
        var t=Assembly.Load("Edulytics.Data").GetType("Edulytics.Data.Seeding.CommonCoreG3Unit8Lesson7RichV2");
        Assert.NotNull(t);
        var field=t!.GetField("ResourceName",BindingFlags.NonPublic|BindingFlags.Static);
        Assert.Equal("Edulytics.Data.Grade3.Unit8Lesson7RichV2",field!.GetRawConstantValue());
        var document=Load();
        Assert.Equal(Code,document.GetProperty("LessonCode").GetString());
        Assert.Equal("Graph and Answer",document.GetProperty("Title").GetString());
    }
    private static JsonElement Load()
    {
        var t=Assembly.Load("Edulytics.Data").GetType("Edulytics.Data.Seeding.CommonCoreG3Unit8Lesson7RichV2");
        Assert.NotNull(t);
        var method=t!.GetMethod("Load",BindingFlags.NonPublic|BindingFlags.Static);
        Assert.NotNull(method);
        return Assert.IsType<JsonElement>(method!.Invoke(null,null));
    }
}
