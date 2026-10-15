using Edulytics.Core.Mathematics.Practice;
using Edulytics.Data.Seeding;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Independent source-purpose regression guards for the first ten IM 2021
/// Grade 1 Unit 1 lessons. Not an L1-L6 academic approval or browser L6 test.
/// </summary>
public sealed class UsCcssT001FirstTenSourceAndPracticeTests
{
    private const string Prefix = "PED:US-CCSS-MATH:G1:U01:L";

    private static readonly string[][] ExpectedFamilies =
    [
        ["usccss.g1.u01.l01.count_collection_to20",
         "usccss.g1.u01.l01.add_dot_cubes_to10"],
        ["usccss.b01.g1.match_dot_cubes_expression",
         "usccss.b01.g1.complete_missing_dots",
         "usccss.b01.g1.check_off_add_within10"],
        ["usccss.b01.g1.add_one_or_two"],
        ["usccss.b01.g1.subtract_one_or_two"],
        ["number.whole.add_subtract.within_10.apply",
         "number.whole.add_subtract.within_10.build"],
        ["usccss.b01.g1.make_ten_missing_part",
         "number.whole.add_subtract.within_10.apply"],
        ["supporting.statistics.charts.mixed",
         "supporting.statistics.table_total"],
        ["supporting.statistics.charts.mixed",
         "supporting.statistics.table_total"],
        ["supporting.statistics.charts.mixed",
         "supporting.statistics.table_total"],
        ["usccss.b01.g1.make_ten_missing_part",
         "number.whole.add_subtract.within_10.apply",
         "number.whole.add_subtract.within_10.build"]
    ];

    [Fact]
    public void FirstTenContractsTeachExactOriginalSourceGoalsAndExcludeUnrelatedFamilies()
    {
        Assert.Equal(10,ExpectedFamilies.Length);
        for(var i=0;i<10;i++)
        {
            var code=Prefix+(i+1).ToString("D2");
            Assert.True(LessonPracticeCapabilityResolver.TryResolve(code,out var contract));
            Assert.NotNull(contract);
            Assert.Equal(ExpectedFamilies[i],contract!.AllowedQuestionFamilies);
            Assert.Equal("READY_VERIFIED",contract.Readiness);
            if(i==3)
            {
                Assert.Equal("number.whole.subtract",contract.SkillId);
                Assert.DoesNotContain(contract.AllowedQuestionFamilies,
                    x => x.Contains("add.direct",StringComparison.Ordinal));
            }
            if(i==6 || i==7 || i==8)
                Assert.All(contract.AllowedQuestionFamilies,
                    f => Assert.StartsWith("supporting.statistics.",f));
            Assert.DoesNotContain(contract.AllowedQuestionFamilies,
                f => f.Contains("across_ten",StringComparison.Ordinal));
        }
    }

    [Fact]
    public void SourceExactNewQuestionFamiliesGenerateAndGradeEveryDifficultyCorrectly()
    {
        var families=new Dictionary<string,string>(StringComparer.Ordinal)
        {
            [Prefix+"03"]="usccss.b01.g1.add_one_or_two",
            [Prefix+"04"]="usccss.b01.g1.subtract_one_or_two",
            [Prefix+"06"]="usccss.b01.g1.make_ten_missing_part",
            [Prefix+"10"]="usccss.b01.g1.make_ten_missing_part"
        };
        var engine=new ExactSkillContractQuestionEngine();
        var count=0;
        foreach(var (code,family) in families)
        foreach(var difficulty in new[]{
            ExactSkillQuestionDifficulty.Standard,
            ExactSkillQuestionDifficulty.Stretch,
            ExactSkillQuestionDifficulty.Challenge})
        for(var seed=0;seed<10;seed++)
        {
            var q=Assert.Single(engine.Generate(
                "T001-strict-source-and-answer-oracle",
                code,[family],difficulty,1,990000+count,[]));
            Assert.Equal(family,q.Family);
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                family,q.Parameters,q.CorrectAnswer));
            Assert.False(ExactSkillContractQuestionEngine.Verify(
                family,q.Parameters,
                (int.Parse(q.CorrectAnswer,System.Globalization.CultureInfo.InvariantCulture)+11)
                .ToString(System.Globalization.CultureInfo.InvariantCulture)));
            var p=q.Parameters;
            if(family.EndsWith(".add_one_or_two",StringComparison.Ordinal))
            {
                Assert.InRange(p["step"],1,2);
                Assert.InRange(p["start"]+p["step"],1,10);
                Assert.Equal((p["start"]+p["step"]).ToString(),q.CorrectAnswer);
            }
            else if(family.EndsWith(".subtract_one_or_two",StringComparison.Ordinal))
            {
                Assert.InRange(p["step"],1,2);
                Assert.InRange(p["start"]-p["step"],0,9);
                Assert.Equal((p["start"]-p["step"]).ToString(),q.CorrectAnswer);
            }
            else
            {
                Assert.Equal(10,p["known"]+p["missing"]);
                Assert.Equal(p["missing"].ToString(),q.CorrectAnswer);
            }
            count++;
        }
        Assert.Equal(120,count);
    }

    [Fact]
    public void CanonicalMaterializationUsesExistingCorrectionsAndNewFindPairTeaching()
    {
        var docs=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments();
        var selected=docs.SelectMany(d=>d.Lessons.Select(l=>(Doc:d,Lesson:l)))
            .Where(x=>x.Lesson.LessonCode.StartsWith(Prefix,StringComparison.Ordinal))
            .Where(x=>int.TryParse(x.Lesson.LessonCode[Prefix.Length..],out var n) && n>=1 && n<=10)
            .ToArray();
        Assert.Equal(10,selected.Length);
        var l04=selected.Single(x=>x.Lesson.LessonCode==Prefix+"04").Lesson;
        Assert.Contains("6 - 2 = 4",l04.Translations.Single().WorkedExamples);
        Assert.DoesNotContain("6 + 1 = 7",l04.Translations.Single().WorkedExamples);
        var l06=selected.Single(x=>x.Lesson.LessonCode==Prefix+"06");
        Assert.Contains("6 + 4 = 10",l06.Lesson.Translations.Single().WorkedExamples);
        Assert.DoesNotContain("flash cards",l06.Lesson.Translations.Single().WorkedExamples,
            StringComparison.OrdinalIgnoreCase);
        Assert.Equal(CommonCoreT001FindThePairContentCorrection.CorrectedVersion,
            CanonicalLessonContentMaterializer.GetEffectiveContentVersion(l06.Doc,l06.Lesson));
        var l10=selected.Single(x=>x.Lesson.LessonCode==Prefix+"10").Lesson;
        Assert.Contains("10 - 4 = 6",l10.Translations.Single().WorkedExamples);
        Assert.DoesNotContain("<path",l10.Translations.Single().WorkedExamples,
            StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void NewFindPairCorrectionIsSourceLockedAndIdempotent()
    {
        var docs=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments();
        var pair=docs.SelectMany(d=>d.Lessons.Select(l=>(Doc:d,Lesson:l)))
            .Single(x=>x.Lesson.LessonCode==Prefix+"06");
        var before=CanonicalLessonContentMaterializer.ComputeLessonFingerprint(pair.Doc,pair.Lesson);
        CommonCoreT001FindThePairContentCorrection.Apply(pair.Doc);
        Assert.Equal(before,CanonicalLessonContentMaterializer
            .ComputeLessonFingerprint(pair.Doc,pair.Lesson));
        var sha=pair.Lesson.CanonicalBodySha256;
        try
        {
            pair.Lesson.CanonicalBodySha256=new string('0',64);
            Assert.Throws<InvalidOperationException>(()=>
                CommonCoreT001FindThePairContentCorrection.Apply(pair.Doc));
        }
        finally {pair.Lesson.CanonicalBodySha256=sha;}
    }
}
