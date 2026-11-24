using System.Globalization;
using Edulytics.Core.Enums;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Original source-exact mathematics verification for UAE G10 General U01L03.
/// Tests authored in A–B–C and NOT RUN until Stage D.
/// </summary>
public sealed class UaeG10GeneralPolynomialProductTests
{
    private const string LessonCode="PED:UAE-MOE-MATH:L10:GENERAL:01:03:MULTIPLYING-POLYNOMIALS";
    private const string Coefficient="uae.g10.general.u01.l03.product_coefficient";
    private const string Whole="uae.g10.general.u01.l03.full_polynomial_product";

    [Theory]
    [InlineData(Coefficient,LessonPracticeInteractionKind.IntegerKeypad)]
    [InlineData(Whole,LessonPracticeInteractionKind.StructuredEntry)]
    public void SupportsExactProductAndSourceCorrectRenderer(string family,LessonPracticeInteractionKind kind)
    {
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
        Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var ui));
        Assert.Equal(kind,ui!.InteractionKind);
    }

    [Theory]
    [InlineData(Coefficient)]
    [InlineData(Whole)]
    public void IndependentPolynomialConvolutionRejectsWrongOrTamperedResponse(string family)
    {
        var engine=new ExactSkillContractQuestionEngine();
        foreach(int seed in new[]{20261124,20261125,20261126})
        foreach(var difficulty in new[]{ExactSkillQuestionDifficulty.Standard,
                                         ExactSkillQuestionDifficulty.Challenge})
        {
            var items=engine.Generate(LessonCode,LessonCode,[family],difficulty,4,seed,[]);
            Assert.Equal(4,items.Count);
            foreach(var question in items)
            {
                var p=question.Parameters;
                Assert.Equal(7,p.Count);
                Assert.Contains("اضرب كثيرتي الحدود",question.Prompt);
                int[] expected=new int[5];
                for(int a=0;a<3;a++)
                for(int b=0;b<3;b++)
                    expected[a+b]+=p["a"+a]*p["b"+b];
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,question.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,"خطأ"));
                if(family==Coefficient)
                {
                    int degree=p["degree"];
                    int numeric=int.Parse(question.CorrectAnswer,CultureInfo.InvariantCulture);
                    Assert.Equal(expected[degree],numeric);
                    Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,(numeric+1).ToString(CultureInfo.InvariantCulture)));
                    Assert.Equal(AssessmentItemType.Numeric,question.ItemType);
                }
                else
                {
                    Assert.Equal(AssessmentItemType.ShortAnswer,question.ItemType);
                    Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,question.CorrectAnswer+"+1"));
                }
                var arabic=string.Concat(question.CorrectAnswer.Select(ch=>ch is >= '0' and <= '9'?(char)('٠'+ch-'0'):ch));
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,arabic));
                var forged=p.ToDictionary(x=>x.Key,x=>x.Value,StringComparer.Ordinal);
                forged["degree"]=9;
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,forged,question.CorrectAnswer));
            }
        }
    }

    [Fact]
    public void IndependentGoldenProductsMatchStudentBookSkillNotMonomialOnly()
    {
        foreach(int x in new[]{-3,-1,0,1,2,4})
        {
            Assert.Equal((x+2)*(x+5),x*x+7*x+10);
            Assert.Equal((2*x-3)*(x+4),2*x*x+5*x-12);
            Assert.Equal((x*x+2*x-1)*(x-3),x*x*x-x*x-7*x+3);
            Assert.Equal((2*x*x-x+3)*(x*x+2),2*x*x*x*x-x*x*x+7*x*x-2*x+6);
        }
    }
}
