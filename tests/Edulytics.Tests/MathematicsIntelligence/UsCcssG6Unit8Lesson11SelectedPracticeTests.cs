using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade6 Unit8 Lesson 11 first-edition selected Practice; AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG6Unit8Lesson11SelectedPracticeTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G6:U08:L11";
    private const string FamilyA = UsCcssG6Unit2RatioQuestions.L0811MadValues;
    private const string FamilyB = UsCcssG6Unit2RatioQuestions.L0811MadReasoning;

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
        // usccss.g6.u08.l11.mean_absolute_deviation_amounts
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0811MadValues,0,"163"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0811MadValues,1,"326"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0811MadValues,2,"76"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0811MadValues,3,"46"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0811MadValues,4,"6"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0811MadValues,5,"34"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0811MadValues,6,"94"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0811MadValues,7,"256"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0811MadValues,8,"512"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0811MadValues,9,"9"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0811MadValues,10,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0811MadValues,11,"24"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0811MadValues,12,"5"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0811MadValues,13,"5"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0811MadValues,14,"6"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0811MadValues,15,"12"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0811MadValues,16,"5"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0811MadValues,17,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0811MadValues,18,"9"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0811MadValues,19,"9"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0811MadValues,20,"42"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0811MadValues,21,"25"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0811MadValues,22,"17"};
        // usccss.g6.u08.l11.mad_interpretation_logic
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0811MadReasoning,0,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0811MadReasoning,1,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0811MadReasoning,2,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0811MadReasoning,3,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0811MadReasoning,4,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0811MadReasoning,5,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0811MadReasoning,6,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0811MadReasoning,7,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0811MadReasoning,8,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0811MadReasoning,9,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0811MadReasoning,10,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0811MadReasoning,11,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0811MadReasoning,12,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0811MadReasoning,13,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0811MadReasoning,14,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0811MadReasoning,15,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0811MadReasoning,16,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0811MadReasoning,17,"1"};
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
            "g6-u08-l11-original-selected-practice",Lesson,contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,questions.Count);
        Assert.All(questions,q=>{
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher",q.Solution);
        });
    }
}
