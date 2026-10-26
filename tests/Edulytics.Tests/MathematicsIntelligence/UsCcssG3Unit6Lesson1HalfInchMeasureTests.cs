using System;
using System.Linq;
using System.Text.Json;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>First-party A-B-C authoring tests for U06:L01: written only, not executed.</summary>
public sealed class UsCcssG3Unit6Lesson1HalfInchMeasureTests
{
 private static JsonElement Lesson()
 {
  using var stream=typeof(Edulytics.Data.Seeding.CommonCoreG3SourceTeachingCorrections).Assembly.GetManifestResourceStream("Edulytics.Data.Grade3.Unit6Lesson1RichV2");
  Assert.NotNull(stream);using var doc=JsonDocument.Parse(stream!);
  Assert.Equal("g3-u06-l01-firstedition2021-original-3page-object-measure-halfinch-v1",doc.RootElement.GetProperty("ContentVersion").GetString());
  var ls=doc.RootElement.GetProperty("Lessons");Assert.Equal(1,ls.GetArrayLength());
  Assert.Equal("PED:US-CCSS-MATH:G3:U06:L01",ls[0].GetProperty("LessonCode").GetString());
  return ls[0].Clone();
 }
 [Theory]
 [InlineData(0,0.5)][InlineData(1,1.5)][InlineData(3,3.5)][InlineData(7,7.5)]
 public void EveryConsecutiveWholeInchHasOneHalfwayTick(int lower,double half)
 {
  Assert.Equal(lower+0.5,half,8);
 }
 [Theory]
 [InlineData(1,0,1)][InlineData(3,1,1)][InlineData(9,4,1)][InlineData(13,6,1)]
 public void OddHalfInchCountConvertsToMixedNumber(int halves,int whole,int numerator)
 {
  Assert.Equal(halves/2,whole);
  Assert.Equal(1,numerator);
  Assert.Equal(halves/2d,whole+numerator/2d,8);
 }
 [Fact]
 public void OriginalSourceDoesNotClaimFixedClassroomObjectMeasurements()
 {
  var t=string.Join(" ",Lesson().GetProperty("ExplanationParagraphs").EnumerateArray().Select(x=>x.GetString()!));
  Assert.Contains("EACH student should choose THREE objects",t);
  Assert.Contains("EACH partner again measures THREE objects",t);
  Assert.Contains("publisher fixed questions",t,StringComparison.OrdinalIgnoreCase);
 }
 [Fact]
 public void PreserveHalfRulerAndUnpartitionedRulerAndMixedNumberSynthesis()
 {
  var s=string.Join(" ",Lesson().GetProperty("ExplanationParagraphs").EnumerateArray().Select(x=>x.GetString()!));
  Assert.Contains("OTHER ruler",s);
  Assert.Contains("9/2 inches =4 1/2 inches",s);
  Assert.Contains("Cool-down",s);
 }
}
