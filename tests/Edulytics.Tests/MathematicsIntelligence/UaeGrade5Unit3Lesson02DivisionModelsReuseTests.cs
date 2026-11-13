using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// Grade5 student book printed 163–168; StageD tests written, NOT_RUN.
public sealed class UaeGrade5Unit3Lesson02DivisionModelsReuseTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L5:GENERAL:03:02";
    [Fact]
    public void BaseTenQuotientAndRemainderReuseTwoExistingG4Families()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var c));
        Assert.Equal("number.base_ten_division_with_remainders",c!.SkillId);
        Assert.Equal(2,c.AllowedQuestionFamilies.Count);
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var family in c.AllowedQuestionFamilies)
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
    public void DivisionModelsRetainRemainderLessThanDivisor()
    {
        var p=new Dictionary<string,int>{{"dividend",56},{"divisor",5}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u06.l03.base_ten_quotient",p,"١١"));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u06.l03.base_ten_remainder",p,"١"));
        p["divisor"]=0;
        Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g4.u06.l03.base_ten_quotient",p,"11"));
    }
}
