using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UsCcssG1FullGradeUnit7GeometryTimePracticeTests
{
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
