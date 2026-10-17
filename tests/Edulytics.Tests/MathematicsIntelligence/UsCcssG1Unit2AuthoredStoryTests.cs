using Edulytics.Core.Enums;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class UsCcssG1Unit2AuthoredStoryTests
{
    [Theory]
    [InlineData(UsCcssG1Unit2StoryQuestions.WriteExpression)]
    [InlineData(UsCcssG1Unit2StoryQuestions.WriteEquation)]
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
            Assert.Equal(family==UsCcssG1Unit2StoryQuestions.WriteEquation,
                expected.Contains('='));
        }
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
