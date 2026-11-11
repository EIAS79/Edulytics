using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeGrade4Unit10Lesson01DecimalPlaceValueTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L4:COMMON:10:01";
    [Fact]
    public void TenthsHundredthsFamiliesGenerateAndVerify()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var contract));
        Assert.Equal(3,contract!.AllowedQuestionFamilies.Count);
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var f in contract.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(f,out var ui));
            Assert.NotNull(ui);
            foreach(var q in engine.Generate(Lesson,f,[f],ExactSkillQuestionDifficulty.Standard,3,20261111,[]))
            {
                Assert.True(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,q.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,"-2"));
            }
        }
    }
    [Theory]
    [InlineData(0,4,2,"0.42","42","4")]
    [InlineData(2,9,1,"2.91","291","9")]
    [InlineData(1,0,0,"1.00","100","0")]
    [InlineData(0,6,0,"0.60","60","6")]
    public void ValuesMatchHundredSquareAndCurrency(int whole,int tenths,int hundredths,string decimalValue,string count,string digit)
    {
        var p=new Dictionary<string,int>{{"whole",whole},{"tenths",tenths},{"hundredths",hundredths}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u10.l01.place_value_decimal",p,decimalValue));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u10.l01.hundredths_count",p,count));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u10.l01.tenths_digit",p,digit));
    }
    [Fact]
    public void ArabicDigitsAndArabicDecimalMarkAreAccepted()
    {
        var p=new Dictionary<string,int>{{"whole",0},{"tenths",4},{"hundredths",2}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u10.l01.place_value_decimal",p,"٠٫٤٢"));
    }
    [Fact]
    public void MalformedPlaceValueParameterRejected()
    {
        var p=new Dictionary<string,int>{{"whole",0},{"tenths",4},{"hundredths",10}};
        Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g4.u10.l01.place_value_decimal",p,"0.50"));
    }
    [Fact]
    public void ArabicRichLessonHasHundredGridAndFilsExamples()
    {
        var l=RichLessonContentV2Registry.Find(Lesson,"ar");
        Assert.NotNull(l);
        Assert.Equal("نشاط عملي: القيمة المكانية في الأجزاء من عشرة والأجزاء من مئة",l!.Title);
        Assert.True(l.WorkedExamples.Count>=4);
        Assert.True(l.KeyConcepts.Count>=3);
        Assert.Equal(RichLessonVisualKind.EquationSet,Assert.Single(l.Visuals).Kind);
    }
}
