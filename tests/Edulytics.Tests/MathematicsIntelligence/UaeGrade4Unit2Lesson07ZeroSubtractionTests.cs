using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeGrade4Unit2Lesson07ZeroSubtractionTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L4:COMMON:02:07";
    [Fact]
    public void ThreeFamiliesGenerateRegroupingAcrossZeroAndValidate()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var contract));
        Assert.Equal(3,contract!.AllowedQuestionFamilies.Count);
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var f in contract.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(f,out var ui));
            Assert.NotNull(ui);
            for(int seed=1;seed<=6;seed++)
            foreach(var q in engine.Generate(Lesson,f,[f],
                ExactSkillQuestionDifficulty.Standard,1,seed,[]))
            {
                Assert.True(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,q.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,"-1"));
                Assert.Equal(2,q.Parameters.Count);
                Assert.DoesNotContain("result",q.Parameters.Keys);
                var invalid=q.Parameters.ToDictionary(x=>x.Key,x=>x.Value);
                invalid["a"]=0;
                Assert.False(ExactSkillContractQuestionEngine.Verify(f,invalid,q.CorrectAnswer));
            }
        }
    }
    [Fact]
    public void ZeroBorrowFixturesMatchIndependentDifference()
    {
        var p=new Dictionary<string,int>{{"a",5100},{"b",3520}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u02.l07.subtract_through_zero_four",p,"١٬٥٨٠"));
        Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g4.u02.l07.subtract_through_zero_four",p,"1579"));
        p["a"]=30200;p["b"]=22977;
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u02.l07.subtract_through_zero_five",p,"7223"));
        p["a"]=600000;p["b"]=28005;
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u02.l07.subtract_through_zero_six",p,"571995"));
        p["a"]=600001;
        Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g4.u02.l07.subtract_through_zero_six",p,"571996"));
    }
    [Fact]
    public void ArabicStudentContentHasZeroBorrowChartAndTeacherReasoning()
    {
        var l=RichLessonContentV2Registry.Find(Lesson,"ar");
        Assert.NotNull(l);
        Assert.Equal("الطرح مع وجود الأصفار",l!.Title);
        Assert.True(l.WorkedExamples.Count>=3);
        Assert.Equal(RichLessonVisualKind.PlaceValueChart,Assert.Single(l.Visuals).Kind);
        Assert.Contains(l.CommonMistakes,x=>x.Mistake.Contains("صفر"));
    }
}
