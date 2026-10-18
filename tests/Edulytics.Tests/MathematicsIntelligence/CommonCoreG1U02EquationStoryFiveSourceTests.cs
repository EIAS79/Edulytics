using Edulytics.Core.Curriculum;
using Edulytics.Core.Lessons;
using Edulytics.Data.Seeding;
using Edulytics.Services.LessonContent;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class CommonCoreG1U02EquationStoryFiveSourceTests
{
    private static IReadOnlyList<(CanonicalLessonContentPackDocument Doc,
        CanonicalLessonContentPackLesson Lesson,CanonicalLessonContentPackTranslation En)> Targets()=>
        MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(doc=>doc.Lessons.Where(l=>
                CommonCoreG1Unit2EquationsAndStoriesSourceCorrections.IsTarget(doc,l))
                .Select(l=>(Doc:doc,Lesson:l,En:l.Translations.Single(t=>t.CultureCode=="en"))))
            .OrderBy(x=>x.Lesson.LessonCode).ToArray();

    [Fact]
    public void ExactlyFiveOriginalLessonsHaveCorrectSourceBodyAndRichV2()
    {
        var rows=Targets();
        Assert.Equal(5,rows.Count);
        var titles=new[]{"Center Day 3","How Do the Stories Compare?",
            "Equations with Unknowns","Story Problems and Equations","What’s the Story?"};
        for(var i=0;i<5;i++)
        {
            var l=rows[i];
            Assert.EndsWith($"L{i+16:00}",l.Lesson.LessonCode);
            Assert.Equal(titles[i],l.En.Title);
            Assert.Contains("Goal:",l.En.KeyConceptsAndRules);
            Assert.Contains("Example 2:",l.En.WorkedExamples);
            Assert.Contains("Step 4:",l.En.StepByStepSolutions);
            Assert.DoesNotContain("The source task combines",l.En.WorkedExamples);
            Assert.Equal("CC BY 4.0",l.Lesson.SourceRights);
            Assert.Equal(CommonCoreG1Unit2EquationsAndStoriesSourceCorrections.CorrectedContentVersion,
                CanonicalLessonContentMaterializer.GetEffectiveContentVersion(l.Doc,l.Lesson));
            var rich=RichLessonContentV2RuntimeComposer.TryCompose(
                l.Lesson.LessonCode,new CanonicalLessonTranslationRecord(
                    l.En.CultureCode,l.En.Title,l.En.Explanation,l.En.KeyConceptsAndRules,
                    l.En.WorkedExamples,l.En.StepByStepSolutions,l.En.CommonMistakes,l.En.QuickSummary));
            Assert.NotNull(rich);
            Assert.NotEmpty(rich!.WorkedExamples);
        }
        Assert.Contains("Capture Squares",rows[0].En.Explanation);
        Assert.Contains("Add To",rows[1].En.Explanation);
        Assert.Contains("4+□=10",rows[2].En.Explanation);
        Assert.Contains("two equations",rows[3].En.Explanation,StringComparison.OrdinalIgnoreCase);
        Assert.Contains("WRITE a story",rows[4].En.Explanation);
    }

    [Fact]
    public void FiveSourceCrosswalkRowsKeepOriginalAndModernEditionTasksSeparate()
    {
        var dir=new DirectoryInfo(AppContext.BaseDirectory);
        while(dir is not null&&!File.Exists(Path.Combine(dir.FullName,"Edulytics.sln")))
            dir=dir.Parent;
        Assert.NotNull(dir);
        var path=Path.Combine(dir!.FullName,"artifacts","curriculum-delivery",
            "US-CCSS-MATH","G1","batches","US-CCSS-G1-007-LOCAL-SOURCE-CROSSWALK.json");
        using var doc=System.Text.Json.JsonDocument.Parse(File.ReadAllText(path));
        var rows=doc.RootElement.GetProperty("lessons").EnumerateArray().ToArray();
        Assert.Equal(5,rows.Length);
        for(var i=0;i<5;i++)
        {
            var item=rows[i];
            Assert.Equal(i+31,item.GetProperty("gradeOrdinal").GetInt32());
            Assert.Equal(Targets()[i].Lesson.LessonCode,item.GetProperty("lessonCode").GetString());
            Assert.Equal(64,item.GetProperty("originalSourceTeacherPreparationSha256").GetString()!.Length);
            Assert.Equal(64,item.GetProperty("originalSourceTeacherLessonSha256").GetString()!.Length);
            Assert.Equal(64,item.GetProperty("modernEditionStudentSource").GetProperty("sha256").GetString()!.Length);
            Assert.NotEmpty(item.GetProperty("activityStandards").EnumerateArray().ToArray());
            Assert.Contains("NOT_VERIFIED",item.GetProperty("independentOriginal2021StudentBookPdf").GetString()!);
            Assert.Equal("NOT_CERTIFIED",item.GetProperty("originalPublisherImageParity").GetString());
        }
        Assert.Equal("DIFFERENT_GOAL",rows[1].GetProperty("modernEditionStudentSource")
            .GetProperty("comparison").GetString());
    }

    [Fact]
    public void VerifiedOriginalLesson19TitleDoesNotRewriteStableIdentity()
    {
        Assert.Equal("Story Problems and Equations",
            CommonCoreG1Unit2NextFiveSourceTeachingCorrections
                .ProjectVerifiedPublisherTitle("PED:US-CCSS-MATH:G1:U02:L19",
                    "Story Problems and Equations PLC Activity"));
        Assert.Throws<InvalidOperationException>(()=>
            CommonCoreG1Unit2NextFiveSourceTeachingCorrections
                .ProjectVerifiedPublisherTitle("PED:US-CCSS-MATH:G1:U02:L19",
                    "Some unrelated publisher title"));
    }

    [Fact]
    public void AlteredSourceShaFailsClosedInsteadOfSilentlyApproving()
    {
        var row=Targets()[0];
        var actual=row.Lesson.CanonicalBodySha256;
        try
        {
            row.Lesson.CanonicalBodySha256=new string('0',64);
            Assert.Throws<InvalidOperationException>(()=>
                CommonCoreG1Unit2EquationsAndStoriesSourceCorrections.Apply(row.Doc));
        }
        finally{row.Lesson.CanonicalBodySha256=actual;}
    }
}
