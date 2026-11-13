using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// Stage ABC source Grade5 Arabic 01.pdf printed pp119–124; tests WRITTEN_NOT_RUN.
public sealed class UaeGrade5Unit2Lesson07DistributiveReuseTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L5:GENERAL:02:07";
    [Fact] public void G5UsesExactG4DistributiveFamilyRouting()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var c));
        Assert.Equal("number.distributive_property",c!.SkillId);
        Assert.Equal(2,c.AllowedQuestionFamilies.Count);
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var family in c.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var ui));Assert.NotNull(ui);
            foreach(var q in engine.Generate(Lesson,family,[family],ExactSkillQuestionDifficulty.Standard,2,20261113,[]))
            {
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,q.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,"-3"));
            }
        }
    }
}
