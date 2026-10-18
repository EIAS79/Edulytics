using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class UsCcssG1TeenFiveTests
{
    private static readonly string[] Codes=[
        "PED:US-CCSS-MATH:G1:U03:L09",
        "PED:US-CCSS-MATH:G1:U03:L10",
        "PED:US-CCSS-MATH:G1:U03:L11",
        "PED:US-CCSS-MATH:G1:U03:L12",
        "PED:US-CCSS-MATH:G1:U03:L13"
    ];
    [Fact]
    public void ExactFiveCanonicalLessonsHaveTwoOriginalSourceQuestionFamiliesEach()
    {
        var contracts=Codes.Select(c=>LessonPracticeContractRegistry.All
            .Single(x=>x.LessonCode==c)).ToArray();
        Assert.Equal(5,contracts.Length);
        Assert.All(contracts,c=>Assert.Equal(2,c.AllowedQuestionFamilies.Count));
        Assert.Equal("usccss.g1.ten_as_unit",contracts[0].SkillId);
        Assert.Contains(UsCcssG1TeenFiveQuestions.TenFrame,contracts[0].AllowedQuestionFamilies);
        Assert.Contains(UsCcssG1TeenFiveQuestions.ChangeStory,contracts[4].AllowedQuestionFamilies);
        foreach(var c in contracts)foreach(var family in c.AllowedQuestionFamilies)
        {
            Assert.True(UsCcssG1TeenFiveQuestions.Supports(family));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var interaction));
            Assert.Equal(LessonPracticeInteractionKind.ContractDriven,interaction!.InteractionKind);
        }
    }
    [Fact]
    public void TenFamiliesGenerateDistinctTenItemMathAndRejectWrongResponses()
    {
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var family in UsCcssG1TeenFiveQuestions.Families)
        {
            var qs=engine.Generate("g1010-10-pool",family,[family],
                ExactSkillQuestionDifficulty.Standard,10,20261018,[]);
            Assert.Equal(10,qs.Count);
            Assert.Equal(10,qs.Select(x=>x.Prompt).Distinct(StringComparer.OrdinalIgnoreCase).Count());
            foreach(var q in qs)
            {
                Assert.True(UsCcssG1TeenFiveQuestions.Valid(family,q.Parameters));
                Assert.True(ExactSkillContractQuestionEngine.Verify(
                    family,q.Parameters,q.CorrectAnswer));
                Assert.True(UsCcssG1TeenFiveQuestions.GradePersistedAnswer(
                    family,q.CorrectAnswer,q.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,"999"));
                Assert.False(UsCcssG1TeenFiveQuestions.GradePersistedAnswer(
                    family,q.CorrectAnswer,"999"));
                var json=System.Text.Json.JsonSerializer.Serialize(new{parameters=q.Parameters});
                var svg=Edulytics.Web.Presentation.PracticeMathVisualRenderer.RenderSvg(family,json);
                Assert.NotNull(svg);
                Assert.Contains("<svg ",svg);
                Assert.DoesNotContain("<image ",svg);
                Assert.DoesNotContain(q.CorrectAnswer,svg);
            }
        }
    }
    [Fact]
    public void AcrossRandomSeedsEveryGeneratedMathProblemIsValid()
    {
        foreach(var family in UsCcssG1TeenFiveQuestions.Families)
            for(int seed=0;seed<240;seed++)
            {
                var q=UsCcssG1TeenFiveQuestions.Build(family,new Random(47*seed+137));
                Assert.True(UsCcssG1TeenFiveQuestions.Valid(family,q.Parameters));
                var expected=UsCcssG1TeenFiveQuestions.Solve(family,q.Parameters);
                Assert.True(UsCcssG1TeenFiveQuestions.Verify(family,q.Parameters,expected));
                Assert.True(UsCcssG1TeenFiveQuestions.GradePersistedAnswer(family,expected,expected));
            }
    }
    [Fact]
    public void EqualVisibleQuestionsShareExposureFingerprintsAcrossSessions()
    {
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var family in UsCcssG1TeenFiveQuestions.Families)
        {
            var seen=new Dictionary<string,string>(StringComparer.Ordinal);
            for(var seed=1;seed<=180;seed++)
            {
                var q=Assert.Single(engine.Generate("g1010-history",family,[family],
                    ExactSkillQuestionDifficulty.Standard,1,seed,[]));
                if(seen.TryGetValue(q.Prompt,out var previous))
                    Assert.Equal(previous,q.ExposureFingerprint);
                else seen[q.Prompt]=q.ExposureFingerprint;
            }
            Assert.True(seen.Count>=10,"Insufficient task diversity: "+family);
            var first=Assert.Single(engine.Generate("g1010-history",family,[family],
                ExactSkillQuestionDifficulty.Standard,1,17,[]));
            var next=Assert.Single(engine.Generate("g1010-history",family,[family],
                ExactSkillQuestionDifficulty.Standard,1,17,[first.ExposureFingerprint]));
            Assert.NotEqual(first.Prompt,next.Prompt);
        }
    }
    [Fact]
    public void StoryMeaningAndTensNegativeControlsCannotBeGradedCorrect()
    {
        var p=new Dictionary<string,int>{{"a",15},{"b",3},{"c",1},{"mode",1}};
        Assert.True(UsCcssG1TeenFiveQuestions.Verify(
            UsCcssG1TeenFiveQuestions.TeenStories,p,
            "UNKNOWN=12;ADDITION=12+3=15;SUBTRACTION=15-3=12"));
        Assert.True(UsCcssG1TeenFiveQuestions.Verify(
            UsCcssG1TeenFiveQuestions.TeenStories,p,
            "UNKNOWN=12;ADDITION=3+12=15;SUBTRACTION=15-3=12"));
        Assert.False(UsCcssG1TeenFiveQuestions.Verify(
            UsCcssG1TeenFiveQuestions.TeenStories,p,
            "UNKNOWN=12;ADDITION=2+13=15;SUBTRACTION=15-3=12"));
        Assert.True(UsCcssG1TeenFiveQuestions.Verify(
            UsCcssG1TeenFiveQuestions.ChangeStory,p,
            "LEFT=12;EQUATION=15-12=3"));
        Assert.False(UsCcssG1TeenFiveQuestions.Verify(
            UsCcssG1TeenFiveQuestions.ChangeStory,p,
            "LEFT=3;EQUATION=15-3=12"));
        Assert.False(UsCcssG1TeenFiveQuestions.Verify(
            UsCcssG1TeenFiveQuestions.TenFrame,p,
            "TEN=10;ONES=5;EQUATION=10+5=15"));
    }
    [Theory]
    [InlineData(19, "TEN=1;ONES=9;EQUATION=10+9=19")]
    [InlineData(20, "TEN=2;ONES=0;EQUATION=10+10=20")]
    public void OriginalNumberCardBoundaryDistinguishesOneTenFromTwoTens(
        int number,string expected)
    {
        var p=new Dictionary<string,int>{{"a",number},{"b",1},{"c",0},{"mode",0}};
        var family=UsCcssG1TeenFiveQuestions.TenFrame;
        Assert.True(UsCcssG1TeenFiveQuestions.Verify(family,p,expected));
        if(number==20)
        {
            Assert.False(UsCcssG1TeenFiveQuestions.Verify(family,p,
                "TEN=1;ONES=10;EQUATION=10+10=20"));
            var missing=UsCcssG1TeenFiveQuestions.TenMissing;
            Assert.True(UsCcssG1TeenFiveQuestions.Verify(missing,p,
                "MISSING=10;CHECK=10+10=20"));
        }
    }

    [Theory]
    [InlineData(0, "MISSING=6;CHECK=10+6=16")]
    [InlineData(1, "MISSING=16;CHECK=10+6=16")]
    [InlineData(2, "MISSING=10;CHECK=10+6=16")]
    [InlineData(3, "MISSING=10;CHECK=10+6=16")]
    public void SourceTenEquationUnknownPositionHasIndependentMathematicalTruth(
        int mode,string correct)
    {
        var p=new Dictionary<string,int>{{"a",16},{"b",1},{"c",0},{"mode",mode}};
        var family=UsCcssG1TeenFiveQuestions.TenMissing;
        Assert.True(UsCcssG1TeenFiveQuestions.Verify(family,p,correct));
        Assert.True(UsCcssG1TeenFiveQuestions.GradePersistedAnswer(
            family,UsCcssG1TeenFiveQuestions.Solve(family,p),correct));
        Assert.False(UsCcssG1TeenFiveQuestions.Verify(
            family,p,"MISSING=6;CHECK=10+6=15"));
        var wrong=mode==0?"MISSING=10;CHECK=10+6=16":
                   mode==1?"MISSING=6;CHECK=10+6=16":
                   "MISSING=16;CHECK=10+6=16";
        Assert.False(UsCcssG1TeenFiveQuestions.Verify(family,p,wrong));
    }
    [Theory]
    [InlineData(0,6)]
    [InlineData(1,6)]
    [InlineData(2,10)]
    [InlineData(3,6)]
    public void TeenSubtractionAndAdditionRelatedUnknownsArePositionAware(
        int mode,int correctMissing)
    {
        var p=new Dictionary<string,int>{{"a",16},{"b",1},{"c",0},{"mode",mode}};
        var family=UsCcssG1TeenFiveQuestions.TeenRelations;
        Assert.True(UsCcssG1TeenFiveQuestions.Verify(family,p,
            $"MISSING={correctMissing};RELATED=10+6=16"));
        Assert.False(UsCcssG1TeenFiveQuestions.Verify(family,p,
            $"MISSING={(correctMissing==10?6:10)};RELATED=10+6=16"));
    }
    [Fact]
    public void OriginalG1010VisualsDoNotShowAnswersOrInventHiddenCounters()
    {
        foreach(var family in UsCcssG1TeenFiveQuestions.Families)
        for(int n=11;n<=19;n++)
        {
            var p=new Dictionary<string,int>{{"a",n},{"b",1},{"c",1},{"mode",0}};
            if(!UsCcssG1TeenFiveQuestions.Valid(family,p))continue;
            var svg=Edulytics.Web.Presentation.PracticeMathVisualRenderer.RenderSvg(
                family,System.Text.Json.JsonSerializer.Serialize(new{parameters=p}));
            Assert.NotNull(svg);
            Assert.DoesNotContain("HIDDEN=",svg);
            Assert.DoesNotContain("REMAIN=",svg);
            if(family==UsCcssG1TeenFiveQuestions.TenFrame)
            {
                Assert.Equal(10,System.Text.RegularExpressions.Regex.Matches(svg!,
                    "data-role='ten-counter'").Count);
                Assert.Equal(10,System.Text.RegularExpressions.Regex.Matches(svg!,
                    "data-role='empty-frame-slot'").Count);
                Assert.DoesNotContain("ONES="+(n-10),svg);
            }
        }
    }
}
