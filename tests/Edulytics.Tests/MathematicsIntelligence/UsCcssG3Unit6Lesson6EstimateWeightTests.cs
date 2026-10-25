using System;
using System.Linq;
using System.Text.Json;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>Original weight-source structure, authored tests only.</summary>
public sealed class UsCcssG3Unit6Lesson6EstimateWeightTests
{
 private static JsonElement Lesson()
 {
  using var s=typeof(Edulytics.Data.Seeding.CommonCoreG3SourceTeachingCorrections).Assembly.GetManifestResourceStream("Edulytics.Data.Grade3.Unit6Lesson6RichV2");
  Assert.NotNull(s);using var document=JsonDocument.Parse(s!);
  Assert.Equal("g3-u06-l06-2021-original-five-weight-benchmarks-four-pets-open-estimates-v1",document.RootElement.GetProperty("ContentVersion").GetString());
  var ls=document.RootElement.GetProperty("Lessons");Assert.Equal(1,ls.GetArrayLength());
  Assert.Equal("PED:US-CCSS-MATH:G3:U06:L06",ls[0].GetProperty("LessonCode").GetString());
  return ls[0].Clone();
 }
 [Theory]
 [InlineData("1 gram")][InlineData("10 grams")][InlineData("100 grams")]
 [InlineData("1 kilogram")][InlineData("2 kilograms")]
 public void OriginalFivePhysicalBenchmarkWeightsRemainInSource(string benchmark)
 {
  var p=string.Join(" ",Lesson().GetProperty("ExplanationParagraphs").EnumerateArray().Select(x=>x.GetString()!));
  Assert.Contains(benchmark,p);
 }
 [Theory]
 [InlineData("guinea pig","1 kilogram")]
 [InlineData("dog","20 kilograms")]
 [InlineData("small fish","3 grams")]
 [InlineData("cat","3 kilograms")]
 public void SourcePetWeightCardRemainsOnePlausibleAssignment(string pet,string choice)
 {
  var p=string.Join(" ",Lesson().GetProperty("ExplanationParagraphs").EnumerateArray().Select(x=>x.GetString()!));
  Assert.Contains(pet,p,StringComparison.OrdinalIgnoreCase);
  Assert.Contains(choice,p);
  Assert.Contains("plausible",p,StringComparison.OrdinalIgnoreCase);
 }
 [Fact]
 public void FourOriginalSortingBandsAreOpenEstimations()
 {
  var p=string.Join(" ",Lesson().GetProperty("ExplanationParagraphs").EnumerateArray().Select(x=>x.GetString()!));
  Assert.Contains("LESS THAN 1 gram",p);
  Assert.Contains("BETWEEN 1 gram and 100 grams",p);
  Assert.Contains("MORE THAN1 kilogram",p);
  Assert.Contains("optional",p,StringComparison.OrdinalIgnoreCase);
 }
}
