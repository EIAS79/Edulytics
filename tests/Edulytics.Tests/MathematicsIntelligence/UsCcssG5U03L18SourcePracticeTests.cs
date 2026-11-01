using Edulytics.Services.Mathematics;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>G5 U03:L18 original source bounded C fixtures AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG5U03L18SourcePracticeTests
{
    private static Dictionary<string,int> P(int c,int s=0)=>
        new(StringComparer.Ordinal){["case"]=c,["slot"]=s};
    [Theory]
    [InlineData(0,0,"1")]
    [InlineData(1,1,"2")]
    [InlineData(2,2,"3")]
    [InlineData(3,0,"1")]
    [InlineData(4,1,"2")]
    [InlineData(5,2,"3")]
    public void Family1ExactSourceCase(int c,int slot,string expected)
    {
        const string family="usccss.g5.u03.l18.original_warmup_and_picnic";
        Assert.Equal(expected,UsCcssG5U03SourceDivisionQuestions.Solve(family,P(c,slot)));
        Assert.True(UsCcssG5U03SourceDivisionQuestions.Verify(family,P(c,slot),expected));
        Assert.False(UsCcssG5U03SourceDivisionQuestions.Verify(family,P(c,slot),"0"));
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
        const string family="usccss.g5.u03.l18.original_partner_contexts";
        Assert.Equal(expected,UsCcssG5U03SourceDivisionQuestions.Solve(family,P(c,slot)));
        Assert.True(UsCcssG5U03SourceDivisionQuestions.Verify(family,P(c,slot),expected));
        Assert.False(UsCcssG5U03SourceDivisionQuestions.Verify(family,P(c,slot),"0"));
    }
    [Fact]
    public void MalformedAndTeacherReviewed()
    {
        foreach(var p in new[]{P(-1),P(99),P(0,-1),P(0,3)})
            Assert.False(UsCcssG5U03SourceDivisionQuestions.Verify("usccss.g5.u03.l18.original_warmup_and_picnic",p,"1"));
        var extra0=P(0);extra0["foreign"]=1;
        Assert.False(UsCcssG5U03SourceDivisionQuestions.Verify("usccss.g5.u03.l18.original_warmup_and_picnic",extra0,"1"));
        Assert.False(UsCcssG5U03SourceDivisionQuestions.Verify("usccss.g5.u03.l18.original_warmup_and_picnic",null,"1"));
        Assert.Contains("TEACHER_REVIEW_REQUIRED",UsCcssG5U03SourceDivisionQuestions.Build("usccss.g5.u03.l18.original_warmup_and_picnic",new Random(5)).Prompt);
        foreach(var p in new[]{P(-1),P(99),P(0,-1),P(0,3)})
            Assert.False(UsCcssG5U03SourceDivisionQuestions.Verify("usccss.g5.u03.l18.original_partner_contexts",p,"1"));
        var extra1=P(0);extra1["foreign"]=1;
        Assert.False(UsCcssG5U03SourceDivisionQuestions.Verify("usccss.g5.u03.l18.original_partner_contexts",extra1,"1"));
        Assert.False(UsCcssG5U03SourceDivisionQuestions.Verify("usccss.g5.u03.l18.original_partner_contexts",null,"1"));
        Assert.Contains("TEACHER_REVIEW_REQUIRED",UsCcssG5U03SourceDivisionQuestions.Build("usccss.g5.u03.l18.original_partner_contexts",new Random(5)).Prompt);
    }
}
