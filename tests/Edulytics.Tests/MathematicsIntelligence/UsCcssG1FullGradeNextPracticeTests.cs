using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class UsCcssG1FullGradeNextPracticeTests
{
    private static readonly string[] Codes =
    [
        "PED:US-CCSS-MATH:G1:U03:L19","PED:US-CCSS-MATH:G1:U03:L20",
        "PED:US-CCSS-MATH:G1:U03:L21","PED:US-CCSS-MATH:G1:U03:L22",
        "PED:US-CCSS-MATH:G1:U03:L23"
    ];
    [Fact]
    public void FiveSourceSpecificLessonsHaveResolvableMathContracts()
    {
        var rows=Codes.Select(x=>LessonPracticeContractRegistry.All.Single(y=>y.LessonCode==x)).ToArray();
        Assert.Equal(5,rows.Length);
        Assert.All(rows,x=>Assert.Single(x.AllowedQuestionFamilies));
        foreach(var row in rows)
            foreach(var family in row.AllowedQuestionFamilies)
            {
                Assert.True(UsCcssG1FullGradeNextQuestions.Supports(family));
                Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
                Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var interaction));
                Assert.Equal(LessonPracticeInteractionKind.ContractDriven,interaction!.InteractionKind);
            }
    }
    [Fact]
    public void GenerateHundredsOfMathematicallyValidProblemsAndRejectWrongAnswers()
    {
        foreach(var family in UsCcssG1FullGradeNextQuestions.Families)
            for(var seed=0;seed<240;seed++)
            {
                var q=UsCcssG1FullGradeNextQuestions.Build(family,new Random(37*seed+9));
                var answer=UsCcssG1FullGradeNextQuestions.Solve(family,q.Parameters);
                Assert.True(UsCcssG1FullGradeNextQuestions.Valid(family,q.Parameters));
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,answer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,"TOTAL=999"));
                Assert.False(UsCcssG1FullGradeNextQuestions.GradePersistedAnswer(family,answer,answer+";EXTRA=1"));
            }
    }
    [Fact]
    public void CrossTenAndAllThreeGroups_AreRequiredInCorrectResponses()
    {
        var p=new Dictionary<string,int>{{"a",14},{"b",6},{"c",0},{"mode",0}};
        Assert.True(UsCcssG1FullGradeNextQuestions.Verify(
            UsCcssG1FullGradeNextQuestions.TeenSubtract,p,"REACH10=4;REST=2;DIFFERENCE=8"));
        Assert.False(UsCcssG1FullGradeNextQuestions.Verify(
            UsCcssG1FullGradeNextQuestions.TeenSubtract,p,"REACH10=4;REST=0;DIFFERENCE=10"));
        var zoo=new Dictionary<string,int>{{"a",3},{"b",5},{"c",7},{"mode",0}};
        Assert.True(UsCcssG1FullGradeNextQuestions.Verify(
            UsCcssG1FullGradeNextQuestions.ZooThree,zoo,"TOTAL=15;EQUATION=3+5+7=15"));
        Assert.False(UsCcssG1FullGradeNextQuestions.Verify(
            UsCcssG1FullGradeNextQuestions.ZooThree,zoo,"TOTAL=8;EQUATION=3+5=8"));
    }
    [Fact]
    public void AllFamiliesGenerateDistinctRealPracticeQuestions()
    {
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var family in UsCcssG1FullGradeNextQuestions.Families)
        {
            var items=engine.Generate("full-grade-g1-56-60",family,[family],
                ExactSkillQuestionDifficulty.Standard,5,20261019,[]);
            Assert.Equal(5,items.Count);
            Assert.Equal(5,items.Select(x=>x.Prompt).Distinct(StringComparer.Ordinal).Count());
            foreach(var item in items)
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,item.Parameters,item.CorrectAnswer));
        }
    }
}
