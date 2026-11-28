using System.Globalization;
using Edulytics.Core.Enums;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// UAE Grade10 Applied algebra lab printed p214, canonical General U04L08.
/// Authored during A–B–C. Intentionally NOT RUN until Stage D.
/// </summary>
public sealed class UaeG10QuadraticRatesTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L10:GENERAL:UNIFIED:U04:L08:C041";
    private const string Rate="uae.g10.general.u04.l08.average_rate";
    private const string Positive="uae.g10.general.u04.l08.rate_is_positive";

    [Theory]
    [InlineData(Rate)]
    [InlineData(Positive)]
    public void BothRateFamiliesHaveExactNumericInputContract(string family)
    {
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
        Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var contract));
        Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad,contract!.InteractionKind);
    }

    [Theory]
    [InlineData(Rate)]
    [InlineData(Positive)]
    public void GeneratorVerifiesIndependentHalfStepRateAndRejectsTampering(string family)
    {
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var seed in new[]{20261125,20261126,20261127})
        foreach(var difficulty in new[]{ExactSkillQuestionDifficulty.Standard,
                                         ExactSkillQuestionDifficulty.Challenge})
        {
            var questions=engine.Generate(Lesson,Lesson,[family],difficulty,5,seed,[]);
            Assert.Equal(5,questions.Count);
            foreach(var q in questions)
            {
                var p=q.Parameters;
                Assert.Equal(5,p.Count);
                int a=p["a"],b=p["b"],start=p["start2"],step=p["step2"];
                Assert.NotEqual(0,a);Assert.Equal(0,a%2);
                Assert.InRange(step,1,4);
                int rate=(a/2)*(2*start+step)+b;
                int expected=family==Rate?rate:(rate>0?1:0);
                Assert.Equal(expected.ToString(CultureInfo.InvariantCulture),q.CorrectAnswer);
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,q.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,"خطأ"));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,
                    (expected+1).ToString(CultureInfo.InvariantCulture)));
                var arabic=string.Concat(q.CorrectAnswer.Select(ch=>ch is >= '0' and <= '9'
                    ?(char)('٠'+ch-'0'):ch));
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,arabic));
                var forged=p.ToDictionary(x=>x.Key,x=>x.Value,StringComparer.Ordinal);
                forged["a"]=3; // odd coefficient invalid in half-step integer family
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,forged,q.CorrectAnswer));
            }
        }
    }

    [Fact]
    public void OriginalSourceRocketAndDropNumbersAreIndependentlyCorrect()
    {
        static double Rocket(double t)=>-16*t*t+144*t;
        static double Fall(double t)=>-16*t*t+100;
        Assert.Equal(324,Rocket(4.5));Assert.Equal(0,Rocket(9));
        Assert.Equal(136,(Rocket(.5)-Rocket(0))/.5);
        Assert.Equal(120,(Rocket(1)-Rocket(.5))/.5);
        Assert.Equal(8,(Rocket(4.5)-Rocket(4))/.5);
        Assert.Equal(-8,(Rocket(5)-Rocket(4.5))/.5);
        Assert.Equal(0,Fall(2.5));
        Assert.Equal(-8,(Fall(.5)-Fall(0))/.5);
        Assert.Equal(-24,(Fall(1)-Fall(.5))/.5);
    }
}
