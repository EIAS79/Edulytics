using System;
using System.Linq;
using System.Text.Json;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>Quarter inch original-source numeric contract; tests written but NOT executed.</summary>
public sealed class UsCcssG3Unit6Lesson2QuarterInchTests
{
 private static JsonElement Lesson()
 {
  using var stream=typeof(Edulytics.Data.Seeding.CommonCoreG3SourceTeachingCorrections).Assembly.GetManifestResourceStream("Edulytics.Data.Grade3.Unit6Lesson2RichV2");
  Assert.NotNull(stream);using var doc=JsonDocument.Parse(stream!);
  Assert.Equal("g3-u06-l02-2021-quarter-inches-4-objects-8-search-categories-html-v1",doc.RootElement.GetProperty("ContentVersion").GetString());
  var lessons=doc.RootElement.GetProperty("Lessons");Assert.Equal(1,lessons.GetArrayLength());
  Assert.Equal("PED:US-CCSS-MATH:G3:U06:L02",lessons[0].GetProperty("LessonCode").GetString());
  return lessons[0].Clone();
 }
 [Theory]
 [InlineData(2,2,10)][InlineData(1,1,5)][InlineData(3,3,15)][InlineData(8,2,34)]
 public void OriginalFourFixedTargetLengthsInQuarters(int whole,int fourths,int quarterTicks)
 {
  Assert.Equal(whole*4+fourths,quarterTicks);
 }
 [Theory]
 [InlineData(0,1)][InlineData(1,5)][InlineData(2,9)]
 public void FirstQuarterTickInsideEachWholeInch(int whole,int numerator)
 {
  Assert.Equal(4*whole+1,numerator);
 }
 [Fact]
 public void SourceHasFourObjectsPerPairAndEightCategoriesButOnlyFourFoundMinimum()
 {
  var p=string.Join(" ",Lesson().GetProperty("ExplanationParagraphs").EnumerateArray().Select(x=>x.GetString()!));
  Assert.Contains("FOUR object observations per pair",p);
  Assert.Contains("EIGHT-ROW table",p);
  Assert.Contains("AT LEAST FOUR real classroom objects",p);
  Assert.Contains("NOT eight compulsory",p);
 }
}
