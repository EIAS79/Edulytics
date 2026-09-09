namespace Edulytics.Tests.Acceptance;

public sealed class PublicContactVisualCleanupContractTests
{
    [Fact]
    public void Inquiry_DoesNotRenderTurnstileOrMailClientExplanatoryCopy()
    {
        var root = FindRoot();
        var view = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/Views/Contact/Inquiry.cshtml"));

        Assert.DoesNotContain("data-contact-key=\"turnstileNote\"", view, StringComparison.Ordinal);
        Assert.DoesNotContain("data-contact-key=\"mailNote\"", view, StringComparison.Ordinal);
        Assert.DoesNotContain("Protected by Cloudflare Turnstile", view, StringComparison.Ordinal);
        Assert.DoesNotContain("No email application will open", view, StringComparison.Ordinal);

        Assert.Contains("class=\"cf-turnstile\"", view, StringComparison.Ordinal);
        Assert.Contains("data-contact-form-status", view, StringComparison.Ordinal);
    }

    [Fact]
    public void PublicLanguageUi_DeduplicatesArabicInsideSwitcherButPreservesBrandLabel()
    {
        var root = FindRoot();
        var script = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/wwwroot/js/public-site-global-ui-v30.js"));

        Assert.Contains("removeDuplicateArabicSwitches", script, StringComparison.Ordinal);
        Assert.Contains("languageHost.children", script, StringComparison.Ordinal);
        Assert.Contains(".ed-home-lang-label", script, StringComparison.Ordinal);
        Assert.Contains("label.textContent = 'AR'", script, StringComparison.Ordinal);
    }

    [Fact]
    public void ArabicInquiryHeadline_HasDesktopOnlyBalanceRule()
    {
        var root = FindRoot();
        var css = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/wwwroot/css/public-arabic-rtl-v29.css"));

        Assert.Contains("@media (min-width: 981px)", css, StringComparison.Ordinal);
        Assert.Contains(".ed-contact-v28 .ed-inquiry-grid", css, StringComparison.Ordinal);
        Assert.Contains(".ed-contact-v28 .ed-inquiry-copy h1", css, StringComparison.Ordinal);
        Assert.Contains("max-width: 620px", css, StringComparison.Ordinal);
    }

    private static string FindRoot()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);
        while (directory is not null)
        {
            if (File.Exists(Path.Combine(directory.FullName, "Edulytics.sln")))
                return directory.FullName;

            directory = directory.Parent;
        }

        throw new DirectoryNotFoundException("Edulytics solution root not found.");
    }
}
