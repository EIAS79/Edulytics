using Edulytics.Services.Mathematics;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>G5 U08:L07 original source bounded C fixtures AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG5U08L07SourcePracticeTests
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
        const string family="usccss.g5.u08.l07.source_wagon_photo_estimation";
        Assert.Equal(expected,UsCcssG5U05SourceThousandths.Solve(family,P(c,slot)));
        Assert.True(UsCcssG5U05SourceThousandths.Verify(family,P(c,slot),expected));
        Assert.False(UsCcssG5U05SourceThousandths.Verify(family,P(c,slot),"0"));
    }
    [Fact]
    public void MalformedAndTeacherReviewed()
    {
        foreach(var p in new[]{P(-1),P(99),P(0,-1),P(0,3)})
            Assert.False(UsCcssG5U05SourceThousandths.Verify("usccss.g5.u08.l07.source_wagon_photo_estimation",p,"1"));
        var extra0=P(0);extra0["foreign"]=1;
        Assert.False(UsCcssG5U05SourceThousandths.Verify("usccss.g5.u08.l07.source_wagon_photo_estimation",extra0,"1"));
        Assert.False(UsCcssG5U05SourceThousandths.Verify("usccss.g5.u08.l07.source_wagon_photo_estimation",null,"1"));
        Assert.Contains("TEACHER_REVIEW_REQUIRED",UsCcssG5U05SourceThousandths.Build("usccss.g5.u08.l07.source_wagon_photo_estimation",new Random(5)).Prompt);
    }
}
