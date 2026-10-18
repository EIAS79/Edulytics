using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UsCcssG1FullGradeTensAndOnesPracticeTests
{
    [Fact]
    public void FiveRegisteredSourceSpecificPlaceValueFamiliesAreQueryable()
    {
        Assert.Equal(5,UsCcssG1FullGradeTensAndOnesQuestions.Families.Count);
        foreach(var family in UsCcssG1FullGradeTensAndOnesQuestions.Families)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var interaction));
            Assert.Equal(LessonPracticeInteractionKind.ContractDriven,interaction!.InteractionKind);
        }
        for(int n=6;n<=10;n++)
        {
            var code=$"PED:US-CCSS-MATH:G1:U04:L{n:00}";
            var item=LessonPracticeContractRegistry.All.Single(x=>x.LessonCode==code);
            Assert.Single(item.AllowedQuestionFamilies);
            Assert.True(UsCcssG1FullGradeTensAndOnesQuestions.Supports(item.AllowedQuestionFamilies[0]));
        }
    }
    [Fact]
    public void ValidTwoDigitValuesStayWithinGrade1AndWrongResponsesFail()
    {
        foreach(var family in UsCcssG1FullGradeTensAndOnesQuestions.Families)
            for(int seed=0;seed<200;seed++)
            {
                var q=UsCcssG1FullGradeTensAndOnesQuestions.Build(family,new Random(89*seed+17));
                Assert.True(UsCcssG1FullGradeTensAndOnesQuestions.Valid(family,q.Parameters));
                var correct=UsCcssG1FullGradeTensAndOnesQuestions.Solve(family,q.Parameters);
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,correct));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,"NUMBER=999"));
                Assert.False(UsCcssG1FullGradeTensAndOnesQuestions.GradePersistedAnswer(family,correct,correct+";EXTRA=1"));
            }
    }
    [Fact]
    public void TensAndOnesNotReversedAndNumberWordsUseCorrectSpelling()
    {
        var p=new Dictionary<string,int>{{"tens",5},{"ones",8},{"mode",0},{"unused",0}};
        Assert.Equal("fifty-eight",UsCcssG1FullGradeTensAndOnesQuestions.NumberWords(5,8));
        Assert.Equal("TENS=5;ONES=8;TOTAL=58",UsCcssG1FullGradeTensAndOnesQuestions.Solve(
            UsCcssG1FullGradeTensAndOnesQuestions.MixedCollection,p));
        Assert.Equal("NAME=fifty-eight",UsCcssG1FullGradeTensAndOnesQuestions.Solve(
            UsCcssG1FullGradeTensAndOnesQuestions.ReadNumber,p));
        Assert.False(UsCcssG1FullGradeTensAndOnesQuestions.Verify(
            UsCcssG1FullGradeTensAndOnesQuestions.MixedCollection,p,"TENS=8;ONES=5;TOTAL=85"));
        Assert.Equal("NUMBER=58",UsCcssG1FullGradeTensAndOnesQuestions.Solve(
            UsCcssG1FullGradeTensAndOnesQuestions.WriteNumeral,p));
        Assert.True(UsCcssG1FullGradeTensAndOnesQuestions.Verify(
            UsCcssG1FullGradeTensAndOnesQuestions.ReadNumber,p,"name = FIFTY-EIGHT"));
    }
    [Fact]
    public void EngineCanGenerateDistinctPracticedPlaceValueAnswers()
    {
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var family in UsCcssG1FullGradeTensAndOnesQuestions.Families)
        {
            var items=engine.Generate("G1-place-value-71-75",family,[family],
                ExactSkillQuestionDifficulty.Standard,5,20261018,[]);
            Assert.Equal(5,items.Count);
            Assert.Equal(5,items.Select(x=>x.Prompt).Distinct(StringComparer.Ordinal).Count());
            foreach(var item in items)
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,item.Parameters,item.CorrectAnswer));
        }
    }
}
