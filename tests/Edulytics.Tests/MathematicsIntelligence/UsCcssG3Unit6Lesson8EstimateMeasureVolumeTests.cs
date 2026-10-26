using System;
using System.Linq;
using System.Text.Json;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>U06 L08 original source item invariants. Written but not executed.</summary>
public sealed class UsCcssG3Unit6Lesson8EstimateMeasureVolumeTests
{
 private static JsonElement Lesson()
 {
  using var s=typeof(Edulytics.Data.Seeding.CommonCoreG3SourceTeachingCorrections).Assembly.GetManifestResourceStream("Edulytics.Data.Grade3.Unit6Lesson8RichV2");
  Assert.NotNull(s);using var d=JsonDocument.Parse(s!);
  Assert.Equal("g3-u06-l08-2021-divide-three-volume-estimate-eight-drawings-five-shade-v1",d.RootElement.GetProperty("ContentVersion").GetString());
  var x=d.RootElement.GetProperty("Lessons");Assert.Equal(1,x.GetArrayLength());
  Assert.Equal("PED:US-CCSS-MATH:G3:U06:L08",x[0].GetProperty("LessonCode").GetString());
  return x[0].Clone();
 }
 [Theory]
 [InlineData(30,10)][InlineData(60,20)][InlineData(63,21)][InlineData(54,18)]
 public void SourceOriginalDivisionByThree(int x,int answer)=>Assert.Equal(answer,x/3);
 [Fact]
 public void EightOriginalDiagramLabelsRetainedWithoutFictitiousReadings()
 {
  var t=string.Join(" ",Lesson().GetProperty("ExplanationParagraphs").EnumerateArray().Select(x=>x.GetString()!));
  Assert.Contains("A,B,C,D,E,F,G,H",t);
  Assert.Contains("NOT inspected",t);
  Assert.Contains("no numeric values for A-H",t);
 }
 [Theory]
 [InlineData("P:1 liter")][InlineData("Q:8 liters")][InlineData("R:7 liters")]
 [InlineData("S:2 1/2 liters")][InlineData("T:23 liters")]
 public void SourceShadeFiveValues(string s)
 {
  var t=string.Join(" ",Lesson().GetProperty("ExplanationParagraphs").EnumerateArray().Select(x=>x.GetString()!));
  Assert.Contains(s,t);
 }
 [Fact]
 public void OriginalEstimateCardsRemainPreserved()
 {
  var t=string.Join(" ",Lesson().GetProperty("ExplanationParagraphs").EnumerateArray().Select(x=>x.GetString()!));
  Assert.Contains("Clare about2 liters",t);
  Assert.Contains("Jada about20 liters",t);
  Assert.Contains("Kiran about200 liters",t);
  Assert.Contains("500L",t);
 }
}
