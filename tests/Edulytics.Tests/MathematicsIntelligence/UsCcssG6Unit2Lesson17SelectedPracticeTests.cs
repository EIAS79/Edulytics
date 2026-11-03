using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade6 Unit2 Lesson 17 first-edition selected Practice; AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG6Unit2Lesson17SelectedPracticeTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G6:U02:L17";
    private const string FamilyA = UsCcssG6Unit2RatioQuestions.L17FermiAssumptions;
    private const string FamilyB = UsCcssG6Unit2RatioQuestions.L17FermiSensitivity;

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
        // usccss.g6.u02.l17.first_party_fermi_assumption_arithmetic
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L17FermiAssumptions,0,"600"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L17FermiAssumptions,1,"1800"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L17FermiAssumptions,2,"100"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L17FermiAssumptions,3,"4000"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L17FermiAssumptions,4,"200"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L17FermiAssumptions,5,"240000"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L17FermiAssumptions,6,"15"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L17FermiAssumptions,7,"1350"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L17FermiAssumptions,8,"240"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L17FermiAssumptions,9,"480"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L17FermiAssumptions,10,"5000"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L17FermiAssumptions,11,"600000"};
        // usccss.g6.u02.l17.first_party_fermi_sensitivity
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L17FermiSensitivity,0,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L17FermiSensitivity,1,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L17FermiSensitivity,2,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L17FermiSensitivity,3,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L17FermiSensitivity,4,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L17FermiSensitivity,5,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L17FermiSensitivity,6,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L17FermiSensitivity,7,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L17FermiSensitivity,8,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L17FermiSensitivity,9,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L17FermiSensitivity,10,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L17FermiSensitivity,11,"1"};
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
            "g6-u02-l17-original-selected-practice",Lesson,contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,questions.Count);
        Assert.All(questions,q=>{
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher",q.Solution);
        });
    }
}
