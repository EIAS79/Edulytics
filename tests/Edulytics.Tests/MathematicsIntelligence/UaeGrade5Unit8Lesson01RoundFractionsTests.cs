using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// Stage A-B-C authored from 02.pdf printed pp549–554; NOT_RUN until Stage D.
public sealed class UaeGrade5Unit8Lesson01RoundFractionsTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L5:GENERAL:08:01";
    [Fact] public void FractionBenchmarksAndMixedNumberRoundCorrectly()
    {
        var p=new Dictionary<string,int>{{"numerator",1},{"denominator",8}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u10.l06.nearest_benchmark",p,"0"));
        p["numerator"]=7;
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u10.l06.nearest_benchmark",p,"2"));
        var mixed=new Dictionary<string,int>{{"whole",6},{"numerator",3},{"denominator",4}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u08.l01.round_mixed_number_to_whole",mixed,"٧"));
        mixed["numerator"]=1;
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u08.l01.round_mixed_number_to_whole",mixed,"6"));
    }
    [Fact] public void ReusesExactBenchmarksAndCorrectInteraction()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var contract));
        Assert.Equal("fractions.compare",contract!.SkillId);
        Assert.Equal(3,contract.AllowedQuestionFamilies.Count);
        foreach(var family in contract.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var ui));
            Assert.NotNull(ui);
        }
    }
}