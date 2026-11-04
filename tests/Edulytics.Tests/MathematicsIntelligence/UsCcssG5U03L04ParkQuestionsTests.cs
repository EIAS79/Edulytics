using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>G5 U03:L04 AUTHORING ONLY: AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG5U03L04ParkQuestionsTests
{
    private static Dictionary<string,int> P(int c,int slot=0) =>
        new(StringComparer.Ordinal) { ["case"]=c, ["slot"]=slot };
    [Theory]
    [InlineData(0, 0, "1")]
    [InlineData(1, 1, "2")]
    [InlineData(2, 2, "3")]
    [InlineData(3, 0, "1")]
    [InlineData(4, 1, "2")]
    public void NestedParkSourceChoices(int source,int slot,string expected)
    {
        var family=UsCcssG5U03L04ParkQuestions.NestedFamily;
        Assert.Equal(expected,UsCcssG5U03L04ParkQuestions.Solve(family,P(source,slot)));
        Assert.True(UsCcssG5U03L04ParkQuestions.Verify(family,P(source,slot),expected));
        Assert.False(UsCcssG5U03L04ParkQuestions.Verify(family,P(source,slot),"0"));
    }
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    public void ElenaSourceRegionChoices(int source)
    {
        var family=UsCcssG5U03L04ParkQuestions.RegionFamily;
        for(int slot=0;slot<3;slot++)
        {
            var p=P(source,slot);
            Assert.Equal((slot+1).ToString(), UsCcssG5U03L04ParkQuestions.Solve(family,p));
            Assert.True(UsCcssG5U03L04ParkQuestions.Verify(family,p,(slot+1).ToString()));
        }
    }
    [Fact]
    public void RejectOtherLessonAndMalformedParameters()
    {
        foreach(var family in new[] {UsCcssG5U03L04ParkQuestions.NestedFamily,
                                  UsCcssG5U03L04ParkQuestions.RegionFamily})
        {
            foreach(var invalid in new[] { P(-1),P(5),P(0,-1),P(0,3) })
                Assert.False(UsCcssG5U03L04ParkQuestions.Verify(family,invalid,"1"));
            var extra=P(1);extra["foreign"]=7;
            Assert.False(UsCcssG5U03L04ParkQuestions.Verify(family,extra,"1"));
            Assert.False(UsCcssG5U03L04ParkQuestions.Verify(family,null,"1"));
            Assert.False(UsCcssG5U03L04ParkQuestions.Verify(family,P(1)," 1"));
            Assert.False(UsCcssG5U03L04ParkQuestions.Verify(family,P(1),"1/10"));
        }
        Assert.False(UsCcssG5U03L04ParkQuestions.Verify(
            UsCcssG5U03L04ParkQuestions.RegionFamily,P(0),"1"));
        var item=UsCcssG5U03L04ParkQuestions.Build(
            UsCcssG5U03L04ParkQuestions.NestedFamily,new Random(9));
        Assert.Contains("TEACHER_REVIEW_REQUIRED",item.Prompt);
        Assert.Contains("not evidence of correct freehand diagrams",item.Solution);
    }
}
