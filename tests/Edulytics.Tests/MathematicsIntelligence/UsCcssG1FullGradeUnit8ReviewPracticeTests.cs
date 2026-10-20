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
    [Theory]
    [InlineData(34,0,"CANDIDATES=34,44,54")]
    [InlineData(34,1,"NUMBER=44")]
    [InlineData(51,2,"CANDIDATES=51,61,71")]
    [InlineData(51,3,"NUMBER=61")]
    public void OriginalStudentFacing2021RiddleFirstAllowsSeveralNumbersThenOneFinalSolution(
        int lowest,int mode,string expected)
    {
        const string family="usccss.g1.u08.l09.solve_number_riddle";
        var p=new Dictionary<string,int>{{"a",lowest},{"b",10},{"c",0},{"mode",mode}};
        Assert.True(UsCcssG1FullGradeUnit8ReviewQuestions.Valid(family,p));
        Assert.Equal(expected,UsCcssG1FullGradeUnit8ReviewQuestions.Solve(family,p));
        Assert.True(UsCcssG1FullGradeUnit8ReviewQuestions.Verify(family,p,expected));
        var prompt=UsCcssG1FullGradeUnit8ReviewQuestions.Build(family,
            new Random(lowest*23+mode)).Prompt;
        Assert.Contains("NUMBER RIDDLE",prompt);
        Assert.Contains("ones",prompt);
        Assert.False(UsCcssG1FullGradeUnit8ReviewQuestions.Verify(family,p,
            mode%2==0?"CANDIDATES="+lowest:"NUMBER="+lowest));
    }

    [Fact]
    public void OriginalRiddleBoundsExcludeImpossibleOrAmbiguousDigitVariants()
    {
        const string family="usccss.g1.u08.l09.solve_number_riddle";
        foreach(int value in new[]{30,80,99,20})
        {
            var p=new Dictionary<string,int>{{"a",value},{"b",10},{"c",0},{"mode",0}};
            Assert.False(UsCcssG1FullGradeUnit8ReviewQuestions.Valid(family,p));
        }
    }

    [Theory]
    [InlineData(37,2,"TENS=3;ONES=7;TOTAL=37")]
    [InlineData(110,3,"TENS=11;ONES=0;TOTAL=110")]
    [InlineData(120,2,"TENS=12;ONES=0;TOTAL=120")]
    public void Original2021Unit8Lesson7ActualCollectionCanBeCountedAndRepresented(
        int collectionSize,int mode,string answer)
    {
        const string family="usccss.g1.u08.l07.count_on_to120";
        var p=new Dictionary<string,int>{{"a",collectionSize},{"b",0},{"c",0},{"mode",mode}};
        Assert.True(UsCcssG1FullGradeUnit8ReviewQuestions.Valid(family,p));
        Assert.Equal(answer,UsCcssG1FullGradeUnit8ReviewQuestions.Solve(family,p));
        Assert.True(UsCcssG1FullGradeUnit8ReviewQuestions.Verify(family,p,answer));
        Assert.False(UsCcssG1FullGradeUnit8ReviewQuestions.Verify(
            family,p,"LAST="+collectionSize));
    }
    [Fact]
    public void CountOnAndCountCollectionModesRequireDifferentValidSourceOperands()
    {
        const string family="usccss.g1.u08.l07.count_on_to120";
        var count=new Dictionary<string,int>{{"a",116},{"b",4},{"c",0},{"mode",0}};
        Assert.True(UsCcssG1FullGradeUnit8ReviewQuestions.Valid(family,count));
        Assert.Equal("LAST=120",UsCcssG1FullGradeUnit8ReviewQuestions.Solve(family,count));
        count["mode"]=3;
        Assert.False(UsCcssG1FullGradeUnit8ReviewQuestions.Valid(family,count));
        count["b"]=0;
        Assert.True(UsCcssG1FullGradeUnit8ReviewQuestions.Valid(family,count));
        count["a"]=121;
        Assert.False(UsCcssG1FullGradeUnit8ReviewQuestions.Valid(family,count));
    }

    [Theory]
    [InlineData(4,5,2,"ADD=4+5=9;SUB=9-5=4")]
    [InlineData(4,5,3,"MISSING_ADD=5;MISSING_SUB=4")]
    [InlineData(2,7,2,"ADD=2+7=9;SUB=9-7=2")]
    public void OriginalUnit8Lesson2ChecksBothEquationsOrBothUnknowns(
        int a,int b,int mode,string correct)
    {
        const string family="usccss.g1.u08.l02.inverse_add_subtract";
        var p=new Dictionary<string,int>{{"a",a},{"b",b},{"c",0},{"mode",mode}};
        Assert.True(UsCcssG1FullGradeUnit8ReviewQuestions.Valid(family,p));
        Assert.Equal(correct,UsCcssG1FullGradeUnit8ReviewQuestions.Solve(family,p));
        Assert.True(UsCcssG1FullGradeUnit8ReviewQuestions.Verify(family,p,correct));
        Assert.False(UsCcssG1FullGradeUnit8ReviewQuestions.Verify(family,p,
            "LEFT="+a+";WHOLE="+(a+b)));
        Assert.False(UsCcssG1FullGradeUnit8ReviewQuestions.Verify(family,p,
            correct+";FAKE=1"));
    }

    [Theory]
    [InlineData(54,2,"ALT_TENS=3;ALT_ONES=24;TOTAL=54")]
    [InlineData(68,3,"WAYS=7;ZERO_TENS_ONES=68;TOTAL=68")]
    [InlineData(20,2,"ALT_TENS=0;ALT_ONES=20;TOTAL=20")]
    [InlineData(99,3,"WAYS=10;ZERO_TENS_ONES=99;TOTAL=99")]
    [InlineData(100,0,"NEW_TENS=9;NEW_ONES=10;TOTAL=100")]
    [InlineData(100,2,"ALT_TENS=8;ALT_ONES=20;TOTAL=100")]
    [InlineData(100,3,"WAYS=11;ZERO_TENS_ONES=100;TOTAL=100")]
    public void OriginalShowAllTheWaysNeedsMultipleRegroupingsAndHowManyModels(
        int value,int mode,string correct)
    {
        const string family="usccss.g1.u08.l08.same_number_tens_ones";
        var p=new Dictionary<string,int>{{"a",value},{"b",0},{"c",0},{"mode",mode}};
        Assert.True(UsCcssG1FullGradeUnit8ReviewQuestions.Valid(family,p));
        Assert.Equal(correct,UsCcssG1FullGradeUnit8ReviewQuestions.Solve(family,p));
        Assert.True(UsCcssG1FullGradeUnit8ReviewQuestions.Verify(family,p,correct));
        Assert.False(UsCcssG1FullGradeUnit8ReviewQuestions.Verify(family,p,
            $"NEW_TENS={value/10-1};NEW_ONES={value%10+10};TOTAL={value}"));
        Assert.False(UsCcssG1FullGradeUnit8ReviewQuestions.Verify(family,p,
            correct+";FALSE=1"));
    }

    [Fact]
    public void SourceClassBookDomainIncludesOneHundredButNotOneHundredOne()
    {
        const string family="usccss.g1.u08.l08.same_number_tens_ones";
        var p=new Dictionary<string,int>{{"a",100},{"b",0},{"c",0},{"mode",1}};
        Assert.True(UsCcssG1FullGradeUnit8ReviewQuestions.Valid(family,p));
        Assert.Equal("NEW_TENS=9;NEW_ONES=10;TOTAL=100",
            UsCcssG1FullGradeUnit8ReviewQuestions.Solve(family,p));
        Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,
            "NEW_TENS=9;NEW_ONES=10;TOTAL=100"));
        p["a"]=101;
        Assert.False(UsCcssG1FullGradeUnit8ReviewQuestions.Valid(family,p));
        p["a"]=19;
        Assert.False(UsCcssG1FullGradeUnit8ReviewQuestions.Valid(family,p));
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
