using Edulytics.Core.Enums;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using System.Globalization;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Source Grade10 canonical36: i powers and exact complex arithmetic
/// numeric components. Written A/B/C; do not execute before Stage D.</summary>
public sealed class UaeG10GeneralComplexNumbersTests
{
    private const string Code="PED:UAE-MOE-MATH:L10:GENERAL:UNIFIED:U04:L03:C036";
    private static readonly string[] Families=[
        "uae.g10.general.u04.l03.sum_real",
        "uae.g10.general.u04.l03.sum_imag",
        "uae.g10.general.u04.l03.product_real",
        "uae.g10.general.u04.l03.product_imag",
        "uae.g10.general.u04.l03.i_power_real",
        "uae.g10.general.u04.l03.modulus_squared"
    ];
    [Fact]
    public void ExactComplexSubskillAnswersMatchIndependentArithmetic()
    {
        var engine=new ExactSkillContractQuestionEngine();
        foreach(string family in Families)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var ui));
            Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad,ui!.InteractionKind);
            foreach(int seed in new[]{20261125,20261126,20261127})
            {
                var batch=engine.Generate(Code,Code,[family],ExactSkillQuestionDifficulty.Standard,4,seed,[]);
                Assert.Equal(4,batch.Count);
                foreach(var q in batch)
                {
                    var p=q.Parameters;Assert.Equal(5,p.Count);
                    int a=p["a"],b=p["b"],c=p["c"],d=p["d"],n=p["n"];
                    int expected=family switch{
                        "uae.g10.general.u04.l03.sum_real"=>a+c,
                        "uae.g10.general.u04.l03.sum_imag"=>b+d,
                        "uae.g10.general.u04.l03.product_real"=>a*c-b*d,
                        "uae.g10.general.u04.l03.product_imag"=>a*d+b*c,
                        "uae.g10.general.u04.l03.i_power_real"=>n%4 switch {0=>1,2=>-1,_=>0},
                        _=>a*a+b*b
                    };
                    Assert.Equal(expected.ToString(CultureInfo.InvariantCulture),q.CorrectAnswer);
                    Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,q.CorrectAnswer));
                    Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,"خطأ"));
                    Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,(expected+1).ToString(CultureInfo.InvariantCulture)));
                    var ar=string.Concat(q.CorrectAnswer.Select(ch=>ch is >= '0' and <= '9'?(char)('٠'+ch-'0'):ch));
                    Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,ar));
                    var tampered=p.ToDictionary(x=>x.Key,x=>x.Value,StringComparer.Ordinal);
                    tampered["n"]=99;
                    Assert.False(ExactSkillContractQuestionEngine.Verify(family,tampered,q.CorrectAnswer));
                }
            }
        }
    }
    [Fact]
    public void OriginalComplexGoldenExamplesUseImaginarySquareNegativeOne()
    {
        // (2+3i)(4-2i)=(2*4-3*(-2))+(2*(-2)+3*4)i
        Assert.Equal(14,2*4-3*(-2));
        Assert.Equal(8,2*(-2)+3*4);
        Assert.Equal(13,3*3+2*2);
        Assert.Equal(169,(-5)*(-5)+12*12);
        // (4+2i)/(1-i) = 1+3i; multiply to verify.
        Assert.Equal(4,1*1-3*(-1));
        Assert.Equal(2,1*(-1)+3*1);
    }
}
