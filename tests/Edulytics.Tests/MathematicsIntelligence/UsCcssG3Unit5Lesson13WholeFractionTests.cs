using System;
using System.Linq;
using System.Text.Json;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>Original source whole-number fraction checks; written not executed.</summary>
public sealed class UsCcssG3Unit5Lesson13WholeFractionTests
{
 private static JsonElement Lesson()
 {
  using var stream=typeof(Edulytics.Data.Seeding.CommonCoreG3SourceTeachingCorrections).Assembly.GetManifestResourceStream("Edulytics.Data.Grade3.Unit5Lesson13RichV2");
  Assert.NotNull(stream);
  using var doc=JsonDocument.Parse(stream!);
  Assert.Equal("g3-u05-l13-2021-four-numberlines-six-whole-checks-three-column-table-v1",doc.RootElement.GetProperty("ContentVersion").GetString());
  var ls=doc.RootElement.GetProperty("Lessons");Assert.Equal(1,ls.GetArrayLength());
  Assert.Equal("PED:US-CCSS-MATH:G3:U05:L13",ls[0].GetProperty("LessonCode").GetString());
  return ls[0].Clone();
 }
 [Theory]
 [InlineData(11,2,false)][InlineData(5,1,true)][InlineData(12,6,true)]
 [InlineData(10,3,false)][InlineData(12,8,false)][InlineData(16,4,true)]
 public void OriginalSixChecks(int n,int d,bool isWhole)
 {
  Assert.Equal(isWhole,n%d==0);
  var text=string.Join(" ",Lesson().GetProperty("ExplanationParagraphs").EnumerateArray().Select(x=>x.GetString()!));
  Assert.Contains(n+"/"+d,text);
 }
 [Theory]
 [InlineData(1,2,3,4)][InlineData(2,4,6,8)][InlineData(3,6,9,12)]
 public void FourOriginalAlignedNumberLines(int whole,int halves,int thirds,int fourths)
 {
  Assert.Equal(whole,halves/2);
  Assert.Equal(whole,thirds/3);
  Assert.Equal(whole,fourths/4);
 }
 [Theory]
 [InlineData(4,4,1)][InlineData(5,30,6)][InlineData(6,48,8)]
 public void SourceTableOriginalAnchorCells(int whole,int n,int d)=>Assert.Equal(whole,n/d);
}
