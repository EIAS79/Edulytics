using System.Globalization;
using Edulytics.Core.Enums;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// UAE G10 General canonical U04L07: source-scoped transformations.
/// Written in Stage C, intentionally NOT RUN until Stage D.
/// </summary>
public sealed class UaeG10QuadraticTransformationsTests
{
    private const string Code="PED:UAE-MOE-MATH:L10:GENERAL:UNIFIED:U04:L07:C040";
    private static readonly string[] Families=[
        "uae.g10.general.u04.l07.vertex_h",
        "uae.g10.general.u04.l07.vertex_k",
        "uae.g10.general.u04.l07.opens_up",
        "uae.g10.general.u04.l07.value_at_x"
    ];

    [Fact]
    public void AllFourSourceFamiliesRequireIntegerKeypadAndExist()
    {
        foreach(var family in Families)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var ui));
            Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad,ui!.InteractionKind);
        }
    }

    [Fact]
    public void IndependentVertexAndProbeVerifiersRejectWrongOrForgedAnswers()
    {
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var family in Families)
        foreach(var seed in new[]{20261125,20261126})
        foreach(var difficulty in new[]{ExactSkillQuestionDifficulty.Standard,
            ExactSkillQuestionDifficulty.Challenge})
        {
            var items=engine.Generate(Code,Code,[family],difficulty,5,seed,[]);
            Assert.Equal(5,items.Count);
            foreach(var item in items)
            {
                var p=item.Parameters;
                Assert.Equal(4,p.Count);
                int a=p["a"],h=p["h"],k=p["k"],probe=p["probe"];
                Assert.InRange(Math.Abs(a),1,4);
                Assert.InRange(h,-4,4);Assert.InRange(k,-7,7);
                Assert.InRange(probe,h-2,h+2);
                int expected=family switch
                {
                    "uae.g10.general.u04.l07.vertex_h"=>h,
                    "uae.g10.general.u04.l07.vertex_k"=>k,
                    "uae.g10.general.u04.l07.opens_up"=>a>0?1:0,
                    _=>a*(probe-h)*(probe-h)+k
                };
                Assert.Equal(expected.ToString(CultureInfo.InvariantCulture),item.CorrectAnswer);
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,item.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,"خطأ"));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,
                    (expected+1).ToString(CultureInfo.InvariantCulture)));
                var arabic=string.Concat(item.CorrectAnswer.Select(x=>
                    x is >= '0' and <= '9'?(char)('٠'+x-'0'):x));
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,arabic));
                var forged=p.ToDictionary(x=>x.Key,x=>x.Value,StringComparer.Ordinal);
                forged["a"]=0;
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,forged,item.CorrectAnswer));
            }
        }
    }

    [Fact]
    public void GoldensForCompletionOfSquareAndDomainApplicationsAreMathematicallyEquivalent()
    {
        foreach(int x in new[]{-4,-1,0,1,2,5})
        {
            Assert.Equal(x*x+6*x-5,(x+3)*(x+3)-14);
            Assert.Equal(-2*x*x+8*x-3,-2*(x-2)*(x-2)+5);
            Assert.Equal(4*x*x-16*x-40,4*(x-2)*(x-2)-56);
        }
        Assert.Equal(18,-2*(3-3)*(3-3)+18);
        Assert.Equal(0,-2*(0-3)*(0-3)+18);
        Assert.Equal(0,-2*(6-3)*(6-3)+18);
    }
}
