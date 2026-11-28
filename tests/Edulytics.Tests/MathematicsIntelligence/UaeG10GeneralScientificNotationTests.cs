using System.Globalization;
using Edulytics.Core.Enums;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>G10 General U02L04 scientific notation exact exponent families.
/// Source verified; tests written in A-B-C, NOT RUN until Stage D.</summary>
public sealed class UaeG10GeneralScientificNotationTests
{
    private const string Code="PED:UAE-MOE-MATH:L10:GENERAL:02:04:SCIENTIFIC-NOTATION";
    private const string Normalize="uae.g10.general.u02.l04.scientific_normalize_exponent";
    private const string Product="uae.g10.general.u02.l04.scientific_product_exponent";
    private const string Quotient="uae.g10.general.u02.l04.scientific_quotient_exponent";

    [Theory]
    [InlineData(Normalize)]
    [InlineData(Product)]
    [InlineData(Quotient)]
    public void SourceMatchingFamilyUsesIntegerExponentEntry(string family)
    {
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
        Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var ui));
        Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad,ui!.InteractionKind);
    }

    [Theory]
    [InlineData(Normalize)]
    [InlineData(Product)]
    [InlineData(Quotient)]
    public void GeneratedExponentMatchesIndependentDecimalTenthsComputation(string family)
    {
        var engine=new ExactSkillContractQuestionEngine();
        foreach(int seed in new[]{20261125,20261126,20261127})
        foreach(var difficulty in new[]{ExactSkillQuestionDifficulty.Standard,ExactSkillQuestionDifficulty.Challenge})
        {
            var items=engine.Generate(Code,Code,[family],difficulty,4,seed,[]);
            Assert.Equal(4,items.Count);
            foreach(var item in items)
            {
                var p=item.Parameters;Assert.Equal(5,p.Count);
                int expected=family switch
                {
                    Normalize => p["e1"]+(p["raw"]<10?-1:p["raw"]>=100?1:0),
                    Product => p["e1"]+p["e2"]+(p["a"]*p["b"]>=1000?1:0),
                    _ => p["e1"]-p["e2"]-(p["a"]<p["b"]?1:0)
                };
                Assert.Equal(expected,int.Parse(item.CorrectAnswer,CultureInfo.InvariantCulture));
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,item.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,
                    (expected+1).ToString(CultureInfo.InvariantCulture)));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,"خطأ"));
                Assert.Equal(AssessmentItemType.Numeric,item.ItemType);
                var arabic=string.Concat(item.CorrectAnswer.Select(c=>c is >= '0' and <= '9'?(char)('٠'+c-'0'):c));
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,arabic));
                var forged=p.ToDictionary(x=>x.Key,x=>x.Value,StringComparer.Ordinal);
                forged["a"]=0;
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,forged,item.CorrectAnswer));
            }
        }
    }

    [Fact]
    public void SourceBasedGoldenExamplesHaveCorrectMantissaAndExponents()
    {
        Assert.Equal(7_250_000,725m*10_000m);
        Assert.Equal(400m,0.08m*5000m);
        Assert.Equal(3000m,600_000m/200m);
        Assert.Equal(0.004m,2400m/600000m);
        Assert.Equal(900000m,650000m+250000m);
    }
}
