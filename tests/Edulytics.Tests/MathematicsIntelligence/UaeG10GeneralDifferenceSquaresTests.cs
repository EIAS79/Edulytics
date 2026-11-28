using System.Globalization;
using Edulytics.Core.Enums;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Grade10 U01L08 inverse difference-of-squares exact Stage C authored tests.
/// Stage D owns actual execution; no tests were run for academic closure.
/// </summary>
public sealed class UaeG10GeneralDifferenceSquaresTests
{
    private const string LessonCode="PED:UAE-MOE-MATH:L10:GENERAL:01:08:DIFFERENCE-OF-TWO-SQUARES";
    private const string Factor="uae.g10.general.u01.l08.full_difference_squares_factor";
    private const string Roots="uae.g10.general.u01.l08.difference_squares_rational_roots";

    [Theory]
    [InlineData(Factor)]
    [InlineData(Roots)]
    public void NewGrade10SourceFamiliesRequireSymbolicStructuredEntry(string family)
    {
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
        Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var input));
        Assert.Equal(LessonPracticeInteractionKind.StructuredEntry,input!.InteractionKind);
    }

    [Theory]
    [InlineData(Factor)]
    [InlineData(Roots)]
    public void ExactFactorAndRationalRootsHaveIndependentBoundedVerifier(string family)
    {
        var engine=new ExactSkillContractQuestionEngine();
        foreach(int seed in new[]{20261124,20261125,20261126})
        foreach(var difficulty in new[]{ExactSkillQuestionDifficulty.Standard,
            ExactSkillQuestionDifficulty.Challenge})
        {
            var questions=engine.Generate(LessonCode,LessonCode,[family],difficulty,4,seed,[]);
            Assert.Equal(4,questions.Count);
            foreach(var q in questions)
            {
                var p=q.Parameters;
                Assert.Equal(2,p.Count);
                int a=p["a"],b=p["b"];
                Assert.InRange(a,1,8);Assert.InRange(b,1,9);
                Assert.Equal(1,Gcd(a,b));
                Assert.Equal(AssessmentItemType.ShortAnswer,q.ItemType);
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,q.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,"خطأ"));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,q.CorrectAnswer+"+1"));
                var localized=string.Concat(q.CorrectAnswer.Select(ch=>
                    ch is >= '0' and <= '9'?(char)('٠'+ch-'0'):ch));
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,localized));
                var bad=p.ToDictionary(x=>x.Key,x=>x.Value,StringComparer.Ordinal);
                bad["b"]=10;
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,bad,q.CorrectAnswer));
                for(int x=-3;x<=3;x++)
                    Assert.Equal(a*a*x*x-b*b,(a*x-b)*(a*x+b));
                if(family==Roots)
                {
                    var parts=q.CorrectAnswer.Split(',');
                    Assert.Equal(2,parts.Length);
                    Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,parts[1]+","+parts[0]));
                }
                else
                {
                    string prefix=a==1?"س":a.ToString(CultureInfo.InvariantCulture)+"س";
                    string other="("+prefix+"+"+b.ToString(CultureInfo.InvariantCulture)+")"+
                        "("+prefix+"−"+b.ToString(CultureInfo.InvariantCulture)+")";
                    Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,other));
                }
            }
        }
    }
    private static int Gcd(int a,int b)
    {
        while(b!=0){int t=a%b;a=b;b=t;}
        return a;
    }

    [Fact]
    public void SourceGoldenCasesAreFactoredInTheInverseDirection()
    {
        foreach(int x in new[]{-5,-2,-1,0,1,2,5})
        {
            Assert.Equal(9*x*x-16,(3*x-4)*(3*x+4));
            Assert.Equal(25-x*x,(5-x)*(5+x));
            Assert.Equal(12*x*x*x-3*x,3*x*(2*x-1)*(2*x+1));
            Assert.Equal(x*x*x*x-81,(x-3)*(x+3)*(x*x+9));
        }
    }
}
