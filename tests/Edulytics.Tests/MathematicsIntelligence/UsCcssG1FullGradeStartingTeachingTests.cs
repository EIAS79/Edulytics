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
        Assert.Contains("WRITE THEIR OWN bird story with 9 birds, 8 birds, and 1 bird",text(15));
        Assert.Contains("teacher reviews",text(15),StringComparison.OrdinalIgnoreCase);
        Assert.Contains("6 + 5 + 4 = 15 + 10 is FALSE",text(16));
        Assert.Contains("8 + 6 = 10 + 4 = 14",text(17));
        Assert.Contains("9 + 4 = 8 + 5 = 7 + 6 = 13",text(18));
        Assert.Contains("7+8=7+7+1=14+1=15",text(19));
        Assert.Contains("5+3+4=12",text(20));
        Assert.Contains("3+6+7=3+7+6=10+6=16",text(21));
        Assert.Contains("14-6=14-4-2=8",text(22));
        Assert.Contains("13-6=13-3-3=10-3=7",text(23));
        Assert.Contains("8 to 10 is 2",text(24));
        Assert.Contains("How Close? Subtract from 20, Stage 2",text(27));
        Assert.Contains("selects TWO or THREE cards",text(27));
        Assert.Contains("ORIGINAL Set A: find missing values",text(24));
        Assert.Contains("ORIGINAL Set B: find missing addends",text(24));
        Assert.Contains("8 snakes, 7 iguanas and 5 frogs",text(20));
        Assert.Contains("3 herons, 6 hawks and 7 hummingbirds",text(20));
        Assert.Contains("8+2+4=14 and 8+6=14",text(20));
        Assert.Contains("7+3+6=16 and 7+9=16",text(20));
        Assert.Contains("8+2+5+5=20",text(20));
        Assert.Contains("8 lions, 4 tigers and 3 cheetahs",text(20));
        Assert.Contains("8 goats, 7 sheep and 4 pigs",text(20));
        Assert.Contains("digital equation check verifies arithmetic",text(20));
        Assert.Contains("display five numeral cards",text(21));
        Assert.Contains("choose to add 7, 8 or 9",text(21));
        Assert.Contains("five markers consecutively in a row",text(21));
        Assert.Contains("12-7=5 pencils",text(25));
        Assert.Contains("8+7=15",text(25));
        Assert.Contains("17-8=9",text(25));
        Assert.Contains("20-15=5",text(25));
        Assert.Contains("7+5=12",text(26));
        Assert.Contains("Elena has 6 counters",text(26));
        Assert.Contains("16-9=7",text(27));
        var preexistingStory=doc.Lessons.Single(x=>x.LessonCode=="PED:US-CCSS-MATH:G1:U03:L28");
        Assert.False(CommonCoreG1FullGradeStartingContentCorrections.IsTarget(doc,preexistingStory));
        Assert.True(CommonCoreG1SourceGroundedInstructionalCorrections.IsTarget(doc,preexistingStory));
        Assert.Contains("4 + 3 + 5 = 12",preexistingStory.Translations.Single(x=>x.CultureCode=="en").WorkedExamples);
        var preexistingTens=doc.Lessons.Single(x=>x.LessonCode=="PED:US-CCSS-MATH:G1:U04:L03");
        Assert.False(CommonCoreG1FullGradeStartingContentCorrections.IsTarget(doc,preexistingTens));
        Assert.True(CommonCoreG1SourceGroundedInstructionalCorrections.IsTarget(doc,preexistingTens));

        string unit4Text(int n)
        {
            var row=doc.Lessons.Single(x=>x.LessonCode==$"PED:US-CCSS-MATH:G1:U04:L{n:00}");
            Assert.True(CommonCoreG1FullGradeStartingContentCorrections.IsTarget(doc,row));
            Assert.Equal(CommonCoreG1FullGradeStartingContentCorrections.Version,
                CanonicalLessonContentMaterializer.GetEffectiveContentVersion(doc,row));
            var tr=row.Translations.Single(x=>x.CultureCode=="en");
            return tr.Explanation+"\n"+tr.KeyConceptsAndRules+"\n"+tr.WorkedExamples+"\n"+tr.StepByStepSolutions+"\n"+tr.CommonMistakes;
        }
        Assert.Contains("30+7=37",unit4Text(6));
        Assert.Contains("fifty-eight",unit4Text(7));
        Assert.Contains("50+3 = 53",unit4Text(8));
        Assert.Contains("60+4",unit4Text(9));
        Assert.Contains("30+7",unit4Text(10));
        Assert.Contains("34+20=54",unit4Text(11));
        Assert.Contains("Ten more than 58 is 68",unit4Text(12));
        Assert.Contains("43+20=63",unit4Text(13));
        Assert.Contains("64 is GREATER THAN 57",unit4Text(14));
        Assert.Contains("52 > 39",unit4Text(15));
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
        Assert.Equal(28,targets.Length);
        Assert.Equal(28,targets.Distinct(StringComparer.Ordinal).Count());
        Assert.Equal(new[]{14,15,16,17,18,19,20,21,22,23,24,25,26,27}.Select(n=>$"PED:US-CCSS-MATH:G1:U03:L{n:00}").Concat(new[]{1,2,4,5,6,7,8,9,10,11,12,13,14,15}.Select(n=>$"PED:US-CCSS-MATH:G1:U04:L{n:00}")),targets);
    }
}
