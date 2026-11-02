using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class UsCcssG5U03SourceDivisionCapacityTests
{
    private static readonly (string Family,string Lesson,int OriginalCount)[] Cases =
    [
        ("usccss.g5.u03.l11.mac_cheese_half_pan_equal_shares","L11",3),
        ("usccss.g5.u03.l12.third_division_teacher_diagrams","L12",3),
        ("usccss.g5.u03.l12.quarter_division_pattern","L12",3),
        ("usccss.g5.u03.l14.snack_servings_story_selection","L14",3),
        ("usccss.g5.u03.l16.source_ninetyeight_fifth_estimate","L16",1),
        ("usccss.g5.u03.l17.source_real_world_fraction_contexts","L17",3),
        ("usccss.g5.u03.l19.source_estimation_exact","L19",1),
        ("usccss.g5.u03.l20.source_calcium_rocket_total","L20",2)
    ];

    [Fact]
    public void EightFamiliesProvideTenDistinctCorrectlyVerifiedItems()
    {
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var (family,lesson,originalCount) in Cases)
        {
            var questions=engine.Generate("g5-u03-session-capacity",
                "PED:US-CCSS-MATH:G5:U03:"+lesson,
                [family],ExactSkillQuestionDifficulty.Standard,10,20261101,[]);
            Assert.Equal(10,questions.Count);
            Assert.Equal(10,questions.Select(x=>x.ExposureFingerprint).Distinct().Count());
            Assert.Contains(questions,x=>x.Parameters["case"]>=originalCount);
            Assert.All(questions,question=>
            {
                Assert.True(ExactSkillContractQuestionEngine.Verify(
                    question.Family,question.Parameters,question.CorrectAnswer));
                Assert.Contains("TEACHER_REVIEW_REQUIRED",question.Prompt);
                Assert.Contains("original whole",question.Prompt);
                Assert.False(ExactSkillContractQuestionEngine.Verify(
                    question.Family,question.Parameters,"0"));
            });
            Assert.False(ExactSkillContractQuestionEngine.Verify(family,
                new Dictionary<string,int>{{"case",99},{"slot",0}},"1"));
        }
    }
}
