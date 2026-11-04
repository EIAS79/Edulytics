using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade6 Unit6 Lesson 9 first-edition selected Practice; AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG6Unit6Lesson9SelectedPracticeTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G6:U06:L09";
    private const string FamilyA = UsCcssG6Unit2RatioQuestions.L0609DistributeNumbers;
    private const string FamilyB = UsCcssG6Unit2RatioQuestions.L0609DistributeLogic;

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
        // usccss.g6.u06.l09.distributive_numeric_products
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0609DistributeNumbers,0,"30"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0609DistributeNumbers,1,"10"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0609DistributeNumbers,2,"20"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0609DistributeNumbers,3,"950"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0609DistributeNumbers,4,"950"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0609DistributeNumbers,5,"2352"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0609DistributeNumbers,6,"315"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0609DistributeNumbers,7,"204"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0609DistributeNumbers,8,"2392"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0609DistributeNumbers,9,"65"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0609DistributeNumbers,10,"32"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0609DistributeNumbers,11,"47"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0609DistributeNumbers,12,"43"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0609DistributeNumbers,13,"12"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0609DistributeNumbers,14,"8"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0609DistributeNumbers,15,"4"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0609DistributeNumbers,16,"2400"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0609DistributeNumbers,17,"8"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0609DistributeNumbers,18,"48"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0609DistributeNumbers,19,"30"};
        // usccss.g6.u06.l09.distributive_area_reasoning
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0609DistributeLogic,0,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0609DistributeLogic,1,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0609DistributeLogic,2,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0609DistributeLogic,3,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0609DistributeLogic,4,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0609DistributeLogic,5,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0609DistributeLogic,6,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0609DistributeLogic,7,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0609DistributeLogic,8,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0609DistributeLogic,9,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0609DistributeLogic,10,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0609DistributeLogic,11,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0609DistributeLogic,12,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0609DistributeLogic,13,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0609DistributeLogic,14,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0609DistributeLogic,15,"1"};
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
            "g6-u06-l09-original-selected-practice",Lesson,contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,questions.Count);
        Assert.All(questions,q=>{
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher",q.Solution);
        });
    }
}
