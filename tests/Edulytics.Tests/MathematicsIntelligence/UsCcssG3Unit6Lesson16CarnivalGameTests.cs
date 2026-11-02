using System;
using System.Linq;
using System.Text.Json;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UsCcssG3Unit6Lesson16CarnivalGameTests
{
 private static string OriginalLessonText()
 {
  var asm=System.Reflection.Assembly.Load("Edulytics.Data");
  using var stream=asm.GetManifestResourceStream("Edulytics.Data.Grade3.Unit6Lesson16RichV2");
  Assert.NotNull(stream);
  using var d=JsonDocument.Parse(stream!);
  Assert.Equal("g3-u06-l16-2021-optional-game-two-skills-teacher-assessed-v1",d.RootElement.GetProperty("ContentVersion").GetString());
  var l=d.RootElement.GetProperty("Lessons");
  Assert.Equal(1,l.GetArrayLength());
  Assert.Equal("PED:US-CCSS-MATH:G3:U06:L16",l[0].GetProperty("LessonCode").GetString());
  return string.Join(" ",l[0].GetProperty("ExplanationParagraphs").EnumerateArray().Select(x=>x.GetString()!));
 }
 [Fact]public void SourceRequiresOnePlayTwoSkillsAndGroupsOfFour()
 {
  var t=OriginalLessonText();Assert.Contains("groups of FOUR",t);
  Assert.Contains("AT LEAST ONCE",t);Assert.Contains("AT LEAST TWO",t);
 }
 [Fact]public void SourceRemainsOptionalAndOutcomeOpen()
 {
  var t=OriginalLessonText();Assert.Contains("OPTIONAL",t);
  Assert.Contains("WITHIN100",t);Assert.Contains("WITHIN1,000",t);
  Assert.Contains("no fixed correct combination",t);
 }
}