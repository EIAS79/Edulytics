using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class UsCcssB001SourceGoalPracticeQuestionsTests
{
    private const string G2Compare="PED:US-CCSS-MATH:G2:U02:L01";
    private const string G2Length="PED:US-CCSS-MATH:G2:U03:L12";
    private const string G3Product="PED:US-CCSS-MATH:G3:U01:L12";
    private const string G3Unknown="PED:US-CCSS-MATH:G3:U01:L15";
    private static readonly (string Code,string Family)[] Cases =
    [
        (G2Compare,"usccss.g2.u02.bar_total_or_difference.no_regroup"),
        (G2Length,"usccss.g2.u03.two_step_ribbon_length"),
        (G3Product,"usccss.g3.u01.equal_groups.model_product"),
        (G3Unknown,"usccss.g3.u01.equal_groups.unknown_factor")
    ];

    [Fact]
    public void FourCanonicalLessonsRespectLaterSourceSpecificPracticeFamilySupersessions()
    {
        foreach(var (code,family) in Cases)
        {
            Assert.True(LessonPracticeCapabilityResolver.TryResolve(code,out var contract),code);
            Assert.NotNull(contract);
            Assert.Equal("READY_VERIFIED",contract!.Readiness);
            // Original B001 mathematical generators remain available for their
            // source-goal proof, but the later source-specific G2 lessons route
            // through four independently reviewed families, not an obsolete single.
            var expected=code switch
            {
                G2Compare=>new[]{
                    "usccss.g2.u02.l01.movie_snacks_combine",
                    "usccss.g2.u02.l01.movie_snacks_difference",
                    "usccss.g2.u02.l01.cube_trains_total",
                    "usccss.g2.u02.l01.cube_trains_difference"
                },
                G2Length=>new[]{
                    "usccss.g2.u03.l12.two_step_take_from",
                    "usccss.g2.u03.l12.extra_piece_given_to_friend",
                    "usccss.g2.u03.l12.missing_length_then_start_unknown",
                    "usccss.g2.u03.l12.two_step_change_then_add"
                },
                _=>new[]{family}
            };
            Assert.Equal(expected,contract.AllowedQuestionFamilies);
            Assert.All(contract.AllowedQuestionFamilies, f =>
                Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f)));
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var interaction));
            Assert.NotNull(interaction);
            Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad,interaction!.InteractionKind);
            Assert.True(interaction.HasVisualRepresentation);
        }
    }

    [Fact]
    public void EveryFamilyGeneratesAndGradesUniqueProblemsWithWrongAnswerAndTamperRejection()
    {
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var (_,family) in Cases)
            foreach(var seed in new[]{20261016,20261017,20261018})
            {
                var items=engine.Generate(
                    "b001-im2021-source-goal",family,[family],
                    ExactSkillQuestionDifficulty.Standard,10,seed,[]);
                Assert.Equal(10,items.Count);
                Assert.Equal(10,items.Select(x=>x.ExposureFingerprint)
                    .Distinct(StringComparer.Ordinal).Count());

                foreach(var item in items)
                {
                    Assert.Equal(family,item.Family);
                    Assert.NotEmpty(item.Prompt);
                    Assert.True(item.Solution.Length>30);
                    Assert.True(ExactSkillContractQuestionEngine.Verify(
                        family,item.Parameters,item.CorrectAnswer));

                    var expected=int.Parse(item.CorrectAnswer,
                        System.Globalization.CultureInfo.InvariantCulture);
                    Assert.False(ExactSkillContractQuestionEngine.Verify(
                        family,item.Parameters,(expected+1).ToString(
                            System.Globalization.CultureInfo.InvariantCulture)));
                    Assert.False(ExactSkillContractQuestionEngine.Verify(
                        family,item.Parameters,"not a valid number"));

                    var altered=new Dictionary<string,int>(item.Parameters,StringComparer.Ordinal);
                    altered[family switch
                    {
                        "usccss.g2.u02.bar_total_or_difference.no_regroup"=>"operation",
                        "usccss.g2.u03.two_step_ribbon_length"=>"initial",
                        _=>"groups"
                    }]=-10;
                    Assert.False(ExactSkillContractQuestionEngine.Verify(
                        family,altered,item.CorrectAnswer));

                    if(family==Cases[0].Family)
                    {
                        var a=item.Parameters["first"];
                        var b=item.Parameters["second"];
                        Assert.True(a%10>=b%10);
                        Assert.True(a%10+b%10<=9);
                        Assert.Contains("no exchanging tens",item.Prompt);
                    }
                    else if(family==Cases[1].Family)
                    {
                        Assert.Contains("another",item.Prompt);
                        Assert.Contains("tape diagram",item.Prompt);
                        Assert.Equal(item.Parameters["initial"]-
                            item.Parameters["cut1"]-item.Parameters["cut2"],expected);
                    }
                    else if(family==Cases[2].Family)
                    {
                        Assert.Contains("equal boxes",item.Prompt);
                        Assert.Equal(item.Parameters["groups"]*
                            item.Parameters["size"],expected);
                    }
                    else
                    {
                        Assert.Contains("[ ? ]",item.Prompt);
                        Assert.Contains("missing",item.Prompt);
                        Assert.True(expected==item.Parameters["groups"]||
                            expected==item.Parameters["size"]);
                    }
                }
            }
    }
}
