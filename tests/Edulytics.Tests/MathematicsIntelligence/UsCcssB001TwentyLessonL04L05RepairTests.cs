using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;
using Edulytics.Core.Lessons;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Data.Seeding;
using Edulytics.Services.LessonContent;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class UsCcssB001TwentyLessonL04L05RepairTests
{
    private const string Prefix = "PED:US-CCSS-MATH:G1:U02:L";
    private static readonly string[] Families =
    [
        "usccss.g1.u02.story.mixed_result_or_change",
        "usccss.g1.u02.center.picture_story_equation"
    ];

    private static IReadOnlyList<(CanonicalLessonContentPackDocument Doc,
        CanonicalLessonContentPackLesson Lesson,
        CanonicalLessonContentPackTranslation En)> Targets() =>
        MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(doc => doc.Lessons
                .Where(l => CommonCoreG1Unit2Part2SourceTeachingCorrections.IsTarget(doc,l))
                .Select(l => (Doc:doc,Lesson:l,En:l.Translations.Single(t=>t.CultureCode=="en"))))
            .OrderBy(x => x.Lesson.LessonCode,StringComparer.Ordinal).ToArray();

    [Fact]
    public void ExactTwoB001LearningBodiesAreSourceGuardedAndRichV2Readable()
    {
        var targets=Targets();
        Assert.Equal(new[]{Prefix+"04",Prefix+"05"},targets.Select(x=>x.Lesson.LessonCode));
        foreach(var t in targets)
        {
            Assert.Equal(CommonCoreG1Unit2Part2SourceTeachingCorrections.CorrectedContentVersion,
                CanonicalLessonContentMaterializer.GetEffectiveContentVersion(t.Doc,t.Lesson));
            Assert.Contains("Step 5:",t.En.StepByStepSolutions);
            Assert.DoesNotContain("The source task",t.En.WorkedExamples);
            Assert.StartsWith("https://im.kendallhunt.com/",t.Lesson.SourceUrl);
            Assert.True(t.En.WorkedExamples.Length>310);
            var rich=RichLessonContentV2RuntimeComposer.TryCompose(t.Lesson.LessonCode,
                new CanonicalLessonTranslationRecord(t.En.CultureCode,t.En.Title,
                    t.En.Explanation,t.En.KeyConceptsAndRules,t.En.WorkedExamples,
                    t.En.StepByStepSolutions,t.En.CommonMistakes,t.En.QuickSummary));
            Assert.NotNull(rich);
            Assert.NotEmpty(rich!.WorkedExamples);
            var before=CanonicalLessonContentMaterializer.ComputeLessonFingerprint(t.Doc,t.Lesson);
            CanonicalLessonContentMaterializer.Materialize(t.Doc);
            Assert.Equal(before,CanonicalLessonContentMaterializer.ComputeLessonFingerprint(t.Doc,t.Lesson));
            var oldSha=t.Lesson.CanonicalBodySha256;
            try
            {
                t.Lesson.CanonicalBodySha256=new string('0',64);
                Assert.Throws<InvalidOperationException>(()=>
                    CommonCoreG1Unit2Part2SourceTeachingCorrections.Apply(t.Doc));
            }
            finally {t.Lesson.CanonicalBodySha256=oldSha;}
        }
        Assert.Contains("4 + [3] = 7",targets[0].En.WorkedExamples);
        Assert.Contains("9 - 2 = [7]",targets[0].En.WorkedExamples);
        Assert.Contains("Math Stories",targets[1].En.Explanation);
        Assert.Contains("Sort and Display",targets[1].En.KeyConceptsAndRules);
    }

    [Fact]
    public void OnlyL04L05AreRoutedAndQuestionsHaveIndependentExactAnswerChecks()
    {
        var engine=new ExactSkillContractQuestionEngine();
        for(var i=0;i<2;i++)
        {
            var code=Prefix+(4+i).ToString("D2");
            var family=Families[i];
            Assert.True(LessonPracticeCapabilityResolver.TryResolve(code,out var practice),code);
            Assert.NotNull(practice);
            Assert.Equal("READY_VERIFIED",practice!.Readiness);
            Assert.Equal(i==0
                ? new[]{family,UsCcssG1Unit2Part2Questions.WriteResultOrChangeEquation}
                : new[]{family},practice.AllowedQuestionFamilies);
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var interaction));
            Assert.NotNull(interaction);
            Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad,interaction!.InteractionKind);
        }
        Assert.False(ExactSkillContractQuestionEngine.SupportsFamily(
            "usccss.g1.u02.pets.put_together_total"));
        Assert.False(ExactSkillContractQuestionEngine.SupportsFamily(
            "usccss.g1.u02.shake_spill.two_unknown_parts"));
        foreach(var family in Families)
        foreach(var seed in new[]{20261016,20261017,20261018})
        {
            var questions=engine.Generate("b001-exact-first20",family,[family],
                ExactSkillQuestionDifficulty.Standard,10,seed,[]);
            Assert.Equal(10,questions.Count);
            Assert.Equal(10,questions.Select(x=>x.ExposureFingerprint)
                .Distinct(StringComparer.Ordinal).Count());
            foreach(var q in questions)
            {
                Assert.Equal(family,q.Family);
                Assert.True(ExactSkillContractQuestionEngine.Verify(
                    family,q.Parameters,q.CorrectAnswer));
                var expected=int.Parse(q.CorrectAnswer,
                    System.Globalization.CultureInfo.InvariantCulture);
                Assert.InRange(expected,0,10);
                Assert.False(ExactSkillContractQuestionEngine.Verify(
                    family,q.Parameters,(expected+11).ToString()));
                Assert.False(ExactSkillContractQuestionEngine.Verify(
                    family,q.Parameters,"?"));
                var altered=new Dictionary<string,int>(q.Parameters,StringComparer.Ordinal)
                {["start"]=-50};
                Assert.False(ExactSkillContractQuestionEngine.Verify(
                    family,altered,q.CorrectAnswer));
                if(family==Families[0])
                    Assert.Equal(q.Parameters["mode"] switch {
                        0=>q.Parameters["start"]+q.Parameters["change"],
                        1=>q.Parameters["change"],_=>q.Parameters["start"]-q.Parameters["change"]
                    },expected);
                else
                    Assert.Equal(q.Parameters["action"]==q.Parameters["flip"]?1:2,expected);
            }
        }
    }
}
