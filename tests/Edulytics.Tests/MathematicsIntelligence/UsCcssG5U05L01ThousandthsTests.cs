using Edulytics.Services.Mathematics;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>Source IM2021 Grade5 U05:L01 thousandths choices; AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG5U05L01ThousandthsTests
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
    public void OriginalSourceDecimalsAndTenToOneUnits(int c,int slot,string expected)
    {
        var f=UsCcssG5U05SourceThousandths.Family;
        Assert.Equal(expected,UsCcssG5U05SourceThousandths.Solve(f,P(c,slot)));
        Assert.True(UsCcssG5U05SourceThousandths.Verify(f,P(c,slot),expected));
        Assert.False(UsCcssG5U05SourceThousandths.Verify(f,P(c,slot),"0"));
    }
    [Fact]
    public void RejectInvalidParametersAndKeepOriginalTeacherBoundary()
    {
        var f=UsCcssG5U05SourceThousandths.Family;
        foreach(var p in new[]{P(-1),P(6),P(0,-1),P(0,3)})
            Assert.False(UsCcssG5U05SourceThousandths.Verify(f,p,"1"));
        var x=P(0);x["foreign"]=2;
        Assert.False(UsCcssG5U05SourceThousandths.Verify(f,x,"1"));
        Assert.False(UsCcssG5U05SourceThousandths.Verify(f,null,"1"));
        var q=UsCcssG5U05SourceThousandths.Build(f,new Random(2));
        Assert.Contains("TEACHER_REVIEW_REQUIRED",q.Prompt);
    }
}
