using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade6 Unit9 Lesson 3 first-edition selected Practice; AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG6Unit9Lesson3SelectedPracticeTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G6:U09:L03";
    private const string FamilyA = UsCcssG6Unit2RatioQuestions.L0903SquareEuclid;
    private const string FamilyB = UsCcssG6Unit2RatioQuestions.L0903SquareLogic;

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
        // usccss.g6.u09.l03.first_party_rectangle_square_decomposition
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0903SquareEuclid,0,"7"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0903SquareEuclid,1,"4"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0903SquareEuclid,2,"12"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0903SquareEuclid,3,"4"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0903SquareEuclid,4,"3"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0903SquareEuclid,5,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0903SquareEuclid,6,"5"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0903SquareEuclid,7,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0903SquareEuclid,8,"3"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0903SquareEuclid,9,"3"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0903SquareEuclid,10,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0903SquareEuclid,11,"3"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0903SquareEuclid,12,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0903SquareEuclid,13,"3"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0903SquareEuclid,14,"4"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0903SquareEuclid,15,"3"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0903SquareEuclid,16,"3"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0903SquareEuclid,17,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0903SquareEuclid,18,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0903SquareEuclid,19,"4"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0903SquareEuclid,20,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0903SquareEuclid,21,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0903SquareEuclid,22,"4"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0903SquareEuclid,23,"3"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0903SquareEuclid,24,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0903SquareEuclid,25,"3"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0903SquareEuclid,26,"5"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0903SquareEuclid,27,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0903SquareEuclid,28,"3"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0903SquareEuclid,29,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0903SquareEuclid,30,"7"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0903SquareEuclid,31,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0903SquareEuclid,32,"4"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0903SquareEuclid,33,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0903SquareEuclid,34,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0903SquareEuclid,35,"7"};
        // usccss.g6.u09.l03.first_party_square_tiling_fraction_reasoning
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0903SquareLogic,0,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0903SquareLogic,1,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0903SquareLogic,2,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0903SquareLogic,3,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0903SquareLogic,4,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0903SquareLogic,5,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0903SquareLogic,6,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0903SquareLogic,7,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0903SquareLogic,8,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0903SquareLogic,9,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0903SquareLogic,10,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0903SquareLogic,11,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0903SquareLogic,12,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0903SquareLogic,13,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0903SquareLogic,14,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0903SquareLogic,15,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0903SquareLogic,16,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0903SquareLogic,17,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0903SquareLogic,18,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0903SquareLogic,19,"1"};
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
            "g6-u09-l03-original-selected-practice",Lesson,contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,questions.Count);
        Assert.All(questions,q=>{
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher",q.Solution);
        });
    }
}
