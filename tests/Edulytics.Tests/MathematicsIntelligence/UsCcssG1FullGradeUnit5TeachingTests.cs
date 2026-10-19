using Edulytics.Core.Curriculum;
using Edulytics.Data.Seeding;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>The existing source-corrected L86 is reused, not overwritten.
/// All other targeted U05 teaching examples must survive actual materialization.
/// This checks teaching only, not official original student-task parity.</summary>
public sealed class UsCcssG1FullGradeUnit5TeachingTests
{
    [Fact]
    public void Unit5Teaching_Has16SourceShaAndOutcomeGuardedTargets()
    {
        var docs=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments();
        var targets=docs.SelectMany(doc=>doc.Lessons.Where(l=>
            CommonCoreG1FullGradeUnit5ContentCorrections.IsTarget(doc,l)))
            .Select(l=>l.LessonCode).ToArray();
        Assert.Equal(16,targets.Length);
        Assert.Equal(16,targets.Distinct(StringComparer.Ordinal).Count());
        Assert.DoesNotContain("PED:US-CCSS-MATH:G1:U04:L21",targets);
        for(var n=1;n<=14;n++)
            Assert.Contains($"PED:US-CCSS-MATH:G1:U05:L{n:00}",targets);
    }

    [Fact]
    public void SourceBoundTeaching_CorrectlySeparatesNoCarryAndComposingTen()
    {
        var doc=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .Single(d=>d.Lessons.Any(l=>l.LessonCode=="PED:US-CCSS-MATH:G1:U05:L01"));
        CanonicalLessonContentMaterializer.Materialize(doc);
        string lesson(int unit,int lesson)
        {
            var code=$"PED:US-CCSS-MATH:G1:U{unit:00}:L{lesson:00}";
            var row=doc.Lessons.Single(x=>x.LessonCode==code);
            Assert.True(CommonCoreG1FullGradeUnit5ContentCorrections.IsTarget(doc,row));
            Assert.Equal(CommonCoreG1FullGradeUnit5ContentCorrections.Version,
                CanonicalLessonContentMaterializer.GetEffectiveContentVersion(doc,row));
            var t=row.Translations.Single(x=>x.CultureCode=="en");
            return t.Explanation+"\n"+t.WorkedExamples+"\n"+t.StepByStepSolutions+"\n"+t.CommonMistakes;
        }
        Assert.Contains("47 + 8 = 47 + 3 + 5 = 50 + 5 = 55",lesson(5,5));
        Assert.Contains("23 + 45 = (20 + 40) + (3 + 5) = 60 + 8 = 68",lesson(5,2));
        Assert.Contains("37+26 = (30+20)+(7+6) = 50+13=63",lesson(5,10));
        Assert.Contains("74 is greater than 68",lesson(4,22));
        Assert.Contains("Many correct options are possible",lesson(5,7));
        Assert.Contains("32+8=40 and 32+9=41",lesson(5,7));
        Assert.Contains("8+12=20, 8+35=43 and 8+49=57",lesson(5,7));
        Assert.Contains("Elena begins with 34+6=40",lesson(5,6));
        Assert.Contains("34+9=9+1+33=10+33=43",lesson(5,6));
    }
}
