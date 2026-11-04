using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade6 Unit7 Lesson 4 first-edition selected Practice; AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG6Unit7Lesson4SelectedPracticeTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G6:U07:L04";
    private const string FamilyA = UsCcssG6Unit2RatioQuestions.L0704RationalOrder;
    private const string FamilyB = UsCcssG6Unit2RatioQuestions.L0704OrderLogic;

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
        // usccss.g6.u07.l04.order_signed_numbers_source
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0704RationalOrder,0,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0704RationalOrder,1,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0704RationalOrder,2,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0704RationalOrder,3,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0704RationalOrder,4,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0704RationalOrder,5,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0704RationalOrder,6,"-3"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0704RationalOrder,7,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0704RationalOrder,8,"-1858"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0704RationalOrder,9,"-34"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0704RationalOrder,10,"-1881"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0704RationalOrder,11,"-25287"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0704RationalOrder,12,"-1532"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0704RationalOrder,13,"153"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0704RationalOrder,14,"145"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0704RationalOrder,15,"594"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0704RationalOrder,16,"1485"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0704RationalOrder,17,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0704RationalOrder,18,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0704RationalOrder,19,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0704RationalOrder,20,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0704RationalOrder,21,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0704RationalOrder,22,"-34"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0704RationalOrder,23,"0"};
        // usccss.g6.u07.l04.signed_ordering_logic
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0704OrderLogic,0,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0704OrderLogic,1,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0704OrderLogic,2,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0704OrderLogic,3,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0704OrderLogic,4,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0704OrderLogic,5,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0704OrderLogic,6,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0704OrderLogic,7,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0704OrderLogic,8,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0704OrderLogic,9,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0704OrderLogic,10,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0704OrderLogic,11,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0704OrderLogic,12,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0704OrderLogic,13,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0704OrderLogic,14,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0704OrderLogic,15,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0704OrderLogic,16,"1"};
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
            "g6-u07-l04-original-selected-practice",Lesson,contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,questions.Count);
        Assert.All(questions,q=>{
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher",q.Solution);
        });
    }
}
