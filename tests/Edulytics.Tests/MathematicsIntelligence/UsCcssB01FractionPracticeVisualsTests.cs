using Edulytics.Core.Curriculum;
using Edulytics.Services.LessonContent;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class UsCcssB01FractionPracticeVisualsTests
{
    private static ExactSkillGeneratedQuestion One(string code, string family, int seed) =>
        Assert.Single(new ExactSkillContractQuestionEngine().Generate(
            "usccss-b01-source-fractions", code, [family],
            ExactSkillQuestionDifficulty.Standard, 1, seed, []));

    [Fact]
    public void UnitAndNonUnitFractionRepresentationUsesOriginalUnreducedParts()
    {
        var code="PED:US-CCSS-MATH:G1:U07:L11";
        var q=One(code,"fractions.represent.interpret.fraction_bar",410021);
        var bar=Assert.Single(UsCcssB01FractionPracticeVisuals.Build(code,[q]));
        Assert.Equal(RichLessonVisualKind.FractionBars,bar.Kind);
        Assert.Equal(q.Parameters["numerator"],bar.Rows[0].Numerator);
        Assert.Equal(q.Parameters["denominator"],bar.Rows[0].Denominator);
        Assert.InRange(bar.Rows[0].Numerator,1,bar.Rows[0].Denominator);
    }

    [Fact]
    public void EquivalentFractionDiagramCrossChecksBothOriginalFractions()
    {
        var code="PED:US-CCSS-MATH:G3:U05:L05";
        var q=One(code,"fractions.equivalent.number_line",410028);
        var bar=Assert.Single(UsCcssB01FractionPracticeVisuals.Build(code,[q]));
        Assert.Equal(RichLessonVisualKind.FractionBars,bar.Kind);
        Assert.Equal(q.Parameters["baseN"],bar.Rows[0].Numerator);
        Assert.Equal(q.Parameters["baseD"],bar.Rows[0].Denominator);
        Assert.Equal(q.Parameters["baseN"]*q.Parameters["targetD"],
            int.Parse(q.CorrectAnswer)*q.Parameters["baseD"]);
    }

    [Fact]
    public void QuarterInchRulerShowsExactlyTheWholeAndPartialInch()
    {
        var code="PED:US-CCSS-MATH:G3:U06:L02";
        var q=One(code,"usccss.b01.measure.quarter_inch",410031);
        var visual=UsCcssB01FractionPracticeVisuals.Build(code,[q]);
        Assert.Equal(2,visual.Count);
        var ruler=visual[0];var bar=visual[1];
        Assert.Equal(RichLessonVisualKind.NumberLine,ruler.Kind);
        Assert.Equal(RichLessonVisualKind.FractionBars,bar.Kind);
        Assert.Equal(q.Parameters["wholeInches"]*4+q.Parameters["quarters"]+1,
            ruler.Items.Count);
        Assert.Equal(q.CorrectAnswer,ruler.Items[^1]);
        Assert.Equal(q.Parameters["quarters"],bar.Rows[0].Numerator);
        Assert.Equal(4,bar.Rows[0].Denominator);
    }

    [Fact]
    public void SourceVerifiedEqualPartsAlwaysShowOneUnitFraction()
    {
        var code="PED:US-CCSS-MATH:G3:U05:L02";
        var family="usccss.b01.fraction.unit_parts";
        for (var seed=410100;seed<410115;seed++)
        {
            var q=One(code,family,seed);
            Assert.Equal("1/"+q.Parameters["denominator"],q.CorrectAnswer);
            var bar=Assert.Single(UsCcssB01FractionPracticeVisuals.Build(code,[q]));
            Assert.Equal(RichLessonVisualKind.FractionBars,bar.Kind);
            Assert.Equal(1,bar.Rows[0].Numerator);
            Assert.Equal(q.Parameters["denominator"],bar.Rows[0].Denominator);
        }
    }

    [Fact]
    public void WrongStructuredCorrectAnswerAndOtherProgrammeCannotCreateFractions()
    {
        var code="PED:US-CCSS-MATH:G3:U05:L05";
        var q=One(code,"fractions.equivalent.number_line",410051);
        var corrupted=q with { CorrectAnswer="9000" };
        Assert.Empty(UsCcssB01FractionPracticeVisuals.Build(code,[corrupted]));
        Assert.Empty(UsCcssB01FractionPracticeVisuals.Build(
            "PED:US-CCSS-MATH:G4:U05:L01",[q]));
        Assert.Empty(UsCcssB01FractionPracticeVisuals.Build(
            "PED:CAMBRIDGE-INTL-MATH:G3:U05:L05",[q]));
    }
}
