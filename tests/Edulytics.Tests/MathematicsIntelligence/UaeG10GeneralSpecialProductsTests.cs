using Edulytics.Core.Enums;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using System.Globalization;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Owner-source Grade10 General U01L04. Authored Stage A–B–C, NOT RUN until Stage D.
/// Square sums/differences and conjugates: full form and independent convolution.
/// </summary>
public sealed class UaeG10GeneralSpecialProductsTests
{
    private const string LessonCode="PED:UAE-MOE-MATH:L10:GENERAL:01:04:SPECIAL-PRODUCTS";
    private const string Whole="uae.g10.general.u01.l04.full_special_product";

    [Fact]
    public void WholeSpecialProductIsStructuredEntryNotNumericOrGenericContract()
    {
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(Whole));
        Assert.True(LessonPracticeInteractionRegistry.TryResolve(Whole,out var interaction));
        Assert.Equal(LessonPracticeInteractionKind.StructuredEntry,interaction!.InteractionKind);
    }

    [Fact]
    public void FullSpecialProductGeneratorVerifiesIndependentConvolutionAcrossSeeds()
    {
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var seed in new[]{20261124,20261125,20261126})
        foreach(var difficulty in new[]{ExactSkillQuestionDifficulty.Standard,
            ExactSkillQuestionDifficulty.Challenge})
        {
            var generated=engine.Generate(LessonCode,LessonCode,[Whole],difficulty,4,seed,[]);
            Assert.Equal(4,generated.Count);
            foreach(var q in generated)
            {
                var p=q.Parameters;
                Assert.Equal(7,p.Count);
                Assert.Equal(0,p["a2"]);Assert.Equal(0,p["b2"]);
                int a=p["a1"],b=p["a0"],c=p["b0"];
                Assert.InRange(a,1,7);Assert.Equal(a,p["b1"]);
                Assert.InRange(Math.Abs(b),1,7);Assert.InRange(Math.Abs(c),1,7);
                Assert.True(q.Prompt.Contains("مربع")||q.Prompt.Contains("المترافقين"));
                Assert.Equal(AssessmentItemType.ShortAnswer,q.ItemType);
                Assert.Equal(a*a,p["a1"]*p["b1"]);
                int constant=b*c,middle=a*(b+c);
                Assert.True(ExactSkillContractQuestionEngine.Verify(Whole,p,q.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(Whole,p,"خطأ"));
                Assert.False(ExactSkillContractQuestionEngine.Verify(Whole,p,q.CorrectAnswer+"+1"));
                var forged=p.ToDictionary(z=>z.Key,z=>z.Value,StringComparer.Ordinal);
                forged["degree"]=9;
                Assert.False(ExactSkillContractQuestionEngine.Verify(Whole,forged,q.CorrectAnswer));
                var digitLocalized=string.Concat(q.CorrectAnswer.Select(ch=>ch is >= '0' and <= '9'
                    ?(char)('٠'+ch-'0'):ch));
                Assert.True(ExactSkillContractQuestionEngine.Verify(Whole,p,digitLocalized));

                for(int x=-2;x<=3;x++)
                    Assert.Equal((a*x+b)*(a*x+c),a*a*x*x+middle*x+constant);
                if(c==-b)Assert.Equal(0,middle);
            }
        }
    }

    [Fact]
    public void OriginalBookSkillIdentitiesHoldForDistinctCases()
    {
        foreach(int x in new[]{-3,-1,0,1,2,5})
        {
            Assert.Equal((x+4)*(x+4),x*x+8*x+16);
            Assert.Equal((2*x-3)*(2*x-3),4*x*x-12*x+9);
            Assert.Equal((3*x+2)*(3*x-2),9*x*x-4);
            Assert.Equal((x+2)*(x-2),x*x-4);
        }
    }
}
