using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeGrade4Unit10Lesson07DecimalAdditionTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L4:COMMON:10:07";
    [Fact]
    public void DecimalAdditionFamiliesGenerateAndVerify()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var c));
        Assert.Equal(2,c!.AllowedQuestionFamilies.Count);
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var f in c.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(f,out var ui));
            Assert.NotNull(ui);
            foreach(var q in engine.Generate(Lesson,f,[f],ExactSkillQuestionDifficulty.Standard,3,20261111,[]))
            {
                Assert.True(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,q.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,"-1"));
            }
        }
    }
    [Theory]
    [InlineData(3,5,"35","0.35")]
    [InlineData(4,7,"47","0.47")]
    [InlineData(0,9,"9","0.09")]
    [InlineData(9,9,"99","0.99")]
    public void SumConvertsTenthsToHundredths(int tenths,int hundredths,string integerSum,string decimalSum)
    {
        var p=new Dictionary<string,int>{{"tenths",tenths},{"hundredths",hundredths},{"otherDays",4}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u10.l07.sum_hundredths",p,integerSum));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u10.l07.sum_decimal",p,decimalSum));
    }
    [Fact]
    public void ArabicDecimalsAndInvalidParametersAreHandled()
    {
        var p=new Dictionary<string,int>{{"tenths",3},{"hundredths",5},{"otherDays",4}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u10.l07.sum_decimal",p,"٠٫٣٥"));
        p["hundredths"]=10;
        Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g4.u10.l07.sum_decimal",p,"0.45"));
    }
    [Fact]
    public void RichArabicLessonExplainsHundredthsGrid()
    {
        var l=RichLessonContentV2Registry.Find(Lesson,"ar");
        Assert.NotNull(l);
        Assert.Equal("استخدام النماذج والقيمة المكانية في الجمع",l!.Title);
        Assert.True(l.WorkedExamples.Count>=4);
        Assert.True(l.KeyConcepts.Count>=3);
        Assert.Equal(RichLessonVisualKind.EquationSet,Assert.Single(l.Visuals).Kind);
    }
}
