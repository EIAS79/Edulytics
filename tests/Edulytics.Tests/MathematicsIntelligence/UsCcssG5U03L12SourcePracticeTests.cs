using Edulytics.Services.Mathematics;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>G5 U03:L12 original source bounded C fixtures AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG5U03L12SourcePracticeTests
{
    private static Dictionary<string,int> P(int c,int s=0)=>
        new(StringComparer.Ordinal){["case"]=c,["slot"]=s};
    [Theory]
    [InlineData(0,0,"1")]
    [InlineData(1,1,"2")]
    [InlineData(2,2,"3")]
    public void Family1ExactSourceCase(int c,int slot,string expected)
    {
        const string family="usccss.g5.u03.l12.third_division_teacher_diagrams";
        Assert.Equal(expected,UsCcssG5U03SourceDivisionQuestions.Solve(family,P(c,slot)));
        Assert.True(UsCcssG5U03SourceDivisionQuestions.Verify(family,P(c,slot),expected));
        Assert.False(UsCcssG5U03SourceDivisionQuestions.Verify(family,P(c,slot),"0"));
    }
    [Theory]
    [InlineData(0,0,"1")]
    [InlineData(1,1,"2")]
    [InlineData(2,2,"3")]
    public void Family2ExactSourceCase(int c,int slot,string expected)
    {
        const string family="usccss.g5.u03.l12.quarter_division_pattern";
        Assert.Equal(expected,UsCcssG5U03SourceDivisionQuestions.Solve(family,P(c,slot)));
        Assert.True(UsCcssG5U03SourceDivisionQuestions.Verify(family,P(c,slot),expected));
        Assert.False(UsCcssG5U03SourceDivisionQuestions.Verify(family,P(c,slot),"0"));
    }
    [Fact]
    public void MalformedAndTeacherReviewed()
    {
        foreach(var p in new[]{P(-1),P(99),P(0,-1),P(0,3)})
            Assert.False(UsCcssG5U03SourceDivisionQuestions.Verify("usccss.g5.u03.l12.third_division_teacher_diagrams",p,"1"));
        var extra0=P(0);extra0["foreign"]=1;
        Assert.False(UsCcssG5U03SourceDivisionQuestions.Verify("usccss.g5.u03.l12.third_division_teacher_diagrams",extra0,"1"));
        Assert.False(UsCcssG5U03SourceDivisionQuestions.Verify("usccss.g5.u03.l12.third_division_teacher_diagrams",null,"1"));
        Assert.Contains("TEACHER_REVIEW_REQUIRED",UsCcssG5U03SourceDivisionQuestions.Build("usccss.g5.u03.l12.third_division_teacher_diagrams",new Random(5)).Prompt);
        foreach(var p in new[]{P(-1),P(99),P(0,-1),P(0,3)})
            Assert.False(UsCcssG5U03SourceDivisionQuestions.Verify("usccss.g5.u03.l12.quarter_division_pattern",p,"1"));
        var extra1=P(0);extra1["foreign"]=1;
        Assert.False(UsCcssG5U03SourceDivisionQuestions.Verify("usccss.g5.u03.l12.quarter_division_pattern",extra1,"1"));
        Assert.False(UsCcssG5U03SourceDivisionQuestions.Verify("usccss.g5.u03.l12.quarter_division_pattern",null,"1"));
        Assert.Contains("TEACHER_REVIEW_REQUIRED",UsCcssG5U03SourceDivisionQuestions.Build("usccss.g5.u03.l12.quarter_division_pattern",new Random(5)).Prompt);
    }
}
