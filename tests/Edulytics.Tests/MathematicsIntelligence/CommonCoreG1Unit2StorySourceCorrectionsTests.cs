using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;
using Edulytics.Core.Lessons;
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
        Assert.Contains("joining",first.Explanation);

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
}
