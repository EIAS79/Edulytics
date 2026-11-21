using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class UaeG9GeneralU07L07NonmonicQuadraticTests
{
    private const string Family="uae.g9.general.u07.l07.nonmonic_integer_roots";
    private const string Lesson="PED:UAE-MOE-MATH:L9:GENERAL:07:07:NONMONIC-QUADRATIC-FACTORING";

    [Fact]
    public void CrossProductVerifierAcceptsOnlyOrderedCorrectRoots()
    {
        // 2(x+3)(x-4) = 2x²-2x-24; exact roots -3 and 4.
        var p=new Dictionary<string,int>
        {
            ["a"]=2, ["r1"]=-3, ["r2"]=4, ["b"]=-2, ["c"]=-24
        };
        Assert.True(ExactSkillContractQuestionEngine.Verify(Family,p,"−٣،٤"));
        Assert.True(ExactSkillContractQuestionEngine.Verify(Family,p,"-3,4"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family,p,"4,-3"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family,p,"-3,4,"));
        p["c"]=-23;
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family,p,"-3,4"));
        p["c"]=-24;
        p["a"]=1;
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family,p,"-3,4"));
    }

    [Fact]
    public void ArabicGeneratedQuadraticAlwaysMatchesItsCoefficients()
    {
        var engine=new ExactSkillContractQuestionEngine();
        for(int seed=1;seed<=15;seed++)
        {
            var q=Assert.Single(engine.Generate("UAE-G9-GENERAL",Lesson,
                new[]{Family},ExactSkillQuestionDifficulty.Standard,
                1,seed,Array.Empty<string>()));
            Assert.Contains("س²",q.Prompt);
            Assert.True(q.Parameters["a"]>1);
            Assert.Equal(-q.Parameters["a"]*(q.Parameters["r1"]+q.Parameters["r2"]),q.Parameters["b"]);
            Assert.Equal(q.Parameters["a"]*q.Parameters["r1"]*q.Parameters["r2"],q.Parameters["c"]);
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.False(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,"999"));
        }
    }
}
