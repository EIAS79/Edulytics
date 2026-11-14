using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// UAE G5 U10 local Arabic source printed pp733 onward. Stage D tests written only; NOT_RUN.
public sealed class UaeGrade5Unit10MetricPracticeTests
{
    [Theory]
    [InlineData("uae.g5.u10.l01.fraction_plot_count")]
    [InlineData("uae.g5.u10.l01.fraction_plot_total")]
    [InlineData("uae.g5.u10.l02.ruler_nearest_centimeter")]
    [InlineData("uae.g5.u10.l02.ruler_length_millimeters")]
    [InlineData("uae.g5.u10.l03.meters_tenths_to_centimeters")]
    [InlineData("uae.g5.u10.l03.millimeters_to_meters")]
    [InlineData("uae.g5.u10.l04.mass_difference_kg_g")]
    [InlineData("uae.g5.u10.l05.kilograms_tenths_to_grams")]
    [InlineData("uae.g5.u10.l05.grams_to_kilograms")]
    [InlineData("uae.g5.u10.l06.liters_tenths_to_milliliters")]
    [InlineData("uae.g5.u10.l06.milliliters_to_liters")]
    [InlineData("uae.g5.u10.l07.logical_remaining")]
    [InlineData("uae.g5.u10.l07.logical_original_total")]
    public void ExactFamiliesGenerateVerifyAndRejectCorruption(string family)
    {
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
        Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var ui));
        Assert.NotNull(ui);
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var item in engine.Generate("PED:UAE-MOE-MATH:L5:GENERAL:10:01",
            family,[family],ExactSkillQuestionDifficulty.Standard,2,20261113,[]))
        {
            Assert.True(ExactSkillContractQuestionEngine.Verify(family,item.Parameters,item.CorrectAnswer));
            Assert.False(ExactSkillContractQuestionEngine.Verify(family,item.Parameters,"-999"));
            Assert.DoesNotContain("result",item.Parameters.Keys);
        }
    }
}
