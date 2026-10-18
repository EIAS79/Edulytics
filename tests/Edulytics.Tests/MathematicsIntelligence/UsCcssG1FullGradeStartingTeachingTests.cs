using Edulytics.Core.Curriculum;
using Edulytics.Data.Seeding;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class UsCcssG1FullGradeStartingTeachingTests
{
    [Fact]
    public void FirstFiveNewGradeLessons_HaveSourceBoundCorrectedTeaching()
    {
        var doc=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .Single(x=>x.PackCode=="US-CCSS-MATH"&&x.VersionCode=="CCSSM-2010"&&
                x.Lessons.Any(l=>l.LessonCode=="PED:US-CCSS-MATH:G1:U03:L15"));
        CanonicalLessonContentMaterializer.Materialize(doc);
        string text(int n)
        {
            var row=doc.Lessons.Single(x=>x.LessonCode==$"PED:US-CCSS-MATH:G1:U03:L{n:00}");
            Assert.True(CommonCoreG1FullGradeStartingContentCorrections.IsTarget(doc,row));
            Assert.Equal(CommonCoreG1FullGradeStartingContentCorrections.Version,
                CanonicalLessonContentMaterializer.GetEffectiveContentVersion(doc,row));
            var tr=row.Translations.Single(x=>x.CultureCode=="en");
            return tr.Explanation+"\n"+tr.KeyConceptsAndRules+"\n"+tr.WorkedExamples+"\n"+tr.StepByStepSolutions+"\n"+tr.CommonMistakes+"\n"+tr.QuickSummary;
        }
        Assert.Contains("13 - 7 = 6",text(14));
        Assert.Contains("3 + 8 + 7 = 10 + 8 = 18",text(15));
        Assert.Contains("6 + 5 + 4 = 15 + 10 is FALSE",text(16));
        Assert.Contains("8 + 6 = 10 + 4 = 14",text(17));
        Assert.Contains("9 + 4 = 8 + 5 = 7 + 6 = 13",text(18));
        Assert.Contains("7+8=7+7+1=14+1=15",text(19));
        Assert.Contains("5+3+4=12",text(20));
        Assert.Contains("3+6+7=3+7+6=10+6=16",text(21));
        Assert.Contains("14-6=14-4-2=8",text(22));
        Assert.Contains("13-6=13-3-3=10-3=7",text(23));
        var historical=doc.Lessons.Single(x=>x.LessonCode=="PED:US-CCSS-MATH:G1:U03:L13");
        Assert.False(CommonCoreG1FullGradeStartingContentCorrections.IsTarget(doc,historical));
    }
    [Fact]
    public void SourceBoundFiveTargets_ExcludePrior50AndAllOtherGrades()
    {
        var docs=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments();
        var targets=docs.SelectMany(d=>d.Lessons.Where(l=>
            CommonCoreG1FullGradeStartingContentCorrections.IsTarget(d,l)))
            .Select(l=>l.LessonCode).ToArray();
        Assert.Equal(10,targets.Length);
        Assert.Equal(10,targets.Distinct(StringComparer.Ordinal).Count());
        Assert.Equal(new[]{14,15,16,17,18,19,20,21,22,23}.Select(n=>$"PED:US-CCSS-MATH:G1:U03:L{n:00}"),targets);
    }
}
