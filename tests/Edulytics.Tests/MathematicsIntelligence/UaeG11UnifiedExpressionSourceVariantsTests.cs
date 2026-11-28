using System.Globalization;
using Edulytics.Services.Mathematics;
using Edulytics.Core.Mathematics.Practice;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class UaeG11UnifiedExpressionSourceVariantsTests
{
    const string Code = "PED:UAE-MOE-MATH:L11:UNIFIED:01:01";
    const string Expression = "uae.g11.unified.u01.l01.source_expression";
    const string Formula = "uae.g11.unified.u01.l01.source_formula";
    const string Rounded = "uae.g11.unified.u01.l01.rounded_formula";
    static Dictionary<string,int> P(int m,int a,int b,int c,int d) =>
        new() { ["mode"]=m,["a"]=a,["b"]=b,["c"]=c,["d"]=d };

    [Theory]
    [InlineData(Expression,0,-20,30,15,10,"-8")]
    [InlineData(Expression,5,20,10,30,40,"13/8")]
    [InlineData(Expression,21,20,-10,10,20,"11")]
    [InlineData(Expression,26,20,10,30,40,"11/5")]
    [InlineData(Expression,28,20,10,30,40,"10/29")]
    [InlineData(Formula,1,8,2,5,1,"20")]
    [InlineData(Formula,2,1,1,1,1,"65/9")]
    [InlineData(Formula,8,3,4,5,1,"20")]
    [InlineData(Formula,9,3,4,5,1,"15")]
    [InlineData(Formula,15,2,1,1,1,"24")]
    [InlineData(Formula,16,2,1,1,1,"6")]
    [InlineData(Formula,18,1,2,3,4,"5/8")]
    [InlineData(Formula,20,2,3,4,1,"5")]
    [InlineData(Formula,21,6,1,1,1,"96")]
    [InlineData(Formula,22,2,3,4,1,"1280")]
    [InlineData(Rounded,0,1,3,1,1,"3.3")]
    [InlineData(Rounded,3,1,2,3,1,"6")]
    [InlineData(Formula,0,4,1,5,1,"30")]
    [InlineData(Formula,20,2,3,4,12,"5")]
    public void IndependentGoldens(string family,int mode,int a,int b,int c,int d,string expected)
    {
        var p=P(mode,a,b,c,d);
        Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,expected));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,"not numeric"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,"999999"));
        string ar=string.Concat(expected.Select(ch=>ch is >= '0' and <= '9'?(char)('٠'+ch-'0'):ch=='.'?'٫':ch=='-'?'−':ch));
        Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,ar));
    }

    [Fact]
    public void RejectsZeroDenominatorsAndInvalidModes()
    {
        Assert.False(ExactSkillContractQuestionEngine.Verify(Expression,P(15,10,20,30,30),"0"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Expression,P(99,10,20,30,40),"0"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Formula,P(18,1,0,3,4),"0"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Expression,P(0,10,20,30,40),"1/0"));
    }

    [Theory]
    [InlineData(Expression,29)]
    [InlineData(Formula,23)]
    [InlineData(Rounded,4)]
    public void FixedSeedsCoverEveryBoundedSourceVariant(string family,int count)
    {
        var modes=new HashSet<int>();
        var engine=new ExactSkillContractQuestionEngine();
        foreach(int seed in Enumerable.Range(20261128,30))
        foreach(var item in engine.Generate(Code,Code,[family],ExactSkillQuestionDifficulty.Standard,20,seed,[]))
        {
            modes.Add(item.Parameters["mode"]);
            Assert.True(ExactSkillContractQuestionEngine.Verify(family,item.Parameters,item.CorrectAnswer));
            Assert.False(ExactSkillContractQuestionEngine.Verify(family,item.Parameters,"999999"));
            Assert.Contains(item.Prompt,ch=>ch is >= '\u0600' and <= '\u06ff');
            Assert.Equal(5,item.Parameters.Count);
        }
        Assert.Equal(Enumerable.Range(0,count),modes.OrderBy(x=>x));
    }
}
