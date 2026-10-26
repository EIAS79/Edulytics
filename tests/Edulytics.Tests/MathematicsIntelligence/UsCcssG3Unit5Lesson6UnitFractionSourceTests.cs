using System;
using System.Linq;
using System.Text.Json;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>First-party 2021 source-specific unit fraction cases, authored but not run.</summary>
public sealed class UsCcssG3Unit5Lesson6UnitFractionSourceTests
{
    private static JsonElement Lesson()
    {
        using var stream=typeof(Edulytics.Data.Seeding.CommonCoreG3SourceTeachingCorrections)
            .Assembly.GetManifestResourceStream("Edulytics.Data.Grade3.Unit5Lesson6RichV2");
        Assert.NotNull(stream);
        using var doc=JsonDocument.Parse(stream!);
        var root=doc.RootElement;
        Assert.Equal("g3-u05-l06-firstedition2021-andre-fourths-nine-unit-fraction-lines-20261025-v1",root.GetProperty("ContentVersion").GetString());
        var array=root.GetProperty("Lessons");
        Assert.Equal(1,array.GetArrayLength());
        Assert.Equal("PED:US-CCSS-MATH:G3:U05:L06",array[0].GetProperty("LessonCode").GetString());
        return array[0].Clone();
    }
    [Theory]
    [InlineData(1,4,1)]
    [InlineData(2,8,1)]
    [InlineData(3,3,1)]
    [InlineData(4,6,1)]
    [InlineData(5,2,4)]
    [InlineData(6,4,2)]
    [InlineData(7,8,4)]
    [InlineData(8,3,3)]
    [InlineData(9,6,2)]
    public void NinePublicSourceTasksTargetUnitNotWholeLine(int ordinal,int denominator,int end)
    {
        Assert.InRange(ordinal,1,9);
        Assert.True(denominator>1);
        Assert.True(end>=1);
        double point=1.0/denominator;
        Assert.True(point>0 && point<1);
        Assert.True(point<end);
        var exp=string.Join(" ",Lesson().GetProperty("ExplanationParagraphs").EnumerateArray()
            .Select(x=>x.GetString()!));
        Assert.Contains("1/4, 1/8, 1/3, 1/6, 1/2",exp);
    }
    [Fact]
    public void SourceRequiresAndreAndFlagsClareDiegoMisconceptions()
    {
        var p=string.Join(" ",Lesson().GetProperty("ExplanationParagraphs").EnumerateArray()
            .Select(x=>x.GetString()!));
        Assert.Contains("Only Andre",p);
        Assert.Contains("Clare divides",p);
        Assert.Contains("Diego places four tick marks",p);
        Assert.Contains("FIVE intervals",p);
    }
    [Fact]
    public void MultiWholeUnitFractionAndPublisherGatedAssessmentRemainSeparated()
    {
        var p=string.Join(" ",Lesson().GetProperty("ExplanationParagraphs").EnumerateArray()
            .Select(x=>x.GetString()!));
        Assert.Contains("NOT halfway from 0 to 4",p);
        Assert.Contains("teacher-gated",p);
        Assert.Contains("not embedded",p);
    }
}
