using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;
using Edulytics.Core.Lessons;
using Edulytics.Data.Seeding;
using Edulytics.Services.LessonContent;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class CommonCoreG1U02NextFiveSourceTests
{
    private static IReadOnlyList<(CanonicalLessonContentPackDocument Doc,
        CanonicalLessonContentPackLesson Lesson,CanonicalLessonContentPackTranslation En)> Targets()=>
        MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(doc=>doc.Lessons.Where(l=>
                CommonCoreG1Unit2NextFiveSourceTeachingCorrections.IsTarget(doc,l))
                .Select(l=>(Doc:doc,Lesson:l,En:l.Translations.Single(t=>t.CultureCode=="en"))))
            .OrderBy(x=>x.Lesson.LessonCode).ToArray();

    [Fact]
    public void OriginalSourceIdentitiesAndExplicitGoalsSurviveMaterialization()
    {
        var targets=Targets();
        Assert.Equal(5,targets.Count);
        for(var i=0;i<5;i++)
        {
            var t=targets[i];
            Assert.EndsWith($"L{i+6:00}",t.Lesson.LessonCode);
            Assert.Equal(CommonCoreG1Unit2NextFiveSourceTeachingCorrections.CorrectedContentVersion,
                CanonicalLessonContentMaterializer.GetEffectiveContentVersion(t.Doc,t.Lesson));
            Assert.StartsWith("https://im.kendallhunt.com/k5/teachers/grade-1/unit-2/lesson-",
                t.Lesson.SourceUrl);
            Assert.Equal("CC BY 4.0",t.Lesson.SourceRights);
            Assert.Contains("Step 4:",t.En.StepByStepSolutions);
            Assert.Contains("Example 2:",t.En.WorkedExamples);
            Assert.DoesNotContain("The source task",t.En.WorkedExamples);
            Assert.DoesNotContain("in a way that makes sense to them",t.En.Explanation);
            Assert.True(t.En.CommonMistakes.Length>140);
            var rich=RichLessonContentV2RuntimeComposer.TryCompose(
                t.Lesson.LessonCode,new CanonicalLessonTranslationRecord(
                    t.En.CultureCode,t.En.Title,t.En.Explanation,t.En.KeyConceptsAndRules,
                    t.En.WorkedExamples,t.En.StepByStepSolutions,t.En.CommonMistakes,
                    t.En.QuickSummary));
            Assert.NotNull(rich);
            Assert.NotEmpty(rich!.WorkedExamples);
            var fingerprint=CanonicalLessonContentMaterializer.ComputeLessonFingerprint(t.Doc,t.Lesson);
            CanonicalLessonContentMaterializer.Materialize(t.Doc);
            Assert.Equal(fingerprint,
                CanonicalLessonContentMaterializer.ComputeLessonFingerprint(t.Doc,t.Lesson));
        }
        Assert.Equal("Problems about Pets",targets[0].En.Title);
        Assert.Equal("One Part Red, One Part Yellow",targets[1].En.Title);
        Assert.Equal("One Part Red, How Many Yellow?",targets[2].En.Title);
        Assert.Equal("What Is the Unknown?",targets[3].En.Title);
        Assert.Equal("Center Day 2",targets[4].En.Title);
        Assert.Contains("two matching addition equations",targets[0].En.Explanation, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("different combinations",targets[1].En.Explanation);
        Assert.Contains("under a cup",targets[2].En.Explanation);
        Assert.Contains("Both Parts Unknown",targets[3].En.KeyConceptsAndRules);
        Assert.Contains("fourth side",targets[4].En.Explanation);
    }

    [Fact]
    public void AChangedOriginalSourceHashFailsClosed()
    {
        var t=Targets()[0];
        var original=t.Lesson.CanonicalBodySha256;
        try
        {
            t.Lesson.CanonicalBodySha256=new string('0',64);
            Assert.Throws<InvalidOperationException>(()=>
                CommonCoreG1Unit2NextFiveSourceTeachingCorrections.Apply(t.Doc));
        }
        finally
        {
            t.Lesson.CanonicalBodySha256=original;
        }
    }

    [Theory]
    [InlineData("PED:US-CCSS-MATH:G1:U02:L06")]
    [InlineData("PED:US-CCSS-MATH:G1:U02:L07")]
    [InlineData("PED:US-CCSS-MATH:G1:U02:L08")]
    [InlineData("PED:US-CCSS-MATH:G1:U02:L09")]
    [InlineData("PED:US-CCSS-MATH:G1:U02:L10")]
    public void EachLessonRoutesASourceSpecificActualPracticeAttempt(string code)
    {
        var contract=LessonPracticeContractRegistry.All.Single(x=>x.LessonCode==code)
            .ToLegacyStage18Contract();
        var items=new Edulytics.Services.Practice.Stage18SkillContractPracticeEngine()
            .GenerateComposed(Guid.NewGuid(),Guid.NewGuid(),Guid.NewGuid(),contract,
                Edulytics.Services.Practice.StudentPrivatePracticeDifficulty.MyLevel,
                5,20261017,[],Guid.NewGuid());
        Assert.Equal(5,items.Count);
        var quality=new Edulytics.Services.Practice.PracticeSessionQualityValidator()
            .Validate(contract,items,5);
        Assert.True(quality.IsReady,string.Join(";",quality.ReasonCodes));
        foreach(var item in items)
        {
            Assert.True(UsCcssG1Unit2NextFiveQuestions.GradePersistedAnswer(
                item.GenerationFamily,item.CorrectAnswer,item.CorrectAnswer));
            Assert.False(UsCcssG1Unit2NextFiveQuestions.GradePersistedAnswer(
                item.GenerationFamily,item.CorrectAnswer,"999"));
        }
    }
}
