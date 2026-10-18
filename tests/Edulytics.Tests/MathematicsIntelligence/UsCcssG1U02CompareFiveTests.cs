using Edulytics.Core.Enums;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Independent generative/negative checks for one V1 five-lesson batch.</summary>
public sealed class UsCcssG1U02CompareFiveTests
{
    public static IEnumerable<object[]> Cases()
    {
        var families=new[]
        {
            UsCcssG1Unit2CompareFiveQuestions.Towers,
            UsCcssG1Unit2CompareFiveQuestions.Supplies,
            UsCcssG1Unit2CompareFiveQuestions.Data,
            UsCcssG1Unit2CompareFiveQuestions.Related,
            UsCcssG1Unit2CompareFiveQuestions.Mixed
        };
        for(var i=0;i<5;i++)
            yield return [i+11,$"PED:US-CCSS-MATH:G1:U02:L{i+11:00}",families[i]];
    }

    [Theory]
    [MemberData(nameof(Cases))]
    public void SourceLessonHasExactlyItsOwnFiveActualAuthoredGradeableItems(
        int originalNumber,string code,string family)
    {
        var contract=LessonPracticeContractRegistry.All.Single(x=>x.LessonCode==code);
        Assert.Equal(new[]{family},contract.AllowedQuestionFamilies);
        Assert.Equal("number.whole.add_subtract",contract.SkillId);
        Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var interaction));
        Assert.Equal(LessonPracticeInteractionKind.ContractDriven,interaction!.InteractionKind);
        var engine=new ExactSkillContractQuestionEngine();
        var items=engine.Generate("V1-G1-006",family,[family],
            ExactSkillQuestionDifficulty.Standard,5,20261018+originalNumber,[]);
        Assert.Equal(5,items.Count);
        foreach(var item in items)
        {
            Assert.Equal(family,item.Family);
            Assert.Equal(AssessmentItemType.ShortAnswer,item.ItemType);
            Assert.True(UsCcssG1Unit2CompareFiveQuestions.Valid(family,item.Parameters));
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                family,item.Parameters,item.CorrectAnswer));
            Assert.True(UsCcssG1Unit2CompareFiveQuestions.GradePersistedAnswer(
                family,item.CorrectAnswer,item.CorrectAnswer));
            Assert.False(ExactSkillContractQuestionEngine.Verify(
                family,item.Parameters,"999"));
            Assert.False(UsCcssG1Unit2CompareFiveQuestions.GradePersistedAnswer(
                family,item.CorrectAnswer,"1+1=2"));
            Assert.Contains("equation",item.Prompt,StringComparison.OrdinalIgnoreCase);
        }
    }

    [Theory]
    [MemberData(nameof(Cases))]
    public void FirstPartyVisualShowsGivenCountsWithoutDisclosingDifference(
        int originalNumber,string code,string family)
    {
        var p=new Dictionary<string,int>
        {{"small",4},{"big",9},{"other",3},{"mode",family==UsCcssG1Unit2CompareFiveQuestions.Mixed?2:0}};
        var json=System.Text.Json.JsonSerializer.Serialize(new{parameters=p});
        var svg=Edulytics.Web.Presentation.PracticeMathVisualRenderer.RenderSvg(family,json);
        Assert.NotNull(svg);
        Assert.Contains("<svg ",svg);
        Assert.DoesNotContain("<image ",svg);
        Assert.DoesNotContain("9-4=5",svg);
        Assert.DoesNotContain("4+5=9",svg);
    }

    [Fact]
    public void DifferenceIsNotWronglyGradedAsSum()
    {
        var p=new Dictionary<string,int>{{"small",5},{"big",9},{"other",2},{"mode",0}};
        var family=UsCcssG1Unit2CompareFiveQuestions.Towers;
        Assert.Equal("5+4=9",UsCcssG1Unit2CompareFiveQuestions.Solve(family,p));
        Assert.True(UsCcssG1Unit2CompareFiveQuestions.Verify(family,p,"9 - 5 = 4"));
        Assert.True(UsCcssG1Unit2CompareFiveQuestions.Verify(family,p,"4 + 5 = 9"));
        Assert.False(UsCcssG1Unit2CompareFiveQuestions.Verify(family,p,"9+5=14"));
        Assert.False(UsCcssG1Unit2CompareFiveQuestions.Verify(family,p,"4"));
    }

    [Fact]
    public void RelatedEquationsRequireBothOperationsForSameStory()
    {
        var family=UsCcssG1Unit2CompareFiveQuestions.Related;
        var p=new Dictionary<string,int>{{"small",5},{"big",9},{"other",2},{"mode",0}};
        Assert.True(UsCcssG1Unit2CompareFiveQuestions.Verify(family,p,
            "9 - 5 = 4 ; 4 + 5 = 9"));
        Assert.False(UsCcssG1Unit2CompareFiveQuestions.Verify(family,p,"5+4=9"));
        Assert.False(UsCcssG1Unit2CompareFiveQuestions.Verify(family,p,
            "5+4=9;4+5=9"));
        Assert.False(UsCcssG1Unit2CompareFiveQuestions.Verify(family,p,
            "5+4=9;9-4=5"));
    }

    [Fact]
    public void MixedStoriesActuallyGenerateThreeDistinctUnknownTypes()
    {
        var f=UsCcssG1Unit2CompareFiveQuestions.Mixed;
        var seen=new HashSet<int>();
        for(var seed=0;seed<120;seed++)
        {
            var q=UsCcssG1Unit2CompareFiveQuestions.Build(f,new Random(seed*43+7));
            var mode=q.Parameters["mode"];
            seen.Add(mode);
            Assert.True(UsCcssG1Unit2CompareFiveQuestions.Verify(f,q.Parameters,
                UsCcssG1Unit2CompareFiveQuestions.Solve(f,q.Parameters)));
            Assert.False(UsCcssG1Unit2CompareFiveQuestions.Verify(f,q.Parameters,
                "999"));
            if(mode==0)Assert.Contains("Total Unknown",q.Prompt);
            if(mode==1)Assert.Contains("One Part Unknown",q.Prompt);
            if(mode==2)Assert.Contains("Difference Unknown",q.Prompt);
        }
        Assert.Equal(new HashSet<int>{0,1,2},seen);
    }

    [Fact]
    public void FailClosedOnMalformedOrOutsideGradeOneParameters()
    {
        var f=UsCcssG1Unit2CompareFiveQuestions.Towers;
        Assert.False(UsCcssG1Unit2CompareFiveQuestions.Valid(f,
            new Dictionary<string,int>{{"small",5},{"big",16},{"other",2},{"mode",0}}));
        Assert.False(UsCcssG1Unit2CompareFiveQuestions.Valid(f,
            new Dictionary<string,int>{{"small",5},{"big",7},{"other",2}}));
        Assert.False(UsCcssG1Unit2CompareFiveQuestions.GradePersistedAnswer(
            f,"5+4=9","9 - 5 = 4; 5 + 4 = 9"));
        Assert.False(UsCcssG1Unit2CompareFiveQuestions.GradePersistedAnswer(
            f,"5+4=9","5+4=10"));
    }
}
