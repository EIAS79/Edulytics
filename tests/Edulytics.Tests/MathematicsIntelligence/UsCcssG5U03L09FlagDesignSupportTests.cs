using Edulytics.Services.Mathematics;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>U03:L09 optional-design supplemental math, AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG5U03L09FlagDesignSupportTests
{
    private static Dictionary<string,int> P(int c,int slot=0)=>
        new(StringComparer.Ordinal){["case"]=c,["slot"]=slot};
    [Theory]
    [InlineData(0,0,"1")]
    [InlineData(1,1,"2")]
    [InlineData(2,2,"3")]
    [InlineData(3,0,"1")]
    public void OptionalFixedModelNotStudentDesign(int c,int slot,string expected)
    {
        var f=UsCcssG5U03L09FlagDesignSupportQuestions.Family;
        Assert.True(UsCcssG5U03L09FlagDesignSupportQuestions.Verify(f,P(c,slot),expected));
        Assert.Equal(expected,UsCcssG5U03L09FlagDesignSupportQuestions.Solve(f,P(c,slot)));
        Assert.False(UsCcssG5U03L09FlagDesignSupportQuestions.Verify(f,P(c,slot),"0"));
    }
    [Fact]
    public void RejectMalformedAndKeepSourceDrawingTeacherLed()
    {
        var f=UsCcssG5U03L09FlagDesignSupportQuestions.Family;
        foreach(var p in new[]{P(-1),P(4),P(0,-1),P(0,3)})
            Assert.False(UsCcssG5U03L09FlagDesignSupportQuestions.Verify(f,p,"1"));
        var extra=P(0);extra["foreign"]=7;
        Assert.False(UsCcssG5U03L09FlagDesignSupportQuestions.Verify(f,extra,"1"));
        Assert.False(UsCcssG5U03L09FlagDesignSupportQuestions.Verify(f,null,"1"));
        var item=UsCcssG5U03L09FlagDesignSupportQuestions.Build(f,new Random(4));
        Assert.Contains("TEACHER_REVIEW_REQUIRED",item.Prompt);
        Assert.Contains("does not score any student's original",item.Solution);
    }
}
