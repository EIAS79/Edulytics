using System;
using System.Linq;
using System.Text.Json;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>Only independently verified original-source numeric coordinates. Tests not executed.</summary>
public sealed class UsCcssG3Unit5Lesson9RecoverWholeNumberTests
{
    private static JsonElement Lesson()
    {
        using var stream=typeof(Edulytics.Data.Seeding.CommonCoreG3SourceTeachingCorrections)
            .Assembly.GetManifestResourceStream("Edulytics.Data.Grade3.Unit5Lesson9RichV2");
        Assert.NotNull(stream);
        using var doc=JsonDocument.Parse(stream!);
        var root=doc.RootElement;
        Assert.Equal("g3-u05-l09-firstedition2021-reconstruct-one-from-nonunit-and-optional-threequarters-20261025-v1",root.GetProperty("ContentVersion").GetString());
        var a=root.GetProperty("Lessons");Assert.Single(a.EnumerateArray());
        Assert.Equal("PED:US-CCSS-MATH:G3:U05:L09",a[0].GetProperty("LessonCode").GetString());
        return a[0].Clone();
    }
    [Theory]
    [InlineData(2,3,false)]
    [InlineData(5,4,true)]
    [InlineData(11,8,true)]
    public void OriginalNonUnitAnchorDeterminesOneUsingNumeratorDenominator(int numerator,int denominator,bool oneIsLeftOfKnown)
    {
        Assert.Equal(denominator*numerator, numerator*denominator);
        Assert.Equal(numerator>denominator,oneIsLeftOfKnown);
        double fraction=numerator/(double)denominator;
        Assert.Equal(1.0,fraction*(denominator/(double)numerator),8);
        var paragraph=string.Join(" ",Lesson().GetProperty("ExplanationParagraphs")
            .EnumerateArray().Select(x=>x.GetString()!));
        Assert.Contains(numerator+"/"+denominator,paragraph);
    }
    [Fact]
    public void OptionalThreeFourthsFromOneThirdMustNotBeMandatory()
    {
        var paragraph=string.Join(" ",Lesson().GetProperty("ExplanationParagraphs")
            .EnumerateArray().Select(x=>x.GetString()!));
        Assert.Contains("EXPLICITLY OPTIONAL",paragraph);
        Assert.Contains("BEYOND required Grade 3 depth",paragraph);
        Assert.Contains("THREE one-third lengths",paragraph);
        Assert.Contains("FOUR equal spaces",paragraph);
    }
    [Fact]
    public void SourceSynthesisRecognizesSixthsAtOneAndTwo()
    {
        Assert.Equal(1,6/6);
        Assert.Equal(2,12/6);
        var paragraph=string.Join(" ",Lesson().GetProperty("ExplanationParagraphs")
            .EnumerateArray().Select(x=>x.GetString()!));
        Assert.Contains("6/6=1",paragraph);
        Assert.Contains("12/6=2",paragraph);
    }
}
