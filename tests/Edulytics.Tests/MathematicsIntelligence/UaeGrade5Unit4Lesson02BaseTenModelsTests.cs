using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// Local Arabic Grade5 01.pdf pp257–262. Stage D tests authored but NOT_RUN.
public sealed class UaeGrade5Unit4Lesson02BaseTenModelsTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L5:GENERAL:04:02";
    [Fact]
    public void BaseTenGroupsRouteToReusedEngine()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var c));
        Assert.Equal("number.base_ten_division_with_remainders",c!.SkillId);
        Assert.Equal(2,c.AllowedQuestionFamilies.Count);
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var f in c.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(f,out var interaction));
            Assert.NotNull(interaction);
            foreach(var q in engine.Generate(Lesson,f,[f],ExactSkillQuestionDifficulty.Standard,2,345,[]))
                Assert.True(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,q.CorrectAnswer));
        }
    }
    [Fact]
    public void RegroupAndArabicAnswersVerifyWithoutTrustingStoredResult()
    {
        var p=new Dictionary<string,int>{{"dividend",156},{"divisor",12}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u04.l02.base_ten_group_quotient",p,"١٣"));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u04.l02.base_ten_group_remainder",p,"0"));
        p["dividend"]=251;
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u04.l02.base_ten_group_remainder",p,"١١"));
        Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g5.u04.l02.base_ten_group_quotient",p,"21"));
    }
}