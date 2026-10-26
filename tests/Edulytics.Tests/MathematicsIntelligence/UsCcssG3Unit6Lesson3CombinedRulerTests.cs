using System;
using System.Linq;
using System.Text.Json;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>Original 2021 combined ruler tests authored only, no .NET run.</summary>
public sealed class UsCcssG3Unit6Lesson3CombinedRulerTests
{
 private static JsonElement Lesson()
 {
  using var stream=typeof(Edulytics.Data.Seeding.CommonCoreG3SourceTeachingCorrections).Assembly.GetManifestResourceStream("Edulytics.Data.Grade3.Unit6Lesson3RichV2");
  Assert.NotNull(stream);using var doc=JsonDocument.Parse(stream!);
  Assert.Equal("g3-u06-l03-2021-kiran-jada-worm-A-D-4objects-fraction-equivalence-v1",doc.RootElement.GetProperty("ContentVersion").GetString());
  var ls=doc.RootElement.GetProperty("Lessons");Assert.Equal(1,ls.GetArrayLength());
  Assert.Equal("PED:US-CCSS-MATH:G3:U06:L03",ls[0].GetProperty("LessonCode").GetString());
  return ls[0].Clone();
 }
 [Theory]
 [InlineData(2,4,1,2)]
 public void HalvesAndTwoFourthsMatch(int quarterNum,int quarterDen,int halfNum,int halfDen)
 {
  Assert.Equal((double)quarterNum/quarterDen,(double)halfNum/halfDen,8);
 }
 [Theory]
 [InlineData(4,8,2)][InlineData(4,16,4)]
 public void OriginalWormAEqualsFourWholeInches(int length,int numerator,int denominator)=>Assert.Equal(length,numerator/denominator);
 [Theory]
 [InlineData(7,2,14,4)]
 public void OriginalWormDEqualsThreeAndHalf(int a,int b,int c,int d)
 {
  Assert.Equal(3.5,a/(double)b,8);
  Assert.Equal(a/(double)b,c/(double)d,8);
 }
 [Fact]
 public void OriginalFourWormsAndTwoMeasuringTasksArePreserved()
 {
  var s=string.Join(" ",Lesson().GetProperty("ExplanationParagraphs").EnumerateArray().Select(x=>x.GetString()!));
  Assert.Contains("FOUR distinct ORIGINAL worm illustrations A,B,C,D",s);
  Assert.Contains("Worms B and C have no fixed textual measurements",s);
  Assert.Contains("AT LEAST ONE classroom object",s);
  Assert.Contains("AT LEAST THREE objects",s);
  Assert.Contains("TRADE",s);
 }
}
