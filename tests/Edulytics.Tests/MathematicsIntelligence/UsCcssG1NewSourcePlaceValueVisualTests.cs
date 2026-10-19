using System.Text.Json;
using Edulytics.Web.Presentation;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Original Grade1 new source-based place-value diagrams: rods
/// represent 10 units, loose ones are single units; no numerical solution
/// text prefilled, no classroom participation falsely recorded.</summary>
public sealed class UsCcssG1NewSourcePlaceValueVisualTests
{
    private static string? Visual(string family,object p)=>
        PracticeMathVisualRenderer.RenderSvg(family,
            JsonSerializer.Serialize(new{parameters=p}));

    [Theory]
    [InlineData("usccss.g1.u04.l06.collection.tens_ones",4,7)]
    [InlineData("usccss.g1.u04.l07.read.two_digit",8,0)]
    [InlineData("usccss.g1.u04.l08.match.tens_ones_expression",2,9)]
    [InlineData("usccss.g1.u04.l09.show.expanded_form",9,5)]
    [InlineData("usccss.g1.u04.l10.write.two_digit_numeral",6,4)]
    public void Unit4LessonSixToTenRepresentsEachTenAndOne(string family,int tens,int ones)
    {
        var svg=Assert.IsType<string>(Visual(family,new{tens,ones,mode=0,unused=0}));
        Assert.Equal(tens,System.Text.RegularExpressions.Regex.Matches(svg,"class='g1-ten-rod'").Count);
        Assert.Equal(ones,System.Text.RegularExpressions.Regex.Matches(svg,"class='g1-one-cube'").Count);
        Assert.Contains("role='img'",svg);
        Assert.DoesNotContain("NUMBER=",svg);
        Assert.DoesNotContain("<image ",svg);
    }
    [Theory]
    [InlineData("usccss.g1.u06.l08.count_to110_cube_groups",110,11,0)]
    [InlineData("usccss.g1.u06.l09.write_number_to120",120,12,0)]
    [InlineData("usccss.g1.u08.l08.same_number_tens_ones",95,9,5)]
    public void UnitSixAndEightRenderUpToOneHundredTwenty(string family,int number,int tens,int ones)
    {
        var svg=Assert.IsType<string>(Visual(family,new{a=number,b=0,c=0,mode=0}));
        Assert.Equal(tens,System.Text.RegularExpressions.Regex.Matches(svg,"class='g1-ten-rod'").Count);
        Assert.Equal(ones,System.Text.RegularExpressions.Regex.Matches(svg,"class='g1-one-cube'").Count);
        Assert.DoesNotContain("TOTAL=",svg);
        Assert.DoesNotContain("NUMBER=",svg);
    }
    [Theory]
    [InlineData(37,3,7)]
    [InlineData(110,11,0)]
    [InlineData(120,12,0)]
    public void OriginalLargeCollectionTaskUsesTensAndOnesVisuals(
        int total,int tenRods,int singleCubes)
    {
        const string family="usccss.g1.u08.l07.count_on_to120";
        var model=Assert.IsType<string>(Visual(
            family,new{a=total,b=0,c=0,mode=2}));
        Assert.Equal(tenRods,System.Text.RegularExpressions.Regex.Matches(
            model,"class='g1-ten-rod'").Count);
        Assert.Equal(singleCubes,System.Text.RegularExpressions.Regex.Matches(
            model,"class='g1-one-cube'").Count);
        Assert.DoesNotContain("TOTAL=",model);
        Assert.DoesNotContain("LAST=",model);
    }

    [Theory]
    [InlineData(80,3)]
    [InlineData(105,5)]
    [InlineData(118,2)]
    public void NumberPathDrawsExactlyTheUnlabelledHops(int start,int hops)
    {
        const string family="usccss.g1.u08.l07.count_on_to120";
        var p=new{a=start,b=hops,c=0,mode=0};
        Assert.True(UsCcssG1FullGradeUnit8ReviewQuestions.Valid(
            family,new Dictionary<string,int>{{"a",start},{"b",hops},{"c",0},{"mode",0}}));
        var svg=Assert.IsType<string>(Visual(family,p));
        Assert.Equal(hops,System.Text.RegularExpressions.Regex.Matches(svg,"<path d='M").Count);
        Assert.DoesNotContain("LAST=",svg);
        Assert.DoesNotContain((start+hops).ToString()+"</text>",svg);
    }
    [Fact]
    public void RefusesOutOfRangeBaseTenNumberAndUnrelatedOldLesson()
    {
        Assert.Null(Visual("usccss.g1.u04.l10.write.two_digit_numeral",
            new{tens=10,ones=3,mode=0,unused=0}));
        Assert.Null(Visual("usccss.g1.u06.l09.write_number_to120",
            new{a=121,b=0,c=0,mode=0}));
    }
}
