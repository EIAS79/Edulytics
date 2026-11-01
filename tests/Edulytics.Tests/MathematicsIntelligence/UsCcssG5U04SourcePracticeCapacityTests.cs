using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UsCcssG5U04SourcePracticeCapacityTests
{
    private static readonly (string Family,string Lesson)[] Products =
    [
        ("usccss.g5.u04.l01.source_reasonable_product_estimate","L01"),
        ("usccss.g5.u04.l02.original_partial_products_totals","L02"),
        ("usccss.g5.u04.l19.source_state_rectangular_area_products","L19"),
        ("usccss.g5.u04.l21.source_food_waste_annual_ounces","L21"),
        ("usccss.g5.u04.l21.independent_group_annual_estimates","L21")
    ];
    private static readonly (string Family,string Lesson)[] Quotients =
    [
        ("usccss.g5.u04.l10.original_folk_dance_equal_groups","L10"),
        ("usccss.g5.u04.l12.source_elena_partial_quotient_totals","L12"),
        ("usccss.g5.u04.l13.original_partial_quotient_estimate_and_solve","L13"),
        ("usccss.g5.u04.l14.original_corrected_error_quotients","L14"),
        ("usccss.g5.u04.l16.source_long_noodle_whole_quotient_parts","L16"),
        ("usccss.g5.u04.l16.source_han_estimate_integer_checks","L16"),
        ("usccss.g5.u04.l17.source_fraction_partial_quotient_totals","L17"),
        ("usccss.g5.u04.l20.original_cargo_container_division","L20")
    ];
    [Fact]
    public void SourceAlignedFamiliesSustainTenUniqueCorrectlyVerifiedDigitalQuestions()
    {
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var (family,lesson) in Products.Concat(Quotients))
        {
            var questions=engine.Generate("g5-u04-capacity",
                "PED:US-CCSS-MATH:G5:U04:"+lesson,[family],
                ExactSkillQuestionDifficulty.Standard,10,20261101,[]);
            Assert.Equal(10,questions.Count);
            Assert.Equal(10,questions.Select(x=>x.ExposureFingerprint).Distinct().Count());
            Assert.All(questions,q=>
            {
                Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,"-999"));
                Assert.Contains("TEACHER_REVIEW_REQUIRED",q.Prompt);
            });
        }
    }
    [Fact]
    public void EstimateFirstPartyVariantsPreserveLockedCorrectSlotAndSourceEstimate()
    {
        const string family="usccss.g5.u04.l01.source_reasonable_product_estimate";
        var source=UsCcssG5U04WholeNumberSourceProducts.Build(family,new Random(42));
        Assert.Equal("3",UsCcssG5U04WholeNumberSourceProducts.Solve(family,source.Parameters));
        Assert.False(UsCcssG5U04WholeNumberSourceProducts.Verify(family,
            new Dictionary<string,int>{{"case",0},{"slot",1}},"3"));
    }
}
