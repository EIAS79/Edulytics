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
    public void StoryModeSupportsStartChangeResultAndSubtractionUnknowns()
    {
        for(int mode=0;mode<6;mode++)
        {
            var p=new Dictionary<string,int>{{"a",7},{"b",5},{"c",0},{"mode",mode}};
            var answer=UsCcssG1FullGradeStrategyAndTensQuestions.Solve(
                UsCcssG1FullGradeStrategyAndTensQuestions.StoryUnknowns,p);
            var expected=mode switch
            {
                0=>"UNKNOWN=12;EQUATION=7+5=12",
                1=>"UNKNOWN=5;EQUATION=7+5=12",
                2=>"UNKNOWN=7;EQUATION=7+5=12",
                3=>"UNKNOWN=7;EQUATION=12-5=7",
                4=>"UNKNOWN=5;EQUATION=12-5=7",
                _=>"UNKNOWN=12;EQUATION=12-5=7"
            };
            Assert.Equal(expected,answer);
            Assert.True(UsCcssG1FullGradeStrategyAndTensQuestions.Verify(
                UsCcssG1FullGradeStrategyAndTensQuestions.StoryUnknowns,p,expected));
            Assert.False(UsCcssG1FullGradeStrategyAndTensQuestions.Verify(
                UsCcssG1FullGradeStrategyAndTensQuestions.StoryUnknowns,p,"UNKNOWN=99;EQUATION=7+5=12"));
        }
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
