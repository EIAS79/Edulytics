using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade6 Unit3 Lesson 17 first-edition selected Practice; AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG6Unit3Lesson17SelectedPracticeTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G6:U03:L17";
    private const string FamilyA = UsCcssG6Unit2RatioQuestions.L0317PaintCalculations;
    private const string FamilyB = UsCcssG6Unit2RatioQuestions.L0317PaintReasoning;

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
        // usccss.g6.u03.l17.first_party_bounded_room_paint_math
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0317PaintCalculations,0,"72"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0317PaintCalculations,1,"600"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0317PaintCalculations,2,"175"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0317PaintCalculations,3,"7"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0317PaintCalculations,4,"7630"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0317PaintCalculations,5,"218"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0317PaintCalculations,6,"872"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0317PaintCalculations,7,"6104"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0317PaintCalculations,8,"6760"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0317PaintCalculations,9,"6106"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0317PaintCalculations,10,"6980"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0317PaintCalculations,11,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0317PaintCalculations,12,"24"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0317PaintCalculations,13,"4"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0317PaintCalculations,14,"400"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0317PaintCalculations,15,"304"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0317PaintCalculations,16,"154"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0317PaintCalculations,17,"15"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0317PaintCalculations,18,"4"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0317PaintCalculations,19,"1225"};
        // usccss.g6.u03.l17.first_party_paint_model_reasoning
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0317PaintReasoning,0,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0317PaintReasoning,1,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0317PaintReasoning,2,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0317PaintReasoning,3,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0317PaintReasoning,4,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0317PaintReasoning,5,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0317PaintReasoning,6,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0317PaintReasoning,7,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0317PaintReasoning,8,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0317PaintReasoning,9,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0317PaintReasoning,10,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0317PaintReasoning,11,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0317PaintReasoning,12,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0317PaintReasoning,13,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0317PaintReasoning,14,"1"};
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
            "g6-u03-l17-original-selected-practice",Lesson,contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,questions.Count);
        Assert.All(questions,q=>{
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher",q.Solution);
        });
    }
}
