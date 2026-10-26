using System;
using System.Linq;
using System.Text.Json;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>Original lesson liquid-volume physical teaching contract. Tests authored, not executed.</summary>
public sealed class UsCcssG3Unit6Lesson7LiquidVolumeTests
{
 private static JsonElement Lesson()
 {
  using var s=typeof(Edulytics.Data.Seeding.CommonCoreG3SourceTeachingCorrections).Assembly.GetManifestResourceStream("Edulytics.Data.Grade3.Unit6Lesson7RichV2");
  Assert.NotNull(s);using var doc=JsonDocument.Parse(s!);
  Assert.Equal("g3-u06-l07-2021-bowl-jar-unit-container-liters-open-volume-v1",doc.RootElement.GetProperty("ContentVersion").GetString());
  var ls=doc.RootElement.GetProperty("Lessons");Assert.Equal(1,ls.GetArrayLength());
  Assert.Equal("PED:US-CCSS-MATH:G3:U06:L07",ls[0].GetProperty("LessonCode").GetString());
  return ls[0].Clone();
 }
 [Theory]
 [InlineData(1,1)][InlineData(2,2)][InlineData(4,4)]
 public void SuccessiveLiterPoursLabelCumulativeAmount(int fullLiterPours,int markedL)
 {
  Assert.Equal(fullLiterPours,markedL);
 }
 [Fact]
 public void OriginalSourceUsesOpenContainersOneSameUnitAndThreeEstimatesEach()
 {
  var text=string.Join(" ",Lesson().GetProperty("ExplanationParagraphs").EnumerateArray().Select(x=>x.GetString()!));
  Assert.Contains("A and B",text);
  Assert.Contains("ONE shared smaller",text);
  Assert.Contains("TOO LOW, ABOUT RIGHT, TOO HIGH",text);
  Assert.Contains("THREE values",text);
 }
 [Fact]
 public void SampleNineLitersIsNotFixedAnswer()
 {
  var text=string.Join(" ",Lesson().GetProperty("ExplanationParagraphs").EnumerateArray().Select(x=>x.GetString()!));
  Assert.Contains("SAMPLE RESPONSE",text);
  Assert.Contains("not a source guaranteed measurement",text);
  Assert.Contains("teacher",text,StringComparison.OrdinalIgnoreCase);
 }
}
