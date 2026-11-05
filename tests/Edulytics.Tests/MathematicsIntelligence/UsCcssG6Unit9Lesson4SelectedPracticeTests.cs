using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade6 Unit9 Lesson 4 first-edition selected Practice; AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG6Unit9Lesson4SelectedPracticeTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G6:U09:L04";
    private const string FamilyA = UsCcssG6Unit2RatioQuestions.L0904VoteNumbers;
    private const string FamilyB = UsCcssG6Unit2RatioQuestions.L0904VoteInterpretations;

    [Fact] public void BothFamiliesAreLessonScopedAndSupported()
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve(Lesson,out var contract));
        Assert.NotNull(contract);
        Assert.Equal("READY_VERIFIED",contract.Readiness);
        Assert.Equal(new[]{FamilyA,FamilyB},contract.AllowedQuestionFamilies);
        Assert.All(contract.AllowedQuestionFamilies,
            f=>Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f)));
    }

    public static IEnumerable<object[]> SourceMathVariants()
    {
        // usccss.g6.u09.l04.first_party_two_choice_voting_math
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0904VoteNumbers,0,"40"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0904VoteNumbers,1,"60"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0904VoteNumbers,2,"27"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0904VoteNumbers,3,"67"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0904VoteNumbers,4,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0904VoteNumbers,5,"3"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0904VoteNumbers,6,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0904VoteNumbers,7,"40"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0904VoteNumbers,8,"65"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0904VoteNumbers,9,"50"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0904VoteNumbers,10,"62"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0904VoteNumbers,11,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0904VoteNumbers,12,"160"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0904VoteNumbers,13,"7"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0904VoteNumbers,14,"132"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0904VoteNumbers,15,"28"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0904VoteNumbers,16,"5"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0904VoteNumbers,17,"4"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0904VoteNumbers,18,"250"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0904VoteNumbers,19,"50"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0904VoteNumbers,20,"40"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0904VoteNumbers,21,"4"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0904VoteNumbers,22,"200"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0904VoteNumbers,23,"67"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0904VoteNumbers,24,"110"};
        // usccss.g6.u09.l04.first_party_supermajority_and_sampling_logic
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0904VoteInterpretations,0,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0904VoteInterpretations,1,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0904VoteInterpretations,2,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0904VoteInterpretations,3,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0904VoteInterpretations,4,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0904VoteInterpretations,5,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0904VoteInterpretations,6,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0904VoteInterpretations,7,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0904VoteInterpretations,8,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0904VoteInterpretations,9,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0904VoteInterpretations,10,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0904VoteInterpretations,11,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0904VoteInterpretations,12,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0904VoteInterpretations,13,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0904VoteInterpretations,14,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0904VoteInterpretations,15,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0904VoteInterpretations,16,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0904VoteInterpretations,17,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0904VoteInterpretations,18,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0904VoteInterpretations,19,"1"};
    }

    [Theory]
    [MemberData(nameof(SourceMathVariants))]
    public void OriginalMathAndIndependentVariantsCheckExpectedAnswers(string family,int variant,string expected)
    {
        var parameters=new Dictionary<string,int>{{"variant",variant}};
        Assert.Equal(expected,UsCcssG6Unit2RatioQuestions.Solve(family,parameters));
        Assert.True(ExactSkillContractQuestionEngine.Verify(family,parameters,expected));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,parameters,"999999"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,
           new Dictionary<string,int>{{"variant",variant},{"injected",1}},expected));
    }

    [Fact] public void NoForeignFamilyCanBeAccepted()
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve(Lesson,out var contract));
        Assert.NotNull(contract);
        var result=LessonPracticeAlignmentValidator.ValidatePersisted(
            contract.ToLegacyStage18Contract(),
            UsCcssG6Unit2RatioQuestions.L05Equivalence,
            new Dictionary<string,int>{{"variant",0}},"1");
        Assert.False(result.IsAligned);
        Assert.Equal("QuestionFamilyNotAllowed",result.ReasonCode);
    }

    [Fact] public void SelectedQuestionGenerationAndVerificationPreserveTeacherBoundary()
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve(Lesson,out var contract));
        Assert.NotNull(contract);
        var questions=new ExactSkillContractQuestionEngine().Generate(
            "g6-u09-l04-original-selected-practice",Lesson,contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,questions.Count);
        Assert.All(questions,q=>{
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher",q.Solution);
        });
    }
}
