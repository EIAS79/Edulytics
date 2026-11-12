using System.Reflection;
using Edulytics.Core.Entities;
using Edulytics.Data.Seeding;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeGrade4StageDReplaceLegacyTests
{
    private static MethodInfo Guard(string method) =>
        typeof(MathematicsPedagogicalLessonSeeder).GetMethod(
            method,BindingFlags.NonPublic|BindingFlags.Static)
        ??throw new InvalidOperationException("Grade 4 replacement guard missing");
    [Theory]
    [InlineData(null, false)]
    [InlineData("", false)]
    [InlineData("true", false)]
    [InlineData("CONFIRM_REPLACE_G4", false)]
    [InlineData("CONFIRM_REPLACE_OLD_GRADE4_93_WITH_NEW_132", true)]
    public void ReplacementRequiresExactOwnerApprovedOneShotToken(string? token,bool allowed)
    {
        Assert.Equal(allowed,(bool)Guard("IsExplicitGrade4ReplacementToken").Invoke(null,[token])!);
    }
    [Fact]
    public void LegacyFingerprintIsDeterministicAcrossInputOrderingAndSensitiveToChanges()
    {
        var a=new CurriculumPedagogicalLesson
          {Id=Guid.Parse("149cb81e-e6de-275c-8acb-bd0599bfa2ac"),
           Code="PED:UAE-MOE-MATH:L4:COMMON:01:01",Title="Math Is Mine"};
        var b=new CurriculumPedagogicalLesson
          {Id=Guid.Parse("df54b3c5-653f-b75b-81c6-d71cc49080f1"),
           Code="PED:UAE-MOE-MATH:L4:COMMON:01:02",Title="Math Is Exploring and Thinking"};
        string Fingerprint(params CurriculumPedagogicalLesson[] rows) =>
            (string)Guard("FingerprintLegacyGrade4").Invoke(null,[rows])!;
        var first=Fingerprint(a,b);
        Assert.Equal(64,first.Length);
        Assert.Equal(first,Fingerprint(b,a));
        a.Title="Unexpected change";
        Assert.NotEqual(first,Fingerprint(a,b));
    }
}
