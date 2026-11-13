using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

// Grade 5 local Arabic student 01.pdf, printed pp61–66. NOT RUN at Stage A–B–C.
public sealed class UaeGrade5Unit1Lesson09FourStepTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L5:GENERAL:01:09";

    [Fact]
    public void G5ReusesFourStepSkillAndRoutesReviewedExactFamilies()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var contract));
        Assert.Equal("problem_solving.four_steps_with_whole_numbers",contract!.SkillId);
        Assert.Equal(4,contract.AllowedQuestionFamilies.Count);
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var family in contract.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var ui));
            Assert.NotNull(ui);
            foreach(var item in engine.Generate(Lesson,family,[family],
                ExactSkillQuestionDifficulty.Standard,2,20261113,[]))
            {
                Assert.DoesNotContain("result",item.Parameters.Keys);
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,item.Parameters,item.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,item.Parameters,"-999"));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,item.Parameters,"1.5"));
                string arabic=new(item.CorrectAnswer.Select(c=>c is >= '0' and <= '9'
                    ? (char)('٠'+c-'0'):c).ToArray());
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,item.Parameters,arabic));
            }
        }
    }
    [Fact]
    public void FourStepIndependentFixturesCheckMissingPartsAndBudgetAndUndo()
    {
        var missing=new Dictionary<string,int>{{"a",5},{"b",8},{"c",13},{"d",34}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u01.l09.find_missing_distance",missing,"8"));
        Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g5.u01.l09.find_missing_distance",missing,"26"));
        var original=new Dictionary<string,int>{{"a",86},{"b",33},{"c",99},{"d",15}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u01.l09.find_starting_total",original,"233"));
        var budget=new Dictionary<string,int>{{"a",61},{"b",20},{"c",2},{"d",7}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u01.l09.budget_purchase_count",budget,"3"));
        budget["d"]=0;
        Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g5.u01.l09.budget_purchase_count",budget,"3"));
        var undo=new Dictionary<string,int>{{"a",3},{"b",6},{"c",7},{"d",8}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u01.l09.undo_four_steps",undo,"27"));
    }
}
