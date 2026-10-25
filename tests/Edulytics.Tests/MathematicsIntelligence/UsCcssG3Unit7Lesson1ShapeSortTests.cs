using System;
using System.Linq;
using System.Text.Json;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UsCcssG3Unit7Lesson1ShapeSortTests
{
 private static string Lesson()
 {
  var asm=System.Reflection.Assembly.Load("Edulytics.Data");
  using var st=asm.GetManifestResourceStream("Edulytics.Data.Grade3.Unit7Lesson1RichV2");
  Assert.NotNull(st);using var d=JsonDocument.Parse(st!);
  Assert.Equal("g3-u07-l01-2021-shape-sorts-angle-rightangle-O-BFMS-v1",d.RootElement.GetProperty("ContentVersion").GetString());
  var a=d.RootElement.GetProperty("Lessons");Assert.Equal(1,a.GetArrayLength());
  Assert.Equal("PED:US-CCSS-MATH:G3:U07:L01",a[0].GetProperty("LessonCode").GetString());
  return string.Join(" ",a[0].GetProperty("ExplanationParagraphs").EnumerateArray().Select(x=>x.GetString()!));
 }
 [Fact]public void SecretTwoCategorySortAndWarmupRemainOpen()
 {
  var t=Lesson();Assert.Contains("TWO NEW categories",t);
  Assert.Contains("SECRETLY",t);Assert.Contains("A,B,C,D",t);
 }
 [Fact]public void PreserveOriginalTextAngleClaimsWithoutImageInvention()
 {
  var t=Lesson();Assert.Contains("FOUR angles",t);
  Assert.Contains("B,F,M,S",t);Assert.Contains("source-pixel",t);
 }
}