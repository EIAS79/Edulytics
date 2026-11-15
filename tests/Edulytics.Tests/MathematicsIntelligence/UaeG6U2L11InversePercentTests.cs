using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeG6U2L11InversePercentTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L6:GENERAL:02:11:STUDENT-AR-128";
    private const string Family="percentages.core.of_quantity";
    [Fact] public void ArabicWordProblemAndExistingFamilyAreBound()
    {
        var rich=RichLessonContentV2Registry.Find(Lesson,"ar");
        Assert.NotNull(rich);
        Assert.True(rich!.WorkedExamples.Count>=7);
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var c));
        Assert.NotNull(c);
        Assert.Contains(Family,c!.AllowedQuestionFamilies);
    }
    [Theory]
    [InlineData(25,10,"40")]
    [InlineData(75,90,"120")]
    [InlineData(30,15,"50")]
    [InlineData(75,225,"300")]
    [InlineData(5,15,"300")]
    [InlineData(20,4200,"21000")]
    public void InversePercentHasIndependentVerifier(int percent,int part,string expected)
    {
        var p=new Dictionary<string,int>{{"wholeUnknownPercent",percent},{"knownPart",part}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(Family,p,expected));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family,p,"-1"));
    }
    [Fact] public void GeneratorAndVerifierAgree()
    {
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var q in engine.Generate(Lesson,Family,[Family],ExactSkillQuestionDifficulty.Standard,3,20261114,[]))
            Assert.True(ExactSkillContractQuestionEngine.Verify(Family,q.Parameters,q.CorrectAnswer));
    }
}
