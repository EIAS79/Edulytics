using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade6 Unit6 Lesson 19 first-edition selected Practice; AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG6Unit6Lesson19SelectedPracticeTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G6:U06:L19";
    private const string FamilyA = UsCcssG6Unit2RatioQuestions.L0619BoundedRelationships;
    private const string FamilyB = UsCcssG6Unit2RatioQuestions.L0619InterpretRelationships;

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
        // usccss.g6.u06.l19.first_party_equation_table_examples
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0619BoundedRelationships,0,"12"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0619BoundedRelationships,1,"5"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0619BoundedRelationships,2,"270"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0619BoundedRelationships,3,"7"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0619BoundedRelationships,4,"4"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0619BoundedRelationships,5,"5"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0619BoundedRelationships,6,"16"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0619BoundedRelationships,7,"40"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0619BoundedRelationships,8,"4"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0619BoundedRelationships,9,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0619BoundedRelationships,10,"4"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0619BoundedRelationships,11,"6"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0619BoundedRelationships,12,"80"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0619BoundedRelationships,13,"640"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0619BoundedRelationships,14,"32"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0619BoundedRelationships,15,"1024"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0619BoundedRelationships,16,"35"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0619BoundedRelationships,17,"24"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0619BoundedRelationships,18,"14"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0619BoundedRelationships,19,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0619BoundedRelationships,20,"18"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0619BoundedRelationships,21,"100"};
        // usccss.g6.u06.l19.first_party_relationship_interpretation
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0619InterpretRelationships,0,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0619InterpretRelationships,1,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0619InterpretRelationships,2,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0619InterpretRelationships,3,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0619InterpretRelationships,4,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0619InterpretRelationships,5,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0619InterpretRelationships,6,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0619InterpretRelationships,7,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0619InterpretRelationships,8,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0619InterpretRelationships,9,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0619InterpretRelationships,10,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0619InterpretRelationships,11,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0619InterpretRelationships,12,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0619InterpretRelationships,13,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0619InterpretRelationships,14,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0619InterpretRelationships,15,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0619InterpretRelationships,16,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0619InterpretRelationships,17,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0619InterpretRelationships,18,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0619InterpretRelationships,19,"0"};
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
            "g6-u06-l19-original-selected-practice",Lesson,contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,questions.Count);
        Assert.All(questions,q=>{
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher",q.Solution);
        });
    }
}
