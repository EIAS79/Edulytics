using Edulytics.Services.Mathematics;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>G5 U03:L08 original flags authoring tests: AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG5U03L08FlagMeasureQuestionsTests
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
    public void OriginalSourceProductsAndMeasuredStripeCases(int c,int s,string expected)
    {
        var f=UsCcssG5U03L08FlagMeasureQuestions.Family;
        Assert.Equal(expected,UsCcssG5U03L08FlagMeasureQuestions.Solve(f,P(c,s)));
        Assert.True(UsCcssG5U03L08FlagMeasureQuestions.Verify(f,P(c,s),expected));
        Assert.False(UsCcssG5U03L08FlagMeasureQuestions.Verify(f,P(c,s),"0"));
    }
    [Fact]
    public void MalformedAndSourceTeacherReview()
    {
        var f=UsCcssG5U03L08FlagMeasureQuestions.Family;
        foreach(var p in new[]{P(-1),P(10),P(0,-1),P(0,3)})
            Assert.False(UsCcssG5U03L08FlagMeasureQuestions.Verify(f,p,"1"));
        var p0=P(0);p0["foreign"]=1;
        Assert.False(UsCcssG5U03L08FlagMeasureQuestions.Verify(f,p0,"1"));
        Assert.False(UsCcssG5U03L08FlagMeasureQuestions.Verify(f,null,"1"));
        Assert.False(UsCcssG5U03L08FlagMeasureQuestions.Verify(f,P(0)," 1"));
        var item=UsCcssG5U03L08FlagMeasureQuestions.Build(f,new Random(2));
        Assert.Contains("TEACHER_REVIEW_REQUIRED",item.Prompt);
    }
}
