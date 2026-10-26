using System;
using System.Linq;
using System.Text.Json;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// 2021 nine source cards / five fold targets with separate teacher gates.
/// Tests are authored only; not executed or academic approval evidence.
/// </summary>
public sealed class UsCcssG3Unit5Lesson5NumberLineSourceTests
{
    private static JsonElement Lesson()
    {
        using var stream=typeof(Edulytics.Data.Seeding.CommonCoreG3SourceTeachingCorrections)
            .Assembly.GetManifestResourceStream("Edulytics.Data.Grade3.Unit5Lesson5RichV2");
        Assert.NotNull(stream);
        using var doc=JsonDocument.Parse(stream!);
        var root=doc.RootElement;
        Assert.Equal("g3-u05-l05-firstedition2021-nine-numberline-cards-five-folds-20261025-v1",root.GetProperty("ContentVersion").GetString());
        var arr=root.GetProperty("Lessons");
        Assert.Equal(1,arr.GetArrayLength());
        Assert.Equal("PED:US-CCSS-MATH:G3:U05:L05",arr[0].GetProperty("LessonCode").GetString());
        return arr[0].Clone();
    }
    [Theory]
    [InlineData("A",0,8,2,4)]
    [InlineData("B",0,1,4,0.25)]
    [InlineData("C",0,3,6,0.5)]
    [InlineData("D",0,2,4,0.5)]
    [InlineData("E",0,4,4,1.0)]
    [InlineData("F",0,1,6,1.0/6.0)]
    [InlineData("G",0,6,3,2.0)]
    [InlineData("H",0,2,6,1.0/3.0)]
    [InlineData("I",0,4,8,0.5)]
    public void SourceCardEqualIntervalLength(string card,int left,int right,int spaces,double expectedStep)
    {
        Assert.Equal(expectedStep,(right-left)/(double)spaces,8);
        var paragraphs=string.Join(" ",Lesson().GetProperty("ExplanationParagraphs")
            .EnumerateArray().Select(x=>x.GetString()!));
        Assert.Contains(card+" ",paragraphs);
    }
    [Theory]
    [InlineData(2,"1/2")]
    [InlineData(3,"1/3")]
    [InlineData(4,"1/4")]
    [InlineData(6,"1/6")]
    [InlineData(8,"1/8")]
    public void OriginalFiveFoldingFirstPointTargets(int intervals,string firstTick)
    {
        Assert.Equal("1/"+intervals,firstTick);
        var paragraphs=string.Join(" ",Lesson().GetProperty("ExplanationParagraphs")
            .EnumerateArray().Select(x=>x.GetString()!));
        Assert.Contains("locate "+firstTick,paragraphs);
    }
    [Fact]
    public void BothAndreAndClareNumberLineInterpretationsRequireTeacherExplanation()
    {
        var paragraphs=string.Join(" ",Lesson().GetProperty("ExplanationParagraphs")
            .EnumerateArray().Select(x=>x.GetString()!));
        Assert.Contains("Andre marks the LENGTH",paragraphs);
        Assert.Contains("Clare labels the POINT",paragraphs);
        Assert.Contains("teacher-gated",paragraphs);
    }
    [Fact]
    public void LessonContentHasIndividuallyWorkedOriginalNumberLineCases()
    {
        var examples=Lesson().GetProperty("WorkedExamples").EnumerateArray().ToArray();
        Assert.True(examples.Length>=6);
        Assert.Contains(examples,x=>x.GetProperty("Title").GetString()!.Contains("Card I"));
    }
}
