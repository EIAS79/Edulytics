using Edulytics.Services.Mathematics;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>Source G5 U04:L10 folk dance choices, AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG5U04L10FolkDanceTests
{
    private static Dictionary<string,int> P(int c,int s=0)=>
        new(StringComparer.Ordinal){["case"]=c,["slot"]=s};
    [Theory]
    [InlineData(0,0,"1")]
    [InlineData(1,1,"2")]
    [InlineData(2,2,"3")]
    public void Original4704GroupDivisions(int c,int s,string expected)
    {
        var f=UsCcssG5U04WholeNumberSourceDivision.L10Dance;
        Assert.Equal(expected,UsCcssG5U04WholeNumberSourceDivision.Solve(f,P(c,s)));
        Assert.True(UsCcssG5U04WholeNumberSourceDivision.Verify(f,P(c,s),expected));
        Assert.False(UsCcssG5U04WholeNumberSourceDivision.Verify(f,P(c,s),"0"));
    }
    [Fact]
    public void RejectForeignOrMalformedAndPreserveTeacherBoundary()
    {
        var f=UsCcssG5U04WholeNumberSourceDivision.L10Dance;
        foreach(var p in new[]{P(-1),P(4),P(0,-1),P(0,3)})
            Assert.False(UsCcssG5U04WholeNumberSourceDivision.Verify(f,p,"1"));
        Assert.True(UsCcssG5U04WholeNumberSourceDivision.Verify(f,P(3,1),"2"));
        var extra=P(0);extra["unknown"]=9;
        Assert.False(UsCcssG5U04WholeNumberSourceDivision.Verify(f,extra,"1"));
        Assert.False(UsCcssG5U04WholeNumberSourceDivision.Verify(f,null,"1"));
        Assert.False(UsCcssG5U04WholeNumberSourceDivision.Verify(f,P(0)," 1"));
        var q=UsCcssG5U04WholeNumberSourceDivision.Build(f,new Random(11));
        Assert.Contains("TEACHER_REVIEW_REQUIRED",q.Prompt);
    }
}
