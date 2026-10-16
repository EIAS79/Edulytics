using Edulytics.Core.Mathematics.Practice;
using Edulytics.Data.Seeding;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class CommonCoreG1Unit2Part3TeachingTests
{
    [Fact]
    public void ThreeCanonicalLessonsUseReviewedSourceGoalsAndContent()
    {
        var documents=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments();
        var selected=documents.SelectMany(d=>d.Lessons.Where(l=>
            CommonCoreG1Unit2Part3SourceTeachingCorrections.IsTarget(d,l))
            .Select(l=>(Doc:d,Lesson:l)))
            .OrderBy(x=>x.Lesson.LessonCode).ToArray();
        Assert.Equal(new[]{
            "PED:US-CCSS-MATH:G1:U02:L08",
            "PED:US-CCSS-MATH:G1:U02:L09",
            "PED:US-CCSS-MATH:G1:U02:L10"
        },selected.Select(x=>x.Lesson.LessonCode).ToArray());
        foreach(var row in selected)
        {
            var t=row.Lesson.Translations.Single(x=>x.CultureCode=="en");
            Assert.Equal(CommonCoreG1Unit2Part3SourceTeachingCorrections.CorrectedContentVersion,
                CanonicalLessonContentMaterializer.GetEffectiveContentVersion(row.Doc,row.Lesson));
            Assert.Contains("Step 5:",t.StepByStepSolutions);
            Assert.DoesNotContain("The source task",t.WorkedExamples);
            Assert.True(t.WorkedExamples.Length>300);
        }
        Assert.Contains("5 + [3] = 8",selected[0].Lesson.Translations.Single(x=>x.CultureCode=="en").WorkedExamples);
        Assert.Contains("8 = [4] + [4]",selected[1].Lesson.Translations.Single(x=>x.CultureCode=="en").WorkedExamples);
        Assert.Contains("Capture Squares",selected[2].Lesson.Translations.Single(x=>x.CultureCode=="en").Explanation);
    }

    [Fact]
    public void ThreeFamiliesAnswerAndRejectIncorrectOrCorruptedItems()
    {
        var families=new[]{
            "usccss.g1.u02.shake_cover.addend_unknown",
            "usccss.g1.u02.story.result_part_or_both_unknown",
            "usccss.g1.u02.center.capture_squares.sum_to10"
        };
        var generator=new ExactSkillContractQuestionEngine();
        for(var i=0;i<3;i++)
        {
            var code="PED:US-CCSS-MATH:G1:U02:L"+(8+i).ToString("D2");
            var family=families[i];
            Assert.True(LessonPracticeCapabilityResolver.TryResolve(code,out var contract));
            Assert.Equal(new[]{family},contract!.AllowedQuestionFamilies);
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out _));
            var questions=generator.Generate("g1-u02-l08-l10",family,[family],
                ExactSkillQuestionDifficulty.Standard,10,20261016+i,[]);
            Assert.Equal(10,questions.Count);
            foreach(var q in questions)
            {
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,q.CorrectAnswer));
                var answer=int.Parse(q.CorrectAnswer,System.Globalization.CultureInfo.InvariantCulture);
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,(answer+1).ToString()));
                var corrupt=new Dictionary<string,int>(q.Parameters);
                corrupt[i==2?"a":"total"]=-100;
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,corrupt,q.CorrectAnswer));
            }
        }
    }
}
