using System;
using System.Linq;
using System.Text.Json;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UsCcssG3Unit6Lesson12PumpkinMeasurementTests
{
 private static JsonElement Lesson()
 {
  using var s=System.Reflection.Assembly.Load("Edulytics.Data").GetManifestResourceStream("Edulytics.Data.Grade3.Unit6Lesson12RichV2");
  Assert.NotNull(s);using var d=JsonDocument.Parse(s!);
  Assert.Equal("g3-u06-l12-2021-giant-pumpkins-276-347-and-card-C-I-v1",d.RootElement.GetProperty("ContentVersion").GetString());
  var l=d.RootElement.GetProperty("Lessons");Assert.Equal("PED:US-CCSS-MATH:G3:U06:L12",l[0].GetProperty("LessonCode").GetString());return l[0].Clone();
 }
 [Fact]
 public void ExactOriginalPumpkinsAndSourceCardFacts()
 {
  Assert.Equal(71,347-276);
  Assert.Equal(623,347+276);
  Assert.Equal(12,84/7);
  var txt=string.Join(" ",Lesson().GetProperty("ExplanationParagraphs").EnumerateArray().Select(x=>x.GetString()!));
  Assert.Contains("diagram C ↔ situation I",txt);
  Assert.Contains("diagram B ↔ situation E",txt);
  Assert.Contains("blackline master",txt,StringComparison.OrdinalIgnoreCase);
 }
}