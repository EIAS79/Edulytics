using Edulytics.Core.Curriculum;
using Edulytics.Core.Lessons;
using Edulytics.Data.Seeding;
using Edulytics.Services.LessonContent;
using System.Text.Json;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class CommonCoreG1SourceFiveNextTests
{
    private static readonly string[] Codes=[
        "PED:US-CCSS-MATH:G1:U03:L04",
        "PED:US-CCSS-MATH:G1:U03:L05",
        "PED:US-CCSS-MATH:G1:U03:L06",
        "PED:US-CCSS-MATH:G1:U03:L07",
        "PED:US-CCSS-MATH:G1:U03:L08"
    ];
    private static IReadOnlyList<(CanonicalLessonContentPackDocument Doc,
        CanonicalLessonContentPackLesson Lesson,CanonicalLessonContentPackTranslation En)> Targets()=>
        MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(doc=>doc.Lessons.Where(l=>
                CommonCoreG1SourceFiveNextCorrections.IsTarget(doc,l))
                .Select(l=>(Doc:doc,Lesson:l,En:l.Translations.Single(t=>t.CultureCode=="en"))))
            .OrderBy(x=>x.Lesson.LessonCode).ToArray();

    [Fact]
    public void AllFiveSourceCorrectedBodiesHaveExplicitOriginalGoalsAndRichV2WorkedMethods()
    {
        var rows=Targets();
        Assert.Equal(5,rows.Count);
        Assert.Equal(Codes,rows.Select(x=>x.Lesson.LessonCode));
        foreach(var row in rows)
        {
            Assert.Equal("CC BY 4.0",row.Lesson.SourceRights);
            Assert.Equal(CommonCoreG1SourceFiveNextCorrections.Version,
                CanonicalLessonContentMaterializer.GetEffectiveContentVersion(row.Doc,row.Lesson));
            Assert.Contains("Goal:",row.En.KeyConceptsAndRules);
            Assert.Contains("Example 2:",row.En.WorkedExamples);
            Assert.Contains("Step 4:",row.En.StepByStepSolutions);
            Assert.DoesNotContain("The source task combines",row.En.WorkedExamples);
            Assert.DoesNotContain("you will introduce you",row.En.Explanation);
            var rich=RichLessonContentV2RuntimeComposer.TryCompose(
                row.Lesson.LessonCode,new CanonicalLessonTranslationRecord(
                    row.En.CultureCode,row.En.Title,row.En.Explanation,row.En.KeyConceptsAndRules,
                    row.En.WorkedExamples,row.En.StepByStepSolutions,row.En.CommonMistakes,
                    row.En.QuickSummary));
            Assert.NotNull(rich);
            Assert.NotEmpty(rich!.WorkedExamples);
        }
        Assert.Contains("ALL the ways",rows[0].En.KeyConceptsAndRules);
        Assert.Contains("number-string",rows[1].En.KeyConceptsAndRules);
        Assert.Contains("Start Unknown",rows[2].En.Explanation);
        Assert.Contains("both",rows[3].En.Explanation,StringComparison.OrdinalIgnoreCase);
        Assert.Contains("unit called a ten",rows[4].En.Explanation,StringComparison.Ordinal);
        Assert.Contains("10 to 19",rows[4].En.Explanation);
        Assert.Equal(["CCSS:1.NBT.B.2"],rows[4].Lesson.OutcomeCodes);
    }

    [Fact]
    public void Original2021SourcesAreFingerprintedAndExactlyFiveApprovalRowsPreserved()
    {
        var folder=new DirectoryInfo(AppContext.BaseDirectory);
        while(folder!=null&&!File.Exists(Path.Combine(folder.FullName,"Edulytics.sln")))
            folder=folder.Parent;
        Assert.NotNull(folder);
        var path=Path.Combine(folder!.FullName,"artifacts","curriculum-delivery",
            "US-CCSS-MATH","G1","batches","US-CCSS-G1-009-LOCAL-SOURCE-CROSSWALK.json");
        using var json=JsonDocument.Parse(File.ReadAllText(path));
        var obj=json.RootElement;
        Assert.Equal("EDULYTICS-GRADE-FIRST-5-V1",obj.GetProperty("planId").GetString());
        Assert.Equal("US-CCSS-G1-009",obj.GetProperty("batchId").GetString());
        Assert.Equal("PENDING",obj.GetProperty("ownerProductionUat").GetString());
        var lessons=obj.GetProperty("lessons").EnumerateArray().ToArray();
        Assert.Equal(5,lessons.Length);
        for(int i=0;i<5;i++)
        {
            var row=lessons[i];
            Assert.Equal(41+i,row.GetProperty("gradeOrdinal").GetInt32());
            Assert.Equal(Codes[i],row.GetProperty("lessonCode").GetString());
            Assert.Equal("1.3."+ (i+4),row.GetProperty("sourceLessonCode").GetString());
            Assert.Equal(64,row.GetProperty("originalTeacherPreparationSha256").GetString()!.Length);
            Assert.Equal(64,row.GetProperty("originalTeacherLessonSha256").GetString()!.Length);
            Assert.Equal(64,row.GetProperty("original2021StudentTaskPdfSha256").GetString()!.Length);
            var goals=row.GetProperty("sourceGoals").EnumerateArray().ToArray();
            Assert.Equal(2,goals.Length);
            Assert.All(goals,g=>Assert.StartsWith("usccss.g1.u03.l0",
                g.GetProperty("practiceFamily").GetString()));
        }
    }
}
