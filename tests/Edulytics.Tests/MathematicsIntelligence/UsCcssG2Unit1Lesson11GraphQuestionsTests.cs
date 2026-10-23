using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade2 L11 Stage C authored, tests intentionally not run.</summary>
public sealed class UsCcssG2Unit1Lesson11GraphQuestionsTests
{
    private static readonly string[] Families=[
        "usccss.g2.u01.l11.graph_question_count",
        "usccss.g2.u01.l11.graph_question_combine",
        "usccss.g2.u01.l11.graph_question_compare"
    ];

    [Fact]
    public void AllThreeNumericalSubskillsAreRestrictedToThisSourceLesson()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(
            "PED:US-CCSS-MATH:G2:U01:L11",out var contract));
        Assert.NotNull(contract);
        Assert.Equal(Families,contract!.AllowedQuestionFamilies);
        foreach(var family in Families)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var ui));
            Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad,ui!.InteractionKind);
        }
    }
    [Fact]
    public void WrongGraphAndForeignOrRepeatedCategoriesFailClosed()
    {
        var count=new Dictionary<string,int>(StringComparer.Ordinal)
            {["graph"]=0,["first"]=0};
        Assert.True(ExactSkillContractQuestionEngine.Verify(Families[0],count,"6"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Families[0],count,"4"));
        var combine=new Dictionary<string,int>(StringComparer.Ordinal)
            {["graph"]=1,["first"]=0,["second"]=3};
        Assert.True(ExactSkillContractQuestionEngine.Verify(Families[1],combine,"11"));
        Assert.True(ExactSkillContractQuestionEngine.Verify(Families[2],combine,"3"));
        combine["second"]=0;
        Assert.False(ExactSkillContractQuestionEngine.Verify(Families[1],combine,"8"));
        combine["second"]=9;
        Assert.False(ExactSkillContractQuestionEngine.Verify(Families[2],combine,"3"));
    }
    [Fact]
    public void GeneratedNumericalAnswersRecomputeFromPriorL10SourceGraphs()
    {
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var family in Families)
        foreach(var seed in new[]{20261021,20261022})
        {
            var items=engine.Generate("im2021-g2-u01-l11",family,[family],
                ExactSkillQuestionDifficulty.Standard,3,seed,[]);
            Assert.Equal(3,items.Count);
            foreach(var item in items)
            {
                Assert.True(ExactSkillContractQuestionEngine.Verify(
                    family,item.Parameters,item.CorrectAnswer));
                var bad=new Dictionary<string,int>(item.Parameters,StringComparer.Ordinal)
                    {["graph"]=9};
                Assert.False(ExactSkillContractQuestionEngine.Verify(
                    family,bad,item.CorrectAnswer));
            }
        }
    }
}
