using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;

// Written at Stage C; deliberately NOT RUN until Stage D.
public sealed class UaeGrade3U01L01PlaceValueContractTests
{
    [Fact] public void ExactlyThreeGrade3FourDigitFamiliesAreRegistered()
    {
        var lesson="PED:UAE-MOE-MATH:L3:COMMON:01:01";
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(lesson,out var contract));
        Assert.NotNull(contract);
        Assert.Equal(3,contract!.AllowedQuestionFamilies.Count);
        foreach(var family in contract.AllowedQuestionFamilies)
        {
            Assert.StartsWith("uae.g3.u01.l01.",family);
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var interaction));
            Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad,interaction!.InteractionKind);
        }
    }
    [Fact] public void G3PlaceValueGeneratedQuestionsAcceptBothDigitScriptsAndRejectTampering()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve("PED:UAE-MOE-MATH:L3:COMMON:01:01",out var contract));
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var family in contract!.AllowedQuestionFamilies)
        foreach(int seed in new[]{20261109,20261110,20261111})
        {
            var items=engine.Generate("uae-g3-t1-u01-l01",family,[family],
                ExactSkillQuestionDifficulty.Standard,4,seed,[]);
            Assert.Equal(4,items.Count);
            foreach(var item in items)
            {
                Assert.InRange(item.Parameters["number"],1000,9999);
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,item.Parameters,item.CorrectAnswer));
                var arabicDigits=new string(item.CorrectAnswer.Select(c=>c is >= '0' and <= '9' ? (char)('٠'+c-'0') : c).ToArray());
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,item.Parameters,arabicDigits));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,item.Parameters,"INVALID"));
                var tampered=item.Parameters.ToDictionary(k=>k.Key,v=>v.Value);
                tampered["result"]=item.Parameters["result"]+1;
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,tampered,item.CorrectAnswer));
            }
        }
    }
}
