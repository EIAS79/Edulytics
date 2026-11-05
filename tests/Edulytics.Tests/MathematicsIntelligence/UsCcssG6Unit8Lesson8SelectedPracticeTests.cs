using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade6 Unit8 Lesson 8 first-edition selected Practice; AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG6Unit8Lesson8SelectedPracticeTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G6:U08:L08";
    private const string FamilyA = UsCcssG6Unit2RatioQuestions.L0808ShapeNumbers;
    private const string FamilyB = UsCcssG6Unit2RatioQuestions.L0808ShapeLogic;

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
        // usccss.g6.u08.l08.histogram_shape_and_cumulative
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0808ShapeNumbers,0,"143"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0808ShapeNumbers,1,"16"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0808ShapeNumbers,2,"6"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0808ShapeNumbers,3,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0808ShapeNumbers,4,"14"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0808ShapeNumbers,5,"3"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0808ShapeNumbers,6,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0808ShapeNumbers,7,"7"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0808ShapeNumbers,8,"7"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0808ShapeNumbers,9,"4"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0808ShapeNumbers,10,"32"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0808ShapeNumbers,11,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0808ShapeNumbers,12,"108"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0808ShapeNumbers,13,"20"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0808ShapeNumbers,14,"8"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0808ShapeNumbers,15,"8"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0808ShapeNumbers,16,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0808ShapeNumbers,17,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0808ShapeNumbers,18,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0808ShapeNumbers,19,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0808ShapeNumbers,20,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0808ShapeNumbers,21,"0"};
        // usccss.g6.u08.l08.histogram_shape_explanations
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0808ShapeLogic,0,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0808ShapeLogic,1,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0808ShapeLogic,2,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0808ShapeLogic,3,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0808ShapeLogic,4,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0808ShapeLogic,5,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0808ShapeLogic,6,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0808ShapeLogic,7,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0808ShapeLogic,8,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0808ShapeLogic,9,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0808ShapeLogic,10,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0808ShapeLogic,11,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0808ShapeLogic,12,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0808ShapeLogic,13,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0808ShapeLogic,14,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0808ShapeLogic,15,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0808ShapeLogic,16,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0808ShapeLogic,17,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0808ShapeLogic,18,"1"};
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
            "g6-u08-l08-original-selected-practice",Lesson,contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,questions.Count);
        Assert.All(questions,q=>{
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher",q.Solution);
        });
    }
}
