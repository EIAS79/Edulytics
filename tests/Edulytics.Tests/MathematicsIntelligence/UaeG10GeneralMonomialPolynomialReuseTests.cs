using System.Globalization;
using Edulytics.Core.Enums;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Source-aligned G10 General U01L02 monomial × polynomial exact assessment.
/// Authored during A–B–C. Not executed until Stage D.
/// </summary>
public sealed class UaeG10GeneralMonomialPolynomialReuseTests
{
    private const string LessonCode =
        "PED:UAE-MOE-MATH:L10:GENERAL:01:02:MULTIPLYING-A-POLYNOMIAL-BY-A-MONOMIAL";

    private const string Coefficient =
        "uae.g10.general.u01.l02.monomial_polynomial_coefficient";

    private const string Whole =
        "uae.g10.general.u01.l02.full_monomial_polynomial_product";

    [Theory]
    [InlineData(Coefficient,LessonPracticeInteractionKind.IntegerKeypad)]
    [InlineData(Whole,LessonPracticeInteractionKind.StructuredEntry)]
    public void ExactFamilyIsMonomialOnlyWithCorrectLearnerInput(
        string family,LessonPracticeInteractionKind expectedInteraction)
    {
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
        Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var interaction));
        Assert.Equal(expectedInteraction,interaction!.InteractionKind);
    }

    [Theory]
    [InlineData(Coefficient)]
    [InlineData(Whole)]
    public void GeneratedSourceSkillHasIndependentCorrectAndWrongAnswerChecks(string family)
    {
        var engine=new ExactSkillContractQuestionEngine();
        foreach(int seed in new[]{20261124,20261125,20261126})
        foreach(var difficulty in new[]{ExactSkillQuestionDifficulty.Standard,
                                         ExactSkillQuestionDifficulty.Challenge})
        {
            var tasks=engine.Generate(LessonCode,LessonCode,[family],
                difficulty,4,seed,[]);
            Assert.Equal(4,tasks.Count);
            foreach(var task in tasks)
            {
                var p=task.Parameters;
                Assert.Equal(6,p.Count);
                int m=p["m"],k=p["k"],degree=p["degree"];
                Assert.InRange(Math.Abs(m),1,7);
                Assert.InRange(k,0,2);
                Assert.InRange(degree,0,4);
                Assert.True(task.Prompt.Contains("اضرب",StringComparison.Ordinal));
                Assert.DoesNotContain("اضرب كثيرتي الحدود",task.Prompt,StringComparison.Ordinal);
                int[] expected=new int[5];
                for(int j=0;j<3;j++)
                    expected[j+k]=m*p["a"+j.ToString(CultureInfo.InvariantCulture)];

                Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,task.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,"خطأ"));
                Assert.Equal(family==Coefficient?AssessmentItemType.Numeric:AssessmentItemType.ShortAnswer,
                    task.ItemType);
                if(family==Coefficient)
                {
                    int actual=int.Parse(task.CorrectAnswer,CultureInfo.InvariantCulture);
                    Assert.Equal(expected[degree],actual);
                    Assert.False(ExactSkillContractQuestionEngine.Verify(
                        family,p,(actual+1).ToString(CultureInfo.InvariantCulture)));
                }
                else
                {
                    // Exact parser enforces canonical standard form. A value
                    // matching at only one substitution point is NOT sufficient.
                    Assert.False(ExactSkillContractQuestionEngine.Verify(
                        family,p,task.CorrectAnswer+"+1"));
                }

                var arabicDigits=string.Concat(task.CorrectAnswer.Select(ch=>
                    ch is >= '0' and <= '9'?(char)('٠'+ch-'0'):ch));
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,arabicDigits));
                var invalid=p.ToDictionary(x=>x.Key,x=>x.Value,StringComparer.Ordinal);
                invalid["k"]=5;
                Assert.False(ExactSkillContractQuestionEngine.Verify(
                    family,invalid,task.CorrectAnswer));
            }
        }
    }

    [Fact]
    public void OriginalIndependentGoldenExamplesMatchSourceLearningGoal()
    {
        // 3س(2س²−5س+4)=6س³−15س²+12س
        foreach(int x in new[]{-3,-1,0,1,2,7})
            Assert.Equal(3*x*(2*x*x-5*x+4),
                6*x*x*x-15*x*x+12*x);
        // −2س²(3س³−4س+5)=−6س⁵+8س³−10س²
        foreach(int x in new[]{-2,-1,0,1,2})
            Assert.Equal(-2*x*x*(3*x*x*x-4*x+5),
                -6*x*x*x*x*x+8*x*x*x-10*x*x);
        // A rectangle of width 2x and length 3x²+4x+5
        foreach(int x in new[]{1,2,4,10})
            Assert.Equal(2*x*(3*x*x+4*x+5),
                6*x*x*x+8*x*x+10*x);
    }

    [Fact]
    public void WrongFamilyFromG9PolynomialTimesPolynomialMustNotLeakIntoG10L02()
    {
        const string wrong="uae.g9.general.u07.l03.multiply_coefficient";
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(wrong));
        Assert.NotEqual(wrong,Coefficient);
        Assert.NotEqual(wrong,Whole);
    }
}
