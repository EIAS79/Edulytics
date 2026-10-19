using System.Text.Json;
using Edulytics.Web.Presentation;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>First-party source-grounded U06 measurement/U07 partition visuals.
/// Independently checks SVG geometry from the prompt operands rather than
/// treating the visual as proof of teacher-led physical activities.</summary>
public sealed class UsCcssG1FullGradeMeasurementFractionVisualTests
{
    private static string? Draw(string family,int a,int b,int mode)
    {
        var json=JsonSerializer.Serialize(new{parameters=new{a,b,c=0,mode}});
        return PracticeMathVisualRenderer.RenderSvg(family,json);
    }

    [Theory]
    [InlineData("usccss.g1.u06.l01.compare_lengths",14,9,0)]
    [InlineData("usccss.g1.u06.l02.indirect_length_comparison",8,12,1)]
    [InlineData("usccss.g1.u06.l03.choose_indirect_comparison",8,8,2)]
    [InlineData("usccss.g1.u06.l05.measure_connecting_cubes",12,0,0)]
    [InlineData("usccss.g1.u06.l06.measure_paperclips",11,0,2)]
    [InlineData("usccss.g1.u06.l07.unit_length_matters",6,2,1)]
    public void SourceMeasurementRepresentationsUseAlignedIdenticalUnits(string family,int a,int b,int mode)
    {
        Assert.True(UsCcssG1FullGradeUnit6MeasurementQuestions.Valid(
            family,new Dictionary<string,int>{{"a",a},{"b",b},{"c",0},{"mode",mode}}));
        var svg=Assert.IsType<string>(Draw(family,a,b,mode));
        Assert.Contains("<svg ",svg);
        Assert.Contains("role='img'",svg);
        Assert.Contains("Same",svg,StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("<image ",svg);
        Assert.DoesNotContain("LENGTH=",svg);
    }

    [Fact]
    public void UnitSizesChangePhysicalUnitCountButNotMeasuredBarLength()
    {
        var svg=Assert.IsType<string>(Draw("usccss.g1.u06.l07.unit_length_matters",6,2,0));
        Assert.Equal(6,System.Text.RegularExpressions.Regex.Matches(svg,"class='long-unit'").Count);
        Assert.Equal(12,System.Text.RegularExpressions.Regex.Matches(svg,"class='short-unit'").Count);
        Assert.Contains("Same length, different unit sizes",svg);
    }

    [Theory]
    [InlineData("usccss.g1.u07.l09.equal_halves_fourths",0)]
    [InlineData("usccss.g1.u07.l09.equal_halves_fourths",1)]
    [InlineData("usccss.g1.u07.l10.parts_make_whole",0)]
    [InlineData("usccss.g1.u07.l10.parts_make_whole",1)]
    [InlineData("usccss.g1.u07.l11.bigger_piece_halves_fourths",0)]
    [InlineData("usccss.g1.u07.l11.bigger_piece_halves_fourths",1)]
    public void OriginalEqualAreaTasksHaveAccuratePartWholeDiagrams(string family,int mode)
    {
        var p=new Dictionary<string,int>{{"a",4},{"b",8},{"c",0},{"mode",mode}};
        Assert.True(UsCcssG1FullGradeUnit7GeometryTimeQuestions.Valid(family,p));
        var svg=Assert.IsType<string>(Draw(family,4,8,mode));
        Assert.Contains("equal",svg,StringComparison.OrdinalIgnoreCase);
        Assert.Contains("<svg ",svg);
        Assert.DoesNotContain("PART=HALF",svg);
        Assert.DoesNotContain("PART=FOURTH",svg);
    }

    [Fact]
    public void OnlySourceGuardedVisualFamiliesProduceDiagrams()
    {
        Assert.Null(Draw("usccss.g1.u06.l12.compare_measurements",14,4,0));
        Assert.Null(Draw("usccss.g1.u07.l17.center_geometry_and_time",6,5,0));
        Assert.Null(Draw("usccss.g1.u06.l01.compare_lengths",100,3,0));
    }
}
