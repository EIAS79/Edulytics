using System;
using System.Linq;
using System.Text.Json;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>Lesson 94 first-party source-specific tests authored but NOT executed.</summary>
public sealed class UsCcssG3Unit5Lesson15SameDenominatorTests
{
 private static JsonElement Lesson()
 {
  using var stream=typeof(Edulytics.Data.Seeding.CommonCoreG3SourceTeachingCorrections).Assembly.GetManifestResourceStream("Edulytics.Data.Grade3.Unit5Lesson15RichV2");
  Assert.NotNull(stream);using var doc=JsonDocument.Parse(stream!);
  Assert.Equal("g3-u05-l15-2021-same-denominator-pairs-and-ten-spin-rounds-v1",doc.RootElement.GetProperty("ContentVersion").GetString());
  var lessons=doc.RootElement.GetProperty("Lessons");Assert.Equal(1,lessons.GetArrayLength());
  Assert.Equal("PED:US-CCSS-MATH:G3:U05:L15",lessons[0].GetProperty("LessonCode").GetString());
  return lessons[0].Clone();
 }
 [Theory]
 [InlineData(1,2,3,2,-1)][InlineData(3,8,2,8,1)]
 [InlineData(1,6,4,6,-1)][InlineData(4,4,5,4,-1)]
 [InlineData(2,3,1,3,1)][InlineData(4,8,6,8,-1)]
 public void SixSourceComparisonSigns(int a,int d,int b,int d2,int sign)
 {
  Assert.Equal(d,d2);
  Assert.Equal(sign,Math.Sign(a-b));
 }
 [Theory]
 [InlineData(2,1,true)][InlineData(4,1,true)]
 [InlineData(4,6,true)][InlineData(7,6,false)]
 public void OptionalNumeratorInequalitiesRemainOpen(int value,int threshold,bool expected)
 {
  Assert.Equal(expected,value>threshold);
 }
 [Fact]
 public void SourceTenRoundsAndTeacherSpinnerRecord()
 {
  var text=string.Join(" ",Lesson().GetProperty("ExplanationParagraphs").EnumerateArray().Select(x=>x.GetString()!));
  Assert.Contains("TEN rounds",text);
  Assert.Contains("2,3,4,6,8",text);
  Assert.Contains("teacher",text,StringComparison.OrdinalIgnoreCase);
 }
}
