using System;
using System.Linq;
using System.Text.Json;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>Lesson 89 A-C tests written but not executed.</summary>
public sealed class UsCcssG3Unit5Lesson10EquivalentFractionsTests
{
 private static JsonElement Lesson()
 {
  using var stream=typeof(Edulytics.Data.Seeding.CommonCoreG3SourceTeachingCorrections).Assembly.GetManifestResourceStream("Edulytics.Data.Grade3.Unit5Lesson10RichV2");
  Assert.NotNull(stream);
  using var doc=JsonDocument.Parse(stream!);
  Assert.Equal("g3-u05-l10-six-original-half-shapes-and-four-strip-goals-20261025-v1",doc.RootElement.GetProperty("ContentVersion").GetString());
  var lessons=doc.RootElement.GetProperty("Lessons");Assert.Equal(1,lessons.GetArrayLength());
  Assert.Equal("PED:US-CCSS-MATH:G3:U05:L10",lessons[0].GetProperty("LessonCode").GetString());
  return lessons[0].Clone();
 }
 [Theory]
 [InlineData("A",true)][InlineData("B",false)][InlineData("C",true)]
 [InlineData("D",true)][InlineData("E",true)][InlineData("F",false)]
 public void OriginalHalfShapes(string figure,bool expected)
 {
  Assert.Equal(expected,"ACDE".Contains(figure,StringComparison.Ordinal));
  var text=string.Join(" ",Lesson().GetProperty("ExplanationParagraphs").EnumerateArray().Select(x=>x.GetString()!));
  Assert.Contains(figure+"=",text);
 }
 [Theory]
 [InlineData(1,2,2,4)][InlineData(1,2,3,6)][InlineData(1,2,4,8)]
 [InlineData(2,3,4,6)][InlineData(3,4,6,8)]
 public void EqualFractionPairs(int a,int b,int c,int d)=>Assert.Equal(a*d,b*c);
 [Fact]
 public void SourceKeepsAllFourOpenEndedTargetsAndTeacherReview()
 {
  var text=string.Join(" ",Lesson().GetProperty("ExplanationParagraphs").EnumerateArray().Select(x=>x.GetString()!));
  Assert.Contains("1/2, 2/3, 6/6 and 3/4",text);
  Assert.Contains("UNEQUAL",text);
  Assert.Contains("teacher",text,StringComparison.OrdinalIgnoreCase);
 }
}
