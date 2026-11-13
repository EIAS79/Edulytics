using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

// Owner-selected Grade 5 Arabic student, printed pp81–86. NOT_RUN in A–B–C.
public sealed class UaeGrade5Unit2Lesson01PrimeFactorTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L5:GENERAL:02:01";

    [Fact]
    public void ExactPrimeFactorizationAndNumericFamiliesAreRegistered()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var contract));
        Assert.Equal("supporting.number.factors_primes",contract!.SkillId);
        Assert.Equal(3,contract.AllowedQuestionFamilies.Count);
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var family in contract.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var ui));
            Assert.NotNull(ui);
            foreach(var item in engine.Generate(Lesson,family,[family],ExactSkillQuestionDifficulty.Standard,2,20261113,[]))
            {
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,item.Parameters,item.CorrectAnswer));
                Assert.DoesNotContain("result",item.Parameters.Keys);
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,item.Parameters,"invalid"));
            }
        }
    }

    [Fact]
    public void PrimeProductRejectsCompositeFactorsAndChecksUnorderedPrimeFactors()
    {
        const string f="uae.g5.u02.l01.prime_factorize";
        var p=new Dictionary<string,int>{{"n",36}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(f,p,"٢×٢×٣×٣"));
        Assert.True(ExactSkillContractQuestionEngine.Verify(f,p,"3*2*3*2"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(f,p,"4×9"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(f,p,"2×3×3"));
    }

    [Fact]
    public void MissingPrimeTreeAndFactorCountAreExact()
    {
        var p=new Dictionary<string,int>{{"n",104},{"a",2},{"b",2},{"c",13}};
        const string missing="uae.g5.u02.l01.complete_factor_tree";
        // This fixture deliberately tests the source's four-prime factor tree.
        Assert.True(ExactSkillContractQuestionEngine.Verify(missing,p,"2"));
        p["n"]=2*2*3*7;
        p["c"]=7;
        Assert.True(ExactSkillContractQuestionEngine.Verify(missing,p,"٣"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(missing,p,"4"));
        const string count="uae.g5.u02.l01.prime_factor_count";
        Assert.True(ExactSkillContractQuestionEngine.Verify(count,
            new Dictionary<string,int>{{"n",72}},"٥"));
    }
}
