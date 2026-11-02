using Edulytics.Web.ViewModels.StudentPortal;

namespace Edulytics.Tests.Phase44;

public sealed class StudentSkillDisplayNameTests
{
    [Theory]
    [InlineData(
        "outcome:CAM:OUT:0096:4Gg.02",
        "Edulytics reference-only Cambridge Mathematics entry for source locator 4Gg.02. Official copyrighted Cambridge wording is not reproduced.",
        "Perimeter & area")]
    [InlineData(
        "outcome:CAM:OUT:0096:4Gg.08",
        "placeholder",
        "Classify angles")]
    [InlineData(
        "outcome:CAM:OUT:0096:4Nf.04",
        "placeholder",
        "Equivalent fractions")]
    [InlineData(
        "outcome:CAM:OUT:0096:4Np.01",
        "placeholder",
        "Place value")]
    public void CambridgeStage4Proxy_UsesConciseStudentName(
        string skillKey,
        string sourceName,
        string expected)
    {
        Assert.Equal(
            expected,
            StudentSkillDisplayName.Format(
                skillKey,
                sourceName));
    }

    [Fact]
    public void CanonicalSkillName_IsNotChanged()
    {
        const string name = "Compare fractions";

        Assert.Equal(
            name,
            StudentSkillDisplayName.Format(
                "fractions.compare",
                name));
    }

    [Fact]
    public void UnknownCambridgeOutcome_PreservesSourceName()
    {
        const string name = "Existing reviewed skill name";

        Assert.Equal(
            name,
            StudentSkillDisplayName.Format(
                "outcome:CAM:OUT:0096:6ZZ.99",
                name));
    }
}
