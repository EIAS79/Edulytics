using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// UAE G5 local student printed 189–194. NOT_RUN in Stage A-B-C.
public sealed class UaeGrade5Unit3Lesson06LargeDivisionModelsTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L5:GENERAL:03:06";
    [Fact]
    public void SameG4ExactFamilyServesThreeDigitG5Models()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var contract));
        Assert.Equal("number.base_ten_division_with_remainders",contract!.SkillId);
        Assert.Equal(2,contract.AllowedQuestionFamilies.Count);
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var family in contract.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var ui));
            Assert.NotNull(ui);
            foreach(var q in engine.Generate(Lesson,family,[family],
                ExactSkillQuestionDifficulty.Standard,2,20261113,[]))
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,q.CorrectAnswer));
        }
    }
    [Fact]
    public void BaseTenHundredsQuotientAndRemainderExact()
    {
        var p=new Dictionary<string,int>{{"dividend",319},{"divisor",2}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u06.l03.base_ten_quotient",p,"١٥٩"));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u06.l03.base_ten_remainder",p,"١"));
        p["dividend"]=476;p["divisor"]=4;
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u06.l03.base_ten_quotient",p,"119"));
        Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g4.u06.l03.base_ten_remainder",p,"1"));
    }
}
