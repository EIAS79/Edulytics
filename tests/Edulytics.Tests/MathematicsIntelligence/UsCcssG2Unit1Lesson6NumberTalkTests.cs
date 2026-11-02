using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Authored Stage C tests, not run until Stage D.</summary>
public sealed class UsCcssG2Unit1Lesson6NumberTalkTests
{
    private const string Lesson="PED:US-CCSS-MATH:G2:U01:L06";
    private const string Family="usccss.g2.u01.l06.number_talk_two_two_digit_within50";
    [Fact]
    public void DigitalAssessmentIsRestrictedToOriginalNumberTalk()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var contract));
        Assert.NotNull(contract);
        Assert.Equal(new[]{Family},contract!.AllowedQuestionFamilies);
        Assert.True(LessonPracticeInteractionRegistry.TryResolve(Family,out var interaction));
        Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad,interaction!.InteractionKind);
    }
    [Fact]
    public void ExactSeededAdditionsRejectFalseAndTamperedAnswers()
    {
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var seed in new[]{20261021,20261022})
        {
            var q=engine.Generate("im2021-g2-u01-l06",Family,[Family],
                ExactSkillQuestionDifficulty.Standard,8,seed,[]);
            Assert.Equal(8,q.Count);
            Assert.Equal(8,q.Select(x=>x.ExposureFingerprint)
                .Distinct(StringComparer.Ordinal).Count());
            foreach(var task in q)
            {
                Assert.True(ExactSkillContractQuestionEngine.Verify(
                    Family,task.Parameters,task.CorrectAnswer));
                var value=int.Parse(task.CorrectAnswer,
                    System.Globalization.CultureInfo.InvariantCulture);
                Assert.InRange(value,20,50);
                Assert.False(ExactSkillContractQuestionEngine.Verify(
                    Family,task.Parameters,(value+1).ToString()));
                Assert.False(ExactSkillContractQuestionEngine.Verify(
                    Family,task.Parameters,"not-valid"));
                var bad=new Dictionary<string,int>(task.Parameters,StringComparer.Ordinal);
                bad["second"]=-1;
                Assert.False(ExactSkillContractQuestionEngine.Verify(
                    Family,bad,task.CorrectAnswer));
            }
        }
    }
    [Fact]
    public void OriginalSource14Plus22IsAValidLesson6NumberTalk()
    {
        var input=new Dictionary<string,int>(StringComparer.Ordinal)
            {["first"]=14,["second"]=22};
        Assert.True(ExactSkillContractQuestionEngine.Verify(Family,input,"36"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family,input,"35"));
    }
}
