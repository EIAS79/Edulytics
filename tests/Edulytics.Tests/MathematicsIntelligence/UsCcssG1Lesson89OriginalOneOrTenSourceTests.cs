using Edulytics.Core.Curriculum;
using Edulytics.Data.Seeding;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Original 2021 Unit 5 L01 source statements, authored without running Stage D.</summary>
public sealed class UsCcssG1Lesson89OriginalOneOrTenSourceTests
{
    private const string Family="usccss.g1.u05.l01.tens_or_ones_no_compose";

    [Fact]
    public void PriyaAndMaiTeacherOriginalNumbersKeepTensAndOnesDistinct()
    {
        foreach(var (mode,a,b,expected,wrong) in new[]{
            (13,34,5,"NEW_TEN=NO;TOTAL=39","NEW_TEN=NO;TOTAL=84"),
            (14,34,50,"NEW_TEN=NO;TOTAL=84","NEW_TEN=NO;TOTAL=39"),
            (15,4,45,"NEW_TEN=NO;TOTAL=49","NEW_TEN=NO;TOTAL=85")
        })
        {
            var p=new Dictionary<string,int>{{"a",a},{"b",b},{"mode",mode}};
            Assert.True(UsCcssG1FullGradeUnit5AdditionQuestions.Valid(Family,p));
            Assert.Equal(expected,UsCcssG1FullGradeUnit5AdditionQuestions.Solve(Family,p));
            Assert.True(UsCcssG1FullGradeUnit5AdditionQuestions.Verify(Family,p,expected));
            Assert.False(UsCcssG1FullGradeUnit5AdditionQuestions.Verify(Family,p,wrong));
            Assert.False(UsCcssG1FullGradeUnit5AdditionQuestions.Verify(Family,p,expected+";EXTRA=1"));
            p["a"]=a+1;
            Assert.False(UsCcssG1FullGradeUnit5AdditionQuestions.Valid(Family,p));
        }
    }

    [Fact]
    public void AuthoringRetainsAllNineFixed2021StudentEquationModes()
    {
        var problems=Enumerable.Range(0,1600)
            .Select(i=>UsCcssG1FullGradeUnit5AdditionQuestions.Build(Family,new Random(i*41+7))).ToArray();
        foreach(int m in Enumerable.Range(4,12))
            Assert.Contains(problems,p=>p.Parameters["mode"]==m);
        Assert.All(problems,p=>Assert.True(UsCcssG1FullGradeUnit5AdditionQuestions.Verify(
            Family,p.Parameters,UsCcssG1FullGradeUnit5AdditionQuestions.Solve(Family,p.Parameters))));
    }

    [Fact]
    public void RichTeachingIncludesThreeOriginalTenFrameCountsAndStrategyWarning()
    {
        var doc=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .Single(x=>x.Lessons.Any(l=>l.LessonCode=="PED:US-CCSS-MATH:G1:U05:L01"));
        CanonicalLessonContentMaterializer.Materialize(doc);
        var lesson=doc.Lessons.Single(x=>x.LessonCode=="PED:US-CCSS-MATH:G1:U05:L01");
        Assert.Equal(CommonCoreG1FullGradeUnit5ContentCorrections.Unit5Lesson01SourceVersion,
            CanonicalLessonContentMaterializer.GetEffectiveContentVersion(doc,lesson));
        Assert.True(CommonCoreG1FullGradeUnit5ContentCorrections.CanUpgradeExisting(
            doc,lesson,CommonCoreG1FullGradeUnit5ContentCorrections.Unit5EarlySourceVersion));
        var en=lesson.Translations.Single(x=>x.CultureCode=="en");
        foreach(var phrase in new[]{"three complete ten-frames (30)","five ones (35)","nine ones (39)",
            "43+50=93","34+5=39","34+50=84","4+45=49, not 85"})
            Assert.Contains(phrase,en.Explanation+" "+en.WorkedExamples);
    }
}
