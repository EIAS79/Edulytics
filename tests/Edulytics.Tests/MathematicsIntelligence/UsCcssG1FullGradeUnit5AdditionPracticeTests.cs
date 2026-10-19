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
    [Theory]
    [InlineData(18,24,0,"PAIR=42;ALL=85")]
    [InlineData(18,24,1,"FIRST_PAIR=42;SECOND_PAIR=43;ALL=85")]
    [InlineData(18,23,2,"TOTAL=41;PACK=YES")]
    [InlineData(51,40,2,"TOTAL=91;PACK=NO")]
    [InlineData(18,23,3,"TOTAL=54;PACK=YES")]
    [InlineData(51,23,3,"TOTAL=87;PACK=NO")]
    public void RealOriginal2021FoodDriveStudentAndGradeTablesAreMathematicallyGradeable(
        int first,int second,int mode,string expected)
    {
        const string f="usccss.g1.u05.l14.optional_food_drive_numeric";
        var p=new Dictionary<string,int>{{"a",first},{"b",second},{"mode",mode}};
        Assert.True(UsCcssG1FullGradeUnit5AdditionQuestions.Valid(f,p));
        Assert.Equal(expected,UsCcssG1FullGradeUnit5AdditionQuestions.Solve(f,p));
        Assert.True(ExactSkillContractQuestionEngine.Verify(f,p,expected));
        Assert.False(ExactSkillContractQuestionEngine.Verify(f,p,
            expected+";FABRICATED=YES"));
    }
    [Fact]
    public void FoodDriveRejectsDuplicatedGradeRowsAndOutOfSourceValues()
    {
        const string f="usccss.g1.u05.l14.optional_food_drive_numeric";
        foreach(var (a,b,mode) in new (int,int,int)[]
            {(18,18,0),(18,51,0),(51,51,2),(24,13,2),(13,40,3),
             (51,40,3),(99,23,2)})
            Assert.False(UsCcssG1FullGradeUnit5AdditionQuestions.Valid(
                f,new Dictionary<string,int>{{"a",a},{"b",b},{"mode",mode}}));
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
