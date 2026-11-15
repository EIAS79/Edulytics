using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeG6U6L01ExpressionStructureTests
{
    const string L="PED:UAE-MOE-MATH:L6:GENERAL:06:01:STUDENT-AR-128",
        Outer="uae.g6.u06.l01.classify_outer_operation",
        Groups="uae.g6.u06.l01.count_repeated_groups",
        Sum="uae.g6.u06.l01.identify_sum_factor";
    [Fact] public void OriginalArabicRichLabHasBarModelsAndScopedContracts()
    {
        var rich=RichLessonContentV2Registry.Find(L,"ar");
        Assert.NotNull(rich);
        Assert.Equal(10,rich!.WorkedExamples.Count);
        Assert.All(rich.WorkedExamples,e=>Assert.True(e.Steps.Count>=3));
        Assert.Equal(2,rich.Visuals.Count(v=>v.Kind==RichLessonVisualKind.ExpressionStripBars));
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(L,out var contract));
        Assert.Equal(3,contract!.AllowedQuestionFamilies.Count);
        foreach(var family in contract.AllowedQuestionFamilies)
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
    }
    [Theory]
    [InlineData(0,"مجموع")]
    [InlineData(1,"حاصل ضرب")]
    [InlineData(2,"حاصل قسمة")]
    public void OuterOperationIgnoresNestedParentheses(int mode,string answer)
    {
        var p=new Dictionary<string,int>{{"mode",mode},{"a",3},{"b",4},{"n",2}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(Outer,p,answer));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Outer,p,"مجموع عوامل"));
    }
    [Theory]
    [InlineData(2,"٢")]
    [InlineData(3,"3")]
    [InlineData(4,"٤")]
    public void EqualGroupsAreCountedAsGroupsNotTerms(int times,string answer)
    {
        var p=new Dictionary<string,int>{{"a",6},{"b",4},{"n",times}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(Groups,p,answer));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Groups,p,"8"));
    }
    [Fact] public void SumFactorAcceptsEitherTermOrderButNotProduct()
    {
        var p=new Dictionary<string,int>{{"a",6},{"b",4},{"n",2}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(Sum,p,"(٦ + ٤)"));
        Assert.True(ExactSkillContractQuestionEngine.Verify(Sum,p,"4+6"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Sum,p,"6*4"));
        p["n"]=0;
        Assert.False(ExactSkillContractQuestionEngine.Verify(Sum,p,"6+4"));
    }
}
