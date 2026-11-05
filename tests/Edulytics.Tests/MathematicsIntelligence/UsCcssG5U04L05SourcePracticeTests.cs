using Edulytics.Services.Mathematics;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>G5 U04:L05 original source bounded C fixtures AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG5U04L05SourcePracticeTests
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
        const string family="usccss.g5.u04.l05.original_no_composing_products";
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
        const string family="usccss.g5.u04.l05.original_number_talk_and_diego_correction";
        Assert.Equal(expected,UsCcssG5U04WholeNumberSourceProducts.Solve(family,P(c,slot)));
        Assert.True(UsCcssG5U04WholeNumberSourceProducts.Verify(family,P(c,slot),expected));
        Assert.False(UsCcssG5U04WholeNumberSourceProducts.Verify(family,P(c,slot),"0"));
    }
    [Fact]
    public void MalformedAndTeacherReviewed()
    {
        foreach(var p in new[]{P(-1),P(99),P(0,-1),P(0,3)})
            Assert.False(UsCcssG5U04WholeNumberSourceProducts.Verify("usccss.g5.u04.l05.original_no_composing_products",p,"1"));
        var extra0=P(0);extra0["foreign"]=1;
        Assert.False(UsCcssG5U04WholeNumberSourceProducts.Verify("usccss.g5.u04.l05.original_no_composing_products",extra0,"1"));
        Assert.False(UsCcssG5U04WholeNumberSourceProducts.Verify("usccss.g5.u04.l05.original_no_composing_products",null,"1"));
        Assert.Contains("TEACHER_REVIEW_REQUIRED",UsCcssG5U04WholeNumberSourceProducts.Build("usccss.g5.u04.l05.original_no_composing_products",new Random(5)).Prompt);
        foreach(var p in new[]{P(-1),P(99),P(0,-1),P(0,3)})
            Assert.False(UsCcssG5U04WholeNumberSourceProducts.Verify("usccss.g5.u04.l05.original_number_talk_and_diego_correction",p,"1"));
        var extra1=P(0);extra1["foreign"]=1;
        Assert.False(UsCcssG5U04WholeNumberSourceProducts.Verify("usccss.g5.u04.l05.original_number_talk_and_diego_correction",extra1,"1"));
        Assert.False(UsCcssG5U04WholeNumberSourceProducts.Verify("usccss.g5.u04.l05.original_number_talk_and_diego_correction",null,"1"));
        Assert.Contains("TEACHER_REVIEW_REQUIRED",UsCcssG5U04WholeNumberSourceProducts.Build("usccss.g5.u04.l05.original_number_talk_and_diego_correction",new Random(5)).Prompt);
    }
}
