using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

// Written for Stage D; WRITTEN_NOT_RUN under Stage A–B–C.
public sealed class UaeGrade1ArabicThreeAddendsTests
{
    [Fact]
    public void TripleAddition_UsesScopedRoute_AndIndependentVerifier()
    {
        const string code="PED:UAE-MOE-MATH:L1:COMMON:04:09";
        const string family="uae.g1.u04.l09.add_three_with_strategy";
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(code,out var contract));
        Assert.Equal(family,Assert.Single(contract!.AllowedQuestionFamilies));
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
        var qs=new ExactSkillContractQuestionEngine().Generate("stage18",code,
            new[]{family},ExactSkillQuestionDifficulty.Standard,10,42009,Array.Empty<string>());
        Assert.NotEmpty(qs);
        Assert.All(qs,q=>{
            Assert.Equal(q.Parameters["a"]+q.Parameters["b"]+q.Parameters["c"],q.Parameters["total"]);
            Assert.InRange(q.Parameters["total"],3,20);
            Assert.True(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,q.CorrectAnswer));
            Assert.False(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,"99"));
        });
        var p=new Dictionary<string,int>{{"a",7},{"b",3},{"c",4},{"pair",10},{"total",14},{"mode",0}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,"١٤"));
        p["pair"]=9;
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,"١٤"));
        p["pair"]=10;p["mode"]=1;
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,"١٤"));
    }
}
