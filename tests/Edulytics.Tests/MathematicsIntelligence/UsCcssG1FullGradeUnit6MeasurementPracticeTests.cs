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
            var families=contract.AllowedQuestionFamilies;
            Assert.Equal(n==4?2:1,families.Count);
            foreach(var f in families)
            {
                Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
                Assert.True(LessonPracticeInteractionRegistry.TryResolve(f,out var choice));
                Assert.Equal(LessonPracticeInteractionKind.ContractDriven,choice!.InteractionKind);
                Assert.True(UsCcssG1FullGradeUnit6MeasurementQuestions.Supports(f) ||
                    UsCcssG1HowCloseStage3Questions.Supports(f));
            }
            if(n==4)
            {
                Assert.Contains(UsCcssG1HowCloseStage3Questions.FamilyId,families);
                Assert.Contains("usccss.g1.u06.l04.center_add_compare",families);
            }
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
    [InlineData(6,"SMALL_UNITS=6;MEDIUM_UNITS=3;LARGE_UNITS=2")]
    [InlineData(12,"SMALL_UNITS=12;MEDIUM_UNITS=6;LARGE_UNITS=4")]
    public void OriginalU06L07RequiresThreeDistinctUnitCountsForOneFixedLength(
        int length,string expected)
    {
        const string family="usccss.g1.u06.l07.unit_length_matters";
        var p=new Dictionary<string,int>{
            ["a"]=length,["b"]=2,["c"]=0,["mode"]=4
        };
        Assert.True(UsCcssG1FullGradeUnit6MeasurementQuestions.Valid(family,p));
        Assert.Equal(expected,UsCcssG1FullGradeUnit6MeasurementQuestions.Solve(family,p));
        Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,expected));
        Assert.False(ExactSkillContractQuestionEngine.Verify(
            family,p,$"SMALL_UNITS={length};MEDIUM_UNITS={length};LARGE_UNITS={length}"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,expected+";EXTRA=1"));
        var invalid=new Dictionary<string,int>(p){["a"]=9};
        Assert.False(UsCcssG1FullGradeUnit6MeasurementQuestions.Valid(family,invalid));
    }

    [Fact]
    public void Unit6Lesson7GeneratesAllOriginalAndAdditionalSourceRelevantModes()
    {
        const string family="usccss.g1.u06.l07.unit_length_matters";
        var seen=new HashSet<int>();
        for(int seed=0;seed<600;seed++)
        {
            var item=UsCcssG1FullGradeUnit6MeasurementQuestions.Build(
                family,new Random(seed*137+17));
            int mode=item.Parameters["mode"];
            seen.Add(mode);
            if(mode!=4)continue;
            Assert.Contains("THREE-UNIT COMPARISON",item.Prompt);
            Assert.Contains("SAME strip",item.Prompt);
            Assert.Contains("not a claim",item.Prompt);
            var answer=UsCcssG1FullGradeUnit6MeasurementQuestions.Solve(
                family,item.Parameters);
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                family,item.Parameters,answer));
            Assert.False(ExactSkillContractQuestionEngine.Verify(
                family,item.Parameters,"SMALL_UNITS=1;MEDIUM_UNITS=1;LARGE_UNITS=1"));
        }
        Assert.Equal(Enumerable.Range(0,5).ToArray(),seen.OrderBy(x=>x));
    }

    [Theory]
    [InlineData(101,"TENS=10;ONES=1;TOTAL=101")]
    [InlineData(109,"TENS=10;ONES=9;TOTAL=109")]
    [InlineData(110,"TENS=11;ONES=0;TOTAL=110")]
    public void OriginalU06L08IncludesSourceMeasurementCountsOverOneHundred(
        int count,string expected)
    {
        const string family="usccss.g1.u06.l08.count_to110_cube_groups";
        var p=new Dictionary<string,int>{
            ["a"]=count,["b"]=0,["c"]=0,["mode"]=4
        };
        Assert.True(UsCcssG1FullGradeUnit6MeasurementQuestions.Valid(family,p));
        Assert.Equal(expected,UsCcssG1FullGradeUnit6MeasurementQuestions.Solve(family,p));
        Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,expected));
        Assert.False(ExactSkillContractQuestionEngine.Verify(
            family,p,$"TENS=10;ONES={count-100};TOTAL=100"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(
            family,p,expected+";EXTRA=1"));
        var outOfSourceRange=new Dictionary<string,int>(p){["a"]=100};
        Assert.False(UsCcssG1FullGradeUnit6MeasurementQuestions.Valid(
            family,outOfSourceRange));
        var tooLarge=new Dictionary<string,int>(p){["a"]=111};
        Assert.False(UsCcssG1FullGradeUnit6MeasurementQuestions.Valid(
            family,tooLarge));
    }

    [Fact]
    public void OriginalU06L08GeneratorExercisesEverySourceAndLegacyMode()
    {
        const string family="usccss.g1.u06.l08.count_to110_cube_groups";
        var modes=new HashSet<int>();
        for(int seed=0;seed<600;seed++)
        {
            var item=UsCcssG1FullGradeUnit6MeasurementQuestions.Build(
                family,new Random(seed*139+23));
            modes.Add(item.Parameters["mode"]);
            var expected=UsCcssG1FullGradeUnit6MeasurementQuestions.Solve(
                family,item.Parameters);
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                family,item.Parameters,expected));
            if(item.Parameters["mode"]==4)
            {
                Assert.InRange(item.Parameters["a"],101,110);
                Assert.Contains("MORE THAN 100 cubes",item.Prompt);
            }
        }
        Assert.Equal(Enumerable.Range(0,5).ToArray(),modes.OrderBy(x=>x));
    }

    [Theory]
    [InlineData(12,9,"ACTUAL=12;ESTIMATE=9;DIFFERENCE=3;DIRECTION=LOW")]
    [InlineData(12,12,"ACTUAL=12;ESTIMATE=12;DIFFERENCE=0;DIRECTION=EXACT")]
    [InlineData(12,15,"ACTUAL=12;ESTIMATE=15;DIFFERENCE=3;DIRECTION=HIGH")]
    public void OriginalU06L10RequiresEstimateBeforeMeasurementAndAccurateDifference(
        int measured,int estimate,string expected)
    {
        const string family="usccss.g1.u06.l10.center_measure_and_add";
        var p=new Dictionary<string,int>{
            ["a"]=measured,["b"]=estimate,["c"]=0,["mode"]=4
        };
        Assert.True(UsCcssG1FullGradeUnit6MeasurementQuestions.Valid(family,p));
        Assert.Equal(expected,UsCcssG1FullGradeUnit6MeasurementQuestions.Solve(family,p));
        Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,expected));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,
            "ACTUAL=12;ESTIMATE=9;DIFFERENCE=0;DIRECTION=EXACT"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(
            family,p,expected+";FABRICATED=1"));
        var invalid=new Dictionary<string,int>(p){["b"]=measured+5};
        Assert.False(UsCcssG1FullGradeUnit6MeasurementQuestions.Valid(family,invalid));
    }

    [Fact]
    public void OriginalU06L10GeneratesEstimateModeAndPreservesLegacyCenterTasks()
    {
        const string family="usccss.g1.u06.l10.center_measure_and_add";
        var modes=new HashSet<int>();
        for(int seed=0;seed<600;seed++)
        {
            var item=UsCcssG1FullGradeUnit6MeasurementQuestions.Build(
                family,new Random(seed*101+71));
            modes.Add(item.Parameters["mode"]);
            var expected=UsCcssG1FullGradeUnit6MeasurementQuestions.Solve(
                family,item.Parameters);
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                family,item.Parameters,expected));
            if(item.Parameters["mode"]==4)
            {
                Assert.Contains("BEFORE counting",item.Prompt);
                Assert.Contains("estimate was HIGH, LOW or EXACT",item.Prompt);
            }
        }
        Assert.Equal(Enumerable.Range(0,9).ToArray(),modes.OrderBy(x=>x));
    }

    [Fact]
    public void OriginalUnit6Lesson15ClareCranesRequiresMissingStartNotOnlyKnownResult()
    {
        const string family="usccss.g1.u06.l15.write_story_equations";
        var source=new Dictionary<string,int>{
            ["a"]=19,["b"]=12,["c"]=0,["mode"]=4
        };
        Assert.True(UsCcssG1FullGradeUnit6MeasurementQuestions.Valid(family,source));
        Assert.Equal("START=19;EQUATION=19-12=7",
            UsCcssG1FullGradeUnit6MeasurementQuestions.Solve(family,source));
        Assert.True(ExactSkillContractQuestionEngine.Verify(
            family,source,"START=19;EQUATION=19-12=7"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(
            family,source,"START=7;EQUATION=19-12=7"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(
            family,source,"START=19;EQUATION=19+12=31"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(
            family,source,"START=19;EQUATION=19-12=7;GALLERY=PASSED"));
        var wrongSource=new Dictionary<string,int>(source){["b"]=11};
        Assert.False(UsCcssG1FullGradeUnit6MeasurementQuestions.Valid(
            family,wrongSource));
    }

    [Fact]
    public void OrigamiPracticeRetainsFourExistingEquationModesAlongsideSource()
    {
        const string family="usccss.g1.u06.l15.write_story_equations";
        var seen=new HashSet<int>();
        for(int seed=0;seed<600;seed++)
        {
            var item=UsCcssG1FullGradeUnit6MeasurementQuestions.Build(
                family,new Random(seed*103+71));
            seen.Add(item.Parameters["mode"]);
            var answer=UsCcssG1FullGradeUnit6MeasurementQuestions.Solve(
                family,item.Parameters);
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                family,item.Parameters,answer));
            if(item.Parameters["mode"]==4)
            {
                Assert.Contains("Clare gave 12 paper cranes",item.Prompt);
                Assert.Contains("The START is unknown",item.Prompt);
            }
        }
        Assert.Equal(Enumerable.Range(0,5).ToArray(),seen.OrderBy(x=>x));
    }

    [Theory]
    [InlineData(101,"TENS=10;ONES=1;TOTAL=101")]
    [InlineData(110,"TENS=11;ONES=0;TOTAL=110")]
    [InlineData(120,"TENS=12;ONES=0;TOTAL=120")]
    public void OriginalU06L16CollectionGroupingIncludesOneHundredTwenty(
        int count,string answer)
    {
        const string family="usccss.g1.u06.l16.center_mixed_math";
        var p=new Dictionary<string,int>{{"a",count},{"b",0},{"c",0},{"mode",4}};
        Assert.True(UsCcssG1FullGradeUnit6MeasurementQuestions.Valid(family,p));
        Assert.Equal(answer,UsCcssG1FullGradeUnit6MeasurementQuestions.Solve(family,p));
        Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,answer));
        Assert.False(ExactSkillContractQuestionEngine.Verify(
            family,p,"TENS=10;ONES=0;TOTAL=100"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(
            family,p,answer+";PUPIL=OBSERVED"));
        var outside=new Dictionary<string,int>(p){["a"]=121};
        Assert.False(UsCcssG1FullGradeUnit6MeasurementQuestions.Valid(family,outside));
        outside["a"]=100;
        Assert.False(UsCcssG1FullGradeUnit6MeasurementQuestions.Valid(family,outside));
    }

    [Fact]
    public void OriginalU06L16NumberTalkPreservesEquivalentEightySixExpressions()
    {
        const string family="usccss.g1.u06.l16.center_mixed_math";
        var p=new Dictionary<string,int>{{"a",65},{"b",21},{"c",0},{"mode",5}};
        Assert.True(UsCcssG1FullGradeUnit6MeasurementQuestions.Valid(family,p));
        const string correct="FIRST=86;SECOND=86;EQUAL=YES";
        Assert.Equal(correct,UsCcssG1FullGradeUnit6MeasurementQuestions.Solve(family,p));
        Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,correct));
        Assert.False(ExactSkillContractQuestionEngine.Verify(
            family,p,"FIRST=86;SECOND=85;EQUAL=NO"));
        var illegal=new Dictionary<string,int>(p){["b"]=20};
        Assert.False(UsCcssG1FullGradeUnit6MeasurementQuestions.Valid(family,illegal));
        var modes=new HashSet<int>();
        for(int seed=0;seed<720;seed++)
        {
            var item=UsCcssG1FullGradeUnit6MeasurementQuestions.Build(
                family,new Random(seed*107+17));
            modes.Add(item.Parameters["mode"]);
            var expected=UsCcssG1FullGradeUnit6MeasurementQuestions.Solve(
                family,item.Parameters);
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                family,item.Parameters,expected));
            if(item.Parameters["mode"]==4)
                Assert.InRange(item.Parameters["a"],101,120);
            if(item.Parameters["mode"]==5)
                Assert.Contains("65+20+1 and 65+21",item.Prompt);
        }
        Assert.Equal(Enumerable.Range(0,10).ToArray(),modes.OrderBy(x=>x));
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
        foreach(var (a,b,mode) in new[]{(4,5,0),(17,5,1),(15,0,0),(14,5,4)})
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
    [Theory]
    [InlineData(9,7,0,4,"TOTAL=16")]
    [InlineData(7,9,4,5,"TOTAL=20")]
    [InlineData(13,8,0,6,"DIFFERENCE=5")]
    [InlineData(8,17,0,7,"ELENA=9")]
    public void Original2021U06L11ShoeStoriesUseFourDifferentCorrectUnknowns(
        int a,int b,int c,int mode,string expected)
    {
        const string family="usccss.g1.u06.l11.compare_shoe_lengths";
        var p=new Dictionary<string,int>{{"a",a},{"b",b},{"c",c},{"mode",mode}};
        Assert.True(UsCcssG1FullGradeUnit6MeasurementQuestions.Valid(family,p));
        Assert.Equal(expected,UsCcssG1FullGradeUnit6MeasurementQuestions.Solve(family,p));
        Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,expected));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,"TOTAL=999"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,expected+";FAKE=YES"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,""));
    }
    [Fact]
    public void ShoeStoryModesNeverPermitInventedSourceOperandsOrThirdLengthOutsideThreeAddends()
    {
        const string family="usccss.g1.u06.l11.compare_shoe_lengths";
        foreach(var (a,b,c,mode) in new[]{
            (9,8,0,4),(9,7,4,4),(7,9,0,5),
            (7,9,5,5),(13,9,0,6),(8,17,1,7),(9,7,0,8)
        })
        {
            var p=new Dictionary<string,int>{{"a",a},{"b",b},{"c",c},{"mode",mode}};
            Assert.False(UsCcssG1FullGradeUnit6MeasurementQuestions.Valid(family,p));
        }
        // The pre-existing four hypothetical comparison modes still accept
        // independently varied shoe lengths and genuine equality.
        var previous=new Dictionary<string,int>{{"a",10},{"b",10},{"c",0},{"mode",2}};
        Assert.True(UsCcssG1FullGradeUnit6MeasurementQuestions.Valid(family,previous));
        Assert.Equal("DIFFERENCE=0",
            UsCcssG1FullGradeUnit6MeasurementQuestions.Solve(family,previous));
    }

    [Theory]
    [InlineData(95, "NUMBER=95")]
    [InlineData(99, "NUMBER=99")]
    [InlineData(100, "NUMBER=100")]
    [InlineData(120, "NUMBER=120")]
    public void OriginalAnimalLengthNumeralsIncludeBothNinetyFiveAndOneHundredTwenty(
        int cubes,string correct)
    {
        const string family="usccss.g1.u06.l09.write_number_to120";
        var parameters=new Dictionary<string,int>{
            ["a"]=cubes,["b"]=0,["c"]=0,["mode"]=0
        };
        Assert.True(UsCcssG1FullGradeUnit6MeasurementQuestions.Valid(family,parameters));
        Assert.Equal(correct,UsCcssG1FullGradeUnit6MeasurementQuestions.Solve(family,parameters));
        Assert.True(ExactSkillContractQuestionEngine.Verify(family,parameters,correct));
        Assert.False(ExactSkillContractQuestionEngine.Verify(
            family,parameters,"NUMBER="+(cubes-1)));
        parameters["a"]=94;
        Assert.False(UsCcssG1FullGradeUnit6MeasurementQuestions.Valid(family,parameters));
        parameters["a"]=121;
        Assert.False(UsCcssG1FullGradeUnit6MeasurementQuestions.Valid(family,parameters));
    }

    [Theory]
    [InlineData("usccss.g1.u06.l13.take_from_unknown",20,9,4,"START=20","ELENA BEADS")]
    [InlineData("usccss.g1.u06.l14.match_story_equation",11,8,4,"MATCH=A,C;UNKNOWN=3","JADA")]
    [InlineData("usccss.g1.u06.l14.match_story_equation",19,11,5,"MATCH=A,C;UNKNOWN=8","KIRAN")]
    [InlineData("usccss.g1.u06.l14.match_story_equation",16,10,6,"MATCH=A,C;UNKNOWN=6","HAN AND LIN")]
    [InlineData("usccss.g1.u06.l14.match_story_equation",14,9,7,"MATCH=A,C;TOTAL=14","ELENA AND ANDRE")]
    [InlineData("usccss.g1.u06.l14.match_story_equation",16,6,8,"MATCH=A,C;UNKNOWN=10","NOAH AND TYLER")]
    [InlineData("usccss.g1.u06.l14.match_story_equation",14,9,9,"MATCH=A,C;UNKNOWN=14","CLARE")]
    public void OriginalTakeFromAndSixSourceStoriesHaveExactDistinctValidCases(
        string family,int a,int b,int mode,string answer,string sourceContext)
    {
        var p=new Dictionary<string,int>{
            ["a"]=a,["b"]=b,["c"]=0,["mode"]=mode
        };
        Assert.True(UsCcssG1FullGradeUnit6MeasurementQuestions.Valid(family,p));
        Assert.Equal(answer,UsCcssG1FullGradeUnit6MeasurementQuestions.Solve(family,p));
        Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,answer));
        Assert.False(ExactSkillContractQuestionEngine.Verify(
            family,p,answer+";FALSE=1"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(
            family,p,"MATCH=B,C;UNKNOWN=999"));
        p["a"]++;
        Assert.False(UsCcssG1FullGradeUnit6MeasurementQuestions.Valid(family,p));
        // Context is verified separately from the deterministic answer oracle.
        var allContexts=new HashSet<string>(StringComparer.Ordinal);
        for(int seed=0;seed<250;seed++)
        {
            var x=UsCcssG1FullGradeUnit6MeasurementQuestions.Build(
                family,new Random(seed*41+17));
            if(x.Parameters["mode"]==mode)
                allContexts.Add(x.Prompt);
        }
        Assert.Contains(allContexts,context=>
            context.Contains(sourceContext,StringComparison.Ordinal));
    }

    [Fact]
    public void OriginalU06L13AndL14NewSourceModesDoNotInvalidateHistoricalInstances()
    {
        foreach(var (family,first,second) in new[]{
            ("usccss.g1.u06.l13.take_from_unknown",15,5),
            ("usccss.g1.u06.l14.match_story_equation",15,5)
        })
        {
            for(int mode=0;mode<4;mode++)
            {
                var old=new Dictionary<string,int>{
                    ["a"]=first,["b"]=second,["c"]=0,["mode"]=mode
                };
                Assert.True(UsCcssG1FullGradeUnit6MeasurementQuestions.Valid(family,old));
                var expected=UsCcssG1FullGradeUnit6MeasurementQuestions.Solve(family,old);
                Assert.True(UsCcssG1FullGradeUnit6MeasurementQuestions.Verify(
                    family,old,expected));
            }
        }
    }

    [Fact]
    public void OriginalU06L04SevenCardFamilyGeneratesUniqueGradeableTasks()
    {
        const string family=UsCcssG1HowCloseStage3Questions.FamilyId;
        var engine=new ExactSkillContractQuestionEngine();
        var results=engine.Generate("grade1-u06-l04-stage3","source-seven-card-v2",
            [family],ExactSkillQuestionDifficulty.Standard,10,20261020,[]);
        Assert.Equal(10,results.Count);
        Assert.Equal(10,results.Select(x=>x.Prompt).Distinct(StringComparer.Ordinal).Count());
        foreach(var item in results)
        {
            Assert.Equal(family,item.Family);
            Assert.True(UsCcssG1HowCloseStage3Questions.Valid(item.Parameters));
            Assert.Contains("WITHOUT GOING OVER",item.Prompt);
            Assert.True(ExactSkillContractQuestionEngine.Verify(family,
                item.Parameters,item.CorrectAnswer));
            Assert.False(ExactSkillContractQuestionEngine.Verify(family,
                item.Parameters,item.CorrectAnswer+";UNREVIEWED=1"));
            Assert.False(ExactSkillContractQuestionEngine.Verify(family,
                item.Parameters,"NUMBERS=99,99;TOTAL=198"));
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
    [Fact]
    public void OriginalPaperClipMeasurementRequiresUniformEndToEndUnitsAndRejectsGapOverlap()
    {
        const string family="usccss.g1.u06.l06.measure_paperclips";
        var seenModes=new HashSet<int>();
        for(int mode=0;mode<=7;mode++)
        {
            var p=new Dictionary<string,int>{
                ["a"]=8,["b"]=0,["c"]=0,["mode"]=mode
            };
            Assert.True(UsCcssG1FullGradeUnit6MeasurementQuestions.Valid(family,p));
            var expected=mode switch
            {
                4=>"VALID=YES",
                5 or 6 or 7=>"VALID=NO",
                _=>"LENGTH=8"
            };
            var rendered=UsCcssG1FullGradeUnit6MeasurementQuestions.Build(
                family,new Random(mode*1151+73));
            // Seeded generator may choose a different mode, but each item
            // must have a mathematically valid and independently gradeable key.
            var generated=UsCcssG1FullGradeUnit6MeasurementQuestions.Solve(
                family,rendered.Parameters);
            Assert.True(UsCcssG1FullGradeUnit6MeasurementQuestions.Verify(
                family,rendered.Parameters,generated));
            seenModes.Add(rendered.Parameters["mode"]);
            Assert.Equal(expected,
                UsCcssG1FullGradeUnit6MeasurementQuestions.Solve(family,p));
            Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,expected));
            Assert.False(ExactSkillContractQuestionEngine.Verify(
                family,p,expected=="VALID=YES"?"VALID=NO":
                    expected=="VALID=NO"?"VALID=YES":"LENGTH=9"));
            Assert.False(ExactSkillContractQuestionEngine.Verify(
                family,p,expected+";UNOBSERVED=1"));
            if(rendered.Parameters["mode"]>=4)
                Assert.Contains("PAPER CLIP CHECK",rendered.Prompt);
        }
        for(int seed=0;seed<500;seed++)
        {
            var item=UsCcssG1FullGradeUnit6MeasurementQuestions.Build(
                family,new Random(seed*127+19));
            seenModes.Add(item.Parameters["mode"]);
            Assert.True(UsCcssG1FullGradeUnit6MeasurementQuestions.Valid(
                family,item.Parameters));
        }
        Assert.Equal(Enumerable.Range(0,8).ToArray(),seenModes.OrderBy(x=>x));
        // No paper-clip task can certify student handling of actual
        // publisher tape-strip drawings or classroom peer explanations.
    }

}
