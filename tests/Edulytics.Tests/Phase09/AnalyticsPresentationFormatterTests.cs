using Edulytics.Services.Analytics;

namespace Edulytics.Tests.Phase09;

public sealed class AnalyticsPresentationFormatterTests
{
    [Fact]
    public void Class_filter_label_hides_internal_class_code()
    {
        Assert.Equal(
            "BG1",
            AnalyticsPresentationFormatter.ClassFilterLabel(" BG1 "));
    }

    [Fact]
    public void Cambridge_internal_outcome_code_becomes_user_facing_reference()
    {
        Assert.Equal(
            "Cambridge 6Nf.11",
            AnalyticsPresentationFormatter.OutcomeLabel(
                "CAM:OUT:0096:6Nf.11"));
    }

    [Fact]
    public void Unknown_internal_outcome_prefix_does_not_leak_full_internal_code()
    {
        Assert.Equal(
            "ABC.12",
            AnalyticsPresentationFormatter.OutcomeLabel(
                "INTERNAL:OUT:0042:ABC.12"));
    }


    [Fact]
    public void Polish_outcome_code_keeps_meaningful_identity_without_internal_hash()
    {
        Assert.Equal(
            "Polish I-III Core 1.001",
            AnalyticsPresentationFormatter.OutcomeLabel(
                "PL:I-III:OSIAGNIECIA-W-ZAKRE-0CBE2130A8A4:core:1:001"));
    }

    [Fact]
    public void Unknown_non_internal_colon_code_is_preserved()
    {
        Assert.Equal(
            "CUSTOM:AREA:SECTION:12",
            AnalyticsPresentationFormatter.OutcomeLabel(
                "CUSTOM:AREA:SECTION:12"));
    }

    [Fact]
    public void Technical_reference_only_description_is_replaced()
    {
        var result = AnalyticsPresentationFormatter.OutcomeDescription(
            "Edulytics reference-only Cambridge Mathematics entry for source locator 6Nf.11. Official copyrighted Cambridge wording is not reproduced.",
            "Cambridge 6Nf.11",
            "Mathematics");

        Assert.Equal(
            "Mathematics skill aligned to Cambridge 6Nf.11.",
            result);
    }


    [Fact]
    public void Cambridge_source_provenance_description_is_replaced()
    {
        var result = AnalyticsPresentationFormatter.OutcomeDescription(
            "Edulytics-authored from OGL material; Cambridge remains the academic reference authority and no Cambridge objective wording is reproduced here.",
            "Cambridge 6Nf.11",
            "Mathematics");

        Assert.Equal(
            "Mathematics skill aligned to Cambridge 6Nf.11.",
            result);
    }

    [Fact]
    public void Normal_user_facing_description_is_preserved()
    {
        Assert.Equal(
            "Compare and order fractions.",
            AnalyticsPresentationFormatter.OutcomeDescription(
                "Compare and order fractions.",
                "Cambridge 6Nf.11",
                "Mathematics"));
    }
}
