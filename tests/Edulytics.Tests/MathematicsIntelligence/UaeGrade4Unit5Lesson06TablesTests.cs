using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeGrade4Unit5Lesson06TablesTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L4:COMMON:05:06";
    [Fact]
    public void TableStrategyFamiliesGenerateAndVerify()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var contract));
        Assert.Equal(2,contract!.AllowedQuestionFamilies.Count);
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
    [Fact]
    public void StudentBookTableExamplesCalculateCorrectly()
    {
        var p=new Dictionary<string,int>{{"first",16},{"second",3},{"count",5}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u05.l06.table_total",p,"٨٠"));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u05.l06.table_number_of_days",p,"٥"));
        p["first"]=20;p["second"]=3;p["count"]=10;
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u05.l06.table_total",p,"٢٠٠"));
    }
    [Fact]
    public void RichArabicSupportsDrawingTables()
    {
        var l=RichLessonContentV2Registry.Find(Lesson,"ar");
        Assert.NotNull(l);
        Assert.Contains("رسم جدول",l!.Title);
        Assert.True(l.WorkedExamples.Count>=3);
        Assert.Equal(RichLessonVisualKind.EquationSet,Assert.Single(l.Visuals).Kind);
    }
}
