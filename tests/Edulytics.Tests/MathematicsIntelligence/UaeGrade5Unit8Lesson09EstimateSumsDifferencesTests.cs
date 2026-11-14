using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeGrade5Unit8Lesson09EstimateSumsDifferencesTests
{
    private const string L="PED:UAE-MOE-MATH:L5:GENERAL:08:09";
    [Fact]
    public void EstimatesTwoMixedNumbersWithBenchmarks()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(L,out var c));
        Assert.Equal(2,c!.AllowedQuestionFamilies.Count);
        const string f="uae.g5.u08.l09.estimate_mixed_difference";
        var p=new Dictionary<string,int>{{"w1",6},{"n1",3},{"d1",4},
            {"w2",2},{"n2",1},{"d2",8},{"proposed",5}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(f,p,"٥"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(f,p,"4"));
    }
}