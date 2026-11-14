using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// Written for Stage D, NOT executed during Stage A-B-C.
public sealed class UaeGrade5Unit8Lesson04UnlikeModelTests
{
    private const string L="PED:UAE-MOE-MATH:L5:GENERAL:08:04";
    [Fact]
    public void AllThreeFamiliesAreExactAndRouted()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(L,out var c));
        Assert.Equal("fractions.add",c!.SkillId);
        Assert.Equal(3,c.AllowedQuestionFamilies.Count);
        foreach(var f in c.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(f,out var ui));
            Assert.NotNull(ui);
        }
    }
    [Fact]
    public void TwoThirdsPlusThreeQuartersHasExactSimplifiedSum()
    {
        const string f="uae.g5.u08.l04.model_unlike_sum_simplified";
        var p=new Dictionary<string,int>{{"n1",2},{"d1",3},{"n2",3},{"d2",4}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(f,p,"١٧/١٢"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(f,p,"34/24"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(f,p,"5/7"));
        p["d1"]=4;
        Assert.False(ExactSkillContractQuestionEngine.Verify(f,p,"17/12"));
    }
    [Fact]
    public void RandomQuestionsHaveIndependentVerifier()
    {
        var engine=new ExactSkillContractQuestionEngine();
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(L,out var c));
        foreach(var f in c!.AllowedQuestionFamilies)
        for(int seed=1;seed<=8;seed++)
        foreach(var q in engine.Generate(L,f,[f],ExactSkillQuestionDifficulty.Standard,2,seed,[]))
        {
            Assert.True(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,q.CorrectAnswer));
            Assert.False(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,"-9"));
        }
    }
}