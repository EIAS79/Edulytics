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
    public void Original2021MakeTenStudentFacingIsActuallyGeneratedWithBothElenaAndAndreStrategies()
    {
        const string lesson5="usccss.g1.u05.l05.make_new_ten";
        const string lesson6="usccss.g1.u05.l06.explain_make_ten_equations";
        var set=new Dictionary<string,int>{{"a",47},{"b",8},{"mode",2}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(lesson5,set,
            "MOVE=3;REST=5;TOTAL=55"));
        set["a"]=26;set["mode"]=3;
        Assert.True(ExactSkillContractQuestionEngine.Verify(lesson5,set,
            "MOVE=4;REST=4;TOTAL=34"));
        set["a"]=34;set["b"]=9;set["mode"]=2;
        const string completed="MOVE=6;REST=3;TOTAL=43;ELENA=34+6+3=43;ANDRE=9+1+33=43";
        Assert.True(ExactSkillContractQuestionEngine.Verify(lesson6,set,completed));
        Assert.False(ExactSkillContractQuestionEngine.Verify(lesson6,set,
            completed.Replace("ANDRE=9+1+33=43","ANDRE=9+1+34=44")));
        Assert.False(ExactSkillContractQuestionEngine.Verify(lesson6,set,
            "MOVE=6;REST=3;TOTAL=43"));
        set["b"]=8;
        Assert.False(UsCcssG1FullGradeUnit5AdditionQuestions.Valid(lesson6,set));
        var generated=Enumerable.Range(0,400)
            .Select(n=>UsCcssG1FullGradeUnit5AdditionQuestions.Build(
                lesson6,new Random(n*83+31))).ToArray();
        Assert.Contains(generated,q=>q.Prompt.Contains("Elena and Andre both find 34+9"));
        Assert.Contains(generated,q=>q.Prompt.Contains("Find 6+68"));
        Assert.All(generated,q=>Assert.True(ExactSkillContractQuestionEngine.Verify(
            lesson6,q.Parameters,UsCcssG1FullGradeUnit5AdditionQuestions.Solve(lesson6,q.Parameters))));
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
    [InlineData(32,8,2,"MISSING=8;NEW_TEN=YES;TOTAL=40")]
    [InlineData(32,9,2,"MISSING=9;NEW_TEN=YES;TOTAL=41")]
    [InlineData(16,3,3,"MISSING=3;NEW_TEN=NO;TOTAL=19")]
    [InlineData(16,0,3,"MISSING=0;NEW_TEN=NO;TOTAL=16")]
    [InlineData(8,12,4,"MISSING=12;NEW_TEN=YES;TOTAL=20")]
    [InlineData(8,35,4,"MISSING=35;NEW_TEN=YES;TOTAL=43")]
    [InlineData(8,75,4,"MISSING=75;NEW_TEN=YES;TOTAL=83")]
    [InlineData(8,71,5,"MISSING=71;NEW_TEN=NO;TOTAL=79")]
    [InlineData(8,11,5,"MISSING=11;NEW_TEN=NO;TOTAL=19")]
    [InlineData(8,30,5,"MISSING=30;NEW_TEN=NO;TOTAL=38")]
    public void AllFourOriginalWaterSmudgeSourceQuestionsAcceptSourceValidChoices(
        int known,int missing,int mode,string expected)
    {
        const string family="usccss.g1.u05.l07.detect_new_ten";
        var p=new Dictionary<string,int>{{"a",known},{"b",missing},{"mode",mode}};
        Assert.True(UsCcssG1FullGradeUnit5AdditionQuestions.Valid(family,p));
        Assert.Equal(expected,UsCcssG1FullGradeUnit5AdditionQuestions.Solve(family,p));
        Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,expected));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,
            expected+";NOT_REVIEWED=1"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,
            expected.Replace($"TOTAL={known+missing}",$"TOTAL={known+missing+1}")));
    }

    [Fact]
    public void OriginalFourOpenChoiceSourceTasksPermitDifferentMathematicallyValidAnswers()
    {
        const string family="usccss.g1.u05.l07.detect_new_ten";
        var p=new Dictionary<string,int>{{"a",32},{"b",8},{"mode",2}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,
            "MISSING=9;NEW_TEN=YES;TOTAL=41"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,
            "MISSING=7;NEW_TEN=YES;TOTAL=39"));
        p["a"]=16;p["b"]=0;p["mode"]=3;
        Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,
            "MISSING=2;NEW_TEN=NO;TOTAL=18"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,
            "MISSING=4;NEW_TEN=NO;TOTAL=20"));
        p["a"]=8;p["b"]=12;p["mode"]=4;
        Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,
            "MISSING=49;NEW_TEN=YES;TOTAL=57"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,
            "MISSING=11;NEW_TEN=YES;TOTAL=19"));
        p["b"]=11;p["mode"]=5;
        Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,
            "MISSING=41;NEW_TEN=NO;TOTAL=49"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,
            "MISSING=12;NEW_TEN=NO;TOTAL=20"));
    }

    [Fact]
    public void MissingNumberCasesRejectNonSourceSizeAndCarryContradictions()
    {
        const string family="usccss.g1.u05.l07.detect_new_ten";
        foreach(var (a,b,mode) in new (int,int,int)[]
        {
            (32,7,2), (16,4,3), (8,11,4), (8,12,5),
            (8,92,4), (32,11,2), (16,10,3),
            (8,8,4), (8,8,5)
        })
            Assert.False(UsCcssG1FullGradeUnit5AdditionQuestions.Valid(family,
                new Dictionary<string,int>{{"a",a},{"b",b},{"mode",mode}}));
    }

    [Fact]
    public void GeneratedOriginalMissingDigitAndTwoDigitTasksIncludeAllSourceCases()
    {
        const string family="usccss.g1.u05.l07.detect_new_ten";
        var variants=new HashSet<int>();
        for(int seed=0;seed<350;seed++)
        {
            var q=UsCcssG1FullGradeUnit5AdditionQuestions.Build(family,new Random(seed*71+19));
            variants.Add(q.Parameters["mode"]);
            Assert.True(UsCcssG1FullGradeUnit5AdditionQuestions.Valid(family,q.Parameters));
            var correct=UsCcssG1FullGradeUnit5AdditionQuestions.Solve(family,q.Parameters);
            Assert.True(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,correct));
            if(q.Parameters["mode"]>=2)
            {
                Assert.Contains("MISSING=",correct);
                Assert.Contains("□",q.Prompt);
                Assert.Contains("COULD",q.Prompt);
                Assert.Contains("Choose ANY",q.Prompt);
                Assert.Contains(q.Parameters["mode"]<=3?"ONE-DIGIT":"TWO-DIGIT",q.Prompt);
            }
        }
        Assert.Equal(6,variants.Count);
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
