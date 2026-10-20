using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UsCcssG1FullGradeUnit7GeometryTimePracticeTests
{
    [Fact]
    public void OriginalUnit7Lesson3ReSortsFlatCardsByTwoDistinctDefiningRules()
    {
        const string f="usccss.g1.u07.l03.sort_flat_shapers";
        const string answer="CURVED=CIRCLE;THREE_SIDES=TRIANGLE;FOUR_SIDES=SQUARE,RECTANGLE";
        var p=new Dictionary<string,int>{
            ["a"]=3,["b"]=9,["c"]=0,["mode"]=4
        };
        Assert.True(UsCcssG1FullGradeUnit7GeometryTimeQuestions.Valid(f,p));
        Assert.Equal(answer,UsCcssG1FullGradeUnit7GeometryTimeQuestions.Solve(f,p));
        Assert.True(ExactSkillContractQuestionEngine.Verify(f,p,answer));
        Assert.False(ExactSkillContractQuestionEngine.Verify(f,p,
            "CURVED=CIRCLE;THREE_SIDES=RECTANGLE;FOUR_SIDES=SQUARE,TRIANGLE"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(f,p,
            answer+";CHILD_SORTED=YES"));
        var modes=new HashSet<int>();
        for(int seed=0;seed<600;seed++)
        {
            var item=UsCcssG1FullGradeUnit7GeometryTimeQuestions.Build(
                f,new Random(seed*97+19));
            modes.Add(item.Parameters["mode"]);
            var key=UsCcssG1FullGradeUnit7GeometryTimeQuestions.Solve(f,item.Parameters);
            Assert.True(ExactSkillContractQuestionEngine.Verify(f,item.Parameters,key));
            if(item.Parameters["mode"]==4)
            {
                Assert.Contains("FIRST sort",item.Prompt);
                Assert.Contains("NEXT sort",item.Prompt);
                Assert.Contains("teacher",item.Prompt,StringComparison.OrdinalIgnoreCase);
            }
        }
        Assert.Equal(Enumerable.Range(0,5).ToArray(),modes.OrderBy(x=>x));
    }

    [Fact]
    public void SourceTrianglesMustRejectOpenBoundariesSquaresAndCurvedEdges()
    {
        const string family="usccss.g1.u07.l05.defining_triangles";
        const string correct="TRIANGLES=A,B,C;NONTRIANGLES=D,E,F";
        var p=new Dictionary<string,int>{
            ["a"]=4,["b"]=3,["c"]=0,["mode"]=4
        };
        Assert.True(UsCcssG1FullGradeUnit7GeometryTimeQuestions.Valid(family,p));
        Assert.Equal(correct,UsCcssG1FullGradeUnit7GeometryTimeQuestions.Solve(family,p));
        Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,correct));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,
            "TRIANGLES=A,B,C,D;NONTRIANGLES=E,F"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,
            correct+";CHILD_DRAWINGS=SUBMITTED"));
        var modes=new HashSet<int>();
        for(int seed=0;seed<700;seed++)
        {
            var item=UsCcssG1FullGradeUnit7GeometryTimeQuestions.Build(
                family,new Random(seed*113+29));
            modes.Add(item.Parameters["mode"]);
            var answer=UsCcssG1FullGradeUnit7GeometryTimeQuestions.Solve(
                family,item.Parameters);
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                family,item.Parameters,answer));
            if(item.Parameters["mode"]==4)
            {
                Assert.Contains("Card D",item.Prompt);
                Assert.Contains("does NOT close",item.Prompt);
                Assert.Contains("CURVED side",item.Prompt);
                Assert.Contains("student creation",item.Prompt);
            }
        }
        Assert.Equal(Enumerable.Range(0,5).ToArray(),modes.OrderBy(x=>x));
    }

    [Theory]
    [InlineData(4,"RECTANGLE=YES;SQUARE=YES")]
    [InlineData(5,"RECTANGLE=YES;SQUARE=NO")]
    [InlineData(6,"RECTANGLE=NO;SQUARE=NO")]
    public void SourceSquareAndRectangleFiguresRequireCorrectDefiningAnglesAndSides(
        int mode,string answer)
    {
        const string f="usccss.g1.u07.l06.square_and_rectangle_properties";
        var p=new Dictionary<string,int>{
            ["a"]=4,["b"]=9,["c"]=0,["mode"]=mode
        };
        Assert.True(UsCcssG1FullGradeUnit7GeometryTimeQuestions.Valid(f,p));
        Assert.Equal(answer,UsCcssG1FullGradeUnit7GeometryTimeQuestions.Solve(f,p));
        Assert.True(ExactSkillContractQuestionEngine.Verify(f,p,answer));
        Assert.False(ExactSkillContractQuestionEngine.Verify(f,p,
            mode==4?"RECTANGLE=NO;SQUARE=YES":"RECTANGLE=YES;SQUARE=YES"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(f,p,
            answer+";STUDENT_DREW=YES"));
    }

    [Fact]
    public void RectangleClassificationStillGeneratesFourLegacyAndThreeNewSourceModes()
    {
        const string f="usccss.g1.u07.l06.square_and_rectangle_properties";
        var modes=new HashSet<int>();
        for(int seed=0;seed<800;seed++)
        {
            var item=UsCcssG1FullGradeUnit7GeometryTimeQuestions.Build(
                f,new Random(seed*109+17));
            modes.Add(item.Parameters["mode"]);
            var answer=UsCcssG1FullGradeUnit7GeometryTimeQuestions.Solve(
                f,item.Parameters);
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                f,item.Parameters,answer));
            if(item.Parameters["mode"]==4)
                Assert.Contains("five rectangles",item.Prompt);
            if(item.Parameters["mode"]==6)
                Assert.Contains("RIGHT angle",item.Prompt);
        }
        Assert.Equal(Enumerable.Range(0,7).ToArray(),modes.OrderBy(x=>x));
    }

    [Fact]
    public void OriginalUnit7Lesson7MustKeepHexagonOutlineAcrossDifferentBlockCounts()
    {
        const string family="usccss.g1.u07.l07.compose_flat_shapers";
        const string correct="TRIANGLES=6;TRAPEZOIDS=2;SAME_OUTLINE=YES";
        var p=new Dictionary<string,int>{{"a",3},{"b",7},{"c",0},{"mode",4}};
        Assert.True(UsCcssG1FullGradeUnit7GeometryTimeQuestions.Valid(family,p));
        Assert.Equal(correct,UsCcssG1FullGradeUnit7GeometryTimeQuestions.Solve(family,p));
        Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,correct));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,
            "TRIANGLES=6;TRAPEZOIDS=3;SAME_OUTLINE=YES"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,
            "TRIANGLES=6;TRAPEZOIDS=2;SAME_OUTLINE=NO"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,
            correct+";STUDENT_TRACED=YES"));
        var modes=new HashSet<int>();
        for(int seed=0;seed<650;seed++)
        {
            var item=UsCcssG1FullGradeUnit7GeometryTimeQuestions.Build(
                family,new Random(seed*131+23));
            modes.Add(item.Parameters["mode"]);
            var answer=UsCcssG1FullGradeUnit7GeometryTimeQuestions.Solve(
                family,item.Parameters);
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                family,item.Parameters,answer));
            if(item.Parameters["mode"]==4)
            {
                Assert.Contains("SAME silhouette",item.Prompt);
                Assert.Contains("SIX equal pattern-block triangles",item.Prompt);
                Assert.Contains("TWO trapezoid blocks",item.Prompt);
            }
        }
        Assert.Equal(Enumerable.Range(0,5).ToArray(),modes.OrderBy(x=>x));
    }

    [Theory]
    [InlineData(2,2,"PER_LAYER=4;LAYERS=2;TOTAL_CUBES=8")]
    [InlineData(3,2,"PER_LAYER=6;LAYERS=2;TOTAL_CUBES=12")]
    [InlineData(4,3,"PER_LAYER=12;LAYERS=2;TOTAL_CUBES=24")]
    public void ComposedSolidSourceModeMustCountTwoLayerCubeConstruction(
        int columns,int rows,string expected)
    {
        const string family="usccss.g1.u07.l02.compose_solids_components";
        var p=new Dictionary<string,int>{
            ["a"]=columns,["b"]=rows,["c"]=0,["mode"]=4
        };
        Assert.True(UsCcssG1FullGradeUnit7GeometryTimeQuestions.Valid(family,p));
        Assert.Equal(expected,UsCcssG1FullGradeUnit7GeometryTimeQuestions.Solve(family,p));
        Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,expected));
        Assert.False(ExactSkillContractQuestionEngine.Verify(
            family,p,$"PER_LAYER={columns*rows};LAYERS=2;TOTAL_CUBES={columns*rows}"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(
            family,p,expected+";REAL_BUILD=CLAIMED"));
        var bad=new Dictionary<string,int>(p){["a"]=5};
        Assert.False(UsCcssG1FullGradeUnit7GeometryTimeQuestions.Valid(family,bad));
        bad["a"]=columns;
        bad["b"]=4;
        Assert.False(UsCcssG1FullGradeUnit7GeometryTimeQuestions.Valid(family,bad));
    }

    [Fact]
    public void ComposedSolidSourceAndLegacyModesAllRemainReachable()
    {
        const string family="usccss.g1.u07.l02.compose_solids_components";
        var seen=new HashSet<int>();
        for(int seed=0;seed<600;seed++)
        {
            var item=UsCcssG1FullGradeUnit7GeometryTimeQuestions.Build(
                family,new Random(seed*149+17));
            int mode=item.Parameters["mode"];
            seen.Add(mode);
            Assert.True(UsCcssG1FullGradeUnit7GeometryTimeQuestions.Valid(
                family,item.Parameters));
            var answer=UsCcssG1FullGradeUnit7GeometryTimeQuestions.Solve(
                family,item.Parameters);
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                family,item.Parameters,answer));
            if(mode==4)
            {
                Assert.Contains("TWO identical layers",item.Prompt);
                Assert.Contains("face to face",item.Prompt);
                Assert.InRange(item.Parameters["a"],2,4);
                Assert.InRange(item.Parameters["b"],2,3);
            }
        }
        Assert.Equal(Enumerable.Range(0,5).ToArray(),seen.OrderBy(x=>x));
    }

    [Fact]
    public void EveryOriginalUnit7LessonHasAnExplicitTextOnlyDigitalContract()
    {
        Assert.Equal(17,UsCcssG1FullGradeUnit7GeometryTimeQuestions.Families.Count);
        for(int n=1;n<=17;n++)
        {
            var code=$"PED:US-CCSS-MATH:G1:U07:L{n:00}";
            var contract=LessonPracticeContractRegistry.All.Single(x=>x.LessonCode==code);
            var f=Assert.Single(contract.AllowedQuestionFamilies);
            Assert.True(UsCcssG1FullGradeUnit7GeometryTimeQuestions.Supports(f));
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(f,out var choice));
            Assert.Equal(LessonPracticeInteractionKind.ContractDriven,choice!.InteractionKind);
        }
    }
    [Fact]
    public void GeometryAndTimeChecksPassCorrectAnswersAndRejectFalseClaims()
    {
        foreach(var f in UsCcssG1FullGradeUnit7GeometryTimeQuestions.Families)
            for(int seed=0;seed<110;seed++)
            {
                var q=UsCcssG1FullGradeUnit7GeometryTimeQuestions.Build(f,new Random(seed*53+11));
                var answer=UsCcssG1FullGradeUnit7GeometryTimeQuestions.Solve(f,q.Parameters);
                Assert.True(UsCcssG1FullGradeUnit7GeometryTimeQuestions.Valid(f,q.Parameters));
                Assert.True(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,answer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,answer+";FAKE=1"));
                Assert.False(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,"TIME=99:99"));
            }
    }
    [Fact]
    public void TextualPracticePromptsDoNotProvidePreFilledCorrectAnswers()
    {
        foreach(var family in UsCcssG1FullGradeUnit7GeometryTimeQuestions.Families)
        {
            for(int seed=0;seed<60;seed++)
            {
                var q=UsCcssG1FullGradeUnit7GeometryTimeQuestions.Build(family,new Random(seed*17+5));
                var answer=UsCcssG1FullGradeUnit7GeometryTimeQuestions.Solve(family,q.Parameters);
                Assert.DoesNotContain("Write "+answer+".",q.Prompt,StringComparison.Ordinal);
            }
        }
    }
    [Theory]
    [InlineData(2,"SORT=UNEQUAL;PIECES=2")]
    [InlineData(3,"SORT=EQUAL;PIECES=4")]
    public void OriginalShadedPieceCardSortDistinguishesEqualAndUnequalArea(
        int mode,string expected)
    {
        const string family="usccss.g1.u07.l10.parts_make_whole";
        var p=new Dictionary<string,int>{{"a",4},{"b",3},{"c",0},{"mode",mode}};
        Assert.True(UsCcssG1FullGradeUnit7GeometryTimeQuestions.Valid(family,p));
        Assert.Equal(expected,UsCcssG1FullGradeUnit7GeometryTimeQuestions.Solve(family,p));
        Assert.True(UsCcssG1FullGradeUnit7GeometryTimeQuestions.Verify(family,p,expected));
        Assert.False(UsCcssG1FullGradeUnit7GeometryTimeQuestions.Verify(family,p,
            mode==2?"SORT=EQUAL;PIECES=2":"SORT=UNEQUAL;PIECES=4"));
        Assert.False(UsCcssG1FullGradeUnit7GeometryTimeQuestions.Verify(family,p,
            "PARTS=2;WHOLES=1"));
    }

    [Theory]
    [InlineData(7,7,2,"SAME=YES;TIME=7:00")]
    [InlineData(7,9,3,"SAME=NO;TIME=7:00")]
    [InlineData(12,1,3,"SAME=NO;TIME=12:00")]
    public void OriginalClockConnectionsCompareAnalogAndDigitalTimes(
        int hour,int otherDigitalHour,int mode,string answer)
    {
        const string family="usccss.g1.u07.l13.hour_clock";
        var p=new Dictionary<string,int>{{"a",hour},{"b",otherDigitalHour},{"c",0},{"mode",mode}};
        Assert.True(UsCcssG1FullGradeUnit7GeometryTimeQuestions.Valid(family,p));
        Assert.Equal(answer,UsCcssG1FullGradeUnit7GeometryTimeQuestions.Solve(family,p));
        Assert.True(UsCcssG1FullGradeUnit7GeometryTimeQuestions.Verify(family,p,answer));
        Assert.False(UsCcssG1FullGradeUnit7GeometryTimeQuestions.Verify(family,p,
            $"TIME={hour}:00"));
        Assert.False(UsCcssG1FullGradeUnit7GeometryTimeQuestions.Verify(family,p,
            mode==2?$"SAME=NO;TIME={hour}:00":$"SAME=YES;TIME={hour}:00"));
        if(mode==3)
        {
            p["b"]=hour;
            Assert.False(UsCcssG1FullGradeUnit7GeometryTimeQuestions.Valid(family,p));
        }
    }

    [Theory]
    [InlineData(7,2,"COLOR=BLUE;TIME=7:00;WORDS=seven_oclock")]
    [InlineData(7,3,"COLOR=RED;TIME=7:30;WORDS=half_past_seven")]
    [InlineData(12,3,"COLOR=RED;TIME=12:30;WORDS=half_past_twelve")]
    public void SourceHalfClockColorClassificationAndSpelledTimeMustBothBeCorrect(
        int hour,int mode,string answer)
    {
        const string f="usccss.g1.u07.l14.half_hour_clock";
        var p=new Dictionary<string,int>{{"a",hour},{"b",2},{"c",0},{"mode",mode}};
        Assert.True(UsCcssG1FullGradeUnit7GeometryTimeQuestions.Valid(f,p));
        Assert.Equal(answer,UsCcssG1FullGradeUnit7GeometryTimeQuestions.Solve(f,p));
        Assert.True(UsCcssG1FullGradeUnit7GeometryTimeQuestions.Verify(f,p,answer));
        Assert.False(UsCcssG1FullGradeUnit7GeometryTimeQuestions.Verify(f,p,
            answer.Replace(mode==2?"COLOR=BLUE":"COLOR=RED",
                           mode==2?"COLOR=RED":"COLOR=BLUE")));
        Assert.False(UsCcssG1FullGradeUnit7GeometryTimeQuestions.Verify(f,p,
            "MINUTES=30;HOUR="+hour));
    }
    [Theory]
    [InlineData(6,2)]
    [InlineData(8,3)]
    public void OriginalWriteTimesMinuteCountingRequiresSixHopsFiveEach(int hour,int mode)
    {
        const string f="usccss.g1.u07.l15.write_hours_halfhours";
        var p=new Dictionary<string,int>{{"a",hour},{"b",2},{"c",0},{"mode",mode}};
        const string expected="HOPS=6;MINUTES=30;STOP_AT=6";
        Assert.Equal(expected,UsCcssG1FullGradeUnit7GeometryTimeQuestions.Solve(f,p));
        Assert.True(UsCcssG1FullGradeUnit7GeometryTimeQuestions.Verify(f,p,expected));
        Assert.False(UsCcssG1FullGradeUnit7GeometryTimeQuestions.Verify(f,p,
            "HOPS=5;MINUTES=25;STOP_AT=5"));
        Assert.False(UsCcssG1FullGradeUnit7GeometryTimeQuestions.Verify(f,p,
            $"TIME={hour}:30"));
    }

    [Fact]
    public void ClocksUseWholeHoursAndHalfHoursNotUnintroducedQuarterHours()
    {
        const string f="usccss.g1.u07.l14.half_hour_clock";
        var p=new Dictionary<string,int>{{"a",7},{"b",3},{"c",0},{"mode",1}};
        Assert.Equal("MINUTES=30;HOUR=7",UsCcssG1FullGradeUnit7GeometryTimeQuestions.Solve(f,p));
        Assert.False(UsCcssG1FullGradeUnit7GeometryTimeQuestions.Verify(f,p,"MINUTES=15;HOUR=7"));
    }
    [Theory]
    [InlineData(7,3,0,"RESULT=7")]
    [InlineData(7,3,1,"SHAPE=RECTANGLE")]
    public void OriginalU07L17CenterUsesShapeOrArithmeticRatherThanOffTopicClock(
        int a,int b,int mode,string answer)
    {
        const string family="usccss.g1.u07.l17.center_geometry_and_time";
        var p=new Dictionary<string,int>{{"a",a},{"b",b},{"c",0},{"mode",mode}};
        Assert.Equal(answer,UsCcssG1FullGradeUnit7GeometryTimeQuestions.Solve(family,p));
        Assert.True(UsCcssG1FullGradeUnit7GeometryTimeQuestions.Verify(family,p,answer));
        Assert.DoesNotContain("minute hand",UsCcssG1FullGradeUnit7GeometryTimeQuestions.Build(
            family,new Random(17)).Prompt,StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void AllTextualGeometryAndTimePromptsOfferTenDistinctCases()
    {
        var e=new ExactSkillContractQuestionEngine();
        foreach(var f in UsCcssG1FullGradeUnit7GeometryTimeQuestions.Families)
        {
            var qs=e.Generate("g1-u07",f,[f],ExactSkillQuestionDifficulty.Standard,10,20261019,[]);
            Assert.Equal(10,qs.Count);
            Assert.Equal(10,qs.Select(x=>x.Prompt).Distinct(StringComparer.Ordinal).Count());
        }
    }
}
