using Edulytics.Core.Curriculum;
using Edulytics.Core.Lessons;
using Edulytics.Data.Seeding;
using Edulytics.Services.LessonContent;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class CommonCoreG1NextSourceFiveTests
{
    private static IReadOnlyList<(CanonicalLessonContentPackDocument Doc,
        CanonicalLessonContentPackLesson Lesson,CanonicalLessonContentPackTranslation En)> Targets()=>
        MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(doc=>doc.Lessons.Where(l=>
                CommonCoreG1NextSourceFiveCorrections.IsTarget(doc,l))
                .Select(l=>(Doc:doc,Lesson:l,En:l.Translations.Single(t=>t.CultureCode=="en"))))
            .OrderBy(x=>x.Lesson.LessonCode).ToArray();
    private static readonly string[] Codes=
    [
        "PED:US-CCSS-MATH:G1:U02:L21",
        "PED:US-CCSS-MATH:G1:U02:L22",
        "PED:US-CCSS-MATH:G1:U03:L01",
        "PED:US-CCSS-MATH:G1:U03:L02",
        "PED:US-CCSS-MATH:G1:U03:L03"
    ];

    [Fact]
    public void AllFiveSourceBoundLearnerBodiesMatchOriginalGoalsAndRenderInRichV2()
    {
        var rows=Targets();
        Assert.Equal(5,rows.Count);
        Assert.Equal(Codes,rows.Select(x=>x.Lesson.LessonCode).ToArray());
        foreach(var row in rows)
        {
            Assert.Contains("Goal:",row.En.KeyConceptsAndRules);
            Assert.Contains("Example 2:",row.En.WorkedExamples);
            Assert.Contains("Step 4:",row.En.StepByStepSolutions);
            Assert.DoesNotContain("The source task combines",row.En.WorkedExamples);
            Assert.Equal("CC BY 4.0",row.Lesson.SourceRights);
            Assert.Equal(CommonCoreG1NextSourceFiveCorrections.Version,
                CanonicalLessonContentMaterializer.GetEffectiveContentVersion(row.Doc,row.Lesson));
            var rich=RichLessonContentV2RuntimeComposer.TryCompose(
                row.Lesson.LessonCode,new CanonicalLessonTranslationRecord(
                    row.En.CultureCode,row.En.Title,row.En.Explanation,row.En.KeyConceptsAndRules,
                    row.En.WorkedExamples,row.En.StepByStepSolutions,row.En.CommonMistakes,
                    row.En.QuickSummary));
            Assert.NotNull(rich);
            Assert.NotEmpty(rich!.WorkedExamples);
        }
        Assert.Contains("Number Puzzles",rows[0].En.Explanation);
        Assert.Contains("survey",rows[1].En.Explanation,StringComparison.OrdinalIgnoreCase);
        Assert.Contains("Not yet",rows[2].En.Explanation,StringComparison.OrdinalIgnoreCase);
        Assert.Contains("count on",rows[3].En.Explanation,StringComparison.OrdinalIgnoreCase);
        Assert.Contains("BOTH sides",rows[4].En.KeyConceptsAndRules,StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void CrosswalkPreservesAllFive2021SourceIdentitiesAndHonestLimitations()
    {
        var dir=new DirectoryInfo(AppContext.BaseDirectory);
        while(dir is not null&&!File.Exists(Path.Combine(dir.FullName,"Edulytics.sln")))
            dir=dir.Parent;
        Assert.NotNull(dir);
        var path=Path.Combine(dir!.FullName,"artifacts","curriculum-delivery","US-CCSS-MATH",
            "G1","batches","US-CCSS-G1-008-LOCAL-SOURCE-CROSSWALK.json");
        using var doc=System.Text.Json.JsonDocument.Parse(File.ReadAllText(path));
        Assert.Equal("EDULYTICS-GRADE-FIRST-5-V1",
            doc.RootElement.GetProperty("planId").GetString());
        var rows=doc.RootElement.GetProperty("lessons").EnumerateArray().ToArray();
        Assert.Equal(5,rows.Length);
        for(int i=0;i<5;i++)
        {
            Assert.Equal(i+36,rows[i].GetProperty("gradeOrdinal").GetInt32());
            Assert.Equal(Codes[i],rows[i].GetProperty("lessonCode").GetString());
            Assert.Equal(64,rows[i].GetProperty("originalTeacherPreparationSha256").GetString()!.Length);
            Assert.Equal(64,rows[i].GetProperty("originalTeacherLessonSha256").GetString()!.Length);
            Assert.NotEmpty(rows[i].GetProperty("sourceGoals").EnumerateArray().ToArray());
        }
        Assert.Equal("1.2.22",rows[1].GetProperty("sourceLessonCode").GetString());
        Assert.Equal("PENDING",doc.RootElement.GetProperty("ownerProductionUat").GetString());
    }
}
