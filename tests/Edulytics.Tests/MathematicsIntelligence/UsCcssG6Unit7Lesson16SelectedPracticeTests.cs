using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade6 Unit7 Lesson 16 first-edition selected Practice; AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG6Unit7Lesson16SelectedPracticeTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G6:U07:L16";
    private const string FamilyA = UsCcssG6Unit2RatioQuestions.L0716Factors;
    private const string FamilyB = UsCcssG6Unit2RatioQuestions.L0716GcfLogic;

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
        // usccss.g6.u07.l16.gcf_source_numbers_and_groups
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0716Factors,0,"12"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0716Factors,1,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0716Factors,2,"3"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0716Factors,3,"4"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0716Factors,4,"6"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0716Factors,5,"6"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0716Factors,6,"9"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0716Factors,7,"42"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0716Factors,8,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0716Factors,9,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0716Factors,10,"3"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0716Factors,11,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0716Factors,12,"15"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0716Factors,13,"15"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0716Factors,14,"6"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0716Factors,15,"5"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0716Factors,16,"8"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0716Factors,17,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0716Factors,18,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0716Factors,19,"81"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0716Factors,20,"75"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0716Factors,21,"32"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0716Factors,22,"96"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0716Factors,23,"7"};
        // usccss.g6.u07.l16.factor_context_and_sign_logic
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0716GcfLogic,0,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0716GcfLogic,1,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0716GcfLogic,2,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0716GcfLogic,3,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0716GcfLogic,4,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0716GcfLogic,5,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0716GcfLogic,6,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0716GcfLogic,7,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0716GcfLogic,8,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0716GcfLogic,9,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0716GcfLogic,10,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0716GcfLogic,11,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0716GcfLogic,12,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0716GcfLogic,13,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0716GcfLogic,14,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0716GcfLogic,15,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0716GcfLogic,16,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0716GcfLogic,17,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0716GcfLogic,18,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0716GcfLogic,19,"0"};
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
            "g6-u07-l16-original-selected-practice",Lesson,contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,questions.Count);
        Assert.All(questions,q=>{
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher",q.Solution);
        });
    }
}
