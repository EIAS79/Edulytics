using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class UsCcssG1Unit2Part2QuestionsTests
{
    private const string Prefix="PED:US-CCSS-MATH:G1:U02:L";
    private static readonly string[] Families =
    [
        "usccss.g1.u02.story.mixed_result_or_change",
        "usccss.g1.u02.center.picture_story_equation",
        "usccss.g1.u02.pets.put_together_total",
        "usccss.g1.u02.shake_spill.two_unknown_parts"
    ];

    [Fact]
    public void ExactFourNewSourceGoalPracticeContractsAreGradeAppropriate()
    {
        for(var index=0;index<4;index++)
        {
            var code=Prefix+(index+4).ToString("D2");
            var family=Families[index];
            Assert.True(LessonPracticeCapabilityResolver.TryResolve(code,out var practice),code);
            Assert.NotNull(practice);
            Assert.Equal("READY_VERIFIED",practice!.Readiness);
            Assert.Equal(new[]{family},practice.AllowedQuestionFamilies);
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var interaction));
            Assert.NotNull(interaction);
            Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad,interaction!.InteractionKind);
            Assert.True(interaction.HasVisualRepresentation);
        }
    }

    [Fact]
    public void IndependentCorrectnessAndTamperRejectionForAllFourStoryFamilies()
    {
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var family in Families)
        foreach(var seed in new[]{20261016,20261017,20261018})
        {
            var questions=engine.Generate(
                "g1-u02-part2-original-story",family,[family],
                ExactSkillQuestionDifficulty.Standard,10,seed,[]);
            Assert.Equal(10,questions.Count);
            Assert.Equal(10,questions.Select(x=>x.ExposureFingerprint)
                .Distinct(StringComparer.Ordinal).Count());
            foreach(var q in questions)
            {
                Assert.Equal(family,q.Family);
                Assert.NotEmpty(q.Prompt);
                Assert.True(q.Solution.Length>30);
                Assert.True(ExactSkillContractQuestionEngine.Verify(
                    family,q.Parameters,q.CorrectAnswer));

                var expected=int.Parse(q.CorrectAnswer,
                    System.Globalization.CultureInfo.InvariantCulture);
                Assert.InRange(expected,0,10);
                Assert.False(ExactSkillContractQuestionEngine.Verify(
                    family,q.Parameters,(expected+1).ToString(
                        System.Globalization.CultureInfo.InvariantCulture)));
                Assert.False(ExactSkillContractQuestionEngine.Verify(
                    family,q.Parameters,"?"));
                var altered=new Dictionary<string,int>(q.Parameters,StringComparer.Ordinal);
                altered[family switch
                {
                    "usccss.g1.u02.story.mixed_result_or_change"=>"start",
                    "usccss.g1.u02.center.picture_story_equation"=>"start",
                    "usccss.g1.u02.pets.put_together_total"=>"first",
                    _=>"total"
                }]=-50;
                Assert.False(ExactSkillContractQuestionEngine.Verify(
                    family,altered,q.CorrectAnswer));

                if(family==Families[0])
                {
                    Assert.Contains("[ ? ]",q.Prompt);
                    Assert.Equal(q.Parameters["mode"] switch
                    {
                        0=>q.Parameters["start"]+q.Parameters["change"],
                        1=>q.Parameters["change"],
                        _=>q.Parameters["start"]-q.Parameters["change"]
                    },expected);
                }
                else if(family==Families[1])
                {
                    Assert.Contains("math story",q.Prompt);
                    Assert.InRange(expected,1,2);
                }
                else if(family==Families[2])
                {
                    Assert.Contains("Nobody arrives or leaves",q.Prompt);
                    Assert.Equal(q.Parameters["first"]+q.Parameters["second"],expected);
                }
                else
                {
                    Assert.Contains("Shake and Spill",q.Prompt);
                    Assert.Contains("[ ? ]",q.Prompt);
                    Assert.Equal(q.Parameters["total"]-q.Parameters["first"],expected);
                }
            }
        }
    }
}
