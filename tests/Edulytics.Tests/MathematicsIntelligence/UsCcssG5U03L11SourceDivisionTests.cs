using Edulytics.Services.Mathematics;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>Grade5 U03:L11 source half-pan and four-divisor fixtures, AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG5U03L11SourceDivisionTests
{
    private static Dictionary<string,int> P(int c,int slot=0)=>
        new(StringComparer.Ordinal){["case"]=c,["slot"]=slot};
    [Theory]
    [InlineData(0,0,"1")]
    [InlineData(1,1,"2")]
    [InlineData(2,2,"3")]
    public void OriginalHalfPanThreeFourFivePeople(int c,int slot,string answer)
    {
        var f="usccss.g5.u03.l11.mac_cheese_half_pan_equal_shares";
        Assert.Equal(answer,UsCcssG5U03SourceDivisionQuestions.Solve(f,P(c,slot)));
        Assert.True(UsCcssG5U03SourceDivisionQuestions.Verify(f,P(c,slot),answer));
        Assert.False(UsCcssG5U03SourceDivisionQuestions.Verify(f,P(c,slot),"1/6"));
    }
    [Theory]
    [InlineData(0,0,"1")]
    [InlineData(1,1,"2")]
    [InlineData(2,2,"3")]
    [InlineData(3,0,"1")]
    public void SourceDoubleDivisorWarmup(int c,int slot,string answer)
    {
        var f="usccss.g5.u03.l11.four_divisor_number_talk";
        Assert.True(UsCcssG5U03SourceDivisionQuestions.Verify(f,P(c,slot),answer));
    }
    [Fact]
    public void InvalidOrForeignAndHumanReview()
    {
        foreach(var f in new[]{"usccss.g5.u03.l11.mac_cheese_half_pan_equal_shares",
            "usccss.g5.u03.l11.four_divisor_number_talk"})
        {
            foreach(var p in new[]{P(-1),P(7),P(0,-1),P(0,3)})
                Assert.False(UsCcssG5U03SourceDivisionQuestions.Verify(f,p,"1"));
            var extra=P(0);extra["foreign"]=42;
            Assert.False(UsCcssG5U03SourceDivisionQuestions.Verify(f,extra,"1"));
            Assert.False(UsCcssG5U03SourceDivisionQuestions.Verify(f,null,"1"));
            Assert.False(UsCcssG5U03SourceDivisionQuestions.Verify(f,P(0)," 1"));
            Assert.Contains("TEACHER_REVIEW_REQUIRED",
                UsCcssG5U03SourceDivisionQuestions.Build(f,new Random(2)).Prompt);
        }
    }
}
