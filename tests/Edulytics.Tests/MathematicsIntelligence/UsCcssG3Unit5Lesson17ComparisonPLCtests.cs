using System;
using System.Linq;
using System.Text.Json;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>Source lesson 96 numeric and open task checks; written not executed.</summary>
public sealed class UsCcssG3Unit5Lesson17ComparisonPLCtests
{
 private static JsonElement Lesson()
 {
  using var stream=typeof(Edulytics.Data.Seeding.CommonCoreG3SourceTeachingCorrections).Assembly.GetManifestResourceStream("Edulytics.Data.Grade3.Unit5Lesson17RichV2");
  Assert.NotNull(stream);using var doc=JsonDocument.Parse(stream!);
  Assert.Equal("g3-u05-l17-2021-insect-four-comparisons-spilled-juice-and-optional-eleven-v1",doc.RootElement.GetProperty("ContentVersion").GetString());
  var ls=doc.RootElement.GetProperty("Lessons");Assert.Equal(1,ls.GetArrayLength());
  Assert.Equal("PED:US-CCSS-MATH:G3:U05:L17",ls[0].GetProperty("LessonCode").GetString());
  return ls[0].Clone();
 }
 [Theory]
 [InlineData(2,8,2,3,-1)][InlineData(4,1,12,3,0)]
 [InlineData(3,8,5,8,-1)][InlineData(5,8,5,6,-1)]
 public void OriginalFourInsectComparisons(int a,int b,int c,int d,int expected)
 {
  Assert.Equal(expected,Math.Sign(a*d-c*b));
 }
 [Theory]
 [InlineData(3,8,true)][InlineData(2,8,false)]
 public void SourceSpilledJuiceNumerator(int n,int denominator,bool valid)
 {
  Assert.Equal(8,denominator);
  Assert.Equal(valid,n>2);
 }
 [Fact]
 public void PreserveOptionalElevenAndThreeSpilledJuiceFamilies()
 {
  var p=string.Join(" ",Lesson().GetProperty("ExplanationParagraphs").EnumerateArray().Select(x=>x.GetString()!));
  Assert.Contains("ALL ELEVEN",p);
  Assert.Contains("2/8 < n/8",p);
  Assert.Contains("3/6 = an unknown",p);
  Assert.Contains("4/3 >4/d",p);
  Assert.Contains("OPTIONAL",p);
 }
}
