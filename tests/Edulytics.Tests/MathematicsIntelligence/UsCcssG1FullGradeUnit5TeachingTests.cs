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
            Assert.Equal(unit==4
                    ?(lesson==22?CommonCoreG1FullGradeUnit5ContentCorrections.Unit4Lesson22SourceVersion
                                 :CommonCoreG1FullGradeUnit5ContentCorrections.Unit4EndSourceVersion)
                    :lesson==1?CommonCoreG1FullGradeUnit5ContentCorrections.Unit5Lesson01SourceVersion
                    :lesson<=7?CommonCoreG1FullGradeUnit5ContentCorrections.Unit5EarlySourceVersion
                    :lesson<=13?CommonCoreG1FullGradeUnit5ContentCorrections.Unit5LaterSourceVersion
                    :CommonCoreG1FullGradeUnit5ContentCorrections.Version,
                CanonicalLessonContentMaterializer.GetEffectiveContentVersion(doc,row));
            var t=row.Translations.Single(x=>x.CultureCode=="en");
            return t.Explanation+"\n"+t.WorkedExamples+"\n"+t.StepByStepSolutions+"\n"+t.CommonMistakes;
        }
        Assert.Contains("Original 8+47=47+3+5=55",lesson(5,5));
        Assert.Contains("37+7=44 dollars",lesson(5,5));
        Assert.Contains("43+50=93",lesson(5,1));
        Assert.Contains("93+6=99",lesson(5,1));
        Assert.Contains("23+45",lesson(5,2));
        Assert.Contains("63+25=88",lesson(5,2));
        Assert.Contains("Five in a Row Stage 5",lesson(5,2));
        Assert.Contains("32+54=86",lesson(5,3));
        Assert.Contains("23+74=97",lesson(5,3));
        Assert.Contains("Number Puzzles Stage 3",lesson(5,4));
        Assert.Contains("Kiran has 14 marbles",lesson(5,4));
        Assert.Contains("45+□=50",lesson(5,6));
        Assert.Contains("Target Numbers",lesson(5,6));
        Assert.Contains("26+3 DOES NOT",lesson(5,7));
        Assert.Contains("Target Numbers Stage 2",lesson(5,8));
        Assert.Contains("STARTS AT 25",lesson(5,8));
        Assert.Contains("35+6=41 points",lesson(5,8));
        Assert.Contains("17+36=53",lesson(5,9));
        Assert.Contains("Grab and Add",lesson(5,9));
        Assert.Contains("28+56",lesson(5,10));
        Assert.Contains("27+44",lesson(5,10));
        Assert.Contains("Number Puzzles Stage 4",lesson(5,10));
        Assert.Contains("37+25",lesson(5,11));
        Assert.Contains("23+68=91",lesson(5,11));
        Assert.Contains("48+15=63",lesson(5,12));
        Assert.Contains("57+36=93",lesson(5,12));
        Assert.Contains("22+18=40",lesson(5,12));
        Assert.Contains("Target Numbers Stage 3",lesson(5,13));
        Assert.Contains("Five in a Row Stage 6",lesson(5,13));
        Assert.Contains("23 + 45 = (20 + 40) + (3 + 5) = 60 + 8 = 68",lesson(5,2));
        Assert.Contains("37+26=(30+20)+(7+6)=50+13=63",lesson(5,10));
        Assert.Contains("74 is greater than 68",lesson(4,22));
        Assert.Contains("Mystery Number",lesson(4,22));
        Assert.Contains("90+2>80+12 FALSE",lesson(4,22));
        Assert.Contains("THREE two-handful",lesson(4,23));
        Assert.Contains("LESS THAN 20, 20 TO 50 or MORE THAN 50",lesson(4,23));
        Assert.Contains("Source requires thinking, not a single preset answer",lesson(5,7));
        Assert.Contains("32+8=40 and 32+9=41",lesson(5,7));
        Assert.Contains("8+12=20, 8+35=43 and 8+49=57",lesson(5,7));
        Assert.Contains("Elena begins with 34+6=40",lesson(5,6));
        Assert.Contains("34+9=9+1+33=10+33=43",lesson(5,6));
    }
    [Fact]
    public void Unit5Lesson14_FoodDrivePreservesBothPartnerTasksAndOriginalMinimumBoxesChallenge()
    {
        const string code = "PED:US-CCSS-MATH:G1:U05:L14";
        var doc = MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .Single(d => d.Lessons.Any(l => l.LessonCode == code));
        CanonicalLessonContentMaterializer.Materialize(doc);
        var lesson = doc.Lessons.Single(l => l.LessonCode == code);
        Assert.True(CommonCoreG1FullGradeUnit5ContentCorrections.IsTarget(doc, lesson));
        Assert.Equal(CommonCoreG1FullGradeUnit5ContentCorrections.Version,
            CanonicalLessonContentMaterializer.GetEffectiveContentVersion(doc, lesson));

        var en = lesson.Translations.Single(t => t.CultureCode == "en");
        var visible = en.Explanation + "\n" + en.WorkedExamples + "\n" +
                      en.StepByStepSolutions + "\n" + en.CommonMistakes;
        Assert.Contains("Lin 18", visible);
        Assert.Contains("Priya 24", visible);
        Assert.Contains("Han 13", visible);
        Assert.Contains("Tyler 30", visible);
        Assert.Contains("Tyler 30 + Priya 24 = 54", visible);
        Assert.Contains("Lin 18 + Han 13 = 31", visible);
        Assert.Contains("42", visible);
        Assert.Contains("43", visible);
        Assert.Contains("85", visible);
        Assert.Contains("LEAST NUMBER OF BOXES", visible);
        Assert.Contains("18+51+23+13+39+40+8+29+30=251", visible);
        Assert.Contains("three-grade box", visible);
        Assert.Contains("strict two-grades-per-box", visible);

        // Independent source-quantity oracle for all NINE original Day 1 grade rows.
        // Four bins achieve the lower bound only if the optional three-grade task is allowed.
        int[] cans = [18, 51, 23, 13, 39, 40, 8, 29, 30];
        int[][] boxGradeIndices = [[0, 4, 6], [1, 3], [2, 5], [7, 8]];
        int total = cans.Sum();
        Assert.Equal(251, total);
        Assert.Equal(4, (total + 64) / 65);
        Assert.Equal(4, boxGradeIndices.Length);
        Assert.Equal(9, boxGradeIndices.SelectMany(box => box).Distinct().Count());
        Assert.Equal(9, boxGradeIndices.Sum(box => box.Length));
        Assert.All(boxGradeIndices, box =>
        {
            int sum = box.Sum(index => cans[index]);
            Assert.InRange(sum, 35, 65);
        });
        Assert.Equal(total, boxGradeIndices.Sum(box => box.Sum(index => cans[index])));

        // The prior real learner version is eligible for this lesson-only content upgrade.
        Assert.True(CommonCoreG1FullGradeUnit5ContentCorrections.CanUpgradeExisting(
            doc, lesson, "us-ccss-g1-full-grade-u04-l22-through-u05-l14-teaching-v1"));
    }

}
