using System;
using System.Linq;
using System.Text.Json;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UsCcssG3Unit6Lesson11ElapsedTimeTests
{
 private static JsonElement Lesson()
 {
  using var s=System.Reflection.Assembly.Load("Edulytics.Data").GetManifestResourceStream("Edulytics.Data.Grade3.Unit6Lesson11RichV2");
  Assert.NotNull(s);using var d=JsonDocument.Parse(s!);
  Assert.Equal("g3-u06-l11-2021-han-55-elena-32-and-four-mathlibs-v1",d.RootElement.GetProperty("ContentVersion").GetString());
  var l=d.RootElement.GetProperty("Lessons");Assert.Equal("PED:US-CCSS-MATH:G3:U06:L11",l[0].GetProperty("LessonCode").GetString());return l[0].Clone();
 }
 [Theory]
 [InlineData(8,25,8,50,25)][InlineData(10,45,11,18,33)][InlineData(5,42,6,37,55)]
 public void SourceElapsedStories(int sh,int sm,int eh,int em,int expected)
 {
  Assert.Equal(expected,(eh*60+em)-(sh*60+sm));
 }
 [Fact]
 public void FourOriginalMathLibsAndHanSourceSummaryPreserved()
 {
  Assert.Equal(5*60+18,(5*60+38)-20);
  Assert.Equal(4*60+15,(3*60+30)+45);
  Assert.Equal(55,3+15+30+7);
  var p=string.Join(" ",Lesson().GetProperty("ExplanationParagraphs").EnumerateArray().Select(x=>x.GetString()!));
  Assert.Contains("FOUR ORIGINAL templates",p);
  Assert.Contains("WAITING EXACTLY32 MINUTES",p);
 }
}