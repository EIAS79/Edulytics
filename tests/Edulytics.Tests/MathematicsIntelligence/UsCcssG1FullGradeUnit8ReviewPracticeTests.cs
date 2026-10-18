using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UsCcssG1FullGradeUnit8ReviewPracticeTests
{
    [Fact]
    public void Unit8HasTenRegisteredSourceSpecificDigitalFamilies()
    {
        Assert.Equal(10,UsCcssG1FullGradeUnit8ReviewQuestions.Families.Count);
        for(int n=1;n<=10;n++)
        {
            var lesson=LessonPracticeContractRegistry.All.Single(x=>
                x.LessonCode==$"PED:US-CCSS-MATH:G1:U08:L{n:00}");
            var f=Assert.Single(lesson.AllowedQuestionFamilies);
            Assert.True(UsCcssG1FullGradeUnit8ReviewQuestions.Supports(f));
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(f,out var choice));
            Assert.Equal(LessonPracticeInteractionKind.ContractDriven,choice!.InteractionKind);
        }
    }
    [Fact]
    public void HundredsOfU08GeneratedInstancesPassCorrectAndRejectWrongAnswers()
    {
        foreach(var f in UsCcssG1FullGradeUnit8ReviewQuestions.Families)
            for(int seed=0;seed<130;seed++)
            {
                var q=UsCcssG1FullGradeUnit8ReviewQuestions.Build(f,new Random(seed*37+13));
                var answer=UsCcssG1FullGradeUnit8ReviewQuestions.Solve(f,q.Parameters);
                Assert.True(UsCcssG1FullGradeUnit8ReviewQuestions.Valid(f,q.Parameters));
                Assert.True(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,answer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,"RESULT=999"));
                Assert.False(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,answer+";FAKE=1"));
            }
    }
    [Fact]
    public void AStudentCreatedRiddleIsNotInferredFromACorrectNumericClue()
    {
        var p=new Dictionary<string,int>{{"a",47},{"b",10},{"c",0},{"mode",0}};
        const string f="usccss.g1.u08.l10.write_riddle_numeric_check";
        Assert.Equal("NUMBER=47;VALID=YES",UsCcssG1FullGradeUnit8ReviewQuestions.Solve(f,p));
        Assert.False(UsCcssG1FullGradeUnit8ReviewQuestions.Verify(f,p,"NUMBER=47;VALID=NO"));
    }
    [Fact]
    public void TenDistinctTasksAreAvailableForEveryU08DigitalFamily()
    {
        var e=new ExactSkillContractQuestionEngine();
        foreach(var family in UsCcssG1FullGradeUnit8ReviewQuestions.Families)
        {
            var rows=e.Generate("g1-u08",family,[family],ExactSkillQuestionDifficulty.Standard,10,20261019,[]);
            Assert.Equal(10,rows.Count);
            Assert.Equal(10,rows.Select(x=>x.Prompt).Distinct(StringComparer.Ordinal).Count());
        }
    }
}
