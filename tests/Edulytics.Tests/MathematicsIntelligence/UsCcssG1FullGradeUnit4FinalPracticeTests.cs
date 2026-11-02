using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UsCcssG1FullGradeUnit4FinalPracticeTests
{
    [Fact]
    public void AllEightLessonFamiliesHaveDedicatedDigitallyGradeableContracts()
    {
        Assert.Equal(8,UsCcssG1FullGradeUnit4FinalQuestions.Families.Count);
        for(int i=16;i<=23;i++)
        {
            var c=LessonPracticeContractRegistry.All.Single(x=>x.LessonCode==$"PED:US-CCSS-MATH:G1:U04:L{i:00}");
            var f=Assert.Single(c.AllowedQuestionFamilies);
            Assert.True(UsCcssG1FullGradeUnit4FinalQuestions.Supports(f));
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(f,out var interaction));
            Assert.Equal(LessonPracticeInteractionKind.ContractDriven,interaction!.InteractionKind);
        }
    }
    [Fact]
    public void DigitalTensAndOnesFractionsAndComparisonsRejectFalseResults()
    {
        foreach(var family in UsCcssG1FullGradeUnit4FinalQuestions.Families)
            for(int seed=0;seed<110;seed++)
            {
                var problem=UsCcssG1FullGradeUnit4FinalQuestions.Build(family,new Random(103*seed+19));
                var answer=UsCcssG1FullGradeUnit4FinalQuestions.Solve(family,problem.Parameters);
                Assert.True(UsCcssG1FullGradeUnit4FinalQuestions.Valid(family,problem.Parameters));
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,problem.Parameters,answer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,problem.Parameters,"ANSWER=999"));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,problem.Parameters,answer+";FAKE=1"));
            }
    }
    [Fact]
    public void Lesson81AllOriginalSymbolBlanksHaveStrictSourceOperandsAndWrongAnswerControls()
    {
        const string family="usccss.g1.u04.l16.write_comparison";
        foreach(var (mode,a,b,expected) in new[]{
            (4,56,26,"SYMBOL=>"),
            (5,72,78,"SYMBOL=<"),
            (6,6,55,"SYMBOL=<"),
            (7,92,29,"SYMBOL=>"),
            (8,23,23,"SYMBOL==")
        })
        {
            var p=new Dictionary<string,int>{{"a",a},{"b",b},{"c",0},{"mode",mode}};
            Assert.True(UsCcssG1FullGradeUnit4FinalQuestions.Valid(family,p));
            Assert.Equal(expected,UsCcssG1FullGradeUnit4FinalQuestions.Solve(family,p));
            Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,expected));
            Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,"SYMBOL=?"));
            Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,expected+";FAKE=1"));
            p["a"]++;
            Assert.False(UsCcssG1FullGradeUnit4FinalQuestions.Valid(family,p));
        }
    }

    [Fact]
    public void Lesson81OriginalOpenNumberBoxesAcceptAllValidChoicesWithoutPredeterminedSingleAnswer()
    {
        const string family="usccss.g1.u04.l16.write_comparison";
        foreach(var (mode,a,validA,validB,invalid) in new[]{
            (9,78,"NUMBER=79","NUMBER=99","NUMBER=78"),
            (10,39,"NUMBER=40","NUMBER=98","NUMBER=39"),
            (11,13,"NUMBER=13","number = 13","NUMBER=14")
        })
        {
            var p=new Dictionary<string,int>{{"a",a},{"b",0},{"c",0},{"mode",mode}};
            Assert.True(UsCcssG1FullGradeUnit4FinalQuestions.Valid(family,p));
            Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,validA));
            Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,validB));
            Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,invalid));
            Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,"NUMBER=100"));
            Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,validA+";EXTRA=1"));
            p["a"]++;
            Assert.False(UsCcssG1FullGradeUnit4FinalQuestions.Valid(family,p));
        }
        foreach(var (mode,valid,wrong) in new[]{
            (12,"LEFT=12;RIGHT=21","LEFT=21;RIGHT=12"),
            (13,"LEFT=93;RIGHT=25","LEFT=25;RIGHT=93")
        })
        {
            var p=new Dictionary<string,int>{{"a",0},{"b",0},{"c",0},{"mode",mode}};
            Assert.True(UsCcssG1FullGradeUnit4FinalQuestions.Valid(family,p));
            Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,valid));
            Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,wrong));
            Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,"LEFT=21;RIGHT=21"));
            Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,"LEFT=4;RIGHT=46"));
            Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,valid+";FAKE=1"));
        }
        var generated=Enumerable.Range(0,3600).Select(i=>
            UsCcssG1FullGradeUnit4FinalQuestions.Build(family,new Random(i*23+20261020))).ToArray();
        foreach(var mode in Enumerable.Range(4,10))
        {
            var sourceCases=generated.Where(x=>x.Parameters["mode"]==mode).ToArray();
            Assert.True(sourceCases.Length>120);
            Assert.All(sourceCases,x=>Assert.True(ExactSkillContractQuestionEngine.Verify(
                family,x.Parameters,UsCcssG1FullGradeUnit4FinalQuestions.Solve(family,x.Parameters))));
        }
    }

    [Fact]
    public void Lesson82Source25ComparisonExtensionsAcceptDistinctCorrectStudentChoices()
    {
        const string family="usccss.g1.u04.l17.order_three_numbers";
        foreach(var (mode,ok1,ok2,bad) in new[]{
            (5,"NUMBER=26","NUMBER=99","NUMBER=25"),
            (6,"NUMBER=1","NUMBER=24","NUMBER=25")
        })
        {
            var p=new Dictionary<string,int>{{"a",25},{"b",0},{"c",0},{"mode",mode}};
            Assert.True(UsCcssG1FullGradeUnit4FinalQuestions.Valid(family,p));
            Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,ok1));
            Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,ok2));
            Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,bad));
            Assert.Equal(mode==6,ExactSkillContractQuestionEngine.Verify(family,p,"NUMBER=0"));
            Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,"NUMBER=100"));
            Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,ok1+";EXTRA=1"));
            p["a"]=26;
            Assert.False(UsCcssG1FullGradeUnit4FinalQuestions.Valid(family,p));
        }
        var generated=Enumerable.Range(0,1400).Select(i=>
            UsCcssG1FullGradeUnit4FinalQuestions.Build(family,new Random(i*29+17))).ToArray();
        foreach(var mode in new[]{4,5,6})
            Assert.True(generated.Count(q=>q.Parameters["mode"]==mode)>100);
        Assert.All(generated,q=>Assert.True(ExactSkillContractQuestionEngine.Verify(
            family,q.Parameters,UsCcssG1FullGradeUnit4FinalQuestions.Solve(family,q.Parameters))));
    }

    [Fact]
    public void Lesson83SourceFourMentalTensExpressionsAreSeparatelyAndStrictlyVerified()
    {
        const string family="usccss.g1.u04.l18.center_count_compare_order";
        foreach(var (mode,b,expected) in new[]{
            (4,10,"RESULT=42"),
            (5,20,"RESULT=52"),
            (6,20,"RESULT=52"),
            (7,20,"RESULT=12")
        })
        {
            var p=new Dictionary<string,int>{{"a",32},{"b",b},{"c",0},{"mode",mode}};
            Assert.True(UsCcssG1FullGradeUnit4FinalQuestions.Valid(family,p));
            Assert.Equal(expected,UsCcssG1FullGradeUnit4FinalQuestions.Solve(family,p));
            Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,expected));
            Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,"RESULT=99"));
            Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,expected+";EXTRA=1"));
            p["a"]=33;
            Assert.False(UsCcssG1FullGradeUnit4FinalQuestions.Valid(family,p));
            p["a"]=32;
            p["b"]=b+10;
            Assert.False(UsCcssG1FullGradeUnit4FinalQuestions.Valid(family,p));
        }
        var generated=Enumerable.Range(0,2000).Select(i=>
            UsCcssG1FullGradeUnit4FinalQuestions.Build(family,new Random(i*47+19))).ToArray();
        foreach(var mode in new[]{4,5,6,7})
        {
            var modeCases=generated.Where(q=>q.Parameters["mode"]==mode).ToArray();
            Assert.True(modeCases.Length>100);
            Assert.All(modeCases,q=>Assert.True(ExactSkillContractQuestionEngine.Verify(
                family,q.Parameters,UsCcssG1FullGradeUnit4FinalQuestions.Solve(family,q.Parameters))));
        }
    }

    [Fact]
    public void ANonstandardRegroupingKeepsTheSameTwoDigitQuantity()
    {
        const string f="usccss.g1.u04.l19.regroup_one_ten";
        var p=new Dictionary<string,int>{{"a",43},{"b",0},{"c",0},{"mode",2}};
        Assert.Equal("TENS=3;ONES=13",UsCcssG1FullGradeUnit4FinalQuestions.Solve(f,p));
        Assert.True(UsCcssG1FullGradeUnit4FinalQuestions.Verify(f,p,"TENS=3;ONES=13"));
        Assert.False(UsCcssG1FullGradeUnit4FinalQuestions.Verify(f,p,"TENS=4;ONES=13"));
    }
    [Theory]
    [InlineData(94,0,"NUMBER=94;TENS=9;ONES=4;ALT_TENS=8;ALT_ONES=14;WAYS=10")]
    [InlineData(94,2,"NUMBER=94;TENS=9;ONES=4;ALT_TENS=6;ALT_ONES=34;WAYS=10")]
    [InlineData(27,3,"NUMBER=27;TENS=2;ONES=7;ALT_TENS=0;ALT_ONES=27;WAYS=3")]
    public void OriginalU04L20MoreThanOneRegroupingAndTotalWaysAreIndependentlyCorrect(
        int total,int mode,string expected)
    {
        const string f="usccss.g1.u04.l20.equivalent_tens_ones";
        var p=new Dictionary<string,int>{{"a",total},{"b",0},{"c",0},{"mode",mode}};
        Assert.True(UsCcssG1FullGradeUnit4FinalQuestions.Valid(f,p));
        Assert.Equal(expected,UsCcssG1FullGradeUnit4FinalQuestions.Solve(f,p));
        Assert.True(UsCcssG1FullGradeUnit4FinalQuestions.Verify(f,p,expected));
        int moved=Math.Min(total/10,mode+1);
        Assert.Equal(total,10*(total/10-moved)+total%10+10*moved);
        Assert.False(UsCcssG1FullGradeUnit4FinalQuestions.Verify(f,p,
            $"NUMBER={total}"));
        Assert.False(UsCcssG1FullGradeUnit4FinalQuestions.Verify(f,p,
            expected.Replace("WAYS="+(total/10+1),"WAYS="+(total/10))));
    }

    [Theory]
    [InlineData(4,52,"NUMBER=52","BAG A")]
    [InlineData(5,65,"NUMBER=65","BAG B")]
    [InlineData(6,49,"TENS=2","BAG C")]
    [InlineData(7,36,"ONES=16","BAG D")]
    public void EachOriginal2021MysteryBagHasTheCorrectSourceMathAndRejectsSpoofedValues(
        int mode,int total,string answer,string bag)
    {
        const string family="usccss.g1.u04.l20.equivalent_tens_ones";
        var p=new Dictionary<string,int>{
            ["a"]=total,["b"]=0,["c"]=0,["mode"]=mode
        };
        Assert.True(UsCcssG1FullGradeUnit4FinalQuestions.Valid(family,p));
        Assert.Equal(answer,UsCcssG1FullGradeUnit4FinalQuestions.Solve(family,p));
        Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,answer));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,answer+";FALSE=1"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,
            mode==6?"TENS=4":mode==7?"ONES=6":"NUMBER=99"));
        p["a"]++;
        Assert.False(UsCcssG1FullGradeUnit4FinalQuestions.Valid(family,p));
        var found=false;
        for(int seed=0;seed<360;seed++)
        {
            var q=UsCcssG1FullGradeUnit4FinalQuestions.Build(
                family,new Random(seed*41+20261020));
            if(q.Parameters["mode"]!=mode)continue;
            found=true;
            Assert.Contains(bag,q.Prompt);
            Assert.Equal(total,q.Parameters["a"]);
        }
        Assert.True(found,"Original source bag case never generated");
    }

    [Fact]
    public void SourceBagAlternativesDoNotBreakEarlierOpenEndedRegroupingValidation()
    {
        const string family="usccss.g1.u04.l20.equivalent_tens_ones";
        var source=new Dictionary<string,int>{
            ["a"]=94,["b"]=0,["c"]=0,["mode"]=2
        };
        Assert.True(UsCcssG1FullGradeUnit4FinalQuestions.Verify(
            family,source,"NUMBER=94;TENS=9;ONES=4;ALT_TENS=2;ALT_ONES=74;WAYS=10"));
        source["mode"]=6;
        Assert.False(UsCcssG1FullGradeUnit4FinalQuestions.Valid(family,source));
    }

    [Fact]
    public void Original2021SixtyFiveTowersCannotBeBrokenAndThirtySevenHasDifferentValidForms()
    {
        const string family="usccss.g1.u04.l19.regroup_one_ten";
        var source=new Dictionary<string,int>{{"a",65},{"b",0},{"c",0},{"mode",0}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(family,source,
            "TENS=5;ONES=15"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,source,
            "TENS=6;ONES=5"));
        source["mode"]=1;source["a"]=37;
        Assert.True(ExactSkillContractQuestionEngine.Verify(family,source,
            "TENS=3;ONES=7;ALT_TENS=2;ALT_ONES=17"));
        Assert.True(ExactSkillContractQuestionEngine.Verify(family,source,
            "ALT_ONES=27;TENS=3;ALT_TENS=1;ONES=7"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,source,
            "TENS=3;ONES=7;ALT_TENS=3;ALT_ONES=7"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,source,
            "TENS=3;ONES=7;ALT_TENS=2;ALT_ONES=18"));
        source["a"]=65;
        Assert.False(UsCcssG1FullGradeUnit4FinalQuestions.Valid(family,source));
        var seen=Enumerable.Range(0,360).Select(n=>
            UsCcssG1FullGradeUnit4FinalQuestions.Build(family,new Random(n*83+27))).ToArray();
        Assert.Contains(seen,q=>q.Prompt.Contains("FIVE existing ten-towers",StringComparison.Ordinal));
        Assert.Contains(seen,q=>q.Prompt.Contains("represent 37",StringComparison.Ordinal));
        Assert.All(seen,q=>Assert.True(ExactSkillContractQuestionEngine.Verify(
            family,q.Parameters,UsCcssG1FullGradeUnit4FinalQuestions.Solve(family,q.Parameters))));
    }

    [Fact]
    public void OriginalHowManyWaysAcceptsAnyCorrectSecondRepresentationButNoFalseTrade()
    {
        const string family="usccss.g1.u04.l20.equivalent_tens_ones";
        var source=new Dictionary<string,int>{{"a",94},{"b",0},{"c",0},{"mode",2}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(family,source,
            "NUMBER=94;TENS=9;ONES=4;ALT_TENS=8;ALT_ONES=14;WAYS=10"));
        Assert.True(ExactSkillContractQuestionEngine.Verify(family,source,
            "ONES=4;ALT_ONES=74;NUMBER=94;WAYS=10;TENS=9;ALT_TENS=2"));
        Assert.True(ExactSkillContractQuestionEngine.Verify(family,source,
            "NUMBER=94;TENS=9;ONES=4;ALT_TENS=0;ALT_ONES=94;WAYS=10"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,source,
            "NUMBER=94;TENS=9;ONES=4;ALT_TENS=9;ALT_ONES=4;WAYS=10"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,source,
            "NUMBER=94;TENS=9;ONES=4;ALT_TENS=2;ALT_ONES=64;WAYS=10"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,source,
            "NUMBER=94;TENS=9;ONES=4;ALT_TENS=2;ALT_ONES=74;WAYS=9"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,source,
            "NUMBER=94;TENS=9;ONES=4;ALT_TENS=2;ALT_ONES=74;WAYS=10;EXTRA=2"));
    }

    [Fact]
    public void ActualGeneratedSourceModesPresentTheExact94And82Vs72Tasks()
    {
        const string ways="usccss.g1.u04.l20.equivalent_tens_ones";
        const string compare="usccss.g1.u04.l21.compare_nonstandard_forms";
        int actual94=0,actualElena=0;
        for(var seed=0;seed<1000;seed++)
        {
            var q=UsCcssG1FullGradeUnit4FinalQuestions.Build(ways,new Random(101*seed+3));
            if(q.Parameters["mode"]==2)
            {
                actual94++;
                Assert.Equal(94,q.Parameters["a"]);
                Assert.Contains("Start with 9 ten-rods and 4 ones",q.Prompt);
                Assert.Equal(10,94/10+1);
            }
            var second=UsCcssG1FullGradeUnit4FinalQuestions.Build(compare,new Random(71*seed+15));
            if(second.Parameters["mode"]==2)
            {
                actualElena++;
                Assert.Equal(82,second.Parameters["a"]);
                Assert.Equal(72,second.Parameters["b"]);
                Assert.Contains("5 tens and 32 ones with 7 tens and 2 ones",second.Prompt);
                Assert.Equal("SYMBOL=>",
                    UsCcssG1FullGradeUnit4FinalQuestions.Solve(compare,second.Parameters));
            }
        }
        Assert.True(actual94>40);
        Assert.True(actualElena>40);
        var changed=new Dictionary<string,int>{{"a",83},{"b",72},{"c",0},{"mode",2}};
        Assert.False(UsCcssG1FullGradeUnit4FinalQuestions.Valid(compare,changed));
        changed["a"]=94;changed["b"]=0;
        Assert.True(UsCcssG1FullGradeUnit4FinalQuestions.Valid(ways,changed));
        changed["a"]=93;
        Assert.False(UsCcssG1FullGradeUnit4FinalQuestions.Valid(ways,changed));
    }

    [Fact]
    public void OriginalUnit4Lesson21ComparesThirtyTwoLooseOnesCorrectly()
    {
        const string family="usccss.g1.u04.l21.compare_nonstandard_forms";
        var p=new Dictionary<string,int>{{"a",82},{"b",72},{"c",0},{"mode",2}};
        Assert.True(UsCcssG1FullGradeUnit4FinalQuestions.Valid(family,p));
        var generated=UsCcssG1FullGradeUnit4FinalQuestions.Build(family,new Random(193));
        Assert.DoesNotContain("symbol=>",generated.Prompt,StringComparison.OrdinalIgnoreCase);
        // 3 exchanged tens: 82 = five tens plus thirty-two ones, original
        // first-edition source compares that with seven tens and two ones.
        string prompt=$"Compare {82/10-Math.Min(3,82/10)} tens and {82%10+10*Math.Min(3,82/10)} ones with {72/10} tens and {72%10} ones. Write SYMBOL=sign.";
        Assert.Equal("Compare 5 tens and 32 ones with 7 tens and 2 ones. Write SYMBOL=sign.",prompt);
        Assert.Equal("SYMBOL=>",UsCcssG1FullGradeUnit4FinalQuestions.Solve(family,p));
        Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,"SYMBOL=>"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,"SYMBOL=<"));
        var varied=Enumerable.Range(0,180).Select(seed=>
            UsCcssG1FullGradeUnit4FinalQuestions.Build(family,new Random(seed*23+29))).ToArray();
        Assert.Contains(varied,x=>x.Prompt.Contains("32 ones",StringComparison.Ordinal)
            ||x.Prompt.Contains("30 ones",StringComparison.Ordinal)
            ||x.Prompt.Contains("33 ones",StringComparison.Ordinal));
    }

    [Fact]
    public void OriginalStudentFacingTenNumberInsertionTaskHasCorrectOrderedAnswer()
    {
        const string f="usccss.g1.u04.l17.order_three_numbers";
        var p=new Dictionary<string,int>{{"a",1},{"b",5},{"c",10},{"d",50},{"e",99},
            {"f",49},{"g",8},{"h",25},{"i",98},{"j",13},{"mode",3}};
        const string correct="ORDER=1,5,8,10,13,25,49,50,98,99";
        Assert.True(UsCcssG1FullGradeUnit4FinalQuestions.Valid(f,p));
        Assert.Equal(correct,UsCcssG1FullGradeUnit4FinalQuestions.Solve(f,p));
        Assert.True(ExactSkillContractQuestionEngine.Verify(f,p,correct));
        Assert.False(ExactSkillContractQuestionEngine.Verify(f,p,
            "ORDER=1,5,8,10,13,25,50,49,98,99"));
        p["i"]=8;
        Assert.False(UsCcssG1FullGradeUnit4FinalQuestions.Valid(f,p));
    }
    [Fact]
    public void Original2021SourceTenCardsAreActuallyGeneratedAndCannotBeForged()
    {
        const string family="usccss.g1.u04.l17.order_three_numbers";
        var found=0;
        for(int seed=0;seed<400;seed++)
        {
            var problem=UsCcssG1FullGradeUnit4FinalQuestions.Build(
                family,new Random(20261020+seed*71));
            if(problem.Parameters["mode"]!=4)continue;
            found++;
            Assert.Contains("ORIGINAL 2021 STUDENT FACING",problem.Prompt);
            var source=problem.Parameters;
            var sourceCards=new Dictionary<string,int>{
                ["a"]=1,["b"]=5,["c"]=10,["d"]=50,["e"]=99,
                ["f"]=49,["g"]=8,["h"]=25,["i"]=98,["j"]=13
            };
            foreach(var entry in sourceCards)
                Assert.Equal(entry.Value,source[entry.Key]);
            const string correct="ORDER=1,5,8,10,13,25,49,50,98,99";
            Assert.Equal(correct,UsCcssG1FullGradeUnit4FinalQuestions.Solve(family,source));
            Assert.True(ExactSkillContractQuestionEngine.Verify(family,source,correct));
            Assert.False(ExactSkillContractQuestionEngine.Verify(family,source,
                "ORDER=1,5,10,8,13,25,49,50,98,99"));
            var altered=source.ToDictionary(x=>x.Key,x=>x.Value);
            altered["f"]=48;
            Assert.False(UsCcssG1FullGradeUnit4FinalQuestions.Valid(family,altered));
        }
        Assert.True(found>=35,
            "The exact original Student Facing problem must be sampled, not just registered.");
    }
    [Fact]
    public void SourceTenCardsRemainSeparateFromLegacyGeneralizedInsertionMode()
    {
        const string family="usccss.g1.u04.l17.order_three_numbers";
        var source=new Dictionary<string,int>{
            ["a"]=1,["b"]=5,["c"]=10,["d"]=50,["e"]=99,
            ["f"]=49,["g"]=8,["h"]=25,["i"]=98,["j"]=13,["mode"]=3
        };
        Assert.True(UsCcssG1FullGradeUnit4FinalQuestions.Valid(family,source));
        source["f"]=48;
        Assert.True(UsCcssG1FullGradeUnit4FinalQuestions.Valid(family,source));
        source["mode"]=4;
        Assert.False(UsCcssG1FullGradeUnit4FinalQuestions.Valid(family,source));
    }

    [Fact]
    public void NumberInsertionGeneratorProducesTenDistinctSortedAnswers()
    {
        const string f="usccss.g1.u04.l17.order_three_numbers";
        int checkedInserted=0;
        for(int seed=0;seed<700;seed++)
        {
            var q=UsCcssG1FullGradeUnit4FinalQuestions.Build(f,new Random(seed*53+9));
            if(q.Parameters["mode"]!=3)continue;
            checkedInserted++;
            Assert.Equal(11,q.Parameters.Count);
            var answer=UsCcssG1FullGradeUnit4FinalQuestions.Solve(f,q.Parameters);
            Assert.Equal(10,answer["ORDER=".Length..].Split(',').Distinct().Count());
            Assert.Contains("INSERT",q.Prompt);
        }
        Assert.True(checkedInserted>30);
    }
    [Fact]
    public void EngineCanGenerateTenDifferentIndependentItemsPerFamily()
    {
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var family in UsCcssG1FullGradeUnit4FinalQuestions.Families)
        {
            var rows=engine.Generate("g1-u04-complete",family,[family],
                ExactSkillQuestionDifficulty.Standard,10,20261019,[]);
            Assert.Equal(10,rows.Count);
            Assert.Equal(10,rows.Select(x=>x.Prompt).Distinct(StringComparer.Ordinal).Count());
        }
    }
}
