using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeG6U3L07MultiDigitDivisionTests
{
    const string L="PED:UAE-MOE-MATH:L6:GENERAL:03:07:STUDENT-AR-128";
    [Fact] public void ArabicLessonAndOldDivisionKernelAreAvailable()
    {
        var content=RichLessonContentV2Registry.Find(L,"ar");
        Assert.NotNull(content);
        Assert.True(content!.WorkedExamples.Count>=8);
        Assert.Contains(content.Visuals,v=>v.Kind==RichLessonVisualKind.CalculationChain);
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(L,out var contract));
        Assert.Equal(4,contract!.AllowedQuestionFamilies.Count);
        foreach(var f in contract.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(f,out _));
        }
    }
    [Theory]
    [InlineData(351,9,39,0)]
    [InlineData(878,31,28,10)]
    [InlineData(6493,75,86,43)]
    [InlineData(1460,365,4,0)]
    [InlineData(54912,44,1248,0)]
    [InlineData(9440,80,118,0)]
    [InlineData(15480,36,430,0)]
    [InlineData(2160,36,60,0)]
    [InlineData(15050,86,175,0)]
    [InlineData(16625,475,35,0)]
    [InlineData(736,8,92,0)]
    [InlineData(318,16,19,14)]
    [InlineData(7814,19,411,5)]
    [InlineData(3511,37,94,33)]
    public void SourceDivisionProofUsesQuotientAndStrictlySmallerRemainder(int n,int d,int quotient,int remainder)
    {
        Assert.Equal(n/d,quotient);
        Assert.Equal(n%d,remainder);
        Assert.Equal(n,d*quotient+remainder);
        Assert.InRange(remainder,0,d-1);
    }
    [Fact] public void ExactExistingLargeAndTwoDigitFamiliesStillVerify()
    {
        var large=new Dictionary<string,int>{{"dividend",54912},{"divisor",44}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u04.l05.large_quotient",large,"1248"));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u04.l05.large_remainder",large,"0"));
        var small=new Dictionary<string,int>{{"dividend",878},{"divisor",31}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u04.l03.two_digit_long_quotient",small,"28"));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u04.l03.two_digit_long_remainder",small,"10"));
        Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g5.u04.l03.two_digit_long_remainder",small,"31"));
    }
}
