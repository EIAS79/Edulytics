using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class UaeG10RationalUnitCandidateTests
{
    private static Dictionary<string,int> Parameters()=>new(){{"a",3},{"b",4},{"c",5},{"d",6},{"e",2},{"f",7}};

    [Theory]
    [InlineData("uae.g10.general.u06.l03.product_cancel_constant", "3/5")]
    [InlineData("uae.g10.general.u06.l03.division_cancel_constant", "3/5")]
    [InlineData("uae.g10.general.u06.l03.cancellation_original_exclusion", "4")]
    [InlineData("uae.g10.general.u06.l03.complex_fraction_quotient", "9/10")]
    [InlineData("uae.g10.general.u06.l04.like_denominator_sum_numerator", "7")]
    [InlineData("uae.g10.general.u06.l04.like_denominator_difference_numerator", "-1")]
    [InlineData("uae.g10.general.u06.l04.unlike_denominator_linear_coefficient", "8")]
    [InlineData("uae.g10.general.u06.l04.unlike_denominator_constant", "-38")]
    [InlineData("uae.g10.general.u06.l04.complex_fraction_sum", "133/24")]
    [InlineData("uae.g10.general.u06.l05.reciprocal_vertical_asymptote", "4")]
    [InlineData("uae.g10.general.u06.l05.reciprocal_horizontal_asymptote", "5")]
    [InlineData("uae.g10.general.u06.l05.reciprocal_graph_value", "13/2")]
    [InlineData("uae.g10.general.u06.l05.removable_hole_x", "4")]
    [InlineData("uae.g10.general.u06.l05.removable_hole_y", "9")]
    [InlineData("uae.g10.general.u06.l05.oblique_asymptote_intercept", "5")]
    [InlineData("uae.g10.general.u06.l06.direct_variation_value", "18")]
    [InlineData("uae.g10.general.u06.l06.inverse_variation_value", "2")]
    [InlineData("uae.g10.general.u06.l06.joint_variation_value", "60")]
    [InlineData("uae.g10.general.u06.l06.combined_variation_value", "12/5")]
    [InlineData("uae.g10.general.u06.l07.rational_equation_exact_root", "38/5")]
    [InlineData("uae.g10.general.u06.l07.rational_equation_extraneous_count", "0")]
    [InlineData("uae.g10.general.u06.l07.rational_inequality_membership", "1")]
    [InlineData("uae.g10.general.u06.l07.rational_combined_work_time", "12/5")]
    public void GoldenSourceSubskillAndInputBoundaries(string family,string expected)
    {
        var p=Parameters();
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
        Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var interaction));
        Assert.NotNull(interaction);
        Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,expected),family);
        Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,string.Concat(expected.Select(ch=>ch is >= '0' and <= '9'?(char)('٠'+ch-'0'):ch))));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,"wrong"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,"1/0"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,"999999"));
        p["d"]=p["b"];
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,expected));
    }

    [Fact]
    public void EquivalentFractionsAndStrictInequality()
    {
        var p=Parameters();
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g10.general.u06.l03.product_cancel_constant",p,"6/10"));
        Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g10.general.u06.l03.product_cancel_constant",p,"3/4"));
        p["a"]=5;p["d"]=6;p["c"]=5;p["e"]=6;
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g10.general.u06.l07.rational_inequality_membership",p,"0"));
        Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g10.general.u06.l07.rational_inequality_membership",p,"1"));
    }
}
