using System.Text.Json;
using Edulytics.Services.Mathematics;
using Edulytics.Web.Presentation;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class UsCcssG1SourceShapesAndHardTimesVisualTests
{
    private static string? Visual(int lesson,int a,int b,int mode)
    {
        string family=UsCcssG1FullGradeUnit7GeometryTimeQuestions.Families[lesson-1];
        var p=new Dictionary<string,int>{{"a",a},{"b",b},{"c",0},{"mode",mode}};
        Assert.True(UsCcssG1FullGradeUnit7GeometryTimeQuestions.Valid(family,p));
        return PracticeMathVisualRenderer.RenderSvg(family,
            JsonSerializer.Serialize(new{parameters=p}));
    }
    [Theory]
    [InlineData(1,0)]
    [InlineData(1,1)]
    [InlineData(1,2)]
    [InlineData(1,3)]
    [InlineData(2,0)]
    [InlineData(3,0)]
    [InlineData(3,1)]
    [InlineData(4,0)]
    [InlineData(4,1)]
    [InlineData(5,0)]
    [InlineData(6,0)]
    [InlineData(6,1)]
    [InlineData(7,0)]
    [InlineData(8,1)]
    [InlineData(12,1)]
    [InlineData(17,1)]
    public void GeometrySourceVisualsActuallyContainFirstPartyShapes(int lesson,int mode)
    {
        var visual=Assert.IsType<string>(Visual(lesson,4,3,mode));
        Assert.Contains("<svg ",visual);
        Assert.Contains("role='img'",visual);
        Assert.DoesNotContain("<image ",visual);
        Assert.DoesNotContain("RESULT=",visual);
    }
    [Fact]
    public void BuildingSolidsRendersEachOneOfActualComponentBlocks()
    {
        var visual=Assert.IsType<string>(Visual(2,4,3,0));
        Assert.Equal(4,System.Text.RegularExpressions.Regex.Matches(visual,"class='shape-cube'").Count);
        Assert.Equal(3,System.Text.RegularExpressions.Regex.Matches(visual,"class='shape-prism'").Count);
        Assert.DoesNotContain("PIECES=7",visual);
    }
    [Theory]
    [InlineData(8)]
    [InlineData(12)]
    [InlineData(17)]
    public void ArithmeticCenterModeDoesNotInventAnUnrelatedShape(int lesson)
    {
        Assert.Null(Visual(lesson,8,4,0));
    }
    [Fact]
    public void OriginalHardTimesShowsClockFaceWithAnActualHalfHourHand()
    {
        var full=Assert.IsType<string>(Visual(16,6,1,0));
        var half=Assert.IsType<string>(Visual(16,6,1,1));
        Assert.Contains("class='clock-minute'",full);
        Assert.Contains("class='clock-minute'",half);
        Assert.NotEqual(full,half);
        Assert.Contains("x2='210' y2='203'",half);
    }
}
