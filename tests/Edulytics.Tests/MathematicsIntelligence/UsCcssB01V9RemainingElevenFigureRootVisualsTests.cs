using Edulytics.Core.Curriculum;
using Edulytics.Core.Lessons;
using Edulytics.Data.Seeding;
using Edulytics.Services.LessonContent;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// B01 all-500 exact source identity: the formerly equations-only eleven
/// figure-source lessons now have a source-goal-bound representative visual.
/// This explicitly does not pass original activity-by-activity fidelity/L6.
/// </summary>
public sealed class UsCcssB01V9RemainingElevenFigureRootVisualsTests
{
    private const string Prefix="PED:US-CCSS-MATH:";

    [Theory]
    [InlineData("G2:U06:L03","940b693c00be0380fb8449140b2648a3aa593291b5894f89643fcd9f7eb15c91","RectangleDiagram")]
    [InlineData("G2:U06:L19","5ed030269c542b6f0a1a7bb27c7108712dbb1c15daea05a36e9e424228de2a36","TapeDiagram")]
    [InlineData("G2:U08:L02","660fafd839d002c655c9495a460b037892e07cea3cc2a907d0bd7ba17246a28a","CounterPairs")]
    [InlineData("G2:U08:L03","41ca33c004b6c33458ca2f0e9003ccfc115f9077702d9c57ac358b6ece291f9c","CounterPairs")]
    [InlineData("G3:U04:L11","22c621a374729fce1b200753918143a74e10bfbb7132656d83a87d5ed10bcf09","RectangleDiagram")]
    [InlineData("G3:U04:L15","89ed7539905eec8ad315639c003331ddd008a90007b81c5434db0cc47788abff","RectangleDiagram")]
    [InlineData("G3:U07:L01","9f71865e95819ead4a1726c0a6f9b55267c73fffaa9a401dd0ddd07e98087543","PolygonDiagram")]
    [InlineData("G3:U07:L04","0d135f72d96aeae8eec1aa54ade2bd79325201f4e56efe9dec167ff3cca88967","PolygonDiagram")]
    [InlineData("G3:U07:L06","c1231a52553453e0c5568f68fc908b247d53728ef1a65a656cc2bf829221933e","RectangleDiagram")]
    [InlineData("G3:U08:L01","9af7452882227dec9eb391337cf76a47c64497fcba9e30f126cb80b04af63da3","FractionBars")]
    [InlineData("G4:U04:L20","26096e487bc9df48865e2a3485c3001d75ab7d8a4a147524b5d7a527fef9ddac","PlaceValueChart")]
    public void ElevenSourcePurposeLessonsGainMathematicallyConcreteVisual(
        string suffix,string rawBodySha,string kind)
    {
        var code=Prefix+suffix;
        var pair=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .Where(d=>d.PackCode=="US-CCSS-MATH"&&d.VersionCode=="CCSSM-2010")
            .SelectMany(d=>d.Lessons.Select(l=>(Doc:d,Lesson:l)))
            .Single(x=>x.Lesson.LessonCode==code);
        Assert.Equal(rawBodySha,pair.Lesson.CanonicalBodySha256);
        Assert.StartsWith("https://im.kendallhunt.com/k5/teachers/",
            pair.Lesson.SourceUrl);
        var original=pair.Lesson.Translations.Single(t=>t.CultureCode=="en");
        var rich=RichLessonContentV2RuntimeComposer.TryCompose(code,
            new CanonicalLessonTranslationRecord(
                original.CultureCode,original.Title,original.Explanation,
                original.KeyConceptsAndRules,original.WorkedExamples,
                original.StepByStepSolutions,original.CommonMistakes,
                original.QuickSummary));
        Assert.NotNull(rich);
        Assert.Contains(rich.Visuals,x=>x.Kind.ToString()==kind);
        Assert.Contains(rich.Visuals,x=>x.Kind==RichLessonVisualKind.EquationSet);
        Assert.All(rich.Visuals.Where(x=>x.Kind.ToString()==kind),
            x=>Assert.False(string.IsNullOrWhiteSpace(x.Description)));
    }

    [Fact]
    public void OneSharedElevenLessonScopeAndEvenOddSemanticsStayFailClosed()
    {
        Assert.Equal(11,UsCcssB01V9SourceFigureStructuralGapVisuals.ExpectedCount);
        var even=UsCcssB01V9SourceFigureStructuralGapVisuals.Build(
            Prefix+"G2:U08:L02",
            GetBody(Prefix+"G2:U08:L02"));
        Assert.NotNull(even);
        Assert.Equal(["8","9"],even.Select(v=>v.Items.Single()).ToArray());
        var measurements=UsCcssB01V9SourceFigureStructuralGapVisuals.Build(
            Prefix+"G2:U06:L03",GetBody(Prefix+"G2:U06:L03"));
        Assert.NotNull(measurements);
        Assert.Equal("cm",Assert.Single(measurements).SecondaryLabel);
        var money=UsCcssB01V9SourceFigureStructuralGapVisuals.Build(
            Prefix+"G2:U06:L19",GetBody(Prefix+"G2:U06:L19"));
        Assert.NotNull(money);
        Assert.Equal("cents",Assert.Single(money).SecondaryLabel);
        var area=UsCcssB01V9SourceFigureStructuralGapVisuals.Build(
            Prefix+"G3:U04:L11",GetBody(Prefix+"G3:U04:L11"));
        Assert.NotNull(area);
        Assert.Equal(3,area.Count);
        Assert.All(area,v=>Assert.Equal("fixed-comparison-scale",v.SecondaryLabel));
        Assert.Contains("zero counters left over",even[0].Description);
        Assert.Contains("one counter left over",even[1].Description);
        var more=UsCcssB01V9SourceFigureStructuralGapVisuals.Build(
            Prefix+"G2:U08:L03",GetBody(Prefix+"G2:U08:L03"));
        Assert.NotNull(more);
        Assert.Equal(["9","10"],more.Select(v=>v.Items.Single()).ToArray());
        Assert.Contains("zero counters left over",more[1].Description);
        Assert.Null(UsCcssB01V9SourceFigureStructuralGapVisuals.Build(
            Prefix+"G2:U08:L04",GetBody(Prefix+"G2:U08:L02")));
        var altered=GetBody(Prefix+"G2:U08:L02") with { Title="Wrong title" };
        Assert.Throws<InvalidOperationException>(()=>
            UsCcssB01V9SourceFigureStructuralGapVisuals.Build(
                Prefix+"G2:U08:L02",altered));
    }

    [Fact]
    public void RegroupedPlaceValueAndFractionDenominatorsAreCorrectNotStyleOnly()
    {
        var regroup=UsCcssB01V9SourceFigureStructuralGapVisuals.Build(
            Prefix+"G4:U04:L20",GetBody(Prefix+"G4:U04:L20"));
        Assert.NotNull(regroup);
        var visual=Assert.Single(regroup);
        Assert.Equal(RichLessonVisualKind.PlaceValueChart,visual.Kind);
        Assert.Equal(["7","5","8","13","10"],visual.Rows[2].Cells);
        var regrouped=75800+130+10;
        Assert.Equal(75940,regrouped);
        Assert.Equal(63154,regrouped-12786);
        Assert.Equal(["6","3","1","5","4"],visual.Rows[4].Cells);
        var fractions=UsCcssB01V9SourceFigureStructuralGapVisuals.Build(
            Prefix+"G3:U08:L01",GetBody(Prefix+"G3:U08:L01"));
        Assert.NotNull(fractions);
        var bar=Assert.Single(fractions);
        Assert.Equal(8,bar.Rows[0].Denominator);
        Assert.Equal(8,bar.Rows[1].Denominator);
        Assert.Equal(4,bar.Rows[0].Numerator);
        Assert.Equal(5,bar.Rows[1].Numerator);
        Assert.True(5.0/8 > 1.0/2);
    }

    private static CanonicalLessonTranslationRecord GetBody(string code)
    {
        var original=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(d=>d.Lessons).Single(l=>l.LessonCode==code)
            .Translations.Single(t=>t.CultureCode=="en");
        return new CanonicalLessonTranslationRecord(
            original.CultureCode,original.Title,original.Explanation,
            original.KeyConceptsAndRules,original.WorkedExamples,
            original.StepByStepSolutions,original.CommonMistakes,
            original.QuickSummary);
    }
}