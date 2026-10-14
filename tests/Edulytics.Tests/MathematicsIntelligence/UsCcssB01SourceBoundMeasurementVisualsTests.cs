using Edulytics.Core.Curriculum;
using Edulytics.Core.Lessons;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Data.Seeding;
using Edulytics.Services.LessonContent;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class UsCcssB01SourceBoundMeasurementVisualsTests
{
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
}
