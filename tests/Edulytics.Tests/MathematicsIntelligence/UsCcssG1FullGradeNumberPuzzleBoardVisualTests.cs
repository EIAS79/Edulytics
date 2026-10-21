using System.Text.Json;
using Edulytics.Services.Mathematics;
using Edulytics.Web.Presentation;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UsCcssG1FullGradeNumberPuzzleBoardVisualTests
{
    [Theory]
    [InlineData(17,4,8,2)]
    [InlineData(16,7,3,3)]
    public void DigitCardSourceBoardShowsTwoUnfilledPlacesNotCorrectAnswer(
        int a,int b,int c,int mode)
    {
        const string family="usccss.g1.u03.l14.center.add_subtract";
        var p=new Dictionary<string,int>{{"a",a},{"b",b},{"c",c},{"mode",mode}};
        Assert.True(UsCcssG1FullGradeStartQuestions.Valid(family,p));
        var svg=Assert.IsType<string>(PracticeMathVisualRenderer.RenderSvg(
            family,JsonSerializer.Serialize(new{parameters=p})));
        Assert.Contains("<svg ",svg);
        Assert.Contains("Different cards",svg);
        Assert.Equal(2,System.Text.RegularExpressions.Regex.Matches(svg,"class='shape'").Count);
        Assert.DoesNotContain("CARDS=",svg);
        Assert.DoesNotContain("<image ",svg);
    }
    [Fact]
    public void PlainArithmeticCenterDoesNotClaimToBeABoard()
    {
        const string family="usccss.g1.u03.l14.center.add_subtract";
        var p=new Dictionary<string,int>{{"a",17},{"b",4},{"c",0},{"mode",0}};
        Assert.Null(PracticeMathVisualRenderer.RenderSvg(
            family,JsonSerializer.Serialize(new{parameters=p})));
    }
}
