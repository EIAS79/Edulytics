using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UsCcssG1FullGradeUnit6MeasurementPracticeTests
{
    [Fact]
    public void SeventeenSourceGoalsHaveDigitalSubskillContracts()
    {
        Assert.Equal(17,UsCcssG1FullGradeUnit6MeasurementQuestions.Families.Count);
        for(int n=1;n<=17;n++)
        {
            var code=$"PED:US-CCSS-MATH:G1:U06:L{n:00}";
            var contract=LessonPracticeContractRegistry.All.Single(x=>x.LessonCode==code);
            var f=Assert.Single(contract.AllowedQuestionFamilies);
            Assert.True(UsCcssG1FullGradeUnit6MeasurementQuestions.Supports(f));
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(f,out var choice));
            Assert.Equal(LessonPracticeInteractionKind.ContractDriven,choice!.InteractionKind);
        }
    }
    [Fact]
    public void CorrectAndIncorrectMeasurementStoryAndCountingMathAreVerified()
    {
        foreach(var family in UsCcssG1FullGradeUnit6MeasurementQuestions.Families)
            for(int seed=0;seed<110;seed++)
            {
                var item=UsCcssG1FullGradeUnit6MeasurementQuestions.Build(family,new Random(seed*19+73));
                var answer=UsCcssG1FullGradeUnit6MeasurementQuestions.Solve(family,item.Parameters);
                Assert.True(UsCcssG1FullGradeUnit6MeasurementQuestions.Valid(family,item.Parameters));
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,item.Parameters,answer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,item.Parameters,answer+";WRONG=1"));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,item.Parameters,"DIFFERENCE=-4"));
            }
    }
    [Fact]
    public void GeneratedOriginalMeasurementComparisonsIncludeBothLongerDirectionsAndEquality()
    {
        var families=new[]{
            "usccss.g1.u06.l01.compare_lengths",
            "usccss.g1.u06.l02.indirect_length_comparison",
            "usccss.g1.u06.l03.choose_indirect_comparison",
            "usccss.g1.u06.l11.compare_shoe_lengths"
        };
        foreach(var family in families)
        {
            var seen=new HashSet<int>();
            var answers=new HashSet<string>(StringComparer.Ordinal);
            for(int seed=0;seed<1200;seed++)
            {
                var problem=UsCcssG1FullGradeUnit6MeasurementQuestions.Build(
                    family,new Random(seed*97+137));
                var p=problem.Parameters;
                if(family==families[0]&&p["mode"]%2==1)continue;
                seen.Add(Math.Sign(p["a"]-p["b"]));
                string result=UsCcssG1FullGradeUnit6MeasurementQuestions.Solve(family,p);
                answers.Add(result);
                Assert.True(UsCcssG1FullGradeUnit6MeasurementQuestions.Verify(family,p,result));
            }
            Assert.Contains(-1,seen); // strip B / second object longer
            Assert.Contains(0,seen);  // genuinely equal length
            Assert.Contains(1,seen);  // strip A / first object longer
            if(family==families[0])
            {
                Assert.Contains(answers,x=>x.StartsWith("LONGER=BLUE;",StringComparison.Ordinal));
                Assert.Contains(answers,x=>x.StartsWith("LONGER=RED;",StringComparison.Ordinal));
                Assert.Contains(answers,x=>x.StartsWith("LONGER=EQUAL;",StringComparison.Ordinal));
            }
        }
    }

    [Theory]
    [InlineData(5,9,1,"SHORTER=MAI")]
    [InlineData(10,4,1,"SHORTER=CLARE")]
    [InlineData(9,9,1,"SHORTER=EQUAL")]
    [InlineData(7,8,2,"FITS=NO")]
    [InlineData(8,7,2,"FITS=YES")]
    [InlineData(9,9,2,"FITS=YES")]
    [InlineData(7,8,3,"FITS=NO")]
    [InlineData(8,7,3,"FITS=YES")]
    public void OriginalUnit6Lesson3StudentTasksIncludeShorterRouteAndDoorFit(
        int referenceA,int referenceB,int mode,string expected)
    {
        const string family="usccss.g1.u06.l03.choose_indirect_comparison";
        var p=new Dictionary<string,int>{
            ["a"]=referenceA,["b"]=referenceB,["c"]=0,["mode"]=mode};
        Assert.True(UsCcssG1FullGradeUnit6MeasurementQuestions.Valid(family,p));
        Assert.Equal(expected,UsCcssG1FullGradeUnit6MeasurementQuestions.Solve(family,p));
        Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,expected));
        Assert.False(ExactSkillContractQuestionEngine.Verify(
            family,p,expected+";UNREVIEWED=1"));
        if(mode>=2)
            Assert.False(ExactSkillContractQuestionEngine.Verify(
                family,p,expected=="FITS=YES"?"FITS=NO":"FITS=YES"));
        else
            Assert.False(ExactSkillContractQuestionEngine.Verify(
                family,p,expected=="SHORTER=MAI"?"SHORTER=CLARE":"SHORTER=MAI"));
    }

    [Fact]
    public void ChangingUnitSizeChangesTheMeasureCountNotThePhysicalLength()
    {
        const string f="usccss.g1.u06.l07.unit_length_matters";
        var p=new Dictionary<string,int>{{"a",6},{"b",2},{"c",0},{"mode",0}};
        Assert.Equal("SMALL_UNITS=12",UsCcssG1FullGradeUnit6MeasurementQuestions.Solve(f,p));
        Assert.True(UsCcssG1FullGradeUnit6MeasurementQuestions.Verify(f,p,"SMALL_UNITS=12"));
        Assert.False(UsCcssG1FullGradeUnit6MeasurementQuestions.Verify(f,p,"SMALL_UNITS=6"));
    }
    [Theory]
    [InlineData(7,4,9,1,"ORDER=B,A,C")]
    [InlineData(8,20,12,1,"ORDER=A,C,B")]
    [InlineData(17,9,4,1,"ORDER=C,B,A")]
    [InlineData(7,4,9,3,"ORDER=C,A,B")]
    [InlineData(8,20,12,3,"ORDER=B,C,A")]
    [InlineData(17,9,4,3,"ORDER=A,B,C")]
    public void OriginalU06L01RequiresThreeDistinctLengthsOrderedNotOnlyTwoCompared(
        int a,int b,int c,int mode,string expected)
    {
        const string family="usccss.g1.u06.l01.compare_lengths";
        var p=new Dictionary<string,int>{{"a",a},{"b",b},{"c",c},{"mode",mode}};
        Assert.True(UsCcssG1FullGradeUnit6MeasurementQuestions.Valid(family,p));
        Assert.Equal(expected,UsCcssG1FullGradeUnit6MeasurementQuestions.Solve(family,p));
        Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,expected));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,expected+";FAKE=1"));
        // Both original directions are distinct whenever all three lengths differ.
        var other=new Dictionary<string,int>(p);
        other["mode"]=mode==1?3:1;
        Assert.True(UsCcssG1FullGradeUnit6MeasurementQuestions.Valid(family,other));
        var reverse=UsCcssG1FullGradeUnit6MeasurementQuestions.Solve(family,other);
        Assert.NotEqual(expected,reverse);
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,reverse));
    }

    [Fact]
    public void OriginalThreeObjectComparisonRejectsDuplicateOrOutOfRangeLengths()
    {
        const string family="usccss.g1.u06.l01.compare_lengths";
        foreach(var (a,b,c) in new[]{(7,7,9),(7,9,9),(7,9,0),(7,9,21)})
        {
            var p=new Dictionary<string,int>{{"a",a},{"b",b},{"c",c},{"mode",1}};
            Assert.False(UsCcssG1FullGradeUnit6MeasurementQuestions.Valid(family,p));
        }
    }

    [Fact]
    public void CenterExpressionsCompareDifferentValuesWithin20()
    {
        const string f="usccss.g1.u06.l04.center_add_compare";
        var p=new Dictionary<string,int>{{"a",31},{"b",6},{"c",0},{"mode",1}};
        Assert.Equal("RELATION=GREATER",UsCcssG1FullGradeUnit6MeasurementQuestions.Solve(f,p));
        var q=UsCcssG1FullGradeUnit6MeasurementQuestions.Build(f,new Random(412));
        // The random branch may choose an addition variant. Both remain
        // individually source-scoped, with a correct and rejectable key.
        Assert.True(UsCcssG1FullGradeUnit6MeasurementQuestions.Valid(f,q.Parameters));
        p["a"]=65;p["b"]=16;
        Assert.Equal("RELATION=LESS",UsCcssG1FullGradeUnit6MeasurementQuestions.Solve(f,p));
        p["a"]=16;p["b"]=1;
        Assert.Equal("RELATION=EQUAL",UsCcssG1FullGradeUnit6MeasurementQuestions.Solve(f,p));
        Assert.False(UsCcssG1FullGradeUnit6MeasurementQuestions.Verify(f,p,"RELATION=GREATER"));
    }
    [Theory]
    [InlineData(14,4,0,"LENGTH=10;DIFFERENCE=4")]
    [InlineData(9,4,1,"LENGTH=13;DIFFERENCE=4")]
    [InlineData(20,3,2,"LENGTH=17;DIFFERENCE=3")]
    [InlineData(15,5,3,"LENGTH=20;DIFFERENCE=5")]
    public void Original2021U06L12SolvesMissingBraceletLengthNotAlreadyKnownDifference(
        int knownLength,int difference,int mode,string expected)
    {
        const string family="usccss.g1.u06.l12.compare_measurements";
        var p=new Dictionary<string,int>{
            ["a"]=knownLength,["b"]=difference,["c"]=0,["mode"]=mode
        };
        Assert.True(UsCcssG1FullGradeUnit6MeasurementQuestions.Valid(family,p));
        var actual=UsCcssG1FullGradeUnit6MeasurementQuestions.Solve(family,p);
        Assert.Equal(expected,actual);
        Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,expected));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,
            "LENGTH="+difference+";DIFFERENCE="+difference));
    }
    [Theory]
    [InlineData(20,9,2,"MATCH=A,C;TOTAL=20")]
    [InlineData(18,7,3,"MATCH=B,C;TOTAL=18")]
    public void OriginalSourceU06L14RequiresTwoMatchingEquationsNotOneOperation(
        int whole,int removed,int mode,string answer)
    {
        const string family="usccss.g1.u06.l14.match_story_equation";
        var p=new Dictionary<string,int>{{"a",whole},{"b",removed},
            {"c",0},{"mode",mode}};
        Assert.True(UsCcssG1FullGradeUnit6MeasurementQuestions.Valid(family,p));
        Assert.Equal(answer,UsCcssG1FullGradeUnit6MeasurementQuestions.Solve(family,p));
        Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,answer));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,
            "MATCH=A,B;TOTAL="+whole));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,
            "OP=ADD;TOTAL="+whole));
        Assert.Equal(whole,whole-removed+removed);
    }

    [Fact]
    public void Original2021ComparisonTaskBoundsCannotCreateNegativeOrOutOfScopeLengths()
    {
        const string family="usccss.g1.u06.l12.compare_measurements";
        foreach(var (a,b,mode) in new[]{(4,5,0),(17,5,1),(15,0,0),(14,4,4)})
        {
            var p=new Dictionary<string,int>{{"a",a},{"b",b},{"c",0},{"mode",mode}};
            Assert.False(UsCcssG1FullGradeUnit6MeasurementQuestions.Valid(family,p));
        }
        for(int seed=0;seed<180;seed++)
        {
            var problem=UsCcssG1FullGradeUnit6MeasurementQuestions.Build(
                family,new Random(seed*59+73));
            var answer=UsCcssG1FullGradeUnit6MeasurementQuestions.Solve(family,problem.Parameters);
            int expectedLength=problem.Parameters["mode"]%2==0?
                problem.Parameters["a"]-problem.Parameters["b"]:
                problem.Parameters["a"]+problem.Parameters["b"];
            Assert.Contains("LENGTH="+expectedLength,answer);
            Assert.InRange(expectedLength,1,20);
            Assert.False(ExactSkillContractQuestionEngine.Verify(family,problem.Parameters,"LENGTH=999;DIFFERENCE=999"));
        }
    }
    [Fact]
    public void EngineSupportsTenDistinctDigitalPromptsPerSourceSkill()
    {
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var f in UsCcssG1FullGradeUnit6MeasurementQuestions.Families)
        {
            var rows=engine.Generate("grade1-u06",f,[f],ExactSkillQuestionDifficulty.Standard,10,20261019,[]);
            Assert.Equal(10,rows.Count);
            Assert.Equal(10,rows.Select(x=>x.Prompt).Distinct(StringComparer.Ordinal).Count());
        }
    }
}
