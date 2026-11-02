using Edulytics.Core.Analytics;
using Edulytics.Core.Enums;
using Edulytics.Core.LearningIntelligence;
using Edulytics.Services.LearningIntelligence;

namespace Edulytics.Tests.Phase37;

public sealed class LearningIntelligenceEngineTests
{
    private readonly LearningIntelligenceEngine _engine = new();

    [Fact]
    public void Build_ProducesDeterministicSchoolStudentClassAndOutcomeIntelligence()
    {
        var schoolId = Guid.NewGuid();
        var studentA = Guid.NewGuid();
        var studentB = Guid.NewGuid();
        var classId = Guid.NewGuid();
        var outcomeId = Guid.NewGuid();
        var teacherId = Guid.NewGuid();
        var day1 = new DateTime(2026, 9, 1, 8, 0, 0, DateTimeKind.Utc);
        var day2 = day1.AddDays(1);

        var snapshots = new[]
        {
            Snapshot(schoolId, studentA, "A Student", classId, teacherId, day1,
                Profile(schoolId, studentA, classId, outcomeId, 45m, MasteryBand.Weak, 40m)),
            Snapshot(schoolId, studentB, "B Student", classId, teacherId, day1,
                Profile(schoolId, studentB, classId, outcomeId, 65m, MasteryBand.Developing, 60m)),
            Snapshot(schoolId, studentA, "A Student", classId, teacherId, day2,
                Profile(schoolId, studentA, classId, outcomeId, 70m, MasteryBand.Developing, 68m)),
            Snapshot(schoolId, studentB, "B Student", classId, teacherId, day2,
                Profile(schoolId, studentB, classId, outcomeId, 82m, MasteryBand.Secure, 80m))
        };
        var recoveries = new[]
        {
            new RecoveryIntelligenceObservation(
                schoolId, studentA, outcomeId, day2, 45m, 70m, false),
            new RecoveryIntelligenceObservation(
                schoolId, studentB, outcomeId, day2, 65m, 82m, true)
        };

        var dashboard = _engine.Build(new LearningIntelligenceRequest(
            schoolId, snapshots, recoveries));

        Assert.Equal("phase37-v1", dashboard.FormulaVersion);
        Assert.Equal(2, dashboard.StudentCount);
        Assert.Equal(76m, dashboard.SchoolMasteryPercentage);
        Assert.Equal(100m, dashboard.ImprovementRatePercentage);
        Assert.Equal(2, dashboard.StudentTrends.Count);
        Assert.Single(dashboard.ClassTrends);
        Assert.Equal(55m, dashboard.ClassTrends[0].FirstMasteryPercentage);
        Assert.Equal(76m, dashboard.ClassTrends[0].LatestMasteryPercentage);
        Assert.Single(dashboard.OutcomeWeaknessDistribution);
        Assert.Equal(0, dashboard.OutcomeWeaknessDistribution[0].WeakStudents);
        Assert.Single(dashboard.RecoveryEffectiveness);
        Assert.Equal(2, dashboard.RecoveryEffectiveness[0].ImprovedCount);
        Assert.Equal(1, dashboard.RecoveryEffectiveness[0].RecoveredCount);
        Assert.Equal(50m, dashboard.RecoveryEffectiveness[0].RecoveryRatePercentage);
        Assert.Equal(2, dashboard.Drilldown.Count);
        Assert.All(dashboard.Drilldown, row => Assert.Equal(teacherId, row.TeacherUserId));
    }

    [Fact]
    public void Build_UsesOnlyLatestSnapshotForCurrentWeaknessAndSchoolMastery()
    {
        var schoolId = Guid.NewGuid();
        var studentId = Guid.NewGuid();
        var classId = Guid.NewGuid();
        var outcomeId = Guid.NewGuid();
        var day1 = new DateTime(2026, 9, 1, 8, 0, 0, DateTimeKind.Utc);
        var day2 = day1.AddDays(1);

        var dashboard = _engine.Build(new LearningIntelligenceRequest(
            schoolId,
            new[]
            {
                Snapshot(schoolId, studentId, "Student", classId, null, day1,
                    Profile(schoolId, studentId, classId, outcomeId, 30m, MasteryBand.CriticalGap, 30m)),
                Snapshot(schoolId, studentId, "Student", classId, null, day2,
                    Profile(schoolId, studentId, classId, outcomeId, 85m, MasteryBand.Secure, 85m))
            },
            Array.Empty<RecoveryIntelligenceObservation>()));

        Assert.Equal(85m, dashboard.SchoolMasteryPercentage);
        Assert.Equal(0, dashboard.OutcomeWeaknessDistribution[0].WeakStudents);
        Assert.Equal(0m, dashboard.WeaknessConcentration[0].WeakOutcomePercentage);
        Assert.Equal(55m, dashboard.StudentTrends[0].ChangePercentagePoints);
    }

    [Fact]
    public void Build_RejectsCrossSchoolSnapshotAndRecoveryObservation()
    {
        var schoolId = Guid.NewGuid();
        var otherSchool = Guid.NewGuid();
        var studentId = Guid.NewGuid();
        var classId = Guid.NewGuid();
        var outcomeId = Guid.NewGuid();
        var captured = new DateTime(2026, 9, 1, 8, 0, 0, DateTimeKind.Utc);

        var crossSchoolSnapshot = Snapshot(
            otherSchool,
            studentId,
            "Student",
            classId,
            null,
            captured,
            Profile(otherSchool, studentId, classId, outcomeId, 50m, MasteryBand.Weak, 50m));

        Assert.Throws<InvalidOperationException>(() =>
            _engine.Build(new LearningIntelligenceRequest(
                schoolId,
                new[] { crossSchoolSnapshot },
                Array.Empty<RecoveryIntelligenceObservation>())));

        var validSnapshot = Snapshot(
            schoolId,
            studentId,
            "Student",
            classId,
            null,
            captured,
            Profile(schoolId, studentId, classId, outcomeId, 50m, MasteryBand.Weak, 50m));

        Assert.Throws<InvalidOperationException>(() =>
            _engine.Build(new LearningIntelligenceRequest(
                schoolId,
                new[] { validSnapshot },
                new[]
                {
                    new RecoveryIntelligenceObservation(
                        otherSchool, studentId, outcomeId, captured, 40m, 50m, false)
                })));
    }

    [Fact]
    public void Build_RejectsAmbiguousDuplicateStudentSnapshotTimestamp()
    {
        var schoolId = Guid.NewGuid();
        var studentId = Guid.NewGuid();
        var classId = Guid.NewGuid();
        var outcomeId = Guid.NewGuid();
        var captured = new DateTime(2026, 9, 1, 8, 0, 0, DateTimeKind.Utc);
        var snapshot = Snapshot(
            schoolId,
            studentId,
            "Student",
            classId,
            null,
            captured,
            Profile(schoolId, studentId, classId, outcomeId, 50m, MasteryBand.Weak, 50m));

        Assert.Throws<InvalidOperationException>(() =>
            _engine.Build(new LearningIntelligenceRequest(
                schoolId,
                new[] { snapshot, snapshot },
                Array.Empty<RecoveryIntelligenceObservation>())));
    }

    private static LearningIntelligenceStudentSnapshot Snapshot(
        Guid schoolId,
        Guid studentId,
        string displayName,
        Guid classId,
        Guid? teacherId,
        DateTime capturedAtUtc,
        StudentLearningProfile profile) =>
        new(
            schoolId,
            studentId,
            displayName,
            "G7",
            "Grade 7",
            classId,
            "7A",
            teacherId,
            teacherId.HasValue ? "Teacher" : null,
            capturedAtUtc,
            profile);

    private static StudentLearningProfile Profile(
        Guid schoolId,
        Guid studentId,
        Guid classId,
        Guid outcomeId,
        decimal mastery,
        MasteryBand band,
        decimal confidence)
    {
        var outcome = new StudentOutcomeLearningProfile(
            outcomeId,
            "MATH.OUT.1",
            "Outcome",
            mastery,
            band,
            3,
            confidence,
            new DateTime(2026, 9, 1, 7, 0, 0, DateTimeKind.Utc),
            1,
            1,
            1,
            2.5m,
            "phase31-v1");

        return new StudentLearningProfile(
            schoolId,
            studentId,
            Guid.NewGuid(),
            classId,
            Guid.NewGuid(),
            mastery,
            band,
            3,
            confidence,
            outcome.LatestEvidenceAtUtc,
            new[] { outcome },
            "phase31-v1");
    }
}
