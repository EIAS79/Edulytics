using Edulytics.Core.Curriculum;
using Edulytics.Core.Lessons;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Data.Seeding;
using Edulytics.Services.LessonContent;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class UsCcssB01SourceBoundMeasurementVisualsTests
{
    private const string G2Plot = "PED:US-CCSS-MATH:G2:U09:L04";

    [Fact]
    public void Grade2U09L04CreatesMeasuredPencilLinePlotsWithExactFrequencyArithmetic()
    {
        const string family="usccss.b01.g2.measurement.pencil_line_plot";
        Assert.True(LessonPracticeContractRegistry.TryResolve(G2Plot,out var contract));
        Assert.Equal([family],contract!.AllowedQuestionFamilies);
        var engine=new ExactSkillContractQuestionEngine();
        var modes=new HashSet<int>();
        for(var seed=880000;seed<880120;seed++)
        {
            var q=Assert.Single(engine.Generate("b01-g2-u09-pencil-plot",
                G2Plot,[family],ExactSkillQuestionDifficulty.Standard,1,seed,[]));
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                family,q.Parameters,q.CorrectAnswer));
            var picture=Assert.Single(UsCcssB01SourceTaskVisuals.Build(G2Plot,[q]));
            Assert.Equal(RichLessonVisualKind.LinePlot,picture.Kind);
            Assert.Equal(7,picture.Rows.Count);
            Assert.Equal(10,picture.Rows.Sum(row=>row.Cells.Count));
            var p=q.Parameters;
            var lower=Enumerable.Range(0,p["targetTick"]+1)
                .Sum(i=>p[$"count{i}"]);
            var upper=10-lower;
            Assert.Equal(p["mode"] switch {
                0=>lower,1=>upper,_=>Math.Abs(upper-lower)
            },int.Parse(q.CorrectAnswer));
            for(var i=0;i<7;i++)
            {
                Assert.Equal($"{p["start"]+i} cm",picture.Rows[i].Label);
                Assert.Equal(p[$"count{i}"],picture.Rows[i].Cells.Count);
                Assert.All(picture.Rows[i].Cells,x=>Assert.Equal("X",x));
            }
            modes.Add(p["mode"]);
            Assert.Empty(UsCcssB01SourceTaskVisuals.Build(G2Plot,
                [q with {CorrectAnswer="999"}]));
            Assert.Empty(UsCcssB01SourceTaskVisuals.Build(
                "PED:US-CCSS-MATH:G2:U09:L03",[q]));
            Assert.Empty(UsCcssB01SourceTaskVisuals.Build(
                "PED:CAMBRIDGE-INTL-MATH:G2:U09:L04",[q]));
        }
        Assert.Equal([0,1,2],modes.Order().ToArray());
    }

    private const string G1 = "PED:US-CCSS-MATH:G1:U06:L07";
    private const string G2 = "PED:US-CCSS-MATH:G2:U09:L03";

    [Theory]
    [InlineData(G1,"supporting.measurement.unit_iteration")]
    [InlineData(G2,"usccss.b01.measure.map_lengths")]
    public void ActualTwoLessonPracticeContractsBuildVerifiedSourceScopedScale(
        string code,string family)
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve(code,out var contract));
        Assert.Equal([family],contract!.AllowedQuestionFamilies);
        var engine=new ExactSkillContractQuestionEngine();
        var modes=new HashSet<int>();
        for (var seed=730100;seed<730164;seed++)
        {
            var question=Assert.Single(engine.Generate(
                "us-ccss-b01-measurement-ruler",code,[family],
                ExactSkillQuestionDifficulty.Standard,1,seed,[]));
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                question.Family,question.Parameters,question.CorrectAnswer));
            var diagram=Assert.Single(UsCcssB01SourceBoundMeasurementVisuals.Build(
                code,[question]));
            Assert.Equal(RichLessonVisualKind.Scale,diagram.Kind);
            var ticks=diagram.Items.Select(int.Parse).ToArray();
            Assert.Equal(0,ticks[0]);
            for(var i=1;i<ticks.Length;i++)
                Assert.Equal(1,ticks[i]-ticks[i-1]);
            if(code==G1)
            {
                var count=question.Parameters["units"];
                Assert.Equal(count,ticks[^1]);
                Assert.Equal(count+1,ticks.Length);
                Assert.Contains($"{count} unit tiles",diagram.Description);
            }
            else
            {
                var a=question.Parameters["a"];
                var b=question.Parameters["b"];
                var mode=question.Parameters["mode"];
                Assert.Equal(a+b,ticks[^1]);
                Assert.Equal([ "0",a.ToString(),(a+b).ToString()],
                    diagram.SecondaryItems);
                Assert.Equal(mode==0?a+b:b,int.Parse(question.CorrectAnswer));
                modes.Add(mode);
            }
            Assert.Empty(UsCcssB01SourceBoundMeasurementVisuals.Build(
                code,[question with {CorrectAnswer="999"}]));
            Assert.Empty(UsCcssB01SourceBoundMeasurementVisuals.Build(
                "PED:CAMBRIDGE-INTL-MATH:G2:U09:L03",[question]));
            Assert.Empty(UsCcssB01SourceBoundMeasurementVisuals.Build(
                "PED:US-CCSS-MATH:G2:U09:L04",[question]));
        }
        if(code==G2)Assert.Equal([0,1],modes.Order().ToArray());
    }

    [Theory]
    [InlineData(G1)]
    [InlineData(G2)]
    public void ActualStudentRichV2ContentIncludesNumberBoundMeasurementScale(string code)
    {
        var lesson=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .Where(doc=>doc.PackCode=="US-CCSS-MATH" &&
                        doc.VersionCode=="CCSSM-2010")
            .SelectMany(doc=>doc.Lessons)
            .Single(x=>x.LessonCode==code);
        var en=lesson.Translations.Single(x=>x.CultureCode=="en");
        var body=new CanonicalLessonTranslationRecord(
            en.CultureCode,en.Title,en.Explanation,en.KeyConceptsAndRules,
            en.WorkedExamples,en.StepByStepSolutions,en.CommonMistakes,en.QuickSummary);
        var rich=RichLessonContentV2Registry.Find(code,"en")
            ?? RichLessonContentV2RuntimeComposer.TryCompose(code,body);
        Assert.NotNull(rich);
        Assert.Contains(rich.Visuals,x=>x.Kind==RichLessonVisualKind.Scale);
        Assert.Contains(rich.Visuals,x=>x.Kind==RichLessonVisualKind.EquationSet);
    }
    [Fact]
    public void Grade2U09Lesson4ActualRichV2ContainsPencilLinePlot()
    {
        var lesson=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .Where(doc=>doc.PackCode=="US-CCSS-MATH" &&
                        doc.VersionCode=="CCSSM-2010")
            .SelectMany(doc=>doc.Lessons)
            .Single(x=>x.LessonCode==G2Plot);
        var en=lesson.Translations.Single(x=>x.CultureCode=="en");
        var body=new CanonicalLessonTranslationRecord(en.CultureCode,en.Title,
            en.Explanation,en.KeyConceptsAndRules,en.WorkedExamples,
            en.StepByStepSolutions,en.CommonMistakes,en.QuickSummary);
        var rich=RichLessonContentV2Registry.Find(G2Plot,"en") ??
            RichLessonContentV2RuntimeComposer.TryCompose(G2Plot,body);
        Assert.NotNull(rich);
        Assert.Contains(rich.Visuals,v=>v.Kind==RichLessonVisualKind.LinePlot);
    }

}
