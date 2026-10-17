using System.Globalization;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Independent arithmetic/count oracle for the B001 Grade 1 source lessons
/// that were not fully independently computed by previous exact-code tests.
/// Test-only: does not rewrite historical lessons or confer P5 acceptance.
/// </summary>
public sealed class UsCcssB001TwentyRemainingIndependentMathOraclesTests
{
    [Fact]
    public void U01L01CollectionAndDotCubeAnswersMatchCountsFromSourceParameters()
    {
        const string lesson="PED:US-CCSS-MATH:G1:U01:L01";
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(lesson,out var contract));
        Assert.NotNull(contract);
        Assert.Equal(new[]
        {
            "usccss.g1.u01.l01.count_collection_to20",
            "usccss.g1.u01.l01.add_dot_cubes_to10"
        },contract!.AllowedQuestionFamilies);
        var engine=new ExactSkillContractQuestionEngine();
        var checkedCases=0;
        foreach(var family in contract.AllowedQuestionFamilies)
            foreach(var level in new[]
            {
                ExactSkillQuestionDifficulty.Standard,
                ExactSkillQuestionDifficulty.Stretch,
                ExactSkillQuestionDifficulty.Challenge
            })
                for(var seed=0;seed<18;seed++)
                {
                    var q=Assert.Single(engine.Generate("b001-20-first-source-oracle",
                        lesson,[family],level,1,20261017+37*seed,[]));
                    var p=q.Parameters;
                    var actual=family.EndsWith("count_collection_to20",
                        StringComparison.Ordinal)
                        ? p["count"]:p["left"]+p["right"];
                    Assert.InRange(actual,1,20);
                    Assert.Equal(actual, q.Prompt.Count(c=>c=='●'));
                    Assert.Equal(actual.ToString(CultureInfo.InvariantCulture),
                        q.CorrectAnswer);
                    Assert.True(ExactSkillContractQuestionEngine.Verify(
                        family,p,q.CorrectAnswer));
                    Assert.False(ExactSkillContractQuestionEngine.Verify(
                        family,p,(actual+1).ToString(CultureInfo.InvariantCulture)));
                    Assert.False(ExactSkillContractQuestionEngine.Verify(
                        family,p,"malformed"));
                    var corrupted=p.ToDictionary(x=>x.Key,x=>x.Value,
                        StringComparer.Ordinal);
                    corrupted[family.EndsWith("count_collection_to20",
                        StringComparison.Ordinal)?"count":"left"]=99;
                    Assert.False(ExactSkillContractQuestionEngine.Verify(
                        family,corrupted,q.CorrectAnswer));
                    checkedCases++;
                }
        Assert.Equal(108,checkedCases);
    }

    [Fact]
    public void U01L11ThroughL15AllDataFamiliesAgreeWithIndependentlyComputedCountsAndTruth()
    {
        const string prefix="PED:US-CCSS-MATH:G1:U01:L";
        var engine=new ExactSkillContractQuestionEngine();
        var reviewedFamilies=new HashSet<string>(StringComparer.Ordinal)
        {
            "usccss.g1.u01.b03.check_data_statement",
            "usccss.g1.u01.b03.count_category_data",
            "usccss.g1.u01.b03.answerable_data_question"
        };
        var checkedCases=0;
        foreach(var ordinal in new[]{11,12,13,15})
        {
            var lesson=prefix+ordinal.ToString("D2",CultureInfo.InvariantCulture);
            Assert.True(LessonPracticeCapabilityResolver.TryResolve(lesson,
                out var contract));
            Assert.NotNull(contract);
            var relevant=contract!.AllowedQuestionFamilies
                .Where(reviewedFamilies.Contains).ToArray();
            Assert.NotEmpty(relevant);
            foreach(var family in relevant)
                for(var seed=0;seed<24;seed++)
                {
                    var q=Assert.Single(engine.Generate(
                        "b001-20-independent-data-math",lesson,[family],
                        ExactSkillQuestionDifficulty.Standard,1,
                        20261017+ordinal*83+seed*37,[]));
                    Assert.Equal(family,q.Family);
                    var p=q.Parameters;
                    var groups=new[]{p["first"],p["second"],p["third"]};
                    Assert.All(groups,value=>Assert.InRange(value,1,5));
                    var index=p["index"];
                    var kind=p["kind"];
                    Assert.InRange(index,0,2);
                    var expected=family switch
                    {
                        "usccss.g1.u01.b03.check_data_statement"=>p["truth"],
                        "usccss.g1.u01.b03.count_category_data"=>
                            kind==3?groups.Sum():groups[index],
                        "usccss.g1.u01.b03.answerable_data_question"=>
                            kind==0?1:0,
                        _=>throw new InvalidOperationException(
                            "Unexpected family in Grade 1 data source oracle")
                    };
                    Assert.Equal(expected.ToString(CultureInfo.InvariantCulture),
                        q.CorrectAnswer);
                    Assert.Contains("class data chart",q.Prompt,
                        StringComparison.OrdinalIgnoreCase);
                    foreach(var group in groups)
                        Assert.Contains("("+group.ToString(CultureInfo.InvariantCulture)+")",
                            q.Prompt,StringComparison.Ordinal);
                    Assert.True(ExactSkillContractQuestionEngine.Verify(
                        family,p,q.CorrectAnswer));
                    Assert.False(ExactSkillContractQuestionEngine.Verify(
                        family,p,(expected+1).ToString(CultureInfo.InvariantCulture)));
                    Assert.False(ExactSkillContractQuestionEngine.Verify(
                        family,p,"malformed"));
                    var corrupted=p.ToDictionary(x=>x.Key,x=>x.Value,
                        StringComparer.Ordinal);
                    corrupted["first"]=0;
                    Assert.False(ExactSkillContractQuestionEngine.Verify(
                        family,corrupted,q.CorrectAnswer));
                    checkedCases++;
                }
        }
        Assert.True(checkedCases>=144,
            "All source-bound data families must have independent counter/truth oracles");
    }

    [Fact]
    public void U01L14ReusesOnlyPreviouslyIndependentlyVerifiedSortAndArithmeticFamilies()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(
            "PED:US-CCSS-MATH:G1:U01:L14",out var contract));
        Assert.NotNull(contract);
        Assert.Equal(new[]
        {
            "usccss.t001.g1.construct_tool_sort",
            "usccss.t001.g1.construct_shape_graph",
            "usccss.t001.g1.mixed_add_subtract"
        },contract!.AllowedQuestionFamilies);
        // The T001 independent source math tests use these same shared
        // generators; B001's exact-code test exercises each L14 routing.
    }
}
