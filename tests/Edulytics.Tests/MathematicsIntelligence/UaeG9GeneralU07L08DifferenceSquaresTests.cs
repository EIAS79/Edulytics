using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeG9GeneralU07L08DifferenceSquaresTests
{
    private const string Family="uae.g9.general.u07.l08.difference_squares_integer_roots";
    private const string Lesson="PED:UAE-MOE-MATH:L9:GENERAL:07:08:DIFFERENCE-OF-SQUARES";
    [Fact]
    public void SymmetricRootsAndSourceSignChecks()
    {
        var p=new Dictionary<string,int> {["r1"]=-5,["r2"]=5,["b"]=0,["c"]=-25};
        Assert.True(ExactSkillContractQuestionEngine.Verify(Family,p,"−٥،٥"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family,p,"٥،−٥"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family,p,"-5"));
        p["c"]=25;
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family,p,"-5,5"));
    }
    [Fact]
    public void GeneratedDifferenceSquaresHasZeroMiddleCoefficient()
    {
        var engine=new ExactSkillContractQuestionEngine();
        for(int seed=1;seed<=15;seed++)
        {
            var q=Assert.Single(engine.Generate("UAE-G9-GENERAL",Lesson,
                new[]{Family},ExactSkillQuestionDifficulty.Standard,
                1,seed,Array.Empty<string>()));
            Assert.Equal(0,q.Parameters["b"]);
            Assert.Equal(-q.Parameters["r2"],q.Parameters["r1"]);
            Assert.Equal(-q.Parameters["r2"]*q.Parameters["r2"],q.Parameters["c"]);
            Assert.Contains("س²−",q.Prompt);
            Assert.True(ExactSkillContractQuestionEngine.Verify(Family,q.Parameters,q.CorrectAnswer));
        }
    }
}
