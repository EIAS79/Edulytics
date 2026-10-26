using System;
using System.Linq;
using System.Text.Json;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UsCcssG3Unit6Lesson13InfoGapTests
{
 private static JsonElement Lesson()
 {
  using var st=System.Reflection.Assembly.Load("Edulytics.Data").GetManifestResourceStream("Edulytics.Data.Grade3.Unit6Lesson13RichV2");
  Assert.NotNull(st);using var d=JsonDocument.Parse(st!);
  Assert.Equal("g3-u06-l13-2021-cantaloupe-344kg-two-info-gap-card-pairs-v1",d.RootElement.GetProperty("ContentVersion").GetString());
  var l=d.RootElement.GetProperty("Lessons");Assert.Equal("PED:US-CCSS-MATH:G3:U06:L13",l[0].GetProperty("LessonCode").GetString());return l[0].Clone();
 }
 [Fact]
 public void SourceActualCantaloupeDoesNotBecomeFixedEstimate()
 {
  var t=string.Join(" ",Lesson().GetProperty("ExplanationParagraphs").EnumerateArray().Select(x=>x.GetString()!));
  Assert.Contains("344 KILOGRAMS",t);
  Assert.Contains("NOT",t);
  Assert.Contains("source card",t,StringComparison.OrdinalIgnoreCase);
 }
 [Fact]
 public void AllFourHiddenCardSetsAndRoleReversalPreserved()
 {
  var t=string.Join(" ",Lesson().GetProperty("ExplanationParagraphs").EnumerateArray().Select(x=>x.GetString()!));
  Assert.Contains("TWO original matched sets",t);
  Assert.Contains("TWO NEW publisher card sets",t);
  Assert.Contains("SWITCH ROLES",t);
 }
}