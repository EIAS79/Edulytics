using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade6 Unit7 Lesson 2 first-edition selected Practice; AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG6Unit7Lesson2SelectedPracticeTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G6:U07:L02";
    private const string FamilyA = UsCcssG6Unit2RatioQuestions.L0702OppositeNumbers;
    private const string FamilyB = UsCcssG6Unit2RatioQuestions.L0702OppositeLogic;

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
        // usccss.g6.u07.l02.signed_opposites_values
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0702OppositeNumbers,0,"5"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0702OppositeNumbers,1,"-28"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0702OppositeNumbers,2,"104"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0702OppositeNumbers,3,"-875"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0702OppositeNumbers,4,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0702OppositeNumbers,5,"8003"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0702OppositeNumbers,6,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0702OppositeNumbers,7,"-1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0702OppositeNumbers,8,"3"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0702OppositeNumbers,9,"-4"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0702OppositeNumbers,10,"5"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0702OppositeNumbers,11,"-3"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0702OppositeNumbers,12,"6"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0702OppositeNumbers,13,"-11"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0702OppositeNumbers,14,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0702OppositeNumbers,15,"3"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0702OppositeNumbers,16,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0702OppositeNumbers,17,"20"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0702OppositeNumbers,18,"3"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0702OppositeNumbers,19,"3"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0702OppositeNumbers,20,"3"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0702OppositeNumbers,21,"30"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0702OppositeNumbers,22,"3048"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0702OppositeNumbers,23,"9144"};
        // usccss.g6.u07.l02.opposite_number_line_reasoning
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0702OppositeLogic,0,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0702OppositeLogic,1,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0702OppositeLogic,2,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0702OppositeLogic,3,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0702OppositeLogic,4,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0702OppositeLogic,5,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0702OppositeLogic,6,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0702OppositeLogic,7,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0702OppositeLogic,8,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0702OppositeLogic,9,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0702OppositeLogic,10,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0702OppositeLogic,11,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0702OppositeLogic,12,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0702OppositeLogic,13,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0702OppositeLogic,14,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0702OppositeLogic,15,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0702OppositeLogic,16,"0"};
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
            "g6-u07-l02-original-selected-practice",Lesson,contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,questions.Count);
        Assert.All(questions,q=>{
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher",q.Solution);
        });
    }
}
