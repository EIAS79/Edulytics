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
            var expectedVersion=n switch
            {
                20=>CommonCoreG1FullGradeStartingContentCorrections.ZooSourceContentVersion,
                21=>CommonCoreG1FullGradeStartingContentCorrections.CenterSourceContentVersion,
                22=>CommonCoreG1FullGradeStartingContentCorrections.TeenSubtractSourceContentVersion,
                23=>CommonCoreG1FullGradeStartingContentCorrections.TenSubtractSourceContentVersion,
                24=>CommonCoreG1FullGradeStartingContentCorrections.CountRelationSourceContentVersion,
                25=>CommonCoreG1FullGradeStartingContentCorrections.ChooseSubtractSourceContentVersion,
                26=>CommonCoreG1FullGradeStartingContentCorrections.StorySourceContentVersion,
                27=>CommonCoreG1FullGradeStartingContentCorrections.CenterFourSourceContentVersion,
                _=>CommonCoreG1FullGradeStartingContentCorrections.Version
            };
            Assert.Equal(expectedVersion,
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
        Assert.Contains("ORIGINAL Set A: fill 12-7=□, 18-5=□",text(24));
        Assert.Contains("ORIGINAL Set B: fill 5+□=18, 13=□+6, 20=15+□",text(24));
        Assert.Contains("8 snakes, 7 iguanas and 5 frogs",text(20));
        Assert.Contains("3 herons, 6 hawks and 7 hummingbirds",text(20));
        Assert.Contains("8+2+4=14 and 8+6=14",text(20));
        Assert.Contains("7+3+6=16 and 7+9=16",text(20));
        Assert.Contains("8+2+5+5=20",text(20));
        Assert.Contains("8 lions, 4 tigers and 3 cheetahs",text(20));
        Assert.Contains("8 goats, 7 sheep and 4 pigs",text(20));
        Assert.Contains("4+8+6=4+6+8=10+8=18",text(20));
        Assert.Contains("4+8=4+6+2=10+2=12",text(20));
        Assert.Contains("four red and six yellow counters",text(20));
        Assert.Contains("not the original publisher artwork",text(20));
        Assert.Contains("8 monkeys, 4 gorillas, and 7 orangutans",text(20));
        Assert.Contains("8+4+7=8+2+2+7=10+9=19 primates",text(20));
        Assert.Contains("2021 login-protected cool-down",text(20));
        Assert.Contains("digital equation check verifies arithmetic",text(20));
        Assert.Contains("What do you know about 20?",text(21));
        Assert.Contains("10+10=13+7=20",text(21));
        Assert.Contains("14-4=10 helps solve 14-5=9",text(22));
        Assert.Contains("Elena has 16 crayons",text(22));
        Assert.Contains("16-7=9 crayons",text(22));
        Assert.Contains("17-7-1=9",text(23));
        Assert.Contains("Diego's and Andre's different ten-frame methods",text(23));
        Assert.Contains("14-5=9 and 13-6=7",text(23));
        var zoo=doc.Lessons.Single(x=>x.LessonCode=="PED:US-CCSS-MATH:G1:U03:L20");
        Assert.True(CommonCoreG1FullGradeStartingContentCorrections.CanUpgradeExisting(
            doc,zoo,CommonCoreG1FullGradeStartingContentCorrections.Version));
        Assert.True(CommonCoreG1FullGradeStartingContentCorrections.CanUpgradeExisting(
            doc,zoo,"us-ccss-g1-u03-l20-zoo-2021-source-v6"));
        Assert.True(CommonCoreG1FullGradeStartingContentCorrections.CanUpgradeExisting(
            doc,zoo,"us-ccss-g1-u03-l20-zoo-2021-source-v7"));
        var center=doc.Lessons.Single(x=>x.LessonCode=="PED:US-CCSS-MATH:G1:U03:L21");
        Assert.True(CommonCoreG1FullGradeStartingContentCorrections.CanUpgradeExisting(
            doc,center,"us-ccss-g1-u03-l21-centers-2021-source-v6"));
        Assert.Equal("us-ccss-g1-u03-l21-centers-2021-source-v7",
            CommonCoreG1FullGradeStartingContentCorrections.CenterSourceContentVersion);
        Assert.Equal("us-ccss-g1-u03-l20-zoo-2021-source-v8",
            CommonCoreG1FullGradeStartingContentCorrections.ZooSourceContentVersion);
        foreach(var number in new[]{21,22,23,24,25,26,27})
        {
            var lesson=doc.Lessons.Single(x=>x.LessonCode==$"PED:US-CCSS-MATH:G1:U03:L{number:00}");
            Assert.True(CommonCoreG1FullGradeStartingContentCorrections.CanUpgradeExisting(
                doc,lesson,CommonCoreG1FullGradeStartingContentCorrections.Version));
        }
        var flexible=doc.Lessons.Single(x=>x.LessonCode=="PED:US-CCSS-MATH:G1:U03:L19");
        Assert.False(CommonCoreG1FullGradeStartingContentCorrections.CanUpgradeExisting(
            doc,flexible,CommonCoreG1FullGradeStartingContentCorrections.Version));
        Assert.Contains("display five numeral cards",text(21));
        Assert.Contains("number-card set is 0 through 10",text(21));
        Assert.Contains("permitted cards are 0 through 9",text(21));
        Assert.Contains("no 11 card",text(21));
        Assert.Contains("choose to add 7, 8 or 9",text(21));
        Assert.Contains("five markers consecutively in a row",text(21));
        Assert.Contains("12-7=5 pencils",text(25));
        Assert.Contains("8+7=15",text(25));
        Assert.Contains("17-8=9",text(25));
        Assert.Contains("20-15=5",text(25));
        Assert.Contains("7+5=12",text(26));
        Assert.Contains("BACKWARD by one from 50 to 20",text(24));
        Assert.Contains("16-3=13, 17-8=9 and 18-15=3",text(24));
        Assert.Contains("18-5=□",text(24));
        Assert.Contains("20=15+□",text(24));
        Assert.Contains("FORWARD from 50 through 100",text(25));
        Assert.Contains("19-3=16, 13-5=8, 18-9=9 and 17-15=2",text(25));
        Assert.Contains("Clare 8+□=15",text(25));
        Assert.Contains("Kiran 17-□=9",text(25));
        Assert.Contains("15-10=5, 15-12=3, 16-10=6, 16-13=3",text(26));
        Assert.Contains("Han story 1: □+9=15",text(26));
        Assert.Contains("Han story 2: 15-□=9",text(26));
        Assert.Contains("Han story 3: 9+□=15",text(26));
        Assert.Contains("Han story 4: 15-9=6",text(26));
        Assert.Contains("20-10-1=9, 18-10-4=4",text(27));
        Assert.Contains("9+6=15 is greater than 19-6=13",text(27));
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
            Assert.Equal(CommonCoreG1FullGradeStartingContentCorrections.UnitFourLaterSourceContentVersion,
                CanonicalLessonContentMaterializer.GetEffectiveContentVersion(doc,row));
            Assert.True(CommonCoreG1FullGradeStartingContentCorrections.CanUpgradeExisting(
                doc,row,CommonCoreG1FullGradeStartingContentCorrections.Version));
            var tr=row.Translations.Single(x=>x.CultureCode=="en");
            return tr.Explanation+"\n"+tr.KeyConceptsAndRules+"\n"+tr.WorkedExamples+"\n"+tr.StepByStepSolutions+"\n"+tr.CommonMistakes;
        }
        string earlierUnit4Text(int n)
        {
            var row=doc.Lessons.Single(x=>x.LessonCode==$"PED:US-CCSS-MATH:G1:U04:L{n:00}");
            Assert.True(CommonCoreG1FullGradeStartingContentCorrections.IsTarget(doc,row));
            var expectedVersion=n switch
            {
                1=>CommonCoreG1FullGradeStartingContentCorrections.UnitFourCollectionSourceContentVersion,
                2=>CommonCoreG1FullGradeStartingContentCorrections.UnitFourTensMatchSourceContentVersion,
                4=>CommonCoreG1FullGradeStartingContentCorrections.UnitFourMultipleTensSourceContentVersion,
                5=>CommonCoreG1FullGradeStartingContentCorrections.UnitFourCenterSourceContentVersion,
                _=>throw new InvalidOperationException()
            };
            Assert.Equal(expectedVersion,CanonicalLessonContentMaterializer.GetEffectiveContentVersion(doc,row));
            Assert.True(CommonCoreG1FullGradeStartingContentCorrections.CanUpgradeExisting(
                doc,row,CommonCoreG1FullGradeStartingContentCorrections.Version));
            var tr=row.Translations.Single(x=>x.CultureCode=="en");
            return tr.Explanation+"\n"+tr.KeyConceptsAndRules+"\n"+tr.WorkedExamples+"\n"+
                tr.StepByStepSolutions+"\n"+tr.CommonMistakes+"\n"+tr.QuickSummary;
        }
        Assert.Contains("physical collection of 40 objects",earlierUnit4Text(1));
        Assert.Contains("ten-cube towers",earlierUnit4Text(2));
        Assert.Contains("too-low, about-right, and too-high estimate",earlierUnit4Text(2));
        Assert.Contains("30+10=40, 40+10=50, 80-10=70 and 70-10=60",earlierUnit4Text(4));
        Assert.Contains("Jada counts 30 cubes",earlierUnit4Text(4));
        Assert.Contains("7 towers of ten in Bag C",earlierUnit4Text(4));
        Assert.Contains("90-70=20",earlierUnit4Text(4));
        Assert.Contains("Check It Off Stage 2",earlierUnit4Text(5));
        Assert.DoesNotContain("Check It Off Stage 3",earlierUnit4Text(5));
        Assert.Contains("90 is already marked",earlierUnit4Text(5));
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
        Assert.Contains("Noah",unit4Text(6));
        Assert.Contains("40+8=48",unit4Text(6));
        Assert.Contains("52 objects",unit4Text(6));
        Assert.Contains("Clare/Diego",unit4Text(7));
        Assert.Contains("EIGHT tens and SIX ones",unit4Text(7));
        Assert.Contains("73",unit4Text(8));
        Assert.Contains("too-low, about-right and too-high estimate",unit4Text(8));
        Assert.Contains("AT LEAST FOUR real collections",unit4Text(9));
        Assert.Contains("1 ten and 4 ones=14",unit4Text(10));
        Assert.Contains("2 ones and 8 tens=82",unit4Text(10));
        Assert.Contains("Write Numbers Stage 1 center",unit4Text(10));
        Assert.Contains("37+20=57",unit4Text(11));
        Assert.Contains("30+65=95",unit4Text(11));
        Assert.Contains("smudged diagram",unit4Text(11));
        Assert.Contains("67+10=77",unit4Text(12));
        Assert.Contains("99-10-10-10=69",unit4Text(12));
        Assert.Contains("Write Numbers Stage 2 center",unit4Text(12));
        Assert.Contains("8+7+5=20",unit4Text(13));
        Assert.Contains("Elena argues 75>65",unit4Text(14));
        Assert.Contains("39<41",unit4Text(14));
        Assert.Contains("17<27",unit4Text(15));
        Assert.Contains("58=53 FALSE",unit4Text(15));
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
