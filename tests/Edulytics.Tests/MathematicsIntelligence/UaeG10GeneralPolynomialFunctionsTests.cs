using Edulytics.Core.Enums;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using System.Globalization;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Original Grade10 General unified U01L11 polynomial functions stage C contract.
/// Written during A-B-C. Not executed until Stage D.
/// </summary>
public sealed class UaeG10GeneralPolynomialFunctionsTests
{
    private const string LessonCode =
        "PED:UAE-MOE-MATH:L10:GENERAL:UNIFIED:U01:L11:C011";
    private const string Evaluate =
        "uae.g10.general.u01.l11.polynomial_evaluate_integer";
    private const string Degree =
        "uae.g10.general.u01.l11.polynomial_degree_integer";
    private const string End =
        "uae.g10.general.u01.l11.polynomial_end_behavior";

    [Theory]
    [InlineData(Evaluate,LessonPracticeInteractionKind.IntegerKeypad)]
    [InlineData(Degree,LessonPracticeInteractionKind.IntegerKeypad)]
    [InlineData(End,LessonPracticeInteractionKind.StructuredEntry)]
    public void FamiliesResolveToSourceCorrectStudentInput(
        string family, LessonPracticeInteractionKind expected)
    {
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
        Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var actual));
        Assert.Equal(expected,actual!.InteractionKind);
    }

    [Theory]
    [InlineData(Evaluate)]
    [InlineData(Degree)]
    [InlineData(End)]
    public void GeneratedExactAnswersAreIndependentlyChecked(string family)
    {
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var seed in new[] {20261124,20261125,20261126})
        foreach(var difficulty in new[] {
            ExactSkillQuestionDifficulty.Standard,
            ExactSkillQuestionDifficulty.Challenge})
        {
            var problems=engine.Generate(
                LessonCode,LessonCode,[family],difficulty,4,seed,[]);
            Assert.Equal(4,problems.Count);

            foreach(var problem in problems)
            {
                var p=problem.Parameters;
                Assert.Equal(8,p.Count);
                int degree=p["degree"],x=p["x"];
                Assert.InRange(degree,1,5);
                Assert.InRange(x,-3,3);
                Assert.NotEqual(0,p["a"+degree.ToString(CultureInfo.InvariantCulture)]);
                for(int d=degree+1;d<=5;d++)
                    Assert.Equal(0,p["a"+d.ToString(CultureInfo.InvariantCulture)]);

                string expected;
                if(family==Degree)
                    expected=degree.ToString(CultureInfo.InvariantCulture);
                else if(family==Evaluate)
                {
                    int value=0;
                    for(int d=5;d>=0;d--)
                        value=value*x+p["a"+d.ToString(CultureInfo.InvariantCulture)];
                    expected=value.ToString(CultureInfo.InvariantCulture);
                }
                else
                {
                    bool even=degree%2==0;
                    bool positive=p["a"+degree.ToString(CultureInfo.InvariantCulture)]>0;
                    expected=even
                        ?(positive?"أعلى,أعلى":"أسفل,أسفل")
                        :(positive?"أسفل,أعلى":"أعلى,أسفل");
                }

                Assert.Equal(expected,problem.CorrectAnswer);
                Assert.True(ExactSkillContractQuestionEngine.Verify(
                    family,p,expected));
                Assert.False(ExactSkillContractQuestionEngine.Verify(
                    family,p,"خطأ"));
                Assert.False(ExactSkillContractQuestionEngine.Verify(
                    family,p,family==End?"أعلى,أسفل,أسفل":expected+"1"));

                if(family==End)
                {
                    Assert.Equal(AssessmentItemType.ShortAnswer,problem.ItemType);
                    Assert.True(ExactSkillContractQuestionEngine.Verify(
                        family,p,expected.Replace(',', '،')));
                }
                else
                {
                    Assert.Equal(AssessmentItemType.Numeric,problem.ItemType);
                    var arabic=string.Concat(expected.Select(ch=>
                        ch is >= '0' and <= '9'?(char)('٠'+ch-'0'):ch));
                    Assert.True(ExactSkillContractQuestionEngine.Verify(
                        family,p,arabic));
                }

                var forged=p.ToDictionary(z=>z.Key,z=>z.Value,StringComparer.Ordinal);
                forged["degree"]=6;
                Assert.False(ExactSkillContractQuestionEngine.Verify(
                    family,forged,expected));
                forged=p.ToDictionary(z=>z.Key,z=>z.Value,StringComparer.Ordinal);
                forged["x"]=100;
                Assert.False(ExactSkillContractQuestionEngine.Verify(
                    family,forged,expected));
            }
        }
    }

    [Fact]
    public void SourceGoldenCubicQuarticAndContextualValuesAreIndependent()
    {
        foreach(int x in new[]{-5,-3,-2,-1,0,1,2,3,5})
        {
            Assert.Equal(x*x*x-4*x,x*(x*x-4));
            Assert.Equal(x*x*x*x-5*x*x+4,(x*x-1)*(x*x-4));
            Assert.Equal(-x*x*x+4*x,-(x*x*x-4*x));
        }
        Assert.Equal(3,(-1)*(-1)*(-1)-4*(-1));
        Assert.Equal(0,2*2*2-4*2);
    }
}
