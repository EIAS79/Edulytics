using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UsCcssG1FullGradeStartingPracticeTests
{
    private static readonly string[] Codes =
    [
        "PED:US-CCSS-MATH:G1:U03:L14",
        "PED:US-CCSS-MATH:G1:U03:L15",
        "PED:US-CCSS-MATH:G1:U03:L16",
        "PED:US-CCSS-MATH:G1:U03:L17",
        "PED:US-CCSS-MATH:G1:U03:L18"
    ];
    [Fact]
    public void NewLessonsHaveFiveExactSourceGoalPracticeContracts()
    {
        var rows=Codes.Select(x=>LessonPracticeContractRegistry.All.Single(y=>y.LessonCode==x)).ToArray();
        Assert.Equal(5,rows.Length);
        Assert.All(rows,x=>Assert.Single(x.AllowedQuestionFamilies));
        foreach(var row in rows)
            foreach(var family in row.AllowedQuestionFamilies)
            {
                Assert.True(UsCcssG1FullGradeStartQuestions.Supports(family));
                Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
                Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var interaction));
                Assert.Equal(LessonPracticeInteractionKind.ContractDriven,interaction!.InteractionKind);
            }
    }
    [Fact]
    public void IndependentCorrectAndWrongAnswerChecksAcrossSeeds()
    {
        foreach(var family in UsCcssG1FullGradeStartQuestions.Families)
            for(var i=0;i<160;i++)
            {
                var problem=UsCcssG1FullGradeStartQuestions.Build(family,new Random(i*59+1));
                var answer=UsCcssG1FullGradeStartQuestions.Solve(family,problem.Parameters);
                Assert.True(UsCcssG1FullGradeStartQuestions.Valid(family,problem.Parameters));
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,problem.Parameters,answer));
                Assert.True(UsCcssG1FullGradeStartQuestions.GradePersistedAnswer(family,answer,answer));
                Assert.False(UsCcssG1FullGradeStartQuestions.GradePersistedAnswer(family,answer,"INCORRECT=999"));
                Assert.False(UsCcssG1FullGradeStartQuestions.GradePersistedAnswer(family,answer,answer+";EXTRA=1"));
            }
    }
    [Theory]
    [InlineData(17,4,8,2)]
    [InlineData(16,7,3,3)]
    public void OriginalNumberPuzzleDigitCardsCannotBeReused(
        int total,int firstCard,int secondCard,int mode)
    {
        const string f=UsCcssG1FullGradeStartQuestions.Center;
        var p=new Dictionary<string,int>{{"a",total},{"b",firstCard},{"c",secondCard},{"mode",mode}};
        Assert.True(UsCcssG1FullGradeStartQuestions.Valid(f,p));
        var answer=$"CARDS={firstCard},{secondCard}";
        Assert.Equal(answer,UsCcssG1FullGradeStartQuestions.Solve(f,p));
        Assert.True(ExactSkillContractQuestionEngine.Verify(f,p,answer));
        Assert.False(ExactSkillContractQuestionEngine.Verify(f,p,
            $"CARDS={firstCard},{firstCard}"));
        p["c"]=firstCard;
        Assert.False(UsCcssG1FullGradeStartQuestions.Valid(f,p));
    }

    [Theory]
    [InlineData(4,6,8,2,"MATCH=10+8;TOTAL=18")]
    [InlineData(3,7,6,3,"WRITE=10+6;TOTAL=16")]
    [InlineData(9,1,10,2,"MATCH=10+10;TOTAL=20")]
    public void SourceUnit3L16MatchesWholeThreeAddendExpressionToEqualTenPlusExpression(
        int first,int tenPartner,int remaining,int mode,string expected)
    {
        const string family=UsCcssG1FullGradeStartQuestions.Expression;
        var p=new Dictionary<string,int>{{"a",first},{"b",tenPartner},
            {"c",remaining},{"mode",mode}};
        Assert.True(UsCcssG1FullGradeStartQuestions.Valid(family,p));
        Assert.Equal(expected,UsCcssG1FullGradeStartQuestions.Solve(family,p));
        Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,expected));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,
            $"MATCH=10+{remaining-1};TOTAL={first+tenPartner+remaining}"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,
            $"TRUE=YES;TOTAL={first+tenPartner+remaining}"));
    }

    [Theory]
    [InlineData(9,5,2,"MATCH=10+4;TOTAL=14")]
    [InlineData(8,6,3,"PAIR=9+5;TOTAL=14")]
    [InlineData(7,8,2,"MATCH=10+5;TOTAL=15")]
    public void OriginalAdditionPatternsRequireCardMatchingAndSameSumRegrouping(
        int a,int b,int mode,string expected)
    {
        const string family=UsCcssG1FullGradeStartQuestions.Pattern;
        var p=new Dictionary<string,int>{{"a",a},{"b",b},{"c",0},{"mode",mode}};
        Assert.True(UsCcssG1FullGradeStartQuestions.Valid(family,p));
        Assert.Equal(expected,UsCcssG1FullGradeStartQuestions.Solve(family,p));
        Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,expected));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,
            $"EQUAL=YES;TOTAL={a+b}"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,
            mode==2?$"MATCH=10+{a+b-9};TOTAL={a+b}":
                     $"PAIR={a+1}+{b};TOTAL={a+b}"));
        Assert.Equal(a+b,a+1+b-1);
    }

    [Fact]
    public void ThreeAddendAndMakeTenSpecificWrongAnswerControls()
    {
        var p=new Dictionary<string,int>{{"a",3},{"b",7},{"c",6},{"mode",1}};
        Assert.True(UsCcssG1FullGradeStartQuestions.Verify(
            UsCcssG1FullGradeStartQuestions.ThreeStory,p,"TOTAL=16;EQUATION=3+7+6=16"));

        // Story authoring permits any valid grouping/order of the exact three
        // source quantities; equal totals from different groups are not valid.
        Assert.True(ExactSkillContractQuestionEngine.Verify(
            UsCcssG1FullGradeStartQuestions.ThreeStory,p,
            "EQUATION=7+6+3=16;TOTAL=16"));
        Assert.True(ExactSkillContractQuestionEngine.Verify(
            UsCcssG1FullGradeStartQuestions.ThreeStory,p,
            "TOTAL=16;EQUATION=6+3+7=16"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(
            UsCcssG1FullGradeStartQuestions.ThreeStory,p,
            "TOTAL=16;EQUATION=2+8+6=16"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(
            UsCcssG1FullGradeStartQuestions.ThreeStory,p,
            "TOTAL=16;EQUATION=3+7+6=15"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(
            UsCcssG1FullGradeStartQuestions.ThreeStory,p,
            "TOTAL=16;EQUATION=3+7+6=16;EXTRA=1"));
        Assert.False(UsCcssG1FullGradeStartQuestions.Verify(
            UsCcssG1FullGradeStartQuestions.ThreeStory,p,"TOTAL=10;EQUATION=3+7+6=16"));
        Assert.False(UsCcssG1FullGradeStartQuestions.Verify(
            UsCcssG1FullGradeStartQuestions.ThreeStory,p,"TOTAL=16;EQUATION=3+7+6=15"));
        Assert.False(UsCcssG1FullGradeStartQuestions.Verify(
            UsCcssG1FullGradeStartQuestions.ThreeStory,p,"TOTAL=16;REST=6"));
        Assert.True(UsCcssG1FullGradeStartQuestions.Verify(
            UsCcssG1FullGradeStartQuestions.Expression,p,"TRUE=NO;TOTAL=16"));
        Assert.False(UsCcssG1FullGradeStartQuestions.Verify(
            UsCcssG1FullGradeStartQuestions.Expression,p,"TRUE=YES;TOTAL=16"));
        var ten=new Dictionary<string,int>{{"a",9},{"b",5},{"c",0},{"mode",0}};
        Assert.True(UsCcssG1FullGradeStartQuestions.Verify(
            UsCcssG1FullGradeStartQuestions.MakeTen,ten,"MOVE=1;REST=4;TOTAL=14"));
        Assert.False(UsCcssG1FullGradeStartQuestions.Verify(
            UsCcssG1FullGradeStartQuestions.MakeTen,ten,"MOVE=1;REST=5;TOTAL=14"));
    }
    [Fact]
    public void Original2021ThreeBirdGroupsAreARealGeneratedSourceMode()
    {
        const string family=UsCcssG1FullGradeStartQuestions.ThreeStory;
        var source=new Dictionary<string,int>{{"a",7},{"b",3},{"c",8},{"mode",2}};
        Assert.True(UsCcssG1FullGradeStartQuestions.Valid(family,source));
        Assert.True(ExactSkillContractQuestionEngine.Verify(family,source,
            "TOTAL=18;EQUATION=7+8+3=18"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,source,
            "TOTAL=18;EQUATION=7+7+4=18"));
        source["c"]=9;
        Assert.False(UsCcssG1FullGradeStartQuestions.Valid(family,source));
        var seen=Enumerable.Range(0,360).Select(n=>
            UsCcssG1FullGradeStartQuestions.Build(family,new Random(n*47+9)))
            .Where(q=>q.Parameters["mode"]==2).ToArray();
        Assert.True(seen.Length>40);
        Assert.All(seen,q=>
        {
            Assert.Contains("7 blue birds",q.Prompt);
            Assert.Contains("8 brown birds",q.Prompt);
            Assert.Contains("3 baby birds",q.Prompt);
            Assert.True(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,
                UsCcssG1FullGradeStartQuestions.Solve(family,q.Parameters)));
        });
    }

    [Fact]
    public void OriginalNinePlusGameRequiresTwoCorrectStudentConstructedEquations()
    {
        const string family=UsCcssG1FullGradeStartQuestions.MakeTen;
        var original=new Dictionary<string,int>{{"a",9},{"b",5},{"c",0},{"mode",2}};
        const string equivalent="EQUATION=9+5=14;TEN_EQUATION=10+4=14;TOTAL=14";
        Assert.True(UsCcssG1FullGradeStartQuestions.Valid(family,original));
        Assert.Equal(equivalent,UsCcssG1FullGradeStartQuestions.Solve(family,original));
        Assert.True(ExactSkillContractQuestionEngine.Verify(family,original,equivalent));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,original,
            "EQUATION=9+5=14;TEN_EQUATION=10+3=13;TOTAL=14"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,original,
            "MOVE=1;REST=4;TOTAL=14"));
        original["a"]=8;
        Assert.False(UsCcssG1FullGradeStartQuestions.Valid(family,original));
        var tasks=Enumerable.Range(0,350)
            .Select(n=>UsCcssG1FullGradeStartQuestions.Build(family,new Random(53*n+19)))
            .Where(q=>q.Parameters["mode"]==2).ToArray();
        Assert.True(tasks.Length>35);
        Assert.All(tasks,q=>
        {
            Assert.Equal(9,q.Parameters["a"]);
            Assert.Contains("Nine Plus Game",q.Prompt);
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                family,q.Parameters,UsCcssG1FullGradeStartQuestions.Solve(family,q.Parameters)));
        });
    }

    [Fact]
    public void ProductionEngineGeneratesFiveDistinctAndGradeableQuestionsPerFamily()
    {
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var family in UsCcssG1FullGradeStartQuestions.Families)
        {
            var pool=engine.Generate("full-grade-new-51-55",family,[family],
                ExactSkillQuestionDifficulty.Standard,5,20261018,[]);
            Assert.Equal(5,pool.Count);
            Assert.Equal(5,pool.Select(x=>x.Prompt).Distinct(StringComparer.Ordinal).Count());
            foreach(var q in pool)
            {
                Assert.True(UsCcssG1FullGradeStartQuestions.Valid(family,q.Parameters));
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,q.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,"ANSWER=999"));
            }
        }
    }
    [Fact]
    public void CenterAdditionNeverExceedsOriginalWithin20Scope()
    {
        const string family=UsCcssG1FullGradeStartQuestions.Center;
        var invalid=new Dictionary<string,int>{{"a",19},{"b",2},{"c",0},{"mode",0}};
        Assert.False(UsCcssG1FullGradeStartQuestions.Valid(family,invalid));
        invalid["a"]=20;invalid["b"]=1;
        Assert.False(UsCcssG1FullGradeStartQuestions.Valid(family,invalid));
        invalid["a"]=19;invalid["b"]=1;
        Assert.True(UsCcssG1FullGradeStartQuestions.Valid(family,invalid));

        var generatedAdditionCount=0;
        for(var seed=0;seed<1000;seed++)
        {
            var question=UsCcssG1FullGradeStartQuestions.Build(family,new Random(seed*67+11));
            if(question.Parameters["mode"]!=0)continue;
            generatedAdditionCount++;
            Assert.InRange(question.Parameters["a"]+question.Parameters["b"],0,20);
            Assert.True(UsCcssG1FullGradeStartQuestions.Valid(family,question.Parameters));
            Assert.True(UsCcssG1FullGradeStartQuestions.Verify(
                family,question.Parameters,UsCcssG1FullGradeStartQuestions.Solve(family,question.Parameters)));
        }
        Assert.True(generatedAdditionCount>100,"Expected randomized addition questions");
    }

}
