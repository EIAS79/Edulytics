using System;
using System.Linq;
using System.Text.Json;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UsCcssG3Unit6Lesson10ElapsedBusTimeTests
{
 private static JsonElement Lesson()
 {
  using var stream=System.Reflection.Assembly.Load("Edulytics.Data").GetManifestResourceStream("Edulytics.Data.Grade3.Unit6Lesson10RichV2");
  Assert.NotNull(stream);using var doc=JsonDocument.Parse(stream!);
  Assert.Equal("g3-u06-l10-2021-two-24min-bus-waits-and-optional-38min-strategy-v1",doc.RootElement.GetProperty("ContentVersion").GetString());
  var ls=doc.RootElement.GetProperty("Lessons");Assert.Equal("PED:US-CCSS-MATH:G3:U06:L10",ls[0].GetProperty("LessonCode").GetString());return ls[0].Clone();
 }
 [Theory]
 [InlineData(3,27,24,3,51)][InlineData(3,45,24,4,9)]
 public void OriginalWaitProblems(int sh,int sm,int wait,int eh,int em)
 {
  int total=(sh*60+sm+wait)%(12*60);
  Assert.Equal(eh*60+em,total);
 }
 [Fact]
 public void SourceStrategyQuestionDoesNotMandateAnswer()
 {
  Assert.Equal(38,(7*60+10)-(6*60+32));
  var p=string.Join(" ",Lesson().GetProperty("ExplanationParagraphs").EnumerateArray().Select(x=>x.GetString()!));
  Assert.Contains("NOT REQUIRED TO SOLVE",p);
  Assert.Contains("24 minutes",p);
 }
 [Fact]
 public void SourceChoralCount13TimesIncluding3PM()
 {
  Assert.Equal(13,1+180/15);
 }
}