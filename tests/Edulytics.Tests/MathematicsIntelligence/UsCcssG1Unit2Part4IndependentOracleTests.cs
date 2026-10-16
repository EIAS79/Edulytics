using System.Globalization;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Independent answer oracle for the five original-source G1 U02 L11-L15
/// candidate families. Recomputes expected results without using generator Solve.
/// Does not certify visual drawings, classroom survey or interactive centers.
/// </summary>
public sealed class UsCcssG1Unit2Part4IndependentOracleTests
{
    private static readonly (string Code,string Family)[] Cases =
    [
        ("PED:US-CCSS-MATH:G1:U02:L11","usccss.g1.u02.towers.difference"),
        ("PED:US-CCSS-MATH:G1:U02:L12","usccss.g1.u02.supplies.difference"),
        ("PED:US-CCSS-MATH:G1:U02:L13","usccss.g1.u02.artdata.difference"),
        ("PED:US-CCSS-MATH:G1:U02:L14","usccss.g1.u02.equations.match_difference"),
        ("PED:US-CCSS-MATH:G1:U02:L15","usccss.g1.u02.story.mixed_unknowns")
    ];

    private static int IndependentAnswer(string family,IReadOnlyDictionary<string,int> p)
    {
        var larger=p["larger"];var smaller=p["smaller"];
        return family switch
        {
            "usccss.g1.u02.equations.match_difference" =>
                p["flip"]==0?1:2,
            "usccss.g1.u02.story.mixed_unknowns" when p["mode"]==0 =>
                larger,
            _=>checked(larger-smaller)
        };
    }
    [Fact]
    public void EverySourceFamilyRoutesAndAcceptsOnlyItsIndependentMathematicalAnswer()
    {
        var engine=new ExactSkillContractQuestionEngine();
        var branches=new Dictionary<string,HashSet<string>>(StringComparer.Ordinal);
        var total=0;
        foreach(var (code,family) in Cases)
        {
            Assert.True(LessonPracticeCapabilityResolver.TryResolve(code,out var contract),code);
            Assert.NotNull(contract);
            Assert.Equal(new[]{family},contract!.AllowedQuestionFamilies);
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var interaction));
            Assert.NotNull(interaction);
            Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad,interaction!.InteractionKind);
            var seen=new HashSet<string>(StringComparer.Ordinal);
            foreach(var difficulty in new[]{
                ExactSkillQuestionDifficulty.Standard,
                ExactSkillQuestionDifficulty.Stretch,
                ExactSkillQuestionDifficulty.Challenge
            })
            foreach(var seed in Enumerable.Range(1,60))
            {
                var question=Assert.Single(engine.Generate(
                    "b001-im2021-independent-g1-u02-part4",code,[family],
                    difficulty,1,1700000+seed+(int)difficulty*1000,[]));
                Assert.Equal(family,question.Family);
                Assert.False(string.IsNullOrWhiteSpace(question.ExposureFingerprint));
                Assert.InRange(question.Parameters["larger"],6,10);
                Assert.InRange(question.Parameters["smaller"],1,9);
                var expected=IndependentAnswer(family,question.Parameters);
                Assert.Equal(expected.ToString(CultureInfo.InvariantCulture),question.CorrectAnswer);
                Assert.True(ExactSkillContractQuestionEngine.Verify(
                    family,question.Parameters,question.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(
                    family,question.Parameters,(expected+1).ToString(CultureInfo.InvariantCulture)));
                Assert.False(ExactSkillContractQuestionEngine.Verify(
                    family,question.Parameters,(expected-1).ToString(CultureInfo.InvariantCulture)));
                Assert.False(ExactSkillContractQuestionEngine.Verify(
                    family,question.Parameters,"bad input"));
                var malformed=question.Parameters.ToDictionary(x=>x.Key,x=>x.Value,
                    StringComparer.Ordinal);
                malformed["larger"]=999;
                Assert.False(ExactSkillContractQuestionEngine.Verify(
                    family,malformed,question.CorrectAnswer));
                seen.Add(family switch
                {
                    "usccss.g1.u02.towers.difference" =>
                        question.Parameters["method"].ToString(CultureInfo.InvariantCulture),
                    "usccss.g1.u02.supplies.difference" or
                    "usccss.g1.u02.artdata.difference" =>
                        question.Parameters["kind"].ToString(CultureInfo.InvariantCulture),
                    "usccss.g1.u02.equations.match_difference" =>
                        question.Parameters["mode"]+","+question.Parameters["flip"],
                    _=>question.Parameters["mode"].ToString(CultureInfo.InvariantCulture)
                });
                total++;
            }
            branches[code]=seen;
        }
        Assert.Equal(5*3*60,total);
        Assert.Equal(2,branches[Cases[0].Code].Count);
        Assert.Equal(3,branches[Cases[1].Code].Count);
        Assert.Equal(3,branches[Cases[2].Code].Count);
        Assert.Equal(4,branches[Cases[3].Code].Count);
        Assert.Equal(4,branches[Cases[4].Code].Count);
    }
}
