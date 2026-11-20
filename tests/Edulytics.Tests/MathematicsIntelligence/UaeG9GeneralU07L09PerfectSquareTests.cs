using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeG9GeneralU07L09PerfectSquareTests
{
    private const string Family="uae.g9.general.u07.l09.square_root_equation_integer_roots";
    private const string Lesson="PED:UAE-MOE-MATH:L9:GENERAL:07:09:PERFECT-SQUARE-TRINOMIALS";
    [Fact]
    public void SquareRootEquationVerifierRequiresBothCorrectOrderedRoots()
    {
        // (x-2)^2=25 is equivalent to x²-4x-21=0.
        var p=new Dictionary<string,int>{{"r1",-3},{"r2",7},{"b",-4},{"c",-21}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(Family,p,"−٣،٧"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family,p,"7,-3"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family,p,"-3"));
        p["r2"]=6; p["b"]=-3; p["c"]=-18;
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family,p,"-3,6"));
    }
    [Fact]
    public void GeneratedSquareRootEquationsRespectMidpointAndRadius()
    {
        var engine=new ExactSkillContractQuestionEngine();
        for(int seed=1;seed<=15;seed++)
        {
            var q=Assert.Single(engine.Generate("UAE-G9-GENERAL",Lesson,new[]{Family},
                ExactSkillQuestionDifficulty.Standard,1,seed,Array.Empty<string>()));
            var p=q.Parameters;
            Assert.Equal(0,(p["r1"]+p["r2"])%2);
            Assert.True(p["r2"]>p["r1"]);
            Assert.Equal(-(p["r1"]+p["r2"]),p["b"]);
            Assert.Equal(p["r1"]*p["r2"],p["c"]);
            Assert.Contains("²=",q.Prompt);
            Assert.True(ExactSkillContractQuestionEngine.Verify(Family,p,q.CorrectAnswer));
        }
    }
}
