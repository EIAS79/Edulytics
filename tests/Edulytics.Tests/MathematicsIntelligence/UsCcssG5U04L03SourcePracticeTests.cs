using Edulytics.Services.Mathematics;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>G5 U04:L03 original source bounded C fixtures AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG5U04L03SourcePracticeTests
{
    private static Dictionary<string,int> P(int c,int s=0)=>
        new(StringComparer.Ordinal){["case"]=c,["slot"]=s};
    [Theory]
    [InlineData(0,0,"1")]
    [InlineData(1,1,"2")]
    [InlineData(2,2,"3")]
    [InlineData(3,0,"1")]
    [InlineData(4,1,"2")]
    public void Family1ExactSourceCase(int c,int slot,string expected)
    {
        const string family="usccss.g5.u04.l03.original_partial_product_algorithm";
        Assert.Equal(expected,UsCcssG5U04WholeNumberSourceProducts.Solve(family,P(c,slot)));
        Assert.True(UsCcssG5U04WholeNumberSourceProducts.Verify(family,P(c,slot),expected));
        Assert.False(UsCcssG5U04WholeNumberSourceProducts.Verify(family,P(c,slot),"0"));
    }
    [Theory]
    [InlineData(0,0,"1")]
    [InlineData(1,1,"2")]
    [InlineData(2,2,"3")]
    [InlineData(3,0,"1")]
    [InlineData(4,1,"2")]
    [InlineData(5,2,"3")]
    public void Family2ExactSourceCase(int c,int slot,string expected)
    {
        const string family="usccss.g5.u04.l03.original_245_by_35_partial_cells";
        Assert.Equal(expected,UsCcssG5U04WholeNumberSourceProducts.Solve(family,P(c,slot)));
        Assert.True(UsCcssG5U04WholeNumberSourceProducts.Verify(family,P(c,slot),expected));
        Assert.False(UsCcssG5U04WholeNumberSourceProducts.Verify(family,P(c,slot),"0"));
    }
    [Fact]
    public void MalformedAndTeacherReviewed()
    {
        foreach(var p in new[]{P(-1),P(99),P(0,-1),P(0,3)})
            Assert.False(UsCcssG5U04WholeNumberSourceProducts.Verify("usccss.g5.u04.l03.original_partial_product_algorithm",p,"1"));
        var extra0=P(0);extra0["foreign"]=1;
        Assert.False(UsCcssG5U04WholeNumberSourceProducts.Verify("usccss.g5.u04.l03.original_partial_product_algorithm",extra0,"1"));
        Assert.False(UsCcssG5U04WholeNumberSourceProducts.Verify("usccss.g5.u04.l03.original_partial_product_algorithm",null,"1"));
        Assert.Contains("TEACHER_REVIEW_REQUIRED",UsCcssG5U04WholeNumberSourceProducts.Build("usccss.g5.u04.l03.original_partial_product_algorithm",new Random(5)).Prompt);
        foreach(var p in new[]{P(-1),P(99),P(0,-1),P(0,3)})
            Assert.False(UsCcssG5U04WholeNumberSourceProducts.Verify("usccss.g5.u04.l03.original_245_by_35_partial_cells",p,"1"));
        var extra1=P(0);extra1["foreign"]=1;
        Assert.False(UsCcssG5U04WholeNumberSourceProducts.Verify("usccss.g5.u04.l03.original_245_by_35_partial_cells",extra1,"1"));
        Assert.False(UsCcssG5U04WholeNumberSourceProducts.Verify("usccss.g5.u04.l03.original_245_by_35_partial_cells",null,"1"));
        Assert.Contains("TEACHER_REVIEW_REQUIRED",UsCcssG5U04WholeNumberSourceProducts.Build("usccss.g5.u04.l03.original_245_by_35_partial_cells",new Random(5)).Prompt);
    }
}
