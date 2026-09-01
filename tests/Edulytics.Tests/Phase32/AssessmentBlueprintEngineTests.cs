using Edulytics.Core.Analytics;
using Edulytics.Core.AssessmentIntelligence;
using Edulytics.Core.Enums;
using Edulytics.Services.AssessmentIntelligence;

namespace Edulytics.Tests.Phase32;

public sealed class AssessmentBlueprintEngineTests
{
    private readonly AssessmentBlueprintEngine _engine = new();

    [Fact]
    public void BalancedDifficulty_TenItems_IsThreeFiveTwo()
    {
        var blueprint = _engine.Build(Request(questionCount: 10));

        Assert.Equal(3, Count(blueprint, AssessmentItemDifficulty.Easy));
        Assert.Equal(5, Count(blueprint, AssessmentItemDifficulty.Medium));
        Assert.Equal(2, Count(blueprint, AssessmentItemDifficulty.Challenging));
    }

    [Fact]
    public void EveryAllocationDimension_SumsToQuestionCount()
    {
        var blueprint = _engine.Build(Request(questionCount: 17));

        Assert.Equal(17, blueprint.OutcomeAllocations.Sum(x => x.ItemCount));
        Assert.Equal(17, blueprint.DifficultyAllocations.Sum(x => x.ItemCount));
        Assert.Equal(17, blueprint.QuestionFamilyAllocations.Sum(x => x.ItemCount));
        Assert.Equal(17, blueprint.ItemTypeAllocations.Sum(x => x.ItemCount));
        Assert.Equal(17, blueprint.RequiredEvidence.Sum(x => x.RequiredItemCount));
    }

    [Fact]
    public void WeakLowConfidenceOutcome_ReceivesMoreCoverage()
    {
        var school = Guid.NewGuid();
        var adoption = Guid.NewGuid();
        var weak = Guid.NewGuid();
        var strong = Guid.NewGuid();
        var profile = Profile(
            school,
            adoption,
            Outcome(weak, mastery: 35m, confidence: 20m, evidence: 1),
            Outcome(strong, mastery: 95m, confidence: 100m, evidence: 8));

        var blueprint = _engine.Build(Request(
            school: school,
            adoption: adoption,
            outcomes: [weak, strong],
            profile: profile,
            questionCount: 8));

        var weakCount = blueprint.OutcomeAllocations.Single(x => x.LearningOutcomeId == weak).ItemCount;
        var strongCount = blueprint.OutcomeAllocations.Single(x => x.LearningOutcomeId == strong).ItemCount;

        Assert.True(weakCount > strongCount);
    }

    [Fact]
    public void Diagnostic_PrioritizesOutcomeWithNoEvidence()
    {
        var school = Guid.NewGuid();
        var adoption = Guid.NewGuid();
        var unseen = Guid.NewGuid();
        var known = Guid.NewGuid();
        var profile = Profile(
            school,
            adoption,
            Outcome(known, mastery: 45m, confidence: 60m, evidence: 3));

        var blueprint = _engine.Build(Request(
            school: school,
            adoption: adoption,
            outcomes: [known, unseen],
            profile: profile,
            purpose: AssessmentPurpose.Diagnostic,
            questionCount: 1));

        Assert.Equal(
            unseen,
            Assert.Single(blueprint.OutcomeAllocations, x => x.ItemCount == 1).LearningOutcomeId);
    }

    [Fact]
    public void StudentProfileOutsideSchoolScope_IsRejected()
    {
        var school = Guid.NewGuid();
        var adoption = Guid.NewGuid();
        var profile = Profile(Guid.NewGuid(), adoption);

        Assert.Throws<InvalidOperationException>(() =>
            _engine.Build(Request(
                school: school,
                adoption: adoption,
                profile: profile)));
    }

    [Fact]
    public void StudentProfileOutsideAdoptionScope_IsRejected()
    {
        var school = Guid.NewGuid();
        var profile = Profile(school, Guid.NewGuid());

        Assert.Throws<InvalidOperationException>(() =>
            _engine.Build(Request(
                school: school,
                adoption: Guid.NewGuid(),
                profile: profile)));
    }

    [Fact]
    public void DuplicateOutcomeIds_DoNotCreateDuplicateAllocations()
    {
        var outcome = Guid.NewGuid();
        var blueprint = _engine.Build(Request(
            outcomes: [outcome, outcome, outcome],
            questionCount: 4));

        var allocation = Assert.Single(blueprint.OutcomeAllocations);
        Assert.Equal(outcome, allocation.LearningOutcomeId);
        Assert.Equal(4, allocation.ItemCount);
    }

    [Fact]
    public void ExposureFingerprints_AreNormalizedDistinctAndStable()
    {
        var request = Request(questionCount: 3) with
        {
            ExcludedExposureFingerprints = [" beta ", "alpha", "beta", " ", "alpha"]
        };

        var blueprint = _engine.Build(request);

        Assert.Equal(["alpha", "beta"], blueprint.ExcludedExposureFingerprints);
    }

    [Fact]
    public void SameInput_ProducesSameBlueprintStructure()
    {
        var request = Request(questionCount: 13);

        var first = _engine.Build(request);
        var second = _engine.Build(request);

        Assert.Equal(first.OutcomeAllocations, second.OutcomeAllocations);
        Assert.Equal(first.DifficultyAllocations, second.DifficultyAllocations);
        Assert.Equal(first.QuestionFamilyAllocations, second.QuestionFamilyAllocations);
        Assert.Equal(first.ItemTypeAllocations, second.ItemTypeAllocations);
        Assert.Equal(first.RequiredEvidence, second.RequiredEvidence);
        Assert.Equal(first.ExcludedExposureFingerprints, second.ExcludedExposureFingerprints);
        Assert.Equal(AssessmentBlueprintEngine.FormulaVersion, first.FormulaVersion);
    }

    [Fact]
    public void InvalidDifficultyPolicy_IsRejected()
    {
        var request = Request() with
        {
            DifficultyPolicy = new AssessmentDifficultyPolicy(30, 30, 30)
        };

        Assert.Throws<InvalidOperationException>(() => _engine.Build(request));
    }

    private static int Count(AssessmentBlueprint blueprint, AssessmentItemDifficulty difficulty) =>
        blueprint.DifficultyAllocations.Single(x => x.Difficulty == difficulty).ItemCount;

    private static AssessmentBlueprintRequest Request(
        Guid? school = null,
        Guid? adoption = null,
        IReadOnlyList<Guid>? outcomes = null,
        StudentLearningProfile? profile = null,
        AssessmentPurpose purpose = AssessmentPurpose.StudentPersonalTest,
        int questionCount = 10) =>
        new(
            school ?? Guid.NewGuid(),
            adoption ?? Guid.NewGuid(),
            "GRADE-6",
            Guid.NewGuid(),
            null,
            outcomes ?? [Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()],
            profile,
            purpose,
            questionCount,
            AssessmentDifficultyPolicy.Balanced,
            []);

    private static StudentLearningProfile Profile(
        Guid school,
        Guid adoption,
        params StudentOutcomeLearningProfile[] outcomes) =>
        new(
            school,
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            adoption,
            outcomes.Length == 0 ? 0m : outcomes.Average(x => x.MasteryPercentage),
            MasteryBand.Developing,
            outcomes.Sum(x => x.EvidenceCount),
            outcomes.Length == 0 ? 0m : outcomes.Average(x => x.ConfidencePercentage),
            DateTime.UtcNow,
            outcomes,
            "phase31-v1");

    private static StudentOutcomeLearningProfile Outcome(
        Guid outcomeId,
        decimal mastery,
        decimal confidence,
        int evidence) =>
        new(
            outcomeId,
            $"OUT-{outcomeId:N}",
            "Outcome",
            mastery,
            mastery < 60m ? MasteryBand.Weak : MasteryBand.Strong,
            evidence,
            confidence,
            DateTime.UtcNow,
            evidence,
            0,
            0,
            evidence,
            "phase31-v1");
}
