using Edulytics.Core.Enums;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using System.Globalization;
namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Original G10 U04L02: 0/1/2 real x-axis intercepts or bounded
/// integer left/right roots. Tests authored Stage C, not executed before Stage D.</summary>
public sealed class UaeG10GeneralQuadraticGraphRootsTests
{
    private const string Code="PED:UAE-MOE-MATH:L10:GENERAL:UNIFIED:U04:L02:C035";
    private static readonly string[] Families=[
      "uae.g10.general.u04.l02.real_root_count",
      "uae.g10.general.u04.l02.left_intercept",
      "uae.g10.general.u04.l02.right_intercept"
    ];
    [Fact]
    public void GraphicalRealRootsFamiliesUseIndependentDiscriminantVerifierAndNumericUi()
    {
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var family in Families)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var ui));
            Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad,ui!.InteractionKind);
            foreach(var seed in new[]{20261125,20261126,20261127,20261128})
            {
                var batch=engine.Generate(Code,Code,[family],ExactSkillQuestionDifficulty.Standard,5,seed,[]);
                Assert.Equal(5,batch.Count);
                foreach(var task in batch)
                {
                    var p=task.Parameters;Assert.Equal(8,p.Count);
                    int a=p["a"],b=p["b"],c=p["c"],mode=p["mode"];
                    int delta=b*b-4*a*c;Assert.NotEqual(0,a);
                    int expected=family switch{
                        "uae.g10.general.u04.l02.real_root_count" => delta>0?2:delta==0?1:0,
                        "uae.g10.general.u04.l02.left_intercept" => Math.Min(p["r1"],p["r2"]),
                        _=>Math.Max(p["r1"],p["r2"])
                    };
                    if(family!=Families[0])Assert.True(mode is 0 or 1);
                    Assert.Equal(expected.ToString(CultureInfo.InvariantCulture),task.CorrectAnswer);
                    Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,task.CorrectAnswer));
                    Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,"خطأ"));
                    Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,(expected+1).ToString(CultureInfo.InvariantCulture)));
                    if(mode!=2)
                    {
                        Assert.Equal(0,a*p["r1"]*p["r1"]+b*p["r1"]+c);
                        Assert.Equal(0,a*p["r2"]*p["r2"]+b*p["r2"]+c);
                    }
                    var arabic=string.Concat(task.CorrectAnswer.Select(ch=>ch is >= '0' and <= '9'?
                       (char)('٠'+ch-'0'):ch));
                    Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,arabic));
                    var invalid=p.ToDictionary(z=>z.Key,z=>z.Value,StringComparer.Ordinal);
                    invalid["a"]=0;
                    Assert.False(ExactSkillContractQuestionEngine.Verify(family,invalid,task.CorrectAnswer));
                }
            }
        }
    }
    [Fact]
    public void SourceOriginalGraphRootsGoldenAndDomainRestrictions()
    {
        foreach(int x in new[]{-2,3})Assert.Equal(0,x*x-x-6);
        Assert.Equal(0,3*3-6*3+9);
        Assert.True(4*4-4*1*5<0);
        foreach(int x in new[]{3,5})Assert.Equal(0,x*x-8*x+15);
        Assert.Equal(0,-16*5*5+64*5+80);
        Assert.Equal(0,-16*(-1)*(-1)+64*(-1)+80);
        Assert.True(-1<0); // mathematical root not physically valid time.
    }
}
