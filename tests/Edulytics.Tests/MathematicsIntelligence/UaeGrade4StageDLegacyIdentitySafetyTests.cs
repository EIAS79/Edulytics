using System.Reflection;
using Edulytics.Core.Entities;
using Edulytics.Data.Seeding;
namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class UaeGrade4StageDLegacyIdentitySafetyTests
{
    private const string Legacy = "PED:UAE-MOE-MATH:L4:COMMON:01:01";

    private static MethodInfo Guard(string name) =>
        typeof(MathematicsPedagogicalLessonSeeder)
            .GetMethod(name, BindingFlags.NonPublic | BindingFlags.Static)
        ?? throw new InvalidOperationException("Required Grade 4 safety guard missing.");

    private static CurriculumPedagogicalLesson Create(
        string code, string title, Guid id, Guid version) =>
        new() { Id=id, FrameworkVersionId=version, Code=code, Title=title,
          UnitKey="U01", UnitTitle="U01", NativeLevel="Grade 4",
          LogicalLevelFrom=4, LogicalLevelTo=4, SortOrder=1 };

    [Fact]
    public void OldEnglishGrade4LessonCannotBeOverwrittenWithDifferentArabicTitle()
    {
        var id=Guid.NewGuid(); var version=Guid.NewGuid();
        var old=Create(Legacy,"Math Is Mine",id,version);
        var incoming=Create(Legacy,"القيمة المكانية",id,version);
        var exception=Assert.Throws<TargetInvocationException>(
            () => Guard("EnsureNoUnsafeGrade4IdentityMutation")
                .Invoke(null, [old,incoming]));
        Assert.IsType<InvalidOperationException>(exception.InnerException);
    }

    [Fact]
    public void LegacyGrade4CannotBeSilentlyRenumberedOrRetired()
    {
        var id=Guid.NewGuid(); var version=Guid.NewGuid();
        var old=Create(Legacy,"Math Is Mine",id,version);
        var different=Create("PED:UAE-MOE-MATH:L4:COMMON:02:01","Math Is Mine",id,version);
        var exception=Assert.Throws<TargetInvocationException>(
            () => Guard("EnsureNoUnsafeGrade4IdentityMutation")
                .Invoke(null, [old,different]));
        Assert.IsType<InvalidOperationException>(exception.InnerException);
        var retiring=Assert.Throws<TargetInvocationException>(
            () => Guard("EnsureNoUnsafeGrade4Retirement").Invoke(null,[old]));
        Assert.IsType<InvalidOperationException>(retiring.InnerException);
    }

    [Fact]
    public void UnchangedGrade4AcademicIdentityIsAccepted()
    {
        var id=Guid.NewGuid(); var version=Guid.NewGuid();
        var old=Create(Legacy,"Math Is Mine",id,version);
        var same=Create(Legacy,"Math Is Mine",id,version);
        Assert.Null(Guard("EnsureNoUnsafeGrade4IdentityMutation").Invoke(null,[old,same]));
    }

    [Fact]
    public void OtherGradesRemainOutsideThisSpecificMigrationSafetyGuard()
    {
        var id=Guid.NewGuid(); var version=Guid.NewGuid();
        var old=Create("PED:UAE-MOE-MATH:L3:COMMON:01:01","Previous",id,version);
        var next=Create("PED:UAE-MOE-MATH:L3:COMMON:01:01","Updated",id,version);
        Assert.Null(Guard("EnsureNoUnsafeGrade4IdentityMutation").Invoke(null,[old,next]));
        Assert.Null(Guard("EnsureNoUnsafeGrade4Retirement").Invoke(null,[old]));
    }
}
