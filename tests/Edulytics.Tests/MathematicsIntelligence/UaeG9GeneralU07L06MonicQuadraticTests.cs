using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class UaeG9GeneralU07L06MonicQuadraticTests
{
    private const string Distinct="uae.g9.general.u07.l06.monic_distinct_roots";
    private const string Repeated="uae.g9.general.u07.l06.monic_repeated_root";
    private const string Lesson="PED:UAE-MOE-MATH:L9:GENERAL:07:06:MONIC-QUADRATIC-FACTORING";

    [Theory]
    [InlineData(Distinct,-3,4, -1, -12,"−٣،٤")]
    [InlineData(Distinct,-4,-3,7,12,"-4,-3")]
    [InlineData(Repeated,4,4,-8,16,"٤")]
    public void VerifierAcceptsOnlyCorrectIntegerRoots(
        string family,int r1,int r2,int linear,int constant,string answer)
    {
        var p=new Dictionary<string,int> { ["r1"]=r1, ["r2"]=r2, ["b"]=linear, ["c"]=constant };
        Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,answer));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,"99"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,""));
        if(family==Distinct)
        {
            Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,$"{r2},{r1}"));
            Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,$"{r1},{r2},"));
        }
        else
            Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,$"{r1},{r2}"));
    }

    [Fact]
    public void InvalidCoefficientOrUnsortedRootDataIsRejected()
    {
        var p=new Dictionary<string,int> { ["r1"]=-3, ["r2"]=4, ["b"]=99, ["c"]=-12 };
        Assert.False(ExactSkillContractQuestionEngine.Verify(Distinct,p,"-3,4"));
        p["b"]=-1;
        p["r1"]=4; p["r2"]=-3;
        Assert.False(ExactSkillContractQuestionEngine.Verify(Distinct,p,"-3,4"));
    }

    [Theory]
    [InlineData(Distinct)]
    [InlineData(Repeated)]
    public void GeneratedArabicQuestionUsesTheIndependentVerifier(string family)
    {
        var engine=new ExactSkillContractQuestionEngine();
        for(int seed=1;seed<=12;seed++)
        {
            var q=Assert.Single(engine.Generate("UAE-G9-GENERAL",Lesson,new[]{family},
                ExactSkillQuestionDifficulty.Standard,1,seed,Array.Empty<string>()));
            Assert.Contains("حل المعادلة س²",q.Prompt);
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.False(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,"١٠٠٠"));
        }
    }
}
