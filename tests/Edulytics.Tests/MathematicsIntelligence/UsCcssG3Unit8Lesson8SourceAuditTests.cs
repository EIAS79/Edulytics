using System;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using Edulytics.Web.Curriculum.Grade3;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UsCcssG3Unit8Lesson8SourceAuditTests
{
  private const string Code="PED:US-CCSS-MATH:G3:U08:L08";
  [Theory]
  [InlineData(0,8)][InlineData(1,20)][InlineData(2,40)][InlineData(3,68)]
  public void MentalProductsAgreeWithPublic2021Source(int index,int result)
  {
    Assert.True(Grade3SourceCompanionCatalogue.TryGet(Code,out var lesson));
    Assert.NotNull(lesson);
    var q=lesson!.SourceSubtasks[index];
    Assert.Equal("integer",q.AnswerType);
    Assert.Equal(result.ToString(),q.Expected);
    var grade=Grade3SourceCompanionCatalogue.Grade(q,result.ToString());
    Assert.True(grade.NumericCorrect);
  }
  [Fact] public void OriginalPartnerCardWorkNotAutoCertified()
  {
    Assert.True(Grade3SourceCompanionCatalogue.TryGet(Code,out var lesson));
    Assert.Equal(7,lesson!.SourceSubtasks.Count);
    foreach(var q in lesson.SourceSubtasks.Skip(4)){
      Assert.Equal("teacher",q.AnswerType);
      Assert.Null(q.Expected);
      var grade=Grade3SourceCompanionCatalogue.Grade(q,"72");
      Assert.Null(grade.NumericCorrect);
      Assert.True(grade.TeacherReview);
    }
  }
  [Fact] public void RichV2ResourceLoadsAndMatchesExpectedVersion()
  {
    var t=Assembly.Load("Edulytics.Data").GetType("Edulytics.Data.Seeding.CommonCoreG3Unit8Lesson8RichV2");
    Assert.NotNull(t);
    var m=t!.GetMethod("Load",BindingFlags.Static|BindingFlags.NonPublic);
    var doc=Assert.IsType<JsonElement>(m!.Invoke(null,null));
    Assert.Equal(Code,doc.GetProperty("LessonCode").GetString());
    var content=string.Join(" ",doc.GetProperty("ExplanationParagraphs").EnumerateArray().Select(e=>e.GetString()));
    Assert.Contains("THREE groups",content);
    Assert.Contains("FIVE facts",content);
    Assert.Contains("publisher",content);
  }
}
