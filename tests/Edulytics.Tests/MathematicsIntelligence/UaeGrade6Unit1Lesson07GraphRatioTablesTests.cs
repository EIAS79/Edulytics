using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;

// Grade6 General Arabic Student Book PDF88-95. Written in A-B-C, not executed.
public sealed class UaeGrade6Unit1Lesson07GraphRatioTablesTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L6:GENERAL:01:07:STUDENT-AR-128";
    [Fact] public void OriginalArabicLessonHasRealCartesianPlotsWithExactPoints()
    {
        var l=RichLessonContentV2Registry.Find(Lesson,"ar");
        Assert.NotNull(l);
        Assert.Equal("تمثيل جداول النسب بيانيًا",l!.Title);
        Assert.True(l.WorkedExamples.Count>=5);
        var plots=l.Visuals.Where(v=>v.Kind==RichLessonVisualKind.CartesianPlot).ToArray();
        Assert.True(plots.Length>=3);
        Assert.Contains(plots[0].Rows,r=>r.Cells.Count==2 && r.Cells[0]=="2" && r.Cells[1]=="6");
        Assert.All(plots.SelectMany(x=>x.Rows),r=>Assert.Equal(2,r.Cells.Count));
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var contract));
        Assert.NotNull(contract);
        Assert.Equal(2,contract!.AllowedQuestionFamilies.Count);
    }
    [Fact] public void ExistingNumericRatioVerifierRemainsAvailable()
    {
        const string family="ratio.unit_rate.equivalent_ratio";
        var p=new Dictionary<string,int>{{"baseFirst",12},{"baseSecond",3},{"targetSecond",4}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,"16"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,"18"));
        var eng=new ExactSkillContractQuestionEngine();
        foreach(var q in eng.Generate(Lesson,Lesson,[family],ExactSkillQuestionDifficulty.Standard,3,20261114,[]))
        {
            Assert.Contains("نسب",q.Prompt);
            Assert.True(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,q.CorrectAnswer));
        }
    }
}
