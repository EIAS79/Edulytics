using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

// Authored in Stage A-B-C, test execution DEFERRED to Stage D.
public sealed class UaeGrade2Term3WrittenNotRunTests
{
    [Theory]
    [InlineData("uae.g2.u09.l01.time_whole_hour")]
    [InlineData("uae.g2.u09.l02.time_half_hour")]
    [InlineData("uae.g2.u09.l03.time_pattern")]
    [InlineData("uae.g2.u09.l04.time_quarter_hour")]
    [InlineData("uae.g2.u09.l05.time_five_minutes")]
    [InlineData("uae.g2.u09.l06.am_pm")]
    [InlineData("uae.g2.u09.l07.time_relationships")]
    [InlineData("uae.g2.u09.l08.calendar_weekdays")]
    [InlineData("uae.g2.u09.l09.time_units")]
    [InlineData("uae.g2.u10.l01.two_d_shape")]
    [InlineData("uae.g2.u10.l02.sides_and_vertices")]
    [InlineData("uae.g2.u10.l03.sketch_shape_counts")]
    [InlineData("uae.g2.u10.l04.three_d_classify")]
    [InlineData("uae.g2.u10.l05.faces_edges_vertices")]
    [InlineData("uae.g2.u10.l06.connect_solids_to_faces")]
    [InlineData("uae.g2.u10.l07.equal_parts_fraction")]
    [InlineData("uae.g2.u10.l08.area_unit_squares")]
    [InlineData("uae.g2.u11.l01.ruler_cm_m")]
    [InlineData("uae.g2.u11.l02.select_metric_unit")]
    [InlineData("uae.g2.u11.l03.compare_lengths")]
    [InlineData("uae.g2.u11.l04.centimeters_meters")]
    [InlineData("uae.g2.u11.l05.number_line_measure")]
    [InlineData("uae.g2.u11.l06.measurement_data")]
    [InlineData("uae.g2.u11.l07.liquid_volume")]
    [InlineData("uae.g2.u11.l08.metric_liquid_volume")]
    [InlineData("uae.g2.u12.l01.coin_1_5_10_fils")]
    [InlineData("uae.g2.u12.l02.coin_25_fils")]
    [InlineData("uae.g2.u12.l03.count_coin_mix")]
    [InlineData("uae.g2.u12.l04.act_out_money")]
    [InlineData("uae.g2.u12.l05.dirham_fils")]
    public void ExistingExactEngineSupportsLessonSourceFamilies(string family)
    {
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
        var engine=new ExactSkillContractQuestionEngine();
        for(var seed=1;seed<=12;seed++)
        {
            var generated=engine.Generate("uae-grade2-term3",family,new[]{family},
                ExactSkillQuestionDifficulty.Standard,1,seed,Array.Empty<string>());
            Assert.NotEmpty(generated);
            foreach(var q in generated)
            {
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,q.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,"99999"));
                var arabic=new string(q.CorrectAnswer.Select(c=>
                    c is >= '0' and <= '9'?(char)('٠'+c-'0'):c).ToArray());
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,arabic));
            }
        }
    }
}
