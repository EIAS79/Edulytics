using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.LessonContent;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class UsCcssB01TenthsHundredthsSourceTests
{
    private const string Within = "usccss.b01.fraction.tenths_hundredths_within_one";
    private const string Over = "usccss.b01.fraction.tenths_hundredths_over_one";
    private static readonly (string Code,string Family,bool GreaterThanOne)[] Cases =
    [
        ("PED:US-CCSS-MATH:G4:U03:L16",Within,false),
        ("PED:US-CCSS-MATH:G4:U03:L17",Over,true)
    ];

    [Fact]
    public void OfficialLessonPurposeAndExactPracticeRoutingAreDifferent()
    {
        foreach(var (code,family,_) in Cases)
        {
            Assert.True(LessonPracticeCapabilityResolver.TryResolve(code,out var contract));
            Assert.NotNull(contract);
            Assert.Equal(new[] {family},contract!.AllowedQuestionFamilies);
            Assert.StartsWith("IM2021_",contract.SourceType);
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
            Assert.DoesNotContain(contract.AllowedQuestionFamilies,
                x=>x.StartsWith("supporting.decimals.place_value",StringComparison.Ordinal));
        }
    }

    [Fact]
    public void IndependentHundredthsArithmeticAndOneBoundaryAcrossDeterministicSeeds()
    {
        var engine = new ExactSkillContractQuestionEngine();
        var generated=0;
        foreach(var (code,family,greaterThanOne) in Cases)
        {
            foreach(var difficulty in new[]{
                ExactSkillQuestionDifficulty.Standard,
                ExactSkillQuestionDifficulty.Stretch,
                ExactSkillQuestionDifficulty.Challenge
            })
            for(var seed=43501;seed<43531;seed++)
            {
                var q=Assert.Single(engine.Generate(
                    "b01-independent-tenths-hundredths-oracle",code,[family],
                    difficulty,1,seed,[]));
                var a=q.Parameters["tenths"];
                var b=q.Parameters["hundredths"];
                var numerator=10*a+b;
                Assert.Equal(numerator,q.Parameters["totalHundredths"]);
                Assert.Equal(numerator+"/100",q.CorrectAnswer);
                Assert.Equal(greaterThanOne,numerator>100);
                Assert.True(ExactSkillContractQuestionEngine.Verify(
                    family,q.Parameters,q.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(
                    family,q.Parameters,(numerator+1)+"/100"));
                var tampered=q.Parameters.ToDictionary(x=>x.Key,x=>x.Value);
                tampered["totalHundredths"]++;
                Assert.False(ExactSkillContractQuestionEngine.Verify(
                    family,tampered,q.CorrectAnswer));
                Assert.Contains(a+"/10",q.Prompt);
                Assert.Contains(b+"/100",q.Prompt);
                generated++;
            }
        }
        Assert.Equal(180,generated);
    }

    [Fact]
    public void StudentHundredthsBarsDoNotExceedOneWholePerVisualRow()
    {
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var (code,family,greaterThanOne) in Cases)
        {
            var q=Assert.Single(engine.Generate(
                "b01-source-hundredths-shaded-bar",code,[family],
                ExactSkillQuestionDifficulty.Challenge,1,74711,[]));
            var visual=Assert.Single(UsCcssB01SourceTaskVisuals.Build(code,[q]));
            Assert.Equal(RichLessonVisualKind.FractionBars,visual.Kind);
            Assert.Equal(greaterThanOne?4:3,visual.Rows.Count);
            Assert.Equal(q.Parameters["tenths"]*10,visual.Rows[0].Numerator);
            Assert.Equal(q.Parameters["hundredths"],visual.Rows[1].Numerator);
            Assert.All(visual.Rows,row=>{
                Assert.Equal(100,row.Denominator);
                Assert.InRange(row.Numerator,0,100);
            });
            if(greaterThanOne)
                Assert.Equal(q.Parameters["totalHundredths"],
                    visual.Rows[2].Numerator+visual.Rows[3].Numerator);
            else
                Assert.Equal(q.Parameters["totalHundredths"],
                    visual.Rows[2].Numerator);
        }
    }
}
