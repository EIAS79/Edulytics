using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;
using Edulytics.Core.Lessons;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Edulytics.Data.Seeding;
using Edulytics.Services.LessonContent;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class CommonCoreG1Unit2StorySourceCorrectionsTests
{
    private const string Prefix = "PED:US-CCSS-MATH:G1:U02:L";

    private static IReadOnlyList<(CanonicalLessonContentPackDocument Doc,
        CanonicalLessonContentPackLesson Lesson,
        CanonicalLessonContentPackTranslation English)> GetTargets() =>
        MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(doc => doc.Lessons.Where(l =>
                CommonCoreG1Unit2StorySourceCorrections.IsTarget(doc, l))
                .Select(l => (Doc: doc, Lesson: l,
                    English: l.Translations.Single(t => t.CultureCode == "en"))))
            .OrderBy(x => x.Lesson.LessonCode)
            .ToArray();

    [Fact]
    public void ThreeLessonsAreSourceBoundAndActuallyTeachDistinctOriginalGoals()
    {
        var targets = GetTargets();
        Assert.Equal(new[] {Prefix+"01",Prefix+"02",Prefix+"03"},
            targets.Select(x => x.Lesson.LessonCode).ToArray());

        foreach (var t in targets)
        {
            Assert.Equal(CommonCoreG1Unit2StorySourceCorrections.CorrectedContentVersion,
                CanonicalLessonContentMaterializer.GetEffectiveContentVersion(t.Doc,t.Lesson));
            Assert.Equal("CC BY 4.0",t.Lesson.SourceRights);
            Assert.StartsWith("https://im.kendallhunt.com/k5/teachers/grade-1/unit-2/",
                t.Lesson.SourceUrl);
            Assert.Contains("Step 5:",t.English.StepByStepSolutions);
            Assert.DoesNotContain("The source task",t.English.WorkedExamples);
            Assert.DoesNotContain("They solve",t.English.Explanation);
            Assert.Contains("story",t.English.Explanation,StringComparison.OrdinalIgnoreCase);
            Assert.True(t.English.WorkedExamples.Length>260);
            Assert.True(t.English.CommonMistakes.Length>150);
            Assert.True(t.English.QuickSummary.Length>65);
            var rendered = RichLessonContentV2RuntimeComposer.TryCompose(
                t.Lesson.LessonCode,
                new CanonicalLessonTranslationRecord(
                    t.English.CultureCode,t.English.Title,t.English.Explanation,
                    t.English.KeyConceptsAndRules,t.English.WorkedExamples,
                    t.English.StepByStepSolutions,t.English.CommonMistakes,
                    t.English.QuickSummary));
            Assert.NotNull(rendered);
            Assert.NotEmpty(rendered!.WorkedExamples);
        }

        var first=targets[0].English;
        Assert.Contains("expression",first.KeyConceptsAndRules);
        Assert.Contains("4 + 3",first.WorkedExamples);
        Assert.Contains("8 - 2",first.WorkedExamples);
        Assert.Contains("join",first.Explanation);

        var second=targets[1].English;
        Assert.Contains("equation",second.KeyConceptsAndRules);
        Assert.Contains("6 + 3 = 9",second.WorkedExamples);
        Assert.Contains("9 - 4 = 5",second.WorkedExamples);
        Assert.Contains("Box",second.StepByStepSolutions);
        Assert.Equal(new[] {"CCSS:1.OA.A.1","CCSS:1.OA.C.6"},targets[1].Lesson.OutcomeCodes);

        var third=targets[2].English;
        Assert.Contains("6 + [2] = 8",third.WorkedExamples);
        Assert.Contains("3 + [6] = 9",third.WorkedExamples);
        Assert.Contains("not necessarily",third.StepByStepSolutions);
        Assert.Contains("unknown",third.Explanation,StringComparison.OrdinalIgnoreCase);
        Assert.Equal(2,8-6);
        Assert.Equal(6,9-3);
        Assert.Equal(8,6+2);
        Assert.Equal(9,3+6);
        Assert.Equal(7,4+3);
        Assert.Equal(5,9-4);
    }

    [Fact]
    public void SourceCorrectionIsIdempotentAndNeverTouchesNextUnreviewedLesson()
    {
        var rows=GetTargets();
        foreach(var row in rows)
        {
            var before=CanonicalLessonContentMaterializer.ComputeLessonFingerprint(
                row.Doc,row.Lesson);
            CanonicalLessonContentMaterializer.Materialize(row.Doc);
            Assert.Equal(before,CanonicalLessonContentMaterializer.ComputeLessonFingerprint(
                row.Doc,row.Lesson));
        }

        var doc=rows[0].Doc;
        var other=Assert.Single(doc.Lessons,x=>x.LessonCode==Prefix+"04");
        Assert.False(CommonCoreG1Unit2StorySourceCorrections.IsTarget(doc,other));
    }

    [Fact]
    public void ExactSourceIdentityAndExistingContentVersionAreRequired()
    {
        foreach(var row in GetTargets())
        {
            var oldSha=row.Lesson.CanonicalBodySha256;
            try
            {
                row.Lesson.CanonicalBodySha256=new string('0',64);
                Assert.Throws<InvalidOperationException>(()=>
                    CommonCoreG1Unit2StorySourceCorrections.Apply(row.Doc));
            }
            finally { row.Lesson.CanonicalBodySha256=oldSha; }

            Assert.True(CommonCoreG1Unit2StorySourceCorrections.CanUpgradeExisting(
                row.Doc,row.Lesson,CommonCoreRichContentCorrections.CorrectionContentVersion));
            Assert.False(CommonCoreG1Unit2StorySourceCorrections.CanUpgradeExisting(
                row.Doc,row.Lesson,"unexpected-content-version"));

            Assert.Throws<InvalidOperationException>(()=>
                CommonCoreG1Unit2StorySourceCorrections.ValidateExistingBodyBeforeUpgrade(
                    row.Doc,row.Lesson,"unexpected-content-version",[]));
            Assert.Throws<InvalidOperationException>(()=>
                CommonCoreG1Unit2StorySourceCorrections.ValidateExistingBodyBeforeUpgrade(
                    row.Doc,row.Lesson,CommonCoreRichContentCorrections.CorrectionContentVersion,
                    [new CurriculumLessonContentTranslation
                    {
                        CultureCode="en",
                        StepByStepSolutions="Unexpected overwritten student prose",
                        WorkedExamples="Unexpected"
                    }]));
            CommonCoreG1Unit2StorySourceCorrections.ValidateExistingBodyBeforeUpgrade(
                row.Doc,row.Lesson,CommonCoreRichContentCorrections.CorrectionContentVersion,
                []);
        }
    }

    [Fact]
    public void AllThreeStoryPracticeContractsUseExactSupportedSourceFamilies()
    {
        var expected=new[]
        {
            "usccss.g1.u02.story.choose_expression",
            "usccss.g1.u02.story.choose_equation",
            "usccss.g1.u02.story.add_change_unknown"
        };
        for(var i=1;i<=3;i++)
        {
            var code=Prefix+i.ToString("D2");
            Assert.True(LessonPracticeCapabilityResolver.TryResolve(code,out var contract),code);
            Assert.NotNull(contract);
            Assert.Equal("READY_VERIFIED",contract!.Readiness);
            Assert.Equal(new[] {expected[i-1]},contract.AllowedQuestionFamilies);
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(expected[i-1]));
        }
    }

    [Fact]
    public void StoryActionAndUnknownQuestionsGenerateGradeRejectWrongAnswersAndTampering()
    {
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var family in new[]
        {
            "usccss.g1.u02.story.choose_expression",
            "usccss.g1.u02.story.choose_equation",
            "usccss.g1.u02.story.add_change_unknown"
        })
        {
            var questions=engine.Generate(
                "g1-u02-source-story-targeted",family,[family],
                ExactSkillQuestionDifficulty.Standard,12,20261016,[]);
            Assert.Equal(12,questions.Count);
            Assert.Equal(12,questions.Select(x=>x.ExposureFingerprint)
                .Distinct(StringComparer.Ordinal).Count());
            foreach(var q in questions)
            {
                Assert.Equal(family,q.Family);
                Assert.Contains("A child",q.Prompt);
                Assert.True(q.Solution.Length > 35);
                Assert.True(ExactSkillContractQuestionEngine.Verify(
                    family,q.Parameters,q.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(
                    family,q.Parameters,q.CorrectAnswer=="1"?"2":"1"));
                Assert.False(ExactSkillContractQuestionEngine.Verify(
                    family,q.Parameters,"-1"));
                Assert.False(ExactSkillContractQuestionEngine.Verify(
                    family,q.Parameters,"not a number"));
                var wrong=q.Parameters.ToDictionary(k=>k.Key,k=>k.Value);
                wrong["start"]=25;
                Assert.False(ExactSkillContractQuestionEngine.Verify(
                    family,wrong,q.CorrectAnswer));
            }
        }
    }

    [Fact]
    public void MathematicalOraclesCheckExpressionVsEquationAndNonfinalUnknownAnswer()
    {
        const string expression="usccss.g1.u02.story.choose_expression";
        const string equation="usccss.g1.u02.story.choose_equation";
        const string unknown="usccss.g1.u02.story.add_change_unknown";
        // 6 + 2 = 8. Choice 1 is addition with flip=0.
        IReadOnlyDictionary<string,int> add=new Dictionary<string,int>
        {
            ["start"]=6,["amount"]=2,["action"]=0,["noun"]=0,["flip"]=0
        };
        Assert.True(ExactSkillContractQuestionEngine.Verify(expression,add,"1"));
        Assert.True(ExactSkillContractQuestionEngine.Verify(equation,add,"1"));
        Assert.True(ExactSkillContractQuestionEngine.Verify(unknown,add,"2"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(unknown,add,"8"));

        // Same story, reordered options: now subtraction is choice 1.
        var flipped=add.ToDictionary(x=>x.Key,x=>x.Value);
        flipped["flip"]=1;
        Assert.True(ExactSkillContractQuestionEngine.Verify(expression,flipped,"2"));
        Assert.True(ExactSkillContractQuestionEngine.Verify(equation,flipped,"2"));

        // 7 - 3 = 4, with subtraction listed first after option flip.
        IReadOnlyDictionary<string,int> take=new Dictionary<string,int>
        {
            ["start"]=7,["amount"]=3,["action"]=1,["noun"]=0,["flip"]=1
        };
        Assert.True(ExactSkillContractQuestionEngine.Verify(expression,take,"1"));
        Assert.True(ExactSkillContractQuestionEngine.Verify(equation,take,"1"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(unknown,take,"3"));
    }
}
