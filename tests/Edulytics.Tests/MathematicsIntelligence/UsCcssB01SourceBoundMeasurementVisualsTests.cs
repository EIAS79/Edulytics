using Edulytics.Core.Curriculum;
using Edulytics.Core.Lessons;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Data.Seeding;
using Edulytics.Services.LessonContent;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class UsCcssB01SourceBoundMeasurementVisualsTests
{
    [Theory]
    [InlineData("PED:US-CCSS-MATH:G1:U06:L11","usccss.b01.g1.measurement.story_result_difference",3)]
    [InlineData("PED:US-CCSS-MATH:G1:U06:L12","usccss.b01.g1.measurement.compare_bigger_smaller_unknown",2)]
    public void SourceBoundGradeOneMeasurementStoryUnknownsAreAccurate(
        string code,string family,int modeCount)
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve(code,out var contract));
        Assert.Equal([family],contract!.AllowedQuestionFamilies);
        var engine=new ExactSkillContractQuestionEngine();
        var modes=new HashSet<int>();
        for(var seed=991000;seed<991100;seed++)
        {
            var q=Assert.Single(engine.Generate("g1-u06-source-stories",code,
                [family],ExactSkillQuestionDifficulty.Standard,1,seed,[]));
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                family,q.Parameters,q.CorrectAnswer));
            var p=q.Parameters;
            int answer=int.Parse(q.CorrectAnswer);
            if(modeCount==3)
            {
                Assert.InRange(p["total"],6,20);
                Assert.Equal(p["first"]+p["other"],p["total"]);
                Assert.Equal(p["mode"]==0?p["total"]:p["other"],answer);
            }
            else
            {
                Assert.InRange(p["longer"],6,20);
                Assert.Equal(p["shorter"]+p["difference"],p["longer"]);
                Assert.Equal(p["mode"]==0?p["longer"]:p["shorter"],answer);
            }
            var diagram=Assert.Single(UsCcssB01SourceTaskVisuals.Build(code,[q]));
            Assert.Equal(RichLessonVisualKind.TapeDiagram,diagram.Kind);
            Assert.Equal(4,diagram.Items.Count);
            Assert.Equal(p["mode"].ToString(),diagram.Items[3]);
            Assert.Empty(UsCcssB01SourceTaskVisuals.Build(code,
                [q with {CorrectAnswer="999"}]));
            Assert.Empty(UsCcssB01SourceTaskVisuals.Build(
                "PED:US-CCSS-MATH:G1:U06:L10",[q]));
            modes.Add(p["mode"]);
        }
        Assert.Equal(Enumerable.Range(0,modeCount),modes.Order());
        var lesson=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .Where(d=>d.PackCode=="US-CCSS-MATH" && d.VersionCode=="CCSSM-2010")
            .SelectMany(d=>d.Lessons).Single(x=>x.LessonCode==code);
        var en=lesson.Translations.Single(x=>x.CultureCode=="en");
        var body=new CanonicalLessonTranslationRecord(en.CultureCode,en.Title,
            en.Explanation,en.KeyConceptsAndRules,en.WorkedExamples,
            en.StepByStepSolutions,en.CommonMistakes,en.QuickSummary);
        var rich=RichLessonContentV2Registry.Find(code,"en") ??
            RichLessonContentV2RuntimeComposer.TryCompose(code,body);
        Assert.NotNull(rich);
        Assert.Contains(rich.Visuals,x=>x.Kind==RichLessonVisualKind.TapeDiagram);
    }


    [Fact]
    public void Grade2Unit3InchAndFootUnitsMatchExactPublisherPractice()
    {
        const string inchCode="PED:US-CCSS-MATH:G2:U03:L08";
        const string feetCode="PED:US-CCSS-MATH:G2:U03:L09";
        const string inchFamily="usccss.b01.g2.measurement.inch_ruler";
        const string feetFamily="usccss.b01.g2.measurement.feet_inch_same_length";
        var engine=new ExactSkillContractQuestionEngine();
        Assert.True(LessonPracticeContractRegistry.TryResolve(inchCode,out var inch));
        Assert.True(LessonPracticeContractRegistry.TryResolve(feetCode,out var feet));
        Assert.Equal([inchFamily],inch!.AllowedQuestionFamilies);
        Assert.Equal([feetFamily],feet!.AllowedQuestionFamilies);
        var foundFish=new HashSet<int>();
        var foundModes=new HashSet<int>();
        for(var seed=900000;seed<900096;seed++)
        {
            var qi=Assert.Single(engine.Generate("b01-inch-ruler",
                inchCode,[inchFamily],ExactSkillQuestionDifficulty.Standard,1,seed,[]));
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                inchFamily,qi.Parameters,qi.CorrectAnswer));
            var ip=qi.Parameters;
            Assert.InRange(ip["actual"],1,11);
            Assert.Equal(Math.Abs(ip["estimate"]-ip["actual"]),
                int.Parse(qi.CorrectAnswer));
            var ruler=Assert.Single(UsCcssB01SourceTaskVisuals.Build(inchCode,[qi]));
            Assert.Equal(RichLessonVisualKind.Scale,ruler.Kind);
            Assert.Equal(Enumerable.Range(0,ip["actual"]+1).Select(i=>i.ToString()),
                ruler.Items);
            Assert.Empty(UsCcssB01SourceTaskVisuals.Build(inchCode,
                [qi with {CorrectAnswer="999"}]));
            Assert.Empty(UsCcssB01SourceTaskVisuals.Build(feetCode,[qi]));

            var qf=Assert.Single(engine.Generate("b01-fish-tape",
                feetCode,[feetFamily],ExactSkillQuestionDifficulty.Standard,1,seed,[]));
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                feetFamily,qf.Parameters,qf.CorrectAnswer));
            var fp=qf.Parameters;
            Assert.Equal(fp["fish"] switch {0=>24,1=>48,_=>36},fp["inchTiles"]);
            Assert.Equal(fp["fish"] switch {0=>2,1=>4,_=>3},fp["footRulers"]);
            Assert.Equal(fp["mode"] switch {0=>1,1=>2,2=>fp["inchTiles"],3=>fp["footRulers"],_=>throw new InvalidOperationException()},int.Parse(qf.CorrectAnswer));
            var comparison=Assert.Single(UsCcssB01SourceTaskVisuals.Build(feetCode,[qf]));
            Assert.Equal(RichLessonVisualKind.RatioTable,comparison.Kind);
            Assert.Equal(2,comparison.Rows.Count);
            Assert.Equal(fp["inchTiles"].ToString(),comparison.Rows[0].Cells[0]);
            Assert.Equal(fp["footRulers"].ToString(),comparison.Rows[1].Cells[0]);
            Assert.Equal(comparison.Rows[0].Cells[2],comparison.Rows[1].Cells[2]);
            Assert.Empty(UsCcssB01SourceTaskVisuals.Build(feetCode,
                [qf with {CorrectAnswer="0"}]));
            Assert.Empty(UsCcssB01SourceTaskVisuals.Build(inchCode,[qf]));
            foundFish.Add(fp["fish"]);
            foundModes.Add(fp["mode"]);
        }
        Assert.Equal([0,1,2],foundFish.Order().ToArray());
        Assert.Equal([0,1,2,3],foundModes.Order().ToArray());

        foreach(var item in new[]{
            (code:inchCode,kind:RichLessonVisualKind.Scale),
            (code:feetCode,kind:RichLessonVisualKind.RatioTable)
        })
        {
            var lesson=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
                .Where(d=>d.PackCode=="US-CCSS-MATH" &&
                           d.VersionCode=="CCSSM-2010")
                .SelectMany(d=>d.Lessons).Single(x=>x.LessonCode==item.code);
            var en=lesson.Translations.Single(x=>x.CultureCode=="en");
            var body=new CanonicalLessonTranslationRecord(en.CultureCode,en.Title,
                en.Explanation,en.KeyConceptsAndRules,en.WorkedExamples,
                en.StepByStepSolutions,en.CommonMistakes,en.QuickSummary);
            var rich=RichLessonContentV2Registry.Find(item.code,"en") ??
                RichLessonContentV2RuntimeComposer.TryCompose(item.code,body);
            Assert.NotNull(rich);
            Assert.Contains(rich.Visuals,x=>x.Kind==item.kind);
        }
    }


    [Fact]
    public void GradeOneSameTapeTwoDifferentPaperclipSizesAreExactlyEquivalent()
    {
        const string family="usccss.b01.g1.measure.same_length_different_units";
        Assert.True(LessonPracticeContractRegistry.TryResolve(G1,out var contract));
        Assert.Contains(family,contract!.AllowedQuestionFamilies);
        var engine=new ExactSkillContractQuestionEngine();
        var modes=new HashSet<int>();
        var tapes=new HashSet<int>();
        for(var seed=733000;seed<733110;seed++)
        {
            var q=Assert.Single(engine.Generate(
                "b01-g1-u06-two-units",G1,[family],
                ExactSkillQuestionDifficulty.Standard,1,seed,[]));
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                family,q.Parameters,q.CorrectAnswer));
            var p=q.Parameters;
            var tape=p["tapeInches"];
            var ones=p["oneInchCount"];
            var twos=p["twoInchCount"];
            Assert.Equal(tape,ones);
            Assert.Equal(tape,twos*2);
            Assert.Equal(p["mode"] switch {
                0=>ones,1=>twos,_=>ones-twos
            },int.Parse(q.CorrectAnswer));
            var visual=Assert.Single(UsCcssB01SourceBoundMeasurementVisuals.Build(
                G1,[q]));
            Assert.Equal(RichLessonVisualKind.RatioTable,visual.Kind);
            Assert.Equal(2,visual.Rows.Count);
            Assert.Equal(ones.ToString(),visual.Rows[0].Cells[0]);
            Assert.Equal(twos.ToString(),visual.Rows[1].Cells[0]);
            Assert.Equal($"{tape} inches total",visual.Rows[0].Cells[2]);
            Assert.Equal(visual.Rows[0].Cells[2],visual.Rows[1].Cells[2]);
            Assert.Empty(UsCcssB01SourceBoundMeasurementVisuals.Build(
                G1,[q with {CorrectAnswer="999"}]));
            Assert.Empty(UsCcssB01SourceBoundMeasurementVisuals.Build(
                "PED:US-CCSS-MATH:G1:U06:L08",[q]));
            modes.Add(p["mode"]);
            tapes.Add(tape);
        }
        Assert.Equal([0,1,2],modes.Order().ToArray());
        Assert.True(tapes.Count>=3);
        var lesson=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .Where(d=>d.PackCode=="US-CCSS-MATH" && d.VersionCode=="CCSSM-2010")
            .SelectMany(d=>d.Lessons).Single(x=>x.LessonCode==G1);
        var en=lesson.Translations.Single(x=>x.CultureCode=="en");
        var body=new CanonicalLessonTranslationRecord(en.CultureCode,en.Title,
            en.Explanation,en.KeyConceptsAndRules,en.WorkedExamples,
            en.StepByStepSolutions,en.CommonMistakes,en.QuickSummary);
        var rich=RichLessonContentV2Registry.Find(G1,"en") ??
            RichLessonContentV2RuntimeComposer.TryCompose(G1,body);
        Assert.NotNull(rich);
        Assert.Contains(rich.Visuals,x=>x.Kind==RichLessonVisualKind.RatioTable);
    }

    [Fact]
    public void Grade2U03L07ReusesBOTHVerifiedEstimateAndStoryFamilies()
    {
        const string code="PED:US-CCSS-MATH:G2:U03:L07";
        var families=new[]{
            "usccss.b01.g2.measurement.estimate_cm",
            "usccss.b01.g2.length_compare.all_unknown_positions100"};
        Assert.True(LessonPracticeContractRegistry.TryResolve(code,out var contract));
        Assert.Equal(families,contract!.AllowedQuestionFamilies);
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var family in families)
            for(var seed=840120;seed<840152;seed++)
            {
                var q=Assert.Single(engine.Generate(
                    "b01-grade2-center-day",code,[family],
                    ExactSkillQuestionDifficulty.Standard,1,seed,[]));
                Assert.True(ExactSkillContractQuestionEngine.Verify(
                    family,q.Parameters,q.CorrectAnswer));
                var picture=Assert.Single(UsCcssB01SourceTaskVisuals.Build(code,[q]));
                Assert.Equal(family==families[0]
                    ?RichLessonVisualKind.Scale
                    :RichLessonVisualKind.TapeDiagram,picture.Kind);
                Assert.Empty(UsCcssB01SourceTaskVisuals.Build(
                    code,[q with {CorrectAnswer="9999"}]));
                Assert.Empty(UsCcssB01SourceTaskVisuals.Build(
                    "PED:US-CCSS-MATH:G2:U03:L08",[q]));
            }
        var lesson=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .Where(d=>d.PackCode=="US-CCSS-MATH"&&d.VersionCode=="CCSSM-2010")
            .SelectMany(d=>d.Lessons).Single(x=>x.LessonCode==code);
        var en=lesson.Translations.Single(x=>x.CultureCode=="en");
        var body=new CanonicalLessonTranslationRecord(en.CultureCode,en.Title,
            en.Explanation,en.KeyConceptsAndRules,en.WorkedExamples,
            en.StepByStepSolutions,en.CommonMistakes,en.QuickSummary);
        var rich=RichLessonContentV2Registry.Find(code,"en") ??
            RichLessonContentV2RuntimeComposer.TryCompose(code,body);
        Assert.NotNull(rich);
        Assert.Contains(rich.Visuals,v=>v.Kind==RichLessonVisualKind.Scale);
        Assert.Contains(rich.Visuals,v=>v.Kind==RichLessonVisualKind.TapeDiagram);
    }

    [Theory]
    [InlineData("PED:US-CCSS-MATH:G1:U06:L08",
        "usccss.b01.g1.measure.count_cubes_up_to110",100,110)]
    [InlineData("PED:US-CCSS-MATH:G1:U06:L09",
        "usccss.b01.g1.measure.write_cubes_to120",95,120)]
    public void GradeOneUnitSixLongLengthsCountActualCubeUnits(
        string code,string family,int lower,int upper)
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve(code,out var contract));
        Assert.Equal([family],contract!.AllowedQuestionFamilies);
        var engine=new ExactSkillContractQuestionEngine();
        var observed=new HashSet<int>();
        for(var seed=611200;seed<611308;seed++)
        {
            var q=Assert.Single(engine.Generate(
                "b01-g1-long-length-cubes",code,[family],
                ExactSkillQuestionDifficulty.Standard,1,seed,[]));
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                family,q.Parameters,q.CorrectAnswer));
            var p=q.Parameters;
            var cubes=p["number"];
            Assert.InRange(cubes,lower,upper);
            Assert.Equal(p["tens"]*10+p["ones"],cubes);
            Assert.Equal(cubes,int.Parse(q.CorrectAnswer));
            var plot=Assert.Single(UsCcssB01SourceTaskVisuals.Build(code,[q]));
            Assert.Equal(RichLessonVisualKind.PlaceValueChart,plot.Kind);
            Assert.Equal(p["tens"]+1,plot.Rows.Count);
            Assert.Equal(cubes,plot.Rows.Sum(row=>row.Cells.Count));
            for(var i=0;i<p["tens"];i++)
                Assert.Equal(10,plot.Rows[i].Cells.Count);
            Assert.Equal(p["ones"],plot.Rows[^1].Cells.Count);
            Assert.Empty(UsCcssB01SourceTaskVisuals.Build(
                code,[q with {CorrectAnswer="9999"}]));
            Assert.Empty(UsCcssB01SourceTaskVisuals.Build(
                "PED:US-CCSS-MATH:G1:U06:L10",[q]));
            observed.Add(cubes);
        }
        Assert.True(observed.Count>=6);
        var lesson=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .Where(d=>d.PackCode=="US-CCSS-MATH"&&d.VersionCode=="CCSSM-2010")
            .SelectMany(d=>d.Lessons).Single(x=>x.LessonCode==code);
        var en=lesson.Translations.Single(x=>x.CultureCode=="en");
        var body=new CanonicalLessonTranslationRecord(en.CultureCode,en.Title,
            en.Explanation,en.KeyConceptsAndRules,en.WorkedExamples,
            en.StepByStepSolutions,en.CommonMistakes,en.QuickSummary);
        var rich=RichLessonContentV2Registry.Find(code,"en") ??
            RichLessonContentV2RuntimeComposer.TryCompose(code,body);
        Assert.NotNull(rich);
        Assert.Contains(rich.Visuals,x=>x.Kind==RichLessonVisualKind.PlaceValueChart);
    }

    private const string G2ReptileCompare = "PED:US-CCSS-MATH:G2:U03:L06";

    [Fact]
    public void GradeTwoReptileLengthComparisonCoversAllThreeUnknownPositionsWithin100()
    {
        const string family="usccss.b01.g2.length_compare.all_unknown_positions100";
        Assert.True(LessonPracticeContractRegistry.TryResolve(
            G2ReptileCompare,out var contract));
        Assert.Equal([family],contract!.AllowedQuestionFamilies);
        var engine=new ExactSkillContractQuestionEngine();
        var modes=new HashSet<int>();
        for(var seed=902000;seed<902130;seed++)
        {
            var q=Assert.Single(engine.Generate(
                "b01-reptile-compare",G2ReptileCompare,[family],
                ExactSkillQuestionDifficulty.Standard,1,seed,[]));
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                family,q.Parameters,q.CorrectAnswer));
            var p=q.Parameters;
            var larger=p["longCount"];
            var smaller=p["shortCount"];
            var difference=p["difference"];
            var mode=p["unknownPosition"];
            Assert.InRange(larger,41,99);
            Assert.InRange(smaller,11,94);
            Assert.Equal(larger-smaller,difference);
            Assert.Equal(mode switch {0=>difference,1=>larger,_=>smaller},
                int.Parse(q.CorrectAnswer));
            var picture=Assert.Single(UsCcssB01SourceTaskVisuals.Build(
                G2ReptileCompare,[q]));
            Assert.Equal(RichLessonVisualKind.TapeDiagram,picture.Kind);
            Assert.Equal([
                larger.ToString(),smaller.ToString(),
                difference.ToString(),mode.ToString()
            ],picture.Items);
            Assert.Empty(UsCcssB01SourceTaskVisuals.Build(
                G2ReptileCompare,[q with {CorrectAnswer="999"}]));
            Assert.Empty(UsCcssB01SourceTaskVisuals.Build(
                "PED:US-CCSS-MATH:G2:U03:L05",[q]));
            modes.Add(mode);
        }
        Assert.Equal([0,1,2],modes.Order().ToArray());

        var lesson=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .Where(d=>d.PackCode=="US-CCSS-MATH"&&d.VersionCode=="CCSSM-2010")
            .SelectMany(d=>d.Lessons).Single(x=>x.LessonCode==G2ReptileCompare);
        var en=lesson.Translations.Single(x=>x.CultureCode=="en");
        var body=new CanonicalLessonTranslationRecord(en.CultureCode,en.Title,
            en.Explanation,en.KeyConceptsAndRules,en.WorkedExamples,
            en.StepByStepSolutions,en.CommonMistakes,en.QuickSummary);
        var rich=RichLessonContentV2Registry.Find(G2ReptileCompare,"en") ??
            RichLessonContentV2RuntimeComposer.TryCompose(G2ReptileCompare,body);
        Assert.NotNull(rich);
        Assert.Contains(rich.Visuals,x=>x.Kind==RichLessonVisualKind.TapeDiagram);
    }

    private const string G2Meter = "PED:US-CCSS-MATH:G2:U03:L05";

    [Fact]
    public void GradeTwoOriginalSevenMeterstickTapesHaveExactCmAndMeters()
    {
        const string family="usccss.b01.g2.measure.reptile_meterstick";
        Assert.True(LessonPracticeContractRegistry.TryResolve(G2Meter,out var contract));
        Assert.Equal([family],contract!.AllowedQuestionFamilies);
        var original=new[]{58,72,44,180,336,190,282};
        var engine=new ExactSkillContractQuestionEngine();
        var seenTapes=new HashSet<int>();
        var modes=new HashSet<int>();
        for(var seed=804001;seed<804156;seed++)
        {
            var q=Assert.Single(engine.Generate("b01-g2-u03-metersticks",
                G2Meter,[family],ExactSkillQuestionDifficulty.Standard,1,seed,[]));
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                family,q.Parameters,q.CorrectAnswer));
            var p=q.Parameters;
            var idx=p["tapeIndex"];
            Assert.InRange(idx,0,6);
            Assert.Equal(original[idx],p["lengthCm"]);
            Assert.Equal(original[idx]/100,p["fullMeters"]);
            Assert.Equal(original[idx]%100,p["remainingCm"]);
            Assert.Equal(p["mode"] switch {
                0=>p["fullMeters"],1=>p["remainingCm"],_=>p["lengthCm"]
            },int.Parse(q.CorrectAnswer));
            var drawing=Assert.Single(UsCcssB01SourceTaskVisuals.Build(
                G2Meter,[q]));
            Assert.Equal(RichLessonVisualKind.PlaceValueChart,drawing.Kind);
            Assert.Equal(p["fullMeters"]+(p["remainingCm"]>0?1:0),
                drawing.Rows.Count);
            for(var i=0;i<p["fullMeters"];i++)
            {
                Assert.Equal((i*100).ToString(),drawing.Rows[i].Cells[0]);
                Assert.Equal(((i+1)*100).ToString(),drawing.Rows[i].Cells[1]);
                Assert.Equal("100 cm",drawing.Rows[i].Cells[2]);
            }
            if(p["remainingCm"]>0)
            {
                var final=drawing.Rows[^1];
                Assert.Equal((p["fullMeters"]*100).ToString(),final.Cells[0]);
                Assert.Equal(p["lengthCm"].ToString(),final.Cells[1]);
                Assert.Equal($"{p["remainingCm"]} cm",final.Cells[2]);
            }
            Assert.Empty(UsCcssB01SourceTaskVisuals.Build(
                G2Meter,[q with {CorrectAnswer="9999"}]));
            Assert.Empty(UsCcssB01SourceTaskVisuals.Build(
                "PED:US-CCSS-MATH:G2:U03:L04",[q]));
            seenTapes.Add(idx);
            modes.Add(p["mode"]);
        }
        Assert.Equal(7,seenTapes.Count);
        Assert.Equal([0,1,2],modes.Order().ToArray());

        var lesson=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .Where(d=>d.PackCode=="US-CCSS-MATH"&&d.VersionCode=="CCSSM-2010")
            .SelectMany(d=>d.Lessons).Single(x=>x.LessonCode==G2Meter);
        var en=lesson.Translations.Single(x=>x.CultureCode=="en");
        var body=new CanonicalLessonTranslationRecord(en.CultureCode,en.Title,
            en.Explanation,en.KeyConceptsAndRules,en.WorkedExamples,
            en.StepByStepSolutions,en.CommonMistakes,en.QuickSummary);
        var rich=RichLessonContentV2Registry.Find(G2Meter,"en") ??
            RichLessonContentV2RuntimeComposer.TryCompose(G2Meter,body);
        Assert.NotNull(rich);
        Assert.Contains(rich.Visuals,x=>x.Kind==RichLessonVisualKind.PlaceValueChart);
        Assert.Contains(rich.Visuals,x=>x.Kind==RichLessonVisualKind.EquationSet);
    }

    private const string G2Estimate = "PED:US-CCSS-MATH:G2:U03:L04";

    [Fact]
    public void Grade2U03L04RulerIterationPreservesExactLongObjectEndpoints()
    {
        const string family="usccss.b01.g2.measurement.iterate_ruler_cm";
        var engine=new ExactSkillContractQuestionEngine();
        var sawRemainder=false;
        var sawExactMultiple=false;
        for(var seed=723000;seed<723128;seed++)
        {
            var q=Assert.Single(engine.Generate(
                "b01-g2-u03-long-ruler",G2Estimate,[family],
                ExactSkillQuestionDifficulty.Standard,1,seed,[]));
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                family,q.Parameters,q.CorrectAnswer));
            var p=q.Parameters;
            var total=p["total"];
            var full=p["fullSpans"];
            var remainder=p["remainder"];
            Assert.InRange(total,50,90);
            Assert.Equal(total,full*30+remainder);
            Assert.Equal(total,int.Parse(q.CorrectAnswer));
            var drawing=Assert.Single(UsCcssB01SourceTaskVisuals.Build(
                G2Estimate,[q]));
            Assert.Equal(RichLessonVisualKind.PlaceValueChart,drawing.Kind);
            Assert.Equal(full+(remainder>0?1:0),drawing.Rows.Count);
            for(var k=0;k<full;k++)
            {
                Assert.Equal((k*30).ToString(),drawing.Rows[k].Cells[0]);
                Assert.Equal(((k+1)*30).ToString(),drawing.Rows[k].Cells[1]);
                Assert.Equal("30 cm",drawing.Rows[k].Cells[2]);
            }
            if(remainder>0)
            {
                sawRemainder=true;
                var row=drawing.Rows[^1];
                Assert.Equal((full*30).ToString(),row.Cells[0]);
                Assert.Equal(total.ToString(),row.Cells[1]);
                Assert.Equal($"{remainder} cm",row.Cells[2]);
            }
            else sawExactMultiple=true;
            Assert.Empty(UsCcssB01SourceTaskVisuals.Build(
                G2Estimate,[q with {CorrectAnswer="999"}]));
            Assert.Empty(UsCcssB01SourceTaskVisuals.Build(
                "PED:US-CCSS-MATH:G2:U03:L05",[q]));
        }
        Assert.True(sawRemainder);
        Assert.True(sawExactMultiple);
    }

    [Fact]
    public void Grade2U03L04EstimationsMatchExactCentimeterRuler()
    {
        const string family="usccss.b01.g2.measurement.estimate_cm";
        Assert.True(LessonPracticeContractRegistry.TryResolve(G2Estimate,out var contract));
        Assert.Equal(["usccss.b01.g2.measurement.estimate_cm",
            "usccss.b01.g2.measurement.iterate_ruler_cm"],
            contract!.AllowedQuestionFamilies);
        var engine=new ExactSkillContractQuestionEngine();
        var over=0;
        var under=0;
        for(var seed=921000;seed<921096;seed++)
        {
            var q=Assert.Single(engine.Generate(
                "b01-g2-u03-estimate-measure",G2Estimate,[family],
                ExactSkillQuestionDifficulty.Standard,1,seed,[]));
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                family,q.Parameters,q.CorrectAnswer));
            var p=q.Parameters;
            var actual=p["actual"];
            var estimate=p["estimate"];
            Assert.InRange(actual,5,30);
            Assert.Equal(Math.Abs(actual-estimate),p["error"]);
            Assert.Equal(p["error"],int.Parse(q.CorrectAnswer));
            if(estimate>actual)over++;else under++;
            var diagram=Assert.Single(UsCcssB01SourceTaskVisuals.Build(
                G2Estimate,[q]));
            Assert.Equal(RichLessonVisualKind.Scale,diagram.Kind);
            var tickValues=diagram.Items.Select(int.Parse).ToArray();
            Assert.Equal(actual+1,tickValues.Length);
            Assert.Equal(actual,tickValues[^1]);
            for(var i=1;i<tickValues.Length;i++)
                Assert.Equal(1,tickValues[i]-tickValues[i-1]);
            Assert.Empty(UsCcssB01SourceTaskVisuals.Build(
                G2Estimate,[q with {CorrectAnswer="999"}]));
            Assert.Empty(UsCcssB01SourceTaskVisuals.Build(
                "PED:US-CCSS-MATH:G2:U03:L05",[q]));
        }
        Assert.True(over>0 && under>0);

        var lesson=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .Where(doc=>doc.PackCode=="US-CCSS-MATH" &&
                        doc.VersionCode=="CCSSM-2010")
            .SelectMany(doc=>doc.Lessons)
            .Single(x=>x.LessonCode==G2Estimate);
        var en=lesson.Translations.Single(x=>x.CultureCode=="en");
        var body=new CanonicalLessonTranslationRecord(en.CultureCode,en.Title,
            en.Explanation,en.KeyConceptsAndRules,en.WorkedExamples,
            en.StepByStepSolutions,en.CommonMistakes,en.QuickSummary);
        var rich=RichLessonContentV2Registry.Find(G2Estimate,"en") ??
            RichLessonContentV2RuntimeComposer.TryCompose(G2Estimate,body);
        Assert.NotNull(rich);
        Assert.Contains(rich.Visuals,x=>x.Kind==RichLessonVisualKind.Scale);
    }

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
        if(code==G1)
            Assert.Equal(["supporting.measurement.unit_iteration",
                "usccss.b01.g1.measure.same_length_different_units"],
                contract!.AllowedQuestionFamilies);
        else
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
        if(code==G1)
            Assert.Contains(rich.Visuals,x=>x.Kind==RichLessonVisualKind.RatioTable);
        else
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
