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
    [Theory]
    [InlineData(1)]
    [InlineData(47)]
    [InlineData(99)]
    [InlineData(100)]
    [InlineData(112)]
    [InlineData(120)]
    public void Original2021RiddleRangeOneTo120AndFourCluesAreSupported(int target)
    {
        const string f="usccss.g1.u08.l10.write_riddle_numeric_check";
        int b=target/2;
        foreach(int mode in new[]{0,1})
        {
            var p=new Dictionary<string,int>{{"a",target},{"b",b},{"c",0},{"mode",mode}};
            Assert.True(UsCcssG1FullGradeUnit8ReviewQuestions.Valid(f,p));
            var answer=UsCcssG1FullGradeUnit8ReviewQuestions.Solve(f,p);
            Assert.Equal(mode==0?$"NUMBER={target};VALID=YES":$"NUMBER={target};VALID=NO",answer);
            Assert.True(UsCcssG1FullGradeUnit8ReviewQuestions.Verify(f,p,answer));
            Assert.False(UsCcssG1FullGradeUnit8ReviewQuestions.Verify(f,p,$"NUMBER={target};VALID=MAYBE"));
            var prompt=UsCcssG1FullGradeUnit8ReviewQuestions.Build(f,new Random(target*17+mode)).Prompt;
            Assert.Contains("FOUR-clue",prompt);
            Assert.Contains("1 to 120",prompt);
            Assert.Contains("addition expression",prompt);
        }
    }
    [Fact]
    public void Original2021RiddleTargetOutsideOneTo120IsInvalid()
    {
        const string f="usccss.g1.u08.l10.write_riddle_numeric_check";
        foreach(int target in new[]{0,121})
        {
            var p=new Dictionary<string,int>{{"a",target},{"b",0},{"c",0},{"mode",0}};
            Assert.False(UsCcssG1FullGradeUnit8ReviewQuestions.Valid(f,p));
        }
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
