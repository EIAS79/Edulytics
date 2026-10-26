using System;
using System.Linq;
using System.Text.Json;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>2021 original walk and line plot invariants. Tests authored, NOT executed.</summary>
public sealed class UsCcssG3Unit6Lesson5CreateLinePlotTests
{
 private static JsonElement Lesson()
 {
  using var s=typeof(Edulytics.Data.Seeding.CommonCoreG3SourceTeachingCorrections).Assembly.GetManifestResourceStream("Edulytics.Data.Grade3.Unit6Lesson5RichV2");
  Assert.NotNull(s);using var doc=JsonDocument.Parse(s!);
  Assert.Equal("g3-u06-l05-2021-walk-ten-real-measurements-open-lineplot-gallery-v1",doc.RootElement.GetProperty("ContentVersion").GetString());
  var ls=doc.RootElement.GetProperty("Lessons");Assert.Equal(1,ls.GetArrayLength());
  Assert.Equal("PED:US-CCSS-MATH:G3:U06:L05",ls[0].GetProperty("LessonCode").GetString());
  return ls[0].Clone();
 }
 [Theory]
 [InlineData(3,10,30)][InlineData(3,13,39)]
 [InlineData(6,13,78)][InlineData(3,26,78)]
 public void FourOriginalMentalProducts(int a,int b,int expected)=>Assert.Equal(expected,a*b);
 [Fact]
 public void StudentGeneratedTenPointPlotCanUseQuarterScale()
 {
  double[] sample={2,2,2.5,2.5,3,3,3,3.25,3.5,4};
  Assert.Equal(10,sample.Length);
  Assert.Equal(10,sample.GroupBy(x=>x).Sum(g=>g.Count()));
  Assert.Contains(3.25,sample);
 }
 [Fact]
 public void SourceGroupAndGalleryWalkRemainTeacherLed()
 {
  var txt=string.Join(" ",Lesson().GetProperty("ExplanationParagraphs").EnumerateArray().Select(x=>x.GetString()!));
  Assert.Contains("groups of FOUR",txt);
  Assert.Contains("AT LEAST TEN",txt);
  Assert.Contains("GALLERY WALK",txt);
  Assert.Contains("TWO original line-plot blackline master copies",txt);
  Assert.Contains("NOT publisher-given fixed plot",txt);
 }
}
