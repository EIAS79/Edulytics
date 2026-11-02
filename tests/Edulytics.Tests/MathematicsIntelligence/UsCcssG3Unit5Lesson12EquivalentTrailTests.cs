using System;
using System.Linq;
using System.Text.Json;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>2021 trail and dice source-specific checks, authored not executed.</summary>
public sealed class UsCcssG3Unit5Lesson12EquivalentTrailTests
{
 private static JsonElement Lesson()
 {
  using var stream=typeof(Edulytics.Data.Seeding.CommonCoreG3SourceTeachingCorrections).Assembly.GetManifestResourceStream("Edulytics.Data.Grade3.Unit5Lesson12RichV2");
  Assert.NotNull(stream);
  using var doc=JsonDocument.Parse(stream!);
  Assert.Equal("g3-u05-l12-firstedition2021-running-trail-four-pairs-eight-dice-rounds-20261025-v1",doc.RootElement.GetProperty("ContentVersion").GetString());
  var ls=doc.RootElement.GetProperty("Lessons");Assert.Equal(1,ls.GetArrayLength());
  Assert.Equal("PED:US-CCSS-MATH:G3:U05:L12",ls[0].GetProperty("LessonCode").GetString());
  return ls[0].Clone();
 }
 [Theory]
 [InlineData(3,6,1,2,true)]
 [InlineData(1,4,2,8,true)]
 [InlineData(2,3,5,6,false)]
 public void OriginalTrailDistanceComparisons(int a,int b,int c,int d,bool equal)
 {
  Assert.Equal(equal,a*d==b*c);
 }
 [Theory]
 [InlineData(1,2,4,8)]
 [InlineData(1,3,2,6)]
 [InlineData(2,3,4,6)]
 [InlineData(3,4,6,8)]
 public void ExactlyFourSourceNumberLinePairs(int a,int b,int c,int d)
 {
  Assert.Equal(a*d,b*c);
  var all=string.Join(" ",Lesson().GetProperty("ExplanationParagraphs").EnumerateArray().Select(x=>x.GetString()!));
  Assert.Contains(a+"/"+b+"="+c+"/"+d,all);
 }
 [Fact]
 public void RollSourcePreservesSixDiceWildFiveEightRoundsAndTwoRerolls()
 {
  var all=string.Join(" ",Lesson().GetProperty("ExplanationParagraphs").EnumerateArray().Select(x=>x.GetString()!));
  Assert.Contains("SIX standard number cubes",all);
  Assert.Contains("FIVE",all);
  Assert.Contains("TWO re-rolls",all);
  Assert.Contains("EIGHT labelled rounds",all);
 }
}
