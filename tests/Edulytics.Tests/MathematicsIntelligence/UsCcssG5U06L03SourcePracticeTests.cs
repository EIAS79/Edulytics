using Edulytics.Services.Mathematics;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>G5 U06:L03 original source bounded C fixtures AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG5U06L03SourcePracticeTests
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
    [InlineData(6,0,"1")]
    [InlineData(7,1,"2")]
    [InlineData(8,2,"3")]
    [InlineData(9,0,"1")]
    [InlineData(10,1,"2")]
    [InlineData(11,2,"3")]
    [InlineData(12,0,"1")]
    [InlineData(13,1,"2")]
    [InlineData(14,2,"3")]
    [InlineData(15,0,"1")]
    [InlineData(16,1,"2")]
    [InlineData(17,2,"3")]
    [InlineData(18,0,"1")]
    public void Family1ExactSourceCase(int c,int slot,string expected)
    {
        const string family="usccss.g5.u06.l03.original_metric_conversions";
        Assert.Equal(expected,UsCcssG5U05SourceThousandths.Solve(family,P(c,slot)));
        Assert.True(UsCcssG5U05SourceThousandths.Verify(family,P(c,slot),expected));
        Assert.False(UsCcssG5U05SourceThousandths.Verify(family,P(c,slot),"0"));
    }
    [Fact]
    public void MalformedAndTeacherReviewed()
    {
        foreach(var p in new[]{P(-1),P(99),P(0,-1),P(0,3)})
            Assert.False(UsCcssG5U05SourceThousandths.Verify("usccss.g5.u06.l03.original_metric_conversions",p,"1"));
        var extra0=P(0);extra0["foreign"]=1;
        Assert.False(UsCcssG5U05SourceThousandths.Verify("usccss.g5.u06.l03.original_metric_conversions",extra0,"1"));
        Assert.False(UsCcssG5U05SourceThousandths.Verify("usccss.g5.u06.l03.original_metric_conversions",null,"1"));
        Assert.Contains("TEACHER_REVIEW_REQUIRED",UsCcssG5U05SourceThousandths.Build("usccss.g5.u06.l03.original_metric_conversions",new Random(5)).Prompt);
    }
}
