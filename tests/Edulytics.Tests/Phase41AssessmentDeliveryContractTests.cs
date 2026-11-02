using Edulytics.Core.Entities;
using Edulytics.Core.Enums;

namespace Edulytics.Tests;

public sealed class Phase41AssessmentDeliveryContractTests
{
    [Fact]
    public void LegacyDefaultsRemainOfflineClassAtClassLevel()
    {
        var assessment = new Assessment();
        Assert.Equal(AssessmentTargetType.Class, assessment.TargetType);
        Assert.Equal(AssessmentDeliveryMode.Offline, assessment.DeliveryMode);
        Assert.Equal(AssessmentDifficultyBand.AtClassLevel, assessment.DifficultyBand);
    }

    [Fact]
    public void StudentAnswerPersistsResponseTextContract()
    {
        var answer = new StudentAnswer { ResponseText = "42" };
        Assert.Equal("42", answer.ResponseText);
    }
}
