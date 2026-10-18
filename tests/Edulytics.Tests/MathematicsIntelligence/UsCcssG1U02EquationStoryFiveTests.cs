using Edulytics.Core.Enums;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class UsCcssG1U02EquationStoryFiveTests
{
    public static IEnumerable<object[]> Cases()
    {
        var families=new[]{
            UsCcssG1Unit2EquationStoryFiveQuestions.Compare,
            UsCcssG1Unit2EquationStoryFiveQuestions.Box,
            UsCcssG1Unit2EquationStoryFiveQuestions.Two,
            UsCcssG1Unit2EquationStoryFiveQuestions.Create
        };
        for(var n=0;n<4;n++)
            yield return [n+17,$"PED:US-CCSS-MATH:G1:U02:L{n+17:00}",families[n]];
    }
    [Fact]
    public void OriginalLesson16ReusesActualCaptureSquaresAndMentalSubtraction()
    {
        var c=LessonPracticeContractRegistry.All.Single(x=>x.LessonCode==
            "PED:US-CCSS-MATH:G1:U02:L16");
        Assert.Contains(UsCcssG1Unit2NextFiveQuestions.Capture,c.AllowedQuestionFamilies);
        Assert.Contains(UsCcssG1Unit2NextFiveQuestions.Mental,c.AllowedQuestionFamilies);
        Assert.Equal(2,c.AllowedQuestionFamilies.Count);
    }
    [Theory]
    [MemberData(nameof(Cases))]
    public void AllFourSourceSpecificFamiliesGenerateGradedAndUnleakedPractice(
        int lesson,string code,string family)
    {
        var contract=LessonPracticeContractRegistry.All.Single(c=>c.LessonCode==code);
        Assert.Equal(new[]{family},contract.AllowedQuestionFamilies);
        Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var resolved));
        Assert.Equal(LessonPracticeInteractionKind.ContractDriven,resolved!.InteractionKind);
        var engine=new ExactSkillContractQuestionEngine();
        var items=engine.Generate("G1-007",family,[family],
            ExactSkillQuestionDifficulty.Standard,5,20261018+lesson,[]);
        Assert.Equal(5,items.Count);
        foreach(var item in items)
        {
            Assert.Equal(family,item.Family);
            Assert.DoesNotContain(item.CorrectAnswer,item.Prompt,StringComparison.Ordinal);
            Assert.True(UsCcssG1Unit2EquationStoryFiveQuestions.Valid(family,item.Parameters));
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                family,item.Parameters,item.CorrectAnswer));
            Assert.False(ExactSkillContractQuestionEngine.Verify(
                family,item.Parameters,"999"));
            Assert.False(ExactSkillContractQuestionEngine.Verify(
                family,item.Parameters,"1+1=99"));
            var svg=Edulytics.Web.Presentation.PracticeMathVisualRenderer.RenderSvg(
                family,System.Text.Json.JsonSerializer.Serialize(new{parameters=item.Parameters}));
            Assert.NotNull(svg);
            Assert.Contains("<svg ",svg);
            Assert.DoesNotContain("<image ",svg);
            Assert.DoesNotContain(item.CorrectAnswer,svg);
        }
    }
    [Fact]
    public void CompareUnknownTypeMattersNotJustNumericalArithmetic()
    {
        var f=UsCcssG1Unit2EquationStoryFiveQuestions.Compare;
        var p=new Dictionary<string,int>{{"a",3},{"b",4},{"sum",7},{"mode",1}};
        Assert.True(UsCcssG1Unit2EquationStoryFiveQuestions.Verify(f,p,
            "TYPE=CHANGE; EQUATION=7-3=4"));
        Assert.True(UsCcssG1Unit2EquationStoryFiveQuestions.Verify(f,p,
            "TYPE=CHANGE; EQUATION=4+3=7"));
        Assert.False(UsCcssG1Unit2EquationStoryFiveQuestions.Verify(f,p,
            "TYPE=TOTAL; EQUATION=7-3=4"));
        Assert.False(UsCcssG1Unit2EquationStoryFiveQuestions.Verify(f,p,
            "TYPE=CHANGE; EQUATION=3+4=8"));
    }
    [Fact]
    public void UnknownBoxIsSolvedInWholeMatchingEquation()
    {
        var f=UsCcssG1Unit2EquationStoryFiveQuestions.Box;
        var p=new Dictionary<string,int>{{"a",4},{"b",6},{"sum",10},{"mode",0}};
        Assert.True(UsCcssG1Unit2EquationStoryFiveQuestions.Verify(f,p,"4+6=10"));
        Assert.True(UsCcssG1Unit2EquationStoryFiveQuestions.Verify(f,p,"10=4+6"));
        Assert.False(UsCcssG1Unit2EquationStoryFiveQuestions.Verify(f,p,"6"));
        Assert.False(UsCcssG1Unit2EquationStoryFiveQuestions.Verify(f,p,"4+0=10"));
    }
    [Fact]
    public void TwoEquationsAllowRelatedFactsReversedEqualityOrCommutativity()
    {
        var f=UsCcssG1Unit2EquationStoryFiveQuestions.Two;
        var p=new Dictionary<string,int>{{"a",3},{"b",4},{"sum",7},{"mode",0}};
        Assert.True(UsCcssG1Unit2EquationStoryFiveQuestions.Verify(f,p,
            "7-3=4; 3+4=7"));
        Assert.True(UsCcssG1Unit2EquationStoryFiveQuestions.Verify(f,p,
            "7=3+4; 4+3=7"));
        Assert.False(UsCcssG1Unit2EquationStoryFiveQuestions.Verify(f,p,
            "3+4=7"));
        Assert.False(UsCcssG1Unit2EquationStoryFiveQuestions.Verify(f,p,
            "3+4=7; 3+4=7"));
        Assert.False(UsCcssG1Unit2EquationStoryFiveQuestions.Verify(f,p,
            "3+4=7;2+5=7"));
    }
    [Fact]
    public void StoryAuthoringMustPreserveActionAndQuantities()
    {
        var f=UsCcssG1Unit2EquationStoryFiveQuestions.Create;
        var p=new Dictionary<string,int>{{"a",3},{"b",4},{"sum",7},{"mode",0}};
        Assert.True(UsCcssG1Unit2EquationStoryFiveQuestions.Verify(f,p,
            "STORY=I had 3 pencils. I got 4 more pencils and now I have 7 pencils."));
        Assert.False(UsCcssG1Unit2EquationStoryFiveQuestions.Verify(f,p,
            "STORY=I had 3 pencils. I got 5 more pencils and now I have 8 pencils."));
        Assert.False(UsCcssG1Unit2EquationStoryFiveQuestions.Verify(f,p,
            "STORY=I had 3 pencils. I gave away 4 pencils and now I have 7 pencils."));
        var remove=new Dictionary<string,int>{{"a",3},{"b",4},{"sum",7},{"mode",1}};
        Assert.True(UsCcssG1Unit2EquationStoryFiveQuestions.Verify(f,remove,
            "STORY=I had 7 beads. I gave away 4 beads and now I have 3 beads."));
        Assert.False(UsCcssG1Unit2EquationStoryFiveQuestions.Verify(f,remove,
            "STORY=I had 7 beads. I got 4 more beads and now I have 3 beads."));
    }
    [Fact]
    public void EveryStoryTypeIsActuallySampledAndParametersAreFailClosed()
    {
        foreach(var f in new[]{UsCcssG1Unit2EquationStoryFiveQuestions.Compare,
            UsCcssG1Unit2EquationStoryFiveQuestions.Box,
            UsCcssG1Unit2EquationStoryFiveQuestions.Create})
        {
            var modes=new HashSet<int>();
            for(var seed=0;seed<125;seed++)
            {
                var q=UsCcssG1Unit2EquationStoryFiveQuestions.Build(f,
                    new Random(seed*61+23));
                modes.Add(q.Parameters["mode"]);
                Assert.True(UsCcssG1Unit2EquationStoryFiveQuestions.Valid(f,q.Parameters));
                Assert.True(UsCcssG1Unit2EquationStoryFiveQuestions.Verify(
                    f,q.Parameters,UsCcssG1Unit2EquationStoryFiveQuestions.Solve(
                        f,q.Parameters)));
            }
            Assert.Equal(3,modes.Count);
        }
        Assert.False(UsCcssG1Unit2EquationStoryFiveQuestions.Valid(
            UsCcssG1Unit2EquationStoryFiveQuestions.Compare,
            new Dictionary<string,int>{{"a",4},{"b",8},{"sum",12},{"mode",1}}));
    }
}
