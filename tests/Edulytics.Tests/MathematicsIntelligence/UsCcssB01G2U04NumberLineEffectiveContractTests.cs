using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Core.Lessons;
using Edulytics.Data.Seeding;
using Edulytics.Services.LessonContent;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// V4.3 focused root-cause diagnostic: inspect only source-affected G2 U04
/// L07-L11 already covered by the earlier 500-lesson audit. No certification.
/// </summary>
public sealed class UsCcssB01G2U04NumberLineEffectiveContractTests
{
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
            var en=lessons[code].Translations.Single(x=>x.CultureCode=="en");
            var body=new CanonicalLessonTranslationRecord(
                en.CultureCode,en.Title,en.Explanation,en.KeyConceptsAndRules,
                en.WorkedExamples,en.StepByStepSolutions,en.CommonMistakes,
                en.QuickSummary);
            var rich=RichLessonContentV2Registry.Find(code,"en") ??
                     RichLessonContentV2RuntimeComposer.TryCompose(code,body);
            Assert.NotNull(rich);
            var families=string.Join(",",contract.AllowedQuestionFamilies);
            var kinds=string.Join(",",rich.Visuals.Select(x=>x.Kind.ToString()));
            Console.WriteLine($"B01_G2_U04_EXACT_CONTRACT {code} "+
                $"sourceType={contract.SourceType} skill={contract.SkillId} "+
                $"families=[{families}] visuals=[{kinds}]");
        }
    }
}
