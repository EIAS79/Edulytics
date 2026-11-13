using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeGrade5Unit9Lesson01FractionOfQuantityTests
{
    private const string L="PED:UAE-MOE-MATH:L5:GENERAL:09:01";
    [Fact]
    public void FractionOfQuantityReusesTwoVerifiedFamilies()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(L,out var c));
        Assert.Equal("fractions.of_quantity",c!.SkillId);
        Assert.Equal(2,c.AllowedQuestionFamilies.Count);
        foreach(var f in c.AllowedQuestionFamilies)
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
    }
}