using System;
using System.Linq;
using System.Text.Json;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>Source-specific lesson 93 tests, written but not executed.</summary>
public sealed class UsCcssG3Unit5Lesson14FractionComparisonTests
{
 private static JsonElement Lesson()
 {
  using var stream=typeof(Edulytics.Data.Seeding.CommonCoreG3SourceTeachingCorrections).Assembly.GetManifestResourceStream("Edulytics.Data.Grade3.Unit5Lesson14RichV2");
  Assert.NotNull(stream);
  using var doc=JsonDocument.Parse(stream!);
  Assert.Equal("g3-u05-l14-2021-fraction-comparison-common-whole-han-lin-v1",doc.RootElement.GetProperty("ContentVersion").GetString());
  var a=doc.RootElement.GetProperty("Lessons");Assert.Equal(1,a.GetArrayLength());
  Assert.Equal("PED:US-CCSS-MATH:G3:U05:L14",a[0].GetProperty("LessonCode").GetString());
  return a[0].Clone();
 }
 [Theory]
 [InlineData(16,1,16)][InlineData(16,2,8)][InlineData(16,4,4)][InlineData(20,4,5)]
 public void OriginalWholeNumberWarmup(int numerator,int denominator,int expected)=>Assert.Equal(expected,numerator/denominator);
 [Theory]
 [InlineData(1,2,1,3,false)][InlineData(4,6,5,6,false)]
 [InlineData(3,4,6,8,true)]
 public void SourceThreeEquivalentChecks(int a,int b,int c,int d,bool eq)=>Assert.Equal(eq,a*d==b*c);
 [Fact]
 public void LinScaleTrapNeedsSameWholeAndTeacherReasoning()
 {
  var s=string.Join(" ",Lesson().GetProperty("ExplanationParagraphs").EnumerateArray().Select(x=>x.GetString()!));
  Assert.Contains("DIFFERENT unit spans",s);
  Assert.Contains("4/6<5/6",s);
  Assert.Contains("teacher",s,StringComparison.OrdinalIgnoreCase);
 }
}
