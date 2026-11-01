using Edulytics.Services.Mathematics;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>G5 U03:L07 source product and missing factor AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG5U03L07FractionEquationQuestionsTests
{
    private static Dictionary<string,int> P(int c,int slot=0)=>
        new(StringComparer.Ordinal){["case"]=c,["slot"]=slot};
    [Theory]
    [InlineData(0,0,"1")]
    [InlineData(1,1,"2")]
    [InlineData(2,2,"3")]
    [InlineData(3,0,"1")]
    [InlineData(4,1,"2")]
    [InlineData(5,2,"3")]
    public void SixSourceProducts(int c,int slot,string expected)
    {
        var f=UsCcssG5U03L07FractionEquationQuestions.ProductFamily;
        Assert.Equal(expected,UsCcssG5U03L07FractionEquationQuestions.Solve(f,P(c,slot)));
        Assert.True(UsCcssG5U03L07FractionEquationQuestions.Verify(f,P(c,slot),expected));
        Assert.False(UsCcssG5U03L07FractionEquationQuestions.Verify(f,P(c,slot),"0"));
    }
    [Theory]
    [InlineData(0,0,"1")]
    [InlineData(1,2,"3")]
    public void SourceMissingFactors(int c,int slot,string expected)
    {
        var f=UsCcssG5U03L07FractionEquationQuestions.MissingFamily;
        Assert.True(UsCcssG5U03L07FractionEquationQuestions.Verify(f,P(c,slot),expected));
        Assert.False(UsCcssG5U03L07FractionEquationQuestions.Verify(f,P(c,slot),"7/4"));
    }
    [Fact]
    public void MalformedAndTeacherBoundary()
    {
        foreach(var f in new[]{UsCcssG5U03L07FractionEquationQuestions.ProductFamily,
                                UsCcssG5U03L07FractionEquationQuestions.MissingFamily})
        {
            foreach(var p in new[]{P(-1),P(6),P(0,-1),P(0,3)})
                Assert.False(UsCcssG5U03L07FractionEquationQuestions.Verify(f,p,"1"));
            var extra=P(0);extra["foreign"]=9;
            Assert.False(UsCcssG5U03L07FractionEquationQuestions.Verify(f,extra,"1"));
            Assert.False(UsCcssG5U03L07FractionEquationQuestions.Verify(f,null,"1"));
            Assert.False(UsCcssG5U03L07FractionEquationQuestions.Verify(f,P(0)," 1"));
            Assert.Contains("TEACHER_REVIEW_REQUIRED",
                UsCcssG5U03L07FractionEquationQuestions.Build(f,new Random(4)).Prompt);
        }
    }
}
