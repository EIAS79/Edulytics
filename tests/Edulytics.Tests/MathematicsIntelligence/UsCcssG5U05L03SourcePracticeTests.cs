using Edulytics.Services.Mathematics;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>G5 U05:L03 original source bounded C fixtures AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG5U05L03SourcePracticeTests
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
        const string family="usccss.g5.u05.l03.original_decimal_expanded_forms";
        Assert.Equal(expected,UsCcssG5U05SourceThousandths.Solve(family,P(c,slot)));
        Assert.True(UsCcssG5U05SourceThousandths.Verify(family,P(c,slot),expected));
        Assert.False(UsCcssG5U05SourceThousandths.Verify(family,P(c,slot),"0"));
    }
    [Theory]
    [InlineData(0,0,"1")]
    [InlineData(1,1,"2")]
    [InlineData(2,2,"3")]
    [InlineData(3,0,"1")]
    public void Family2ExactSourceCase(int c,int slot,string expected)
    {
        const string family="usccss.g5.u05.l03.source_wodb_and_mai_error";
        Assert.Equal(expected,UsCcssG5U05SourceThousandths.Solve(family,P(c,slot)));
        Assert.True(UsCcssG5U05SourceThousandths.Verify(family,P(c,slot),expected));
        Assert.False(UsCcssG5U05SourceThousandths.Verify(family,P(c,slot),"0"));
    }
    [Fact]
    public void MalformedAndTeacherReviewed()
    {
        foreach(var p in new[]{P(-1),P(99),P(0,-1),P(0,3)})
            Assert.False(UsCcssG5U05SourceThousandths.Verify("usccss.g5.u05.l03.original_decimal_expanded_forms",p,"1"));
        var extra0=P(0);extra0["foreign"]=1;
        Assert.False(UsCcssG5U05SourceThousandths.Verify("usccss.g5.u05.l03.original_decimal_expanded_forms",extra0,"1"));
        Assert.False(UsCcssG5U05SourceThousandths.Verify("usccss.g5.u05.l03.original_decimal_expanded_forms",null,"1"));
        Assert.Contains("TEACHER_REVIEW_REQUIRED",UsCcssG5U05SourceThousandths.Build("usccss.g5.u05.l03.original_decimal_expanded_forms",new Random(5)).Prompt);
        foreach(var p in new[]{P(-1),P(99),P(0,-1),P(0,3)})
            Assert.False(UsCcssG5U05SourceThousandths.Verify("usccss.g5.u05.l03.source_wodb_and_mai_error",p,"1"));
        var extra1=P(0);extra1["foreign"]=1;
        Assert.False(UsCcssG5U05SourceThousandths.Verify("usccss.g5.u05.l03.source_wodb_and_mai_error",extra1,"1"));
        Assert.False(UsCcssG5U05SourceThousandths.Verify("usccss.g5.u05.l03.source_wodb_and_mai_error",null,"1"));
        Assert.Contains("TEACHER_REVIEW_REQUIRED",UsCcssG5U05SourceThousandths.Build("usccss.g5.u05.l03.source_wodb_and_mai_error",new Random(5)).Prompt);
    }
}
