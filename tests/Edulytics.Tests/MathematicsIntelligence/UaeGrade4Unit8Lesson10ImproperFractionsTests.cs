using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeGrade4Unit8Lesson10ImproperFractionsTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L4:COMMON:08:10";
    [Fact]
    public void MixedAndImproperFamiliesGenerateVerify()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var c));
        Assert.Equal(3,c!.AllowedQuestionFamilies.Count);
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
    [InlineData(9,4,"2","1")]
    [InlineData(11,8,"1","3")]
    [InlineData(12,3,"4","0")]
    [InlineData(8,6,"1","2")]
    public void QuotientAndRemainderProduceCorrectMixedNumber(int numerator,int denominator,string whole,string remainder)
    {
        var p=new Dictionary<string,int>{{"improper",numerator},{"denominator",denominator}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u08.l10.improper_whole",p,whole));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u08.l10.improper_remainder",p,remainder));
        Assert.Equal(numerator,int.Parse(whole)*denominator+int.Parse(remainder));
    }
    [Fact]
    public void InvalidFractionsAreRejected()
    {
        var p=new Dictionary<string,int>{{"improper",3},{"denominator",5}};
        Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g4.u08.l10.improper_whole",p,"0"));
    }
    [Fact]
    public void ArabicRichLessonExplainsBothDirections()
    {
        var l=RichLessonContentV2Registry.Find(Lesson,"ar");
        Assert.NotNull(l);
        Assert.Equal("الأعداد الكسرية والكسور المعتلة",l!.Title);
        Assert.True(l.WorkedExamples.Count>=4);
        Assert.True(l.KeyConcepts.Count>=3);
        Assert.Equal(RichLessonVisualKind.EquationSet,Assert.Single(l.Visuals).Kind);
    }
}
