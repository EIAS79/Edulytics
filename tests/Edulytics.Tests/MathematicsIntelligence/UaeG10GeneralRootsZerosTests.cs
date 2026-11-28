using Edulytics.Core.Enums;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using System.Globalization;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Source-aligned Grade10 General U01L15: roots and zeros, fundamental
/// theorem, sign variations, and real-coefficient complex conjugates.
/// Test source authored in A–B–C. Runtime execution belongs to Stage D.
/// </summary>
public sealed class UaeG10GeneralRootsZerosTests
{
    private const string LessonCode=
        "PED:UAE-MOE-MATH:L10:GENERAL:UNIFIED:U01:L15:C015";
    private const string Descartes="uae.g10.general.u01.l15.descartes_variations";
    private const string Conjugate="uae.g10.general.u01.l15.complex_conjugate_zero";
    private const string Multiplicity="uae.g10.general.u01.l15.total_roots_with_multiplicity";

    [Theory]
    [InlineData(Descartes,LessonPracticeInteractionKind.StructuredEntry)]
    [InlineData(Conjugate,LessonPracticeInteractionKind.StructuredEntry)]
    [InlineData(Multiplicity,LessonPracticeInteractionKind.IntegerKeypad)]
    public void FamilyUsesCorrectSourceSpecificResponseUI(string family,LessonPracticeInteractionKind kind)
    {
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
        Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var contract));
        Assert.Equal(kind,contract!.InteractionKind);
    }

    [Theory]
    [InlineData(Descartes)]
    [InlineData(Conjugate)]
    [InlineData(Multiplicity)]
    public void GeneratorAndIndependentVerifierRejectIncorrectAnswersAndTampering(string family)
    {
        var engine=new ExactSkillContractQuestionEngine();
        foreach(int seed in new[]{20261124,20261125,20261126,20261127})
        foreach(var difficulty in new[]{
            ExactSkillQuestionDifficulty.Standard,
            ExactSkillQuestionDifficulty.Challenge})
        {
            var problems=engine.Generate(LessonCode,LessonCode,[family],difficulty,4,seed,[]);
            Assert.Equal(4,problems.Count);
            foreach(var problem in problems)
            {
                var p=problem.Parameters;
                string expected;
                if(family==Descartes)
                {
                    Assert.Equal(7,p.Count);
                    int degree=p["degree"];
                    Assert.InRange(degree,2,5);
                    int Changes(bool useNegative)
                    {
                        int prior=0,changes=0;
                        for(int d=degree;d>=0;d--)
                        {
                            int coeff=p["a"+d.ToString(CultureInfo.InvariantCulture)];
                            if(useNegative&&d%2==1)coeff=-coeff;
                            if(coeff==0)continue;
                            int sign=Math.Sign(coeff);
                            if(prior!=0&&sign!=prior)changes++;
                            prior=sign;
                        }
                        return changes;
                    }
                    int positive=Changes(false),negative=Changes(true);
                    expected=$"{positive},{negative}";
                    Assert.Equal(AssessmentItemType.ShortAnswer,problem.ItemType);
                    Assert.InRange(positive,0,5);Assert.InRange(negative,0,5);
                    Assert.True(ExactSkillContractQuestionEngine.Verify(
                        family,p,expected.Replace(',', '،')));
                }
                else if(family==Conjugate)
                {
                    Assert.Equal(2,p.Count);
                    int a=p["real"],b=p["imag"];
                    Assert.InRange(a,-4,4);Assert.NotEqual(0,b);
                    expected=$"{a}{(-b<0?"-":"+")}{Math.Abs(b)}i";
                    Assert.Equal(AssessmentItemType.ShortAnswer,problem.ItemType);
                }
                else
                {
                    Assert.Equal(4,p.Count);
                    Assert.NotEqual(p["r"],p["s"]);
                    int count=p["m"]+p["n"];
                    Assert.InRange(count,2,6);
                    expected=count.ToString(CultureInfo.InvariantCulture);
                    Assert.Equal(AssessmentItemType.Numeric,problem.ItemType);
                }
                Assert.Equal(expected,problem.CorrectAnswer);
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,expected));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,"خطأ"));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,expected+"1"));
                var arabic=string.Concat(expected.Select(ch=>ch is >= '0' and <= '9'
                    ?(char)('٠'+ch-'0'):ch));
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,arabic));
                var forged=p.ToDictionary(x=>x.Key,x=>x.Value,StringComparer.Ordinal);
                if(family==Descartes)forged["degree"]=6;
                else if(family==Conjugate)forged["imag"]=0;
                else forged["m"]=9;
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,forged,expected));
            }
        }
    }

    [Fact]
    public void OriginalMathematicalZerosAndConjugationIdentitiesCheckIndependently()
    {
        foreach(int x in new[]{-3,-2,-1,0,1,2,3})
        {
            Assert.Equal((x-1)*(x-1)*(x+2),x*x*x-3*x+2);
            Assert.Equal((x+1)*(x*x-6*x+13),x*x*x-5*x*x+7*x+13);
            Assert.Equal((x-1)*(x+2)*(x*x-4*x+5),
                x*x*x*x-3*x*x*x-x*x+13*x-10);
            Assert.Equal(x*x*x-x*x-4*x+4,(x-1)*(x-2)*(x+2));
        }
        // Quadratic factor of 2±3i is (x-2)^2+3^2.
        Assert.Equal(13,(0-2)*(0-2)+3*3);
        // Degree 5 with multiplicities 2 and 3 has five complex zeros,
        // even if there are only two distinct real abscissae.
        Assert.Equal(5,2+3);
    }
}
