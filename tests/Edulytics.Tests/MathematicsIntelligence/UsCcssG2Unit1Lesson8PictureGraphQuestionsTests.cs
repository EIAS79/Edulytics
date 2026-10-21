using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Authoring only: Stage D must execute separately.</summary>
public sealed class UsCcssG2Unit1Lesson8PictureGraphQuestionsTests
{
    private static readonly string[] Families=
    [
        "usccss.g2.u01.l08.read_pictograph_category",
        "usccss.g2.u01.l08.combine_or_compare_categories",
        "usccss.g2.u01.l08.graph_question_answerable"
    ];

    [Fact]
    public void Lesson8HasThreeAndOnlyThreeSourceGoalGradeableFamilies()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(
            "PED:US-CCSS-MATH:G2:U01:L08",out var contract));
        Assert.NotNull(contract);
        Assert.Equal(Families,contract!.AllowedQuestionFamilies);
        foreach(var family in Families)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var interaction));
            Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad,interaction!.InteractionKind);
        }
    }

    [Fact]
    public void UnsupportedExternalQuestionCannotBeAnsweredFromPictureGraph()
    {
        var parms=new Dictionary<string,int>(StringComparer.Ordinal)
        {["carrots"]=8,["peas"]=2,["corn"]=3,["question"]=2};
        Assert.True(ExactSkillContractQuestionEngine.Verify(Families[2],parms,"0"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Families[2],parms,"1"));
        parms["question"]=1;
        Assert.True(ExactSkillContractQuestionEngine.Verify(Families[2],parms,"1"));
    }

    [Fact]
    public void NumericFamiliesUseActualOneSymbolPerPersonAndFailClosed()
    {
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var family in Families)
        foreach(var seed in new[]{20261021,20261022})
        {
            var questions=engine.Generate("im2021-g2-u01-l08",family,[family],
                ExactSkillQuestionDifficulty.Standard,6,seed,[]);
            Assert.Equal(6,questions.Count);
            Assert.Equal(6,questions.Select(x=>x.ExposureFingerprint)
                .Distinct(StringComparer.Ordinal).Count());
            foreach(var q in questions)
            {
                Assert.True(ExactSkillContractQuestionEngine.Verify(
                    family,q.Parameters,q.CorrectAnswer));
                var correct=int.Parse(q.CorrectAnswer,
                    System.Globalization.CultureInfo.InvariantCulture);
                Assert.False(ExactSkillContractQuestionEngine.Verify(
                    family,q.Parameters,(correct+20).ToString()));
                Assert.False(ExactSkillContractQuestionEngine.Verify(
                    family,q.Parameters,"bad"));
                var bad=new Dictionary<string,int>(q.Parameters,StringComparer.Ordinal);
                bad["peas"]=-1;
                Assert.False(ExactSkillContractQuestionEngine.Verify(
                    family,bad,q.CorrectAnswer));
            }
        }
    }
}
