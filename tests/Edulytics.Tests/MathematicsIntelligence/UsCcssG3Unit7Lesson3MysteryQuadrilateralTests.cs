using System;
using System.Linq;
using System.Text.Json;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UsCcssG3Unit7Lesson3MysteryQuadrilateralTests
{
 private static string Lesson()
 {
  var asm=System.Reflection.Assembly.Load("Edulytics.Data");
  using var st=asm.GetManifestResourceStream("Edulytics.Data.Grade3.Unit7Lesson3RichV2");
  Assert.NotNull(st);using var d=JsonDocument.Parse(st!);
  Assert.Equal("g3-u07-l03-2021-mystery-quadrilateral-five-questions-switchroles-v1",d.RootElement.GetProperty("ContentVersion").GetString());
  var l=d.RootElement.GetProperty("Lessons");Assert.Equal(1,l.GetArrayLength());
  Assert.Equal("PED:US-CCSS-MATH:G3:U07:L03",l[0].GetProperty("LessonCode").GetString());
  return string.Join(" ",l[0].GetProperty("ExplanationParagraphs").EnumerateArray().Select(x=>x.GetString()!));
 }
 [Theory][InlineData(4,160)][InlineData(8,320)][InlineData(7,280)][InlineData(9,360)]
 public void OriginalProducts(int n,int x)=>Assert.Equal(x,n*40);
 [Fact]public void LimitFiveGuessRevealSwitchRole()
 {
  var p=Lesson();Assert.Contains("NO MORE THAN FIVE",p);
  Assert.Contains("SWITCH ROLES",p);Assert.Contains("guess",p,StringComparison.OrdinalIgnoreCase);
  Assert.Contains("Teacher",p);Assert.Contains("source blackline",p);
 }
}