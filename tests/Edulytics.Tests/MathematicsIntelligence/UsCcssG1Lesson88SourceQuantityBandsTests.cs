using Edulytics.Core.Curriculum;
using Edulytics.Data.Seeding;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Lesson 88: first-party numerical estimates/classifications are not evidence
/// that a pupil handled two real handfuls, sorted publisher photos or estimated before counting.
/// Authored only; Stage D tests unexecuted.
/// </summary>
public sealed class UsCcssG1Lesson88SourceQuantityBandsTests
{
    private const string Family="usccss.g1.u04.l23.estimate_and_count";

    [Fact]
    public void ThreeSourceQuantityCategoriesGradeOnlyCorrectNumbersAndCategories()
    {
        foreach(var (mode,a,category) in new[]{
            (4,10,"LESS_THAN_20"),
            (4,19,"LESS_THAN_20"),
            (5,20,"TWENTY_TO_FIFTY"),
            (5,50,"TWENTY_TO_FIFTY"),
            (6,51,"MORE_THAN_50"),
            (6,99,"MORE_THAN_50")
        })
        {
            var p=new Dictionary<string,int>{{"a",a},{"b",30},{"c",0},{"mode",mode}};
            var correct=$"COUNT={a};CATEGORY={category};TENS={a/10};ONES={a%10}";
            Assert.True(UsCcssG1FullGradeUnit4FinalQuestions.Valid(Family,p));
            Assert.Equal(correct,UsCcssG1FullGradeUnit4FinalQuestions.Solve(Family,p));
            Assert.True(UsCcssG1FullGradeUnit4FinalQuestions.Verify(Family,p,correct));
            Assert.False(UsCcssG1FullGradeUnit4FinalQuestions.Verify(Family,p,
                correct.Replace($"COUNT={a}",$"COUNT={a+1}")));
            Assert.False(UsCcssG1FullGradeUnit4FinalQuestions.Verify(Family,p,
                correct+";EXTRA=1"));
            p["a"]=mode==4?20:mode==5?51:50;
            Assert.False(UsCcssG1FullGradeUnit4FinalQuestions.Valid(Family,p));
        }
    }

    [Fact]
    public void SourceOriginalClassroomRequirementsRemainVisibleInTeaching()
    {
        var doc=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .Single(x=>x.Lessons.Any(l=>l.LessonCode=="PED:US-CCSS-MATH:G1:U04:L23"));
        CanonicalLessonContentMaterializer.Materialize(doc);
        var row=doc.Lessons.Single(x=>x.LessonCode=="PED:US-CCSS-MATH:G1:U04:L23");
        var tr=row.Translations.Single(x=>x.CultureCode=="en");
        var text=tr.Explanation+" "+tr.StepByStepSolutions;
        foreach(var required in new[]{"THREE two-handful", "LESS THAN 20", "20 TO 50",
            "MORE THAN 50","teacher"})
            Assert.Contains(required,text,StringComparison.OrdinalIgnoreCase);
    }
}
