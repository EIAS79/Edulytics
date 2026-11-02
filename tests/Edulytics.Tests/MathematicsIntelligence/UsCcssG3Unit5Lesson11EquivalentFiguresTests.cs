using System;
using System.Linq;
using System.Text.Json;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>2021 nine-image equivalence source tests, authored but not executed.</summary>
public sealed class UsCcssG3Unit5Lesson11EquivalentFiguresTests
{
 private static JsonElement Lesson()
 {
  using var stream=typeof(Edulytics.Data.Seeding.CommonCoreG3SourceTeachingCorrections).Assembly.GetManifestResourceStream("Edulytics.Data.Grade3.Unit5Lesson11RichV2");
  Assert.NotNull(stream);
  using var doc=JsonDocument.Parse(stream!);
  Assert.Equal("g3-u05-l11-original-nine-figure-equivalences-two-wholes-20261025-v1",doc.RootElement.GetProperty("ContentVersion").GetString());
  var ls=doc.RootElement.GetProperty("Lessons");Assert.Equal(1,ls.GetArrayLength());
  Assert.Equal("PED:US-CCSS-MATH:G3:U05:L11",ls[0].GetProperty("LessonCode").GetString());
  return ls[0].Clone();
 }
 [Theory]
 [InlineData(2,16)][InlineData(6,48)][InlineData(10,80)][InlineData(12,96)]
 public void OriginalFourProducts(int factor,int product)=>Assert.Equal(product,factor*8);
 [Theory]
 [InlineData(2,4,4,8)][InlineData(1,3,2,6)][InlineData(6,8,3,4)]
 [InlineData(6,6,2,2)][InlineData(4,6,2,3)][InlineData(2,8,1,4)]
 [InlineData(4,4,2,2)][InlineData(3,2,6,4)]
 public void SourceEquivalentFractions(int a,int b,int c,int d)=>Assert.Equal(a*d,b*c);
 [Fact]
 public void OriginalMultipleWholesStayThreeHalves()
 {
  var s=string.Join(" ",Lesson().GetProperty("ExplanationParagraphs").EnumerateArray().Select(x=>x.GetString()!));
  Assert.Contains("TWO equal-size whole rectangles",s);
  Assert.Contains("NOT 3/4",s);
  Assert.Contains("teacher",s,StringComparison.OrdinalIgnoreCase);
 }
}
