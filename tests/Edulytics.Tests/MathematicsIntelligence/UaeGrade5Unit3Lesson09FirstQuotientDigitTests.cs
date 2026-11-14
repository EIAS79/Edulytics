using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// Stage A-B-C test authored, NOT RUN.
public sealed class UaeGrade5Unit3Lesson09FirstQuotientDigitTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L5:GENERAL:03:09";
    [Fact]
    public void FirstDigitSkillHasTwoExactFamilies()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var contract));
        Assert.Equal("number.long_division.first_quotient_digit_place",contract!.SkillId);
        Assert.Equal(2,contract.AllowedQuestionFamilies.Count);
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var family in contract.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var ui));
            Assert.NotNull(ui);
            foreach(var item in engine.Generate(Lesson,family,[family],ExactSkillQuestionDifficulty.Standard,2,20261113,[]))
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,item.Parameters,item.CorrectAnswer));
        }
    }
    [Fact]
    public void LeadingDigitAndPlaceMatchLocalStudentSource()
    {
        var p=new Dictionary<string,int>{{"dividend",135},{"divisor",3}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u03.l09.first_digit_place",p,"١٠"));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u03.l09.first_quotient_digit",p,"٤"));
        Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g5.u03.l09.first_digit_place",p,"100"));
        p["dividend"]=3085;p["divisor"]=5;
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u03.l09.first_digit_place",p,"100"));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u03.l09.first_quotient_digit",p,"6"));
    }
}
