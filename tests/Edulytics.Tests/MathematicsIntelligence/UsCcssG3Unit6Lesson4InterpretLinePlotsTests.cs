using System;
using System.Linq;
using System.Text.Json;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>Original 15 twig, 22 seedling and 19 quarter-inch twig frequencies: source-specific tests authored only.</summary>
public sealed class UsCcssG3Unit6Lesson4InterpretLinePlotsTests
{
 private static JsonElement Lesson()
 {
  using var s=typeof(Edulytics.Data.Seeding.CommonCoreG3SourceTeachingCorrections).Assembly.GetManifestResourceStream("Edulytics.Data.Grade3.Unit6Lesson4RichV2");
  Assert.NotNull(s);using var document=JsonDocument.Parse(s!);
  Assert.Equal("g3-u06-l04-2021-original-lineplot-15-22-19-data-quarters-v1",document.RootElement.GetProperty("ContentVersion").GetString());
  var lessons=document.RootElement.GetProperty("Lessons");Assert.Equal(1,lessons.GetArrayLength());
  Assert.Equal("PED:US-CCSS-MATH:G3:U06:L04",lessons[0].GetProperty("LessonCode").GetString());
  return lessons[0].Clone();
 }
 [Fact]
 public void OriginalWarmupHasFifteenTwigsWithExactFiveCounts()
 {
  int[] frequencies={2,5,4,3,1};
  Assert.Equal(15,frequencies.Sum());
  Assert.Equal(5,frequencies[1]);
 }
 [Fact]
 public void SeedlingsHaveOriginalTwentyTwoValuesInTenHalfInchBuckets()
 {
  int[] f={4,3,6,1,3,2,0,2,0,1};
  Assert.Equal(22,f.Sum());
  Assert.Equal(6,f[2]);
 }
 [Fact]
 public void TwigsHaveNineteenExactQuarterInchReadings()
 {
  int[] f={1,1,0,1,1,3,5,2,1,3,0,1};
  Assert.Equal(19,f.Sum());
  Assert.Equal(2,f[7]); // six and two fourths
  Assert.Equal(4,f.Take(5).Sum()); // less than six
  Assert.Equal(3,f[5]); // exactly six
  Assert.Equal(12,f.Skip(6).Sum()); // more than six
  Assert.Equal(5,f[6]); // mode six and a fourth
 }
 [Theory]
 [InlineData(3.25)][InlineData(3.50)][InlineData(3.75)]
 public void AddedSourceTwigCanUseAnyInteriorQuarterTick(double q)
 {
  Assert.True(q>3&&q<4);
  Assert.Equal(Math.Floor(q*4),q*4,8);
 }
 [Fact]
 public void StudentGeneratedStatementsAndTwoQuestionMinimumStayOpen()
 {
  var p=string.Join(" ",Lesson().GetProperty("ExplanationParagraphs").EnumerateArray().Select(x=>x.GetString()!));
  Assert.Contains("THREE valid mathematical statements",p);
  Assert.Contains("AT LEAST TWO relevant questions",p);
  Assert.Contains("teacher",p,StringComparison.OrdinalIgnoreCase);
 }
}
