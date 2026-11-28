using Edulytics.Core.Enums;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using System.Globalization;

namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>Grade10 U03L06 source-exact matrix product. Written in A–B–C, NOT RUN until Stage D.</summary>
public sealed class UaeG10GeneralMatrixProductTests
{
 private const string Code="PED:UAE-MOE-MATH:L10:GENERAL:UNIFIED:U03:L06:C030";
 [Theory]
 [InlineData("uae.g10.general.u03.l06.matrix_product_entry")]
 [InlineData("uae.g10.general.u03.l06.matrix_product_compatible")]
 public void MatrixProductEntryOrCompatibilityIsIndependentlyScored(string family)
 {
    Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
    Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var ui));
    Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad,ui!.InteractionKind);
    var engine=new ExactSkillContractQuestionEngine();
    foreach(int seed in new[]{20261125,20261126,20261127})
    {
        var tasks=engine.Generate(Code,Code,[family],ExactSkillQuestionDifficulty.Standard,5,seed,[]);
        Assert.Equal(5,tasks.Count);
        foreach(var item in tasks)
        {
            var p=item.Parameters;
            int expected;
            if(family.EndsWith("_compatible",StringComparison.Ordinal))
            {
                Assert.Equal(4,p.Count);
                expected=p["ac"]==p["br"]?1:0;
            }
            else
            {
                Assert.Equal(10,p.Count);
                string row=p["row"].ToString(CultureInfo.InvariantCulture);
                string col=p["col"].ToString(CultureInfo.InvariantCulture);
                expected=p["a"+row+"1"]*p["b"+"1"+col]+p["a"+row+"2"]*p["b"+"2"+col];
            }
            Assert.Equal(expected,int.Parse(item.CorrectAnswer,CultureInfo.InvariantCulture));
            Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,item.CorrectAnswer));
            Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,(expected+1).ToString(CultureInfo.InvariantCulture)));
            var localized=string.Concat(item.CorrectAnswer.Select(ch=>ch is >= '0' and <= '9'?(char)('٠'+ch-'0'):ch));
            Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,localized));
        }
    }
 }
 [Fact]
 public void MatrixOrderIsNotGenerallyCommutative()
 {
    int[,] a={{1,2},{3,4}},b={{5,-1},{2,0}};
    int[,] ab={{9,-1},{23,-3}},ba={{2,6},{2,4}};
    for(int i=0;i<2;i++)for(int j=0;j<2;j++)
    {
        Assert.Equal(ab[i,j],a[i,0]*b[0,j]+a[i,1]*b[1,j]);
        Assert.Equal(ba[i,j],b[i,0]*a[0,j]+b[i,1]*a[1,j]);
    }
 }
}