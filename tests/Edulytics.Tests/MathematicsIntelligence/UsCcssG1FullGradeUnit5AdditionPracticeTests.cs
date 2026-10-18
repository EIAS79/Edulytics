using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UsCcssG1FullGradeUnit5AdditionPracticeTests
{
    [Fact]
    public void WholeUnit5HasFourteenDistinctSourceGoalFamiliesAndContracts()
    {
        Assert.Equal(14,UsCcssG1FullGradeUnit5AdditionQuestions.Families.Count);
        for(int n=1;n<=14;n++)
        {
            var code=$"PED:US-CCSS-MATH:G1:U05:L{n:00}";
            var contract=LessonPracticeContractRegistry.All.Single(x=>x.LessonCode==code);
            var family=Assert.Single(contract.AllowedQuestionFamilies);
            Assert.True(UsCcssG1FullGradeUnit5AdditionQuestions.Supports(family));
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var interaction));
            Assert.Equal(LessonPracticeInteractionKind.ContractDriven,interaction!.InteractionKind);
        }
    }
    [Fact]
    public void AllUnit5FamiliesGenerateAndGradeHundredsOfIndependentCorrectAndWrongAnswers()
    {
        foreach(var family in UsCcssG1FullGradeUnit5AdditionQuestions.Families)
        {
            for(int seed=0;seed<160;seed++)
            {
                var question=UsCcssG1FullGradeUnit5AdditionQuestions.Build(family,new Random(seed*41+23));
                var expected=UsCcssG1FullGradeUnit5AdditionQuestions.Solve(family,question.Parameters);
                Assert.True(UsCcssG1FullGradeUnit5AdditionQuestions.Valid(family,question.Parameters));
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,question.Parameters,expected));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,question.Parameters,expected+";FAKE=1"));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,question.Parameters,"TOTAL=999"));
            }
        }
    }
    [Fact]
    public void ComposingAndNoComposingCannotBeMixed()
    {
        var no=new Dictionary<string,int>{{"a",43},{"b",5},{"mode",0}};
        var yes=new Dictionary<string,int>{{"a",47},{"b",8},{"mode",0}};
        const string noFamily="usccss.g1.u05.l01.tens_or_ones_no_compose";
        const string yesFamily="usccss.g1.u05.l05.make_new_ten";
        Assert.True(UsCcssG1FullGradeUnit5AdditionQuestions.Valid(noFamily,no));
        Assert.False(UsCcssG1FullGradeUnit5AdditionQuestions.Valid(noFamily,yes));
        Assert.Equal("NEW_TEN=NO;TOTAL=48",UsCcssG1FullGradeUnit5AdditionQuestions.Solve(noFamily,no));
        Assert.Equal("MOVE=3;REST=5;TOTAL=55",UsCcssG1FullGradeUnit5AdditionQuestions.Solve(yesFamily,yes));
        Assert.False(UsCcssG1FullGradeUnit5AdditionQuestions.Verify(yesFamily,yes,"MOVE=3;REST=8;TOTAL=55"));
    }
    [Fact]
    public void SourceSpecificEngineProvidesAtLeastTenDistinctPracticeItemsPerLesson()
    {
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var family in UsCcssG1FullGradeUnit5AdditionQuestions.Families)
        {
            var rows=engine.Generate("g1-full-grade-unit5",family,[family],
                ExactSkillQuestionDifficulty.Standard,10,20261018,[]);
            Assert.Equal(10,rows.Count);
            Assert.Equal(10,rows.Select(x=>x.Prompt).Distinct(StringComparer.Ordinal).Count());
            foreach(var row in rows)
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,row.Parameters,row.CorrectAnswer));
        }
    }
}
