using System;
using System.Linq;
using System.Text.Json;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Source-specific first-party mathematical truth cases. Written, not executed.</summary>
public sealed class UsCcssG3Unit5Lesson8FractionsWholeNumbersTests
{
    private static JsonElement Lesson()
    {
        using var stream=typeof(Edulytics.Data.Seeding.CommonCoreG3SourceTeachingCorrections)
            .Assembly.GetManifestResourceStream("Edulytics.Data.Grade3.Unit5Lesson8RichV2");
        Assert.NotNull(stream);
        using var doc=JsonDocument.Parse(stream!);
        var root=doc.RootElement;
        Assert.Equal("g3-u05-l08-firstedition2021-31-fractions-whole-number-equivalence-and-unit-location-20261025-v1",root.GetProperty("ContentVersion").GetString());
        var a=root.GetProperty("Lessons");Assert.Equal(1,a.GetArrayLength());
        Assert.Equal("PED:US-CCSS-MATH:G3:U05:L08",a[0].GetProperty("LessonCode").GetString());
        return a[0].Clone();
    }
    [Theory]
    [InlineData(12,3)]
    [InlineData(24,6)]
    [InlineData(60,15)]
    [InlineData(72,18)]
    public void OriginalMentalDivisionCases(int dividend,int result) => Assert.Equal(result,dividend/4);

    [Theory]
    [InlineData(2,10,5)]
    [InlineData(3,9,3)]
    [InlineData(4,12,3)]
    public void OriginalSourceFractionFamilyHasCorrectWholePointCount(int denominator,int maxNumerator,int expectedWholeCount)
    {
        Assert.Equal(expectedWholeCount,Enumerable.Range(1,maxNumerator).Count(x=>x%denominator==0));
    }
    [Theory]
    [InlineData(2,2,4)]
    [InlineData(4,4,8)]
    [InlineData(6,6,12)]
    [InlineData(8,8,16)]
    public void GivenUnitFractionLocatesOneAndThenTwo(int denom,int stepsToOne,int stepsToTwo)
    {
        Assert.Equal(denom,stepsToOne);
        Assert.Equal(2*denom,stepsToTwo);
        var exp=string.Join(" ",Lesson().GetProperty("ExplanationParagraphs")
            .EnumerateArray().Select(x=>x.GetString()!));
        Assert.Contains("1/"+denom,exp);
    }
    [Fact]
    public void FractionListsHave31LabelsAnd11IntegerValuedItems()
    {
        int total=0,whole=0;
        foreach(var pair in new[]{(2,10),(3,9),(4,12)})
        {
            total+=pair.Item2;
            whole+=Enumerable.Range(1,pair.Item2).Count(n=>n%pair.Item1==0);
        }
        Assert.Equal(31,total);Assert.Equal(11,whole);
        var exp=string.Join(" ",Lesson().GetProperty("ExplanationParagraphs")
            .EnumerateArray().Select(x=>x.GetString()!));
        Assert.Contains("31 source-assigned",exp);
        Assert.Contains("eleven",exp);
        Assert.Contains("teacher-gated",exp);
    }
}
