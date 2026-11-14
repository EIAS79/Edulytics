using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// UAE local Arabic Grade5 print pp277–282; Stage D tests written NOT_RUN.
public sealed class UaeGrade5Unit4Lesson05LargeDivisionTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L5:GENERAL:04:05";
    [Fact]
    public void LargeDivisionUsesExistingExactEngineWithThreeFamilies()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var c));
        Assert.Equal("number.long_division_large_numbers",c!.SkillId);
        Assert.Equal(3,c.AllowedQuestionFamilies.Count);
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var f in c.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(f,out var ui));
            Assert.NotNull(ui);
            for(int seed=1;seed<=5;seed++)
            foreach(var q in engine.Generate(Lesson,f,[f],ExactSkillQuestionDifficulty.Standard,1,seed,[]))
                Assert.True(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,q.CorrectAnswer));
        }
    }
    [Fact]
    public void FiveDigitAndThreeDigitDivisorsRetainZerosAndRemainders()
    {
        var p=new Dictionary<string,int>{{"dividend",22500},{"divisor",75}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u04.l05.large_quotient",p,"٣٠٠"));
        p["dividend"]=64506;p["divisor"]=26;
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u04.l05.large_quotient",p,"2481"));
        p["dividend"]=46534;p["divisor"]=152;
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u04.l05.zero_place_quotient",p,"٣٠٦"));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u04.l05.large_remainder",p,"22"));
        p["divisor"]=0;
        Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g5.u04.l05.large_quotient",p,"0"));
    }
}