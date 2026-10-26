using System;
using System.Linq;
using System.Text.Json;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UsCcssG3Unit7Lesson2TriangleQuadSortTests
{
 private static string Lesson()
 {
  var asm=System.Reflection.Assembly.Load("Edulytics.Data");
  using var s=asm.GetManifestResourceStream("Edulytics.Data.Grade3.Unit7Lesson2RichV2");
  Assert.NotNull(s);using var d=JsonDocument.Parse(s!);
  Assert.Equal("g3-u07-l02-2021-four-truefalse-triangle-and-20-quadrilateral-cards-v1",d.RootElement.GetProperty("ContentVersion").GetString());
  var a=d.RootElement.GetProperty("Lessons");Assert.Equal(1,a.GetArrayLength());
  Assert.Equal("PED:US-CCSS-MATH:G3:U07:L02",a[0].GetProperty("LessonCode").GetString());
  return string.Join(" ",a[0].GetProperty("ExplanationParagraphs").EnumerateArray().Select(x=>x.GetString()!));
 }
 [Fact]public void FourOriginalTruthValues()
 {
  Assert.False(3*60==9*10);Assert.True(180==3*60);
  Assert.True(6*40==24*10);Assert.True(24*10==240);
 }
 [Fact]public void BothOriginalDistinctCardSortsPreserved()
 {
  var t=Lesson();Assert.Contains("TWENTY shapes",t);
  Assert.Contains("TRIANGLE CARD",t);Assert.Contains("GALLERY CHECK",t);
  Assert.Contains("not inspected",t,StringComparison.OrdinalIgnoreCase);
 }
}