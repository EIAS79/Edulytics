
using System;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using Edulytics.Web.Curriculum.Grade3;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UsCcssG3Unit8Lesson11SourceAuditTests
{
  private const string Code="PED:US-CCSS-MATH:G3:U08:L11";
  private const int SourceMathCount=4;
  private const int TeacherTasks=3;
  [Theory] [InlineData(0,24)] [InlineData(1,16)] [InlineData(2,12)] [InlineData(3,8)]
  public void SourceNumericAnchorsExact(int index,int expected)
  {
    Assert.True(Grade3SourceCompanionCatalogue.TryGet(Code,out var lesson));
    Assert.NotNull(lesson);
    var q=lesson!.SourceSubtasks[index];
    Assert.Equal("integer",q.AnswerType);
    Assert.Equal(expected.ToString(),q.Expected);
    Assert.True(Grade3SourceCompanionCatalogue.Grade(q,expected.ToString()).NumericCorrect);
  }

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
    var t=Assembly.Load("Edulytics.Data").GetType("Edulytics.Data.Seeding.CommonCoreG3Unit8Lesson11RichV2");
    Assert.NotNull(t);
    var load=t!.GetMethod("Load",BindingFlags.Static|BindingFlags.NonPublic);
    var content=Assert.IsType<JsonElement>(load!.Invoke(null,null));
    Assert.Equal(Code,content.GetProperty("LessonCode").GetString());
    Assert.Equal("Division Game Day",content.GetProperty("Title").GetString());
    Assert.True(content.GetProperty("ExplanationParagraphs").GetArrayLength()>=5);
    Assert.True(content.GetProperty("WorkedExamples").GetArrayLength()>=1);
  }
}
