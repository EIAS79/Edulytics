using Edulytics.Core.Curriculum;
using Edulytics.Services.LessonContent;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class UsCcssB01CategoricalGraphVisualsTests
{
    private static ExactSkillGeneratedQuestion Generate(
        string code,string family,int seed) =>
        Assert.Single(new ExactSkillContractQuestionEngine().Generate(
            "b01-real-categorical-source",code,[family],
            ExactSkillQuestionDifficulty.Standard,1,seed,[]));

    [Theory]
    [InlineData("PED:US-CCSS-MATH:G2:U01:L08","supporting.statistics.charts.mixed")]
    [InlineData("PED:US-CCSS-MATH:G2:U01:L09","supporting.statistics.table_total")]
    [InlineData("PED:US-CCSS-MATH:G3:U01:L05","supporting.statistics.charts.mixed")]
    public void CategoryBarsAndTotalMatchIndependentlyGradedActualQuestions(
        string lesson,string family)
    {
        var q=Generate(lesson,family,550012);
        var chart=Assert.Single(UsCcssB01CategoricalGraphVisuals.Build(lesson,[q]));
        Assert.Equal(RichLessonVisualKind.BarGraph,chart.Kind);
        Assert.Equal(4,chart.Rows.Count);
        var counts=chart.Rows.Select(r=>int.Parse(Assert.Single(r.Cells))).ToArray();
        Assert.All(counts,c=>Assert.InRange(c,1,50));
        Assert.Equal(int.Parse(q.CorrectAnswer),counts.Sum());
        Assert.Equal(new[] {q.Parameters["f1"],q.Parameters["f2"],
            q.Parameters["f3"],q.Parameters["f4"]},counts);
        Assert.Contains(counts.Sum().ToString(),chart.Description);
    }

    [Fact]
    public void ScaledGrade3GraphBarsExplainExactlyTheHowManyMoreOrFewerAnswer()
    {
        const string code="PED:US-CCSS-MATH:G3:U01:L07";
        const string family="usccss.b01.graph.scaled_compare";
        for(var seed=550100;seed<550110;seed++)
        {
            var q=Generate(code,family,seed);
            var chart=Assert.Single(UsCcssB01CategoricalGraphVisuals.Build(code,[q]));
            Assert.Equal(RichLessonVisualKind.BarGraph,chart.Kind);
            Assert.Equal(2,chart.Rows.Count);
            var counts=chart.Rows.Select(r=>int.Parse(Assert.Single(r.Cells))).ToArray();
            Assert.Equal(q.Parameters["ticksA"]*q.Parameters["unit"],counts[0]);
            Assert.Equal(q.Parameters["ticksB"]*q.Parameters["unit"],counts[1]);
            Assert.Equal(Math.Abs(counts[0]-counts[1]),int.Parse(q.CorrectAnswer));
            Assert.All(counts,c=>Assert.InRange(c,1,100));
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                family,q.Parameters,q.CorrectAnswer));
        }
    }

    [Theory]
    [InlineData("PED:US-CCSS-MATH:G2:U01:L10", "supporting.statistics.charts.mixed")]
    [InlineData("PED:US-CCSS-MATH:G2:U01:L10", "supporting.statistics.table_total")]
    public void SourceGrade2Unit1Lesson10GraphsDisplayTrueCategoricalCounts(
        string lesson,string family)
    {
        for (var seed=881110;seed<881114;seed++)
        {
            var question=Generate(lesson,family,seed);
            var graph=Assert.Single(UsCcssB01CategoricalGraphVisuals.Build(lesson,[question]));
            Assert.Equal(RichLessonVisualKind.BarGraph,graph.Kind);
            var counts=graph.Rows.Select(row=>int.Parse(Assert.Single(row.Cells))).ToArray();
            Assert.Equal(4,counts.Length);
            Assert.Equal(question.Parameters["f1"],counts[0]);
            Assert.Equal(question.Parameters["f2"],counts[1]);
            Assert.Equal(question.Parameters["f3"],counts[2]);
            Assert.Equal(question.Parameters["f4"],counts[3]);
            Assert.Equal(int.Parse(question.CorrectAnswer),counts.Sum());
        }
    }

    [Fact]
    public void SourceGrade2Unit1Lesson11GraphsServeTotalsAndComparisons()
    {
        const string code="PED:US-CCSS-MATH:G2:U01:L11";
        for (var seed=881200;seed<881205;seed++)
        {
            var totalQuestion=Generate(code,"supporting.statistics.category_total",seed);
            var totalGraph=Assert.Single(UsCcssB01CategoricalGraphVisuals.Build(code,[totalQuestion]));
            Assert.Equal(RichLessonVisualKind.BarGraph,totalGraph.Kind);
            var counts=totalGraph.Rows.Select(row=>int.Parse(Assert.Single(row.Cells))).ToArray();
            Assert.Equal(new[]{totalQuestion.Parameters["a"],totalQuestion.Parameters["b"],
                totalQuestion.Parameters["c"]},counts);
            Assert.Equal(int.Parse(totalQuestion.CorrectAnswer),counts.Sum());

            var comparison=Generate(code,"supporting.statistics.compare_category_counts",seed+101);
            var comparisonGraph=Assert.Single(UsCcssB01CategoricalGraphVisuals.Build(code,[comparison]));
            Assert.Equal(RichLessonVisualKind.BarGraph,comparisonGraph.Kind);
            var pair=comparisonGraph.Rows.Select(row=>int.Parse(Assert.Single(row.Cells))).ToArray();
            Assert.Equal(new[]{comparison.Parameters["a"],comparison.Parameters["b"]},pair);
            Assert.Equal(pair[0]>pair[1] ? "A" :
                pair[1]>pair[0] ? "B" : "equal",comparison.CorrectAnswer);
            Assert.Contains("Category A",comparisonGraph.Description);
            Assert.Contains("Category B",comparisonGraph.Description);
            Assert.Empty(UsCcssB01CategoricalGraphVisuals.Build(code,
                [comparison with {CorrectAnswer="9999"}]));
            Assert.Empty(UsCcssB01CategoricalGraphVisuals.Build(code,
                [comparison with {CorrectAnswer=Math.Abs(pair[0]-pair[1]).ToString()}]));
        }
    }

    [Fact]
    public void WrongQuestionAnswerOrNonGraphLessonCannotProduceMisleadingChart()
    {
        var lesson="PED:US-CCSS-MATH:G2:U01:L09";
        var q=Generate(lesson,"supporting.statistics.table_total",550013);
        Assert.Empty(UsCcssB01CategoricalGraphVisuals.Build(lesson,
            [q with {CorrectAnswer="999999"}]));
        Assert.Empty(UsCcssB01CategoricalGraphVisuals.Build(
            "PED:US-CCSS-MATH:G4:U05:L01",[q]));
        Assert.Empty(UsCcssB01CategoricalGraphVisuals.Build(
            "PED:US-CCSS-MATH:G2:U04:L08",[q]));
        Assert.Empty(UsCcssB01CategoricalGraphVisuals.Build(
            "PED:CAMBRIDGE-INTL-MATH:G2:U01:L09",[q]));
    }
}
