using System;
using System.Linq;
using System.Text.Json;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>Original lesson 95 same numerator tests: written but unexecuted.</summary>
public sealed class UsCcssG3Unit5Lesson16SameNumeratorTests
{
 private static JsonElement Lesson()
 {
  using var stream=typeof(Edulytics.Data.Seeding.CommonCoreG3SourceTeachingCorrections).Assembly.GetManifestResourceStream("Edulytics.Data.Grade3.Unit5Lesson16RichV2");
  Assert.NotNull(stream);using var doc=JsonDocument.Parse(stream!);
  Assert.Equal("g3-u05-l16-2021-same-numerator-five-over-two-through-eight-v1",doc.RootElement.GetProperty("ContentVersion").GetString());
  var ls=doc.RootElement.GetProperty("Lessons");Assert.Equal(1,ls.GetArrayLength());
  Assert.Equal("PED:US-CCSS-MATH:G3:U05:L16",ls[0].GetProperty("LessonCode").GetString());
  return ls[0].Clone();
 }
 [Theory]
 [InlineData(2,4,true)][InlineData(4,3,false)][InlineData(6,8,true)]
 public void OriginalTrueFalseUnitFractions(int leftDen,int rightDen,bool expected)
 {
  Assert.Equal(expected,leftDen<rightDen);
 }
 [Theory]
 [InlineData(1,4,3,-1)][InlineData(3,4,8,1)]
 [InlineData(5,3,6,1)][InlineData(9,8,6,-1)]
 [InlineData(2,2,6,1)][InlineData(4,3,8,1)]
 [InlineData(8,8,4,-1)][InlineData(5,4,3,-1)]
 public void OriginalEightPairsWithSameNumerator(int numerator,int firstDen,int secondDen,int sign)
 {
  Assert.True(numerator>0);
  Assert.Equal(sign,Math.Sign(secondDen-firstDen));
 }
 [Theory]
 [InlineData(2,3,true)][InlineData(3,3,false)]
 [InlineData(5,4,true)][InlineData(4,4,false)]
 public void OpenDenominatorConstraintsExamples(int chosen,int threshold,bool expected)
 {
  Assert.Equal(expected,chosen!=threshold);
 }
 [Fact]
 public void PriyaCorrectAndAllFiveOriginalFractionsPresent()
 {
  var text=string.Join(" ",Lesson().GetProperty("ExplanationParagraphs").EnumerateArray().Select(x=>x.GetString()!));
  Assert.Contains("Priya is correct",text);
  Assert.Contains("5/2,5/3,5/4,5/6,5/8",text);
  Assert.Contains("multiple legitimate",text);
 }
}
