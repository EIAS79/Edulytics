using System;
using System.Linq;
using System.Text.Json;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UsCcssG3Unit6Lesson9TimeToMinuteTests
{
 private static JsonElement Lesson()
 {
  using var s=System.Reflection.Assembly.Load("Edulytics.Data").GetManifestResourceStream("Edulytics.Data.Grade3.Unit6Lesson9RichV2");
  Assert.NotNull(s);using var d=JsonDocument.Parse(s!);
  Assert.Equal("g3-u06-l09-2021-source-lin-137-and-four-draw-clock-v1",d.RootElement.GetProperty("ContentVersion").GetString());
  var ls=d.RootElement.GetProperty("Lessons");
  Assert.Equal("PED:US-CCSS-MATH:G3:U06:L09",ls[0].GetProperty("LessonCode").GetString());
  return ls[0].Clone();
 }
 [Theory]
 [InlineData(2,36)][InlineData(3,18)][InlineData(12,17)][InlineData(9,2)]
 public void OriginalDrawTargetsHaveValidMinutePositions(int hour,int minute)
 {
  Assert.InRange(hour,1,12);
  Assert.InRange(minute,0,59);
 }
 [Fact]
 public void OriginalLinIs37Not35()
 {
  Assert.Equal(2,37-35);
  var p=string.Join(" ",Lesson().GetProperty("ExplanationParagraphs").EnumerateArray().Select(x=>x.GetString()!));
  Assert.Contains("LIN",p);
  Assert.Contains("FOUR distinct original",p);
  Assert.Contains("FIFTH original open task",p);
 }
}