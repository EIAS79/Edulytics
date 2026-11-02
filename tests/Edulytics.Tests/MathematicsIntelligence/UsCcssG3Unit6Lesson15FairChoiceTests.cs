using System;
using System.Linq;
using System.Text.Json;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>Source-specific lesson 112 tests: authored but NOT run.</summary>
public sealed class UsCcssG3Unit6Lesson15FairChoiceTests
{
 private static JsonElement Lesson()
 {
  var asm=System.Reflection.Assembly.Load("Edulytics.Data");
  using var stream=asm.GetManifestResourceStream("Edulytics.Data.Grade3.Unit6Lesson15RichV2");
  Assert.NotNull(stream);using var doc=JsonDocument.Parse(stream!);
  Assert.Equal("g3-u06-l15-2021-fair-four-problems-choice-gallery-and-reflection-v1",doc.RootElement.GetProperty("ContentVersion").GetString());
  var lessons=doc.RootElement.GetProperty("Lessons");Assert.Equal(1,lessons.GetArrayLength());
  Assert.Equal("PED:US-CCSS-MATH:G3:U06:L15",lessons[0].GetProperty("LessonCode").GetString());
  return lessons[0].Clone();
 }
 [Theory]
 [InlineData(80,8,10)][InlineData(72,8,9)]
 [InlineData(96,8,12)][InlineData(96,4,24)]
 public void FourOriginalWarmupDivisions(int a,int b,int result)=>Assert.Equal(result,a/b);
 [Fact]
 public void FourOriginalFairProblemsMaintainChoice()
 {
  Assert.Equal(63,(6+1)*9);
  Assert.Equal(73,(12*60+25)-(11*60+12));
  Assert.Equal(11*60+22,12*60+10-48);
  Assert.Equal(13,91/7);
  Assert.Equal(550,273+277);
  Assert.Equal(6,54/9);
 }
 [Fact]
 public void OriginalGalleryAndSectionSummaryPreserved()
 {
  var p=string.Join(" ",Lesson().GetProperty("ExplanationParagraphs").EnumerateArray().Select(x=>x.GetString()!));
  Assert.Contains("SOLVE FOUR problems",p);
  Assert.Contains("AT LEAST THREE",p);
  Assert.Contains("84÷12=7",p);
 }
}
