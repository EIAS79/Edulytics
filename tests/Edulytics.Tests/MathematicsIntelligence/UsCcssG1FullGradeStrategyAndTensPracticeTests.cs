using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class UsCcssG1FullGradeStrategyAndTensPracticeTests
{
    [Fact]
    public void NineNewFamiliesHaveRuntimeInteractionContractsAndOnlyScopedLessons()
    {
        Assert.Equal(9,UsCcssG1FullGradeStrategyAndTensQuestions.Families.Count);
        foreach(var family in UsCcssG1FullGradeStrategyAndTensQuestions.Families)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family),family);
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var interaction),family);
            Assert.Equal(LessonPracticeInteractionKind.ContractDriven,interaction!.InteractionKind);
        }
        var reviewed=LessonPracticeContractRegistry.All.Where(x=>
           x.LessonCode.StartsWith("PED:US-CCSS-MATH:G1:U03:L2",StringComparison.Ordinal)||
           x.LessonCode.StartsWith("PED:US-CCSS-MATH:G1:U04:L0",StringComparison.Ordinal))
           .Where(x=>x.AllowedQuestionFamilies.Any(f=>UsCcssG1FullGradeStrategyAndTensQuestions.Supports(f))).ToArray();
        Assert.Equal(9,reviewed.Length);
        Assert.DoesNotContain(reviewed,x=>x.LessonCode=="PED:US-CCSS-MATH:G1:U03:L28");
    }
    [Fact]
    public void NineFamiliesRejectWrongAndMalformedValuesOnHundredsOfSeededInstances()
    {
        foreach(var family in UsCcssG1FullGradeStrategyAndTensQuestions.Families)
        {
            for(var seed=0;seed<160;seed++)
            {
                var item=UsCcssG1FullGradeStrategyAndTensQuestions.Build(family,new Random(seed*43+11));
                Assert.True(UsCcssG1FullGradeStrategyAndTensQuestions.Valid(family,item.Parameters));
                var answer=UsCcssG1FullGradeStrategyAndTensQuestions.Solve(family,item.Parameters);
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,item.Parameters,answer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,item.Parameters,answer+";INVALID=1"));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,item.Parameters,"TOTAL=-1"));
                Assert.False(UsCcssG1FullGradeStrategyAndTensQuestions.GradePersistedAnswer(family,answer,""));
            }
        }
    }
    [Fact]
    public void OriginalCheckItOffStageTwoRejectsTotalsOutsideZeroToNinetyBoard()
    {
        const string family=UsCcssG1FullGradeStrategyAndTensQuestions.CenterTens;
        var tooHigh=new Dictionary<string,int>{{"a",7},{"b",3},{"c",0},{"mode",0}};
        Assert.False(UsCcssG1FullGradeStrategyAndTensQuestions.Valid(family,tooHigh));
        var maximum=new Dictionary<string,int>{{"a",7},{"b",2},{"c",0},{"mode",0}};
        Assert.True(UsCcssG1FullGradeStrategyAndTensQuestions.Valid(family,maximum));
        Assert.True(ExactSkillContractQuestionEngine.Verify(family,maximum,"RESULT=90"));
        var zero=new Dictionary<string,int>{{"a",7},{"b",7},{"c",0},{"mode",1}};
        Assert.True(UsCcssG1FullGradeStrategyAndTensQuestions.Valid(family,zero));
        Assert.True(ExactSkillContractQuestionEngine.Verify(family,zero,"RESULT=0"));
        var generated=Enumerable.Range(0,420)
            .Select(n=>UsCcssG1FullGradeStrategyAndTensQuestions.Build(family,new Random(n*43+19)))
            .Where(q=>q.Parameters["mode"]==0).ToArray();
        Assert.True(generated.Length>50);
        Assert.All(generated,q=>
        {
            var expected=UsCcssG1FullGradeStrategyAndTensQuestions.Solve(family,q.Parameters);
            Assert.True(q.Parameters["a"]+q.Parameters["b"]<=9);
            Assert.True(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,expected));
        });
    }

    [Fact]
    public void Original2021U03L24SetASetBMissingNumberEquationsAreActuallyGraded()
    {
        const string family=UsCcssG1FullGradeStrategyAndTensQuestions.CountOnOrAway;
        var p=new Dictionary<string,int>{{"a",13},{"b",6},{"c",0},{"mode",2}};
        Assert.Equal("UNKNOWN=7;EQUATION=13-6=7",
            UsCcssG1FullGradeStrategyAndTensQuestions.Solve(family,p));
        Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,
            "UNKNOWN=7;EQUATION=13-6=7"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,
            "UNKNOWN=7;EQUATION=13-6=8"));
        p["mode"]=3;
        Assert.Equal("UNKNOWN=7;EQUATION=7+6=13",
            UsCcssG1FullGradeStrategyAndTensQuestions.Solve(family,p));
        Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,
            "UNKNOWN=7;EQUATION=7+6=13"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,
            "UNKNOWN=7;EQUATION=8+6=14"));
        var sourceModes=Enumerable.Range(0,320)
            .Select(n=>UsCcssG1FullGradeStrategyAndTensQuestions.Build(family,new Random(73*n+13)))
            .Where(q=>q.Parameters["mode"]>=2).ToArray();
        Assert.True(sourceModes.Length>100);
        Assert.All(sourceModes,q=>
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                family,q.Parameters,UsCcssG1FullGradeStrategyAndTensQuestions.Solve(family,q.Parameters))));
    }

    [Fact]
    public void Lesson61Full2021SetASetBAndBothSubtractionMethodsAreSourceAccurateAndGradeable()
    {
        const string family=UsCcssG1FullGradeStrategyAndTensQuestions.CountOnOrAway;
        var setA=new[]{
            (a:12,b:7,expected:"UNKNOWN=5;EQUATION=12-7=5"),
            (a:18,b:5,expected:"UNKNOWN=13;EQUATION=18-5=13"),
            (a:14,b:6,expected:"UNKNOWN=8;EQUATION=8=14-6"),
            (a:15,b:12,expected:"UNKNOWN=3;EQUATION=15-12=3"),
            (a:13,b:4,expected:"UNKNOWN=9;EQUATION=13-4=9")
        };
        var setB=new[]{
            (a:18,b:5,expected:"UNKNOWN=13;EQUATION=5+13=18"),
            (a:13,b:6,expected:"UNKNOWN=7;EQUATION=13=7+6"),
            (a:20,b:15,expected:"UNKNOWN=5;EQUATION=20=15+5")
        };
        foreach(var (mode,tasks) in new[]{(mode:4,tasks:setA),(mode:5,tasks:setB)})
            foreach(var (a,b,expected) in tasks)
            {
                var p=new Dictionary<string,int>{{"a",a},{"b",b},{"c",0},{"mode",mode}};
                Assert.True(UsCcssG1FullGradeStrategyAndTensQuestions.Valid(family,p));
                Assert.Equal(expected,UsCcssG1FullGradeStrategyAndTensQuestions.Solve(family,p));
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,expected));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,
                    expected.Replace("UNKNOWN="+(a-b),"UNKNOWN=99")));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,
                    expected+";FAKE=1"));
            }
        var sourceMethods=new[]{(16,3),(17,8),(18,15)};
        foreach(var (a,b) in sourceMethods)
        {
            var p=new Dictionary<string,int>{{"a",a},{"b",b},{"c",0},{"mode",6}};
            Assert.True(UsCcssG1FullGradeStrategyAndTensQuestions.Valid(family,p));
            Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,
                $"DIFFERENCE={a-b};CHECK={a-b}+{b}={a}"));
            Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,
                $"DIFFERENCE={a-b};CHECK={b}+{b}={a}"));
        }
        Assert.False(UsCcssG1FullGradeStrategyAndTensQuestions.Valid(family,
            new Dictionary<string,int>{{"a",15},{"b",9},{"c",0},{"mode",4}}));
        Assert.False(UsCcssG1FullGradeStrategyAndTensQuestions.Valid(family,
            new Dictionary<string,int>{{"a",19},{"b",15},{"c",0},{"mode",5}}));
        Assert.False(UsCcssG1FullGradeStrategyAndTensQuestions.Valid(family,
            new Dictionary<string,int>{{"a",19},{"b",15},{"c",0},{"mode",6}}));
        Assert.False(UsCcssG1FullGradeStrategyAndTensQuestions.Valid(
            UsCcssG1FullGradeStrategyAndTensQuestions.FlexibleSubtract,
            new Dictionary<string,int>{{"a",18},{"b",15},{"c",0},{"mode",6}}));
        var generated=Enumerable.Range(0,840)
            .Select(i=>UsCcssG1FullGradeStrategyAndTensQuestions.Build(family,new Random(i*53+19)))
            .ToArray();
        foreach(var mode in new[]{4,5,6})Assert.True(generated.Count(q=>q.Parameters["mode"]==mode)>60);
        foreach(var (a,b,_) in setA)Assert.Contains(generated,
            q=>q.Parameters["mode"]==4&&q.Parameters["a"]==a&&q.Parameters["b"]==b);
        foreach(var (a,b,_) in setB)Assert.Contains(generated,
            q=>q.Parameters["mode"]==5&&q.Parameters["a"]==a&&q.Parameters["b"]==b);
        foreach(var (a,b) in sourceMethods)Assert.Contains(generated,
            q=>q.Parameters["mode"]==6&&q.Parameters["a"]==a&&q.Parameters["b"]==b);
        Assert.All(generated,q=>
        {
            Assert.True(UsCcssG1FullGradeStrategyAndTensQuestions.Valid(family,q.Parameters));
            Assert.True(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,
                UsCcssG1FullGradeStrategyAndTensQuestions.Solve(family,q.Parameters)));
            if(q.Parameters["mode"]==4)Assert.Contains("ORIGINAL 2021 SET A",q.Prompt);
            if(q.Parameters["mode"]==5)Assert.Contains("ORIGINAL 2021 SET B",q.Prompt);
            if(q.Parameters["mode"]==6)Assert.Contains("DIEGO AND TYLER",q.Prompt);
        });
    }

    [Fact]
    public void StoryModeSupportsStartChangeResultAndSubtractionUnknowns()
    {
        // Use source-approved parameter values for the four original fixed
        // Student Facing variants, and generic values only in remaining modes.
        var cases=new (int A,int B,int Mode,string Expected)[]
        {
            (7,5,0,"UNKNOWN=12;EQUATION=7+5=12"),
            (6,12,1,"UNKNOWN=12;EQUATION=6+12=18"),
            (6,9,2,"UNKNOWN=6;EQUATION=6+9=15"),
            (7,5,3,"UNKNOWN=7;EQUATION=12-5=7"),
            (9,6,4,"UNKNOWN=6;EQUATION=15-6=9"),
            (7,5,5,"UNKNOWN=12;EQUATION=12-5=7")
        };
        foreach(var (first,second,mode,expected) in cases)
        {
            var p=new Dictionary<string,int>{{"a",first},{"b",second},{"c",0},{"mode",mode}};
            const string family=UsCcssG1FullGradeStrategyAndTensQuestions.StoryUnknowns;
            Assert.True(UsCcssG1FullGradeStrategyAndTensQuestions.Valid(family,p));
            Assert.Equal(expected,UsCcssG1FullGradeStrategyAndTensQuestions.Solve(family,p));
            Assert.True(UsCcssG1FullGradeStrategyAndTensQuestions.Verify(family,p,expected));
            Assert.False(UsCcssG1FullGradeStrategyAndTensQuestions.Verify(
                family,p,expected+";UNREVIEWED=1"));
        }
    }
    [Fact]
    public void OriginalElenaAndHanTakeAwayStoryVariantsHaveCorrectUnknownPositions()
    {
        const string family=UsCcssG1FullGradeStrategyAndTensQuestions.StoryUnknowns;
        var elena=new Dictionary<string,int>{{"a",6},{"b",12},{"c",0},{"mode",4}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(family,elena,
            "UNKNOWN=12;EQUATION=18-12=6"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,elena,
            "UNKNOWN=6;EQUATION=18-12=6"));
        elena["a"]=9;elena["b"]=6;
        Assert.True(ExactSkillContractQuestionEngine.Verify(family,elena,
            "UNKNOWN=6;EQUATION=15-6=9"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,elena,
            "UNKNOWN=6;EQUATION=15-5=10"));
        var tasks=Enumerable.Range(0,340).Select(n=>
            UsCcssG1FullGradeStrategyAndTensQuestions.Build(family,new Random(n*71+9))).ToArray();
        Assert.Contains(tasks,q=>q.Prompt.Contains("Elena has 18 counters"));
        Assert.Contains(tasks,q=>q.Prompt.Contains("Han has 15 pencils"));
        Assert.All(tasks,q=>Assert.True(ExactSkillContractQuestionEngine.Verify(
            family,q.Parameters,UsCcssG1FullGradeStrategyAndTensQuestions.Solve(family,q.Parameters))));
    }

    [Fact]
    public void Original2021HanUnknownStartSixPlusNineEqualsFifteenIsReachable()
    {
        const string family=UsCcssG1FullGradeStrategyAndTensQuestions.StoryUnknowns;
        var original=new Dictionary<string,int>{{"a",6},{"b",9},{"c",0},{"mode",2}};
        Assert.True(UsCcssG1FullGradeStrategyAndTensQuestions.Valid(family,original));
        Assert.True(ExactSkillContractQuestionEngine.Verify(family,original,
            "UNKNOWN=6;EQUATION=6+9=15"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,original,
            "UNKNOWN=6;EQUATION=6+9=16"));
        original["b"]=8;
        Assert.False(UsCcssG1FullGradeStrategyAndTensQuestions.Valid(family,original));
        var made=Enumerable.Range(0,360)
            .Select(n=>UsCcssG1FullGradeStrategyAndTensQuestions.Build(
                family,new Random(n*61+37))).Where(q=>q.Parameters["mode"]==2).ToArray();
        Assert.True(made.Length>40);
        Assert.All(made,q=>
        {
            Assert.Equal(6,q.Parameters["a"]);
            Assert.Equal(9,q.Parameters["b"]);
            Assert.Contains("Han has some pencils",q.Prompt);
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                family,q.Parameters,UsCcssG1FullGradeStrategyAndTensQuestions.Solve(family,q.Parameters)));
        });
    }

    [Fact]
    public void Lesson63Original2021HanFourStoryUnknownPositionsAreDistinctAndVerifiable()
    {
        const string family=UsCcssG1FullGradeStrategyAndTensQuestions.StoryUnknowns;
        var sourceHan=new[]{
            (mode:2,a:6,b:9,unknown:6,expected:"UNKNOWN=6;EQUATION=6+9=15"),
            (mode:4,a:9,b:6,unknown:6,expected:"UNKNOWN=6;EQUATION=15-6=9"),
            (mode:6,a:9,b:6,unknown:6,expected:"UNKNOWN=6;EQUATION=9+6=15"),
            (mode:7,a:9,b:6,unknown:6,expected:"UNKNOWN=6;EQUATION=15-9=6")
        };
        foreach(var (mode,a,b,unknown,expected) in sourceHan)
        {
            var p=new Dictionary<string,int>{{"a",a},{"b",b},{"c",0},{"mode",mode}};
            Assert.True(UsCcssG1FullGradeStrategyAndTensQuestions.Valid(family,p));
            Assert.Equal(expected,UsCcssG1FullGradeStrategyAndTensQuestions.Solve(family,p));
            Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,expected));
            Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,
                expected.Replace("UNKNOWN="+unknown,"UNKNOWN=7")));
            Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,
                expected+";UNREVIEWED=1"));
        }
        var elenaAdd=new Dictionary<string,int>{{"a",6},{"b",12},{"c",0},{"mode",1}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(family,elenaAdd,
            "UNKNOWN=12;EQUATION=6+12=18"));
        elenaAdd["mode"]=4;
        Assert.True(ExactSkillContractQuestionEngine.Verify(family,elenaAdd,
            "UNKNOWN=12;EQUATION=18-12=6"));
        Assert.False(UsCcssG1FullGradeStrategyAndTensQuestions.Valid(family,
            new Dictionary<string,int>{{"a",6},{"b",9},{"c",0},{"mode",6}}));
        Assert.False(UsCcssG1FullGradeStrategyAndTensQuestions.Valid(family,
            new Dictionary<string,int>{{"a",10},{"b",5},{"c",0},{"mode",7}}));
        Assert.False(UsCcssG1FullGradeStrategyAndTensQuestions.Valid(family,
            new Dictionary<string,int>{{"a",9},{"b",6},{"c",1},{"mode",7}}));
        Assert.False(UsCcssG1FullGradeStrategyAndTensQuestions.Valid(
            UsCcssG1FullGradeStrategyAndTensQuestions.CountOnOrAway,
            new Dictionary<string,int>{{"a",15},{"b",9},{"c",0},{"mode",7}}));
        var generated=Enumerable.Range(0,960).Select(n=>
            UsCcssG1FullGradeStrategyAndTensQuestions.Build(family,new Random(n*67+31))).ToArray();
        foreach(var mode in new[]{2,4,6,7})
            Assert.True(generated.Count(q=>q.Parameters["mode"]==mode)>60);
        foreach(var mode in new[]{6,7})
            Assert.Contains(generated,q=>q.Parameters["mode"]==mode&&
                q.Parameters["a"]==9&&q.Parameters["b"]==6);
        Assert.All(generated,q=>
        {
            Assert.True(UsCcssG1FullGradeStrategyAndTensQuestions.Valid(family,q.Parameters));
            Assert.True(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,
                UsCcssG1FullGradeStrategyAndTensQuestions.Solve(family,q.Parameters)));
            if(q.Parameters["mode"]==6)Assert.Contains("HAN STORY 3",q.Prompt);
            if(q.Parameters["mode"]==7)Assert.Contains("HAN STORY 4",q.Prompt);
        });
    }

    [Fact]
    public void OriginalHowCloseSubtractFromTwentyChecksTwoAndThreeChosenCards()
    {
        const string family=UsCcssG1FullGradeStrategyAndTensQuestions.CenterWithin20;
        var two=new Dictionary<string,int>{{"a",9},{"b",8},{"c",0},{"mode",0}};
        Assert.True(UsCcssG1FullGradeStrategyAndTensQuestions.Valid(family,two));
        Assert.True(ExactSkillContractQuestionEngine.Verify(family,two,
            "RESULT=3;EQUATION=20-9-8=3"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,two,
            "RESULT=17;EQUATION=9+8=17"));
        var three=new Dictionary<string,int>{{"a",6},{"b",4},{"c",9},{"mode",2}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(family,three,
            "RESULT=1;EQUATION=20-6-4-9=1"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,three,
            "RESULT=1;EQUATION=20-6-4=10"));
        three["c"]=11;
        Assert.False(UsCcssG1FullGradeStrategyAndTensQuestions.Valid(family,three));
        var made=Enumerable.Range(0,400)
            .Select(n=>UsCcssG1FullGradeStrategyAndTensQuestions.Build(
                family,new Random(59*n+41))).ToArray();
        Assert.True(made.Count(q=>q.Parameters["mode"]==0)>50);
        Assert.True(made.Count(q=>q.Parameters["mode"]==2)>50);
        Assert.All(made,q=>
        {
            Assert.True(UsCcssG1FullGradeStrategyAndTensQuestions.Valid(family,q.Parameters));
            Assert.True(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,
                UsCcssG1FullGradeStrategyAndTensQuestions.Solve(family,q.Parameters)));
            if(q.Parameters["mode"]==2)Assert.Contains("THREE number cards",q.Prompt);
        });
    }

    [Fact]
    public void Lesson64SourceCenterZeroCardsDistinctHandAndTwoExpressionComparisonAreCorrect()
    {
        const string f=UsCcssG1FullGradeStrategyAndTensQuestions.CenterWithin20;
        var hand=new Dictionary<string,int>{{"a",6},{"b",4},{"c",9},{"mode",5}};
        Assert.True(UsCcssG1FullGradeStrategyAndTensQuestions.Valid(f,hand));
        Assert.Equal("RESULT=1;EQUATION=20-6-4-9=1",
            UsCcssG1FullGradeStrategyAndTensQuestions.Solve(f,hand));
        Assert.True(ExactSkillContractQuestionEngine.Verify(f,hand,
            "RESULT=1;EQUATION=20-6-4-9=1"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(f,hand,
            "RESULT=1;EQUATION=20-6-4-8=2"));
        hand["c"]=8;
        Assert.False(UsCcssG1FullGradeStrategyAndTensQuestions.Valid(f,hand));
        var comparison=new Dictionary<string,int>{{"a",9},{"b",6},{"c",0},{"mode",4}};
        Assert.True(UsCcssG1FullGradeStrategyAndTensQuestions.Valid(f,comparison));
        Assert.Equal("LEFT=15;RIGHT=13;GREATER=LEFT",
            UsCcssG1FullGradeStrategyAndTensQuestions.Solve(f,comparison));
        Assert.True(ExactSkillContractQuestionEngine.Verify(f,comparison,
            "LEFT=15;RIGHT=13;GREATER=LEFT"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(f,comparison,
            "LEFT=15;RIGHT=13;GREATER=RIGHT"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(f,comparison,
            "LEFT=15;RIGHT=13")); // Missing comparison is not complete.
        Assert.False(ExactSkillContractQuestionEngine.Verify(f,comparison,
            "LEFT=15;RIGHT=13;GREATER=LEFT;FAKE=1"));
        comparison["a"]=10;
        Assert.False(UsCcssG1FullGradeStrategyAndTensQuestions.Valid(f,comparison));
        var subtraction=new Dictionary<string,int>{{"a",19},{"b",6},{"c",0},{"mode",3}};
        Assert.True(UsCcssG1FullGradeStrategyAndTensQuestions.Valid(f,subtraction));
        Assert.True(ExactSkillContractQuestionEngine.Verify(f,subtraction,"RESULT=13"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(f,subtraction,"RESULT=15"));
        var zeroCard=new Dictionary<string,int>{{"a",0},{"b",9},{"c",0},{"mode",0}};
        Assert.True(UsCcssG1FullGradeStrategyAndTensQuestions.Valid(f,zeroCard));
        Assert.Equal("RESULT=11;EQUATION=20-0-9=11",
            UsCcssG1FullGradeStrategyAndTensQuestions.Solve(f,zeroCard));
        zeroCard["b"]=0;
        Assert.False(UsCcssG1FullGradeStrategyAndTensQuestions.Valid(f,zeroCard));
        var zeroThird=new Dictionary<string,int>{{"a",9},{"b",10},{"c",0},{"mode",2}};
        Assert.True(UsCcssG1FullGradeStrategyAndTensQuestions.Valid(f,zeroThird));
        Assert.True(ExactSkillContractQuestionEngine.Verify(f,zeroThird,
            "RESULT=1;EQUATION=20-9-10-0=1"));
        zeroThird["c"]=9;
        Assert.False(UsCcssG1FullGradeStrategyAndTensQuestions.Valid(f,zeroThird));
        zeroThird["c"]=8;
        Assert.False(UsCcssG1FullGradeStrategyAndTensQuestions.Valid(f,zeroThird)); // Negative result.
        var generated=Enumerable.Range(0,960)
            .Select(n=>UsCcssG1FullGradeStrategyAndTensQuestions.Build(f,new Random(47*n+29)))
            .ToArray();
        Assert.True(generated.Count(q=>q.Parameters["mode"]==4)>60);
        Assert.True(generated.Count(q=>q.Parameters["mode"]==5)>60);
        Assert.All(generated,q=>
        {
            Assert.True(UsCcssG1FullGradeStrategyAndTensQuestions.Valid(f,q.Parameters));
            Assert.True(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,
                UsCcssG1FullGradeStrategyAndTensQuestions.Solve(f,q.Parameters)));
            var p=q.Parameters;
            if(p["mode"]==0)Assert.NotEqual(p["a"],p["b"]);
            if(p["mode"]==2)
            {
                Assert.NotEqual(p["a"],p["b"]);
                Assert.NotEqual(p["a"],p["c"]);
                Assert.NotEqual(p["b"],p["c"]);
                Assert.InRange(p["a"]+p["b"]+p["c"],0,20);
            }
            if(p["mode"]==4)Assert.Contains("9+6",q.Prompt);
            if(p["mode"]==5)Assert.Contains("6, 4, 9 and 8",q.Prompt);
        });
    }

    [Fact]
    public void Lesson66Original2021FortyObjectsAndAllSixTensBagsHaveExactMathContracts()
    {
        const string family=UsCcssG1FullGradeStrategyAndTensQuestions.CountTens;
        var forty=new Dictionary<string,int>{{"a",4},{"b",0},{"c",0},{"mode",0}};
        Assert.True(UsCcssG1FullGradeStrategyAndTensQuestions.Valid(family,forty));
        Assert.Equal("GROUPS=4;TOTAL=40",UsCcssG1FullGradeStrategyAndTensQuestions.Solve(family,forty));
        Assert.True(ExactSkillContractQuestionEngine.Verify(family,forty,"GROUPS=4;TOTAL=40"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,forty,"GROUPS=4;TOTAL=4"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,forty,"GROUPS=40;TOTAL=40"));
        forty["a"]=5;
        Assert.False(UsCcssG1FullGradeStrategyAndTensQuestions.Valid(family,forty));
        var sourceBags=new[]{(letter:"F",tens:1),(letter:"A",tens:2),
            (letter:"E",tens:3),(letter:"B",tens:4),(letter:"C",tens:5),
            (letter:"D",tens:6)};
        foreach(var (letter,tens) in sourceBags)
        {
            var parameters=new Dictionary<string,int>{{"a",tens},{"b",0},{"c",0},{"mode",1}};
            Assert.True(UsCcssG1FullGradeStrategyAndTensQuestions.Valid(family,parameters));
            Assert.Equal($"GROUPS={tens};TOTAL={10*tens}",
                UsCcssG1FullGradeStrategyAndTensQuestions.Solve(family,parameters));
            Assert.True(ExactSkillContractQuestionEngine.Verify(family,parameters,
                $"GROUPS={tens};TOTAL={10*tens}"));
            Assert.False(ExactSkillContractQuestionEngine.Verify(family,parameters,
                $"GROUPS={tens};TOTAL={tens}"));
            var found=Enumerable.Range(0,800).Select(i=>
                UsCcssG1FullGradeStrategyAndTensQuestions.Build(family,new Random(i*43+11)))
                .First(q=>q.Parameters["mode"]==1&&q.Parameters["a"]==tens);
            Assert.Contains($"Bag {letter}",found.Prompt);
            Assert.Contains($"exactly {10*tens} objects",found.Prompt);
            Assert.DoesNotContain("automatically certified",found.Prompt);
        }
        foreach(var invalid in new[]{
            new Dictionary<string,int>{{"a",0},{"b",0},{"c",0},{"mode",1}},
            new Dictionary<string,int>{{"a",7},{"b",0},{"c",0},{"mode",1}},
            new Dictionary<string,int>{{"a",6},{"b",1},{"c",0},{"mode",1}},
            new Dictionary<string,int>{{"a",6},{"b",0},{"c",1},{"mode",1}},
            new Dictionary<string,int>{{"a",4},{"b",0},{"c",0},{"mode",4}}
        })
            Assert.False(UsCcssG1FullGradeStrategyAndTensQuestions.Valid(family,invalid));
        var generated=Enumerable.Range(0,600).Select(i=>
            UsCcssG1FullGradeStrategyAndTensQuestions.Build(family,new Random(i*67+29)))
            .ToArray();
        Assert.Contains(generated,q=>q.Parameters["mode"]==0);
        Assert.Contains(generated,q=>q.Parameters["mode"]==1);
        Assert.All(generated,q=>
        {
            Assert.True(UsCcssG1FullGradeStrategyAndTensQuestions.Valid(family,q.Parameters));
            Assert.True(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,
                UsCcssG1FullGradeStrategyAndTensQuestions.Solve(family,q.Parameters)));
            Assert.False(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,
                UsCcssG1FullGradeStrategyAndTensQuestions.Solve(family,q.Parameters)+";FABRICATED=1"));
        });
    }

    [Fact]
    public void Lesson67MatchesEveryTenThroughNinetyAndGradesOwnedSeventyExit()
    {
        const string family=UsCcssG1FullGradeStrategyAndTensQuestions.MatchTens;
        for(int tens=1;tens<=9;tens++)
        {
            for(int mode=0;mode<=3;mode++)
            {
                var p=new Dictionary<string,int>{{"a",tens},{"b",0},{"c",0},{"mode",mode}};
                Assert.True(UsCcssG1FullGradeStrategyAndTensQuestions.Valid(family,p));
                var solved=UsCcssG1FullGradeStrategyAndTensQuestions.Solve(family,p);
                Assert.Contains($"TENS={tens}",solved);
                Assert.Contains($"NUMBER={tens*10}",solved);
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,solved));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,
                    solved.Replace($"NUMBER={tens*10}",$"NUMBER={tens}")));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,solved+";FAKE=1"));
            }
        }
        var exit=new Dictionary<string,int>{{"a",7},{"b",0},{"c",0},{"mode",4}};
        Assert.True(UsCcssG1FullGradeStrategyAndTensQuestions.Valid(family,exit));
        const string correct="TENS=7;NUMBER=70;WORD=SEVENTY";
        Assert.Equal(correct,UsCcssG1FullGradeStrategyAndTensQuestions.Solve(family,exit));
        Assert.True(ExactSkillContractQuestionEngine.Verify(family,exit,correct));
        Assert.True(ExactSkillContractQuestionEngine.Verify(family,exit,
            "WORD=seventy;NUMBER=70;TENS=7"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,exit,
            "TENS=7;NUMBER=70")); // All three representations must be checked.
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,exit,
            "TENS=7;NUMBER=70;WORD=SIXTY"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,exit,
            "TENS=7;NUMBER=7;WORD=SEVENTY"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,exit,
            correct+";TENS=7"));
        exit["a"]=8;
        Assert.False(UsCcssG1FullGradeStrategyAndTensQuestions.Valid(family,exit));
        exit["a"]=7;
        exit["b"]=1;
        Assert.False(UsCcssG1FullGradeStrategyAndTensQuestions.Valid(family,exit));
        Assert.False(UsCcssG1FullGradeStrategyAndTensQuestions.Valid(
            UsCcssG1FullGradeStrategyAndTensQuestions.CountTens,exit));
        var generated=Enumerable.Range(0,950).Select(i=>
            UsCcssG1FullGradeStrategyAndTensQuestions.Build(family,new Random(43*i+19)))
            .ToArray();
        Assert.True(generated.Count(q=>q.Parameters["mode"]==4)>80);
        Assert.All(generated,q=>
        {
            Assert.True(UsCcssG1FullGradeStrategyAndTensQuestions.Valid(family,q.Parameters));
            Assert.True(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,
                UsCcssG1FullGradeStrategyAndTensQuestions.Solve(family,q.Parameters)));
            if(q.Parameters["mode"]==4)
            {
                Assert.Equal(7,q.Parameters["a"]);
                Assert.Contains("EDULYTICS ORIGINAL EXIT CHECK",q.Prompt);
                Assert.DoesNotContain("ORIGINAL 2021",q.Prompt);
            }
        });
    }

    [Fact]
    public void Lesson68Original2021NineTensSequencesCardBoundariesAndHanSixtyQuestion()
    {
        const string family=UsCcssG1FullGradeStrategyAndTensQuestions.AddRemoveTen;
        var addSeries=new Dictionary<string,int>{{"a",9},{"b",1},{"c",0},{"mode",2}};
        Assert.True(UsCcssG1FullGradeStrategyAndTensQuestions.Valid(family,addSeries));
        const string addExpected="TOTALS=20,30,40,50,60,70,80,90;LAST=90";
        Assert.True(ExactSkillContractQuestionEngine.Verify(family,addSeries,addExpected));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,addSeries,"TOTALS=20,30,40,50,60,70,80;LAST=90"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,addSeries,"TOTALS=20,30,40,50,60,70,80,100;LAST=100"));
        var subtractSeries=new Dictionary<string,int>{{"a",1},{"b",1},{"c",0},{"mode",3}};
        Assert.True(UsCcssG1FullGradeStrategyAndTensQuestions.Valid(family,subtractSeries));
        const string subtractExpected="TOTALS=80,70,60,50,40,30,20,10;LAST=10";
        Assert.True(ExactSkillContractQuestionEngine.Verify(family,subtractSeries,subtractExpected));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,subtractSeries,"TOTALS=90,80,70,60,50,40,30,20;LAST=20"));
        var cardZero=new Dictionary<string,int>{{"a",0},{"b",1},{"c",0},{"mode",0}};
        Assert.True(UsCcssG1FullGradeStrategyAndTensQuestions.Valid(family,cardZero));
        Assert.True(ExactSkillContractQuestionEngine.Verify(family,cardZero,"TENS=1;TOTAL=10"));
        cardZero["mode"]=1;
        Assert.False(UsCcssG1FullGradeStrategyAndTensQuestions.Valid(family,cardZero));
        var cardNinety=new Dictionary<string,int>{{"a",9},{"b",1},{"c",0},{"mode",1}};
        Assert.True(UsCcssG1FullGradeStrategyAndTensQuestions.Valid(family,cardNinety));
        Assert.True(ExactSkillContractQuestionEngine.Verify(family,cardNinety,"TENS=8;TOTAL=80"));
        cardNinety["mode"]=0; // Cannot assert an unverified 100 gameboard cell.
        Assert.False(UsCcssG1FullGradeStrategyAndTensQuestions.Valid(family,cardNinety));
        var han=new Dictionary<string,int>{{"a",6},{"b",1},{"c",0},{"mode",4}};
        Assert.True(UsCcssG1FullGradeStrategyAndTensQuestions.Valid(family,han));
        Assert.Equal("ADD=70;SUBTRACT=50",
            UsCcssG1FullGradeStrategyAndTensQuestions.Solve(family,han));
        Assert.True(ExactSkillContractQuestionEngine.Verify(family,han,"ADD=70;SUBTRACT=50"));
        Assert.True(ExactSkillContractQuestionEngine.Verify(family,han,"SUBTRACT=50;ADD=70"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,han,"ADD=70"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,han,"ADD=70;SUBTRACT=60"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,han,"ADD=70;SUBTRACT=50;FAKE=1"));
        han["a"]=5;
        Assert.False(UsCcssG1FullGradeStrategyAndTensQuestions.Valid(family,han));
        han["a"]=6;han["c"]=1;
        Assert.False(UsCcssG1FullGradeStrategyAndTensQuestions.Valid(family,han));
        var generated=Enumerable.Range(0,980)
            .Select(i=>UsCcssG1FullGradeStrategyAndTensQuestions.Build(family,new Random(i*53+17)))
            .ToArray();
        for(int mode=0;mode<=4;mode++)
            Assert.True(generated.Count(q=>q.Parameters["mode"]==mode)>60);
        Assert.Contains(generated,q=>q.Parameters["mode"]==0&&q.Parameters["a"]==0);
        Assert.Contains(generated,q=>q.Parameters["mode"]==1&&q.Parameters["a"]==9);
        Assert.All(generated,q=>
        {
            Assert.True(UsCcssG1FullGradeStrategyAndTensQuestions.Valid(family,q.Parameters));
            Assert.True(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,
                UsCcssG1FullGradeStrategyAndTensQuestions.Solve(family,q.Parameters)));
            if(q.Parameters["mode"]==4)Assert.Contains("Han",q.Prompt);
        });
    }

    [Fact]
    public void Lesson69ExactSixSourceTensEquationsAndOriginalEdulyticsInverseExit()
    {
        const string family=UsCcssG1FullGradeStrategyAndTensQuestions.AddRemoveMultipleTens;
        var original=new[]{
            (a:4,b:5,mode:4,total:90,tens:9),
            (a:2,b:6,mode:4,total:80,tens:8),
            (a:4,b:4,mode:4,total:80,tens:8),
            (a:8,b:3,mode:5,total:50,tens:5),
            (a:7,b:2,mode:5,total:50,tens:5),
            (a:9,b:7,mode:5,total:20,tens:2)
        };
        foreach(var (a,b,mode,total,tens) in original)
        {
            var p=new Dictionary<string,int>{{"a",a},{"b",b},{"c",0},{"mode",mode}};
            string expected=$"TENS={tens};TOTAL={total}";
            Assert.True(UsCcssG1FullGradeStrategyAndTensQuestions.Valid(family,p));
            Assert.Equal(expected,UsCcssG1FullGradeStrategyAndTensQuestions.Solve(family,p));
            Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,expected));
            Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,
                $"TENS={tens};TOTAL={tens}"));
            Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,
                expected+";EXTRA=1"));
        }
        var jada=new Dictionary<string,int>{{"a",3},{"b",2},{"c",0},{"mode",0}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(family,jada,"TENS=5;TOTAL=50"));
        var tyler=new Dictionary<string,int>{{"a",7},{"b",4},{"c",0},{"mode",1}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(family,tyler,"TENS=3;TOTAL=30"));
        var exit=new Dictionary<string,int>{{"a",5},{"b",3},{"c",0},{"mode",6}};
        Assert.True(UsCcssG1FullGradeStrategyAndTensQuestions.Valid(family,exit));
        Assert.True(ExactSkillContractQuestionEngine.Verify(family,exit,
            "ADDITION=80;SUBTRACTION=50"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,exit,
            "ADDITION=80"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,exit,
            "ADDITION=80;SUBTRACTION=60"));
        exit["a"]=6;
        Assert.False(UsCcssG1FullGradeStrategyAndTensQuestions.Valid(family,exit));
        Assert.False(UsCcssG1FullGradeStrategyAndTensQuestions.Valid(family,
            new Dictionary<string,int>{{"a",8},{"b",4},{"c",0},{"mode",4}}));
        Assert.False(UsCcssG1FullGradeStrategyAndTensQuestions.Valid(family,
            new Dictionary<string,int>{{"a",7},{"b",3},{"c",0},{"mode",5}}));
        var sampled=Enumerable.Range(0,1190).Select(i=>
            UsCcssG1FullGradeStrategyAndTensQuestions.Build(family,new Random(37*i+71)))
            .ToArray();
        for(int mode=0;mode<=6;mode++)
            Assert.True(sampled.Count(q=>q.Parameters["mode"]==mode)>80);
        foreach(var (a,b,mode,_,_) in original)
            Assert.Contains(sampled,q=>q.Parameters["mode"]==mode&&
                q.Parameters["a"]==a&&q.Parameters["b"]==b);
        Assert.All(sampled,q=>
        {
            Assert.True(UsCcssG1FullGradeStrategyAndTensQuestions.Valid(family,q.Parameters));
            Assert.True(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,
                UsCcssG1FullGradeStrategyAndTensQuestions.Solve(family,q.Parameters)));
            if(q.Parameters["mode"]==6)
            {
                Assert.Contains("EDULYTICS ORIGINAL EXIT CHECK",q.Prompt);
                Assert.DoesNotContain("ORIGINAL 2021 ACTIVITY",q.Prompt);
            }
        });
    }

    [Fact]
    public void Original2021TwelvePencilsMinusSevenStoryIsGeneratedAndGradeable()
    {
        const string family=UsCcssG1FullGradeStrategyAndTensQuestions.FlexibleSubtract;
        var p=new Dictionary<string,int>{{"a",12},{"b",7},{"c",0},{"mode",2}};
        Assert.True(UsCcssG1FullGradeStrategyAndTensQuestions.Valid(family,p));
        Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,
            "DIFFERENCE=5;CHECK=5+7=12"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,
            "DIFFERENCE=6;CHECK=6+7=13"));
        p["a"]=13;
        Assert.False(UsCcssG1FullGradeStrategyAndTensQuestions.Valid(family,p));
        var generated=Enumerable.Range(0,330)
            .Select(n=>UsCcssG1FullGradeStrategyAndTensQuestions.Build(family,new Random(67*n+11)))
            .Where(q=>q.Parameters["mode"]==2).ToArray();
        Assert.True(generated.Length>40);
        Assert.All(generated,q=>
        {
            Assert.Contains("12 pencils",q.Prompt);
            Assert.Equal(12,q.Parameters["a"]);
            Assert.Equal(7,q.Parameters["b"]);
            Assert.True(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,
                UsCcssG1FullGradeStrategyAndTensQuestions.Solve(family,q.Parameters)));
        });
    }

    [Fact]
    public void Lesson62Exact2021FlexibleSubtractionUnknownStoriesAndOwnedExitCheck()
    {
        const string family=UsCcssG1FullGradeStrategyAndTensQuestions.FlexibleSubtract;
        var sourceExpressions=new[]{
            (a:20,b:15,answer:5),(a:19,b:3,answer:16),(a:13,b:5,answer:8),
            (a:18,b:9,answer:9),(a:17,b:15,answer:2)
        };
        foreach(var (a,b,difference) in sourceExpressions)
        {
            var p=new Dictionary<string,int>{{"a",a},{"b",b},{"c",0},{"mode",4}};
            var expected=$"DIFFERENCE={difference};CHECK={difference}+{b}={a}";
            Assert.True(UsCcssG1FullGradeStrategyAndTensQuestions.Valid(family,p));
            Assert.Equal(expected,UsCcssG1FullGradeStrategyAndTensQuestions.Solve(family,p));
            Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,expected));
            Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,
                $"DIFFERENCE={difference};CHECK={difference}+{b}={a+1}"));
            Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,
                $"DIFFERENCE={difference+1};CHECK={difference}+{b}={a}"));
        }
        var clare=new Dictionary<string,int>{{"a",15},{"b",8},{"c",0},{"mode",5}};
        Assert.Equal("UNKNOWN=7;EQUATION=8+7=15",
            UsCcssG1FullGradeStrategyAndTensQuestions.Solve(family,clare));
        Assert.True(ExactSkillContractQuestionEngine.Verify(family,clare,
            "UNKNOWN=7;EQUATION=8+7=15"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,clare,
            "UNKNOWN=8;EQUATION=8+7=15"));
        var kiran=new Dictionary<string,int>{{"a",17},{"b",9},{"c",0},{"mode",6}};
        Assert.Equal("UNKNOWN=8;EQUATION=17-8=9",
            UsCcssG1FullGradeStrategyAndTensQuestions.Solve(family,kiran));
        Assert.True(ExactSkillContractQuestionEngine.Verify(family,kiran,
            "UNKNOWN=8;EQUATION=17-8=9"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,kiran,
            "UNKNOWN=9;EQUATION=17-9=8"));
        var exit=new Dictionary<string,int>{{"a",16},{"b",7},{"c",0},{"mode",7}};
        Assert.True(UsCcssG1FullGradeStrategyAndTensQuestions.Valid(family,exit));
        Assert.Equal("DIFFERENCE=9;CHECK=9+7=16",
            UsCcssG1FullGradeStrategyAndTensQuestions.Solve(family,exit));
        Assert.True(ExactSkillContractQuestionEngine.Verify(family,exit,
            "DIFFERENCE=9;CHECK=9+7=16"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,exit,
            "DIFFERENCE=9")); // Bare result is not the checked numeric contract.
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,exit,
            "DIFFERENCE=9;CHECK=9+7=17"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,exit,
            "DIFFERENCE=9;CHECK=9+7=16;EXTRA=1"));
        exit["a"]=14;exit["b"]=9;
        Assert.False(UsCcssG1FullGradeStrategyAndTensQuestions.Valid(family,exit));
        var incorrect=new Dictionary<string,int>{{"a",20},{"b",14},{"c",0},{"mode",4}};
        Assert.False(UsCcssG1FullGradeStrategyAndTensQuestions.Valid(family,incorrect));
        incorrect["mode"]=5;
        Assert.False(UsCcssG1FullGradeStrategyAndTensQuestions.Valid(family,incorrect));
        var generated=Enumerable.Range(0,960)
            .Select(n=>UsCcssG1FullGradeStrategyAndTensQuestions.Build(family,new Random(n*59+17)))
            .ToArray();
        foreach(var mode in new[]{4,5,6,7})
            Assert.True(generated.Count(q=>q.Parameters["mode"]==mode)>65);
        foreach(var (a,b,_) in sourceExpressions)
            Assert.Contains(generated,q=>q.Parameters["mode"]==4&&
                q.Parameters["a"]==a&&q.Parameters["b"]==b);
        Assert.All(generated,q=>
        {
            Assert.True(UsCcssG1FullGradeStrategyAndTensQuestions.Valid(family,q.Parameters));
            Assert.True(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,
                UsCcssG1FullGradeStrategyAndTensQuestions.Solve(family,q.Parameters)));
            if(q.Parameters["mode"]==5)Assert.Contains("CLARE STORY",q.Prompt);
            if(q.Parameters["mode"]==6)Assert.Contains("KIRAN STORY",q.Prompt);
            if(q.Parameters["mode"]==7)
            {
                Assert.Contains("EDULYTICS ORIGINAL EXIT CHECK",q.Prompt);
                Assert.DoesNotContain("ORIGINAL 2021",q.Prompt);
            }
        });
    }

    [Fact]
    public void Source2021ElenaSixToEighteenUnknownChangeMustBeReachableAndCorrect()
    {
        const string family=UsCcssG1FullGradeStrategyAndTensQuestions.StoryUnknowns;
        var p=new Dictionary<string,int>{{"a",6},{"b",12},{"c",0},{"mode",1}};
        Assert.True(UsCcssG1FullGradeStrategyAndTensQuestions.Valid(family,p));
        Assert.Equal("UNKNOWN=12;EQUATION=6+12=18",
            UsCcssG1FullGradeStrategyAndTensQuestions.Solve(family,p));
        Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,
            "UNKNOWN=12;EQUATION=6+12=18"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,
            "UNKNOWN=11;EQUATION=6+11=17"));
        var seen=Enumerable.Range(0,350)
            .Select(n=>UsCcssG1FullGradeStrategyAndTensQuestions.Build(family,new Random(n*71+7)))
            .Where(q=>q.Parameters["mode"]==1).ToArray();
        Assert.True(seen.Length>30);
        Assert.All(seen,q=>
        {
            Assert.Equal(6,q.Parameters["a"]);
            Assert.Equal(12,q.Parameters["b"]);
            Assert.Contains("Elena has 6 counters",q.Prompt);
            Assert.True(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,
                UsCcssG1FullGradeStrategyAndTensQuestions.Solve(family,q.Parameters)));
        });
        p["b"]=13;
        Assert.False(UsCcssG1FullGradeStrategyAndTensQuestions.Valid(family,p));
    }

    [Fact]
    public void TensAreNeverMistakenForSingleOnesOrNegativeOne()
    {
        var add=new Dictionary<string,int>{{"a",4},{"b",1},{"c",0},{"mode",0}};
        Assert.Equal("TENS=5;TOTAL=50",UsCcssG1FullGradeStrategyAndTensQuestions.Solve(
          UsCcssG1FullGradeStrategyAndTensQuestions.AddRemoveTen,add));
        add["mode"]=1;
        Assert.Equal("TENS=3;TOTAL=30",UsCcssG1FullGradeStrategyAndTensQuestions.Solve(
          UsCcssG1FullGradeStrategyAndTensQuestions.AddRemoveTen,add));
        Assert.False(UsCcssG1FullGradeStrategyAndTensQuestions.Verify(
          UsCcssG1FullGradeStrategyAndTensQuestions.AddRemoveTen,add,"TENS=3;TOTAL=3"));
        Assert.False(UsCcssG1FullGradeStrategyAndTensQuestions.Valid(
          UsCcssG1FullGradeStrategyAndTensQuestions.AddRemoveTen,
          new Dictionary<string,int>{{"a",2},{"b",3},{"c",0},{"mode",1}}));
    }
    [Theory]
    [InlineData(5,4,4,"MOVE=SUBTRACT;COVER=10;EQUATION=50-40=10")]
    [InlineData(2,1,5,"MOVE=ADD;COVER=30;EQUATION=20+10=30")]
    [InlineData(3,2,5,"MOVE=ADD;COVER=50;EQUATION=30+20=50")]
    [InlineData(5,4,5,"MOVE=ADD;COVER=90;EQUATION=50+40=90")]
    public void Original2021U04L05CheckItOffRequiresChoosingTheActuallyAvailableBoardCell(
        int firstCardTens,int secondCardTens,int mode,string expected)
    {
        const string family=UsCcssG1FullGradeStrategyAndTensQuestions.CenterTens;
        var p=new Dictionary<string,int>{
            ["a"]=firstCardTens,["b"]=secondCardTens,["c"]=0,["mode"]=mode};
        Assert.True(UsCcssG1FullGradeStrategyAndTensQuestions.Valid(family,p));
        Assert.Equal(expected,UsCcssG1FullGradeStrategyAndTensQuestions.Solve(family,p));
        Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,expected));
        string wrongOperation=mode==4?
            expected.Replace("MOVE=SUBTRACT","MOVE=ADD"):
            expected.Replace("MOVE=ADD","MOVE=SUBTRACT");
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,wrongOperation));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,expected+";FAKE=1"));
    }

    [Fact]
    public void OriginalCheckItOffCardLimitsRejectFakeOptionsOutsideTheGameBoard()
    {
        const string family=UsCcssG1FullGradeStrategyAndTensQuestions.CenterTens;
        foreach(var (a,b,mode) in new[]{
            (4,5,4),(5,3,4),(7,3,5),(8,1,5),
            (4,0,5),(4,6,5),(5,4,6)
        })
            Assert.False(UsCcssG1FullGradeStrategyAndTensQuestions.Valid(family,
                new Dictionary<string,int>{
                    ["a"]=a,["b"]=b,["c"]=0,["mode"]=mode}));
        var seen=new HashSet<int>();
        for(int seed=0;seed<350;seed++)
        {
            var p=UsCcssG1FullGradeStrategyAndTensQuestions.Build(
                family,new Random(seed*41+31));
            seen.Add(p.Parameters["mode"]);
            Assert.True(UsCcssG1FullGradeStrategyAndTensQuestions.Valid(
                family,p.Parameters));
            if(p.Parameters["mode"]==4)
            {
                Assert.Contains("40 and 50",p.Prompt);
                Assert.Contains("90 box",p.Prompt);
                Assert.Contains("10 box",p.Prompt);
            }
            if(p.Parameters["mode"]==5)
                Assert.Contains("NOT yet checked",p.Prompt);
        }
        Assert.Equal(6,seen.Count);
    }

    [Theory]
    [InlineData(1,"F")]
    [InlineData(2,"A")]
    [InlineData(3,"E")]
    [InlineData(4,"B")]
    [InlineData(5,"C")]
    [InlineData(6,"D")]
    public void Original2021U04L01NamedBagsCountExactlyTheSourceMultiplesOfTen(int tens,string bag)
    {
        const string family=UsCcssG1FullGradeStrategyAndTensQuestions.CountTens;
        var p=new Dictionary<string,int>{{"a",tens},{"b",0},{"c",0},{"mode",1}};
        Assert.True(UsCcssG1FullGradeStrategyAndTensQuestions.Valid(family,p));
        string expected=$"GROUPS={tens};TOTAL={tens*10}";
        Assert.Equal(expected,UsCcssG1FullGradeStrategyAndTensQuestions.Solve(family,p));
        Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,expected));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,
            $"GROUPS={tens};TOTAL={tens*10+10}"));
        // Exercise the actual lesson generator, including original bag text.
        var q=Enumerable.Range(0,3000)
            .Select(seed=>UsCcssG1FullGradeStrategyAndTensQuestions.Build(
                family,new Random(seed*41+13)))
            .First(x=>x.Parameters["mode"]==1&&x.Parameters["a"]==tens);
        Assert.Contains($"Bag {bag}",q.Prompt);
        Assert.Contains($"{tens*10} objects",q.Prompt);
    }

    [Fact]
    public void Original2021U04L01FirstSourceActivityUsesExactlyFortyObjects()
    {
        const string family=UsCcssG1FullGradeStrategyAndTensQuestions.CountTens;
        var original=new Dictionary<string,int>{{"a",4},{"b",0},{"c",0},{"mode",0}};
        Assert.True(UsCcssG1FullGradeStrategyAndTensQuestions.Valid(family,original));
        Assert.Equal("GROUPS=4;TOTAL=40",
            UsCcssG1FullGradeStrategyAndTensQuestions.Solve(family,original));
        foreach(int altered in new[]{1,2,3,5,6})
        {
            original["a"]=altered;
            Assert.False(UsCcssG1FullGradeStrategyAndTensQuestions.Valid(family,original));
        }
        var seen=new HashSet<int>();
        for(int seed=0;seed<300;seed++)
        {
            var q=UsCcssG1FullGradeStrategyAndTensQuestions.Build(
                family,new Random(seed*31+11));
            seen.Add(q.Parameters["mode"]);
            if(q.Parameters["mode"]==0)
            {
                Assert.Equal(4,q.Parameters["a"]);
                Assert.Contains("40 objects",q.Prompt);
            }
        }
        Assert.Equal(4,seen.Count);
    }

    [Theory]
    [InlineData(9,1,2,"TOTALS=20,30,40,50,60,70,80,90;LAST=90")]
    [InlineData(1,1,3,"TOTALS=80,70,60,50,40,30,20,10;LAST=10")]
    [InlineData(4,1,0,"TENS=5;TOTAL=50")]
    [InlineData(4,1,1,"TENS=3;TOTAL=30")]
    public void OriginalU04L03ShowsEightSuccessiveTensChangesAndCorrectAnswers(
        int a,int b,int mode,string expected)
    {
        const string family=UsCcssG1FullGradeStrategyAndTensQuestions.AddRemoveTen;
        var p=new Dictionary<string,int>{
            ["a"]=a,["b"]=b,["c"]=0,["mode"]=mode};
        Assert.True(UsCcssG1FullGradeStrategyAndTensQuestions.Valid(family,p));
        Assert.Equal(expected,UsCcssG1FullGradeStrategyAndTensQuestions.Solve(family,p));
        Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,expected));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,
            expected.Replace(mode>=2?"TOTALS=":"TENS=",mode>=2?"TOTALS=99,":"TENS=99")));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,expected+";FICTITIOUS=1"));
    }

    [Fact]
    public void OriginalU04L03SequenceEndpointsCannotBeSilentlyReplaced()
    {
        const string family=UsCcssG1FullGradeStrategyAndTensQuestions.AddRemoveTen;
        foreach(var (a,mode) in new[]{
            (8,2),(1,2),(9,3),(2,3),(0,3),(9,0),(1,1)
        })
            Assert.False(UsCcssG1FullGradeStrategyAndTensQuestions.Valid(family,
                new Dictionary<string,int>{
                    ["a"]=a,["b"]=1,["c"]=0,["mode"]=mode}));
        var covered=new HashSet<int>();
        for(int seed=0;seed<350;seed++)
        {
            var q=UsCcssG1FullGradeStrategyAndTensQuestions.Build(
                family,new Random(seed*37+29));
            covered.Add(q.Parameters["mode"]);
            if(q.Parameters["mode"]>=2)
            {
                Assert.Contains("EIGHT totals",q.Prompt);
                Assert.Contains("1 TEN",q.Prompt);
                Assert.True(ExactSkillContractQuestionEngine.Verify(
                    family,q.Parameters,
                    UsCcssG1FullGradeStrategyAndTensQuestions.Solve(family,q.Parameters)));
            }
        }
        Assert.Equal(4,covered.Count);
    }

    [Theory]
    [InlineData(1,"B","ten")]
    [InlineData(2,"C","twenty")]
    [InlineData(3,"A","thirty")]
    [InlineData(4,"B","forty")]
    [InlineData(5,"C","fifty")]
    [InlineData(6,"A","sixty")]
    [InlineData(7,"B","seventy")]
    [InlineData(8,"C","eighty")]
    [InlineData(9,"A","ninety")]
    public void Original2021U04L02SourceWordCardsAreRecognizedWithoutSpelling(
        int tens,string correctCard,string correctWord)
    {
        const string family=UsCcssG1FullGradeStrategyAndTensQuestions.MatchTens;
        var parameters=new Dictionary<string,int>{
            ["a"]=tens,["b"]=0,["c"]=0,["mode"]=3};
        Assert.True(UsCcssG1FullGradeStrategyAndTensQuestions.Valid(family,parameters));
        // Independently sample the real generator until this ten-word card
        // is emitted, so the student-facing option and letter are tested.
        var sourceCard=Enumerable.Range(0,2400)
            .Select(seed=>UsCcssG1FullGradeStrategyAndTensQuestions.Build(
                family,new Random(seed*71+17)))
            .First(q=>q.Parameters["a"]==tens&&q.Parameters["mode"]==3);
        Assert.Contains($"{correctCard}: {correctWord}",sourceCard.Prompt);
        Assert.Contains("no spelling required",sourceCard.Prompt);
        string expected=$"MATCH={correctCard};TENS={tens};NUMBER={tens*10}";
        Assert.Equal(expected,UsCcssG1FullGradeStrategyAndTensQuestions.Solve(
            family,parameters));
        Assert.True(ExactSkillContractQuestionEngine.Verify(family,parameters,expected));
        string wrongCard=correctCard=="A"?"B":"A";
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,parameters,
            $"MATCH={wrongCard};TENS={tens};NUMBER={tens*10}"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,parameters,
            $"MATCH={correctCard};TENS={tens};NUMBER={tens*10+10}"));
    }

    [Theory]
    [InlineData(3,2,0,"TENS=5;TOTAL=50")]
    [InlineData(7,4,1,"TENS=3;TOTAL=30")]
    [InlineData(4,3,2,"TENS=7;TOTAL=70")]
    [InlineData(7,3,3,"TENS=4;TOTAL=40")]
    public void Original2021G1U04L04BagStoriesAndTensPracticeHaveIndependentMath(
        int firstTens,int otherTens,int mode,string expected)
    {
        const string family=UsCcssG1FullGradeStrategyAndTensQuestions.AddRemoveMultipleTens;
        var p=new Dictionary<string,int>{
            ["a"]=firstTens,["b"]=otherTens,["c"]=0,["mode"]=mode};
        Assert.True(UsCcssG1FullGradeStrategyAndTensQuestions.Valid(family,p));
        Assert.Equal(expected,UsCcssG1FullGradeStrategyAndTensQuestions.Solve(family,p));
        Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,expected));
        Assert.False(ExactSkillContractQuestionEngine.Verify(
            family,p,expected.Replace("TOTAL=","TOTAL=9")));
    }

    [Fact]
    public void OriginalBagOfCubesStoryOperandsCannotBeRewrittenAsDifferentSourceNumbers()
    {
        const string family=UsCcssG1FullGradeStrategyAndTensQuestions.AddRemoveMultipleTens;
        foreach(var (a,b,mode) in new[]{
            (4,2,0),(3,1,0),(6,4,1),(7,3,1),
            (4,7,2),(3,4,3)
        })
            Assert.False(UsCcssG1FullGradeStrategyAndTensQuestions.Valid(
                family,new Dictionary<string,int>{
                    ["a"]=a,["b"]=b,["c"]=0,["mode"]=mode}));
        var seen=new HashSet<int>();
        for(int seed=0;seed<170;seed++)
        {
            var problem=UsCcssG1FullGradeStrategyAndTensQuestions.Build(
                family,new Random(seed*31+17));
            seen.Add(problem.Parameters["mode"]);
            if(problem.Parameters["mode"]==0)
            {
                Assert.Equal(3,problem.Parameters["a"]);
                Assert.Equal(2,problem.Parameters["b"]);
                Assert.Contains("Jada",problem.Prompt);
                Assert.Contains("30 cubes",problem.Prompt);
            }
            if(problem.Parameters["mode"]==1)
            {
                Assert.Equal(7,problem.Parameters["a"]);
                Assert.Equal(4,problem.Parameters["b"]);
                Assert.Contains("Tyler",problem.Prompt);
                Assert.Contains("40 cubes",problem.Prompt);
            }
        }
        Assert.Equal(4,seen.Count);
    }

    [Fact]
    public void RealEngineEmitsFiveDistinctQuestionsPerFamily()
    {
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var family in UsCcssG1FullGradeStrategyAndTensQuestions.Families)
        {
            var items=engine.Generate("G1-real-numeric-61-70",family,[family],
                ExactSkillQuestionDifficulty.Standard,10,20261018,[]);
            Assert.Equal(10,items.Count);
            Assert.Equal(10,items.Select(x=>x.Prompt).Distinct(StringComparer.Ordinal).Count());
            foreach(var item in items)
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,item.Parameters,item.CorrectAnswer));
        }
    }
}
