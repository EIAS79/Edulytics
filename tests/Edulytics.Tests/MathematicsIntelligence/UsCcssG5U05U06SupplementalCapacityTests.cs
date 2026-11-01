using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class UsCcssG5U05U06SupplementalCapacityTests
{
    private static readonly (string Family,string Unit,string Lesson)[] Families =
    [
        ("usccss.g5.u05.l05.original_decimal_true_false","U05","L05"),
        ("usccss.g5.u05.l05.original_frisbee_swim_comparisons","U05","L05"),
        ("usccss.g5.u05.l09.source_decimal_inequality_truth","U05","L09"),
        ("usccss.g5.u05.l11.original_decimal_addition","U05","L11"),
        ("usccss.g5.u05.l11.original_shaded_hundredth_grid_sums","U05","L11"),
        ("usccss.g5.u05.l12.source_nearest_whole_estimates","U05","L12"),
        ("usccss.g5.u05.l13.original_elena_andre_error","U05","L13"),
        ("usccss.g5.u06.l08.original_fraction_sums_differences","U06","L08")
    ];

    [Fact]
    public void DecimalAndFractionFamiliesProduceTenUniqueVerifiableSelections()
    {
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var (family,unit,lesson) in Families)
        {
            var items=engine.Generate("g5-u05-supplemental-capacity",
                "PED:US-CCSS-MATH:G5:"+unit+":"+lesson,
                [family],ExactSkillQuestionDifficulty.Standard,10,20261101,[]);
            Assert.Equal(10,items.Count);
            Assert.Equal(10,items.Select(x=>x.ExposureFingerprint).Distinct().Count());
            Assert.All(items,item=>
            {
                Assert.True(ExactSkillContractQuestionEngine.Verify(
                    item.Family,item.Parameters,item.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(
                    item.Family,item.Parameters,"-1"));
                Assert.Contains("TEACHER_REVIEW_REQUIRED",item.Prompt);
            });
            Assert.False(ExactSkillContractQuestionEngine.Verify(family,
                new Dictionary<string,int>{{"case",999},{"slot",0}},"1"));
        }
    }
}
