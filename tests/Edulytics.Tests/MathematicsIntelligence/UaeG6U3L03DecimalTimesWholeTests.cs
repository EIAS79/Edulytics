using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeG6U3L03DecimalTimesWholeTests
{
    private const string L="PED:UAE-MOE-MATH:L6:GENERAL:03:03:STUDENT-AR-128";
    private const string F="uae.g5.u05.l03.decimal_times_whole";
    [Fact] public void ArabicLessonAndExistingDecimalFamilyAreRegistered()
    {
        var rich=RichLessonContentV2Registry.Find(L,"ar");
        Assert.NotNull(rich);
        Assert.True(rich!.WorkedExamples.Count>=8);
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(L,out var c));
        Assert.NotNull(c);
        Assert.Contains(F,c!.AllowedQuestionFamilies);
    }
    [Theory]
    [InlineData(8300,4,"3.32")]
    [InlineData(180,2,"0.036")]
    [InlineData(36,19,"0.0684")]
    [InlineData(650,18,"1.17")]
    public void TenThousandthPrecisionVerifiesExactly(int fraction,int whole,string expected)
    {
        var p=new Dictionary<string,int>{{"g6DecimalFactorTenThousandths",fraction},{"g6WholeFactor",whole}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(F,p,expected));
        Assert.False(ExactSkillContractQuestionEngine.Verify(F,p,"-99"));
    }
    [Fact] public void SourceScopedGeneratedItemsVerifyIndependently()
    {
        var g=new ExactSkillContractQuestionEngine();
        foreach(var q in g.Generate(L,F,[F],ExactSkillQuestionDifficulty.Standard,3,20261114,[]))
            Assert.True(ExactSkillContractQuestionEngine.Verify(F,q.Parameters,q.CorrectAnswer));
    }
}
