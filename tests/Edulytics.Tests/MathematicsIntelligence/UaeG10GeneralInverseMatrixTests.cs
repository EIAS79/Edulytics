using Edulytics.Core.Enums;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using System.Globalization;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>Grade10 U03L08 inverse matrix exact rational. Authored Stage ABC, NOT RUN until D.</summary>
public sealed class UaeG10GeneralInverseMatrixTests
{
 private const string Code="PED:UAE-MOE-MATH:L10:GENERAL:UNIFIED:U03:L08:C032";
 [Theory]
 [InlineData("uae.g10.general.u03.l08.inverse2_entry")]
 [InlineData("uae.g10.general.u03.l08.inverse2_exists")]
 public void ExactInverseMatchesRationalMatrixFormula(string family)
 {
    Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
    Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var ui));
    Assert.Equal(family.EndsWith("_entry",StringComparison.Ordinal)?LessonPracticeInteractionKind.StructuredEntry:LessonPracticeInteractionKind.IntegerKeypad,ui!.InteractionKind);
    var engine=new ExactSkillContractQuestionEngine();
    foreach(var seed in new[]{20261125,20261126,20261127})
    {
        var batch=engine.Generate(Code,Code,[family],ExactSkillQuestionDifficulty.Standard,5,seed,[]);
        Assert.Equal(5,batch.Count);
        foreach(var q in batch)
        {
            var p=q.Parameters;Assert.Equal(6,p.Count);
            Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,q.CorrectAnswer));
            Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,"خطأ"));
            int D=p["a"]*p["d"]-p["b"]*p["c"];
            if(family.EndsWith("_exists",StringComparison.Ordinal))
                Assert.Equal(D==0?"0":"1",q.CorrectAnswer);
            else
            {
                Assert.NotEqual(0,D);
                int numerator=p["row"]==1?(p["col"]==1?p["d"]:-p["b"]):(p["col"]==1?-p["c"]:p["a"]);
                string[] fraction=q.CorrectAnswer.Split('/');
                long n=long.Parse(fraction[0],CultureInfo.InvariantCulture);
                long d=fraction.Length==2?long.Parse(fraction[1],CultureInfo.InvariantCulture):1;
                Assert.Equal((long)numerator*d,n*D);
            }
            var ar=string.Concat(q.CorrectAnswer.Select(ch=>ch is >= '0' and <= '9'?(char)('٠'+ch-'0'):ch));
            Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,ar));
        }
    }
 }
 [Fact]
 public void ExactInverseHasCorrectGoldenEntriesAndIdentity()
 {
    int a=1,b=2,c=3,d=4,D=a*d-b*c;
    Assert.Equal(-2,D);
    Assert.Equal(-2,d/D);Assert.Equal(1,-b/D);
    Assert.Equal(3.0/2.0,(double)-c/D);Assert.Equal(-.5,(double)a/D);
 }
}