using System;
using System.Linq;
using System.Text.Json;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Lesson 97 academic modelling source contract — written only; no execution or Stage D.</summary>
public sealed class UsCcssG3Unit5Lesson18DesignWithFractionsTests
{
    private static JsonElement Source()
    {
        using var stream=typeof(Edulytics.Data.Seeding.CommonCoreG3SourceTeachingCorrections).Assembly
            .GetManifestResourceStream("Edulytics.Data.Grade3.Unit5Lesson18RichV2");
        Assert.NotNull(stream);
        using var doc=JsonDocument.Parse(stream!);
        Assert.Equal("g3-u05-l18-2021-optional-design-3-page-pdf-midpoints-quarters-v1",doc.RootElement.GetProperty("ContentVersion").GetString());
        var lessons=doc.RootElement.GetProperty("Lessons");
        Assert.Equal(1,lessons.GetArrayLength());
        Assert.Equal("PED:US-CCSS-MATH:G3:U05:L18",lessons[0].GetProperty("LessonCode").GetString());
        return lessons[0].Clone();
    }
    [Fact]
    public void HalfIterationsMatchOriginalThreeQuestionsAndAlwaysStaySquares()
    {
        var p=string.Join(" ",Source().GetProperty("ExplanationParagraphs").EnumerateArray().Select(x=>x.GetString()!));
        Assert.Contains("THREE separate source tasks",p);
        Assert.Contains("AT LEAST TWO MORE TIMES",p);
        Assert.Contains("nested SQUARES",p);
    }
    [Fact]
    public void QuarterEndpointChoiceMayVaryAndIsNotSingleNumericAnswer()
    {
        var p=string.Join(" ",Source().GetProperty("ExplanationParagraphs").EnumerateArray().Select(x=>x.GetString()!));
        Assert.Contains("NOT interchangeable",p);
        Assert.Contains("a non-square quadrilateral",p);
        Assert.Contains("Teacher",p,StringComparison.OrdinalIgnoreCase);
    }
    [Theory]
    [InlineData(0d,1d,0.5d)]
    [InlineData(0d,10d,5d)]
    [InlineData(2d,6d,4d)]
    public void MidpointIsIndependentOfChosenStartingEndpoint(double a,double b,double expected)
    {
        Assert.Equal(expected,a+(b-a)/2);
        Assert.Equal(expected,b+(a-b)/2);
    }
    [Theory]
    [InlineData(0d,1d,0.25d,0.75d)]
    [InlineData(2d,6d,3d,5d)]
    public void QuarterPointDependsOnEndpoint(double a,double b,double expectedFromA,double expectedFromB)
    {
        Assert.Equal(expectedFromA,a+(b-a)/4);
        Assert.Equal(expectedFromB,b+(a-b)/4);
        Assert.NotEqual(expectedFromA,expectedFromB);
    }
}