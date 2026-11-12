using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeGrade4Unit9Lesson05WorkingBackwardsTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L4:COMMON:09:05";
    [Fact]
    public void WorkingBackwardsFamiliesGenerateAndVerify()
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
    [InlineData(8,2,3,"3")]
    [InlineData(6,1,2,"3")]
    [InlineData(4,1,2,"1")]
    [InlineData(10,4,3,"3")]
    public void MissingPartCompletesWhole(int denominator,int used,int remaining,string expected)
    {
        var p=new Dictionary<string,int>{{"denominator",denominator},{"used",used},{"remaining",remaining}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u09.l05.missing_fraction_part",p,expected));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u09.l05.reverse_operation",p,expected));
        Assert.Equal(denominator,used+remaining+int.Parse(expected));
    }
    [Fact]
    public void ImpossibleAllocationIsRejected()
    {
        var p=new Dictionary<string,int>{{"denominator",8},{"used",5},{"remaining",4}};
        Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g4.u09.l05.missing_fraction_part",p,"-1"));
    }
    [Fact]
    public void ArabicRichContentContainsInverseLogic()
    {
        var l=RichLessonContentV2Registry.Find(Lesson,"ar");
        Assert.NotNull(l);
        Assert.Equal("استقصاء حل المسائل: الحل بترتيب عكسي",l!.Title);
        Assert.True(l.WorkedExamples.Count>=4);
        Assert.True(l.KeyConcepts.Count>=3);
        Assert.Equal(RichLessonVisualKind.EquationSet,Assert.Single(l.Visuals).Kind);
    }
}
