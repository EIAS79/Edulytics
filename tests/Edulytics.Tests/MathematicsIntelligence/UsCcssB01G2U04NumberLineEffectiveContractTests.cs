using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Core.Lessons;
using Edulytics.Data.Seeding;
using Edulytics.Services.Mathematics;
using Edulytics.Services.LessonContent;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// V4.3 focused root-cause diagnostic: inspect only source-affected G2 U04
/// L07-L11 already covered by the earlier 500-lesson audit. No certification.
/// </summary>
public sealed class UsCcssB01G2U04NumberLineEffectiveContractTests
{
    [Fact]
    public void Grade2Lesson10NumberLineUsesTrueArithmeticAndEqualTickDistances()
    {
        const string code="PED:US-CCSS-MATH:G2:U04:L10";
        Assert.True(LessonPracticeContractRegistry.TryResolve(code,out var contract));
        Assert.NotNull(contract);
        var engine=new ExactSkillContractQuestionEngine();
        var checkedCases=0;
        foreach(var family in contract.AllowedQuestionFamilies)
        {
            for(var seed=421000;seed<421030;seed++)
            {
                var q=Assert.Single(engine.Generate(
                    "b01-g2-u04-l10-numberline-verified",code,[family],
                    ExactSkillQuestionDifficulty.Standard,1,seed,[]));
                Assert.True(ExactSkillContractQuestionEngine.Verify(
                    q.Family,q.Parameters,q.CorrectAnswer));
                var visual=Assert.Single(
                    UsCcssB01SourceBoundNumberLineVisuals.Build(code,[q]));
                Assert.Equal(RichLessonVisualKind.NumberLine,visual.Kind);
                var p=q.Parameters;
                Assert.Equal(p["expected"],int.Parse(q.CorrectAnswer));
                var ticks=visual.Items.Select(int.Parse).ToArray();
                Assert.InRange(ticks.Length,2,3);
                Assert.Contains(p["left"],ticks);
                Assert.Contains(p["expected"],ticks);
                for(var i=1;i<ticks.Length;i++)
                    Assert.Equal(p["right"],ticks[i]-ticks[i-1]);
                Assert.Empty(UsCcssB01SourceBoundNumberLineVisuals.Build(
                    code,[q with {CorrectAnswer="999"}]));
                checkedCases++;
            }
        }
        Assert.Equal(60,checkedCases);
    }

    [Fact]
    public void ReportExistingVerifiedContractsAndActualRichVisualKinds()
    {
        var codes=Enumerable.Range(7,5)
            .Select(i=>$"PED:US-CCSS-MATH:G2:U04:L{i:00}").ToArray();
        var lessons=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .Where(doc=>doc.PackCode=="US-CCSS-MATH" &&
                        doc.VersionCode=="CCSSM-2010")
            .SelectMany(doc=>doc.Lessons)
            .Where(x=>codes.Contains(x.LessonCode,StringComparer.Ordinal))
            .ToDictionary(x=>x.LessonCode,StringComparer.Ordinal);
        Assert.Equal(5,lessons.Count);
        foreach(var code in codes)
        {
            Assert.True(LessonPracticeContractRegistry.TryResolve(code,out var contract),
                "Missing exact Practice contract: "+code);
            Assert.NotNull(contract);
            if(code.EndsWith(":L10",StringComparison.Ordinal))
            {
                Assert.Equal("number.whole.add_subtract",contract.SkillId);
                Assert.Equal(
                    ["number.whole.add_subtract.within_100.apply",
                     "number.whole.add_subtract.within_100.build"],
                    contract.AllowedQuestionFamilies);
                Assert.StartsWith("IM2021_G2_U04_L10_",contract.SourceType);
            }
            var en=lessons[code].Translations.Single(x=>x.CultureCode=="en");
            var body=new CanonicalLessonTranslationRecord(
                en.CultureCode,en.Title,en.Explanation,en.KeyConceptsAndRules,
                en.WorkedExamples,en.StepByStepSolutions,en.CommonMistakes,
                en.QuickSummary);
            var rich=RichLessonContentV2Registry.Find(code,"en") ??
                     RichLessonContentV2RuntimeComposer.TryCompose(code,body);
            Assert.NotNull(rich);
            // Verify the real learner-side composer, not just that a builder
            // class exists or a question generator can produce arithmetic.
            Assert.Contains(rich.Visuals,v=>
                v.Kind==RichLessonVisualKind.NumberLine);
            var families=string.Join(",",contract.AllowedQuestionFamilies);
            var kinds=string.Join(",",rich.Visuals.Select(x=>x.Kind.ToString()));
            Console.WriteLine($"B01_G2_U04_EXACT_CONTRACT {code} "+
                $"sourceType={contract.SourceType} skill={contract.SkillId} "+
                $"families=[{families}] visuals=[{kinds}]");
        }
    }
}
