using Edulytics.Core.Enums;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using System.Globalization;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>Stage A–B–C authored, Stage D NOT RUN. Grade10 source Cramer exact 2x2 grader.</summary>
public sealed class UaeG10GeneralCramerDeterminantsTests
{
 private const string Code="PED:UAE-MOE-MATH:L10:GENERAL:UNIFIED:U03:L07:C031";
 [Theory]
 [InlineData("uae.g10.general.u03.l07.determinant2")]
 [InlineData("uae.g10.general.u03.l07.cramer_x")]
 [InlineData("uae.g10.general.u03.l07.cramer_y")]
 public void CramerExactAnswersUseIndependentDeterminantAndArabicDigits(string family)
 {
    Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
    Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var ui));
    Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad,ui!.InteractionKind);
    var engine=new ExactSkillContractQuestionEngine();
    foreach(var seed in new[]{20261125,20261126,20261127})
    {
        var tasks=engine.Generate(Code,Code,[family],ExactSkillQuestionDifficulty.Standard,5,seed,[]);
        Assert.Equal(5,tasks.Count);
        foreach(var item in tasks)
        {
            var p=item.Parameters;
            Assert.Equal(6,p.Count);
            int a=p["a"],b=p["b"],c=p["c"],d=p["d"],r1=p["r1"],r2=p["r2"];
            int D=a*d-b*c,Dx=r1*d-b*r2,Dy=a*r2-c*r1;
            Assert.NotEqual(0,D);
            int expected=family.EndsWith("determinant2",StringComparison.Ordinal)?D:family.EndsWith("cramer_x",StringComparison.Ordinal)?Dx/D:Dy/D;
            Assert.Equal(expected,int.Parse(item.CorrectAnswer,CultureInfo.InvariantCulture));
            Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,item.CorrectAnswer));
            Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,(expected+1).ToString(CultureInfo.InvariantCulture)));
            var localized=string.Concat(item.CorrectAnswer.Select(ch=>ch is >= '0' and <= '9'?(char)('٠'+ch-'0'):ch));
            Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,localized));
        }
    }
 }
 [Fact]
 public void GoldenCramerAndDeterminantMath()
 {
    Assert.Equal(39,4*6-5*(-3));
    int D=2*(-1)-1*1,Dx=7*(-1)-1*(-1),Dy=2*(-1)-7*1;
    Assert.Equal(-3,D);Assert.Equal(2,Dx/D);Assert.Equal(3,Dy/D);
 }
}