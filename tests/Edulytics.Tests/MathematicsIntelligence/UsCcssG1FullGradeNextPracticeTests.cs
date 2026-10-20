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
            UsCcssG1FullGradeNextQuestions.TeenSubtract,p,"REACH10=4;REST=2;DIFFERENCE=8;EQUATION=14-6=8"));
        Assert.False(UsCcssG1FullGradeNextQuestions.Verify(
            UsCcssG1FullGradeNextQuestions.TeenSubtract,p,"REACH10=4;REST=0;DIFFERENCE=10;EQUATION=14-6=10"));
        var zoo=new Dictionary<string,int>{{"a",3},{"b",5},{"c",7},{"mode",0}};
        Assert.True(UsCcssG1FullGradeNextQuestions.Verify(
            UsCcssG1FullGradeNextQuestions.ZooThree,zoo,"TOTAL=15;EQUATION=3+5+7=15"));
        Assert.False(UsCcssG1FullGradeNextQuestions.Verify(
            UsCcssG1FullGradeNextQuestions.ZooThree,zoo,"TOTAL=8;EQUATION=3+5=8"));
    }
    [Fact]
    public void Original2021TeenAndTenFrameSubtractionRequireStudentWrittenEquations()
    {
        var p=new Dictionary<string,int>{{"a",15},{"b",8},{"c",0},{"mode",2}};
        foreach(var family in new[]{
            UsCcssG1FullGradeNextQuestions.TeenSubtract,
            UsCcssG1FullGradeNextQuestions.TenSubtract})
        {
            var expected=UsCcssG1FullGradeNextQuestions.Solve(family,p);
            Assert.Contains("EQUATION=15-8=7",expected);
            Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,expected));
            Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,
                expected.Replace("EQUATION=15-8=7","EQUATION=15-8=6")));
            Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,
                expected.Split(";EQUATION=",StringSplitOptions.None)[0]));
        }
    }

    [Fact]
    public void OriginalHowCloseAndFiveInARowStayDifferentSourceGames()
    {
        const string family=UsCcssG1FullGradeNextQuestions.CenterTwoThree;
        // 2021 original uses number cards 0 through 10, never an 11-card.
        var five=new Dictionary<string,int>{{"a",10},{"b",9},{"c",0},{"mode",3}};
        Assert.True(UsCcssG1FullGradeNextQuestions.Valid(family,five));
        Assert.True(ExactSkillContractQuestionEngine.Verify(family,five,
            "TOTAL=19;EQUATION=9+10=19"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,five,
            "TOTAL=19;EQUATION=9+9+1=19"));
        five["a"]=0;five["b"]=7;
        Assert.True(UsCcssG1FullGradeNextQuestions.Valid(family,five));
        Assert.True(ExactSkillContractQuestionEngine.Verify(family,five,
            "TOTAL=7;EQUATION=0+7=7"));
        five["a"]=11;five["b"]=9;
        Assert.False(UsCcssG1FullGradeNextQuestions.Valid(family,five));
        five["a"]=10;five["b"]=6;
        Assert.False(UsCcssG1FullGradeNextQuestions.Valid(family,five));
        // How Close? removes the 10-cards, but permits a 0- or 9-card.
        var close=new Dictionary<string,int>{{"a",0},{"b",9},{"c",8},{"mode",1}};
        Assert.True(UsCcssG1FullGradeNextQuestions.Valid(family,close));
        Assert.True(ExactSkillContractQuestionEngine.Verify(family,close,
            "TOTAL=17;EQUATION=8+0+9=17"));
        close["a"]=10;
        Assert.False(UsCcssG1FullGradeNextQuestions.Valid(family,close));
        close["a"]=0;close["c"]=0; // The chosen third card can be ZERO.
        Assert.True(UsCcssG1FullGradeNextQuestions.Valid(family,close));
        Assert.True(ExactSkillContractQuestionEngine.Verify(family,close,
            "TOTAL=9;EQUATION=9+0+0=9"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,close,
            "TOTAL=9;EQUATION=9+0=9")); // Cannot drop a selected zero-card.
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,close,
            "TOTAL=9;EQUATION=9+1+0=9")); // Wrong selected source card.

        var triple=new Dictionary<string,int>{{"a",3},{"b",6},{"c",7},{"mode",1}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(family,triple,
            "TOTAL=16;EQUATION=3+7+6=16"));
        var modeThree=Enumerable.Range(0,350)
            .Select(seed=>UsCcssG1FullGradeNextQuestions.Build(family,new Random(41*seed+17)))
            .Where(q=>q.Parameters["mode"]==3).ToArray();
        Assert.True(modeThree.Length>50);
        Assert.All(modeThree,q=>
        {
            Assert.InRange(q.Parameters["a"],0,10);
            Assert.InRange(q.Parameters["b"],7,9);
            Assert.Equal(0,q.Parameters["c"]);
            Assert.Contains("FIVE IN A ROW",q.Prompt);
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                family,q.Parameters,UsCcssG1FullGradeNextQuestions.Solve(family,q.Parameters)));
        });
    }

    [Fact]
    public void OriginalZooStudentTasksIncludeCatsAndPettingAnimals()
    {
        const string family=UsCcssG1FullGradeNextQuestions.ZooThree;
        var tasks=Enumerable.Range(0,450).Select(n=>
            UsCcssG1FullGradeNextQuestions.Build(family,new Random(n*79+5))).ToArray();
        Assert.Contains(tasks,q=>q.Prompt.Contains("8 lions, 4 tigers and 3 cheetahs"));
        Assert.Contains(tasks,q=>q.Prompt.Contains("8 goats, 7 sheep and 4 pigs"));
        Assert.All(tasks,q=>Assert.True(ExactSkillContractQuestionEngine.Verify(
            family,q.Parameters,UsCcssG1FullGradeNextQuestions.Solve(family,q.Parameters))));
    }

    [Fact]
    public void Zoo2021OriginalBirdExhibitUsesAllThreeSourceGroups()
    {
        const string family=UsCcssG1FullGradeNextQuestions.ZooThree;
        var p=new Dictionary<string,int>{{"a",3},{"b",6},{"c",7},{"mode",3}};
        Assert.True(UsCcssG1FullGradeNextQuestions.Valid(family,p));
        Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,
            "TOTAL=16;EQUATION=7+3+6=16"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,
            "TOTAL=16;EQUATION=6+4+6=16"));
        p["a"]=4;
        Assert.False(UsCcssG1FullGradeNextQuestions.Valid(family,p));
        var generated=Enumerable.Range(0,340).Select(n=>
            UsCcssG1FullGradeNextQuestions.Build(family,new Random(97*n+23)))
            .Where(q=>q.Parameters["mode"]==3).ToArray();
        Assert.True(generated.Length>40);
        Assert.All(generated,q=>
        {
            Assert.Contains("3 herons",q.Prompt);
            Assert.Contains("6 hawks",q.Prompt);
            Assert.Contains("7 hummingbirds",q.Prompt);
            Assert.True(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,
                UsCcssG1FullGradeNextQuestions.Solve(family,q.Parameters)));
        });
    }

    [Fact]
    public void Original2021ElenaSixteenMinusSevenUsesExactCrayonStoryAndIndependentChecks()
    {
        const string family=UsCcssG1FullGradeNextQuestions.TeenSubtract;
        var p=new Dictionary<string,int>{{"a",16},{"b",7},{"c",0},{"mode",3}};
        Assert.True(UsCcssG1FullGradeNextQuestions.Valid(family,p));
        const string correct="REACH10=6;REST=1;DIFFERENCE=9;EQUATION=16-7=9";
        Assert.Equal(correct,UsCcssG1FullGradeNextQuestions.Solve(family,p));
        Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,correct));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,
            "REACH10=6;REST=1;DIFFERENCE=10;EQUATION=16-7=10"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,
            "REACH10=6;REST=1;DIFFERENCE=9;EQUATION=16-7=8"));
        p["a"]=15;
        Assert.False(UsCcssG1FullGradeNextQuestions.Valid(family,p));
        var sampled=Enumerable.Range(0,360)
            .Select(n=>UsCcssG1FullGradeNextQuestions.Build(family,new Random(31*n+19)))
            .Where(q=>q.Parameters["mode"]==3).ToArray();
        Assert.True(sampled.Length>40);
        Assert.All(sampled,q=>
        {
            Assert.Contains("Elena has 16 crayons",q.Prompt);
            Assert.Equal(16,q.Parameters["a"]);
            Assert.Equal(7,q.Parameters["b"]);
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                family,q.Parameters,UsCcssG1FullGradeNextQuestions.Solve(family,q.Parameters)));
        });
    }

    [Fact]
    public void Diego2021FifteenMinusEightIsGeneratedAndTenFrameMethodChecked()
    {
        const string family=UsCcssG1FullGradeNextQuestions.TenSubtract;
        var p=new Dictionary<string,int>{{"a",15},{"b",8},{"c",0},{"mode",2}};
        Assert.True(UsCcssG1FullGradeNextQuestions.Valid(family,p));
        Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,
            "TO10=5;REST=3;DIFFERENCE=7;EQUATION=15-8=7"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,
            "TO10=5;REST=3;DIFFERENCE=8;EQUATION=15-8=8"));
        p["a"]=16;
        Assert.False(UsCcssG1FullGradeNextQuestions.Valid(family,p));
        var generated=Enumerable.Range(0,320)
            .Select(n=>UsCcssG1FullGradeNextQuestions.Build(family,new Random(43*n+31)))
            .Where(q=>q.Parameters["mode"]==2).ToArray();
        Assert.True(generated.Length>35);
        Assert.All(generated,q=>
        {
            Assert.Contains("Diego starts Number Card Subtraction with 15",q.Prompt);
            Assert.Equal(15,q.Parameters["a"]);
            Assert.Equal(8,q.Parameters["b"]);
            Assert.True(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,
                UsCcssG1FullGradeNextQuestions.Solve(family,q.Parameters)));
        });
    }

    [Fact]
    public void Original2021LinHanKiranStrategiesRetainAllThreeEquivalentExpressions()
    {
        const string family=UsCcssG1FullGradeNextQuestions.FlexibleAdd;
        var p=new Dictionary<string,int>{{"a",8},{"b",7},{"c",0},{"mode",2}};
        const string original="LIN=8+2+5=15;HAN=7+7+1=15;KIRAN=8+8-1=15;TOTAL=15";
        Assert.True(UsCcssG1FullGradeNextQuestions.Valid(family,p));
        Assert.Equal(original,UsCcssG1FullGradeNextQuestions.Solve(family,p));
        Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,original));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,
            "LIN=8+2+5=15;HAN=7+7+1=15;KIRAN=8+8-1=16;TOTAL=15"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,
            "MOVE=2;REST=5;TOTAL=15"));
        p["b"]=6;
        Assert.False(UsCcssG1FullGradeNextQuestions.Valid(family,p));
        var sourceTasks=Enumerable.Range(0,360)
            .Select(n=>UsCcssG1FullGradeNextQuestions.Build(family,new Random(73*n+11)))
            .Where(q=>q.Parameters["mode"]==2).ToArray();
        Assert.True(sourceTasks.Length>35);
        Assert.All(sourceTasks,q=>
        {
            Assert.Contains("Lin, Han and Kiran",q.Prompt);
            Assert.Equal(original,UsCcssG1FullGradeNextQuestions.Solve(family,q.Parameters));
        });
    }

    [Fact]
    public void OriginalSourceGroupEquationsAcceptValidAddendOrderNotReplacementNumbers()
    {
        const string zoo=UsCcssG1FullGradeNextQuestions.ZooThree;
        var actual=new Dictionary<string,int>{{"a",8},{"b",7},{"c",5},{"mode",2}};
        Assert.True(UsCcssG1FullGradeNextQuestions.Verify(zoo,actual,
            "EQUATION=5+8+7=20;TOTAL=20"));
        Assert.True(ExactSkillContractQuestionEngine.Verify(zoo,actual,
            "TOTAL=20;EQUATION=7+5+8=20"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(zoo,actual,
            "TOTAL=20;EQUATION=6+6+8=20"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(zoo,actual,
            "TOTAL=20;EQUATION=8+7+5=19"));
        const string center=UsCcssG1FullGradeNextQuestions.CenterTwoThree;
        var three=new Dictionary<string,int>{{"a",3},{"b",6},{"c",7},{"mode",1}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(center,three,
            "TOTAL=16;EQUATION=7+3+6=16"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(center,three,
            "TOTAL=16;EQUATION=7+4+5=16"));
        three["mode"]=0;three["c"]=0;
        Assert.True(ExactSkillContractQuestionEngine.Verify(center,three,
            "TOTAL=9;EQUATION=6+3=9"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(center,three,
            "TOTAL=9;EQUATION=6+2+1=9"));
    }

    [Fact]
    public void Original2021ZooStudentFacingReptileStoryHasExactlyThreeSourceGroups()
    {
        const string f=UsCcssG1FullGradeNextQuestions.ZooThree;
        var p=new Dictionary<string,int>{{"a",8},{"b",7},{"c",5},{"mode",2}};
        Assert.True(UsCcssG1FullGradeNextQuestions.Valid(f,p));
        Assert.Equal("TOTAL=20;EQUATION=8+7+5=20",
            UsCcssG1FullGradeNextQuestions.Solve(f,p));
        Assert.True(UsCcssG1FullGradeNextQuestions.Verify(
            f,p,"TOTAL=20;EQUATION=8+7+5=20"));
        Assert.False(UsCcssG1FullGradeNextQuestions.Verify(
            f,p,"TOTAL=15;EQUATION=8+7=15"));
        p["c"]=4;
        Assert.False(UsCcssG1FullGradeNextQuestions.Valid(f,p));
        var seen=Enumerable.Range(0,350)
            .Select(n=>UsCcssG1FullGradeNextQuestions.Build(f,new Random(n*71+7)))
            .Where(q=>q.Parameters["mode"]==2).ToArray();
        Assert.True(seen.Length>30);
        Assert.All(seen,q=>
        {
            Assert.Contains("8 snakes, 7 iguanas and 5 frogs",q.Prompt);
            Assert.Equal("TOTAL=20;EQUATION=8+7+5=20",
                UsCcssG1FullGradeNextQuestions.Solve(f,q.Parameters));
        });
    }

    [Fact]
    public void Lesson57FourOriginalZooStoriesAndPublishedPrimateCoolDownRejectWrongGroups()
    {
        const string family=UsCcssG1FullGradeNextQuestions.ZooThree;
        // Four original public 2021 tasks plus a matching 2023 published CKMath primate cool-down.
        var stories=new[]
        {
            (a:8,b:7,c:5,mode:2,total:20), // Reptiles: snakes, iguanas, frogs.
            (a:3,b:6,c:7,mode:3,total:16), // Herons, hawks, hummingbirds.
            (a:8,b:4,c:3,mode:0,total:15), // Lions, tigers, cheetahs.
            (a:8,b:7,c:4,mode:1,total:19), // Goats, sheep, pigs.
            (a:8,b:4,c:7,mode:4,total:19)  // Public CKMath guide: monkeys, gorillas, orangutans.
        };
        var sourceBoundPrompts=Enumerable.Range(0,500)
            .Select(n=>UsCcssG1FullGradeNextQuestions.Build(family,new Random(n*37+13)))
            .Where(q=>q.Parameters["mode"]==4).ToArray();
        Assert.True(sourceBoundPrompts.Length>50);
        Assert.All(sourceBoundPrompts,q=>
        {
            Assert.Contains("8 monkeys, 4 gorillas and 7 orangutans",q.Prompt);
            Assert.Equal("TOTAL=19;EQUATION=8+4+7=19",
                UsCcssG1FullGradeNextQuestions.Solve(family,q.Parameters));
        });
        var invalidMode=new Dictionary<string,int>
        {["a"]=8,["b"]=4,["c"]=7,["mode"]=5};
        Assert.False(UsCcssG1FullGradeNextQuestions.Valid(family,invalidMode));
        invalidMode["mode"]=4;
        Assert.False(UsCcssG1FullGradeNextQuestions.Valid(
            UsCcssG1FullGradeNextQuestions.CenterTwoThree,invalidMode));
        foreach(var (a,b,c,mode,total) in stories)
        {
            var parameters=new Dictionary<string,int>
            {["a"]=a,["b"]=b,["c"]=c,["mode"]=mode};
            Assert.True(UsCcssG1FullGradeNextQuestions.Valid(family,parameters));
            var expected=$"TOTAL={total};EQUATION={a}+{b}+{c}={total}";
            Assert.Equal(expected,UsCcssG1FullGradeNextQuestions.Solve(family,parameters));
            Assert.True(ExactSkillContractQuestionEngine.Verify(family,parameters,expected));
            Assert.True(ExactSkillContractQuestionEngine.Verify(family,parameters,
                $"EQUATION={c}+{a}+{b}={total};TOTAL={total}"));
            Assert.False(ExactSkillContractQuestionEngine.Verify(family,parameters,
                $"TOTAL={a+b};EQUATION={a}+{b}={a+b}")); // Lost third group.
            Assert.False(ExactSkillContractQuestionEngine.Verify(family,parameters,
                $"TOTAL={total};EQUATION={a}+{b+1}+{c-1}={total}")); // Same sum, wrong source.
            Assert.False(ExactSkillContractQuestionEngine.Verify(family,parameters,
                $"TOTAL={total+1};EQUATION={a}+{b}+{c}={total+1}"));
            Assert.False(ExactSkillContractQuestionEngine.Verify(family,parameters,
                "TOTAL=INVALID;EQUATION=0+0+0=0"));
            Assert.False(ExactSkillContractQuestionEngine.Verify(family,parameters,
                expected+";TOTAL="+total)); // Duplicate answer key.
        }
    }

    [Fact]
    public void Lesson59OriginalTeenSubtractionCoversCrossTenNoCrossAndZeroCardWithoutFalseMethods()
    {
        const string family=UsCcssG1FullGradeNextQuestions.TeenSubtract;
        var noCross=new Dictionary<string,int>{{"a",17},{"b",4},{"c",0},{"mode",0}};
        Assert.True(UsCcssG1FullGradeNextQuestions.Valid(family,noCross));
        Assert.Equal("DIFFERENCE=13;EQUATION=17-4=13",
            UsCcssG1FullGradeNextQuestions.Solve(family,noCross));
        Assert.True(ExactSkillContractQuestionEngine.Verify(family,noCross,
            "EQUATION=17-4=13;DIFFERENCE=13"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,noCross,
            "REACH10=7;REST=-3;DIFFERENCE=13;EQUATION=17-4=13"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,noCross,
            "DIFFERENCE=13;EQUATION=17-4=12"));

        var zero=new Dictionary<string,int>{{"a",19},{"b",0},{"c",0},{"mode",1}};
        Assert.True(UsCcssG1FullGradeNextQuestions.Valid(family,zero));
        Assert.Equal("DIFFERENCE=19;EQUATION=19-0=19",
            UsCcssG1FullGradeNextQuestions.Solve(family,zero));
        Assert.True(ExactSkillContractQuestionEngine.Verify(family,zero,
            "DIFFERENCE=19;EQUATION=19-0=19"));
        zero["b"]=10; // Teacher source explicitly excludes 10 from subtractor cards.
        Assert.False(UsCcssG1FullGradeNextQuestions.Valid(family,zero));
        zero["a"]=20;zero["b"]=0; // 20 is not a teen number card.
        Assert.False(UsCcssG1FullGradeNextQuestions.Valid(family,zero));

        var cross=new Dictionary<string,int>{{"a",14},{"b",6},{"c",0},{"mode",2}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(family,cross,
            "REACH10=4;REST=2;DIFFERENCE=8;EQUATION=14-6=8"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,cross,
            "DIFFERENCE=8;EQUATION=14-6=8")); // Missing decomposition when needed.

        var sourceElena=new Dictionary<string,int>{{"a",16},{"b",7},{"c",0},{"mode",3}};
        Assert.True(UsCcssG1FullGradeNextQuestions.Valid(family,sourceElena));
        Assert.False(UsCcssG1FullGradeNextQuestions.Valid(family,
            new Dictionary<string,int>{{"a",16},{"b",6},{"c",0},{"mode",3}}));
        Assert.True(ExactSkillContractQuestionEngine.Verify(family,sourceElena,
            "REACH10=6;REST=1;DIFFERENCE=9;EQUATION=16-7=9"));

        var examples=Enumerable.Range(0,400)
            .Select(n=>UsCcssG1FullGradeNextQuestions.Build(family,new Random(n*41+23)))
            .ToArray();
        Assert.Contains(examples,q=>q.Parameters["b"]<q.Parameters["a"]-10);
        Assert.Contains(examples,q=>q.Parameters["b"]>=q.Parameters["a"]-10);
        Assert.All(examples,q=>
            Assert.True(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,
                UsCcssG1FullGradeNextQuestions.Solve(family,q.Parameters))));
    }

    [Fact]
    public void Lesson60TenFrameNumberCardSubtractionAcceptsNoCrossAndExactOriginalDiegoTasks()
    {
        const string f=UsCcssG1FullGradeNextQuestions.TenSubtract;
        var withoutCross=new Dictionary<string,int>{{"a",17},{"b",4},{"c",0},{"mode",0}};
        Assert.True(UsCcssG1FullGradeNextQuestions.Valid(f,withoutCross));
        Assert.Equal("DIFFERENCE=13;EQUATION=17-4=13",
            UsCcssG1FullGradeNextQuestions.Solve(f,withoutCross));
        Assert.True(ExactSkillContractQuestionEngine.Verify(f,withoutCross,
            "DIFFERENCE=13;EQUATION=17-4=13"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(f,withoutCross,
            "TO10=7;REST=-3;DIFFERENCE=13;EQUATION=17-4=13"));
        var zero=new Dictionary<string,int>{{"a",19},{"b",0},{"c",0},{"mode",1}};
        Assert.True(UsCcssG1FullGradeNextQuestions.Valid(f,zero));
        Assert.True(ExactSkillContractQuestionEngine.Verify(f,zero,
            "DIFFERENCE=19;EQUATION=19-0=19"));
        zero["b"]=10;
        Assert.False(UsCcssG1FullGradeNextQuestions.Valid(f,zero));
        zero["a"]=20;zero["b"]=0;
        Assert.False(UsCcssG1FullGradeNextQuestions.Valid(f,zero));
        var diego=new Dictionary<string,int>{{"a",15},{"b",8},{"c",0},{"mode",2}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(f,diego,
            "TO10=5;REST=3;DIFFERENCE=7;EQUATION=15-8=7"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(f,diego,
            "TO10=5;REST=2;DIFFERENCE=7;EQUATION=15-8=7"));
        Assert.False(UsCcssG1FullGradeNextQuestions.Valid(f,
            new Dictionary<string,int>{{"a",15},{"b",7},{"c",0},{"mode",2}}));
        var originalFollowUps=new[]{(a:14,b:5,d:9),(a:13,b:6,d:7)};
        foreach(var (a,b,d) in originalFollowUps)
        {
            var p=new Dictionary<string,int>{{"a",a},{"b",b},{"c",0},{"mode",3}};
            Assert.True(UsCcssG1FullGradeNextQuestions.Valid(f,p));
            Assert.True(ExactSkillContractQuestionEngine.Verify(f,p,
                $"TO10={a-10};REST={b-(a-10)};DIFFERENCE={d};EQUATION={a}-{b}={d}"));
            Assert.False(ExactSkillContractQuestionEngine.Verify(f,p,
                $"TO10={a-10};REST=99;DIFFERENCE={d};EQUATION={a}-{b}={d}"));
        }
        var generated=Enumerable.Range(0,400)
            .Select(n=>UsCcssG1FullGradeNextQuestions.Build(f,new Random(n*61+7)))
            .ToArray();
        Assert.Contains(generated,q=>q.Parameters["b"]<q.Parameters["a"]-10);
        Assert.Contains(generated,q=>q.Parameters["b"]>=q.Parameters["a"]-10);
        Assert.All(generated,q=>
            Assert.True(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,
                UsCcssG1FullGradeNextQuestions.Solve(f,q.Parameters))));
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
