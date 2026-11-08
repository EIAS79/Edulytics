using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

// WRITTEN_NOT_RUN in A–B–C.
public sealed class UaeGrade1ArabicCommutativeAdditionTests
{
    [Fact]
    public void SwappedAddends_PreserveSumAndCheckMissingAddend()
    {
        const string code="PED:UAE-MOE-MATH:L1:COMMON:04:08";
        const string family="uae.g1.u04.l08.commutative_add";
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(code, out var route));
        Assert.Equal(family, Assert.Single(route!.AllowedQuestionFamilies));
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
        var q=new ExactSkillContractQuestionEngine().Generate("stage18",code,
            new[]{family},ExactSkillQuestionDifficulty.Standard,10,41008,Array.Empty<string>());
        Assert.NotEmpty(q);
        Assert.All(q,x=>{
            Assert.Equal(x.Parameters["a"]+x.Parameters["b"],x.Parameters["total"]);
            Assert.True(ExactSkillContractQuestionEngine.Verify(family,x.Parameters,x.CorrectAnswer));
            Assert.False(ExactSkillContractQuestionEngine.Verify(family,x.Parameters,"99"));
        });
        var p=new Dictionary<string,int>{{"a",7},{"b",4},{"total",11},{"mode",0},{"scene",0}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,"٧"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,"١١"));
        p["mode"]=1;
        Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,"١١"));
        p["total"]=12;
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,"١١"));
    }
}
