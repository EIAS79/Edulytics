using System.Globalization;
using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.LessonContent;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class UsCcssB01Grade3RegroupingVisualsTests
{
    public static TheoryData<string,string> ExactSourceCases => new()
    {
        { "PED:US-CCSS-MATH:G3:U03:L03","usccss.b01.g3.add_within1000" },
        { "PED:US-CCSS-MATH:G3:U03:L05","usccss.b01.g3.add_regroup1000" },
        { "PED:US-CCSS-MATH:G3:U03:L07","usccss.b01.g3.subtract_within1000" },
        { "PED:US-CCSS-MATH:G3:U03:L08","usccss.b01.g3.subtract_regroup1000" },
        { "PED:US-CCSS-MATH:G3:U03:L09","usccss.b01.g3.subtract_regroup1000" },
        { "PED:US-CCSS-MATH:G3:U03:L10","usccss.b01.g3.subtract_regroup1000" },
        { "PED:US-CCSS-MATH:G3:U03:L11","usccss.b01.g3.subtract_across_zero" },
        { "PED:US-CCSS-MATH:G3:U03:L12","usccss.b01.g3.subtract_within1000" }
    };

    [Theory]
    [MemberData(nameof(ExactSourceCases))]
    public void OriginalThreeDigitSourceScopeProducesIndependentlyCheckableExamples(
        string code,string family)
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(code,out var contract));
        Assert.Equal([family],contract!.AllowedQuestionFamilies);
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var difficulty in new[]{
            ExactSkillQuestionDifficulty.Standard,
            ExactSkillQuestionDifficulty.Stretch,
            ExactSkillQuestionDifficulty.Challenge})
        for(var seed=0;seed<6;seed++)
        {
            var question=Assert.Single(engine.Generate(
                "g3-u03-source-regrouping",code,[family],difficulty,1,
                221000+seed,[]));
            var p=question.Parameters;
            int left=p["left"], right=p["right"], operation=p["operation"];
            int actual=operation==0?left+right:left-right;
            Assert.InRange(left,100,999);
            Assert.InRange(right,100,999);
            Assert.InRange(actual,0,999);
            Assert.Equal(actual,int.Parse(question.CorrectAnswer,CultureInfo.InvariantCulture));
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                family,p,question.CorrectAnswer));
            Assert.False(ExactSkillContractQuestionEngine.Verify(
                family,p,(actual+1001).ToString(CultureInfo.InvariantCulture)));
            if(family=="usccss.b01.g3.add_regroup1000")
                Assert.True(left%10+right%10>=10 || left/10%10+right/10%10+
                    (left%10+right%10>=10?1:0)>=10);
            if(family=="usccss.b01.g3.subtract_regroup1000")
                Assert.True(left%10<right%10 ||
                    left/10%10<right/10%10+(left%10<right%10?1:0));
            if(family=="usccss.b01.g3.subtract_across_zero")
                Assert.Equal(0,left%100);
            var chart=Assert.Single(UsCcssB01Grade3RegroupingVisuals.Build(code,[question]));
            Assert.Equal(RichLessonVisualKind.PlaceValueChart,chart.Kind);
            Assert.Equal(3,chart.Rows.Count);
            Assert.Equal(new[]{left,right,actual},chart.Rows.Select(row=>{
                var digits=row.Cells.Select(int.Parse).ToArray();
                Assert.Equal(3,digits.Length);
                return digits[0]*100+digits[1]*10+digits[2];
            }).ToArray());
            if(family=="usccss.b01.g3.subtract_across_zero")
                Assert.Contains("zero",chart.Description,StringComparison.OrdinalIgnoreCase);
            Assert.Empty(UsCcssB01Grade3RegroupingVisuals.Build(code,
                [question with {CorrectAnswer="99999"}]));
        }
    }

    [Fact]
    public void CannotLeakSourceRegroupingChartsIntoOtherCourses()
    {
        Assert.Empty(UsCcssB01Grade3RegroupingVisuals.Build(
            "PED:US-CCSS-MATH:G4:U03:L11",[]));
        Assert.Empty(UsCcssB01Grade3RegroupingVisuals.Build(
            "PED:CAMBRIDGE-INTL-MATH:G3:U03:L11",[]));
    }
}
