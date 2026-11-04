using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade6 Unit9 Lesson 5 first-edition selected Practice; AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG6Unit9Lesson5SelectedPracticeTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G6:U09:L05";
    private const string FamilyA = UsCcssG6Unit2RatioQuestions.L0905VotingMath;
    private const string FamilyB = UsCcssG6Unit2RatioQuestions.L0905VotingLogic;

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
        // usccss.g6.u09.l05.first_party_multichoice_voting_arithmetic
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0905VotingMath,0,"40"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0905VotingMath,1,"40"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0905VotingMath,2,"24"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0905VotingMath,3,"60"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0905VotingMath,4,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0905VotingMath,5,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0905VotingMath,6,"50"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0905VotingMath,7,"21"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0905VotingMath,8,"13"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0905VotingMath,9,"9"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0905VotingMath,10,"7"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0905VotingMath,11,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0905VotingMath,12,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0905VotingMath,13,"4"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0905VotingMath,14,"16"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0905VotingMath,15,"29"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0905VotingMath,16,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0905VotingMath,17,"6"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0905VotingMath,18,"7"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0905VotingMath,19,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0905VotingMath,20,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0905VotingMath,21,"3"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0905VotingMath,22,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0905VotingMath,23,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0905VotingMath,24,"63"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0905VotingMath,25,"97"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0905VotingMath,26,"75"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0905VotingMath,27,"65"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0905VotingMath,28,"2"};
        // usccss.g6.u09.l05.first_party_multichoice_election_reasoning
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0905VotingLogic,0,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0905VotingLogic,1,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0905VotingLogic,2,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0905VotingLogic,3,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0905VotingLogic,4,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0905VotingLogic,5,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0905VotingLogic,6,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0905VotingLogic,7,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0905VotingLogic,8,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0905VotingLogic,9,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0905VotingLogic,10,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0905VotingLogic,11,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0905VotingLogic,12,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0905VotingLogic,13,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0905VotingLogic,14,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0905VotingLogic,15,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0905VotingLogic,16,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0905VotingLogic,17,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0905VotingLogic,18,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0905VotingLogic,19,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0905VotingLogic,20,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0905VotingLogic,21,"1"};
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
            "g6-u09-l05-original-selected-practice",Lesson,contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,questions.Count);
        Assert.All(questions,q=>{
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher",q.Solution);
        });
    }
}
