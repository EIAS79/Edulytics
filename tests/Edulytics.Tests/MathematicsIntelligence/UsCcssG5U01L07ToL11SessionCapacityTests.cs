using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

// Exact source examples are retained; supplementary first-party numeric drills
// increase genuine variation without pretending to grade source constructions.
public sealed class UsCcssG5U01L07ToL11SessionCapacityTests
{
    private static readonly (string Family,string Lesson,int[] Original)[] Cases =
    [
        ("usccss.g5.u01.l07.cubic_unit_context","L07",[3,2,1]),
        ("usccss.g5.u01.l08.add_nonoverlap_prism_volumes","L08",[22,30,46]),
        ("usccss.g5.u01.l09.measured_composite_decompositions","L09",[78,259,420]),
        ("usccss.g5.u01.l10.equivalent_composite_volume_expressions","L10",[95,95,168]),
        ("usccss.g5.u01.l11.real_world_prism_volume","L11",[300,180,270,216])
    ];

    private static int IndependentExpected(string family,int v,int[] originals)
    {
        if(v<originals.Length)return originals[v];
        return family switch
        {
            "usccss.g5.u01.l07.cubic_unit_context" => v%3+1,
            "usccss.g5.u01.l08.add_nonoverlap_prism_volumes" => 12*v+18,
            "usccss.g5.u01.l09.measured_composite_decompositions" => 14*v+20,
            "usccss.g5.u01.l10.equivalent_composite_volume_expressions" => 15*v+78,
            "usccss.g5.u01.l11.real_world_prism_volume" => 3*(v+4)*(v+2),
            _ => throw new ArgumentOutOfRangeException(nameof(family))
        };
    }

    [Fact]
    public void EverySourceAlignedFamilyProducesTenDistinctCorrectlyGradedQuestions()
    {
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var (family,lesson,originals) in Cases)
        {
            var questions=engine.Generate("g5-u01-session-capacity",
                "PED:US-CCSS-MATH:G5:U01:"+lesson,
                [family],ExactSkillQuestionDifficulty.Standard,10,20261101,[]);
            Assert.Equal(10,questions.Count);
            Assert.Equal(10,questions.Select(x=>x.ExposureFingerprint).Distinct().Count());
            Assert.All(questions,question=>
            {
                Assert.Equal(family,question.Family);
                Assert.Single(question.Parameters);
                var variant=question.Parameters["variant"];
                Assert.InRange(variant,0,11);
                Assert.Equal(IndependentExpected(family,variant,originals).ToString(
                    System.Globalization.CultureInfo.InvariantCulture),question.CorrectAnswer);
                Assert.True(ExactSkillContractQuestionEngine.Verify(
                    question.Family,question.Parameters,question.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(
                    question.Family,question.Parameters,"-1"));
            });
            Assert.Contains(questions,x=>x.Parameters["variant"]>=originals.Length);
            Assert.False(ExactSkillContractQuestionEngine.Verify(family,
                new Dictionary<string,int>{{"variant",12}},"1"));
            Assert.False(ExactSkillContractQuestionEngine.Verify(family,
                new Dictionary<string,int>{{"variant",-1}},"1"));
            Assert.False(ExactSkillContractQuestionEngine.Verify(family,
                new Dictionary<string,int>{{"variant",0},{"foreign",1}},"1"));
        }
    }
}
