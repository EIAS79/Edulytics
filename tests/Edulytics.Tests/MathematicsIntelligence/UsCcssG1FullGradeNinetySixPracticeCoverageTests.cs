using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Full-grade scope test, not an academic release certificate. The old
/// 50 are intentionally not re-run as an acceptance cohort. The 96 new
/// grade lessons comprise 95 machine-gradeable DIGITAL PARTS and one
/// teacher-reviewed authored-story lesson (U03 L28 = ordinal 65).
/// Full lesson source task parity, graphics and real student persistence
/// remain separate mandatory gates.
/// </summary>
public sealed class UsCcssG1FullGradeNinetySixPracticeCoverageTests
{
    [Fact]
    public void AllNewNinetySixLessonsHaveUniqueDigitalContractsOrHonestAuthorshipException()
    {
        var units=new(int Unit,int Start,int End)[]
        {
            new(3,14,28),
            new(4,1,23),
            new(5,1,14),
            new(6,1,17),
            new(7,1,17),
            new(8,1,10)
        };
        int checkedDigital=0;
        var unique=new HashSet<string>(StringComparer.Ordinal);
        foreach(var (unit,start,end) in units)
            for(int n=start;n<=end;n++)
            {
                var code=$"PED:US-CCSS-MATH:G1:U{unit:00}:L{n:00}";
                Assert.True(unique.Add(code),"Duplicate G1 lesson identity: "+code);
                var contract=LessonPracticeContractRegistry.All.Single(x=>x.LessonCode==code);
                if(unit==3&&n==28)
                {
                    // Lesson 65 learner-authored optional stories:
                    // a numeric generated answer is NOT evidence of writing one.
                    Assert.DoesNotContain(contract.AllowedQuestionFamilies,x=>
                        x.StartsWith("usccss.g1.u03.l28.",StringComparison.Ordinal));
                    continue;
                }
                var f=Assert.Single(contract.AllowedQuestionFamilies);
                Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f),code+" | "+f);
                Assert.True(LessonPracticeInteractionRegistry.TryResolve(f,out var shape),f);
                Assert.Equal(LessonPracticeInteractionKind.ContractDriven,shape!.InteractionKind);
                checkedDigital++;
            }
        Assert.Equal(96,unique.Count);
        Assert.Equal(95,checkedDigital);
    }

    [Fact]
    public void EveryDigitalSourceGoalFamilyCanGenerateAndRejectFalseAnswers()
    {
        var gradeNewCodes=new List<string>();
        for(int n=14;n<=28;n++)gradeNewCodes.Add($"PED:US-CCSS-MATH:G1:U03:L{n:00}");
        for(int unit=4;unit<=8;unit++)
        {
            int count=unit switch {4=>23,5=>14,6=>17,7=>17,_=>10};
            for(int n=1;n<=count;n++)
                gradeNewCodes.Add($"PED:US-CCSS-MATH:G1:U{unit:00}:L{n:00}");
        }
        foreach(var code in gradeNewCodes.Where(x=>x!="PED:US-CCSS-MATH:G1:U03:L28"))
        {
            var contract=LessonPracticeContractRegistry.All.Single(x=>x.LessonCode==code);
            var f=Assert.Single(contract.AllowedQuestionFamilies);
            var engine=new ExactSkillContractQuestionEngine();
            var questions=engine.Generate(code,f,[f],
                ExactSkillQuestionDifficulty.Standard,10,20261019,[]);
            Assert.Equal(10,questions.Count);
            Assert.Equal(10,questions.Select(x=>x.Prompt).Distinct(StringComparer.Ordinal).Count());
            foreach(var item in questions)
            {
                Assert.True(ExactSkillContractQuestionEngine.Verify(f,item.Parameters,item.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(f,item.Parameters,
                    item.CorrectAnswer+";UNVERIFIED=1"));
            }
        }
    }
}
