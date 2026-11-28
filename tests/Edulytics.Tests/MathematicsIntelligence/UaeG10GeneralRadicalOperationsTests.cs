using System.Globalization;
using Edulytics.Core.Enums;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Original UAE Grade10 General U02L06 radical operations, written
/// during A–B–C; DO NOT EXECUTE before Stage D.</summary>
public sealed class UaeG10GeneralRadicalOperationsTests
{
    private const string Code="PED:UAE-MOE-MATH:L10:GENERAL:02:06:OPERATIONS-WITH-RADICAL-EXPRESSIONS";
    private const string Product="uae.g10.general.u02.l06.same_root_product_integer";
    private const string Conjugate="uae.g10.general.u02.l06.radical_conjugate_integer";
    [Theory]
    [InlineData(Product)]
    [InlineData(Conjugate)]
    public void NewExactFamilyShowsIntegerKeypad(string family)
    {
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
        Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var ui));
        Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad,ui!.InteractionKind);
    }
    [Theory]
    [InlineData(Product)]
    [InlineData(Conjugate)]
    public void SourceScopeExactResultsAreIndependentlyVerified(string family)
    {
        var engine=new ExactSkillContractQuestionEngine();
        foreach(int seed in new[]{20261125,20261126,20261127})
        foreach(var difficulty in new[]{ExactSkillQuestionDifficulty.Standard,ExactSkillQuestionDifficulty.Challenge})
        {
            var tasks=engine.Generate(Code,Code,[family],difficulty,4,seed,[]);
            Assert.Equal(4,tasks.Count);
            foreach(var q in tasks)
            {
                var p=q.Parameters;
                Assert.Equal(3,p.Count);
                int a=p["a"],b=p["b"],r=p["radicand"];
                Assert.InRange(a,2,7);Assert.InRange(b,2,7);
                Assert.Contains(r,new[]{2,3,5,6,7,10,11});
                int expected=family==Product?a*b*r:a*a*r-b*b;
                Assert.Equal(expected,int.Parse(q.CorrectAnswer,CultureInfo.InvariantCulture));
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,q.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,(expected+1).ToString(CultureInfo.InvariantCulture)));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,"خطأ"));
                var arabic=string.Concat(q.CorrectAnswer.Select(x=>x is >= '0' and <= '9'?(char)('٠'+x-'0'):x));
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,arabic));
                var forged=p.ToDictionary(x=>x.Key,x=>x.Value,StringComparer.Ordinal);
                forged["radicand"]=9;
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,forged,q.CorrectAnswer));
                Assert.Equal(AssessmentItemType.Numeric,q.ItemType);
            }
        }
    }
    [Fact]
    public void SourceGoldenRadicalOperationsHaveCorrectRealDomainValues()
    {
        Assert.InRange(Math.Abs(Math.Sqrt(18)+Math.Sqrt(50)-8*Math.Sqrt(2)),0,1e-9);
        Assert.InRange(Math.Abs(2*Math.Sqrt(3)*4*Math.Sqrt(3)-24),0,1e-9);
        Assert.InRange(Math.Abs((Math.Sqrt(5)+2)*(Math.Sqrt(5)-2)-1),0,1e-9);
        Assert.InRange(Math.Abs(Math.Sqrt(18)/Math.Sqrt(2)-3),0,1e-9);
        Assert.True(double.IsNaN(Math.Sqrt(-1)));
    }
}
