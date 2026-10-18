using Edulytics.Core.Enums;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class UsCcssG1U02NextFiveTests
{
    public static IEnumerable<object[]> SourceLessons()
    {
        var ids=new[]{
            UsCcssG1Unit2NextFiveQuestions.Pets,
            UsCcssG1Unit2NextFiveQuestions.Shake,
            UsCcssG1Unit2NextFiveQuestions.Cover,
            UsCcssG1Unit2NextFiveQuestions.Compare,
            UsCcssG1Unit2NextFiveQuestions.Capture
        };
        for(var i=0;i<5;i++)
            yield return [i+6,$"PED:US-CCSS-MATH:G1:U02:L{i+6:00}",ids[i]];
    }

    [Theory]
    [MemberData(nameof(SourceLessons))]
    public void EachOriginalLessonHasOwnExactConstructedResponse(
        int ordinal,string lessonCode,string family)
    {
        var contract=LessonPracticeContractRegistry.All.Single(x=>x.LessonCode==lessonCode);
        Assert.Equal(ordinal==10?
            new[]{family,UsCcssG1Unit2NextFiveQuestions.Mental}:
            new[]{family},contract.AllowedQuestionFamilies);
        Assert.Equal("number.whole.add_subtract",contract.SkillId);
        Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var interaction));
        Assert.Equal(LessonPracticeInteractionKind.ContractDriven,interaction!.InteractionKind);
        Assert.Equal("contract_defined",interaction.AnswerType);
        var engine=new ExactSkillContractQuestionEngine();
        var items=engine.Generate("V1G1005",family,[family],
            ExactSkillQuestionDifficulty.Standard,family==UsCcssG1Unit2NextFiveQuestions.Shake?5:12,20261017+ordinal,[]);
        Assert.Equal(family==UsCcssG1Unit2NextFiveQuestions.Shake?5:12,items.Count);
        foreach(var item in items)
        {
            Assert.Equal(AssessmentItemType.ShortAnswer,item.ItemType);
            Assert.Equal(family,item.Family);
            Assert.True(UsCcssG1Unit2NextFiveQuestions.Valid(family,item.Parameters));
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                family,item.Parameters,item.CorrectAnswer));
            Assert.True(UsCcssG1Unit2NextFiveQuestions.GradePersistedAnswer(
                family,item.CorrectAnswer,item.CorrectAnswer));
            Assert.False(ExactSkillContractQuestionEngine.Verify(
                family,item.Parameters,"999"));
        }
    }

    [Theory]
    [MemberData(nameof(SourceLessons))]
    public void LessonPicturesAreFirstPartyVisibleAndDoNotRevealFullSolution(
        int ordinal,string lessonCode,string family)
    {
        Assert.StartsWith("PED:US-CCSS-MATH:G1:U02:L",lessonCode);
        var task=UsCcssG1Unit2NextFiveQuestions.Build(family,new Random(ordinal*17));
        var json=System.Text.Json.JsonSerializer.Serialize(new {parameters=task.Parameters});
        var svg=Edulytics.Web.Presentation.PracticeMathVisualRenderer.RenderSvg(family,json);
        Assert.NotNull(svg);
        Assert.Contains("<svg ",svg);
        Assert.DoesNotContain("<image ",svg);
        Assert.DoesNotContain(task.Solution,svg);
        Assert.DoesNotContain(task.Prompt,svg);
        if (family==UsCcssG1Unit2NextFiveQuestions.Capture)
        {
            Assert.Contains("A1",svg);
            Assert.Contains("A2",svg);
            Assert.Contains("B1",svg);
            Assert.Contains("B2",svg);
        }
    }

    [Fact]
    public void CenterDay2AlsoChecksActualWithin10SubtractionWarmup()
    {
        var family=UsCcssG1Unit2NextFiveQuestions.Mental;
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
        var p=new Dictionary<string,int>{{"start",8},{"amount",2}};
        Assert.Equal("6",UsCcssG1Unit2NextFiveQuestions.Solve(family,p));
        Assert.True(UsCcssG1Unit2NextFiveQuestions.Verify(family,p,"6"));
        Assert.False(UsCcssG1Unit2NextFiveQuestions.Verify(family,p,"8"));
        var engine=new ExactSkillContractQuestionEngine();
        var items=engine.Generate("V1-G1-005","mental-subtraction",[family],
            ExactSkillQuestionDifficulty.Standard,10,20261017,[]);
        Assert.Equal(10,items.Count);
        Assert.All(items,item=>
        {
            Assert.Equal(AssessmentItemType.Numeric,item.ItemType);
            Assert.True(UsCcssG1Unit2NextFiveQuestions.Verify(
                family,item.Parameters,item.CorrectAnswer));
        });
    }

    [Fact]
    public void PetsRequiresBothOrderingsNotJustAnArithmeticTotal()
    {
        var p=new Dictionary<string,int>{{"a",3},{"b",4}};
        var f=UsCcssG1Unit2NextFiveQuestions.Pets;
        Assert.Equal("3+4=7;4+3=7",UsCcssG1Unit2NextFiveQuestions.Solve(f,p));
        Assert.True(UsCcssG1Unit2NextFiveQuestions.Verify(f,p," 4+3=7 ; 3+4=7 "));
        Assert.False(UsCcssG1Unit2NextFiveQuestions.Verify(f,p,"7"));
        Assert.False(UsCcssG1Unit2NextFiveQuestions.Verify(f,p,"3+4=7"));
    }

    [Fact]
    public void ShakeSpillAcceptsTwoDistinctRedYellowArrangementsAndOptionalExtra()
    {
        var p=new Dictionary<string,int>{{"total",6}};
        var f=UsCcssG1Unit2NextFiveQuestions.Shake;
        Assert.Equal("0+6=6;1+5=6;2+4=6;3+3=6",
            UsCcssG1Unit2NextFiveQuestions.Solve(f,p));
        Assert.True(UsCcssG1Unit2NextFiveQuestions.Verify(f,p,
            "6+0=6;5+1=6;4+2=6;3+3=6"));
        Assert.True(UsCcssG1Unit2NextFiveQuestions.Verify(f,p,
            "1+5=6;5+1=6"));
        Assert.False(UsCcssG1Unit2NextFiveQuestions.Verify(f,p,
            "0+6=6;0+6=6"));
    }

    [Fact]
    public void CoveredCountersAcceptAdditionOrSubtractionSourceStrategies()
    {
        var p=new Dictionary<string,int>{{"total",8},{"visible",5}};
        var f=UsCcssG1Unit2NextFiveQuestions.Cover;
        Assert.Equal("5+3=8;8-5=3",UsCcssG1Unit2NextFiveQuestions.Solve(f,p));
        Assert.False(UsCcssG1Unit2NextFiveQuestions.Verify(f,p,"3"));
        Assert.True(UsCcssG1Unit2NextFiveQuestions.Verify(f,p,"5+3=8"));
        Assert.True(UsCcssG1Unit2NextFiveQuestions.Verify(f,p,"8-5=3"));
        Assert.True(UsCcssG1Unit2NextFiveQuestions.Verify(f,p,"8 - 5 = 3;5+3=8"));
        Assert.False(UsCcssG1Unit2NextFiveQuestions.Verify(f,p,"8-3=5"));
    }

    [Fact]
    public void BothPartsUnknownAcceptsMultipleDistinctValidDecompositions()
    {
        var p=new Dictionary<string,int>{{"total",9},{"a",4},{"mode",2}};
        var f=UsCcssG1Unit2NextFiveQuestions.Compare;
        Assert.True(UsCcssG1Unit2NextFiveQuestions.Verify(f,p,"3+6=9;4+5=9;UNKNOWN=BOTH"));
        Assert.False(UsCcssG1Unit2NextFiveQuestions.Verify(f,p,"3+6=9;4+5=9"));
        Assert.False(UsCcssG1Unit2NextFiveQuestions.Verify(f,p,"3+6=9;6+3=9;UNKNOWN=BOTH"));
        Assert.False(UsCcssG1Unit2NextFiveQuestions.Verify(f,p,"3+6=9;4+5=10;UNKNOWN=BOTH"));
        Assert.False(UsCcssG1Unit2NextFiveQuestions.Verify(f,p,"3+6=9;4+5=9;UNKNOWN=TOTAL"));
    }

    [Fact]
    public void CaptureSquaresChecksActualCellAndOneValidEdge()
    {
        var p=new Dictionary<string,int>{{"first",4},{"second",3},{"mask",7}};
        var f=UsCcssG1Unit2NextFiveQuestions.Capture;
        Assert.Equal("A2:LEFT:CAPTURE",UsCcssG1Unit2NextFiveQuestions.Solve(f,p));
        Assert.True(UsCcssG1Unit2NextFiveQuestions.Verify(f,p,"a2:left:capture"));
        Assert.False(UsCcssG1Unit2NextFiveQuestions.Verify(f,p,"B2:LEFT:CAPTURE"));
        Assert.False(UsCcssG1Unit2NextFiveQuestions.Verify(f,p,"A2:TOP:CAPTURE"));
        Assert.False(UsCcssG1Unit2NextFiveQuestions.Verify(f,p,"A2:LEFT:CONTINUE"));
        p["mask"]=0;
        Assert.True(UsCcssG1Unit2NextFiveQuestions.Verify(f,p,"A2:TOP:CONTINUE"));
        Assert.False(UsCcssG1Unit2NextFiveQuestions.Verify(f,p,"A2:TOP:CAPTURE"));
    }
    [Fact]
    public void SourceTwoColorCountersAreUnassignedNotASolvedSplit()
    {
        var svg=Edulytics.Web.Presentation.PracticeMathVisualRenderer.RenderSvg(
            UsCcssG1Unit2NextFiveQuestions.Shake,
            """{"parameters":{"total":6}}""");
        Assert.NotNull(svg);
        Assert.Equal(6,System.Text.RegularExpressions.Regex.Matches(
            svg,"data-role='unassigned-two-color-counter'").Count);
        Assert.DoesNotContain("example split",svg);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    public void ReptileStoryPicturesOnlyShowInformationGivenToThePupil(int mode)
    {
        var json=System.Text.Json.JsonSerializer.Serialize(new {
            parameters=new {total=8,a=3,mode}
        });
        var svg=Edulytics.Web.Presentation.PracticeMathVisualRenderer.RenderSvg(
            UsCcssG1Unit2NextFiveQuestions.Compare,json);
        Assert.NotNull(svg);
        Assert.DoesNotContain("UNKNOWN=",svg);
        var turtles=System.Text.RegularExpressions.Regex.Matches(svg,"data-animal='turtle'").Count;
        var lizards=System.Text.RegularExpressions.Regex.Matches(svg,"data-animal='lizard'").Count;
        if(mode==0)
        {
            Assert.Equal(3,turtles);
            Assert.Equal(5,lizards);
            Assert.DoesNotContain("Lizards: ?",svg);
        }
        else if(mode==1)
        {
            Assert.Equal(3,turtles);
            Assert.Equal(0,lizards);
            Assert.Contains("Lizards: ?",svg);
            Assert.DoesNotContain("data-role='unassigned-counter'",svg);
        }
        else
        {
            Assert.Equal(0,turtles);
            Assert.Equal(0,lizards);
            Assert.Equal(8,System.Text.RegularExpressions.Regex.Matches(
                svg,"data-role='unassigned-counter'").Count);
            Assert.Contains("Turtles: ?",svg);
            Assert.Contains("Lizards: ?",svg);
        }
    }

    [Fact]
    public void PetDiagramDepictsExactlyTheTwoNamedSpecies()
    {
        var svg=Edulytics.Web.Presentation.PracticeMathVisualRenderer.RenderSvg(
            UsCcssG1Unit2NextFiveQuestions.Pets,
            """{"parameters":{"a":3,"b":4}}""");
        Assert.NotNull(svg);
        Assert.Equal(3,System.Text.RegularExpressions.Regex.Matches(
            svg,"data-animal='cat'").Count);
        Assert.Equal(4,System.Text.RegularExpressions.Regex.Matches(
            svg,"data-animal='dog'").Count);
        Assert.DoesNotContain("3+4=7",svg);
    }

    [Fact]
    public void InstructionsDoNotIncludeLiteralCopyableCompletedAnswers()
    {
        for(var seed=0;seed<64;seed++)
        {
            var shake=UsCcssG1Unit2NextFiveQuestions.Build(
                UsCcssG1Unit2NextFiveQuestions.Shake,new Random(seed));
            var capture=UsCcssG1Unit2NextFiveQuestions.Build(
                UsCcssG1Unit2NextFiveQuestions.Capture,new Random(seed));
            Assert.DoesNotContain("1+2=3; 2+1=3",shake.Prompt);
            Assert.DoesNotContain("A1:TOP:CONTINUE",capture.Prompt);
            Assert.DoesNotContain("Choose the TOP edge",capture.Prompt);
        }
    }

    [Fact]
    public void CaptureSquaresEmptyBoardAcceptsEveryLegalFirstEdgeNotOnlyTop()
    {
        var p=new Dictionary<string,int>{{"first",4},{"second",3},{"mask",0}};
        var family=UsCcssG1Unit2NextFiveQuestions.Capture;
        foreach(var edge in new[]{"TOP","RIGHT","BOTTOM","LEFT"})
        {
            Assert.True(UsCcssG1Unit2NextFiveQuestions.Verify(
                family,p,$"A2:{edge}:CONTINUE"));
            Assert.False(UsCcssG1Unit2NextFiveQuestions.Verify(
                family,p,$"A2:{edge}:CAPTURE"));
            Assert.False(UsCcssG1Unit2NextFiveQuestions.Verify(
                family,p,$"A1:{edge}:CONTINUE"));
        }
        p["mask"]=7;
        Assert.True(UsCcssG1Unit2NextFiveQuestions.Verify(
            family,p,"A2:LEFT:CAPTURE"));
        Assert.False(UsCcssG1Unit2NextFiveQuestions.Verify(
            family,p,"A2:RIGHT:CAPTURE"));
        var prompt=UsCcssG1Unit2NextFiveQuestions.Build(
            family,new Random(1)).Prompt;
        Assert.DoesNotContain("start by marking the TOP side",prompt);
    }

}
