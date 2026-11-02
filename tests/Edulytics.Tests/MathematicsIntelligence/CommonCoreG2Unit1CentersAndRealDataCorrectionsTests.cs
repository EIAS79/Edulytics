using Edulytics.Data.Seeding;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Grade 2 source/identity tests authored for Stage D, not executed now.
/// Original class responses and physical gameboard evidence must be teacher
/// verified; these source assertions cannot replace that observation.
/// </summary>
public sealed class CommonCoreG2Unit1CentersAndRealDataCorrectionsTests
{
    private static readonly string[] Codes =
    [
        "PED:US-CCSS-MATH:G2:U01:L06",
        "PED:US-CCSS-MATH:G2:U01:L07"
    ];

    [Fact]
    public void BothSourceLessonsKeepTheirDifferentOriginalLearningActivities()
    {
        var rows=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(doc=>doc.Lessons
                .Where(lesson=>CommonCoreG2Unit1CentersAndRealDataCorrections
                    .IsTarget(doc,lesson))
                .Select(lesson=>(Document:doc,Lesson:lesson)))
            .ToArray();
        Assert.Equal(Codes,rows.Select(x=>x.Lesson.LessonCode).ToArray());
        foreach(var x in rows)
            Assert.Equal(CommonCoreG2Unit1CentersAndRealDataCorrections.Version,
                CanonicalLessonContentMaterializer.GetEffectiveContentVersion(
                    x.Document,x.Lesson));

        var centers=rows[0].Lesson.Translations.Single(t=>t.CultureCode=="en");
        Assert.Contains("Number Puzzles Stage 2",centers.Explanation);
        Assert.Contains("only once",centers.Explanation,StringComparison.OrdinalIgnoreCase);
        Assert.Contains("two-digit sum",centers.StepByStepSolutions);

        var data=rows[1].Lesson.Translations.Single(t=>t.CultureCode=="en");
        Assert.Contains("REAL CLASS",data.Explanation);
        Assert.Contains("not have to be grouped",data.KeyConceptsAndRules);
        Assert.Contains("A display does not have to be grouped",data.KeyConceptsAndRules);
        Assert.Contains("Only actual survey responses",data.CommonMistakes);
    }

    [Fact]
    public void DifferentSourceHashOrVersionCannotClaimOriginalGradeTwoCorrection()
    {
        var row=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(doc=>doc.Lessons
                .Where(lesson=>CommonCoreG2Unit1CentersAndRealDataCorrections
                    .IsTarget(doc,lesson))
                .Select(lesson=>(Document:doc,Lesson:lesson)))
            .First();
        Assert.True(CommonCoreG2Unit1CentersAndRealDataCorrections.CanUpgradeExisting(
            row.Document,row.Lesson,
            CommonCoreRichContentCorrections.CorrectionContentVersion));
        Assert.False(CommonCoreG2Unit1CentersAndRealDataCorrections.CanUpgradeExisting(
            row.Document,row.Lesson,"unreviewed-v2"));
        var previousSha=row.Lesson.CanonicalBodySha256;
        try
        {
            row.Lesson.CanonicalBodySha256=new string('0',64);
            Assert.Throws<InvalidOperationException>(()=>
                CommonCoreG2Unit1CentersAndRealDataCorrections.Apply(row.Document));
        }
        finally { row.Lesson.CanonicalBodySha256=previousSha; }
    }
}
