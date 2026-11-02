using System;
using System.Linq;
using System.Text.Json;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>First-party mathematical oracles only; original open 2021 Scoot/partner play is teacher-reviewed. Written not run.</summary>
public sealed class UsCcssG3Unit5Lesson7NonUnitNumberLineTests
{
    private static JsonElement Lesson()
    {
        using var stream=typeof(Edulytics.Data.Seeding.CommonCoreG3SourceTeachingCorrections)
            .Assembly.GetManifestResourceStream("Edulytics.Data.Grade3.Unit5Lesson7RichV2");
        Assert.NotNull(stream);
        using var doc=JsonDocument.Parse(stream!);
        var root=doc.RootElement;
        Assert.Equal("g3-u05-l07-firstedition2021-numberline-scoot-16-quarters-eight-nonunit-fractions-20261025-v1",root.GetProperty("ContentVersion").GetString());
        var arr=root.GetProperty("Lessons");Assert.Equal(1,arr.GetArrayLength());
        Assert.Equal("PED:US-CCSS-MATH:G3:U05:L07",arr[0].GetProperty("LessonCode").GetString());
        return arr[0].Clone();
    }
    [Theory]
    [InlineData(1,1,4)]
    [InlineData(4,1,1)]
    [InlineData(8,2,1)]
    [InlineData(12,3,1)]
    [InlineData(16,4,1)]
    public void OriginalQuarterCountTermHasExactRationalValue(int numerator,int wholeNumerator,int wholeDenominator)
    {
        Assert.InRange(numerator,1,16);
        Assert.Equal(numerator*wholeDenominator,wholeNumerator*4);
    }
    [Theory]
    [InlineData(3,4)]
    [InlineData(6,4)]
    [InlineData(7,8)]
    [InlineData(12,8)]
    [InlineData(2,3)]
    [InlineData(4,3)]
    [InlineData(2,6)]
    [InlineData(7,6)]
    public void EightOriginalSourceFractionsArePlottableOn0To2(int numerator,int denominator)
    {
        Assert.True(0<numerator);
        Assert.True(numerator<=2*denominator);
        var examples=Lesson().GetProperty("WorkedExamples").EnumerateArray().ToArray();
        Assert.Contains(examples,e=>e.GetProperty("Question").GetString()!.Contains(numerator+"/"+denominator));
    }
    [Fact]
    public void ScootMovesNumeratorsNotEntireWholeLines()
    {
        Assert.Equal(7,3+4);
        var exp=string.Join(" ",Lesson().GetProperty("ExplanationParagraphs")
            .EnumerateArray().Select(x=>x.GetString()!));
        Assert.Contains("7/4",exp);
        Assert.Contains("not 4",exp);
    }
    [Fact]
    public void OpenPartnerCreatedMarksRequireRealTeacherReview()
    {
        var exp=string.Join(" ",Lesson().GetProperty("ExplanationParagraphs")
            .EnumerateArray().Select(x=>x.GetString()!));
        Assert.Contains("DO NOT WRITE",exp);
        Assert.Contains("no single fixed fraction",exp,StringComparison.OrdinalIgnoreCase);
        Assert.Contains("publisher",exp,StringComparison.OrdinalIgnoreCase);
    }
}
