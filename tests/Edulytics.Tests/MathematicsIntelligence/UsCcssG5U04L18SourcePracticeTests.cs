using Edulytics.Services.Mathematics;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>G5 U04:L18 original source bounded C fixtures AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG5U04L18SourcePracticeTests
{
    private static Dictionary<string,int> P(int c,int s=0)=>
        new(StringComparer.Ordinal){["case"]=c,["slot"]=s};
    [Theory]
    [InlineData(0,0,"1")]
    [InlineData(1,1,"2")]
    [InlineData(2,2,"3")]
    [InlineData(3,0,"1")]
    public void Family1ExactSourceCase(int c,int slot,string expected)
    {
        const string family="usccss.g5.u04.l18.edulytiks_conditional_milk_days";
        Assert.Equal(expected,UsCcssG5U04WholeNumberSourceDivision.Solve(family,P(c,slot)));
        Assert.True(UsCcssG5U04WholeNumberSourceDivision.Verify(family,P(c,slot),expected));
        Assert.False(UsCcssG5U04WholeNumberSourceDivision.Verify(family,P(c,slot),"0"));
    }
    [Fact]
    public void MalformedAndTeacherReviewed()
    {
        foreach(var p in new[]{P(-1),P(99),P(0,-1),P(0,3)})
            Assert.False(UsCcssG5U04WholeNumberSourceDivision.Verify("usccss.g5.u04.l18.edulytiks_conditional_milk_days",p,"1"));
        var extra0=P(0);extra0["foreign"]=1;
        Assert.False(UsCcssG5U04WholeNumberSourceDivision.Verify("usccss.g5.u04.l18.edulytiks_conditional_milk_days",extra0,"1"));
        Assert.False(UsCcssG5U04WholeNumberSourceDivision.Verify("usccss.g5.u04.l18.edulytiks_conditional_milk_days",null,"1"));
        Assert.Contains("TEACHER_REVIEW_REQUIRED",UsCcssG5U04WholeNumberSourceDivision.Build("usccss.g5.u04.l18.edulytiks_conditional_milk_days",new Random(5)).Prompt);
    }
}
