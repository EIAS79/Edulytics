using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// Original Arabic Grade5 student pp271–276; tests written NOT_RUN.
public sealed class UaeGrade5Unit4Lesson04AdjustQuotientTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L5:GENERAL:04:04";
    [Fact]
    public void TrialAdjustmentHasThreeExactRegisteredFamilies()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var c));
        Assert.Equal("number.base_ten_division_with_remainders",c!.SkillId);
        Assert.Equal(3,c.AllowedQuestionFamilies.Count);
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var f in c.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(f,out var ui));
            Assert.NotNull(ui);
            for(int seed=1;seed<=8;seed++)
            foreach(var q in engine.Generate(Lesson,f,[f],ExactSkillQuestionDifficulty.Standard,1,seed,[]))
            {
                Assert.True(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,q.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,"-999"));
            }
        }
    }
    [Fact]
    public void HighAndLowTrialsAreCorrectedByInequalities()
    {
        var p=new Dictionary<string,int>{{"dividend",238},{"divisor",62},{"trial",4}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u04.l04.corrected_trial_quotient",p,"٣"));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u04.l04.trial_too_high_indicator",p,"1"));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u04.l04.adjusted_remainder",p,"٥٢"));
        p["dividend"]=144;p["divisor"]=16;p["trial"]=8;
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u04.l04.corrected_trial_quotient",p,"9"));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u04.l04.trial_too_high_indicator",p,"0"));
        p["trial"]=9;
        Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g5.u04.l04.corrected_trial_quotient",p,"9"));
    }
}