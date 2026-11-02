using System;
using System.Linq;
using System.Text.Json;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UsCcssG3Unit7Lesson4ShapePropertiesTests
{
 private static string Lesson()
 {
  var asm=System.Reflection.Assembly.Load("Edulytics.Data");
  using var st=asm.GetManifestResourceStream("Edulytics.Data.Grade3.Unit7Lesson4RichV2");
  Assert.NotNull(st);using var d=JsonDocument.Parse(st!);
  Assert.Equal("g3-u07-l04-2021-righttriangles-rectangle-rhombus-square-source-image-review-v1",d.RootElement.GetProperty("ContentVersion").GetString());
  var ls=d.RootElement.GetProperty("Lessons");Assert.Equal(1,ls.GetArrayLength());
  Assert.Equal("PED:US-CCSS-MATH:G3:U07:L04",ls[0].GetProperty("LessonCode").GetString());
  return string.Join(" ",ls[0].GetProperty("ExplanationParagraphs").EnumerateArray().Select(x=>x.GetString()!));
 }
 [Fact]public void PreserveSourceKnownWarmupAndTriangleE()
 {
  var text=Lesson();Assert.Contains("A, B and D",text);Assert.Contains("E in this set",text);
 }
 [Fact]public void ShapeDefinitionsAndImageGate()
 {
  var text=Lesson();Assert.Contains("both rectangle and rhombus",text);
  Assert.Contains("not been pixel",text,StringComparison.OrdinalIgnoreCase);
 }
}