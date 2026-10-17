using Edulytics.Core.Enums;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class UsCcssG1Unit2AuthoredStoryTests
{
    [Theory]
    [InlineData(UsCcssG1Unit2StoryQuestions.WriteExpression)]
    [InlineData(UsCcssG1Unit2StoryQuestions.WriteEquation)]
    [InlineData(UsCcssG1Unit2StoryQuestions.WriteChangeEquation)]
    public void ExactGeneratedAuthoredResponsesUseTextNotBinaryChoice(string family)
    {
        Assert.True(LessonPracticeInteractionRegistry.TryResolve(family, out var interaction));
        Assert.Equal(LessonPracticeInteractionKind.ContractDriven,interaction!.InteractionKind);
        Assert.Equal("contract_defined",interaction.AnswerType);
        var engine=new ExactSkillContractQuestionEngine();
        var questions=engine.Generate("v1-g1-004-authored",family,[family],
            ExactSkillQuestionDifficulty.Standard,12,20261017,[]);
        Assert.Equal(12,questions.Count);
        foreach(var q in questions)
        {
            Assert.Equal(AssessmentItemType.ShortAnswer,q.ItemType);
            Assert.Contains("WRITE",q.Prompt);
            Assert.True(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,q.CorrectAnswer));
            Assert.True(UsCcssG1Unit2StoryQuestions.GradePersistedAuthoredAnswer(
                family,q.CorrectAnswer,"  "+string.Join(" ",q.CorrectAnswer.ToCharArray())+"  "));
            Assert.False(UsCcssG1Unit2StoryQuestions.GradePersistedAuthoredAnswer(
                family,q.CorrectAnswer,q.CorrectAnswer=="1"?"2":"1"));
            Assert.False(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,
                (q.CorrectAnswer.Contains('+') ? q.CorrectAnswer.Replace('+','-') : q.CorrectAnswer.Replace('-','+'))));
            Assert.False(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,"not an equation"));
            Assert.False(ExactSkillContractQuestionEngine.Verify(family,
                q.Parameters.ToDictionary(k => k.Key,k => k.Key=="start" ? 99 : k.Value),q.CorrectAnswer));
            var expected=q.CorrectAnswer;
            Assert.Equal(family!=UsCcssG1Unit2StoryQuestions.WriteExpression,
                expected.Contains('='));
        }
    }

    [Theory]
    [InlineData("PED:US-CCSS-MATH:G1:U02:L01",
        UsCcssG1Unit2StoryQuestions.WriteExpression)]
    [InlineData("PED:US-CCSS-MATH:G1:U02:L02",
        UsCcssG1Unit2StoryQuestions.WriteEquation)]
    [InlineData("PED:US-CCSS-MATH:G1:U02:L03",
        UsCcssG1Unit2StoryQuestions.WriteChangeEquation)]
    public void ActualLearnerPracticeComposesAndGradesAuthoredTasks(
        string lessonCode, string writtenFamily)
    {
        var contract = LessonPracticeContractRegistry.All
            .Single(x => x.LessonCode == lessonCode).ToLegacyStage18Contract();
        var items = new Edulytics.Services.Practice.Stage18SkillContractPracticeEngine()
            .GenerateComposed(
                Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), contract,
                Edulytics.Services.Practice.StudentPrivatePracticeDifficulty.MyLevel,
                6, 20261017, [], Guid.NewGuid());
        Assert.Equal(6, items.Count);
        Assert.Contains(items, x => x.GenerationFamily == writtenFamily);
        var check = new Edulytics.Services.Practice.PracticeSessionQualityValidator()
            .Validate(contract, items, 6);
        Assert.True(check.IsReady,
            string.Join("; ", check.ReasonCodes));
        foreach (var item in items.Where(x => x.GenerationFamily == writtenFamily))
        {
            Assert.True(UsCcssG1Unit2StoryQuestions.GradePersistedAuthoredAnswer(
                item.GenerationFamily, item.CorrectAnswer, item.CorrectAnswer));
            Assert.False(UsCcssG1Unit2StoryQuestions.GradePersistedAuthoredAnswer(
                item.GenerationFamily, item.CorrectAnswer, "8"));
        }
    }

    [Fact]
    public void Unit2LessonFourGeneratesAndGradesStudentWrittenEquations()
    {
        var family=UsCcssG1Unit2Part2Questions.WriteResultOrChangeEquation;
        Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var ui));
        Assert.Equal(LessonPracticeInteractionKind.ContractDriven,ui!.InteractionKind);
        Assert.Equal("contract_defined",ui.AnswerType);
        var lesson=LessonPracticeContractRegistry.All.Single(
            x => x.LessonCode=="PED:US-CCSS-MATH:G1:U02:L04");
        Assert.Contains(family,lesson.AllowedQuestionFamilies);
        var contract=lesson.ToLegacyStage18Contract();
        var items=new Edulytics.Services.Practice.Stage18SkillContractPracticeEngine()
            .GenerateComposed(Guid.NewGuid(),Guid.NewGuid(),Guid.NewGuid(),contract,
                Edulytics.Services.Practice.StudentPrivatePracticeDifficulty.MyLevel,
                6,20261017,[],Guid.NewGuid());
        Assert.Equal(6,items.Count);
        Assert.Contains(items,x=>x.GenerationFamily==family);
        var quality=new Edulytics.Services.Practice.PracticeSessionQualityValidator()
            .Validate(contract,items,6);
        Assert.True(quality.IsReady,string.Join(";",quality.ReasonCodes));
        foreach(var item in items.Where(x=>x.GenerationFamily==family))
        {
            Assert.Equal(AssessmentItemType.ShortAnswer,item.ItemType);
            Assert.Contains('=',item.CorrectAnswer);
            Assert.True(UsCcssG1Unit2Part2Questions.GradePersistedAuthoredAnswer(
                family,item.CorrectAnswer," "+item.CorrectAnswer.Replace("="," = ")+" "));
            Assert.False(UsCcssG1Unit2Part2Questions.GradePersistedAuthoredAnswer(
                family,item.CorrectAnswer,"9"));
            Assert.False(UsCcssG1Unit2Part2Questions.GradePersistedAuthoredAnswer(
                family,item.CorrectAnswer,item.CorrectAnswer[..item.CorrectAnswer.IndexOf('=')]));
            Assert.False(UsCcssG1Unit2Part2Questions.Verify(family,
                new Dictionary<string,int> { ["start"]=99,["change"]=2,["mode"]=0,["context"]=0 },
                item.CorrectAnswer));
        }
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    public void AuthorPictureStoryUsesWordsQuestionAndCorrectEquation(int action)
    {
        var family=UsCcssG1Unit2Part2Questions.AuthorStory;
        var p=new Dictionary<string,int> {
            ["start"]=6,["change"]=2,["action"]=action,["flip"]=0
        };
        var answer=UsCcssG1Unit2Part2Questions.Solve(family,p);
        Assert.StartsWith("Story:",answer);
        Assert.True(UsCcssG1Unit2Part2Questions.Verify(family,p,answer));
        Assert.False(UsCcssG1Unit2Part2Questions.Verify(family,p,"8"));
        Assert.False(UsCcssG1Unit2Part2Questions.Verify(family,p,
            answer.Replace(action==0?"6+2=8":"6-2=4","6+2=9")));
        Assert.False(UsCcssG1Unit2Part2Questions.Verify(family,p,
            "Story: Six balls are shown. How many? Equation: "+
            (action==0?"6+2=8":"6-2=4")));
        var contract=LessonPracticeContractRegistry.All.Single(
            x=>x.LessonCode=="PED:US-CCSS-MATH:G1:U02:L05")
            .ToLegacyStage18Contract();
        Assert.Contains(family,contract.AllowedQuestionFamilies);
        var items=new Edulytics.Services.Practice.Stage18SkillContractPracticeEngine()
            .GenerateComposed(Guid.NewGuid(),Guid.NewGuid(),Guid.NewGuid(),contract,
                Edulytics.Services.Practice.StudentPrivatePracticeDifficulty.MyLevel,
                6,20261017,[],Guid.NewGuid());
        Assert.Contains(items,x=>x.GenerationFamily==family);
        var quality=new Edulytics.Services.Practice.PracticeSessionQualityValidator()
            .Validate(contract,items,6);
        Assert.True(quality.IsReady,string.Join(";",quality.ReasonCodes));
        foreach(var item in items.Where(x=>x.GenerationFamily==family))
        {
            Assert.Equal(AssessmentItemType.ShortAnswer,item.ItemType);
            Assert.True(UsCcssG1Unit2Part2Questions.GradePersistedStoryAnswer(
                family,item.CorrectAnswer,item.CorrectAnswer));
            Assert.False(UsCcssG1Unit2Part2Questions.GradePersistedStoryAnswer(
                family,item.CorrectAnswer,"42"));
        }
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    public void CenterStoryShowsFirstPartyCountedPictureWithoutSolvedEquation(int action)
    {
        var parameters=new Dictionary<string,int>
        {
            ["start"]=6, ["change"]=2, ["action"]=action,["flip"]=0
        };
        var json=System.Text.Json.JsonSerializer.Serialize(new {parameters});
        var svg=Edulytics.Web.Presentation.PracticeMathVisualRenderer.RenderSvg(
            UsCcssG1Unit2Part2Questions.CenterStory,json);
        Assert.NotNull(svg);
        Assert.Contains("<svg ",svg);
        Assert.Contains("Tell a story",svg);
        Assert.Contains(action==0 ? "JOINING" : "LEAVING",svg);
        Assert.Equal(8,System.Text.RegularExpressions.Regex.Matches(svg,
            "<circle cx=").Count);
        Assert.DoesNotContain("6+2=8",svg);
        Assert.DoesNotContain("6-2=4",svg);
        Assert.DoesNotContain("<image",svg);
        Assert.Null(Edulytics.Web.Presentation.PracticeMathVisualRenderer.RenderSvg(
            UsCcssG1Unit2Part2Questions.CenterStory,
            System.Text.Json.JsonSerializer.Serialize(new
            {
                parameters=new {start=99,change=2,action,flip=0}
            })));
    }

    [Fact]
    public void SameResultCannotSubstituteForConstructedMath()
    {
        var p=new Dictionary<string,int> { ["start"]=6,["amount"]=2,["action"]=0,["noun"]=0,["flip"]=0 };
        Assert.Equal("6+2",UsCcssG1Unit2StoryQuestions.Solve(UsCcssG1Unit2StoryQuestions.WriteExpression,p));
        Assert.Equal("6+2=8",UsCcssG1Unit2StoryQuestions.Solve(UsCcssG1Unit2StoryQuestions.WriteEquation,p));
        Assert.True(UsCcssG1Unit2StoryQuestions.Verify(UsCcssG1Unit2StoryQuestions.WriteExpression,p,"6 + 2"));
        Assert.True(UsCcssG1Unit2StoryQuestions.Verify(UsCcssG1Unit2StoryQuestions.WriteEquation,p,"6 + 2 = 8"));
        Assert.False(UsCcssG1Unit2StoryQuestions.Verify(UsCcssG1Unit2StoryQuestions.WriteExpression,p,"8"));
        Assert.False(UsCcssG1Unit2StoryQuestions.Verify(UsCcssG1Unit2StoryQuestions.WriteEquation,p,"8"));
        Assert.False(UsCcssG1Unit2StoryQuestions.Verify(UsCcssG1Unit2StoryQuestions.WriteEquation,p,"6+2=9"));
        Assert.False(UsCcssG1Unit2StoryQuestions.Verify(UsCcssG1Unit2StoryQuestions.WriteEquation,p,"6-2=4"));
    }
}
