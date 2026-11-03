using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade6 Unit7 Lesson 5 first-edition selected Practice; AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG6Unit7Lesson5SelectedPracticeTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G6:U07:L05";
    private const string FamilyA = UsCcssG6Unit2RatioQuestions.L0705SignedChanges;
    private const string FamilyB = UsCcssG6Unit2RatioQuestions.L0705ChangeLogic;

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
        // usccss.g6.u07.l05.positive_negative_changes
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0705SignedChanges,0,"-4"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0705SignedChanges,1,"-7"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0705SignedChanges,2,"13"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0705SignedChanges,3,"-2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0705SignedChanges,4,"5"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0705SignedChanges,5,"-2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0705SignedChanges,6,"10"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0705SignedChanges,7,"-10"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0705SignedChanges,8,"-550"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0705SignedChanges,9,"30"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0705SignedChanges,10,"-30"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0705SignedChanges,11,"4"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0705SignedChanges,12,"-4"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0705SignedChanges,13,"5"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0705SignedChanges,14,"24"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0705SignedChanges,15,"8"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0705SignedChanges,16,"3"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0705SignedChanges,17,"9"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0705SignedChanges,18,"6"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0705SignedChanges,19,"125"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0705SignedChanges,20,"3"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0705SignedChanges,21,"-1"};
        // usccss.g6.u07.l05.signed_context_representations
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0705ChangeLogic,0,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0705ChangeLogic,1,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0705ChangeLogic,2,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0705ChangeLogic,3,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0705ChangeLogic,4,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0705ChangeLogic,5,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0705ChangeLogic,6,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0705ChangeLogic,7,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0705ChangeLogic,8,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0705ChangeLogic,9,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0705ChangeLogic,10,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0705ChangeLogic,11,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0705ChangeLogic,12,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0705ChangeLogic,13,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0705ChangeLogic,14,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0705ChangeLogic,15,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0705ChangeLogic,16,"1"};
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
            "g6-u07-l05-original-selected-practice",Lesson,contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,questions.Count);
        Assert.All(questions,q=>{
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher",q.Solution);
        });
    }
}
