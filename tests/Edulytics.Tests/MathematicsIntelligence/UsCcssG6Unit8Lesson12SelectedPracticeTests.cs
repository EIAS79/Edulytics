using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade6 Unit8 Lesson 12 first-edition selected Practice; AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG6Unit8Lesson12SelectedPracticeTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G6:U08:L12";
    private const string FamilyA = UsCcssG6Unit2RatioQuestions.L0812CompareMeans;
    private const string FamilyB = UsCcssG6Unit2RatioQuestions.L0812CompareLogic;

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
        // usccss.g6.u08.l12.mean_mad_source_datasets
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0812CompareMeans,0,"85"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0812CompareMeans,1,"85"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0812CompareMeans,2,"45"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0812CompareMeans,3,"181"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0812CompareMeans,4,"181"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0812CompareMeans,5,"79"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0812CompareMeans,6,"235"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0812CompareMeans,7,"235"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0812CompareMeans,8,"162"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0812CompareMeans,9,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0812CompareMeans,10,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0812CompareMeans,11,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0812CompareMeans,12,"96"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0812CompareMeans,13,"54"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0812CompareMeans,14,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0812CompareMeans,15,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0812CompareMeans,16,"11"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0812CompareMeans,17,"20"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0812CompareMeans,18,"-2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0812CompareMeans,19,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0812CompareMeans,20,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0812CompareMeans,21,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0812CompareMeans,22,"5"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0812CompareMeans,23,"5"};
        // usccss.g6.u08.l12.mean_mad_spread_context
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0812CompareLogic,0,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0812CompareLogic,1,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0812CompareLogic,2,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0812CompareLogic,3,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0812CompareLogic,4,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0812CompareLogic,5,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0812CompareLogic,6,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0812CompareLogic,7,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0812CompareLogic,8,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0812CompareLogic,9,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0812CompareLogic,10,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0812CompareLogic,11,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0812CompareLogic,12,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0812CompareLogic,13,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0812CompareLogic,14,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0812CompareLogic,15,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0812CompareLogic,16,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0812CompareLogic,17,"1"};
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
            "g6-u08-l12-original-selected-practice",Lesson,contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,questions.Count);
        Assert.All(questions,q=>{
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher",q.Solution);
        });
    }
}
