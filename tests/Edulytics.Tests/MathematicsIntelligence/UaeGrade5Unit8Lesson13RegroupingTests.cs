using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeGrade5Unit8Lesson13RegroupingTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L5:GENERAL:08:13";
    [Fact]
    public void BorrowingHasExactSkillAndVerifiedFractionFamily()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var c));
        Assert.Equal("fractions.subtract",c!.SkillId);
        Assert.Single(c.AllowedQuestionFamilies);
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(c.AllowedQuestionFamilies[0]));
    }
    [Fact]
    public void ValidBorrowedUnitRestoresExactDifference()
    {
        const string f="uae.g5.u08.l13.borrow_mixed_difference";
        var p=new Dictionary<string,int>{{"w1",4},{"n1",1},{"d1",4},
            {"w2",2},{"n2",5},{"d2",8}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(f,p,"١٣/٨"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(f,p,"26/16"));
        p["n1"]=3;
        Assert.False(ExactSkillContractQuestionEngine.Verify(f,p,"13/8"));
    }
}