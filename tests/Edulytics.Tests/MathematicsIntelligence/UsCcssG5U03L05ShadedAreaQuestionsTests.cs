using Edulytics.Services.Mathematics;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>Grade 5 U03:L05 authoring-only fixture, AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG5U03L05ShadedAreaQuestionsTests
{
    private static Dictionary<string,int> P(int c,int s=0) =>
        new(StringComparer.Ordinal) {["case"]=c,["slot"]=s};
    [Theory]
    [InlineData(0,0,"1")]
    [InlineData(1,1,"2")]
    [InlineData(2,2,"3")]
    public void ExactSourceAreaSelection(int c,int s,string expected)
    {
        var f=UsCcssG5U03L05ShadedAreaQuestions.Family;
        Assert.Equal(expected,UsCcssG5U03L05ShadedAreaQuestions.Solve(f,P(c,s)));
        Assert.True(UsCcssG5U03L05ShadedAreaQuestions.Verify(f,P(c,s),expected));
        Assert.False(UsCcssG5U03L05ShadedAreaQuestions.Verify(f,P(c,s),"1/3"));
    }
    [Fact]
    public void BoundedMalformedAndTeacherBoundary()
    {
        var f=UsCcssG5U03L05ShadedAreaQuestions.Family;
        foreach(var p in new[] {P(-1),P(3),P(0,-1),P(0,3)})
            Assert.False(UsCcssG5U03L05ShadedAreaQuestions.Verify(f,p,"1"));
        var extra=P(0);extra["foreign"]=7;
        Assert.False(UsCcssG5U03L05ShadedAreaQuestions.Verify(f,extra,"1"));
        Assert.False(UsCcssG5U03L05ShadedAreaQuestions.Verify(f,null,"1"));
        Assert.False(UsCcssG5U03L05ShadedAreaQuestions.Verify(f,P(0)," 1"));
        Assert.False(UsCcssG5U03L05ShadedAreaQuestions.Supports(f+".foreign"));
        var item=UsCcssG5U03L05ShadedAreaQuestions.Build(f,new Random(5));
        Assert.Contains("TEACHER_REVIEW_REQUIRED",item.Prompt);
    }
}
