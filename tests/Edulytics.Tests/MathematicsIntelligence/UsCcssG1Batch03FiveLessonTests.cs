using System.Reflection;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Core.Lessons;
using Edulytics.Services.LessonContent;
using Edulytics.Services.Mathematics;
using Edulytics.Data.Seeding;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class UsCcssG1Batch03FiveLessonTests
{
    private const string Prefix="PED:US-CCSS-MATH:G1:U01:L";
    private static readonly string[] DataFamilies=
    [
        "usccss.g1.u01.b03.check_data_statement",
        "usccss.g1.u01.b03.count_category_data",
        "usccss.g1.u01.b03.answerable_data_question"
    ];

    [Fact]
    public void ExactlyFiveLessonContractsAreReadyAndUseActualSupportedFamilies()
    {
        for(var number=11;number<=15;number++)
        {
            var code=Prefix+number.ToString("D2");
            Assert.True(LessonPracticeCapabilityResolver.TryResolve(code,out var contract),code);
            Assert.NotNull(contract);
            Assert.Equal("READY_VERIFIED",contract!.Readiness);
            Assert.NotEmpty(contract.AllowedQuestionFamilies);
            foreach(var family in contract.AllowedQuestionFamilies)
                Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family),
                    $"Unimplemented family {family} on {code}");
        }
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Prefix+"10",out var old));
        Assert.NotNull(old);
        Assert.Equal("usccss-t001-ten-practice-v1",old!.ContractVersion);
    }

    [Fact]
    public void EveryNewDataFamilyGeneratesAndGradesTenDifferentAnswersOrPrompts()
    {
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var family in DataFamilies)
            foreach(var seed in new[]{11015,22025})
            {
                var generated=engine.Generate(
                    "g1-b03-source-aligned-targeted-tests",family,[family],
                    ExactSkillQuestionDifficulty.Standard,10,seed,[]);
                Assert.Equal(10,generated.Count);
                Assert.Equal(10,generated.Select(q=>q.ExposureFingerprint)
                    .Distinct(StringComparer.Ordinal).Count());
                foreach(var q in generated)
                {
                    Assert.Equal(family,q.Family);
                    Assert.Contains("class data chart",q.Prompt,StringComparison.OrdinalIgnoreCase);
                    Assert.True(ExactSkillContractQuestionEngine.Verify(
                        family,q.Parameters,q.CorrectAnswer));
                    Assert.False(ExactSkillContractQuestionEngine.Verify(
                        family,q.Parameters,q.CorrectAnswer=="0"?"1":"0"));
                    Assert.False(ExactSkillContractQuestionEngine.Verify(
                        family,q.Parameters,"-1"));
                    Assert.False(ExactSkillContractQuestionEngine.Verify(
                        family,q.Parameters,"10 apples"));
                    var corrupted=q.Parameters.ToDictionary(x=>x.Key,x=>x.Value);
                    corrupted["first"]=0;
                    Assert.False(ExactSkillContractQuestionEngine.Verify(
                        family,corrupted,q.CorrectAnswer));
                    Assert.InRange(q.Parameters["first"],1,5);
                    Assert.InRange(q.Parameters["second"],1,5);
                    Assert.InRange(q.Parameters["third"],1,5);
                }
            }
    }

    [Fact]
    public void AllFiveRichV2LessonsTeachTheirDistinctSourceGoals()
    {
        var append=typeof(RichLessonContentV2RuntimeComposer).GetMethod(
            "AddT001LearnerTeaching",BindingFlags.NonPublic|BindingFlags.Static);
        Assert.NotNull(append);
        for(var number=11;number<=15;number++)
        {
            var code=Prefix+number.ToString("D2");
            var lesson=new RichLessonContentV2Lesson
            {
                LessonCode=code,CultureCode="en",Title="G1 source lesson",
                ExplanationParagraphs=[],KeyConcepts=[],WorkedExamples=[],
                CommonMistakes=[],SummaryPoints=[],Visuals=[],Videos=[]
            };
            append!.Invoke(null,[code,lesson]);
            Assert.Equal(2,lesson.WorkedExamples.Count);
            Assert.InRange(lesson.ExplanationParagraphs.Count,3,4);
            Assert.Contains("Learning goal:",lesson.ExplanationParagraphs[0]);
            Assert.StartsWith("Try:",lesson.ExplanationParagraphs[1]);
            Assert.Contains("Check:",lesson.SummaryPoints[2]);
            if(number==11)Assert.Contains("true or false",lesson.ExplanationParagraphs[0]);
            if(number==12)Assert.Contains("in all",lesson.ExplanationParagraphs[0]);
            if(number==13)Assert.Contains("questions",lesson.ExplanationParagraphs[0]);
            if(number==14)Assert.Contains("sort",lesson.ExplanationParagraphs[0]);
            if(number==15)Assert.Contains("optional",lesson.ExplanationParagraphs[0]);
            Assert.NotEmpty(lesson.CommonMistakes[0].Correction);
        }
    }

    [Fact]
    public void EffectiveCanonicalRepairAlreadyExistsForL11L13AndL15()
    {
        var docs=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments();
        var src=docs.SelectMany(doc=>doc.Lessons.Select(lesson=>(doc,lesson)))
            .Where(pair=>pair.lesson.LessonCode.StartsWith(Prefix,StringComparison.Ordinal))
            .ToDictionary(pair=>pair.lesson.LessonCode);
        Assert.True(src.Count>=15);
        foreach(var number in new[]{11,13,15})
        {
            var (doc,lesson)=src[Prefix+number.ToString("D2")];
            var en=Assert.Single(lesson.Translations);
            Assert.DoesNotContain("− 4 = -3",en.WorkedExamples);
            Assert.DoesNotContain("1 − 4 = -3",en.StepByStepSolutions);
            Assert.Contains("data",en.Explanation,StringComparison.OrdinalIgnoreCase);
            Assert.NotEmpty(CanonicalLessonContentMaterializer.GetEffectiveContentVersion(doc,lesson));
        }
    }
}
