using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UsCcssG1FullGradeTensAndOnesPracticeTests
{
    [Fact]
    public void FiveRegisteredSourceSpecificPlaceValueFamiliesAreQueryable()
    {
        Assert.Equal(5,UsCcssG1FullGradeTensAndOnesQuestions.Families.Count);
        foreach(var family in UsCcssG1FullGradeTensAndOnesQuestions.Families)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var interaction));
            Assert.Equal(LessonPracticeInteractionKind.ContractDriven,interaction!.InteractionKind);
        }
        for(int n=6;n<=10;n++)
        {
            var code=$"PED:US-CCSS-MATH:G1:U04:L{n:00}";
            var item=LessonPracticeContractRegistry.All.Single(x=>x.LessonCode==code);
            Assert.Single(item.AllowedQuestionFamilies);
            Assert.True(UsCcssG1FullGradeTensAndOnesQuestions.Supports(item.AllowedQuestionFamilies[0]));
        }
    }
    [Fact]
    public void OriginalShowMeYourNumberAcceptsAnyDistinctMathematicallyCorrectTensTrade()
    {
        const string family=UsCcssG1FullGradeTensAndOnesQuestions.ShowMultipleWays;
        var p=new Dictionary<string,int>{{"tens",6},{"ones",5},{"mode",1},{"unused",0}};
        const string oneTrade="EXPANDED=60+5;TENS=6;ONES=5;ALT_TENS=5;ALT_ONES=15";
        const string fourTrades="ONES=5;TENS=6;EXPANDED=60+5;ALT_ONES=45;ALT_TENS=2";
        Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,oneTrade));
        Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,fourTrades));
        Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,
            "EXPANDED=60+5;TENS=6;ONES=5;ALT_TENS=0;ALT_ONES=65"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,
            "EXPANDED=60+5;TENS=6;ONES=5;ALT_TENS=6;ALT_ONES=5"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,
            "EXPANDED=60+5;TENS=6;ONES=5;ALT_TENS=2;ALT_ONES=35"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,
            "EXPANDED=60+5;TENS=6;ONES=5;ALT_TENS=-1;ALT_ONES=75"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,
            "EXPANDED=60+5;TENS=6;ONES=5;ALT_TENS=2;ALT_ONES=45;EXTRA=1"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,
            "EXPANDED=60+5;TENS=6;ONES=5;ALT_TENS=2;ALT_TENS=2"));
    }

    [Fact]
    public void Lesson71NoahIncompleteTowerIsNotAFullTen()
    {
        const string family=UsCcssG1FullGradeTensAndOnesQuestions.MixedCollection;
        var noah=new Dictionary<string,int>{{"tens",4},{"ones",8},{"mode",5},{"unused",0}};
        Assert.True(UsCcssG1FullGradeTensAndOnesQuestions.Valid(family,noah));
        Assert.Equal("DECISION=DISAGREE;TENS=4;ONES=8;TOTAL=48;CLAIM=50",
            UsCcssG1FullGradeTensAndOnesQuestions.Solve(family,noah));
        Assert.True(ExactSkillContractQuestionEngine.Verify(family,noah,
            "DECISION=DISAGREE;TENS=4;ONES=8;TOTAL=48;CLAIM=50"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,noah,
            "DECISION=AGREE;TENS=4;ONES=8;TOTAL=48;CLAIM=50"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,noah,
            "DECISION=DISAGREE;TENS=5;ONES=0;TOTAL=50;CLAIM=50"));
        var fiftyTwo=new Dictionary<string,int>{{"tens",5},{"ones",2},{"mode",4},{"unused",0}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(family,fiftyTwo,
            "TENS=5;ONES=2;TOTAL=52"));
        var thirtySeven=new Dictionary<string,int>{{"tens",3},{"ones",7},{"mode",6},{"unused",0}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(family,thirtySeven,
            "TENS=3;ONES=7;TOTAL=37"));
        Assert.False(UsCcssG1FullGradeTensAndOnesQuestions.Valid(
            UsCcssG1FullGradeTensAndOnesQuestions.ReadNumber,noah));
    }

    [Fact]
    public void Lesson72TwoCardsAcceptBothOrdersAndCheckDiegoTylerMath()
    {
        const string family=UsCcssG1FullGradeTensAndOnesQuestions.ReadNumber;
        var cards=new Dictionary<string,int>{{"tens",3},{"ones",5},{"mode",4},{"unused",0}};
        const string thirtyFive="DIGITS=3,5;NUMBER=35;TENS=3;ONES=5;NAME=thirty-five";
        const string fiftyThree="DIGITS=3,5;NUMBER=53;TENS=5;ONES=3;NAME=fifty-three";
        Assert.True(UsCcssG1FullGradeTensAndOnesQuestions.Valid(family,cards));
        Assert.Equal(thirtyFive,UsCcssG1FullGradeTensAndOnesQuestions.Solve(family,cards));
        Assert.True(ExactSkillContractQuestionEngine.Verify(family,cards,thirtyFive));
        Assert.True(ExactSkillContractQuestionEngine.Verify(family,cards,fiftyThree));
        Assert.True(UsCcssG1FullGradeTensAndOnesQuestions.GradePersistedAnswer(
            family,thirtyFive,fiftyThree));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,cards,
            "DIGITS=3,5;NUMBER=53;TENS=3;ONES=5;NAME=fifty-three"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,cards,
            "DIGITS=3,5;NUMBER=53;TENS=5;ONES=3;NAME=thirty-five"));
        var diego=new Dictionary<string,int>{{"tens",8},{"ones",6},{"mode",5},{"unused",0}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(family,diego,
            "AGREE=DIEGO;TENS=8;ONES=6;NUMBER=86;NAME=eighty-six"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,diego,
            "AGREE=CLARE;TENS=8;ONES=6;NUMBER=86;NAME=eighty-six"));
        var tyler=new Dictionary<string,int>{{"tens",4},{"ones",2},{"mode",6},{"unused",0}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(family,tyler,
            "DECISION=DISAGREE;DRAWN=42;CLAIM=24;TENS=4;ONES=2"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,tyler,
            "DECISION=AGREE;DRAWN=42;CLAIM=24;TENS=4;ONES=2"));
        Assert.False(UsCcssG1FullGradeTensAndOnesQuestions.Valid(
            UsCcssG1FullGradeTensAndOnesQuestions.WriteNumeral,tyler));
        cards["tens"]=5;cards["ones"]=3;
        Assert.False(UsCcssG1FullGradeTensAndOnesQuestions.Valid(family,cards));
    }

    [Fact]
    public void Lesson73DifferentRepresentationsMatchValuesNotReversedDigitOrder()
    {
        const string f=UsCcssG1FullGradeTensAndOnesQuestions.MatchRepresentations;
        var cases=new[]{
            (m:4,t:7,o:3,expected:"MATCH=YES;TENS=7;ONES=3;NUMBER=73;EXPANDED=70+3"),
            (m:5,t:4,o:1,expected:"MATCH=NO;FIRST=41;OTHER=14"),
            (m:6,t:3,o:2,expected:"MATCH=J,S;NUMBER=32;OTHER=23"),
            (m:7,t:9,o:5,expected:"TENS=9;ONES=5;NUMBER=95;EXPANDED=90+5")
        };
        foreach(var (m,t,o,expected) in cases)
        {
            var p=new Dictionary<string,int>{{"tens",t},{"ones",o},{"mode",m},{"unused",0}};
            Assert.True(UsCcssG1FullGradeTensAndOnesQuestions.Valid(f,p));
            Assert.Equal(expected,UsCcssG1FullGradeTensAndOnesQuestions.Solve(f,p));
            Assert.True(ExactSkillContractQuestionEngine.Verify(f,p,expected));
            Assert.False(ExactSkillContractQuestionEngine.Verify(f,p,expected+";EXTRA=1"));
            p["ones"]=o+1;
            Assert.False(UsCcssG1FullGradeTensAndOnesQuestions.Valid(f,p));
        }
        var jse=new Dictionary<string,int>{{"tens",3},{"ones",2},{"mode",6},{"unused",0}};
        Assert.False(ExactSkillContractQuestionEngine.Verify(f,jse,
            "MATCH=J,E;NUMBER=32;OTHER=23"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(f,jse,
            "MATCH=J,S;NUMBER=23;OTHER=32"));
        Assert.False(UsCcssG1FullGradeTensAndOnesQuestions.Valid(
            UsCcssG1FullGradeTensAndOnesQuestions.ShowMultipleWays,jse));
        var generated=Enumerable.Range(0,560).Select(i=>
            UsCcssG1FullGradeTensAndOnesQuestions.Build(f,new Random(i*59+23))).ToArray();
        foreach(var mode in new[]{4,5,6,7})
        {
            var selected=generated.Where(q=>q.Parameters["mode"]==mode).ToArray();
            Assert.True(selected.Length>25);
            Assert.All(selected,q=>Assert.True(ExactSkillContractQuestionEngine.Verify(
                f,q.Parameters,UsCcssG1FullGradeTensAndOnesQuestions.Solve(f,q.Parameters))));
        }
    }

    [Fact]
    public void ValidTwoDigitValuesStayWithinGrade1AndWrongResponsesFail()
    {
        foreach(var family in UsCcssG1FullGradeTensAndOnesQuestions.Families)
            for(int seed=0;seed<200;seed++)
            {
                var q=UsCcssG1FullGradeTensAndOnesQuestions.Build(family,new Random(89*seed+17));
                Assert.True(UsCcssG1FullGradeTensAndOnesQuestions.Valid(family,q.Parameters));
                var correct=UsCcssG1FullGradeTensAndOnesQuestions.Solve(family,q.Parameters);
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,correct));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,"NUMBER=999"));
                Assert.False(UsCcssG1FullGradeTensAndOnesQuestions.GradePersistedAnswer(family,correct,correct+";EXTRA=1"));
            }
    }
    [Fact]
    public void TensAndOnesNotReversedAndNumberWordsUseCorrectSpelling()
    {
        var p=new Dictionary<string,int>{{"tens",5},{"ones",8},{"mode",0},{"unused",0}};
        Assert.Equal("fifty-eight",UsCcssG1FullGradeTensAndOnesQuestions.NumberWords(5,8));
        Assert.Equal("TENS=5;ONES=8;TOTAL=58",UsCcssG1FullGradeTensAndOnesQuestions.Solve(
            UsCcssG1FullGradeTensAndOnesQuestions.MixedCollection,p));
        Assert.Equal("NAME=fifty-eight",UsCcssG1FullGradeTensAndOnesQuestions.Solve(
            UsCcssG1FullGradeTensAndOnesQuestions.ReadNumber,p));
        Assert.False(UsCcssG1FullGradeTensAndOnesQuestions.Verify(
            UsCcssG1FullGradeTensAndOnesQuestions.MixedCollection,p,"TENS=8;ONES=5;TOTAL=85"));
        Assert.Equal("NUMBER=58",UsCcssG1FullGradeTensAndOnesQuestions.Solve(
            UsCcssG1FullGradeTensAndOnesQuestions.WriteNumeral,p));
        Assert.True(UsCcssG1FullGradeTensAndOnesQuestions.Verify(
            UsCcssG1FullGradeTensAndOnesQuestions.ReadNumber,p,"name = FIFTY-EIGHT"));
    }
    [Theory]
    [InlineData(4,7,0,"EXPANDED=40+7;TENS=4;ONES=7;ALT_TENS=3;ALT_ONES=17")]
    [InlineData(4,7,1,"EXPANDED=40+7;TENS=4;ONES=7;ALT_TENS=2;ALT_ONES=27")]
    [InlineData(4,7,2,"EXPANDED=40+7;TENS=4;ONES=7;ALT_TENS=1;ALT_ONES=37")]
    [InlineData(2,3,3,"EXPANDED=20+3;TENS=2;ONES=3;ALT_TENS=0;ALT_ONES=23")]
    public void OriginalShowMeYourNumberRequiresTwoDifferentButEquivalentRepresentations(
        int tens,int ones,int mode,string expected)
    {
        const string family=UsCcssG1FullGradeTensAndOnesQuestions.ShowMultipleWays;
        var p=new Dictionary<string,int>{{"tens",tens},{"ones",ones},{"mode",mode},{"unused",0}};
        Assert.True(UsCcssG1FullGradeTensAndOnesQuestions.Valid(family,p));
        Assert.Equal(expected,UsCcssG1FullGradeTensAndOnesQuestions.Solve(family,p));
        Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,expected));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,
            $"EXPANDED={10*tens}+{ones};TENS={tens};ONES={ones}"));
        int exchange=Math.Min(tens,mode+1);
        Assert.Equal(10*tens+ones,10*(tens-exchange)+(ones+10*exchange));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,
            expected.Replace("ALT_ONES="+(ones+10*exchange),
                             "ALT_ONES="+(ones+10*exchange+1))));
    }

    [Fact]
    public void EngineCanGenerateDistinctPracticedPlaceValueAnswers()
    {
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var family in UsCcssG1FullGradeTensAndOnesQuestions.Families)
        {
            var items=engine.Generate("G1-place-value-71-75",family,[family],
                ExactSkillQuestionDifficulty.Standard,5,20261018,[]);
            Assert.Equal(5,items.Count);
            Assert.Equal(5,items.Select(x=>x.Prompt).Distinct(StringComparer.Ordinal).Count());
            foreach(var item in items)
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,item.Parameters,item.CorrectAnswer));
        }
    }
}
