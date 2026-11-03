using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade6 Unit9 Lesson 6 first-edition selected Practice; AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG6Unit9Lesson6SelectedPracticeTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G6:U09:L06";
    private const string FamilyA = UsCcssG6Unit2RatioQuestions.L0906Allocations;
    private const string FamilyB = UsCcssG6Unit2RatioQuestions.L0906RepresentationLogic;

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
        // usccss.g6.u09.l06.first_party_proportional_allocation_math
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0906Allocations,0,"16"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0906Allocations,1,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0906Allocations,2,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0906Allocations,3,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0906Allocations,4,"3"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0906Allocations,5,"20"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0906Allocations,6,"5"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0906Allocations,7,"16"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0906Allocations,8,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0906Allocations,9,"29"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0906Allocations,10,"43"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0906Allocations,11,"72"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0906Allocations,12,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0906Allocations,13,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0906Allocations,14,"120"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0906Allocations,15,"12"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0906Allocations,16,"4"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0906Allocations,17,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0906Allocations,18,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0906Allocations,19,"3"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0906Allocations,20,"850"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0906Allocations,21,"85"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0906Allocations,22,"40"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0906Allocations,23,"60"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0906Allocations,24,"50"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0906Allocations,25,"3"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0906Allocations,26,"90"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0906Allocations,27,"15"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0906Allocations,28,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0906Allocations,29,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0906Allocations,30,"3"};
        // usccss.g6.u09.l06.first_party_representatives_fairness_reasoning
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0906RepresentationLogic,0,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0906RepresentationLogic,1,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0906RepresentationLogic,2,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0906RepresentationLogic,3,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0906RepresentationLogic,4,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0906RepresentationLogic,5,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0906RepresentationLogic,6,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0906RepresentationLogic,7,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0906RepresentationLogic,8,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0906RepresentationLogic,9,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0906RepresentationLogic,10,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0906RepresentationLogic,11,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0906RepresentationLogic,12,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0906RepresentationLogic,13,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0906RepresentationLogic,14,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0906RepresentationLogic,15,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0906RepresentationLogic,16,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0906RepresentationLogic,17,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0906RepresentationLogic,18,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0906RepresentationLogic,19,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0906RepresentationLogic,20,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0906RepresentationLogic,21,"1"};
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
            "g6-u09-l06-original-selected-practice",Lesson,contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,questions.Count);
        Assert.All(questions,q=>{
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher",q.Solution);
        });
    }
}
