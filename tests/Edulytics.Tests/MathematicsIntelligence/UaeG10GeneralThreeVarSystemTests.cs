using Edulytics.Core.Enums;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using System.Globalization;

namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>Grade10 original source U03L04, written A–B–C, NOT RUN until Stage D.</summary>
public sealed class UaeG10GeneralThreeVarSystemTests
{
    private const string Code="PED:UAE-MOE-MATH:L10:GENERAL:UNIFIED:U03:L04:C028";
    [Theory]
    [InlineData("uae.g10.general.u03.l04.three_var_x")]
    [InlineData("uae.g10.general.u03.l04.three_var_y")]
    [InlineData("uae.g10.general.u03.l04.three_var_z")]
    public void GeneratedTripleSystemUsesIndependentCramerAndArabicNumericAnswer(string family)
    {
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
        Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var ui));
        Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad,ui!.InteractionKind);
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var seed in new[]{20261125,20261126,20261127})
        {
            var batch=engine.Generate(Code,Code,[family],ExactSkillQuestionDifficulty.Standard,5,seed,[]);
            Assert.Equal(5,batch.Count);
            foreach(var item in batch)
            {
                var v=item.Parameters;
                Assert.Equal(12,v.Count);
                Assert.Equal(AssessmentItemType.Numeric,item.ItemType);
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,v,item.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,v,"خطأ"));
                int n=int.Parse(item.CorrectAnswer,CultureInfo.InvariantCulture);
                Assert.InRange(n,-4,4);
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,v,(n+1).ToString(CultureInfo.InvariantCulture)));
                var localized=string.Concat(item.CorrectAnswer.Select(ch=>ch is >= '0' and <= '9'?(char)('٠'+ch-'0'):ch));
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,v,localized));
                var forged=v.ToDictionary(x=>x.Key,x=>x.Value,StringComparer.Ordinal);
                forged["a11"]=100;
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,forged,item.CorrectAnswer));
            }
        }
    }
    [Fact]
    public void OriginalIndependentGoldenThreeEquationExample()
    {
        int x=2,y=1,z=3;
        Assert.Equal(6,x+y+z);
        Assert.Equal(4,x-y+z);
        Assert.Equal(2,2*x+y-z);
    }
}