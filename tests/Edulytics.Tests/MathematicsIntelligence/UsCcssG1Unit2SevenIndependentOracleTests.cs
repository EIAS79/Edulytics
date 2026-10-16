using System.Globalization;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Independent numeric/choice reference oracle for the seven original-source
/// G1 U02 candidate repairs. Does not call the generator's Solve/Verify to
/// compute expected answers. NOT a teacher or student-PDF approval.
/// </summary>
public sealed class UsCcssG1Unit2SevenIndependentOracleTests
{
    private static readonly (string Code, string Family)[] Cases =
    [
        ("PED:US-CCSS-MATH:G1:U02:L04", "usccss.g1.u02.story.mixed_result_or_change"),
        ("PED:US-CCSS-MATH:G1:U02:L05", "usccss.g1.u02.center.picture_story_equation"),
        ("PED:US-CCSS-MATH:G1:U02:L06", "usccss.g1.u02.pets.put_together_total"),
        ("PED:US-CCSS-MATH:G1:U02:L07", "usccss.g1.u02.shake_spill.two_unknown_parts"),
        ("PED:US-CCSS-MATH:G1:U02:L08", "usccss.g1.u02.shake_cover.addend_unknown"),
        ("PED:US-CCSS-MATH:G1:U02:L09", "usccss.g1.u02.story.result_part_or_both_unknown"),
        ("PED:US-CCSS-MATH:G1:U02:L10", "usccss.g1.u02.center.capture_squares.sum_to10")
    ];

    private static int IndependentAnswer(string family, IReadOnlyDictionary<string,int> p) =>
        family switch
        {
            "usccss.g1.u02.story.mixed_result_or_change" => p["mode"] switch
            {
                0 => checked(p["start"]+p["change"]),
                1 => p["change"],
                2 => checked(p["start"]-p["change"]),
                _ => throw new ArgumentOutOfRangeException(nameof(p))
            },
            "usccss.g1.u02.center.picture_story_equation" =>
                p["action"] == p["flip"] ? 1 : 2,
            "usccss.g1.u02.pets.put_together_total" =>
                checked(p["first"]+p["second"]),
            "usccss.g1.u02.shake_spill.two_unknown_parts" =>
                p["order"] == 0 ? 1 : 2,
            "usccss.g1.u02.shake_cover.addend_unknown" =>
                checked(p["total"]-p["visible"]),
            "usccss.g1.u02.story.result_part_or_both_unknown" => p["mode"] switch
            {
                0 => p["total"],
                1 => checked(p["total"]-p["part"]),
                2 => p["flip"] == 0 ? 1 : 2,
                _ => throw new ArgumentOutOfRangeException(nameof(p))
            },
            "usccss.g1.u02.center.capture_squares.sum_to10" =>
                checked(p["a"]+p["b"]),
            _ => throw new NotSupportedException(family)
        };

    [Fact]
    public void SevenSourceFamiliesHaveIndependentAnswersAndAdversarialNearMissRejection()
    {
        var engine = new ExactSkillContractQuestionEngine();
        var seenBranches = new Dictionary<string,HashSet<string>>(StringComparer.Ordinal);
        var total = 0;
        foreach (var (code,family) in Cases)
        {
            Assert.True(LessonPracticeCapabilityResolver.TryResolve(code,out var contract), code);
            Assert.NotNull(contract);
            Assert.Equal("READY_VERIFIED", contract!.Readiness);
            Assert.Equal([family], contract.AllowedQuestionFamilies);
            var branches = new HashSet<string>(StringComparer.Ordinal);
            foreach(var difficulty in new[] {
                ExactSkillQuestionDifficulty.Standard,
                ExactSkillQuestionDifficulty.Stretch,
                ExactSkillQuestionDifficulty.Challenge
            })
            foreach(var seed in Enumerable.Range(1,60))
            {
                var question = Assert.Single(engine.Generate(
                    "b001-im2021-independent-g1-u02-seven",code,[family],difficulty,
                    1,1600000+seed+((int)difficulty*1000),[]));
                Assert.Equal(family,question.Family);
                var expected = IndependentAnswer(family,question.Parameters);
                Assert.Equal(expected.ToString(CultureInfo.InvariantCulture),question.CorrectAnswer);
                Assert.InRange(expected,0,10);
                Assert.False(ExactSkillContractQuestionEngine.Verify(
                    family,question.Parameters,(expected+1).ToString(CultureInfo.InvariantCulture)));
                Assert.False(ExactSkillContractQuestionEngine.Verify(
                    family,question.Parameters,(expected-1).ToString(CultureInfo.InvariantCulture)));
                Assert.True(ExactSkillContractQuestionEngine.Verify(
                    family,question.Parameters,expected.ToString(CultureInfo.InvariantCulture)));
                Assert.False(string.IsNullOrWhiteSpace(question.ExposureFingerprint));
                branches.Add(family switch {
                    "usccss.g1.u02.story.mixed_result_or_change" => question.Parameters["mode"].ToString(),
                    "usccss.g1.u02.center.picture_story_equation" =>
                        question.Parameters["action"]+","+question.Parameters["flip"],
                    "usccss.g1.u02.pets.put_together_total" => question.Parameters["flip"].ToString(),
                    "usccss.g1.u02.shake_spill.two_unknown_parts" => question.Parameters["order"].ToString(),
                    "usccss.g1.u02.shake_cover.addend_unknown" =>
                        question.Parameters["side"]+","+question.Parameters["color"],
                    "usccss.g1.u02.story.result_part_or_both_unknown" =>
                        question.Parameters["mode"].ToString(),
                    _ => question.Parameters["kind"].ToString()
                });
                total++;
            }
            seenBranches[code] = branches;
        }
        Assert.Equal(7*3*60,total);
        var branchMinimum = new[]{3,4,2,2,4,3,3};
        for(int i=0;i<Cases.Length;i++)
            Assert.True(seenBranches[Cases[i].Code].Count >= branchMinimum[i],
                "Missing source scenario coverage: "+Cases[i].Code+
                " observed "+string.Join(",",seenBranches[Cases[i].Code]));
    }
}
