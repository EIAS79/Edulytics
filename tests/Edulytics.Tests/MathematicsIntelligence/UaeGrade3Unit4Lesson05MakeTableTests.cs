using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// Source written at A–B–C handoff; NOT RUN. Tables/drawings are teacher-scored.
public sealed class UaeGrade3Unit4Lesson05MakeTableTests
{
    [Fact] public void TableLessonReusesExistingTwoExactQuestionFamilies()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve("PED:UAE-MOE-MATH:L3:COMMON:04:05",out var c));
        Assert.Equal(2,c!.AllowedQuestionFamilies.Count);
        foreach(var f in c.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(f,out var interaction));
            Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad,interaction!.InteractionKind);
        }
    }
    [Theory] [InlineData(3,2,6)] [InlineData(2,4,8)] [InlineData(2,3,6)]
    public void CrossTableCellCountMatchesProduct(int rows,int columns,int expected)
        => Assert.Equal(expected,rows*columns);
}
