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
        Assert.Equal(new[]{family},contract.AllowedQuestionFamilies);
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
    public void ShakeSpillRequiresAllUniqueTwoColorDecompositions()
    {
        var p=new Dictionary<string,int>{{"total",6}};
        var f=UsCcssG1Unit2NextFiveQuestions.Shake;
        Assert.Equal("0+6=6;1+5=6;2+4=6;3+3=6",
            UsCcssG1Unit2NextFiveQuestions.Solve(f,p));
        Assert.True(UsCcssG1Unit2NextFiveQuestions.Verify(f,p,
            "6+0=6;5+1=6;4+2=6;3+3=6"));
        Assert.False(UsCcssG1Unit2NextFiveQuestions.Verify(f,p,
            "0+6=6;1+5=6;2+4=6;2+4=6"));
    }

    [Fact]
    public void CoveredCountersRequireAddAndRelatedSubtract()
    {
        var p=new Dictionary<string,int>{{"total",8},{"visible",5}};
        var f=UsCcssG1Unit2NextFiveQuestions.Cover;
        Assert.Equal("5+3=8;8-5=3",UsCcssG1Unit2NextFiveQuestions.Solve(f,p));
        Assert.False(UsCcssG1Unit2NextFiveQuestions.Verify(f,p,"3"));
        Assert.True(UsCcssG1Unit2NextFiveQuestions.Verify(f,p,"8 - 5 = 3;5+3=8"));
    }

    [Fact]
    public void BothPartsUnknownAcceptsMultipleDistinctValidDecompositions()
    {
        var p=new Dictionary<string,int>{{"total",9},{"a",4},{"mode",2}};
        var f=UsCcssG1Unit2NextFiveQuestions.Compare;
        Assert.True(UsCcssG1Unit2NextFiveQuestions.Verify(f,p,"3+6=9;4+5=9"));
        Assert.False(UsCcssG1Unit2NextFiveQuestions.Verify(f,p,"3+6=9;6+3=9"));
        Assert.False(UsCcssG1Unit2NextFiveQuestions.Verify(f,p,"3+6=9;4+5=10"));
    }

    [Fact]
    public void CaptureSquaresChecksActualCellAndOneValidEdge()
    {
        var p=new Dictionary<string,int>{{"first",4},{"second",3}};
        var f=UsCcssG1Unit2NextFiveQuestions.Capture;
        Assert.Equal("A2:TOP",UsCcssG1Unit2NextFiveQuestions.Solve(f,p));
        Assert.True(UsCcssG1Unit2NextFiveQuestions.Verify(f,p,"a2:right"));
        Assert.False(UsCcssG1Unit2NextFiveQuestions.Verify(f,p,"B2:TOP"));
        Assert.False(UsCcssG1Unit2NextFiveQuestions.Verify(f,p,"A2:DIAGONAL"));
    }
}
