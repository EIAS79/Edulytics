using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// Stage A-B-C source authored, tests intentionally NOT_RUN.
public sealed class UaeGrade5Unit3Lesson12InterpretRemainderTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L5:GENERAL:03:12";
    [Fact]
    public void ContextualInterpretationRoutesToThreeExactQuestionFamilies()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var contract));
        Assert.Equal("number.division.contextual_remainder_interpretation",contract!.SkillId);
        Assert.Equal(3,contract.AllowedQuestionFamilies.Count);
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var family in contract.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var ui));
            Assert.NotNull(ui);
            foreach(var item in engine.Generate(Lesson,family,[family],
              ExactSkillQuestionDifficulty.Standard,2,20261113,[]))
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,item.Parameters,item.CorrectAnswer));
        }
    }
    [Fact]
    public void BusNeedsCeilingAndCompletePacksNeedFloorAndLeftoversStayRemainder()
    {
        var p=new Dictionary<string,int>{{"dividend",50},{"divisor",8}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u03.l12.minimum_groups_ceil",p,"7"));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u03.l12.full_groups_floor",p,"6"));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u03.l12.leftover_amount",p,"2"));
        Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g5.u03.l12.minimum_groups_ceil",p,"6"));
        p["dividend"]=144;p["divisor"]=5;
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u03.l12.full_groups_floor",p,"28"));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u03.l12.minimum_groups_ceil",p,"29"));
        p["divisor"]=0;
        Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g5.u03.l12.minimum_groups_ceil",p,"29"));
    }
}
