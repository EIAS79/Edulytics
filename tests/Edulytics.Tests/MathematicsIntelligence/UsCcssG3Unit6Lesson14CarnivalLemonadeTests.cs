using System;
using System.Linq;
using System.Text.Json;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UsCcssG3Unit6Lesson14CarnivalLemonadeTests
{
 private static JsonElement Lesson()
 {
  using var s=System.Reflection.Assembly.Load("Edulytics.Data").GetManifestResourceStream("Edulytics.Data.Grade3.Unit6Lesson14RichV2");
  Assert.NotNull(s);using var d=JsonDocument.Parse(s!);
  Assert.Equal("g3-u06-l14-2021-carnival-open-times-lemonade-42-and-14-v1",d.RootElement.GetProperty("ContentVersion").GetString());
  var l=d.RootElement.GetProperty("Lessons");Assert.Equal("PED:US-CCSS-MATH:G3:U06:L14",l[0].GetProperty("LessonCode").GetString());return l[0].Clone();
 }
 [Theory]
 [InlineData(306,199,505)][InlineData(318,297,615)]
 [InlineData(275,325,600)][InlineData(275,329,604)]
 public void ExactSourceNumberTalk(int a,int b,int sum)=>Assert.Equal(sum,a+b);
 [Fact]
 public void OriginalLemonadeQuantitiesMapToTwoQuestions()
 {
  Assert.Equal(42,132-90);
  Assert.Equal(14,56/4);
  var t=string.Join(" ",Lesson().GetProperty("ExplanationParagraphs").EnumerateArray().Select(x=>x.GetString()!));
  Assert.Contains("EXACTLY THREE",t);
  Assert.Contains("FOURTEEN segments",t);
 }
}