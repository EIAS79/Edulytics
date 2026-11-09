using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;
// Test source WRITTEN_NOT_RUN — execution belongs to Stage D.
public sealed class UaeGrade3Unit3Lesson07SubtractAcrossZerosTests
{
    [Fact] public void NewFamilyIsRegisteredAndReusesExistingExactPractice()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve("PED:UAE-MOE-MATH:L3:COMMON:03:07",out var contract));
        Assert.NotNull(contract);
        Assert.Equal(2,contract!.AllowedQuestionFamilies.Count);
        Assert.Contains("uae.g3.u03.l06.four_digit_subtract_exact",contract.AllowedQuestionFamilies);
        const string family="uae.g3.u03.l07.subtract_across_zeros_exact";
        Assert.Contains(family,contract.AllowedQuestionFamilies);
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
        Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var ui));
        Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad,ui!.InteractionKind);
    }
    [Theory]
    [InlineData(300,134,166)]
    [InlineData(5004,2815,2189)]
    [InlineData(3000,1872,1128)]
    [InlineData(8007,4836,3171)]
    [InlineData(7000,3462,3538)]
    public void KnownCrossZeroCasesAreMathematicallyExact(int a,int b,int difference)
    {
        Assert.Equal(difference,a-b);
        Assert.Equal(a,difference+b);
    }
    [Fact] public void GeneratedQuestionsHaveZeroCrossingAndRejectTampering()
    {
        var engine=new ExactSkillContractQuestionEngine();
        const string family="uae.g3.u03.l07.subtract_across_zeros_exact";
        foreach(var item in engine.Generate("uae-g3-u03-l07",family,[family],
                     ExactSkillQuestionDifficulty.Standard,12,20261109,[]))
        {
            int a=item.Parameters["a"],b=item.Parameters["b"];
            Assert.Equal(a-b,int.Parse(item.CorrectAnswer));
            Assert.True(item.Parameters["mode"] switch {
                0 => a%1000==0 && b%10!=0,
                1 => a%100==0 && b%10!=0,
                _ => a%1000 is >=1 and <=8 && b%10>a%10
            });
            Assert.True(ExactSkillContractQuestionEngine.Verify(family,item.Parameters,item.CorrectAnswer));
            var arabic=new string(item.CorrectAnswer.Select(c=>(char)('٠'+c-'0')).ToArray());
            Assert.True(ExactSkillContractQuestionEngine.Verify(family,item.Parameters,arabic));
            var corrupted=item.Parameters.ToDictionary(x=>x.Key,x=>x.Value);
            corrupted["result"]++;
            Assert.False(ExactSkillContractQuestionEngine.Verify(family,corrupted,item.CorrectAnswer));
        }
    }
}
