using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeGrade4Unit7Lesson03SequencesTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L4:COMMON:07:03";
    [Fact]
    public void ExtendedSequencesGenerateAndVerify()
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
                Assert.False(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,"-1"));
            }
        }
    }
    [Fact]
    public void MultiplicationAndDivisionSequencesAreExact()
    {
        var p=new Dictionary<string,int>{{"first",3},{"factor",3},{"index",4}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u07.l03.multiply_sequence",p,"٢٤٣"));
        p["first"]=4;p["factor"]=2;p["index"]=4;
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u07.l03.divide_sequence",p,"٤"));
    }
    [Fact]
    public void ArabicSequenceRichContentIsComplete()
    {
        var l=RichLessonContentV2Registry.Find(Lesson,"ar");
        Assert.NotNull(l);
        Assert.Equal("المتتاليات",l!.Title);
        Assert.True(l.WorkedExamples.Count>=3);
        Assert.Equal(RichLessonVisualKind.EquationSet,Assert.Single(l.Visuals).Kind);
    }
}
