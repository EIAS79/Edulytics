using Edulytics.Core.Enums;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using System.Globalization;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Grade10 General U04L01 source-aligned integer parabola features.
/// Authored Stage C; execute under Stage D only.
/// </summary>
public sealed class UaeG10GeneralQuadraticGraphTests
{
    private const string Code="PED:UAE-MOE-MATH:L10:GENERAL:UNIFIED:U04:L01:C034";
    private static readonly string[] Families=
    [
      "uae.g10.general.u04.l01.vertex_x",
      "uae.g10.general.u04.l01.vertex_y",
      "uae.g10.general.u04.l01.opens_up",
      "uae.g10.general.u04.l01.value_at_x"
    ];
    [Fact]
    public void ParabolaFamilyQuestionsHaveCorrectIndependentVertexAndSymmetryMath()
    {
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var family in Families)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var ui));
            Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad,ui!.InteractionKind);
            foreach(var seed in new[]{20261125,20261126,20261127})
            {
                var batch=engine.Generate(Code,Code,[family],ExactSkillQuestionDifficulty.Standard,4,seed,[]);
                Assert.Equal(4,batch.Count);
                foreach(var q in batch)
                {
                    var p=q.Parameters;Assert.Equal(4,p.Count);
                    int a=p["a"],b=p["b"],c=p["c"],probe=p["probe"];
                    Assert.NotEqual(0,a);Assert.Equal(0,b%(2*a));
                    int h=-b/(2*a),k=c-a*h*h;
                    Assert.InRange(h,-4,4);Assert.InRange(k,-8,8);
                    int expected=family switch
                    {
                        "uae.g10.general.u04.l01.vertex_x"=>h,
                        "uae.g10.general.u04.l01.vertex_y"=>k,
                        "uae.g10.general.u04.l01.opens_up"=>a>0?1:0,
                        _=>a*probe*probe+b*probe+c
                    };
                    Assert.Equal(expected.ToString(CultureInfo.InvariantCulture),q.CorrectAnswer);
                    Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,q.CorrectAnswer));
                    Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,"خطأ"));
                    Assert.False(ExactSkillContractQuestionEngine.Verify(
                        family,p,(expected+1).ToString(CultureInfo.InvariantCulture)));
                    var ar=string.Concat(q.CorrectAnswer.Select(ch=>ch is >= '0' and <= '9'?
                        (char)('٠'+ch-'0'):ch));
                    Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,ar));
                    var tampered=p.ToDictionary(x=>x.Key,x=>x.Value,StringComparer.Ordinal);
                    tampered["a"]=0;
                    Assert.False(ExactSkillContractQuestionEngine.Verify(family,tampered,q.CorrectAnswer));
                }
            }
        }
    }
    [Fact]
    public void OriginalQuadraticGraphGoldenCoordinatesAreSymmetric()
    {
        foreach(int x in new[]{-3,-1,0,1,2,3,4})
        {
            int y=x*x-4*x+3;
            Assert.Equal(y,(4-x)*(4-x)-4*(4-x)+3);
            Assert.Equal(-1,2*2-4*2+3);
            Assert.Equal(5,-2*2*2+8*2-3);
            Assert.Equal(-2,(-1)*(-1)+4*(-1)-3);
        }
        Assert.Equal(550,-10*5*5+100*5+300);
    }
}
