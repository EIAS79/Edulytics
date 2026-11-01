using Edulytics.Services.Mathematics;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>U03:L06 source exact numeric authored tests, NOT_RUN.</summary>
public sealed class UsCcssG5U03L06NonunitAreaQuestionsTests
{
    private static Dictionary<string,int> P(int c,int s=0) =>
        new(StringComparer.Ordinal){["case"]=c,["slot"]=s};
    [Theory]
    [InlineData(0,0,"1")]
    [InlineData(1,1,"2")]
    [InlineData(2,2,"3")]
    public void SourceCases(int c,int s,string expected)
    {
        var f=UsCcssG5U03L06NonunitAreaQuestions.Family;
        Assert.Equal(expected,UsCcssG5U03L06NonunitAreaQuestions.Solve(f,P(c,s)));
        Assert.True(UsCcssG5U03L06NonunitAreaQuestions.Verify(f,P(c,s),expected));
        Assert.False(UsCcssG5U03L06NonunitAreaQuestions.Verify(f,P(c,s),"0"));
    }
    [Fact]
    public void MalformedAndTeacher()
    {
        var f=UsCcssG5U03L06NonunitAreaQuestions.Family;
        foreach(var p in new[]{P(-1),P(3),P(0,-1),P(0,3)})
            Assert.False(UsCcssG5U03L06NonunitAreaQuestions.Verify(f,p,"1"));
        var extra=P(1);extra["foreign"]=2;
        Assert.False(UsCcssG5U03L06NonunitAreaQuestions.Verify(f,extra,"1"));
        Assert.False(UsCcssG5U03L06NonunitAreaQuestions.Verify(f,null,"1"));
        Assert.False(UsCcssG5U03L06NonunitAreaQuestions.Verify(f,P(0)," 1"));
        Assert.False(UsCcssG5U03L06NonunitAreaQuestions.Supports(f+".foreign"));
        Assert.Contains("TEACHER_REVIEW_REQUIRED",
            UsCcssG5U03L06NonunitAreaQuestions.Build(f,new Random(2)).Prompt);
    }
}
