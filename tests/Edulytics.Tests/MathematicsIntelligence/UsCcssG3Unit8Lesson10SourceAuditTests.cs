
using System;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using Edulytics.Web.Curriculum.Grade3;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UsCcssG3Unit8Lesson10SourceAuditTests
{
  private const string Code="PED:US-CCSS-MATH:G3:U08:L10";
  private const int SourceMathCount=0;
  private const int TeacherTasks=4;

  [Fact] public void OpenSourceActivitiesStayTeacherReviewed()
  {
    Assert.True(Grade3SourceCompanionCatalogue.TryGet(Code,out var lesson));
    Assert.NotNull(lesson);
    Assert.Equal(SourceMathCount+TeacherTasks,lesson!.SourceSubtasks.Count);
    foreach(var q in lesson.SourceSubtasks.Skip(SourceMathCount)){
      Assert.Equal("teacher",q.AnswerType);
      Assert.Null(q.Expected);
      var g=Grade3SourceCompanionCatalogue.Grade(q,"12");
      Assert.Null(g.NumericCorrect);
      Assert.True(g.TeacherReview);
    }
  }
  [Fact] public void Original2021VersionAndSourceSpecificContentLoad()
  {
    var t=Assembly.Load("Edulytics.Data").GetType("Edulytics.Data.Seeding.CommonCoreG3Unit8Lesson10RichV2");
    Assert.NotNull(t);
    var load=t!.GetMethod("Load",BindingFlags.Static|BindingFlags.NonPublic);
    var content=Assert.IsType<JsonElement>(load!.Invoke(null,null));
    Assert.Equal(Code,content.GetProperty("LessonCode").GetString());
    Assert.Equal("Multiplication and Division PLC Activity",content.GetProperty("Title").GetString());
    Assert.True(content.GetProperty("ExplanationParagraphs").GetArrayLength()>=5);
    Assert.True(content.GetProperty("WorkedExamples").GetArrayLength()>=1);
  }
}
