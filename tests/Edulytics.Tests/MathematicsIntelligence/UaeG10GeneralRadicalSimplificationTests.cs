using System.Globalization;
using Edulytics.Core.Enums;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>G10 General U02L05 radical perfect-factor coefficient only.
/// Written in A-B-C. DO NOT RUN until Stage D.</summary>
public sealed class UaeG10GeneralRadicalSimplificationTests
{
    private const string Code="PED:UAE-MOE-MATH:L10:GENERAL:02:05:SIMPLIFYING-RADICAL-EXPRESSIONS";
    private const string Square="uae.g10.general.u02.l05.square_root_extract_coefficient";
    private const string Nth="uae.g10.general.u02.l05.nth_root_extract_coefficient";
    [Theory]
    [InlineData(Square)]
    [InlineData(Nth)]
    public void ExactExtractedCoefficientHasCorrectNumericRenderer(string family)
    {
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
        Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var ui));
        Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad,ui!.InteractionKind);
    }
    [Theory]
    [InlineData(Square)]
    [InlineData(Nth)]
    public void GeneratorAndVerifierAreIndependentAndRejectInvalidFactors(string family)
    {
        var engine=new ExactSkillContractQuestionEngine();
        foreach(int seed in new[]{20261125,20261126,20261127})
        foreach(var difficulty in new[]{ExactSkillQuestionDifficulty.Standard,ExactSkillQuestionDifficulty.Challenge})
        {
            var items=engine.Generate(Code,Code,[family],difficulty,4,seed,[]);
            Assert.Equal(4,items.Count);
            foreach(var item in items)
            {
                var p=item.Parameters;
                Assert.Equal(4,p.Count);
                int degree=p["degree"],c=p["coefficient"],r=p["residual"];
                Assert.Equal(family==Square?2:degree,degree);
                Assert.InRange(degree,family==Square?2:3,family==Square?2:4);
                Assert.InRange(c,2,7);
                int raised=1;for(int i=0;i<degree;i++)raised*=c;
                Assert.Equal(raised*r,p["radicand"]);
                Assert.Equal(c,int.Parse(item.CorrectAnswer,CultureInfo.InvariantCulture));
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,item.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,(c+1).ToString(CultureInfo.InvariantCulture)));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,"invalid"));
                var arabic=string.Concat(item.CorrectAnswer.Select(x=>x is >= '0' and <= '9'?(char)('٠'+x-'0'):x));
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,arabic));
                var tamper=p.ToDictionary(x=>x.Key,x=>x.Value,StringComparer.Ordinal);
                tamper["radicand"]=p["radicand"]+1;
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,tamper,item.CorrectAnswer));
                Assert.Equal(AssessmentItemType.Numeric,item.ItemType);
            }
        }
    }
    [Fact]
    public void SourceGoldenRootIdentityAndDomainExamplesAreMathematicallySound()
    {
        Assert.InRange(Math.Abs(Math.Sqrt(72)-6*Math.Sqrt(2)),0,1e-9);
        Assert.InRange(Math.Abs(Math.Cbrt(54)-3*Math.Cbrt(2)),0,1e-9);
        Assert.InRange(Math.Abs(Math.Pow(162,0.25)-3*Math.Pow(2,0.25)),0,1e-9);
        Assert.True(double.IsNaN(Math.Sqrt(-9)));
        Assert.Equal(-2,Math.Cbrt(-8));
    }
}
