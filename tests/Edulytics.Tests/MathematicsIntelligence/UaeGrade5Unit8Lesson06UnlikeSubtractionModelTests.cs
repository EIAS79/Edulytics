using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// StageD tests written but not executed.
public sealed class UaeGrade5Unit8Lesson06UnlikeSubtractionModelTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L5:GENERAL:08:06";
    [Fact]
    public void ModelSubtractionFamiliesAreExact()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var c));
        Assert.Equal("fractions.subtract",c!.SkillId);
        Assert.Equal(3,c.AllowedQuestionFamilies.Count);
        foreach(var f in c.AllowedQuestionFamilies)
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
    }
    [Fact]
    public void ThreeQuartersMinusOneSixthIsSevenTwelfths()
    {
        const string f="uae.g5.u08.l06.model_subtract_difference";
        var p=new Dictionary<string,int>{{"n1",3},{"d1",4},{"n2",1},{"d2",6}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(f,p,"٧/١٢"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(f,p,"14/24"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(f,p,"2/2"));
    }
}