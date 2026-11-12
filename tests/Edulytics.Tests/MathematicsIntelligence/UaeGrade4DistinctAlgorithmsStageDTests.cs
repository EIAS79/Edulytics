using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeGrade4DistinctAlgorithmsStageDTests
{
    [Theory]
    [InlineData("PED:UAE-MOE-MATH:L4:COMMON:04:04","uae.g4.u04.l04.array_product","uae.g4.u04.l04.array_partial_tens")]
    [InlineData("PED:UAE-MOE-MATH:L4:COMMON:04:05","uae.g4.u04.l05.vertical_product","uae.g4.u04.l05.units_carry")]
    [InlineData("PED:UAE-MOE-MATH:L4:COMMON:04:06","uae.g4.u04.l06.regroup_tens","uae.g4.u04.l06.regroup_product")]
    [InlineData("PED:UAE-MOE-MATH:L4:COMMON:04:07","uae.g4.u04.l07.distributive_tens","uae.g4.u04.l07.distributive_sum")]
    [InlineData("PED:UAE-MOE-MATH:L4:COMMON:04:08","uae.g4.u04.l08.vertical_three_digit_product","uae.g4.u04.l08.vertical_units_carry")]
    [InlineData("PED:UAE-MOE-MATH:L4:COMMON:04:11","uae.g4.u04.l11.zero_place_value_product","uae.g4.u04.l11.added_zero_places")]
    [InlineData("PED:UAE-MOE-MATH:L4:COMMON:05:04","uae.g4.u05.l04.two_digit_column_product","uae.g4.u05.l04.two_digit_tens_partial")]
    [InlineData("PED:UAE-MOE-MATH:L4:COMMON:06:07","uae.g4.u06.l07.first_quotient_digit","uae.g4.u06.l07.first_quotient_place_value")]
    [InlineData("PED:UAE-MOE-MATH:L4:COMMON:06:08","uae.g4.u06.l08.partial_quotient_sum","uae.g4.u06.l08.partial_quotient_remainder")]
    [InlineData("PED:UAE-MOE-MATH:L4:COMMON:06:09","uae.g4.u06.l09.large_quotient","uae.g4.u06.l09.large_remainder")]
    [InlineData("PED:UAE-MOE-MATH:L4:COMMON:06:10","uae.g4.u06.l10.zero_middle_quotient","uae.g4.u06.l10.zero_middle_remainder")]
    public void DistinctAlgorithmFamiliesGenerateVerifiedAnswers(
        string lessonCode,string one,string two)
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(lessonCode,out var contract));
        Assert.NotNull(contract);
        Assert.Contains(one,contract!.AllowedQuestionFamilies);
        Assert.Contains(two,contract.AllowedQuestionFamilies);
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var family in new[]{one,two})
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var ui));
            Assert.NotNull(ui);
            foreach(var problem in engine.Generate(lessonCode,family,[family],
              ExactSkillQuestionDifficulty.Standard,5,20261112,[]))
            {
                Assert.True(ExactSkillContractQuestionEngine.Verify(
                  family,problem.Parameters,problem.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(
                  family,problem.Parameters,"-1"));
            }
        }
    }
}
