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
    [InlineData(3,7,6,3,"MATCH=10+6;TOTAL=16")]
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
            UsCcssG1FullGradeStartQuestions.ThreeStory,p,"TOTAL=16;REST=6"));
        Assert.False(UsCcssG1FullGradeStartQuestions.Verify(
            UsCcssG1FullGradeStartQuestions.ThreeStory,p,"TOTAL=10;REST=6"));
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
}
